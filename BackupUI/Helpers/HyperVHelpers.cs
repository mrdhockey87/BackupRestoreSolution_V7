using SecureServerBackupCommon;
using SecureServerBackup.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public static class HyperVHelpers
	{
		public static async Task LoadHyperVSystems(List<DriveTreeItem> driveItems)
		{
			List<DriveTreeItem> hyperVSystems = await Task.Run(() =>
			{
				List<DriveTreeItem> discoveredSystems = new();

				try
				{
					Dictionary<string, DriveTreeItem> systemLookup = new(StringComparer.OrdinalIgnoreCase);

					var vmBuffer = new StringBuilder(4096);
					var vmResult = BackupEngineInterop.EnumerateHyperVMachines(vmBuffer, vmBuffer.Capacity);
					if (vmResult == 0)
					{
						foreach (string vm in vmBuffer.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries))
						{
							string normalizedVmName = RegularHyperVRestoreHelper.NormalizeHyperVVmName(vm);
							if (string.IsNullOrWhiteSpace(normalizedVmName))
							{
								continue;
							}

							systemLookup[normalizedVmName] = new DriveTreeItem
							{
								Name = $"Hyper-V: {vm}",
								FullPath = vm,
								VirtualMachineName = normalizedVmName,
								ItemType = DriveTreeItemType.HyperVSystem
							};
						}
					}

					foreach (HyperVVirtualDiskInfo diskInfo in EnumerateHyperVVirtualDiskInfos())
					{
						if (!systemLookup.TryGetValue(diskInfo.VirtualMachineName, out DriveTreeItem? hyperVItem))
						{
							hyperVItem = new DriveTreeItem
							{
								Name = $"Hyper-V: {diskInfo.VirtualMachineDisplayName}",
								FullPath = diskInfo.VirtualMachineDisplayName,
								VirtualMachineName = diskInfo.VirtualMachineName,
								ItemType = DriveTreeItemType.HyperVSystem
							};
							systemLookup[diskInfo.VirtualMachineName] = hyperVItem;
						}

						AddHyperVVirtualDiskItem(hyperVItem, diskInfo);
					}

					foreach (DriveTreeItem hyperVSystem in systemLookup.Values)
					{
						if (hyperVSystem.Children.Count == 0)
						{
							hyperVSystem.Children.Add(new DriveTreeItem
							{
								Name = "(No guest disks found)",
								ItemType = DriveTreeItemType.Folder,
								Parent = hyperVSystem
							});
						}
					}

					discoveredSystems.AddRange(systemLookup.Values.OrderBy(item => item.Name, StringComparer.OrdinalIgnoreCase));
				}
				catch
				{
				}

				return discoveredSystems;
			});

			foreach (DriveTreeItem hyperVSystem in hyperVSystems)
			{
				driveItems.Add(hyperVSystem);
			}
		}

		public static string NormalizeHyperVDisplayName(string name)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name);

			string normalized = name.Trim();
			const string hyperVPrefix = "Hyper-V:";
			if (normalized.StartsWith(hyperVPrefix, StringComparison.OrdinalIgnoreCase))
			{
				normalized = normalized[hyperVPrefix.Length..].Trim();
			}

			int statusIndex = normalized.LastIndexOf(" (", StringComparison.Ordinal);
			if (statusIndex > 0 && normalized.EndsWith(")", StringComparison.Ordinal))
			{
				normalized = normalized[..statusIndex].TrimEnd();
			}

			return normalized;
		}
		public static void AddHyperVVirtualDiskItem(DriveTreeItem hyperVItem, HyperVVirtualDiskInfo diskInfo)
		{
			if (hyperVItem.Children.Any(item =>
					item.ItemType == DriveTreeItemType.HyperVVirtualDisk &&
					string.Equals(item.VirtualDiskPath, diskInfo.VirtualDiskPath, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}

			var virtualDiskItem = new DriveTreeItem
			{
				Name = Path.GetFileName(diskInfo.VirtualDiskPath),
				FullPath = HyperVGuestSelectionPath.Encode(HyperVGuestSelectionKind.VirtualDisk, diskInfo.VirtualMachineName, diskInfo.VirtualDiskPath, 0, string.Empty),
				VirtualMachineName = diskInfo.VirtualMachineName,
				VirtualDiskPath = diskInfo.VirtualDiskPath,
				ItemType = DriveTreeItemType.HyperVVirtualDisk,
				Parent = hyperVItem
			};
			virtualDiskItem.Children.Add(new DriveTreeItem
			{
				Name = "Loading...",
				ItemType = DriveTreeItemType.Folder,
				Parent = virtualDiskItem
			});
			hyperVItem.Children.Add(virtualDiskItem);
		}

		public static IReadOnlyList<HyperVVirtualDiskInfo> EnumerateHyperVVirtualDiskInfos()
		{
			var diskBuffer = new StringBuilder(32768);
			var diskResult = BackupEngineInterop.EnumerateHyperVVirtualMachineDisks(diskBuffer, diskBuffer.Capacity);
			IReadOnlyList<HyperVVirtualDiskInfo> disks = HyperVBackupTreeHelper.ParseVirtualDiskEnumeration(diskBuffer.ToString());
			if (disks.Count > 0)
			{
				return disks;
			}

			var error = new StringBuilder(1024);
			BackupEngineInterop.GetLastErrorMessage(error, error.Capacity);
			Debug.WriteLine($"EnumerateHyperVVirtualMachineDisks returned {diskResult}: {error}");

			disks = EnumerateHyperVVirtualDiskInfosFromWmi();
			if (disks.Count > 0)
			{
				Debug.WriteLine($"Enumerated {disks.Count} Hyper-V virtual disks using managed WMI fallback.");
				return disks;
			}

			try
			{
				string fallbackOutput = RunPowerShell("$ErrorActionPreference='Stop'; Get-VM -ErrorAction Stop | ForEach-Object { $vm = $_; $displayName = $vm.Name; if ($vm.State) { $displayName += ' (' + $vm.State.ToString() + ')'; } Get-VMHardDiskDrive -VMName $vm.Name -ErrorAction SilentlyContinue | ForEach-Object { if (-not [string]::IsNullOrWhiteSpace($_.Path)) { [Console]::WriteLine(($vm.Name + \"`t\" + $displayName + \"`t\" + $_.Path)); } } }");
				disks = HyperVBackupTreeHelper.ParseVirtualDiskEnumeration(fallbackOutput);
				if (disks.Count > 0)
				{
					Debug.WriteLine($"Enumerated {disks.Count} Hyper-V virtual disks using PowerShell fallback.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"PowerShell Hyper-V disk fallback failed: {ex.Message}");
			}

			return disks;
		}

		public static IReadOnlyList<HyperVVirtualDiskInfo> EnumerateHyperVVirtualDiskInfosFromWmi()
		{
			foreach (string scopePath in new[] { @"\\.\ROOT\virtualization\v2", @"\\.\ROOT\virtualization" })
			{
				try
				{
					var scope = new ManagementScope(scopePath);
					scope.Connect();

					var disks = EnumerateHyperVVirtualDiskInfosFromWmiScope(scope);
					if (disks.Count > 0)
					{
						return disks;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Managed Hyper-V WMI disk fallback failed for {scopePath}: {ex.Message}");
				}
			}

			return Array.Empty<HyperVVirtualDiskInfo>();
		}

		public static IReadOnlyList<HyperVVirtualDiskInfo> EnumerateHyperVVirtualDiskInfosFromWmiScope(ManagementScope scope)
		{
			Dictionary<string, HyperVVirtualDiskInfo> disks = new(StringComparer.OrdinalIgnoreCase);
			using var vmSearcher = new ManagementObjectSearcher(
				scope,
				new ObjectQuery("SELECT ElementName, EnabledState, __PATH FROM Msvm_ComputerSystem WHERE Caption='Virtual Machine'"));

			foreach (ManagementObject vm in vmSearcher.Get().OfType<ManagementObject>())
			{
				string vmName = RegularHyperVRestoreHelper.NormalizeHyperVVmName(vm["ElementName"]?.ToString());
				string vmPath = vm.Path?.Path ?? vm["__PATH"]?.ToString() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(vmName) || string.IsNullOrWhiteSpace(vmPath))
				{
					continue;
				}

				string vmDisplayName = HyperVBackupTreeHelper.BuildVmDisplayName(vmName, vm["EnabledState"]);
				string query = $"ASSOCIATORS OF {{{vmPath}}} WHERE AssocClass=Msvm_SystemDevice ResultClass=Msvm_StorageAllocationSettingData";
				using var storageSearcher = new ManagementObjectSearcher(scope, new ObjectQuery(query));
				foreach (ManagementObject storage in storageSearcher.Get().OfType<ManagementObject>())
				{
					if (!HyperVBackupTreeHelper.IsVirtualDiskResource(storage["ResourceSubType"]?.ToString()))
					{
						continue;
					}

					foreach (string hostResource in HyperVBackupTreeHelper.GetHostResources(storage["HostResource"]))
					{
						if (string.IsNullOrWhiteSpace(hostResource))
						{
							continue;
						}

						string normalizedPath = hostResource.Trim().Trim('"');
						disks[vmName + "|" + normalizedPath] = new HyperVVirtualDiskInfo(vmName, vmDisplayName, normalizedPath);
					}
				}
			}

			return disks.Values.ToArray();
		}


		public static async Task LoadHyperVVirtualDiskChildrenAsync(DriveTreeItem virtualDiskItem,string? _hyperVGuestMountRoot)
		{
			virtualDiskItem.Children.Clear();

			if (string.IsNullOrWhiteSpace(virtualDiskItem.VirtualDiskPath) || string.IsNullOrWhiteSpace(virtualDiskItem.VirtualMachineName))
			{
				virtualDiskItem.Children.Add(new DriveTreeItem
				{
					Name = "(Hyper-V virtual disk information is missing)",
					ItemType = DriveTreeItemType.Folder,
					Parent = virtualDiskItem
				});
				return;
			}

			try
			{
				string root = EnsureHyperVGuestMountRoot(_hyperVGuestMountRoot);
				string diskFolderName = Regex.Replace(Path.GetFileNameWithoutExtension(virtualDiskItem.VirtualDiskPath) ?? "HyperVDisk", "[^A-Za-z0-9._-]", "_");
				string diskMountRoot = Path.Combine(root, diskFolderName + "_" + Math.Abs(StringComparer.OrdinalIgnoreCase.GetHashCode(virtualDiskItem.VirtualDiskPath)).ToString("X8"));
				Directory.CreateDirectory(diskMountRoot);

				// Run blocking PowerShell mount on a background thread
				(List<MountedHyperVGuestExecutionPartition> partitions, string mountedDiskPath) = await Task.Run(() =>
				{
					List<MountedHyperVGuestExecutionPartition> p = MountHyperVGuestDiskReadOnlyCore(virtualDiskItem.VirtualDiskPath, diskMountRoot, out string mdp);
					return (p, mdp);
				});

				List<string> mountDirectories = new();
				foreach (MountedHyperVGuestExecutionPartition partition in partitions)
				{
					int partitionNumber = partition.PartitionNumber;
					string mountPath = partition.MountPath;
					if (string.IsNullOrWhiteSpace(mountPath) || !Directory.Exists(mountPath))
					{
						continue;
					}

					if (mountPath.StartsWith(diskMountRoot, StringComparison.OrdinalIgnoreCase))
					{
						mountDirectories.Add(mountPath);
					}

					string encodedPath = HyperVGuestSelectionPath.Encode(
						HyperVGuestSelectionKind.Volume,
						virtualDiskItem.VirtualMachineName,
						virtualDiskItem.VirtualDiskPath,
						partitionNumber,
						string.Empty);

					var volumeItem = new DriveTreeItem
					{
						Name = $"Partition {partitionNumber}",
						FullPath = encodedPath,
						ResolvedPath = mountPath,
						VirtualMachineName = virtualDiskItem.VirtualMachineName,
						VirtualDiskPath = virtualDiskItem.VirtualDiskPath,
						PartitionNumber = partitionNumber,
						ItemType = DriveTreeItemType.HyperVVolume,
						Parent = virtualDiskItem
					};

					if (Directory.GetDirectories(mountPath).Length > 0)
					{
						volumeItem.Children.Add(new DriveTreeItem
						{
							Name = "Loading...",
							ItemType = DriveTreeItemType.Folder,
							Parent = volumeItem
						});
					}

					virtualDiskItem.Children.Add(volumeItem);
				}

				_hyperVDiskMountDirectories[virtualDiskItem.VirtualDiskPath] = new MountedHyperVGuestTreeDisk(mountedDiskPath, mountDirectories);

				if (virtualDiskItem.Children.Count == 0)
				{
					virtualDiskItem.Children.Add(new DriveTreeItem
					{
						Name = "(No accessible guest partitions)",
						ItemType = DriveTreeItemType.Folder,
						Parent = virtualDiskItem
					});
				}
			}
			catch (Exception ex)
			{
				if (IsHyperVVirtualDiskSharingViolation(ex))
				{
					// Show alert and revert the node — leave no error placeholder in the tree.
					virtualDiskItem.Children.Clear();
					virtualDiskItem.ChildrenLoaded = false;
					virtualDiskItem.IsExpanded = false;

					CustomDialogService.ShowWarning(
						"The virtual disk is locked because the VM is currently running.\n\n" +
						"To back up individual guest partitions the VM must be stopped first.\n\n" +
						"To back up the running VM, select the VM node in the tree instead.",
						"Virtual Disk Locked");
				}
				else
				{
					virtualDiskItem.Children.Add(new DriveTreeItem
					{
						Name = $"(Error mounting virtual disk: {ex.Message})",
						ItemType = DriveTreeItemType.Folder,
						Parent = virtualDiskItem
					});
				}
			}
		}

		public static string EnsureHyperVGuestMountRoot(string? _hyperVGuestMountRoot)
		{
			if (!string.IsNullOrWhiteSpace(_hyperVGuestMountRoot))
			{
				Directory.CreateDirectory(_hyperVGuestMountRoot);
				return _hyperVGuestMountRoot;
			}

			_hyperVGuestMountRoot = Path.Combine(Path.GetTempPath(), "SecureServerBackup", "HyperVGuestMounts", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(_hyperVGuestMountRoot);
			return _hyperVGuestMountRoot;
		}
		private static readonly Dictionary<string, MountedHyperVGuestTreeDisk> _hyperVDiskMountDirectories = new(StringComparer.OrdinalIgnoreCase);
		
		public static void CleanupHyperVGuestMounts(string? _hyperVGuestMountRoot)
		{
			foreach (var virtualDiskPath in _hyperVDiskMountDirectories.Keys.ToList())
			{
				try
				{
					RunPowerShell($"Dismount-VHD -Path '{EscapePowerShellSingleQuotedString(_hyperVDiskMountDirectories[virtualDiskPath].MountedDiskPath)}' -ErrorAction SilentlyContinue | Out-Null");
				}
				catch
				{
				}
			}

			foreach (MountedHyperVGuestTreeDisk mountedDisk in _hyperVDiskMountDirectories.Values)
			{
				foreach (string directory in mountedDisk.MountDirectories)
				{
					try
					{
						if (Directory.Exists(directory))
						{
							Directory.Delete(directory, recursive: true);
						}
					}
					catch
					{
					}
				}
			}

			_hyperVDiskMountDirectories.Clear();

			if (!string.IsNullOrWhiteSpace(_hyperVGuestMountRoot))
			{
				try
				{
					if (Directory.Exists(_hyperVGuestMountRoot))
					{
						Directory.Delete(_hyperVGuestMountRoot, recursive: true);
					}
				}
				catch
				{
				}
			}
		}

		public static List<MountedHyperVGuestExecutionPartition> MountHyperVGuestDiskReadOnlyCore(string virtualDiskPath, string mountRoot, out string mountedDiskPath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(virtualDiskPath);
			ArgumentException.ThrowIfNullOrWhiteSpace(mountRoot);

			static List<MountedHyperVGuestExecutionPartition> MountCore(string diskPath, string root)
			{
				string escapedPath = EscapePowerShellSingleQuotedString(diskPath);
				string escapedRoot = EscapePowerShellSingleQuotedString(root);
				string script = $"$vhd = Mount-VHD -Path '{escapedPath}' -ReadOnly -Passthru -ErrorAction Stop; $disk = $vhd | Get-Disk -ErrorAction Stop; Get-Partition -DiskNumber $disk.Number -ErrorAction Stop | Sort-Object PartitionNumber | ForEach-Object {{ $partition = $_; $mountPath = $null; $created = $false; if ($partition.AccessPaths) {{ $mountPath = @($partition.AccessPaths | Where-Object {{ $_ }}) | Select-Object -First 1; }} if ([string]::IsNullOrWhiteSpace($mountPath)) {{ $folder = Join-Path '{escapedRoot}' ('Partition' + $partition.PartitionNumber); New-Item -ItemType Directory -Path $folder -Force | Out-Null; Add-PartitionAccessPath -DiskNumber $disk.Number -PartitionNumber $partition.PartitionNumber -AccessPath $folder -ErrorAction Stop | Out-Null; $mountPath = $folder; $created = $true; }} [Console]::WriteLine(($partition.PartitionNumber.ToString() + \"`t\" + $mountPath + \"`t\" + $created.ToString())); }}";
				string output = RunPowerShell(script);
				List<MountedHyperVGuestExecutionPartition> partitions = new();
				foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
				{
					string[] parts = line.Split('\t');
					if (parts.Length < 3 ||
						!int.TryParse(parts[0], out int partitionNumber) ||
						string.IsNullOrWhiteSpace(parts[1]))
					{
						continue;
					}

					partitions.Add(new MountedHyperVGuestExecutionPartition(
						partitionNumber,
						parts[1].Trim(),
						bool.TryParse(parts[2], out bool createdMountDirectory) && createdMountDirectory));
				}

				return partitions;
			}

			mountedDiskPath = virtualDiskPath;

			try
			{
				return MountCore(virtualDiskPath, mountRoot);
			}
			catch (Exception ex) when (IsHyperVVirtualDiskSharingViolation(ex))
			{
				string fallbackDiskPath = ResolveMountableHyperVVirtualDiskPath(virtualDiskPath);
				if (string.Equals(fallbackDiskPath, virtualDiskPath, StringComparison.OrdinalIgnoreCase))
				{
					throw;
				}

				mountedDiskPath = fallbackDiskPath;
				return MountCore(fallbackDiskPath, mountRoot);
			}
		}


		public static MountedHyperVGuestExecutionDisk MountHyperVGuestExecutionDiskReadOnly(string vmName, string virtualDiskPath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(vmName);
			ArgumentException.ThrowIfNullOrWhiteSpace(virtualDiskPath);

			string mountRoot = Path.Combine(Path.GetTempPath(), "SecureServerBackup", "HyperVGuestMounts", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(mountRoot);

			try
			{
				List<MountedHyperVGuestExecutionPartition> partitions = MountHyperVGuestDiskReadOnlyCore(virtualDiskPath, mountRoot, out string mountedDiskPath);
				return new MountedHyperVGuestExecutionDisk(mountedDiskPath, mountRoot, partitions);
			}
			catch
			{
				try
				{
					if (Directory.Exists(mountRoot))
					{
						Directory.Delete(mountRoot, recursive: true);
					}
				}
				catch
				{
				}

				throw;
			}
		}

		public static IReadOnlyList<string> ResolveHyperVGuestExecutionSourcePaths(HyperVGuestSelectionInfo selection, MountedHyperVGuestExecutionDisk mountedDisk)
		{
			return HyperVGuestSelectionPath.GetCandidateSourcePaths(
					selection,
					mountedDisk.Partitions.Select(partition => new HyperVGuestMountedPartition(partition.PartitionNumber, partition.MountPath)))
				.Where(Directory.Exists)
				.ToArray();
		}


		public static string ResolveMountableHyperVVirtualDiskPath(string virtualDiskPath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(virtualDiskPath);

			string script = $"$path = '{EscapePowerShellSingleQuotedString(virtualDiskPath)}'; while (-not [string]::IsNullOrWhiteSpace($path)) {{ $vhd = Get-VHD -Path $path -ErrorAction Stop; [Console]::WriteLine($vhd.Path); if ([string]::IsNullOrWhiteSpace($vhd.ParentPath)) {{ break }} $path = $vhd.ParentPath }}";
			string output = RunPowerShell(script);
			string resolvedPath = HyperVBackupTreeHelper.SelectMountableVirtualDiskPath(
				virtualDiskPath,
				output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

			if (!string.Equals(resolvedPath, virtualDiskPath, StringComparison.OrdinalIgnoreCase))
			{
				Debug.WriteLine($"Using parent Hyper-V virtual disk fallback for {virtualDiskPath}: {resolvedPath}");
			}

			return resolvedPath;
		}

		public static bool IsHyperVVirtualDiskSharingViolation(Exception ex)
		{
			// Walk the full exception chain so exceptions wrapped in AggregateException
			// or re-thrown with an inner cause are still caught correctly.
			for (Exception? e = ex; e != null; e = e.InnerException)
			{
				if (e.Message.Contains("being used by another process", StringComparison.OrdinalIgnoreCase) ||
					e.Message.Contains("0x80070020", StringComparison.OrdinalIgnoreCase) ||
					(e is System.IO.IOException ioEx && (uint)System.Runtime.InteropServices.Marshal.GetHRForException(ioEx) == 0x80070020))
					return true;
			}

			// AggregateException may flatten multiple inner exceptions
			if (ex is AggregateException agg)
			{
				foreach (var inner in agg.Flatten().InnerExceptions)
				{
					if (IsHyperVVirtualDiskSharingViolation(inner))
						return true;
				}
			}

			return false;
		}
		public static bool IsDescendantOfHyperVItem(DriveTreeItem item)
		{
			DriveTreeItem? parent = item.Parent;
			while (parent != null)
			{
				if (IsHyperVItem(parent))
					return true;
				parent = parent.Parent;
			}
			return false;
		}
		public static bool IsHyperVItem(DriveTreeItem item) =>
			item.ItemType == DriveTreeItemType.HyperVSystem ||
			item.ItemType == DriveTreeItemType.HyperVVirtualDisk ||
			item.ItemType == DriveTreeItemType.HyperVVolume;


		public static string BuildHyperVGuestRelativePath(DriveTreeItem parentItem, string childResolvedPath)
		{
			if (!HyperVGuestSelectionPath.TryParse(parentItem.FullPath, out HyperVGuestSelectionInfo? parentSelection) || parentSelection == null)
			{
				return string.Empty;
			}

			string childName = Path.GetFileName(childResolvedPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
			if (string.IsNullOrWhiteSpace(childName))
			{
				return parentSelection.RelativePath;
			}

			return HyperVGuestSelectionPath.NormalizeRelativePath(Path.Combine(parentSelection.RelativePath, childName));
		}
		public static string RunPowerShell(string script)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(script);

			string encodedCommand = Convert.ToBase64String(Encoding.Unicode.GetBytes(script));

			using var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encodedCommand}",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			string errors = process.StandardError.ReadToEnd();
			process.WaitForExit();

			if (process.ExitCode != 0)
			{
				string message = string.IsNullOrWhiteSpace(errors)
					? "The PowerShell command failed."
					: StripCliXml(errors).Trim();
				throw new InvalidOperationException(message);
			}

			return output;
		}

		/// <summary>
		/// Extracts readable text from a PowerShell CLIXML error stream.
		/// If the string does not contain CLIXML markup, it is returned unchanged.
		/// </summary>
		public static string StripCliXml(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
				return raw;

			// PowerShell stderr starts with "#< CLIXML" when it wraps errors in XML
			const string clixmlMarker = "#< CLIXML";
			if (!raw.Contains(clixmlMarker, StringComparison.OrdinalIgnoreCase))
				return raw;

			try
			{
				// Extract all <S S="Error">...</S> text nodes — these carry the human-readable message
				var matches = System.Text.RegularExpressions.Regex.Matches(
					raw,
					@"<S S=""Error"">(?<msg>.*?)</S>",
					System.Text.RegularExpressions.RegexOptions.Singleline);

				var lines = matches
					.Select(m => System.Net.WebUtility.HtmlDecode(m.Groups["msg"].Value)
						.Replace("_x000D__x000A_", "\n", StringComparison.Ordinal)
						.Trim())
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.ToList();

				return lines.Count > 0 ? string.Join("\n", lines) : raw;
			}
			catch
			{
				return raw;
			}
		}
		public static string EscapePowerShellSingleQuotedString(string value)
			=> (value ?? string.Empty).Replace("'", "''", StringComparison.Ordinal);

	}
}
