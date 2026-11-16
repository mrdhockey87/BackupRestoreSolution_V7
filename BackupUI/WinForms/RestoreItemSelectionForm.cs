using SecureServerBackup.Enums;
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

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestoreItemSelectionForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		public RestoreItemSelectionForm() : this(CreateDesignTimeBackup(), CreateDesignTimeRestorePoint())
		{
		    //, [@"Users\\Admin\\Documents", @"Users\\Admin\\Desktop\\Report.txt"]
			System.Diagnostics.Debug.WriteLine("RestoreImageSelectionForm initialized CreateDesignTimeRestorePoint(), [@\"Users\\\\Admin\\\\Documents\", @\"Users\\\\Admin\\\\Desktop\\\\Report.txt\"])");
		}

		public RestoreItemSelectionForm(AvailableBackupInfo backup, RestorePoint restorePoint) //, IReadOnlyList<string> items)
		{
			System.Diagnostics.Debug.WriteLine("RestoreImageSelectionForm initialized AvailableBackupInfo, RestorePoint, IReadOnlyList<string>");
			ArgumentNullException.ThrowIfNull(backup);
			ArgumentNullException.ThrowIfNull(restorePoint);
			//ArgumentNullException.ThrowIfNull(items);

			Backup = backup;
			RestorePoint = restorePoint;

			InitializeComponent();
			summaryLabel!.Text = $"Restore point: {restorePoint.DisplayName}{Environment.NewLine}Source: {restorePoint.FilePath}";
			helpLabel!.Text = "Select one or more files or folders to restore, then click Next.";
			this.Load += RestoreItemSelectionForm_Load;

			nextButton!.Enabled = itemsListBox!.Items.Count > 0;
		}
		private async void RestoreItemSelectionForm_Load(object? sender, EventArgs e)
		{
			if (pnlLoading is null)
			{
				return;
			}

			pnlLoading.Visible = true;
			pnlLoading.BringToFront();
			nextButton.Enabled = false;
			pbLoading.Style = ProgressBarStyle.Marquee;
			txtLoadingStatus.Text = "Loading backup contents…";

			IReadOnlyList<string> items;
			try
			{
				items = await Task.Run(() => RestoreWorkflowHelper.GetBackupItemsForRestorePoint(RestorePoint));
			}
			catch (Exception ex)
			{
				pnlLoading.Visible = false;
				MessageBox.Show(this, $"Failed to load backup contents: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				DialogResult = DialogResult.Cancel;
				Close();
				return;
			}

			// Populate list in batches so the UI stays responsive
			txtLoadingStatus.Text = $"Populating list ({items.Count} items)…";
			pbLoading.Style = ProgressBarStyle.Continuous;
			pbLoading.Minimum = 0;
			pbLoading.Maximum = Math.Max(items.Count, 1);
			pbLoading.Value = 0;

			const int batchSize = 200;
			int added = 0;
			itemsListBox.BeginUpdate();
			try
			{
				foreach (string item in items.Where(item => !string.IsNullOrWhiteSpace(item)))
				{
					itemsListBox.Items.Add(item);
					added++;

					if (added % batchSize == 0)
					{
						pbLoading.Value = added;
						await Task.Yield();
					}
				}
			}
			finally
			{
				itemsListBox.EndUpdate();
			}

			pbLoading.Value = Math.Max(added, 0);
			pnlLoading.Visible = false;
			nextButton.Enabled = itemsListBox.Items.Count > 0;

			if (itemsListBox.Items.Count == 0)
			{
				MessageBox.Show(this, "No selectable files or folders were found for the selected restore point.", "Restore Items Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				DialogResult = DialogResult.Cancel;
				Close();
			}
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
				CustomDialogService.ShowWarning(this, "Please select at least one file or folder to continue.", "Restore Items Required");
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
