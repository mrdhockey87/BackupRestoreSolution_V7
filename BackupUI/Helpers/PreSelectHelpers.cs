using SecureServerBackup.Models;

using SecureServerBackupCommon;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public static class PreSelectHelpers
	{
		public static async Task<bool> PreSelectStandardItemAsync(string pathToSelect, List<DriveTreeItem> driveItems, HashSet<string> _pendingSelectionPaths)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(pathToSelect);

			foreach (DriveTreeItem item in driveItems)
			{
				if (await PreSelectItemRecursiveAsync(item, pathToSelect, _pendingSelectionPaths))
				{
					return true;
				}
			}

			return false;
		}

		public static async Task<bool> PreSelectItemRecursiveAsync(DriveTreeItem item, string pathToSelect, HashSet<string> _pendingSelectionPaths)
		{
			ArgumentNullException.ThrowIfNull(item);
			ArgumentException.ThrowIfNullOrWhiteSpace(pathToSelect);

			string targetPath = NormalizeSelectionPath(pathToSelect);
			string itemPath = NormalizeSelectionPath(item.FullPath);
			string resolvedPath = NormalizeSelectionPath(item.ResolvedPath);

			if ((!string.IsNullOrWhiteSpace(itemPath) && string.Equals(itemPath, targetPath, StringComparison.OrdinalIgnoreCase)) ||
				(!string.IsNullOrWhiteSpace(resolvedPath) && string.Equals(resolvedPath, targetPath, StringComparison.OrdinalIgnoreCase)))
			{
				item.IsChecked = true;
				return true;
			}

			bool itemPathCanContainSelection = PathCouldContainSelection(itemPath, targetPath);
			bool resolvedPathCanContainSelection = PathCouldContainSelection(resolvedPath, targetPath);
			if (!itemPathCanContainSelection && !resolvedPathCanContainSelection)
			{
				foreach (DriveTreeItem child in item.Children)
				{
					if (await PreSelectItemRecursiveAsync(child, pathToSelect, _pendingSelectionPaths))
					{
						return true;
					}
				}

				return false;
			}

			if (item.IsChecked != true)
			{
				item.IsChecked = null;
			}

			if (RequiresLazyLoad(item) && !item.ChildrenLoaded)
			{
				if (item.ItemType == DriveTreeItemType.HyperVVirtualDisk)
				{
					await HyperVHelpers.LoadHyperVVirtualDiskChildrenAsync(item, pathToSelect);
				}
				else
				{
					List<DriveTreeItem> childItems = await Task.Run(() => VolumeConfigHelpers.BuildVolumeChildItems(item));
					ReplaceChildren(item, childItems);
				}

				item.ChildrenLoaded = true;
				ApplyPendingSelectionState(item, _pendingSelectionPaths);
			}

			foreach (DriveTreeItem child in item.Children)
			{
				if (await PreSelectItemRecursiveAsync(child, pathToSelect, _pendingSelectionPaths))
				{
					return true;
				}
			}

			return false;
		}


		public static void ApplyPendingSelectionState(DriveTreeItem item,HashSet<string> _pendingSelectionPaths)
		{
			ArgumentNullException.ThrowIfNull(item);

			foreach (DriveTreeItem child in item.Children)
			{
				ApplyPendingSelectionStateRecursive(child, _pendingSelectionPaths);
			}

			item.RefreshCheckStateFromChildren();
		}

		public static void ApplyPendingSelectionStateRecursive(DriveTreeItem item, HashSet<string> _pendingSelectionPaths)
		{
			ArgumentNullException.ThrowIfNull(item);

			if (item.IsLoadingPlaceholder)
			{
				return;
			}

			GetPendingSelectionMatchState(item, out bool hasExactMatch, out bool hasDescendantMatch, _pendingSelectionPaths);

			if (hasExactMatch)
			{
				item.IsChecked = true;
				return;
			}

			if (hasDescendantMatch && item.IsChecked != true)
			{
				item.IsChecked = null;
			}

			foreach (DriveTreeItem child in item.Children)
			{
				ApplyPendingSelectionStateRecursive(child, _pendingSelectionPaths);
			}

			item.RefreshCheckStateFromChildren();
		}

		public static void GetPendingSelectionMatchState(DriveTreeItem item, out bool hasExactMatch, out bool hasDescendantMatch, HashSet<string> _pendingSelectionPaths)
		{
			ArgumentNullException.ThrowIfNull(item);

			hasExactMatch = false;
			hasDescendantMatch = false;

			string itemPath = NormalizeSelectionPath(item.FullPath);
			string resolvedPath = NormalizeSelectionPath(item.ResolvedPath);

			foreach (string pendingPath in _pendingSelectionPaths)
			{
				if ((!string.IsNullOrWhiteSpace(itemPath) && string.Equals(itemPath, pendingPath, StringComparison.OrdinalIgnoreCase)) ||
					(!string.IsNullOrWhiteSpace(resolvedPath) && string.Equals(resolvedPath, pendingPath, StringComparison.OrdinalIgnoreCase)))
				{
					hasExactMatch = true;
					hasDescendantMatch = true;
					return;
				}

				if (PathCouldContainSelection(itemPath, pendingPath) || PathCouldContainSelection(resolvedPath, pendingPath))
				{
					hasDescendantMatch = true;
				}
			}
		}
		public static bool RequiresLazyLoad(DriveTreeItem item)
		{
			ArgumentNullException.ThrowIfNull(item);

			return (item.ItemType == DriveTreeItemType.Volume ||
					item.ItemType == DriveTreeItemType.HyperVVolume ||
					item.ItemType == DriveTreeItemType.NetworkDrive ||
					item.ItemType == DriveTreeItemType.NetworkShare ||
					(item.ItemType == DriveTreeItemType.Folder && !string.IsNullOrWhiteSpace(item.ResolvedPath)) ||
					item.ItemType == DriveTreeItemType.HyperVVirtualDisk) &&
				   !item.ChildrenLoaded;
		}
		/// <summary>
		/// Selects the HyperVSystem tree node whose normalized VirtualMachineName matches the saved name.
		/// Returns true if a match was found and selected.
		/// </summary>
		public static bool PreSelectHyperVSystemByName(string savedVmName, List<DriveTreeItem> driveItems)
		{
			string normalizedSavedVmName = HyperVBackupTreeHelper.NormalizeSavedHyperVSystemName(savedVmName);
			if (string.IsNullOrWhiteSpace(normalizedSavedVmName))
				return false;

			DriveTreeItem? match = driveItems.FirstOrDefault(item =>
				item.ItemType == DriveTreeItemType.HyperVSystem &&
				(string.Equals(HyperVBackupTreeHelper.NormalizeSavedHyperVSystemName(item.VirtualMachineName), normalizedSavedVmName, StringComparison.OrdinalIgnoreCase) ||
				 string.Equals(HyperVBackupTreeHelper.NormalizeSavedHyperVSystemName(item.FullPath), normalizedSavedVmName, StringComparison.OrdinalIgnoreCase)));

			if (match == null)
				return false;

			match.IsChecked = true;
			return true;
		}

		public static async Task PreSelectHyperVGuestItemAsync(HyperVGuestSelectionInfo selection, List<DriveTreeItem> driveItems, HashSet<string> _pendingSelectionPaths)
		{
			DriveTreeItem? hyperVSystem = driveItems.FirstOrDefault(item =>
				item.ItemType == DriveTreeItemType.HyperVSystem &&
				string.Equals(item.VirtualMachineName, selection.VirtualMachineName, StringComparison.OrdinalIgnoreCase));
			if (hyperVSystem == null)
			{
				return;
			}

			DriveTreeItem? virtualDiskItem = hyperVSystem.Children.FirstOrDefault(item =>
				item.ItemType == DriveTreeItemType.HyperVVirtualDisk &&
				string.Equals(item.VirtualDiskPath, selection.VirtualDiskPath, StringComparison.OrdinalIgnoreCase));
			if (virtualDiskItem == null)
			{
				return;
			}

			if (selection.Kind == HyperVGuestSelectionKind.VirtualDisk)
			{
				virtualDiskItem.IsChecked = true;
				return;
			}

			if (!virtualDiskItem.ChildrenLoaded)
			{
				await HyperVHelpers.LoadHyperVVirtualDiskChildrenAsync(virtualDiskItem, selection.RelativePath);
				virtualDiskItem.ChildrenLoaded = true;
			}

			DriveTreeItem? volumeItem = virtualDiskItem.Children.FirstOrDefault(item =>
				item.ItemType == DriveTreeItemType.HyperVVolume &&
				item.PartitionNumber == selection.PartitionNumber);
			if (volumeItem == null)
			{
				return;
			}

			if (selection.Kind == HyperVGuestSelectionKind.Volume || string.IsNullOrWhiteSpace(selection.RelativePath))
			{
				volumeItem.IsChecked = true;
				return;
			}

			DriveTreeItem current = volumeItem;
			foreach (string segment in selection.RelativePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (!current.ChildrenLoaded)
				{
					List<DriveTreeItem> childItems = await Task.Run(() => VolumeConfigHelpers.BuildVolumeChildItems(current));
					ReplaceChildren(current, childItems);
					current.ChildrenLoaded = true;
				}

				DriveTreeItem? next = current.Children.FirstOrDefault(item =>
					item.ItemType == DriveTreeItemType.Folder &&
					item.ResolvedPath.EndsWith(segment, StringComparison.OrdinalIgnoreCase));
				if (next == null)
				{
					return;
				}

				current = next;
			}

			current.IsChecked = true;
		}

		public static void RefreshLoadedSelectionStates(List<DriveTreeItem> driveItems)
		{
			foreach (DriveTreeItem rootItem in driveItems)
			{
				RefreshLoadedSelectionStateRecursive(rootItem);
			}
		}

		public static void RefreshLoadedSelectionStateRecursive(DriveTreeItem item)
		{
			foreach (DriveTreeItem child in item.Children)
			{
				RefreshLoadedSelectionStateRecursive(child);
			}

			item.RefreshCheckStateFromChildren();
		}


		public static void ReplaceChildren(DriveTreeItem parentItem, IEnumerable<DriveTreeItem> childItems)
		{
			ArgumentNullException.ThrowIfNull(parentItem);
			ArgumentNullException.ThrowIfNull(childItems);

			parentItem.Children.Clear();
			foreach (DriveTreeItem childItem in childItems)
			{
				parentItem.Children.Add(childItem);
			}

			if (parentItem.IsChecked == true)
			{
				foreach (DriveTreeItem childItem in parentItem.Children.Where(child => child.ParticipatesInCheckState))
				{
					childItem.IsChecked = true;
				}

				parentItem.RefreshCheckStateFromChildren();
			}
		}

		public static string NormalizeSelectionPath(string? path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return string.Empty;
			}

			return path.Trim().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}


		public static bool PathCouldContainSelection(string containerPath, string targetPath)
		{
			if (string.IsNullOrWhiteSpace(containerPath) || string.IsNullOrWhiteSpace(targetPath))
			{
				return false;
			}

			return string.Equals(containerPath, targetPath, StringComparison.OrdinalIgnoreCase) ||
				   targetPath.StartsWith(containerPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
				   targetPath.StartsWith(containerPath + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
		}
	}
}
