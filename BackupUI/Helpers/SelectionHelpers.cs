using SecureServerBackup.Models;

using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public static class SelectionHelpers
	{
		public static void CollectSelectedHyperVMachines(BackupJob job, List<DriveTreeItem> driveItems)
		{
			job.IsHyperVBackup = true;
			job.Target = BackupTarget.HyperV;

			foreach (var drive in driveItems)
			{
				CollectHyperVMachinesRecursive(drive, job);
			}
		}

		public static void CollectHyperVMachinesRecursive(DriveTreeItem item, BackupJob job)
		{
			if (item.ItemType == DriveTreeItemType.HyperVSystem && item.IsChecked == true)
			{
				string vmName = HyperVBackupTreeHelper.NormalizeSavedHyperVSystemName(
					string.IsNullOrWhiteSpace(item.VirtualMachineName) ? item.FullPath : item.VirtualMachineName);
				if (!string.IsNullOrWhiteSpace(vmName))
				{
					job.HyperVMachines.Add(vmName);
				}
				return; // whole VM selected; don't recurse into its children
			}

			if (item.IsChecked == null || item.IsChecked == true)
			{
				foreach (var child in item.Children)
				{
					CollectHyperVMachinesRecursive(child, job);
				}
			}
		}

		public static void CollectSelectedItems(BackupJob job, List<DriveTreeItem> driveItems)
		{
			bool selectedFilesAndFoldersOnly = (job.Type == BackupType.SelectedFilesAndFolders || job.Target == BackupTarget.FilesAndFolders)
												&& job.Target != BackupTarget.Disk  && job.Target != BackupTarget.Volume
												&& job.Target != BackupTarget.HyperV;

			if (selectedFilesAndFoldersOnly)
			{
				job.Target = BackupTarget.FilesAndFolders;
				List<DriveTreeItem> selectedFilesItems = GetEffectiveSelectedFilesAndFoldersItems(driveItems);
				foreach (DriveTreeItem selectedItem in selectedFilesItems)
				{
					if (string.IsNullOrWhiteSpace(selectedItem.FullPath))
					{
						continue;
					}
					job.SourcePaths.Add(selectedItem.FullPath);
				}

				job.SourcePaths = job.SourcePaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();

				return;
			}

			foreach (var drive in driveItems)
			{
				if (drive.IsChecked == true)
				{
					if (drive.ItemType == DriveTreeItemType.Disk)
					{
						job.Target = BackupTarget.Disk;
						job.SourcePaths.Add(drive.FullPath);
					}
					else if (drive.ItemType == DriveTreeItemType.HyperVSystem)
					{
						job.Target = BackupTarget.HyperV;
						string vmName = RegularHyperVRestoreHelper.NormalizeHyperVVmName(drive.FullPath);
						if (!string.IsNullOrWhiteSpace(vmName))
						{
							job.HyperVMachines.Add(vmName);
						}
					}
					else if (drive.ItemType == DriveTreeItemType.HyperVVirtualDisk ||
							 drive.ItemType == DriveTreeItemType.HyperVVolume)
					{
						// Guest-disk or guest-volume path backup — treated as files/folders
						job.Target = BackupTarget.FilesAndFolders;
						job.SourcePaths.Add(drive.FullPath);
					}
					else if (drive.ItemType == DriveTreeItemType.NetworkDrive ||
							 drive.ItemType == DriveTreeItemType.NetworkShare)
					{
						job.Target = BackupTarget.FilesAndFolders;
						job.SourcePaths.Add(drive.FullPath);
					}
					// NetworkRoot and NetworkBrowser are display-only sentinels; skip them
				}
				else if (drive.IsChecked == null && drive.Children.Count > 0)
				{
					// Partial selection — recurse into children
					CollectSelectedChildren(drive, job);
				}
			}

			// Determine target type if not already set
			if (job.Target == 0 && job.SourcePaths.Count > 0)
			{
				if (job.Type == BackupType.SelectedFilesAndFolders)
				{
					job.Target = BackupTarget.FilesAndFolders;
				}
				else
				{
					// Check if all sources are drive letters (volumes), device paths (disks), or regular paths (files/folders)
					var firstPath = job.SourcePaths[0];

					// Check for PHYSICALDRIVE device paths (e.g., \\.\PHYSICALDRIVE5)
					if (firstPath.StartsWith(@"\\.\", StringComparison.OrdinalIgnoreCase) &&
						firstPath.Contains("PHYSICALDRIVE", StringComparison.OrdinalIgnoreCase))
					{
						job.Target = BackupTarget.Disk;
					}
					// Check for Volume GUID paths (e.g., \\?\\Volume{guid})
					else if (firstPath.StartsWith(@"\\?\", StringComparison.OrdinalIgnoreCase) &&
							 firstPath.Contains("Volume{", StringComparison.OrdinalIgnoreCase))
					{
						job.Target = BackupTarget.Volume;
					}
					// Check for simple drive letters (e.g., C:, E:, W:)
					else if (firstPath.Length <= 3 && firstPath.EndsWith(":"))
					{
						job.Target = BackupTarget.Volume;
					}
					// Everything else is files/folders
					else
					{
						job.Target = BackupTarget.FilesAndFolders;
					}
				}
			}

			if (job.HyperVMachines.Count > 0)
			{
				job.IsHyperVBackup = true;
				job.Target = BackupTarget.HyperV;
			}
		}

		public static void CollectSelectedChildren(DriveTreeItem parent, BackupJob job)
		{
			bool selectedFilesAndFoldersOnly = job.Type == BackupType.SelectedFilesAndFolders || job.Target == BackupTarget.FilesAndFolders;

			foreach (var child in parent.Children)
			{
				if (child.IsChecked == true)
				{
					if (selectedFilesAndFoldersOnly)
					{
						if (!string.IsNullOrWhiteSpace(child.FullPath))
						{
							job.SourcePaths.Add(child.FullPath);
						}

						continue;
					}
					else if (child.ItemType == DriveTreeItemType.Volume)
					{
						if (job.Target == 0) job.Target = BackupTarget.Volume;
						job.SourcePaths.Add(child.FullPath);
					}
					else if (child.ItemType == DriveTreeItemType.Disk)
					{
						job.Target = BackupTarget.Disk;
						job.SourcePaths.Add(child.FullPath);
					}
					else if (child.ItemType == DriveTreeItemType.HyperVSystem)
					{
						string vmName = RegularHyperVRestoreHelper.NormalizeHyperVVmName(child.FullPath);
						if (!string.IsNullOrWhiteSpace(vmName))
						{
							job.HyperVMachines.Add(vmName);
						}
					}
					else if (child.ItemType == DriveTreeItemType.HyperVVirtualDisk ||
							 child.ItemType == DriveTreeItemType.HyperVVolume)
					{
						job.Target = BackupTarget.FilesAndFolders;
						job.SourcePaths.Add(child.FullPath);
					}
					else if (child.ItemType == DriveTreeItemType.Folder ||
							 child.ItemType == DriveTreeItemType.File)
					{
						// Skip placeholder nodes that are children of Hyper-V items
						if (HyperVHelpers.IsDescendantOfHyperVItem(child) && !HyperVGuestSelectionPath.IsEncodedPath(child.FullPath))
							continue;
						job.Target = BackupTarget.FilesAndFolders;
						job.SourcePaths.Add(child.FullPath);
					}
					else if (child.ItemType == DriveTreeItemType.NetworkDrive ||
							 child.ItemType == DriveTreeItemType.NetworkShare)
					{
						job.Target = BackupTarget.FilesAndFolders;
						job.SourcePaths.Add(child.FullPath);
					}
					// NetworkRoot and NetworkBrowser are display-only sentinels; skip them
				}
				else if (child.IsChecked == null && child.Children.Count > 0)
				{
					CollectSelectedChildren(child, job);
				}
			}
		}


		public static void CollectEffectiveSelectedFilesAndFoldersItems(DriveTreeItem item, List<DriveTreeItem> selectedItems)
		{
			ArgumentNullException.ThrowIfNull(item);
			ArgumentNullException.ThrowIfNull(selectedItems);

			if (item.IsChecked == true)
			{
				if (IsSelectedFilesAndFoldersAllowedItem(item))
				{
					selectedItems.Add(item);
					return;
				}

				foreach (DriveTreeItem child in item.Children)
				{
					CollectEffectiveSelectedFilesAndFoldersItems(child, selectedItems);
				}

				return;
			}

			if (item.IsChecked == null)
			{
				foreach (DriveTreeItem child in item.Children)
				{
					CollectEffectiveSelectedFilesAndFoldersItems(child, selectedItems);
				}
			}
		}

		public static List<DriveTreeItem> GetEffectiveSelectedFilesAndFoldersItems(List<DriveTreeItem> driveItems)
		{
			List<DriveTreeItem> selectedItems = new();

			foreach (DriveTreeItem drive in driveItems)
			{
				CollectEffectiveSelectedFilesAndFoldersItems(drive, selectedItems);
			}

			return selectedItems;
		}

		public static List<string> GetSelectedFilesSourceRoots(List<DriveTreeItem> driveItems)
		{
			List<string> sourceRoots = new();

			foreach (DriveTreeItem selectedItem in GetEffectiveSelectedFilesAndFoldersItems(driveItems))
			{
				AddSelectedFilesSourceRoots(selectedItem, sourceRoots);
			}

			return sourceRoots
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		public static void AddSelectedFilesSourceRoots(DriveTreeItem selectedItem, List<string> sourceRoots)
		{
			ArgumentNullException.ThrowIfNull(selectedItem);
			ArgumentNullException.ThrowIfNull(sourceRoots);

			for (DriveTreeItem? current = selectedItem; current != null; current = current.Parent)
			{
				switch (current.ItemType)
				{
					case DriveTreeItemType.Disk:
					case DriveTreeItemType.Volume:
					case DriveTreeItemType.NetworkDrive:
					case DriveTreeItemType.NetworkShare:
					case DriveTreeItemType.HyperVVirtualDisk:
					case DriveTreeItemType.HyperVVolume:
						if (!string.IsNullOrWhiteSpace(current.FullPath))
						{
							sourceRoots.Add(current.FullPath);
						}

						break;
				}
			}
		}
		public static bool IsSelectedFilesAndFoldersAllowedItem(DriveTreeItem item)
		{
			ArgumentNullException.ThrowIfNull(item);

			return item.ItemType switch
			{
				DriveTreeItemType.Folder => !HyperVHelpers.IsDescendantOfHyperVItem(item) || HyperVGuestSelectionPath.IsEncodedPath(item.FullPath),
				DriveTreeItemType.File => !HyperVHelpers.IsDescendantOfHyperVItem(item) || HyperVGuestSelectionPath.IsEncodedPath(item.FullPath),
				DriveTreeItemType.NetworkDrive => true,
				DriveTreeItemType.NetworkShare => true,
				_ => false
			};
		}

	}
}
