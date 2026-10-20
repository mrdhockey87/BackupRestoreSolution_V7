using SecureServerBackup.Models;
using SecureServerBackup.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureServerBackup.Helpers
{
	public static class NetworkPathHelpers
	{
		public static readonly string SavedNetworkPathsFilePath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"BackupRestoreApp",
				"saved-network-paths.json");
		public static async Task LoadNetworkDrives(List<DriveTreeItem> driveItems)
		{
			DriveTreeItem? networkRoot = await Task.Run(() =>
			{
				try
				{
					var discoveredNetworkRoot = new DriveTreeItem
					{
						Name = "Network Locations",
						FullPath = "",
						ItemType = DriveTreeItemType.NetworkRoot
					};

					// Enumerate mapped network drives
					foreach (var drive in DriveInfo.GetDrives())
					{
						try
						{
							if (drive.DriveType == DriveType.Network && drive.IsReady)
							{
								var driveName = drive.Name.TrimEnd('\\');
								var volumeLabel = string.IsNullOrEmpty(drive.VolumeLabel)
									? "Network Drive"
									: drive.VolumeLabel;

								var networkDrive = new DriveTreeItem
								{
									Name = $"{driveName} ({volumeLabel}) - Mapped",
									FullPath = drive.Name,
									ItemType = DriveTreeItemType.NetworkDrive,
									Size = drive.TotalSize,
									Parent = discoveredNetworkRoot
								};

								// Add placeholder for folders
								networkDrive.Children.Add(new DriveTreeItem
								{
									Name = "Loading...",
									ItemType = DriveTreeItemType.Folder,
									Parent = networkDrive
								});

								discoveredNetworkRoot.Children.Add(networkDrive);
							}
						}
						catch
						{
							// Skip drives that can't be accessed
						}
					}

					// Add "Add Network Path..." option
					var addNetworkPath = new DriveTreeItem
					{
						Name = "?? Add Network Path...",
						FullPath = "",
						ItemType = DriveTreeItemType.NetworkBrowser,
						Parent = discoveredNetworkRoot
					};

					discoveredNetworkRoot.Children.Add(addNetworkPath);

					// Only add Network Locations if there are mapped drives or the add option
					return discoveredNetworkRoot.Children.Count > 0
						? discoveredNetworkRoot
						: null;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Error loading network drives: {ex.Message}");
				}

				return null;
			});

			if (networkRoot != null)
			{
				foreach (string savedPath in LoadSavedNetworkPaths())
				{
					TryInsertNetworkPath(networkRoot, savedPath, saveAfterInsert: false);
				}

				driveItems.Add(networkRoot);
			}
		}

		/// <summary>
		/// Adds a UNC network path to the tree
		/// </summary>
		public static void AddNetworkPathToTree(string uncPath, List<DriveTreeItem> driveItems, Form anyForm, TreeView treeView)
		{
			try
			{
				// Find the Network Locations root
				var networkRoot = driveItems.FirstOrDefault(d => d.ItemType == DriveTreeItemType.NetworkRoot);

				if (networkRoot == null)
				{
					CustomDialogService.ShowError("Network Locations node not found.", "Error");
					return;
				}

				if (!TryInsertNetworkPath(networkRoot, uncPath, saveAfterInsert: true))
				{
					CustomDialogService.ShowInfo($"Network path already added:\n{uncPath}", "Duplicate Path");
					return;
				}

				// Refresh the tree view Might need to redo this method or move to form
				RefreshTreeView(anyForm, treeView);

				CustomDialogService.ShowSuccess($"Network path added successfully:\n{uncPath}", "Success");
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError($"Error adding network path:\n{ex.Message}", "Error");
			}
		}
		/// <summary>
		/// Refreshes the entire tree view
		/// </summary>
		private static void RefreshTreeView(Form anyForm, TreeView treeViewDrives)
		{
			//not sure This can be done with out dierctly calling the forms refresh method mdail 10-12-2026
			//might need to move this to the form and call it from there
			//treeViewDrives.Da = null;
			//treeViewDrives.ItemsSource = driveItems;
		}
		public static List<string> LoadSavedNetworkPaths()
		{
			try
			{
				if (!File.Exists(SavedNetworkPathsFilePath))
				{
					return new List<string>();
				}

				string json = File.ReadAllText(SavedNetworkPathsFilePath);
				return JsonSerializer.Deserialize<List<string>>(json)
					?.Where(path => !string.IsNullOrWhiteSpace(path))
					.Select(NormalizeNetworkPath)
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList() ?? new List<string>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error loading saved network paths: {ex.Message}");
				return new List<string>();
			}
		}

		public static void SaveNetworkPaths(IEnumerable<string> networkPaths)
		{
			ArgumentNullException.ThrowIfNull(networkPaths);

			try
			{
				string? directory = Path.GetDirectoryName(SavedNetworkPathsFilePath);
				if (!string.IsNullOrWhiteSpace(directory))
				{
					Directory.CreateDirectory(directory);
				}

				List<string> normalizedPaths = networkPaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Select(NormalizeNetworkPath)
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();

				string json = JsonSerializer.Serialize(normalizedPaths, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(SavedNetworkPathsFilePath, json);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error saving network paths: {ex.Message}");
			}
		}

		public static string NormalizeNetworkPath(string path)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(path);

			return path.Trim().TrimEnd('\\') + "\\";
		}

		public static bool TryInsertNetworkPath(DriveTreeItem networkRoot, string uncPath, bool saveAfterInsert)
		{
			ArgumentNullException.ThrowIfNull(networkRoot);
			ArgumentException.ThrowIfNullOrWhiteSpace(uncPath);

			string normalizedPath = NormalizeNetworkPath(uncPath);
			DriveTreeItem? existing = networkRoot.Children
				.FirstOrDefault(c => c.ItemType == DriveTreeItemType.NetworkShare &&
									 c.FullPath.Equals(normalizedPath, StringComparison.OrdinalIgnoreCase));

			if (existing != null)
			{
				return false;
			}

			string[] pathParts = normalizedPath.TrimEnd('\\').Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
			string displayName = pathParts.Length >= 2
				? $"\\\\{pathParts[0]}\\{pathParts[1]}"
				: normalizedPath;

			DriveTreeItem networkShare = new()
			{
				Name = $"{displayName} - Network Share",
				FullPath = normalizedPath,
				ItemType = DriveTreeItemType.NetworkShare,
				IsRemovableNetworkPath = true,
				Parent = networkRoot
			};

			networkShare.Children.Add(new DriveTreeItem
			{
				Name = "Loading...",
				ItemType = DriveTreeItemType.Folder,
				Parent = networkShare
			});

			DriveTreeItem? addOption = networkRoot.Children.FirstOrDefault(c => c.ItemType == DriveTreeItemType.NetworkBrowser);
			if (addOption != null)
			{
				int index = networkRoot.Children.IndexOf(addOption);
				networkRoot.Children.Insert(index, networkShare);
			}
			else
			{
				networkRoot.Children.Add(networkShare);
			}

			if (saveAfterInsert)
			{
				PersistCurrentNetworkPaths(networkRoot);
			}

			return true;
		}

		public static void PersistCurrentNetworkPaths(DriveTreeItem networkRoot)
		{
			ArgumentNullException.ThrowIfNull(networkRoot);

			SaveNetworkPaths(networkRoot.Children
				.Where(child => child.ItemType == DriveTreeItemType.NetworkShare && child.IsRemovableNetworkPath)
				.Select(child => child.FullPath));
		}
	}
}
