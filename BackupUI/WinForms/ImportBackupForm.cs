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
	internal sealed class ImportBackupForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly TextBox filePathTextBox;
		private readonly Panel validationPanel;
		private readonly Label validationStatusLabel;
		private readonly Label validationDetailsLabel;
		private readonly GroupBox backupInfoGroupBox;
		private readonly Label formatValueLabel;
		private readonly Label backupNameValueLabel;
		private readonly Label backupTypeValueLabel;
		private readonly Label timestampValueLabel;
		private readonly Label sizeValueLabel;
		private readonly Label compressedValueLabel;
		private readonly Label encryptedValueLabel;
		private readonly CheckBox renameJobCheckBox;
		private readonly Panel renamePanel;
		private readonly TextBox jobNameTextBox;
		private readonly Button importButton;

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
			Text = "Import Backup";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			StartPosition = FormStartPosition.CenterParent;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(700, 500);

			var root = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(15),
				ColumnCount = 1,
				RowCount = 4
			};
			root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			var headerLabel = new Label
			{
				Text = "Import External Backup",
				Font = new Font(Font ?? SystemFonts.DefaultFont, FontStyle.Bold),
				AutoSize = true,
				Margin = new Padding(0, 0, 0, 15)
			};

			var fileGroupBox = new GroupBox
			{
				Text = "Select Backup File",
				Dock = DockStyle.Fill,
				Margin = new Padding(0, 0, 0, 15)
			};

			var fileLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(10),
				ColumnCount = 1,
				RowCount = 4
			};
			fileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			fileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			fileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			fileLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

			fileLayout.Controls.Add(new Label { Text = "Backup File:", AutoSize = true, Margin = new Padding(0, 0, 0, 5) }, 0, 0);

			var fileBrowsePanel = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				ColumnCount = 2,
				AutoSize = true,
				Margin = new Padding(0, 0, 0, 15)
			};
			fileBrowsePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			fileBrowsePanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			filePathTextBox = new TextBox
			{
				Dock = DockStyle.Fill,
				Margin = new Padding(0, 0, 10, 0)
			};
			var browseButton = new Button
			{
				Text = "Browse...",
				AutoSize = true,
				MinimumSize = new Size(100, 30)
			};
			browseButton.Click += Browse_Click;

			fileBrowsePanel.Controls.Add(filePathTextBox, 0, 0);
			fileBrowsePanel.Controls.Add(browseButton, 1, 0);
			fileLayout.Controls.Add(fileBrowsePanel, 0, 1);

			validationPanel = new Panel
			{
				Dock = DockStyle.Top,
				Padding = new Padding(10),
				Margin = new Padding(0, 0, 0, 15),
				Visible = false,
				BackColor = Color.FromArgb(232, 245, 233),
				AutoSize = true
			};
			validationStatusLabel = new Label
			{
				AutoSize = true,
				Font = new Font(Font ?? SystemFonts.DefaultFont, FontStyle.Bold),
				Margin = new Padding(0, 0, 0, 10),
				ForeColor = Color.Green
			};
			validationDetailsLabel = new Label
			{
				AutoSize = true,
				MaximumSize = new Size(620, 0)
			};
			var validationLayout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.TopDown,
				WrapContents = false,
				AutoSize = true
			};
			validationLayout.Controls.Add(validationStatusLabel);
			validationLayout.Controls.Add(validationDetailsLabel);
			validationPanel.Controls.Add(validationLayout);
			fileLayout.Controls.Add(validationPanel, 0, 2);

			backupInfoGroupBox = new GroupBox
			{
				Text = "Backup Information",
				Dock = DockStyle.Fill,
				Visible = false
			};
			var infoLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(10),
				ColumnCount = 2,
				RowCount = 7
			};
			infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
			infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

			formatValueLabel = AddInfoRow(infoLayout, 0, "Format:");
			backupNameValueLabel = AddInfoRow(infoLayout, 1, "Backup Name:");
			backupTypeValueLabel = AddInfoRow(infoLayout, 2, "Backup Type:");
			timestampValueLabel = AddInfoRow(infoLayout, 3, "Date Created:");
			sizeValueLabel = AddInfoRow(infoLayout, 4, "Size:");
			compressedValueLabel = AddInfoRow(infoLayout, 5, "Compressed:");
			encryptedValueLabel = AddInfoRow(infoLayout, 6, "Encrypted:");
			backupInfoGroupBox.Controls.Add(infoLayout);
			fileLayout.Controls.Add(backupInfoGroupBox, 0, 3);
			fileGroupBox.Controls.Add(fileLayout);

			var optionsGroupBox = new GroupBox
			{
				Text = "Import Options",
				Dock = DockStyle.Top,
				Margin = new Padding(0, 0, 0, 15),
				AutoSize = true
			};
			var optionsLayout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.TopDown,
				WrapContents = false,
				AutoSize = true,
				Padding = new Padding(10)
			};
			renameJobCheckBox = new CheckBox
			{
				Text = "Rename imported backup",
				AutoSize = true,
				Margin = new Padding(0, 0, 0, 10)
			};
			renameJobCheckBox.CheckedChanged += RenameJob_Changed;
			renamePanel = new Panel
			{
				AutoSize = true,
				Visible = false
			};
			var renameLabel = new Label
			{
				Text = "New Job Name:",
				AutoSize = true,
				Margin = new Padding(0, 0, 0, 5)
			};
			jobNameTextBox = new TextBox
			{
				Width = 300
			};
			renamePanel.Controls.Add(renameLabel);
			renamePanel.Controls.Add(jobNameTextBox);
			jobNameTextBox.Top = renameLabel.Bottom + 5;
			optionsLayout.Controls.Add(renameJobCheckBox);
			optionsLayout.Controls.Add(renamePanel);
			optionsGroupBox.Controls.Add(optionsLayout);

			var buttonsPanel = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.RightToLeft,
				Dock = DockStyle.Fill,
				AutoSize = true
			};
			var cancelButton = new Button
			{
				Text = "Cancel",
				DialogResult = DialogResult.Cancel,
				MinimumSize = new Size(100, 35),
				AutoSize = true
			};
			importButton = new Button
			{
				Text = "Import",
				Enabled = false,
				MinimumSize = new Size(100, 35),
				AutoSize = true,
				Margin = new Padding(0, 0, 10, 0)
			};
			importButton.Click += Import_Click;
			buttonsPanel.Controls.Add(cancelButton);
			buttonsPanel.Controls.Add(importButton);

			root.Controls.Add(headerLabel, 0, 0);
			root.Controls.Add(fileGroupBox, 0, 1);
			root.Controls.Add(optionsGroupBox, 0, 2);
			root.Controls.Add(buttonsPanel, 0, 3);

			Controls.Add(root);
			AcceptButton = importButton;
			CancelButton = cancelButton;
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

			filePathTextBox.Text = dialog.FileName;
			ValidateBackupFile(dialog.FileName);
		}

		private void ValidateBackupFile(string filePath)
		{
			try
			{
				validationPanel.Visible = true;
				backupInfoGroupBox.Visible = false;
				importButton.Enabled = false;
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
				validationPanel.BackColor = Color.FromArgb(255, 235, 238);
				validationStatusLabel.Text = "Validation Error";
				validationStatusLabel.ForeColor = Color.DarkRed;
				validationDetailsLabel.Text = $"Exception: {ex.Message}";

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
				validationPanel.BackColor = Color.FromArgb(232, 245, 233);
				validationStatusLabel.Text = "Valid Backup File";
				validationStatusLabel.ForeColor = Color.Green;
				validationDetailsLabel.Text = isWimFormat
					? "This is a legacy SSB backup file using the .wim extension."
					: "This is a standard SSB backup archive (.ssb) file.";

				formatValueLabel.Text = isWimFormat ? ".wim (Legacy SSB)" : ".ssb (Standard SSB)";
				backupNameValueLabel.Text = backupName;
				backupTypeValueLabel.Text = backupType;
				timestampValueLabel.Text = backupDate.ToString("yyyy-MM-dd HH:mm:ss");
				sizeValueLabel.Text = FormatBytes(backupSize);
				compressedValueLabel.Text = compressed ? "Yes" : "No";
				encryptedValueLabel.Text = isEncrypted ? "Yes" : "No";

				backupInfoGroupBox.Visible = true;
				importButton.Enabled = true;
				isValidBackup = true;
				isCompressed = compressed;

				BackupLogger.LogInfo("ImportBackup", $"Valid backup file detected: {Path.GetFileName(originalPath)}", originalPath);
				return;
			}

			validationPanel.BackColor = Color.FromArgb(255, 235, 238);
			validationStatusLabel.Text = "Invalid Backup File";
			validationStatusLabel.ForeColor = Color.DarkRed;
			validationDetailsLabel.Text = $"Error: {errorMsg}\r\n\r\nOnly .ssb and legacy .wim SSB backup files are supported.";
			BackupLogger.LogWarning("ImportBackup", $"Invalid backup file: {Path.GetFileName(originalPath)}", errorMsg.ToString());
		}

		private void RenameJob_Changed(object? sender, EventArgs e)
		{
			renamePanel.Visible = renameJobCheckBox.Checked;
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
				string jobName = renameJobCheckBox.Checked && !string.IsNullOrWhiteSpace(jobNameTextBox.Text)
					? jobNameTextBox.Text.Trim()
					: backupName;

				var job = new BackupJob
				{
					Id = Guid.NewGuid(),
					Name = jobName,
					Type = ConvertBackupType(backupType),
					Target = BackupTarget.Disk,
					DestinationPath = filePathTextBox.Text,
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
