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
	internal sealed partial class RestoreItemSelectionForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		public RestoreItemSelectionForm()
			: this(CreateDesignTimeBackup(), CreateDesignTimeRestorePoint(), [@"Users\\Admin\\Documents", @"Users\\Admin\\Desktop\\Report.txt"])
		{
		}

		public RestoreItemSelectionForm(AvailableBackupInfo backup, RestorePoint restorePoint, IReadOnlyList<string> items)
		{
			ArgumentNullException.ThrowIfNull(backup);
			ArgumentNullException.ThrowIfNull(restorePoint);
			ArgumentNullException.ThrowIfNull(items);

			Backup = backup;
			RestorePoint = restorePoint;

			InitializeComponent();
			summaryLabel!.Text = $"Restore point: {restorePoint.DisplayName}{Environment.NewLine}Source: {restorePoint.FilePath}";
			helpLabel!.Text = "Select one or more files or folders to restore, then click Next.";

			foreach (string item in items.Where(item => !string.IsNullOrWhiteSpace(item)))
			{
				itemsListBox!.Items.Add(item);
			}

			nextButton!.Enabled = itemsListBox!.Items.Count > 0;
		}

		public AvailableBackupInfo Backup { get; }

		public RestorePoint RestorePoint { get; }

		public IReadOnlyList<string> SelectedItems { get; private set; } = Array.Empty<string>();

		private void ConfirmSelection()
		{
			List<string> selectedItems = itemsListBox.SelectedItems.Cast<object>()
				.Select(item => item?.ToString() ?? string.Empty)
				.Where(item => !string.IsNullOrWhiteSpace(item))
				.ToList();

			if (selectedItems.Count == 0)
			{
				MessageBox.Show(this, "Please select at least one file or folder to continue.", "Restore Items Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			SelectedItems = selectedItems;
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

		private void NextButton_Click(object? sender, EventArgs e)
		{
			ConfirmSelection();
		}
	}
}
