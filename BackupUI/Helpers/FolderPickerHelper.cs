// FolderPickerHelper.cs - Folder and file selection helpers for backup/restore operations
using Microsoft.Win32;

using SecureServerBackup.Models;
using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SecureServerBackup.Helpers
{
	public static class FolderPickerHelper
	{
		/// <summary>
		/// Opens a folder browser dialog to select a folder
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="initialDirectory">Starting directory (optional)</param>
		/// <returns>Selected folder path or null if cancelled</returns>
		public static string? PickFolder(string title = "Select Folder", string? initialDirectory = null)
		{
			using var dialog = new System.Windows.Forms.FolderBrowserDialog 
			{ 
				Description = title,
				ShowNewFolderButton = true,
				UseDescriptionForTitle = true
			};

			// Set initial directory if provided and valid
			if (!string.IsNullOrWhiteSpace(initialDirectory) && Directory.Exists(initialDirectory))
			{
				dialog.SelectedPath = initialDirectory;
			}

			return dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ? dialog.SelectedPath : null;
		}

		/// <summary>
		/// Opens a file picker dialog to select a file
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="filter">File filter (e.g., "Backup Files|*.brs;*.ssb|All Files|*.*")</param>
		/// <param name="initialDirectory">Starting directory (optional)</param>
		/// <returns>Selected file path or null if cancelled</returns>
		public static string? PickFile(string title, string filter, string? initialDirectory = null)
		{
			var dialog = new OpenFileDialog 
			{ 
				Title = title, 
				Filter = filter,
				CheckFileExists = true,
				CheckPathExists = true
			};

			// Set initial directory if provided and valid
			if (!string.IsNullOrWhiteSpace(initialDirectory) && Directory.Exists(initialDirectory))
			{
				dialog.InitialDirectory = initialDirectory;
			}

			return dialog.ShowDialog() == true ? dialog.FileName : null;
		}

		/// <summary>
		/// Opens a folder picker specifically for selecting backup destination
		/// </summary>
		/// <param name="suggestedName">Suggested folder name for the backup</param>
		/// <returns>Selected backup location or null if cancelled</returns>
		public static string? PickBackupLocation(string suggestedName)
		{
			// Try to determine a good initial directory
			string? initialDir = null;

			// Check common backup locations in order of preference
			string[] commonBackupPaths = {
				@"D:\Backups",
				@"E:\Backups",
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Backups"),
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			};

			foreach (var path in commonBackupPaths)
			{
				if (Directory.Exists(path))
				{
					initialDir = path;
					break;
				}
			}

			var selectedPath = PickFolder($"Select Backup Location for '{suggestedName}'", initialDir);

			// If user selected a path and suggestedName is provided, suggest creating a subfolder
			if (!string.IsNullOrWhiteSpace(selectedPath) && !string.IsNullOrWhiteSpace(suggestedName))
			{
				// Return the path with suggested name as subfolder
				return Path.Combine(selectedPath, suggestedName);
			}

			return selectedPath;
		}

		/// <summary>
		/// Opens a folder picker for selecting a backup to restore
		/// </summary>
		/// <returns>Selected backup path or null if cancelled</returns>
		public static string? PickBackupToRestore()
		{
			// Try to find common backup locations
			string? initialDir = null;

			string[] commonBackupPaths = {
				@"D:\Backups",
				@"E:\Backups",
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Backups")
			};

			foreach (var path in commonBackupPaths)
			{
				if (Directory.Exists(path))
				{
					initialDir = path;
					break;
				}
			}

			return PickFolder("Select Backup to Restore", initialDir);
		}

		public static List<DriveTreeItem> GetEffectiveSelectedFilesAndFoldersItems(IEnumerable<DriveTreeItem> driveItems)
		{
			List<DriveTreeItem> selectedItems = new();

			foreach (DriveTreeItem drive in driveItems)
			{
				CollectEffectiveSelectedFilesAndFoldersItems(drive, selectedItems);
			}

			return selectedItems;
		}

		public static List<string> GetSelectedFilesSourceRoots(IEnumerable<DriveTreeItem> driveItems)
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

		public static bool IsSelectedFilesAndFoldersSelectionAllowed(IEnumerable<DriveTreeItem> selectedItems)
		{
			ArgumentNullException.ThrowIfNull(selectedItems);

			bool hasAllowedSelection = false;
			foreach (DriveTreeItem item in selectedItems)
			{
				if (!IsSelectedFilesAndFoldersAllowedItem(item))
				{
					return false;
				}

				hasAllowedSelection = true;
			}

			return hasAllowedSelection;
		}

	}
}