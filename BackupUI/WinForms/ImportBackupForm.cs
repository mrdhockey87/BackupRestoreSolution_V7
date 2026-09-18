using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackup.Windows;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class ImportBackupForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		private bool isValidBackup;
		private bool isWimFormat;
		private bool isCompressed;
		private bool isEncrypted;
		private string backupName = string.Empty;
		private string backupType = string.Empty;
		private DateTime backupDate = DateTime.Now;
		private long backupSize;

		public ImportBackupForm()
		{
			InitializeComponent();
		}

		private void Browse_Click(object? sender, EventArgs e)
		{
			using var dialog = new OpenFileDialog
			{
				Title = "Select Backup File",
				Filter = "Backup Files (*.ssb)|*.ssb|SSB Files (*.ssb)|*.ssb|Legacy SSB Backup File (*.wim)|*.wim|All Files (*.*)|*.*",
				CheckFileExists = true
			};

			if (dialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			filePathTextBox!.Text = dialog.FileName;
			ValidateBackupFile(dialog.FileName);
		}

		private void ValidateBackupFile(string filePath)
		{
			try
			{
				validationPanel!.Visible = true;
				backupInfoGroupBox!.Visible = false;
				importButton!.Enabled = false;
				isValidBackup = false;
				isWimFormat = false;

				isEncrypted = BackupEncryptionService.IsEncryptedBackupFile(filePath);
				string validationPath = filePath;

				if (isEncrypted)
				{
					using var preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms(this, filePath, Path.GetFileNameWithoutExtension(filePath));
					validationPath = preparedBackup.WorkingPath;

					ValidatePreparedBackup(filePath, validationPath);
					return;
				}

				ValidatePreparedBackup(filePath, validationPath);
			}
			catch (Exception ex)
			{
				validationPanel!.BackColor = Color.FromArgb(255, 235, 238);
				validationStatusLabel!.Text = "Validation Error";
				validationStatusLabel!.ForeColor = Color.DarkRed;
				validationDetailsLabel!.Text = $"Exception: {ex.Message}";

				BackupLogger.LogError("ImportBackup", "Exception validating backup file", ex.Message);
			}
		}

		private void ValidatePreparedBackup(string originalPath, string validationPath)
		{
			bool compressed;
			var errorMsg = new StringBuilder(512);

			bool valid = NativeBrsValidator.ValidateBackupFile(
				validationPath,
				out compressed,
				out backupName,
				out backupType,
				out backupDate,
				out backupSize,
				errorMsg,
				512);

			if (valid)
			{
				isWimFormat = string.Equals(Path.GetExtension(originalPath), ".wim", StringComparison.OrdinalIgnoreCase);
				validationPanel!.BackColor = Color.FromArgb(232, 245, 233);
				validationStatusLabel!.Text = "Valid Backup File";
				validationStatusLabel!.ForeColor = Color.Green;
				validationDetailsLabel!.Text = isWimFormat
					? "This is a legacy SSB backup file using the .wim extension."
					: "This is a standard SSB backup archive (.ssb) file.";

				formatValueLabel!.Text = isWimFormat ? ".wim (Legacy SSB)" : ".ssb (Standard SSB)";
				backupNameValueLabel!.Text = backupName;
				backupTypeValueLabel!.Text = backupType;
				timestampValueLabel!.Text = backupDate.ToString("yyyy-MM-dd HH:mm:ss");
				sizeValueLabel!.Text = FormatBytes(backupSize);
				compressedValueLabel!.Text = compressed ? "Yes" : "No";
				encryptedValueLabel!.Text = isEncrypted ? "Yes" : "No";

				backupInfoGroupBox!.Visible = true;
				importButton!.Enabled = true;
				isValidBackup = true;
				isCompressed = compressed;

				BackupLogger.LogInfo("ImportBackup", $"Valid backup file detected: {Path.GetFileName(originalPath)}", originalPath);
				return;
			}

			validationPanel!.BackColor = Color.FromArgb(255, 235, 238);
			validationStatusLabel!.Text = "Invalid Backup File";
			validationStatusLabel!.ForeColor = Color.DarkRed;
			validationDetailsLabel!.Text = $"Error: {errorMsg}\r\n\r\nOnly .ssb and legacy .wim SSB backup files are supported.";
			BackupLogger.LogWarning("ImportBackup", $"Invalid backup file: {Path.GetFileName(originalPath)}", errorMsg.ToString());
		}

		private void RenameJob_Changed(object? sender, EventArgs e)
		{
			renamePanel!.Visible = renameJobCheckBox!.Checked;
		}

		private void Import_Click(object? sender, EventArgs e)
		{
			if (!isValidBackup)
			{
				CustomDialogService.ShowWarning(this, "Please select a valid backup file first.", "Invalid Backup");
				return;
			}

			try
			{
				string jobName = renameJobCheckBox!.Checked && !string.IsNullOrWhiteSpace(jobNameTextBox!.Text)
					? jobNameTextBox.Text.Trim()
					: backupName;

				var job = new BackupJob
				{
					Id = Guid.NewGuid(),
					Name = jobName,
					Type = ConvertBackupType(backupType),
					Target = BackupTarget.Disk,
					DestinationPath = filePathTextBox!.Text,
					SourcePaths = [],
					IsImported = true,
					Schedule = null,
					EncryptBackup = isEncrypted
				};

				var jobManager = new JobManager();
				jobManager.AddJob(job);

				BackupLogger.LogSuccess("ImportBackup", $"Backup imported successfully: {jobName}", filePathTextBox.Text);

				CustomDialogService.ShowInfo(
					this,
					$"Backup '{jobName}' imported successfully!\n\n" +
					$"Format: {(isWimFormat ? ".wim" : ".ssb")}\n" +
					$"Size: {FormatBytes(backupSize)}\n" +
					$"Compressed: {(isCompressed ? "Yes" : "No")}\n\n" +
					"The backup is now available in the main window.",
					"Import Successful");

				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				BackupLogger.LogError("ImportBackup", "Failed to import backup", ex.Message);
				CustomDialogService.ShowError(this, $"Failed to import backup:\n{ex.Message}", "Import Error");
			}
		}

		private static BackupType ConvertBackupType(string typeString)
		{
			return typeString.ToLowerInvariant() switch
			{
				"full" => BackupType.Full,
				"incremental" => BackupType.Incremental,
				"differential" => BackupType.Differential,
				_ => BackupType.Full
			};
		}

		private static string FormatBytes(long bytes)
		{
			string[] sizes = ["B", "KB", "MB", "GB", "TB"];
			double length = bytes;
			int order = 0;
			while (length >= 1024 && order < sizes.Length - 1)
			{
				order++;
				length /= 1024;
			}

			return $"{length:0.##} {sizes[order]}";
		}

		private static Label AddInfoRow(TableLayoutPanel table, int rowIndex, string labelText)
		{
			table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			var nameLabel = new Label
			{
				Text = labelText,
				AutoSize = true,
				Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
				Margin = new Padding(0, 5, 0, 5)
			};
			var valueLabel = new Label
			{
				Text = "-",
				AutoSize = true,
				Margin = new Padding(0, 5, 0, 5)
			};

			table.Controls.Add(nameLabel, 0, rowIndex);
			table.Controls.Add(valueLabel, 1, rowIndex);
			return valueLabel;
		}
	}
}
