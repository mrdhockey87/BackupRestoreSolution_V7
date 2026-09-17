using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using SecureServerBackup.Helpers;

using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed class ActivityManagementForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly DataGridView jobsGrid;
		private readonly Label statusLabel;
		private readonly BindingSource bindingSource = new();
		private List<JobLogSummaryRow> currentRows = new();

		public ActivityManagementForm()
		{
			Text = "Backup Job Activity Logs";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(960, 560);
			ClientSize = new Size(960, 560);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Backup Job Activity Logs",
				Font = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(16, 16)
			};

			var actionsPanel = new FlowLayoutPanel
			{
				Location = new Point(16, 48),
				Size = new Size(920, 36),
				WrapContents = true
			};
			actionsPanel.Controls.Add(CreateButton("Refresh", (_, _) => LoadJobLogs()));
			actionsPanel.Controls.Add(CreateButton("View All Activities", (_, _) => OpenActivityDetails(null)));

			jobsGrid = new DataGridView
			{
				Location = new Point(16, 96),
				Size = new Size(920, 410),
				ReadOnly = true,
				AllowUserToAddRows = false,
				AllowUserToDeleteRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				MultiSelect = false,
				AutoGenerateColumns = false,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
			};
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.JobName), HeaderText = "Job Name", FillWeight = 180 });
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.TotalActivities), HeaderText = "Total", FillWeight = 70 });
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.LastActivity), HeaderText = "Last Activity", FillWeight = 120 });
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.SuccessCount), HeaderText = "Success", FillWeight = 70 });
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.WarningCount), HeaderText = "Warnings", FillWeight = 70 });
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.ErrorCount), HeaderText = "Errors", FillWeight = 70 });
			jobsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(JobLogSummaryRow.InfoCount), HeaderText = "Info", FillWeight = 70 });
			jobsGrid.DataSource = bindingSource;
			jobsGrid.CellDoubleClick += (_, e) =>
			{
				if (e.RowIndex >= 0)
				{
					OpenSelectedJobDetails();
				}
			};

			var contextMenu = new ContextMenuStrip();
			contextMenu.Items.Add("View Details", null, (_, _) => OpenSelectedJobDetails());
			contextMenu.Items.Add("Export Activities", null, (_, _) => ExportSelectedJobActivities());
			jobsGrid.ContextMenuStrip = contextMenu;

			var lowerActionsPanel = new FlowLayoutPanel
			{
				Location = new Point(16, 516),
				Size = new Size(920, 36),
				WrapContents = true
			};
			lowerActionsPanel.Controls.Add(CreateButton("View Details", (_, _) => OpenSelectedJobDetails()));
			lowerActionsPanel.Controls.Add(CreateButton("Export Activities", (_, _) => ExportSelectedJobActivities()));

			statusLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 556),
				Size = new Size(920, 24),
				ForeColor = Color.DimGray,
				Visible = false
			};

			Controls.Add(titleLabel);
			Controls.Add(actionsPanel);
			Controls.Add(jobsGrid);
			Controls.Add(lowerActionsPanel);
			Controls.Add(statusLabel);

			if (IsInDesignMode)
			{
				LoadDesignTimeRows();
			}
			else
			{
				Load += (_, _) => LoadJobLogs();
			}
		}

		private void LoadDesignTimeRows()
		{
			currentRows =
			[
				new JobLogSummaryRow
				{
					JobName = "Sample Backup",
					TotalActivities = 12,
					LastActivity = DateTime.Now.AddMinutes(-5),
					SuccessCount = 8,
					WarningCount = 1,
					ErrorCount = 0,
					InfoCount = 3
				},
				new JobLogSummaryRow
				{
					JobName = "Weekly Clone",
					TotalActivities = 4,
					LastActivity = DateTime.Now.AddHours(-2),
					SuccessCount = 2,
					WarningCount = 1,
					ErrorCount = 1,
					InfoCount = 0
				}
			];

			bindingSource.DataSource = currentRows;
			statusLabel.Visible = true;
			statusLabel.Text = $"Found {currentRows.Count} backup jobs with activity logs";
		}

		private static Button CreateButton(string text, EventHandler onClick)
		{
			var button = new Button
			{
				Text = text,
				AutoSize = true,
				MinimumSize = new Size(120, 30),
				Margin = new Padding(0, 0, 8, 0)
			};
			button.Click += onClick;
			return button;
		}

		private void LoadJobLogs()
		{
			try
			{
				List<BackupLogEntry> allLogs = BackupLogger.GetRecentLogs(10000);
				currentRows = allLogs
					.Where(log => !string.IsNullOrEmpty(log.JobName))
					.GroupBy(log => log.JobName)
					.Select(group => new JobLogSummaryRow
					{
						JobName = group.Key ?? "Unknown",
						TotalActivities = group.Count(),
						LastActivity = group.Max(log => log.Timestamp),
						SuccessCount = group.Count(log => log.Level == BackupLogLevel.Success),
						WarningCount = group.Count(log => log.Level == BackupLogLevel.Warning),
						ErrorCount = group.Count(log => log.Level == BackupLogLevel.Error),
						InfoCount = group.Count(log => log.Level == BackupLogLevel.Info)
					})
					.OrderByDescending(row => row.LastActivity)
					.ToList();

				bindingSource.DataSource = currentRows;
				statusLabel.Visible = true;
				statusLabel.Text = $"Found {currentRows.Count} backup jobs with activity logs";
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error loading job logs: {ex.Message}{Environment.NewLine}{Environment.NewLine}Details: {ex.StackTrace}", "Error");
			}
		}

		private JobLogSummaryRow? GetSelectedRow()
		{
			return jobsGrid.CurrentRow?.DataBoundItem as JobLogSummaryRow;
		}

		private void OpenSelectedJobDetails()
		{
			JobLogSummaryRow? selectedRow = GetSelectedRow();
			if (selectedRow == null || string.IsNullOrWhiteSpace(selectedRow.JobName))
			{
				CustomDialogService.ShowWarning(this, "Please select a job activity row first.", "No Selection");
				return;
			}

			OpenActivityDetails(selectedRow.JobName);
		}

		private void OpenActivityDetails(string? jobName)
		{
			try
			{
				using var detailForm = new ActivityDetailForm(jobName);
				detailForm.ShowDialog(this);
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error opening activity details: {ex.Message}{Environment.NewLine}{Environment.NewLine}Details: {ex.StackTrace}", "Error");
			}
		}

		private void ExportSelectedJobActivities()
		{
			JobLogSummaryRow? selectedRow = GetSelectedRow();
			if (selectedRow == null)
			{
				CustomDialogService.ShowWarning(this, "Please select a job activity row first.", "No Selection");
				return;
			}

			List<BackupLogEntry> allLogs = BackupLogger.GetRecentLogs(10000);
			List<BackupLogEntry> jobLogs = allLogs.Where(log => log.JobName == selectedRow.JobName).ToList();
			if (jobLogs.Count == 0)
			{
				CustomDialogService.ShowInfo(this, "No activities found for this job.", "No Data");
				return;
			}

			using var exportForm = new ExportOptionsForm();
			if (exportForm.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			try
			{
				bool exported = ActivityLogExportHelper.PromptAndExport(this, jobLogs, $"{selectedRow.JobName}_activities", exportForm.ExportFormat);
				if (exported)
				{
					CustomDialogService.ShowSuccess(this, $"Successfully exported {jobLogs.Count} activities.", "Export Complete");
				}
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error exporting activities: {ex.Message}", "Export Error");
			}
		}

		private sealed class JobLogSummaryRow
		{
			public string JobName { get; init; } = string.Empty;
			public int TotalActivities { get; init; }
			public DateTime LastActivity { get; init; }
			public int SuccessCount { get; init; }
			public int WarningCount { get; init; }
			public int ErrorCount { get; init; }
			public int InfoCount { get; init; }
		}
	}
}
