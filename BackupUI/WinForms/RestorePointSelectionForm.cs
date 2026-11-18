using SecureServerBackup.Helpers;
using SecureServerBackup.Models;
using SecureServerBackup.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestorePointSelectionForm : Form
	{
		private IReadOnlyList<RestorePoint> _restorePoints = Array.Empty<RestorePoint>();

		public RestorePointSelectionForm(AvailableBackupInfo backup)
		{
			System.Diagnostics.Debug.WriteLine("RestorePointSelectionForm initialized AvailableBackupInfo");
			ArgumentNullException.ThrowIfNull(backup);

			Backup = backup;

			InitializeComponent();
			summaryLabel!.Text = $"Backup: {backup.BackupName} ({backup.BackupType}){Environment.NewLine}Source: {backup.BackupPath}";
			helpLabel!.Text = "Loading restore points…";
			nextButton!.Enabled = false;

			// Show loading panel immediately
			pnlLoading!.Visible = true;
			pnlLoading.BringToFront();
			pbLoading!.Style = ProgressBarStyle.Marquee;
			txtLoadingStatus!.Text = "Loading restore points…";

			// Hook Shown event instead of Load - fires AFTER form is fully rendered
			this.Shown += RestorePointSelectionForm_Shown;
		}

		private async void RestorePointSelectionForm_Shown(object? sender, EventArgs e)
		{
			// Unhook to prevent multiple calls
			this.Shown -= RestorePointSelectionForm_Shown;

			await LoadRestorePointsAsync();
		}

		private async Task LoadRestorePointsAsync()
		{
			try
			{
				_restorePoints = await Task.Run(() =>
					RestoreWorkflowHelper.GetRestorePointsForBackup(Backup.BackupPath));

				restorePointsListBox!.BeginUpdate();
				restorePointsListBox.Items.Clear();

				foreach (RestorePoint restorePoint in _restorePoints)
				{
					restorePointsListBox.Items.Add(restorePoint);
				}

				if (restorePointsListBox.Items.Count > 0)
				{
					restorePointsListBox.SelectedIndex = 0;
				}

						restorePointsListBox.EndUpdate();

						helpLabel!.Text = _restorePoints.Count == 0
							? "No restore points were found for the selected backup."
							: "Select the restore point you want to open, then click Next.";
						nextButton!.Enabled = _restorePoints.Count > 0;

						// Hide loading panel
						pnlLoading!.Visible = false;
					}
					catch (Exception ex)
					{
						pnlLoading!.Visible = false;
						MessageBox.Show(this, $"Error loading restore points: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						DialogResult = DialogResult.Cancel;
						Close();
					}
				}

		public AvailableBackupInfo Backup { get; }

		public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

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
