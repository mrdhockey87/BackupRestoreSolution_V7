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
	internal sealed class ActivityDetailForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly string? filterJobName;
		private readonly Label statusLabel;
		private readonly Label selectionCountLabel;
		private readonly ComboBox filterComboBox;
		private readonly DataGridView activitiesGrid;
		private readonly BindingSource bindingSource = new();
		private List<BackupLogEntry> currentLogs = new();

		public ActivityDetailForm()
			: this(null)
		{
		}

		public ActivityDetailForm(string? jobName)
		{
			filterJobName = string.IsNullOrWhiteSpace(jobName) ? null : jobName;
			Text = string.IsNullOrEmpty(filterJobName) ? "All Activity Details" : $"Activity Details - {filterJobName}";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(1000, 620);
			ClientSize = new Size(1000, 620);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = string.IsNullOrEmpty(filterJobName) ? "All Activities" : $"Activities for: {filterJobName}",
				Font = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(16, 16)
			};

			var topActions = new FlowLayoutPanel
			{
				Location = new Point(16, 48),
				Size = new Size(950, 36),
				WrapContents = true
			};

			var refreshButton = CreateButton("Refresh", (_, _) => LoadActivities());
			var exportCsvButton = CreateButton("Export CSV", (_, _) => ExportSelected("CSV"));
			var exportTextButton = CreateButton("Export Text", (_, _) => ExportSelected("Text"));
			var deleteButton = CreateButton("Delete Selected", (_, _) => DeleteSelectedActivities());
			var copyButton = CreateButton("Copy Selected", (_, _) => CopySelectedToClipboard());
			var selectAllButton = CreateButton("Select All", SelectAllActivities);
			var clearSelectionButton = CreateButton("Clear Selection", ClearActivitySelection);
			topActions.Controls.AddRange([refreshButton, exportCsvButton, exportTextButton, deleteButton, copyButton, selectAllButton, clearSelectionButton]);

			var filterLabel = new Label
			{
				Text = "Level:",
				AutoSize = true,
				Location = new Point(16, 96)
			};

			filterComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Location = new Point(66, 92),
				Size = new Size(140, 24)
			};
			filterComboBox.Items.AddRange(["All", "Info", "Success", "Warning", "Error"]);
			filterComboBox.SelectedIndexChanged += (_, _) => ApplyLevelFilter();
			filterComboBox.SelectedIndex = 0;

			selectionCountLabel = new Label
			{
				Text = "0 activities selected",
				AutoSize = true,
				Location = new Point(230, 96)
			};

			activitiesGrid = new DataGridView
			{
				Location = new Point(16, 128),
				Size = new Size(952, 430),
				ReadOnly = true,
				AllowUserToAddRows = false,
				AllowUserToDeleteRows = false,
				MultiSelect = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoGenerateColumns = false,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
			};
			activitiesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BackupLogEntry.Timestamp), HeaderText = "Timestamp", FillWeight = 140 });
			activitiesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BackupLogEntry.JobName), HeaderText = "Job", FillWeight = 120 });
			activitiesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BackupLogEntry.Level), HeaderText = "Level", FillWeight = 70 });
			activitiesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BackupLogEntry.Message), HeaderText = "Message", FillWeight = 220 });
			activitiesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BackupLogEntry.Details), HeaderText = "Details", FillWeight = 220 });
			activitiesGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BackupLogEntry.BackupPath), HeaderText = "Backup Path", FillWeight = 180 });
			activitiesGrid.DataSource = bindingSource;
			activitiesGrid.SelectionChanged += (_, _) => UpdateSelectionCount();

			var contextMenu = new ContextMenuStrip();
			contextMenu.Items.Add("Export CSV", null, (_, _) => ExportSelected("CSV"));
			contextMenu.Items.Add("Export Text", null, (_, _) => ExportSelected("Text"));
			contextMenu.Items.Add(new ToolStripSeparator());
			contextMenu.Items.Add("Copy Selected", null, (_, _) => CopySelectedToClipboard());
			contextMenu.Items.Add("Delete Selected", null, (_, _) => DeleteSelectedActivities());
			contextMenu.Items.Add(new ToolStripSeparator());
			contextMenu.Items.Add("Select All", null, (_, _) => activitiesGrid.SelectAll());
			contextMenu.Items.Add("Clear Selection", null, (_, _) => activitiesGrid.ClearSelection());
			activitiesGrid.ContextMenuStrip = contextMenu;

			statusLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 570),
				Size = new Size(952, 40),
				ForeColor = Color.DimGray
			};

			Controls.Add(titleLabel);
			Controls.Add(topActions);
			Controls.Add(filterLabel);
			Controls.Add(filterComboBox);
			Controls.Add(selectionCountLabel);
			Controls.Add(activitiesGrid);
			Controls.Add(statusLabel);

			if (IsInDesignMode)
			{
				LoadDesignTimeActivities();
			}
			else
			{
				Load += (_, _) => LoadActivities();
			}
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

		private static Button CreateButton(string text, EventHandler onClick)
		{
			var button = new Button
			{
				Text = text,
				AutoSize = true,
				MinimumSize = new Size(100, 30),
				Margin = new Padding(0, 0, 8, 0)
			};
			button.Click += onClick;
			return button;
		}

		private void SelectAllActivities(object? sender, EventArgs e)
		{
			activitiesGrid.SelectAll();
		}

		private void ClearActivitySelection(object? sender, EventArgs e)
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
