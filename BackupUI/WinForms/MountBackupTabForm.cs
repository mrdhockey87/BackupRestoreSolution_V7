using SecureServerBackup.Enums;
using SecureServerBackup.Helpers;
using SecureServerBackup.Models;
using SecureServerBackup.Services;

using SecureServerBackupCommon;

using SecureServerBackupService;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	public partial class MountBackupTabForm : Form
	{
		public MountBackupTabForm()
		{
			InitializeComponent();
			ConfigureAvailableBackupsGrid();
			ConfigureMountedBackupsGrid();
			LoadAvailableBackups();
			LoadMountedBackups();
		}

		internal void ConfigureForEmbeddedHost()
		{
			ControlBox = false;
			ShowIcon = false;
			ShowInTaskbar = false;
		}

		private void ConfigureAvailableBackupsGrid()
		{
			dgAvailableBackups.AutoGenerateColumns = false;
			dgAvailableBackups.CellContentClick += DgAvailableBackups_CellContentClick;
			dgAvailableBackups.SelectionChanged += DgAvailableBackups_SelectionChanged;

			BackupName.DataPropertyName = nameof(AvailableBackupInfo.BackupName);
			BackupType.DataPropertyName = nameof(AvailableBackupInfo.BackupType);
			IsEncrypted.DataPropertyName = nameof(AvailableBackupInfo.IsEncrypted);
			BackupDate.DataPropertyName = nameof(AvailableBackupInfo.BackupDate);
			BackupPath.DataPropertyName = nameof(AvailableBackupInfo.BackupPath);

			Mount.DataPropertyName = string.Empty;
			Mount.UseColumnTextForButtonValue = true;
		}

		private void ConfigureMountedBackupsGrid()
		{
			dgMountedBackups.AutoGenerateColumns = false;
			dgMountedBackups.CellContentClick += DgMountedBackups_CellContentClick;

			MountedBackupName.DataPropertyName = nameof(NativeBackupMountManager.MountedBackup.BackupName);
			MountedBackupType.DataPropertyName = nameof(NativeBackupMountManager.MountedBackup.BackupType);
			MountedIsEncrypted.DataPropertyName = nameof(NativeBackupMountManager.MountedBackup.IsReadOnly);
			MountedBackupDate.DataPropertyName = nameof(NativeBackupMountManager.MountedBackup.MountTime);
			MountedBackupPath.DataPropertyName = nameof(NativeBackupMountManager.MountedBackup.MountPath);

			UnmountBtn.DataPropertyName = string.Empty;
			UnmountBtn.UseColumnTextForButtonValue = true;
		}

		private async void MountBackup_Click(object sender, RoutedEventArgs e)
		{
			if (sender is System.Windows.Controls.Button btn && btn.Tag is AvailableBackupInfo backup)
			{
				await MountBackupAsync(backup);
			}
		}

		private async Task MountBackupAsync(AvailableBackupInfo backup)
		{
			try
			{
				// Get selected backup point if Inc/Diff
				string ssbPath = GetBackupPointPath(backup);

				if (string.IsNullOrEmpty(ssbPath))
				{
					CustomDialogService.ShowWarning(this, "Please select a backup point to mount.",
									  "No Backup Point Selected");
					return;
				}

				System.Diagnostics.Debug.WriteLine($"[Mount] Checking image count for: {ssbPath}");
				var (countSuccess, imageCount, countError) = NativeBackupMountManager.GetImageCount(ssbPath);

				int selectedImageIndex = 1;

				if (!countSuccess)
				{
					CustomDialogService.ShowError(this, $"Failed to check backup images:\n{countError}",
								  "Error");
					return;
				}

				if (imageCount > 1)
				{
					System.Diagnostics.Debug.WriteLine($"[Mount] Backup has {imageCount} images - showing selection dialog");

					var (infoSuccess, images, infoError) = NativeBackupMountManager.GetImageInfo(ssbPath);

					if (!infoSuccess || images.Count == 0)
					{
						CustomDialogService.ShowError(this, $"Failed to get image details:\n{infoError}",
										  "Error");
						return;
					}

					var imageDialog = new SecureServerBackup.WinForms.ImageSelectionDialog(images)
					{
						Owner = this
					};

					if (imageDialog.Visible != true)
					{
						System.Diagnostics.Debug.WriteLine("[Mount] User cancelled image selection");
						return;
					}

					selectedImageIndex = imageDialog.SelectedImageIndex;
					System.Diagnostics.Debug.WriteLine($"[Mount] User selected image index: {selectedImageIndex}");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"[Mount] Backup has {imageCount} image(s) - using first image");
				}

				var tempPathDialog = new SecureServerBackup.WinForms.TempPathSelectionDialog
				{
					Owner = this
				};

				System.Diagnostics.Debug.WriteLine("[Mount] Showing TempPathSelectionDialog...");

				if (tempPathDialog.Visible != true)
				{
					System.Diagnostics.Debug.WriteLine("[Mount] User cancelled temp path selection");
					return;
				}

				string selectedTempPath = tempPathDialog.SelectedTempPath;

				System.Diagnostics.Debug.WriteLine($"[Mount] User selected temp path: '{selectedTempPath}'");
				System.Diagnostics.Debug.WriteLine($"[Mount] Path length: {selectedTempPath?.Length ?? 0}");
				System.Diagnostics.Debug.WriteLine($"[Mount] Path is null or empty: {string.IsNullOrEmpty(selectedTempPath)}");
				System.Diagnostics.Debug.WriteLine("[Mount] About to create progress window...");

				var progressWindow = new SecureServerBackup.WinForms.MountProgressForm
				{
					Owner = this
				};

				System.Diagnostics.Debug.WriteLine($"[Mount] Progress window created, setting backup name: {backup.BackupName}");
				progressWindow.SetBackupName(backup.BackupName);
				System.Diagnostics.Debug.WriteLine("[Mount] Showing progress window...");
				progressWindow.Show();

				try
				{
					System.Diagnostics.Debug.WriteLine("[Mount] Calling NativeBackupMountManager.MountBackupAsync...");
					System.Diagnostics.Debug.WriteLine($"[Mount] Parameters: ssbPath={ssbPath}, backupName={backup.BackupName}, backupType={backup.BackupType}, imageIndex={selectedImageIndex}, tempPath={selectedTempPath}");

					using var preparedBackup = EncryptedBackupFileService.PrepareForRead(
						this,
						ssbPath,
						backup.BackupName,
						backup.ProtectedEncryptionPassword);

					var (success, mountPath, error) = await NativeBackupMountManager.MountBackupAsync(
						preparedBackup.WorkingPath,
						backup.BackupName,
						backup.BackupType,
						selectedImageIndex,
						(percentage, message) =>
						{
							progressWindow.SetStatus(message, percentage);
						},
						selectedTempPath);

					progressWindow.CloseProgress();

					if (success)
					{
						CustomDialogService.ShowSuccess(this, $"Backup mounted successfully!\n\n" +
									  $"Mount Path: {mountPath}\n\n" +
									  $"You can now browse the backup in Windows Explorer.\n" +
									  $"Backup is READ-ONLY to prevent modifications.",
									  "Backup Mounted");

						LoadMountedBackups();
						OpenExplorer(mountPath);
					}
					else
					{
						CustomDialogService.ShowError(this, $"Failed to mount backup:\n{error}",
									  "Mount Error");
					}
				}
				catch (Exception ex)
				{
					progressWindow.CloseProgress();
					CustomDialogService.ShowError(this, $"Error mounting backup:\n{ex.Message}",
								  "Error");
				}
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error initializing mount:\n{ex.Message}",
								  "Error");
			}
		}

		private async void UnmountBackup_Click(object sender, RoutedEventArgs e)
		{
			if (sender is System.Windows.Controls.Button btn && btn.Tag is string mountPath)
			{
				await UnmountBackupAsync(mountPath);
			}
		}

		private async Task UnmountBackupAsync(string mountPath)
		{
			var result = CustomDialogService.ShowQuestion(this,
				$"Unmount backup from {mountPath}?\n\nIMPORTANT: Please close all Windows Explorer windows that are browsing mounted files before proceeding.",
				"Unmount Backup");

			if (result != CustomDialogResult.Yes)
			{
				return;
			}

			var progressWindow = new SecureServerBackup.WinForms.MountProgressForm
			{
				Owner = this,
				Text = "Unmounting Backup"
			};

			progressWindow.SetBackupName("Unmounting...");
			progressWindow.Show();

			try
			{
				var (success, error) = await NativeBackupMountManager.UnmountBackupAsync(
					mountPath,
					(percentage, message) =>
					{
						progressWindow.SetStatus(message, percentage);
					});

				progressWindow.CloseProgress();

				if (success)
				{
					LoadMountedBackups();
					CustomDialogService.ShowSuccess($"Backup unmounted successfully from {mountPath}",
									  "Success");
				}
				else
				{
					CustomDialogService.ShowError($"Failed to unmount:\n{error}",
									  "Unmount Error");
				}
			}
			catch (Exception ex)
			{
				progressWindow.CloseProgress();
				CustomDialogService.ShowError($"Error unmounting backup:\n{ex.Message}",
								  "Error");
			}
		}

		private async void DgAvailableBackups_CellContentClick(object? sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex != Mount.Index)
			{
				return;
			}

			if (dgAvailableBackups.Rows[e.RowIndex].DataBoundItem is AvailableBackupInfo backup)
			{
				await MountBackupAsync(backup);
			}
		}

		private async void DgMountedBackups_CellContentClick(object? sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex != UnmountBtn.Index)
			{
				return;
			}

			if (dgMountedBackups.Rows[e.RowIndex].DataBoundItem is NativeBackupMountManager.MountedBackup mountedBackup)
			{
				await UnmountBackupAsync(mountedBackup.MountPath);
			}
		}

		private void DgAvailableBackups_SelectionChanged(object? sender, EventArgs e)
		{
			UpdateAvailableBackupSelection();
		}

		private void UnmountAll_Click(object sender, RoutedEventArgs e)
		{
			var mounted = NativeBackupMountManager.GetMountedBackups();

			if (mounted.Count == 0)
			{
				CustomDialogService.ShowInfo(this, "No mounted backups to unmount.",
							  "No Mounted Backups");
				return;
			}

			var result = CustomDialogService.ShowQuestion(this,
				$"Unmount all {mounted.Count} mounted backup(s)?\n\nIMPORTANT: Please close all Windows Explorer windows that are browsing mounted files before proceeding.",
				"Unmount All");

			if (result == CustomDialogResult.Yes)
			{
				NativeBackupMountManager.UnmountAll();
				LoadMountedBackups();
				CustomDialogService.ShowSuccess(this, "All backups unmounted successfully.",
							  "Success");
			}
		}

		private void AvailableBackups_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateAvailableBackupSelection();
		}

		private void UpdateAvailableBackupSelection()
		{
			if (dgAvailableBackups == null || pnlBackupPoints == null)
			{
				return;
			}

			if (dgAvailableBackups.CurrentRow?.DataBoundItem is AvailableBackupInfo backup)
			{
				if (backup.BackupType == "Incremental" || backup.BackupType == "Differential")
				{
					LoadBackupPoints(backup);
					pnlBackupPoints.Visible = true;
				}
				else
				{
					pnlBackupPoints.Visible = false;
				}

				return;
			}

			pnlBackupPoints.Visible = false;
		}

		private void LoadAvailableBackups()
		{
			if (dgAvailableBackups == null)
				return;

			var backups = new System.Collections.Generic.List<AvailableBackupInfo>();

			try
			{
				var jobManager = new SecureServerBackup.Services.JobManager();
				// Scan backup directories for .ssb (WIM) files
				var jobs = jobManager.GetAllJobs();

				foreach (var job in jobs)
				{
					string destPath = job.DestinationPath;

					if (System.IO.Directory.Exists(destPath))
					{
						// Find .ssb (WIM backup) files
						var ssbFiles = System.IO.Directory.GetFiles(destPath, "*.ssb", System.IO.SearchOption.AllDirectories);

						foreach (var ssb in ssbFiles)
						{
							var fileInfo = new System.IO.FileInfo(ssb);

							backups.Add(new AvailableBackupInfo
							{
								BackupName = job.Name,
								BackupType = job.Type.ToString(),
								BackupDate = fileInfo.LastWriteTime,
								BackupPath = ssb,
								IsEncrypted = BackupEncryptionService.IsEncryptedBackupFile(ssb),
								ProtectedEncryptionPassword = job.ProtectedEncryptionPassword
							});
						}
					}
				}

				dgAvailableBackups.DataSource = backups;

				if (lblNoBackups != null)
					lblNoBackups.Visible = backups.Count == 0 ? true : false;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading available backups: {ex.Message}");
			}
		}

		private void LoadMountedBackups()
		{
			if (dgMountedBackups == null)
				return;

			var mounted = NativeBackupMountManager.GetMountedBackups();
			dgMountedBackups.DataSource = mounted;
		}

		private void LoadBackupPoints(AvailableBackupInfo backup)
		{
			if (cmbBackupPoints == null)
				return;

			// For now, just show the main backup
			// In a full implementation, scan for incremental/differential points
			var points = new System.Collections.Generic.List<BackupPoint>
			{
				new() {
					PointDate = backup.BackupDate,
					PointType = backup.BackupType,
					VhdxPath = backup.BackupPath
				}
			};

			cmbBackupPoints.DataSource = points;
			cmbBackupPoints.DisplayMember = "DisplayName";
			if (points.Count > 0)
				cmbBackupPoints.SelectedIndex = 0;
		}

		private string GetBackupPointPath(AvailableBackupInfo backup)
		{
			if (backup.BackupType == "Incremental" || backup.BackupType == "Differential")
			{
				if (cmbBackupPoints?.SelectedItem is BackupPoint point)
				{
					return point.VhdxPath;
				}
				return "";
			}
			else
			{
				return backup.BackupPath;
			}
		}

		private string GetBackupTypeFromFilename(string filenameWithoutExt)
		{
			string lower = filenameWithoutExt.ToLower();

			if (lower.Contains("full"))
				return "Full";
			else if (lower.Contains("incremental") || lower.Contains("incr"))
				return "Incremental";
			else if (lower.Contains("differential") || lower.Contains("diff"))
				return "Differential";
			else
				return "Full"; // Default for job name only (WDrive1.ssb = full backup)
		}

		private void OpenExplorer(string driveLetter)
		{
			try
			{
				System.Diagnostics.Process.Start("explorer.exe", driveLetter);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Failed to open Explorer: {ex.Message}");
			}
		}
	}
}