using SecureServerBackup.Models;
using SecureServerBackup.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SecureServerBackup.Helpers
{
	public static class DriveEnumeration
	{
		private static bool _showHiddenPartitions = false;
		public static async Task LoadPhysicalDrives(List<DriveTreeItem> driveItems, bool showHiddenPartitions)
		{
			_showHiddenPartitions = showHiddenPartitions;
			List<DriveTreeItem> disks = await Task.Run(() =>
			{
				var discovered = new List<(int Index, DriveTreeItem Item)>();
				try
				{
					Debug.WriteLine("=== Starting LoadPhysicalDrives ===");
					// Only request the columns we use; WQL has no ORDER BY, so sort in code.
					using var searcher = new ManagementObjectSearcher(
						"SELECT Index, Model, DeviceID, Size FROM Win32_DiskDrive")
					{
						// Forward-only, no deep copies: lower memory and faster enumeration.
						Options = new System.Management.EnumerationOptions
						{
							ReturnImmediately = true,
							Rewindable = false
						}
					};
					using ManagementObjectCollection results = searcher.Get();
					foreach (ManagementObject disk in results.Cast<ManagementObject>())
					{
						using (disk)
						{
							try
							{
								int diskIndex = 0;
								var indexObj = disk["Index"];
								if (indexObj != null)
									diskIndex = Convert.ToInt32(indexObj);
								else
									Debug.WriteLine("Warning: Index property is null");
								string model = disk["Model"]?.ToString() ?? "Unknown Model";
								string deviceId = disk["DeviceID"]?.ToString() ?? "";
								long size = disk["Size"] is { } s ? Convert.ToInt64(s) : 0;
								var diskItem = new DriveTreeItem
								{
									Name = $"Disk {diskIndex} - {model}",
									FullPath = deviceId,
									ItemType = DriveTreeItemType.Disk,
									Size = size
								};
								Debug.WriteLine($"=== Found Disk {diskIndex}: {model} ({deviceId}) ===");
								LoadVolumesForDisk(diskItem, diskIndex);
								Debug.WriteLine($"Disk {diskIndex}: Found {diskItem.Children.Count} volumes");
								if (diskItem.Children.Count == 0)
								{
									diskItem.Children.Add(new DriveTreeItem
									{
										Name = "(No accessible volumes)",
										ItemType = DriveTreeItemType.Volume,
										Parent = diskItem
									});
								}
								discovered.Add((diskIndex, diskItem));
							}
							catch (Exception diskEx)
							{
								Debug.WriteLine($"Error processing individual disk: {diskEx.Message}");
								UiDispatcherHelper.Invoke(() =>
									CustomDialogService.ShowWarning(
										$"Error processing a disk: {diskEx.Message}\n\nContinuing with remaining disks...",
										"Warning"));
							}
						}
					}
					// Sort in memory by disk index, replacing ORDER BY.
					discovered.Sort((a, b) => a.Index.CompareTo(b.Index));
					Debug.WriteLine($"=== Completed LoadPhysicalDrives: {discovered.Count} disks loaded ===");
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"CRITICAL ERROR in LoadPhysicalDrives: {ex.Message}\nStack: {ex.StackTrace}");
					UiDispatcherHelper.Invoke(() =>
						CustomDialogService.ShowError(
							$"Error loading physical drives: {ex.Message}\n\nDetails: {ex.GetType().Name}\n\nPlease check Output window for details.",
							"Error"));
				}
				return discovered.Select(d => d.Item).ToList();
			});
			driveItems.AddRange(disks);
		}

		public static void LoadVolumesForDisk(DriveTreeItem diskItem, int diskNum)
		{
			var volumesFound = false;

			try
			{
				System.Diagnostics.Debug.WriteLine($"=== Loading volumes for Disk {diskNum}: {diskItem.FullPath} ===");

				// Try method 1: WMI Associators (most accurate but sometimes fails)
				volumesFound = TryLoadVolumesViaSSB(diskItem, diskNum);

				// Try method 2: Alternative WMI query if method 1 failed
				if (!volumesFound)
				{
					System.Diagnostics.Debug.WriteLine($"All SSB methods failed for disk {diskNum}, using fallback");
					LoadVolumesSimpleFallback(diskItem, diskNum);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in LoadVolumesForDisk for disk {diskNum}: {ex.Message}");
			}
		}

		public static bool TryLoadVolumesViaSSB(DriveTreeItem diskItem, int diskNum)
		{
			var volumesFound = false;

			try
			{
				// Query all partitions on this disk
				var query = $"SELECT * FROM Win32_DiskPartition WHERE DiskIndex = {diskNum}";

				System.Diagnostics.Debug.WriteLine($"Alternative query: {query}");

				using var searcher = new ManagementObjectSearcher(query);
				var partitions = searcher.Get();

				System.Diagnostics.Debug.WriteLine($"Alternative query found {partitions.Count} partitions");

				foreach (ManagementObject partition in partitions)
				{
					var partitionDeviceId = partition["DeviceID"]?.ToString();
					var partitionSize = Convert.ToInt64(partition["Size"] ?? 0);
					System.Diagnostics.Debug.WriteLine($"  Partition: {partitionDeviceId} ({partitionSize / (1024.0 * 1024.0 * 1024.0):F2} GB)");

					// Try to find logical disk for this partition
					var logicalQuery = $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionDeviceId}'}} WHERE AssocClass = Win32_LogicalDiskToPartition";

					using var logicalSearcher = new ManagementObjectSearcher(logicalQuery);
					var logicalDisks = logicalSearcher.Get();

					if (logicalDisks.Count > 0)
					{
						// Has drive letter
						foreach (ManagementObject logical in logicalDisks)
						{
							var driveLetter = logical["DeviceID"]?.ToString();
							if (string.IsNullOrEmpty(driveLetter)) continue;

							System.Diagnostics.Debug.WriteLine($"    Found logical disk: {driveLetter}");

							if (AddVolumeToTree(diskItem, driveLetter))
								volumesFound = true;
						}
					}
					else
					{
						// No drive letter - query Win32_Volume directly
						System.Diagnostics.Debug.WriteLine($"    No logical disk, checking Win32_Volume...");

						// Query volumes by DiskNumber (Win32_Volume has DeviceID that includes partition info)
						var volumeQuery = $"SELECT * FROM Win32_Volume WHERE DriveType = 3"; // Fixed disk
						using var volumeSearcher = new ManagementObjectSearcher(volumeQuery);

						foreach (ManagementObject volume in volumeSearcher.Get())
						{
							try
							{
								var volumeDeviceId = volume["DeviceID"]?.ToString();
								var volumeName = volume["Label"]?.ToString() ?? "";
								var volumeCapacity = Convert.ToInt64(volume["Capacity"] ?? 0);

								// Check if this volume's size matches the partition
								if (Math.Abs(volumeCapacity - partitionSize) < 1024 * 1024 * 100) // Within 100MB
								{
									var volumeType = "Unknown";
									if (volumeName.Contains("EFI", StringComparison.OrdinalIgnoreCase) ||
										volumeDeviceId?.Contains("EFI", StringComparison.OrdinalIgnoreCase) == true)
									{
										volumeType = "EFI System Partition";
									}
									else if (volumeName.Contains("Recovery", StringComparison.OrdinalIgnoreCase) ||
											 volumeDeviceId?.Contains("Recovery", StringComparison.OrdinalIgnoreCase) == true)
									{
										volumeType = "Recovery Partition";
									}
									else
									{
										volumeType = string.IsNullOrEmpty(volumeName) ? "System Reserved" : volumeName;
									}

									var volumeItem = new DriveTreeItem
									{
										Name = $"(No Letter) {volumeType} ({volumeCapacity / (1024.0 * 1024.0 * 1024.0):F2} GB)",
										FullPath = volumeDeviceId ?? "",
										ItemType = DriveTreeItemType.Volume,
										Size = volumeCapacity,
										Parent = diskItem,
										IsBootVolume = volumeType.Contains("EFI"),
										IsHiddenPartition = true
									};

									// These volumes typically can't be browsed
									volumeItem.Children.Add(new DriveTreeItem
									{
										Name = "(System partition - not accessible)",
										ItemType = DriveTreeItemType.Folder,
										Parent = volumeItem
									});

									if (_showHiddenPartitions)
									{
										diskItem.Children.Add(volumeItem);
										volumesFound = true;
									}

									System.Diagnostics.Debug.WriteLine($"      {(_showHiddenPartitions ? "Added" : "Skipped (hidden)")} system volume: {volumeType}");
									break; // Found matching volume
								}
							}
							catch (Exception ex)
							{
								System.Diagnostics.Debug.WriteLine($"      Error checking volume: {ex.Message}");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"TryLoadVolumesViaAlternativeSSB failed: {ex.Message}");
			}

			return volumesFound;
		}
		public static bool AddVolumeToTree(DriveTreeItem diskItem, string driveLetter)
		{
			try
			{
				var driveInfo = new DriveInfo(driveLetter);
				if (!driveInfo.IsReady)
				{
					System.Diagnostics.Debug.WriteLine($"      Drive {driveLetter} not ready");
					return false;
				}

				var volumeLabel = string.IsNullOrEmpty(driveInfo.VolumeLabel)
					? "Local Disk"
					: driveInfo.VolumeLabel;

				// Ensure the FullPath has a trailing backslash for directory enumeration
				var volumePath = driveLetter.TrimEnd('\\') + "\\";

				var volumeItem = new DriveTreeItem
				{
					Name = $"{driveLetter} ({volumeLabel})",
					FullPath = volumePath,  // Changed: Now includes trailing backslash (e.g., "E:\")
					ItemType = DriveTreeItemType.Volume,
					Size = driveInfo.TotalSize,
					Parent = diskItem,
					IsBootVolume = IsBootVolume(driveLetter),
					IsWindowsServer = IsWindowsServerVolume(driveLetter)
				};

				volumeItem.Children.Add(new DriveTreeItem
				{
					Name = "Loading...",
					ItemType = DriveTreeItemType.Folder,
					Parent = volumeItem
				});

				diskItem.Children.Add(volumeItem);
				System.Diagnostics.Debug.WriteLine($"      Added {driveLetter} to tree (path: {volumePath})");
				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"      Error adding {driveLetter}: {ex.Message}");
				return false;
			}
		}

		public static void LoadVolumesSimpleFallback(DriveTreeItem diskItem, int diskNum)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine($"Using simple fallback for disk {diskNum}");

				// Simple approach: Show all fixed drives
				// We can't determine which disk they're on, so we'll add them to the first disk
				if (diskNum == 0)
				{
					foreach (var drive in DriveInfo.GetDrives())
					{
						try
						{
							if (!drive.IsReady || drive.DriveType != DriveType.Fixed)
								continue;

							var volumeLabel = string.IsNullOrEmpty(drive.VolumeLabel)
								? "Local Disk"
								: drive.VolumeLabel;

							// Ensure trailing backslash for directory enumeration
							var volumePath = drive.Name.TrimEnd('\\') + "\\";
							var displayName = drive.Name.TrimEnd('\\');

							var volumeItem = new DriveTreeItem
							{
								Name = $"{displayName} ({volumeLabel})",
								FullPath = volumePath,  // Changed: Now includes trailing backslash
								ItemType = DriveTreeItemType.Volume,
								Size = drive.TotalSize,
								Parent = diskItem,
								IsBootVolume = IsBootVolume(drive.Name),
								IsWindowsServer = IsWindowsServerVolume(drive.Name)
							};

							// Add placeholder for folders
							volumeItem.Children.Add(new DriveTreeItem
							{
								Name = "Loading...",
								ItemType = DriveTreeItemType.Folder,
								Parent = volumeItem
							});

							diskItem.Children.Add(volumeItem);
							System.Diagnostics.Debug.WriteLine($"Fallback: Added {drive.Name} to disk {diskNum} (path: {volumePath})");
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine($"Error adding drive in fallback: {ex.Message}");
						}
					}
				}
				else
				{
					// For other disks, just show a message
					diskItem.Children.Add(new DriveTreeItem
					{
						Name = "(Cannot map volumes - see Disk 0 for all volumes)",
						ItemType = DriveTreeItemType.Volume,
						Parent = diskItem
					});
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in fallback method: {ex.Message}");
			}
		}

		public static bool IsBootVolume(string driveLetter)
		{
			try
			{
				var systemDrive = Environment.GetFolderPath(Environment.SpecialFolder.System);
				return driveLetter.TrimEnd('\\').Equals(Path.GetPathRoot(systemDrive)?.TrimEnd('\\'),
					StringComparison.OrdinalIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		public static bool IsWindowsServerVolume(string driveLetter)
		{
			try
			{
				var systemDrive = Environment.GetFolderPath(Environment.SpecialFolder.System);
				if (!driveLetter.TrimEnd('\\').Equals(Path.GetPathRoot(systemDrive)?.TrimEnd('\\'),
					StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}

				// Check if it's Windows Server
				using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
				foreach (ManagementObject os in searcher.Get())
				{
					var caption = os["Caption"]?.ToString() ?? "";
					return caption.Contains("Server", StringComparison.OrdinalIgnoreCase);
				}
			}
			catch { }

			return false;
		}

		public static IReadOnlyList<int> GetCurrentSystemDiskIndexes()
		{
			List<int> indexes = new();

			try
			{
				string systemRoot = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))?.TrimEnd('\\') ?? string.Empty;
				if (string.IsNullOrWhiteSpace(systemRoot))
				{
					return indexes;
				}

				using var partitionSearcher = new ManagementObjectSearcher(
					$"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{systemRoot}'}} WHERE AssocClass=Win32_LogicalDiskToPartition");

				foreach (ManagementObject partition in partitionSearcher.Get())
				{
					string? partitionId = partition["DeviceID"]?.ToString();
					if (string.IsNullOrWhiteSpace(partitionId))
					{
						continue;
					}

					using var driveSearcher = new ManagementObjectSearcher(
						$"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionId}'}} WHERE AssocClass=Win32_DiskDriveToDiskPartition");

					foreach (ManagementObject drive in driveSearcher.Get())
					{
						if (int.TryParse(drive["Index"]?.ToString(), out int diskIndex) && !indexes.Contains(diskIndex))
						{
							indexes.Add(diskIndex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"GetCurrentSystemDiskIndexes warning: {ex.Message}");
			}

			return indexes;
		}

	}
}
