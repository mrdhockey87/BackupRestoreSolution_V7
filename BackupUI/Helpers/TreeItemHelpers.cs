using SecureServerBackup.Models;

using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SecureServerBackup.Helpers
{
	public static class TreeItemHelpers
	{
		public static TreeViewItem CreateLoadingTreeViewItem(string message)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(message);

			return new TreeViewItem
			{
				Header = new StackPanel
				{
					Orientation = System.Windows.Controls.Orientation.Horizontal,
					Children =
					{
						new System.Windows.Controls.ProgressBar
						{
							IsIndeterminate = true,
							Width = 16,
							Height = 16,
							Margin = new Thickness(0, 0, 6, 0),
							VerticalAlignment = VerticalAlignment.Center
						},
						new TextBlock
						{
							Text = message,
							VerticalAlignment = VerticalAlignment.Center
						}
					}
				},
				IsEnabled = false
			};
		}

		public static DriveTreeItem CreateDirectoryTreeItem(DriveTreeItem parentItem, DirectoryInfo dirInfo)
		{
			string folderName = AppendAttributesSuffix(dirInfo.Name, dirInfo.Attributes);

			DriveTreeItem folderItem = new()
			{
				Name = folderName,
				FullPath = BuildChildFullPath(parentItem, dirInfo.FullName, HyperVGuestSelectionKind.Folder),
				ResolvedPath = dirInfo.FullName,
				VirtualMachineName = parentItem.VirtualMachineName,
				VirtualDiskPath = parentItem.VirtualDiskPath,
				PartitionNumber = parentItem.PartitionNumber,
				ItemType = DriveTreeItemType.Folder,
				Parent = parentItem
			};

			try
			{
				if (Directory.GetDirectories(dirInfo.FullName).Length > 0 || Directory.GetFiles(dirInfo.FullName).Length > 0)
				{
					folderItem.Children.Add(new DriveTreeItem
					{
						Name = "Loading...",
						ItemType = DriveTreeItemType.Folder,
						Parent = folderItem
					});
				}
			}
			catch
			{
			}

			return folderItem;
		}

		public static DriveTreeItem CreateAccessDeniedDirectoryTreeItem(DriveTreeItem parentItem, string directoryPath)
		{
			return new DriveTreeItem
			{
				Name = $"{Path.GetFileName(directoryPath)} [Access Denied]",
				FullPath = BuildChildFullPath(parentItem, directoryPath, HyperVGuestSelectionKind.Folder),
				ResolvedPath = directoryPath,
				VirtualMachineName = parentItem.VirtualMachineName,
				VirtualDiskPath = parentItem.VirtualDiskPath,
				PartitionNumber = parentItem.PartitionNumber,
				ItemType = DriveTreeItemType.Folder,
				Parent = parentItem
			};
		}

		public static DriveTreeItem CreateFileTreeItem(DriveTreeItem parentItem, FileInfo fileInfo)
		{
			return new DriveTreeItem
			{
				Name = AppendAttributesSuffix(fileInfo.Name, fileInfo.Attributes),
				FullPath = BuildChildFullPath(parentItem, fileInfo.FullName, HyperVGuestSelectionKind.File),
				ResolvedPath = fileInfo.FullName,
				VirtualMachineName = parentItem.VirtualMachineName,
				VirtualDiskPath = parentItem.VirtualDiskPath,
				PartitionNumber = parentItem.PartitionNumber,
				ItemType = DriveTreeItemType.File,
				Size = fileInfo.Length,
				Parent = parentItem
			};
		}

		public static string AppendAttributesSuffix(string name, FileAttributes attributes)
		{
			if ((attributes & FileAttributes.System) == FileAttributes.System)
			{
				return name + " [System]";
			}

			if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
			{
				return name + " [Hidden]";
			}

			return name;
		}
		public static string BuildChildFullPath(DriveTreeItem parentItem, string resolvedPath, HyperVGuestSelectionKind selectionKind)
		{
			if (!HyperVGuestSelectionPath.IsEncodedPath(parentItem.FullPath))
			{
				return resolvedPath;
			}

			return HyperVGuestSelectionPath.Encode(
				selectionKind,
				parentItem.VirtualMachineName,
				parentItem.VirtualDiskPath,
				parentItem.PartitionNumber,
				HyperVHelpers.BuildHyperVGuestRelativePath(parentItem, resolvedPath));
		}
	}
}
