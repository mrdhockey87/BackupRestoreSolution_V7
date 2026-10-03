using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	internal sealed record CloneHyperVPaths(string RootDirectory, string HyperVSystemDirectory, string HyperVDiskDirectory, string VirtualDiskPath, string VmName);

	public sealed record HyperVVirtualDiskInfo(string VirtualMachineName, string VirtualMachineDisplayName, string VirtualDiskPath);
	public static class HyperVBackupTreeHelper
	{
		public static string NormalizeSavedHyperVSystemName(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			string normalizedValue = value.Trim();
			const string hyperVPrefix = "Hyper-V:";
			if (normalizedValue.StartsWith(hyperVPrefix, StringComparison.OrdinalIgnoreCase))
			{
				normalizedValue = normalizedValue[hyperVPrefix.Length..].Trim();
			}

			return RegularHyperVRestoreHelper.NormalizeHyperVVmName(normalizedValue);
		}

		public static IReadOnlyList<HyperVVirtualDiskInfo> ParseVirtualDiskEnumeration(string? output)
		{
			if (string.IsNullOrWhiteSpace(output))
			{
				return Array.Empty<HyperVVirtualDiskInfo>();
			}

			List<HyperVVirtualDiskInfo> disks = new();
			foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
			{
				string[] parts = line.Split('\t');
				if (parts.Length < 3)
				{
					continue;
				}

				string vmName = RegularHyperVRestoreHelper.NormalizeHyperVVmName(parts[0].Trim());
				string vmDisplayName = string.IsNullOrWhiteSpace(parts[1]) ? vmName : parts[1].Trim();
				string virtualDiskPath = parts[2].Trim().Trim('"');

				if (string.IsNullOrWhiteSpace(vmName) || string.IsNullOrWhiteSpace(virtualDiskPath))
				{
					continue;
				}

				disks.Add(new HyperVVirtualDiskInfo(vmName, vmDisplayName, virtualDiskPath));
			}

			return disks;
		}

		public static bool IsVirtualDiskResource(string? resourceSubType)
		{
			if (string.IsNullOrWhiteSpace(resourceSubType))
			{
				return false;
			}

			return resourceSubType.Contains("Virtual Hard Disk", StringComparison.OrdinalIgnoreCase) ||
				   resourceSubType.Contains("Microsoft:Hyper-V:Virtual Hard Disk", StringComparison.OrdinalIgnoreCase);
		}

		public static IEnumerable<string> GetHostResources(object? hostResourceValue)
		{
			if (hostResourceValue is string singleValue)
			{
				yield return singleValue;
				yield break;
			}

			if (hostResourceValue is string[] array)
			{
				foreach (string value in array)
				{
					if (!string.IsNullOrWhiteSpace(value))
					{
						yield return value;
					}
				}

				yield break;
			}

			if (hostResourceValue is Array values)
			{
				foreach (object? value in values)
				{
					string? resource = value?.ToString();
					if (!string.IsNullOrWhiteSpace(resource))
					{
						yield return resource;
					}
				}
			}
		}

		public static string BuildVmDisplayName(string vmName, object? enabledState)
		{
			if (enabledState is null || !int.TryParse(enabledState.ToString(), out int state))
			{
				return vmName;
			}

			return state switch
			{
				2 => $"{vmName} (Running)",
				3 => $"{vmName} (Off)",
				32768 => $"{vmName} (Paused)",
				32769 => $"{vmName} (Saved)",
				_ => $"{vmName} (Unknown State)"
			};
		}

		public static string SelectMountableVirtualDiskPath(string requestedPath, IEnumerable<string>? chainPaths)
		{
			if (string.IsNullOrWhiteSpace(requestedPath))
			{
				return string.Empty;
			}

			string selectedPath = requestedPath;
			if (chainPaths is null)
			{
				return selectedPath;
			}

			foreach (string chainPath in chainPaths)
			{
				string normalizedPath = chainPath.Trim().Trim('"');
				if (!string.IsNullOrWhiteSpace(normalizedPath))
				{
					selectedPath = normalizedPath;
				}
			}

			return selectedPath;
		}

		public static bool ShouldScheduleSetupCl(bool renameHyperVSystem, string? renameHyperVSystemName, BackupTarget target, IEnumerable<string>? sourcePaths, IEnumerable<int>? protectedDiskIndexes = null)
		{
			bool renamedClone = renameHyperVSystem && !string.IsNullOrWhiteSpace(renameHyperVSystemName);
			HashSet<int> protectedDisks = protectedDiskIndexes?
				.Where(index => index >= 0)
				.ToHashSet() ?? new HashSet<int>();

			bool clonedFromSystemDisk = target == BackupTarget.Disk && (sourcePaths?.Any(path =>
				TryGetPhysicalDriveNumber(path, out int diskNumber) && protectedDisks.Contains(diskNumber)) ?? false);

			return renamedClone || clonedFromSystemDisk;
		}

		private static bool TryGetPhysicalDriveNumber(string? path, out int diskNumber)
		{
			diskNumber = -1;
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}

			const string physicalDrivePrefix = "PHYSICALDRIVE";
			int prefixIndex = path.LastIndexOf(physicalDrivePrefix, StringComparison.OrdinalIgnoreCase);
			if (prefixIndex < 0)
			{
				return false;
			}

			string suffix = path[(prefixIndex + physicalDrivePrefix.Length)..].Trim();
			return int.TryParse(suffix, out diskNumber);
		}
	}
}
