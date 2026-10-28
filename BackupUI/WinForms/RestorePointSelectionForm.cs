using SecureServerBackup.Helpers;
using SecureServerBackup.Models;
using SecureServerBackup.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestorePointSelectionForm : Form
	{
		public RestorePointSelectionForm(AvailableBackupInfo backup)
		{
			System.Diagnostics.Debug.WriteLine("RestorePointSelectionForm initialized AvailableBackupInfo");
			ArgumentNullException.ThrowIfNull(backup);

			Backup = backup;
			RestorePoints = [.. RestoreWorkflowHelper.GetRestorePointsForBackup(backup.BackupPath)];

			InitializeComponent();
			summaryLabel!.Text = $"Backup: {backup.BackupName} ({backup.BackupType}){Environment.NewLine}Source: {backup.BackupPath}";
			helpLabel!.Text = RestorePoints.Count == 0
				? "No restore points were found for the selected backup."
				: "Select the restore point you want to open, then click Next.";
			nextButton!.Enabled = RestorePoints.Count > 0;

			foreach (RestorePoint restorePoint in RestorePoints)
			{
				restorePointsListBox!.Items.Add(restorePoint);
			}

			if (restorePointsListBox!.Items.Count > 0)
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
				CustomDialogService.ShowWarning(this, "Please select a restore point to continue.", "Restore Point Required");
				return;
			}

			SelectedRestorePoint = selectedRestorePoint;
			DialogResult = DialogResult.OK;
			Close();
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
