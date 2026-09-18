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
	internal sealed partial class ActivityManagementForm : Form
	{
		private const string ActionsColumnName = "actionsColumn";
		private const string ViewDetailsActionText = "View Details";
		private const string ExportActivitiesActionText = "Export Activities";
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly BindingSource bindingSource = new();
		private List<JobLogSummaryRow> currentRows = new();

		public ActivityManagementForm()
		{
			InitializeComponent();
			InitializeJobsGrid();

			if (IsInDesignMode)
			{
				LoadDesignTimeRows();
			}
			else
			{
				Load += ActivityManagementForm_Load;
			}
		}

		private void InitializeJobsGrid()
		{
			jobsGrid.DataSource = bindingSource;
			lastActivityColumn.DefaultCellStyle.Format = "g";
			totalActivitiesColumn.DefaultCellStyle.ForeColor = WinFormsThemeManager.PrimaryText;
			successCountColumn.DefaultCellStyle.ForeColor = WinFormsThemeManager.SuccessText;
			warningCountColumn.DefaultCellStyle.ForeColor = WinFormsThemeManager.WarningText;
			errorCountColumn.DefaultCellStyle.ForeColor = WinFormsThemeManager.ErrorText;
			actionsColumn.DefaultCellStyle.ForeColor = WinFormsThemeManager.PrimaryText;
		}

		private void ActivityManagementForm_Load(object? sender, EventArgs e)
		{
			LoadJobLogs();
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

		private void RefreshButton_Click(object? sender, EventArgs e)
		{
			LoadJobLogs();
		}

		private void ViewAllActivitiesButton_Click(object? sender, EventArgs e)
		{
			OpenActivityDetails(null);
		}

		private void JobsGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex != jobsGrid.Columns[ActionsColumnName].Index)
			{
				OpenSelectedJobDetails();
			}
		}

		private void JobsGrid_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || e.RowIndex < 0 || e.ColumnIndex != jobsGrid.Columns[ActionsColumnName].Index)
			{
				return;
			}

			JobLogSummaryRow? selectedRow = GetRow(e.RowIndex);
			if (selectedRow == null)
			{
				return;
			}

			Rectangle displayedCellBounds = jobsGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			(Rectangle viewDetailsBounds, Rectangle exportActivitiesBounds) = GetActionButtonBounds(new Rectangle(Point.Empty, displayedCellBounds.Size));
			Point clickPoint = new(e.X, e.Y);

			if (viewDetailsBounds.Contains(clickPoint))
			{
				OpenJobDetails(selectedRow);
				return;
			}

			if (exportActivitiesBounds.Contains(clickPoint))
			{
				ExportJobActivities(selectedRow);
			}
		}

		private void JobsGrid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex != jobsGrid.Columns[ActionsColumnName].Index)
			{
				return;
			}

			if (e.Graphics == null)
			{
				return;
			}

			e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
			(Rectangle viewDetailsBounds, Rectangle exportActivitiesBounds) = GetActionButtonBounds(e.CellBounds);
			Graphics graphics = e.Graphics;
			DrawActionButton(graphics, viewDetailsBounds, ViewDetailsActionText, WinFormsThemeManager.MediumTurquoise);
			DrawActionButton(graphics, exportActivitiesBounds, ExportActivitiesActionText, WinFormsThemeManager.ButtonBackground);
			e.Handled = true;
		}

		private void ViewDetailsToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			OpenSelectedJobDetails();
		}

		private void ExportActivitiesToolStripMenuItem_Click(object? sender, EventArgs e)
		{
			ExportSelectedJobActivities();
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

		private JobLogSummaryRow? GetRow(int rowIndex)
		{
			return rowIndex >= 0 && rowIndex < jobsGrid.Rows.Count
				? jobsGrid.Rows[rowIndex].DataBoundItem as JobLogSummaryRow
				: null;
		}

		private void OpenSelectedJobDetails()
		{
			JobLogSummaryRow? selectedRow = GetSelectedRow();
			if (selectedRow == null || string.IsNullOrWhiteSpace(selectedRow.JobName))
			{
				CustomDialogService.ShowWarning(this, "Please select a job activity row first.", "No Selection");
				return;
			}

			OpenJobDetails(selectedRow);
		}

		private void OpenJobDetails(JobLogSummaryRow selectedRow)
		{
			ArgumentNullException.ThrowIfNull(selectedRow);

			if (string.IsNullOrWhiteSpace(selectedRow.JobName))
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

			ExportJobActivities(selectedRow);
		}

		private void ExportJobActivities(JobLogSummaryRow selectedRow)
		{
			ArgumentNullException.ThrowIfNull(selectedRow);

			if (string.IsNullOrWhiteSpace(selectedRow.JobName))
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

		private static (Rectangle ViewDetailsBounds, Rectangle ExportActivitiesBounds) GetActionButtonBounds(Rectangle cellBounds)
		{
			const int horizontalPadding = 6;
			const int verticalPadding = 4;
			const int buttonSpacing = 6;
			int availableWidth = Math.Max(0, cellBounds.Width - (horizontalPadding * 2) - buttonSpacing);
			int viewDetailsWidth = Math.Max(0, availableWidth / 2);
			int exportActivitiesWidth = Math.Max(0, availableWidth - viewDetailsWidth);
			int buttonHeight = Math.Max(0, cellBounds.Height - (verticalPadding * 2));

			Rectangle viewDetailsBounds = new(
				cellBounds.X + horizontalPadding,
				cellBounds.Y + verticalPadding,
				viewDetailsWidth,
				buttonHeight);

			Rectangle exportActivitiesBounds = new(
				viewDetailsBounds.Right + buttonSpacing,
				cellBounds.Y + verticalPadding,
				exportActivitiesWidth,
				buttonHeight);

			return (viewDetailsBounds, exportActivitiesBounds);
		}

		private void DrawActionButton(Graphics graphics, Rectangle bounds, string text, Color backColor)
		{
			using SolidBrush backBrush = new(backColor);
			using Pen borderPen = new(ControlPaint.Dark(backColor));

			graphics.FillRectangle(backBrush, bounds);
			graphics.DrawRectangle(borderPen, bounds);

			TextRenderer.DrawText(
				graphics,
				text,
				jobsGrid.Font,
				bounds,
				WinFormsThemeManager.PrimaryText,
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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
