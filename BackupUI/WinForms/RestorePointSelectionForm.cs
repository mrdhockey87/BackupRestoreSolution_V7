using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Windows;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestorePointSelectionForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		public RestorePointSelectionForm()
			: this(CreateDesignTimeBackup())
		{
		}

		public RestorePointSelectionForm(AvailableBackupInfo backup)
		{
			ArgumentNullException.ThrowIfNull(backup);

			Backup = backup;
			RestorePoints = IsInDesignMode
				? [CreateDesignTimeRestorePoint()]
				: RestoreWorkflowHelper.GetRestorePointsForBackup(backup.BackupPath).ToList();

			InitializeComponent();
			summaryLabel.Text = $"Backup: {backup.BackupName} ({backup.BackupType}){Environment.NewLine}Source: {backup.BackupPath}";
			helpLabel.Text = RestorePoints.Count == 0
				? "No restore points were found for the selected backup."
				: "Select the restore point you want to open, then click Next.";
			nextButton.Enabled = RestorePoints.Count > 0;

			foreach (RestorePoint restorePoint in RestorePoints)
			{
				restorePointsListBox.Items.Add(restorePoint);
			}

			if (restorePointsListBox.Items.Count > 0)
			{
				restorePointsListBox.SelectedIndex = 0;
			}
		}

		public AvailableBackupInfo Backup { get; }

		public IReadOnlyList<RestorePoint> RestorePoints { get; }

		public RestorePoint? SelectedRestorePoint { get; private set; }

		private void ConfirmSelection()
		{
			if (restorePointsListBox.SelectedItem is not RestorePoint selectedRestorePoint)
			{
				MessageBox.Show(this, "Please select a restore point to continue.", "Restore Point Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			SelectedRestorePoint = selectedRestorePoint;
			DialogResult = DialogResult.OK;
			Close();
		}

		private static AvailableBackupInfo CreateDesignTimeBackup()
		{
			return new AvailableBackupInfo
			{
				BackupName = "Sample Backup",
				BackupType = "Full",
				BackupPath = @"D:\Backups\Sample.ssb"
			};
		}

		private static RestorePoint CreateDesignTimeRestorePoint()
		{
			return new RestorePoint
			{
				DisplayName = "2026-08-15 10:30 (Full)",
				BackupType = "Full",
				FilePath = @"D:\Backups\Sample.ssb",
				Timestamp = new DateTime(2026, 8, 15, 10, 30, 0)
			};
		}

		private void RestorePointsListBox_DoubleClick(object? sender, EventArgs e)
		{
			ConfirmSelection();
		}

		private void NextButton_Click(object? sender, EventArgs e)
		{
			ConfirmSelection();
		}
	}
}
