using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using SecureServerBackup.Helpers;

using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class ActivityDetailForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly string? filterJobName;
		private readonly BindingSource bindingSource = new();
		private List<BackupLogEntry> currentLogs = new();

		public ActivityDetailForm()
			: this(null)
		{
		}

		public ActivityDetailForm(string? jobName)
		{
			filterJobName = string.IsNullOrWhiteSpace(jobName) ? null : jobName;

			InitializeComponent();
			activitiesGrid.DataSource = bindingSource;
			filterComboBox.SelectedIndex = 0;
			ApplyFormText();

			if (IsInDesignMode)
			{
				LoadDesignTimeActivities();
			}
			else
			{
				Load += ActivityDetailForm_Load;
			}
		}

		private void ApplyFormText()
		{
			Text = string.IsNullOrEmpty(filterJobName) ? "All Activity Details" : $"Activity Details - {filterJobName}";
			titleLabel.Text = string.IsNullOrEmpty(filterJobName) ? "All Activities" : $"Activities for: {filterJobName}";
		}

		private void ActivityDetailForm_Load(object? sender, EventArgs e)
		{
			LoadActivities();
		}

		private void LoadDesignTimeActivities()
		{
			currentLogs =
			[
				new BackupLogEntry
				{
					Timestamp = DateTime.Now.AddMinutes(-10),
					JobName = "Sample Backup",
					Level = BackupLogLevel.Info,
					Message = "Backup started",
					Details = "Design-time preview entry",
					BackupPath = @"D:\Backups\Sample.ssb"
				},
				new BackupLogEntry
				{
					Timestamp = DateTime.Now.AddMinutes(-2),
					JobName = "Sample Backup",
					Level = BackupLogLevel.Success,
					Message = "Backup completed",
					Details = "Designer preview data",
					BackupPath = @"D:\Backups\Sample.ssb"
				}
			];

			ApplyLevelFilter();
		}

		private void RefreshButton_Click(object? sender, EventArgs e)
		{
			LoadActivities();
		}

		private void ExportCsvButton_Click(object? sender, EventArgs e)
		{
			ExportSelected("CSV");
		}

		private void ExportTextButton_Click(object? sender, EventArgs e)
		{
			ExportSelected("Text");
		}

		private void DeleteButton_Click(object? sender, EventArgs e)
		{
			DeleteSelectedActivities();
		}

		private void CopyButton_Click(object? sender, EventArgs e)
		{
			CopySelectedToClipboard();
		}

		private void SelectAllButton_Click(object? sender, EventArgs e)
		{
			SelectAllActivities();
		}

		private void ClearSelectionButton_Click(object? sender, EventArgs e)
		{
			ClearActivitySelection();
		}

		private void FilterComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			ApplyLevelFilter();
		}

		private void ActivitiesGrid_SelectionChanged(object? sender, EventArgs e)
		{
			UpdateSelectionCount();
		}

		private void ExportCsvToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			ExportSelected("CSV");
		}

		private void ExportTextToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			ExportSelected("Text");
		}

		private void CopySelectedToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			CopySelectedToClipboard();
		}

		private void DeleteSelectedToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			DeleteSelectedActivities();
		}

		private void SelectAllToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			SelectAllActivities();
		}

		private void ClearSelectionToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			ClearActivitySelection();
		}

		private void SelectAllActivities()
		{
			activitiesGrid.SelectAll();
		}

		private void ClearActivitySelection()
		{
			activitiesGrid.ClearSelection();
		}

		private void LoadActivities()
		{
			try
			{
				List<BackupLogEntry> allLogs = BackupLogger.GetRecentLogs(10000);
				currentLogs = string.IsNullOrEmpty(filterJobName)
					? allLogs
					: allLogs.Where(log => log.JobName == filterJobName).ToList();
				ApplyLevelFilter();
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error loading activities: {ex.Message}{Environment.NewLine}{Environment.NewLine}Details: {ex.StackTrace}", "Error");
			}
		}

		private void ApplyLevelFilter()
		{
			IEnumerable<BackupLogEntry> filteredLogs = currentLogs;
			string selectedFilter = filterComboBox.SelectedItem?.ToString() ?? "All";
			filteredLogs = selectedFilter switch
			{
				"Info" => filteredLogs.Where(log => log.Level == BackupLogLevel.Info),
				"Success" => filteredLogs.Where(log => log.Level == BackupLogLevel.Success),
				"Warning" => filteredLogs.Where(log => log.Level == BackupLogLevel.Warning),
				"Error" => filteredLogs.Where(log => log.Level == BackupLogLevel.Error),
				_ => filteredLogs
			};

			bindingSource.DataSource = filteredLogs.OrderByDescending(log => log.Timestamp).ToList();
			UpdateSelectionCount();
			UpdateStatusText();
		}

		private void UpdateSelectionCount()
		{
			int selectedCount = activitiesGrid.SelectedRows.Count;
			selectionCountLabel.Text = selectedCount == 1 ? "1 activity selected" : $"{selectedCount} activities selected";
		}

		private void UpdateStatusText()
		{
			int count = bindingSource.Count;
			statusLabel.Text = string.IsNullOrEmpty(filterJobName)
				? $"Showing {count} activities from all jobs. Use Shift+Click or Ctrl+Click to select multiple. Right-click for options."
				: $"Showing {count} activities for {filterJobName}. Use Shift+Click or Ctrl+Click to select multiple. Right-click for options.";
		}

		private List<BackupLogEntry> GetSelectedLogs()
		{
			return activitiesGrid.SelectedRows
				.Cast<DataGridViewRow>()
				.Select(row => row.DataBoundItem)
				.OfType<BackupLogEntry>()
				.ToList();
		}

		private void ExportSelected(string format)
		{
			List<BackupLogEntry> selectedLogs = GetSelectedLogs();
			if (selectedLogs.Count == 0)
			{
				CustomDialogService.ShowInfo(this, "Please select activities to export.", "No Selection");
				return;
			}

			string suggestedName = string.IsNullOrEmpty(filterJobName) ? "activities_export" : $"{filterJobName}_activities_export";
			try
			{
				bool exported = ActivityLogExportHelper.PromptAndExport(this, selectedLogs, suggestedName, format);
				if (exported)
				{
					CustomDialogService.ShowSuccess(this, $"Successfully exported {selectedLogs.Count} activities.", "Export Complete");
				}
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error exporting activities: {ex.Message}", "Export Error");
			}
		}

		private void DeleteSelectedActivities()
		{
			List<BackupLogEntry> selectedLogs = GetSelectedLogs();
			if (selectedLogs.Count == 0)
			{
				CustomDialogService.ShowInfo(this, "Please select activities to delete.", "No Selection");
				return;
			}

			CustomDialogResult result = CustomDialogService.ShowOKCancel(this,
				$"Are you sure you want to delete {selectedLogs.Count} selected activity log(s)?{Environment.NewLine}{Environment.NewLine}This action cannot be undone.",
				"Confirm Delete",
				DialogIcon.Warning);
			if (result != CustomDialogResult.OK)
			{
				return;
			}

			try
			{
				int deletedCount = 0;
				foreach (BackupLogEntry log in selectedLogs)
				{
					if (BackupLogger.DeleteLogEntry(log))
					{
						deletedCount++;
					}
				}

				CustomDialogService.ShowSuccess(this, $"Successfully deleted {deletedCount} activity log(s).", "Delete Complete");
				LoadActivities();
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error deleting activities: {ex.Message}", "Delete Error");
			}
		}

		private void CopySelectedToClipboard()
		{
			List<BackupLogEntry> selectedLogs = GetSelectedLogs();
			if (selectedLogs.Count == 0)
			{
				CustomDialogService.ShowInfo(this, "Please select activities to copy.", "No Selection");
				return;
			}

			try
			{
				Clipboard.SetText(ActivityLogExportHelper.BuildClipboardText(selectedLogs));
				CustomDialogService.ShowSuccess(this, $"Copied {selectedLogs.Count} activity log(s) to clipboard.", "Copy Complete");
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error copying to clipboard: {ex.Message}", "Copy Error");
			}
		}
	}
}
