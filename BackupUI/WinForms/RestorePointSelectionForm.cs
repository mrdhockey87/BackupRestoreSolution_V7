using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Windows;

namespace SecureServerBackup.WinForms
{
	internal sealed class RestorePointSelectionForm : Form
	{
		private readonly ListBox restorePointsListBox;
		private readonly Button nextButton;

		public RestorePointSelectionForm(AvailableBackupInfo backup)
		{
			ArgumentNullException.ThrowIfNull(backup);

			Backup = backup;
			RestorePoints = RestoreWorkflowHelper.GetRestorePointsForBackup(backup.BackupPath)
				.ToList();

			Text = "Select Restore Point";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(760, 420);
			MinimumSize = new Size(760, 420);
			BackColor = Color.White;

			var summaryLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 16),
				Size = new Size(720, 56),
				Text = $"Backup: {backup.BackupName} ({backup.BackupType}){Environment.NewLine}Source: {backup.BackupPath}"
			};

			restorePointsListBox = new ListBox
			{
				Location = new Point(16, 82),
				Size = new Size(720, 250),
				DisplayMember = nameof(RestorePoint.DisplayName),
				Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
			};

			foreach (RestorePoint restorePoint in RestorePoints)
			{
				restorePointsListBox.Items.Add(restorePoint);
			}

			if (restorePointsListBox.Items.Count > 0)
			{
				restorePointsListBox.SelectedIndex = 0;
			}

			restorePointsListBox.DoubleClick += (_, _) => ConfirmSelection();

			var helpLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 340),
				Size = new Size(720, 24),
				Text = RestorePoints.Count == 0
					? "No restore points were found for the selected backup."
					: "Select the restore point you want to open, then click Next."
			};

			nextButton = new Button
			{
				Text = "Next",
				Size = new Size(96, 32),
				Location = new Point(540, 374),
				Enabled = RestorePoints.Count > 0,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right
			};
			nextButton.Click += (_, _) => ConfirmSelection();

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(96, 32),
				Location = new Point(640, 374),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.Cancel
			};

			AcceptButton = nextButton;
			CancelButton = cancelButton;

			Controls.Add(summaryLabel);
			Controls.Add(restorePointsListBox);
			Controls.Add(helpLabel);
			Controls.Add(nextButton);
			Controls.Add(cancelButton);
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
	}
}
