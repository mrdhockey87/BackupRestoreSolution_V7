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

			// Hook Shown event instead of Load - fires AFTER form is fully rendered
			this.Shown += RestoreItemSelectionForm_Shown;

			nextButton!.Enabled = itemsListBox!.Items.Count > 0;
		}

		private async void RestoreItemSelectionForm_Shown(object? sender, EventArgs e)
		{
			// Unhook to prevent multiple calls
			this.Shown -= RestoreItemSelectionForm_Shown;

			if (pnlLoading is null)
			{
				return;
			}

			System.Diagnostics.Debug.WriteLine($"RestoreItemSelectionForm_Shown: Starting for {RestorePoint.DisplayName}");
			pnlLoading.Visible = true;
			pnlLoading.BringToFront();
			nextButton.Enabled = false;
			pbLoading.Style = ProgressBarStyle.Marquee;
			txtLoadingStatus.Text = "Loading backup contents…";

			try
			{
				// Give the UI a chance to render the loading panel
				await Task.Delay(100);

				// Load items on background thread WITH progress callback
				IReadOnlyList<string> allItems = await Task.Run(() =>
				{
					System.Diagnostics.Debug.WriteLine($"RestoreItemSelectionForm_Shown: Task.Run starting GetBackupItemsForRestorePointWithProgress");
					return RestoreWorkflowHelper.GetBackupItemsForRestorePointWithProgress(
						RestorePoint,
						batchItems =>
						{
							System.Diagnostics.Debug.WriteLine($"RestoreItemSelectionForm_Shown: Callback invoked with {batchItems.Count} items");

							// Update UI with new items - safe thread marshaling
							this.BeginInvoke(() => 
							{
								// Add batch items to list (don't clear, just append)
								foreach (string item in batchItems.Where(item => !string.IsNullOrWhiteSpace(item)))
								{
									itemsListBox.Items.Add(item);
								}

								// Update progress bar and status
								pbLoading.Style = ProgressBarStyle.Continuous;
								pbLoading.Minimum = 0;
								pbLoading.Maximum = 100;
								pbLoading.Value = Math.Min(itemsListBox.Items.Count * 100 / Math.Max(itemsListBox.Items.Count + 50, 1), 95);
								txtLoadingStatus.Text = $"Loaded {itemsListBox.Items.Count} items…";

								// Move to end of list
								if (itemsListBox.Items.Count > 0)
								{
									itemsListBox.TopIndex = Math.Max(0, itemsListBox.Items.Count - 1);
								}
							});
						});
				});
				//Added because even the small file said there were not items to select, after I put this here
				//it worked. On the large file however the items were there multiple times for each loop that
				//LoadBackupItems looped mdail 11/18/2026
				if (itemsListBox.Items.Count == 0 && allItems.Count != 0)
				{
					foreach (string item in allItems.Where(item => !string.IsNullOrWhiteSpace(item)))
					{
						itemsListBox.Items.Add(item);
					}
					System.Diagnostics.Debug.WriteLine($"RestoreItemSelectionForm_Shown: No items found for restore point {RestorePoint.DisplayName}");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"RestoreItemSelectionForm_Shown: Exception: {ex.Message}");
				pnlLoading.Visible = false;
				MessageBox.Show(this, $"Failed to load backup contents: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				DialogResult = DialogResult.Cancel;
				Close();
				return;
			}

			// Hide loading panel
			pnlLoading.Visible = false;
			nextButton.Enabled = itemsListBox!.Items.Count > 0;

			System.Diagnostics.Debug.WriteLine($"RestoreItemSelectionForm_Shown: Final state - itemsListBox.Items.Count={itemsListBox.Items.Count}");

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
