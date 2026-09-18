using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackupCommon;
using SecureServerBackup.Windows;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestoreFileSelectionForm : Form
	{
		private readonly AvailableBackupInfo backup;
		private readonly RestoreSelectionContext restoreSelection;
		private readonly bool requireAlternateDestination;

		public RestoreFileSelectionForm()
			: this(CreateDesignTimeRestoreSelection())
		{
		}

		public RestoreFileSelectionForm(RestoreSelectionContext restoreSelection)
		{
			ArgumentNullException.ThrowIfNull(restoreSelection);
			ArgumentNullException.ThrowIfNull(restoreSelection.Backup);
			ArgumentNullException.ThrowIfNull(restoreSelection.RestorePoint);

			this.restoreSelection = restoreSelection;
			backup = restoreSelection.Backup;
			requireAlternateDestination = restoreSelection.RequireAlternateDestination;
			InitializeComponent();
			summaryLabel.Text = BuildSummaryText();
			noteLabel.Text = requireAlternateDestination
				? "This restore must target a location that is not on the currently booted drive."
				: "Select the destination folder and start the restore.";

			UpdateActionState();
		}

		private void DestinationTextBox_TextChanged(object? sender, EventArgs e)
		{
			UpdateActionState();
		}

		private void BrowseButton_Click(object? sender, EventArgs e)
		{
			BrowseDestination();
		}

		private async void StartRestoreButton_Click(object? sender, EventArgs e)
		{
			await StartRestoreAsync();
		}

		private string BuildSummaryText()
		{
			string scopeText = restoreSelection.ScopeKind switch
			{
				RestoreScopeKind.All => "Scope: all files or folders from the selected restore point.",
				RestoreScopeKind.SelectedItems => $"Scope: {restoreSelection.SelectedItems.Count} selected files or folders.",
				_ => "Scope: selected restore items."
			};

			return $"Backup: {backup.BackupName} ({backup.BackupType}){Environment.NewLine}" +
				   $"Restore point: {restoreSelection.RestorePoint.DisplayName}{Environment.NewLine}" +
				   scopeText;
		}

		private static RestoreSelectionContext CreateDesignTimeRestoreSelection()
		{
			return new RestoreSelectionContext
			{
				Backup = new AvailableBackupInfo
				{
					BackupName = "Sample Backup",
					BackupType = "Selected Files & Folder",
					BackupPath = @"D:\Backups\Sample.ssb"
				},
				RestorePoint = new RestorePoint
				{
					DisplayName = "2026-08-15 10:30 (Full)",
					FilePath = @"D:\Backups\Sample.ssb"
				},
				ScopeKind = RestoreScopeKind.SelectedItems,
				SelectedItems = [@"Users\Admin\Documents", @"Users\Admin\Desktop\Report.txt"]
			};
		}

		private void BrowseDestination()
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select a folder to restore into",
				ShowNewFolderButton = true
			};

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				destinationTextBox.Text = dialog.SelectedPath;
			}
		}

		private void UpdateActionState()
		{
			startRestoreButton.Enabled = IsValidRestoreTargetPath(destinationTextBox.Text.Trim());
		}

		private async Task StartRestoreAsync()
		{
			string destinationPath = destinationTextBox.Text.Trim();
			if (!IsValidRestoreTargetPath(destinationPath))
			{
				MessageBox.Show(this, "Please enter a valid target path. Local and network paths are supported.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (requireAlternateDestination)
			{
				string destinationRoot = Path.GetPathRoot(destinationPath)?.TrimEnd('\\') ?? destinationPath.TrimEnd('\\');
				string systemRoot = Path.GetPathRoot(Environment.SystemDirectory)?.TrimEnd('\\') ?? string.Empty;
				if (!string.IsNullOrWhiteSpace(destinationRoot) && string.Equals(destinationRoot, systemRoot, StringComparison.OrdinalIgnoreCase))
				{
					MessageBox.Show(this, "Please select a restore destination that is not on the currently booted drive.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
			}

			if (overwriteCheckBox.Checked)
			{
				DialogResult overwriteDecision = MessageBox.Show(this,
					"WARNING: Existing files in the target location may be overwritten when the restore starts.\n\nDo you want to continue?",
					"Confirm File Restore",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);
				if (overwriteDecision != DialogResult.Yes)
				{
					return;
				}
			}

			bool keepWindowOpen = RestoreWorkflowHelper.ShouldKeepRestoreCompletionWindowOpen(
				RestoreTargetKind.FileOrFolder,
				requireAlternateDestination,
				null,
				null);

			using var progressForm = new RestoreProgressForm(
				restoreSelection.RestorePoint.DisplayName,
				keepWindowOpen,
				callback => PerformRestoreAsync(destinationPath, callback));

			Hide();
			try
			{
				progressForm.ShowDialog(this);
				if (progressForm.RestoreSucceeded)
				{
					DialogResult = DialogResult.OK;
					Close();
				}
			}
			finally
			{
				if (!IsDisposed && Visible == false)
				{
					Show();
				}
			}
		}

		private async Task PerformRestoreAsync(string destinationPath, SecureServerBackup.Services.BackupEngineInterop.ProgressCallback callback)
		{
			RestorePoint selectedPoint = restoreSelection.RestorePoint;
			await Task.Run(() =>
			{
				using var preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms(this, selectedPoint.FilePath, Path.GetFileNameWithoutExtension(selectedPoint.FilePath), backup.ProtectedEncryptionPassword);
				int result;
				int lastLoggedPercent = -1;

				BackupLogger.LogInfo("[Restore]", "Restore started", $"Restore point: {selectedPoint.DisplayName}; Type: FileOrFolder; Source: {selectedPoint.FilePath}; Destination: {destinationPath}");

				SecureServerBackup.Services.BackupEngineInterop.ProgressCallback wrappedCallback = (percent, message) =>
				{
					callback(percent, message);
					if (percent >= 0 && percent != lastLoggedPercent)
					{
						lastLoggedPercent = percent;
						BackupLogger.LogInfo("[Restore]", $"Restore progress {percent}%", message ?? string.Empty);
					}
					else if (!string.IsNullOrWhiteSpace(message) &&
						(message.StartsWith("Restore warning:", StringComparison.OrdinalIgnoreCase) ||
						 message.StartsWith("Restore error:", StringComparison.OrdinalIgnoreCase) ||
						 message.StartsWith("Restoring:", StringComparison.OrdinalIgnoreCase) ||
						 message.StartsWith("Processing:", StringComparison.OrdinalIgnoreCase)))
					{
						BackupLogger.LogInfo("[Restore]", message, $"Progress: {percent}%");
					}
				};

				if (restoreSelection.ScopeKind == RestoreScopeKind.SelectedItems && restoreSelection.SelectedItems.Count > 0)
				{
					string manifest = string.Join(Environment.NewLine, restoreSelection.SelectedItems);
					result = SecureServerBackup.Services.BackupEngineInterop.RestoreWithManifest(preparedBackup.WorkingPath, destinationPath, manifest, overwriteCheckBox.Checked, restoreSystemState: false, preservePermissions: false, wrappedCallback);
				}
				else
				{
					result = SecureServerBackup.Services.BackupEngineInterop.RestoreFiles(preparedBackup.WorkingPath, destinationPath, overwriteCheckBox.Checked, wrappedCallback);
				}

				if (result != 0)
				{
					var error = new StringBuilder(1024);
					SecureServerBackup.Services.BackupEngineInterop.GetLastErrorMessage(error, error.Capacity);
					BackupLogger.LogError("[Restore]", "Restore failed", error.ToString());
					throw new Exception($"Restore failed: {error}");
				}

				BackupLogger.LogSuccess("[Restore]", "Restore completed successfully", selectedPoint.FilePath, $"Type: FileOrFolder; Destination: {destinationPath}");
			});
		}

		private static bool IsValidRestoreTargetPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}

			try
			{
				string expandedPath = Environment.ExpandEnvironmentVariables(path.Trim());
				if (expandedPath.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
				{
					return expandedPath.Length > 2;
				}

				char[] invalidChars = Path.GetInvalidPathChars();
				return expandedPath.IndexOfAny(invalidChars) < 0;
			}
			catch
			{
				return false;
			}
		}
	}
}
