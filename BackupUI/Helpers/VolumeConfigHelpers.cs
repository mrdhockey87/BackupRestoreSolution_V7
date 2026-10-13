using SecureServerBackup.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SecureServerBackup.Helpers
{
	public static class VolumeConfigHelpers
	{/// <summary>
	 /// Gets the list of selected volumes for volume configuration
	 /// </summary>
		public static List<VolumeInfo> GetSelectedVolumesForVolumeConfig(List<DriveTreeItem> driveItems)
		{
			var volumes = new List<VolumeInfo>();

			try
			{
				var checkedItems = GetCheckedDriveItems(driveItems);

				foreach (var item in checkedItems)
				{
					if (item.ItemType == DriveTreeItemType.Volume)
					{
						// Get volume info
						var (totalSize, usedSpace, fileSystem) = GetVolumeInfo(item.FullPath);
						bool isSystemVolume = IsSystemVolume(item.FullPath);
						int aus = GetAllocationUnitSize(fileSystem);

						volumes.Add(new VolumeInfo
						{
							Label = item.Name,
							Size = totalSize,
							UsedSpace = usedSpace,
							FileSystem = fileSystem,
							IsSystemVolume = isSystemVolume,
							AllocationUnitSize = aus,
							IsResizable = false // Will be determined by VolumeConfigurationWindow
						});
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error getting selected volumes: {ex.Message}");
			}

			return volumes;
		}

		/// <summary>
		/// Gets comprehensive volume information
		/// </summary>
		public static (long TotalSize, long UsedSpace, string FileSystem) GetVolumeInfo(string volumePath)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(volumePath))
					return (0, 0, "Unknown");

				var driveInfo = new DriveInfo(volumePath);
				long totalSize = driveInfo.TotalSize;
				long usedSpace = totalSize - driveInfo.AvailableFreeSpace;
				string fileSystem = driveInfo.DriveFormat; // "NTFS", "FAT32", etc.

				return (totalSize, usedSpace, fileSystem);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error getting volume info for {volumePath}: {ex.Message}");
				return (100L * 1024 * 1024 * 1024, 50L * 1024 * 1024 * 1024, "NTFS");
			}
		}

		/// <summary>
		/// Checks if a volume is a system volume
		/// </summary>
		public static bool IsSystemVolume(string volumePath)
		{
			try
			{
				string winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
				return winDir.StartsWith(volumePath, StringComparison.OrdinalIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the allocation unit size for a file system
		/// </summary>
		public static int GetAllocationUnitSize(string fileSystem)
		{
			// Default allocation unit sizes by file system type
			return fileSystem.ToUpperInvariant() switch
			{
				"NTFS" => 4096,      // 4 KB (most common for NTFS)
				"FAT32" => 4096,     // 4 KB
				"EXFAT" => 32768,    // 32 KB
				"REFS" => 65536,     // 64 KB (ReFS default)
				_ => 4096            // Default to 4 KB
			};
		}

		/// <summary>
		/// Gets the target disk allocation unit size
		/// </summary>
		public static int GetTargetAllocationUnitSize(RadioButton rbCloneDisk, RadioButton rbCloneVirtual, TextBox txtCloneDestination, TextBox txtDestination)
		{
			try
			{
				string targetPath = rbCloneDisk?.IsChecked == true
					? txtCloneDestination.Text
					: txtDestination.Text;

				if (string.IsNullOrWhiteSpace(targetPath))
					return 4096;

				string? rootPath = Path.GetPathRoot(targetPath);
				if (string.IsNullOrEmpty(rootPath))
					return 4096;

				var driveInfo = new DriveInfo(rootPath);
				return GetAllocationUnitSize(driveInfo.DriveFormat);
			}
			catch
			{
				return 4096; // Default to 4 KB
			}
		}

		/// <summary>
		/// Gets the target disk size from the clone destination
		/// </summary>
		public static long GetTargetDiskSize(RadioButton rbCloneDisk, RadioButton rbCloneVirtual, TextBox txtCloneDestination)
		{
			try
			{
				bool isCloneToDisk = rbCloneDisk?.IsChecked == true;
				bool isCloneToVirtual = rbCloneVirtual?.IsChecked == true;

				if (isCloneToDisk)
				{
					// For physical disk clones, get size from selected disk
					if (txtCloneDestination.Tag is DiskInfo disk)
					{
						System.Diagnostics.Debug.WriteLine($"[GetTargetDiskSize] Using disk size from DiskInfo: {disk.SizeBytes}");
						return disk.SizeBytes;
					}

					System.Diagnostics.Debug.WriteLine($"[GetTargetDiskSize] No DiskInfo found in tag");
					return 500L * 1024 * 1024 * 1024; // Default: 500GB
				}
				else if (isCloneToVirtual)
				{
					// For virtual disk clones, default to 500GB (user can adjust)
					// In a full implementation, you'd allow the user to specify VHDX size
					System.Diagnostics.Debug.WriteLine($"[GetTargetDiskSize] Using default for virtual disk: 500GB");
					return 500L * 1024 * 1024 * 1024; // Default: 500GB
				}

				return 0;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error getting target disk size: {ex.Message}");
				return 500L * 1024 * 1024 * 1024; // Default: 500GB
			}
		}
		/// <summary>
		/// Gets all checked items from the drive tree
		/// </summary>
		public static List<DriveTreeItem> GetCheckedDriveItems(List<DriveTreeItem> driveItems)
		{
			var checkedItems = new List<DriveTreeItem>();

			foreach (var item in driveItems)
			{
				GetCheckedItemsRecursive(item, checkedItems);
			}

			return checkedItems;
		}
		/// <summary>
		/// Recursively gets checked items from the tree
		/// </summary>
		public static void GetCheckedItemsRecursive(DriveTreeItem item, List<DriveTreeItem> checkedItems)
		{
			if (item.IsChecked == true)
			{
				checkedItems.Add(item);
			}

			foreach (var child in item.Children)
			{
				GetCheckedItemsRecursive(child, checkedItems);
			}
		}

		public static List<DriveTreeItem> BuildVolumeChildItems(DriveTreeItem volumeItem)
		{
			try
			{
				var rootPath = string.IsNullOrWhiteSpace(volumeItem.ResolvedPath) ? volumeItem.FullPath : volumeItem.ResolvedPath;
				List<DriveTreeItem> childItems = new();

				System.Diagnostics.Debug.WriteLine($"=== LoadFoldersForVolume ===");
				System.Diagnostics.Debug.WriteLine($"Volume: {volumeItem.Name}");
				System.Diagnostics.Debug.WriteLine($"Path: '{rootPath}'");

				// Check if this is a system partition without drive letter
				if (rootPath.StartsWith("\\\\?\\Volume{"))
				{
					childItems.Add(new DriveTreeItem
					{
						Name = "(System partition - cannot browse)",
						ItemType = DriveTreeItemType.Folder,
						Parent = volumeItem
					});
					return childItems;
				}

				if (!Directory.Exists(rootPath))
				{
					System.Diagnostics.Debug.WriteLine($"ERROR: Directory does not exist: '{rootPath}'");
					childItems.Add(new DriveTreeItem
					{
						Name = "(Volume not accessible)",
						ItemType = DriveTreeItemType.Folder,
						Parent = volumeItem
					});
					return childItems;
				}

				System.Diagnostics.Debug.WriteLine($"Directory exists, enumerating folders and files...");

				var entriesAdded = 0;
				try
				{
					foreach (DriveTreeItem childItem in BuildDirectoryChildItems(volumeItem, rootPath))
					{
						childItems.Add(childItem);
						entriesAdded++;
						System.Diagnostics.Debug.WriteLine($"  Added: {childItem.Name}");
					}
				}
				catch (UnauthorizedAccessException)
				{
					System.Diagnostics.Debug.WriteLine("ERROR: Access denied to volume root");
					childItems.Add(new DriveTreeItem
					{
						Name = "(Access Denied - Run as Administrator)",
						ItemType = DriveTreeItemType.Folder,
						Parent = volumeItem
					});
					return childItems;
				}

				System.Diagnostics.Debug.WriteLine($"Total entries added: {entriesAdded}");

				// If no folders or files were accessible, show a message
				if (entriesAdded == 0)
				{
					childItems.Add(new DriveTreeItem
					{
						Name = "(Empty or no accessible files/folders)",
						ItemType = DriveTreeItemType.Folder,
						Parent = volumeItem
					});
				}

				return childItems;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR in LoadFoldersForVolume: {ex.Message}\nStack: {ex.StackTrace}");
				return new List<DriveTreeItem>
				{
					new()
					{
						Name = $"(Error: {ex.Message})",
						ItemType = DriveTreeItemType.Folder,
						Parent = volumeItem
					}
				};
			}
		}

		public static List<DriveTreeItem> BuildDirectoryChildItems(DriveTreeItem parentItem, string rootPath)
		{
			ArgumentNullException.ThrowIfNull(parentItem);
			ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);

			List<DriveTreeItem> childItems = new();

			foreach (string directory in Directory.GetDirectories(rootPath).OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
			{
				try
				{
					DirectoryInfo dirInfo = new(directory);
					childItems.Add(TreeItemHelpers.CreateDirectoryTreeItem(parentItem, dirInfo));
				}
				catch (UnauthorizedAccessException)
				{
					childItems.Add(TreeItemHelpers.CreateAccessDeniedDirectoryTreeItem(parentItem, directory));
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"  Error processing folder {directory}: {ex.Message}");
				}
			}

			foreach (string file in Directory.GetFiles(rootPath).OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
			{
				try
				{
					FileInfo fileInfo = new(file);
					childItems.Add(TreeItemHelpers.CreateFileTreeItem(parentItem, fileInfo));
				}
				catch (UnauthorizedAccessException)
				{
					Debug.WriteLine($"  Access denied file: {Path.GetFileName(file)}");
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"  Error processing file {file}: {ex.Message}");
				}
			}

			return childItems;
		}

	}
}
