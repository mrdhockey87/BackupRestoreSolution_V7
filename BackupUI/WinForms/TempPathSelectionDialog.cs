using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
namespace SecureServerBackup.WinForms
{
	public partial class TempPathSelectionDialog : Form
	{
		private static readonly string SavedTempPathSettingsPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"BackupRestoreApp",
			"mount-temp-path.json");
		public string SelectedTempPath { get; private set; }
		public TempPathSelectionDialog()
		{
			InitializeComponent();
			// Set initial temp path from saved settings when still valid, otherwise default
			SelectedTempPath = GetInitialTempPath();
			txtTempPath.Text = SelectedTempPath;
			// Update space info
			UpdateSpaceInfo();
		}
		private static string GetInitialTempPath()
		{
			string defaultTempPath = EnsureTrailingBackslash(Path.GetTempPath());
			try
			{
				if (!File.Exists(SavedTempPathSettingsPath))
				{
					return defaultTempPath;
				}
				var json = File.ReadAllText(SavedTempPathSettingsPath);
				var settings = JsonSerializer.Deserialize<TempPathSettings>(json);
				if (settings != null && IsSavedPathStillValid(settings.LastTempPath))
				{
					return EnsureTrailingBackslash(Path.GetFullPath(settings.LastTempPath!));
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Failed to load saved temp path: {ex.Message}");
			}
			return defaultTempPath;
		}
		private static bool IsSavedPathStillValid(string? path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			try
			{
				string fullPath = Path.GetFullPath(path);
				return Directory.Exists(fullPath);
			}
			catch
			{
				return false;
			}
		}
		private static string EnsureTrailingBackslash(string path)
		{
			return path.EndsWith("\\", StringComparison.Ordinal) ? path : path + "\\";
		}
		private void SaveSelectedTempPath()
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(SavedTempPathSettingsPath)!);
				var settings = new TempPathSettings
				{
					LastTempPath = SelectedTempPath
				};
				var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(SavedTempPathSettingsPath, json);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Failed to save temp path: {ex.Message}");
			}
		}
		private void UpdateSpaceInfo()
		{
			try
			{
				if (string.IsNullOrEmpty(SelectedTempPath))
				{
					txtSpaceInfo.Text = "No path selected";
					return;
				}
				// Get drive info
				string? root = Path.GetPathRoot(SelectedTempPath);
				if (string.IsNullOrEmpty(root))
				{
					txtSpaceInfo.Text = "Cannot determine drive";
					return;
				}
				DriveInfo drive = new DriveInfo(root);
				if (drive.IsReady)
				{
					long freeGB = drive.AvailableFreeSpace / (1024 * 1024 * 1024);
					long totalGB = drive.TotalSize / (1024 * 1024 * 1024);
					txtSpaceInfo.Text = $"Drive {drive.Name} - Free: {freeGB:N0} GB / Total: {totalGB:N0} GB";
					// Warning if less than 10GB free
					if (freeGB < 10)
					{
						txtSpaceInfo.ForeColor = System.Drawing.Color.DarkOrange;
						txtSpaceInfo.Text += " ⚠️ Low disk space! Consider using a different drive.";
					}
					else
					{
						txtSpaceInfo.ForeColor = System.Drawing.Color.Gray;
					}
				}
				else
				{
					txtSpaceInfo.Text = $"Drive {drive.Name} is not ready";
				}
			}
			catch (Exception ex)
			{
				txtSpaceInfo.Text = $"Error checking space: {ex.Message}";
			}
		}
		private void Browse_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.Description = "Select temporary directory for backup archive mount operations";
				dialog.SelectedPath = SelectedTempPath;
				dialog.ShowNewFolderButton = true;
				var result = dialog.ShowDialog(this);
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Browse dialog result: {result}");
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Selected path from dialog: {dialog.SelectedPath}");
				if (result == DialogResult.OK)
				{
					SelectedTempPath = EnsureTrailingBackslash(dialog.SelectedPath);
					txtTempPath.Text = SelectedTempPath;
					System.Diagnostics.Debug.WriteLine($"[TempPathDialog] SelectedTempPath set to: {SelectedTempPath}");
					System.Diagnostics.Debug.WriteLine($"[TempPathDialog] txtTempPath.Text set to: {txtTempPath.Text}");
					UpdateSpaceInfo();
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("[TempPathDialog] User cancelled browse dialog");
				}
			}
		}
		private void OK_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"[TempPathDialog] OK clicked - SelectedTempPath: {SelectedTempPath}");
			// Validate path
			if (string.IsNullOrEmpty(SelectedTempPath))
			{
				MessageBox.Show(this, "Please select a temporary path.",
					"No Path Selected",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return;
			}
			// Check if directory exists or can be created
			try
			{
				if (!Directory.Exists(SelectedTempPath))
				{
					var result = MessageBox.Show(
						this,
						$"Directory does not exist:\n{SelectedTempPath}\n\nCreate it now?",
						"Create Directory",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question);
					if (result == DialogResult.Yes)
					{
						Directory.CreateDirectory(SelectedTempPath);
						System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Created directory: {SelectedTempPath}");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("[TempPathDialog] User declined directory creation");
						return;
					}
				}
				// Test write access
				string testFile = Path.Combine(SelectedTempPath, $"_archive_test_{Guid.NewGuid()}.tmp");
				File.WriteAllText(testFile, "test");
				File.Delete(testFile);
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Write test successful for: {SelectedTempPath}");
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Returning DialogResult=OK with path: {SelectedTempPath}");
				SaveSelectedTempPath();
				this.DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[TempPathDialog] Error during validation: {ex.Message}");
				MessageBox.Show(
					this,
					$"Cannot use selected path:\n{ex.Message}\n\nPlease select a different location.",
					"Invalid Path",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}
		private void Cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}
		private sealed class TempPathSettings
		{
			public string? LastTempPath { get; set; }
		}
	}
}