using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackup.Enums;
using SecureServerBackup.Helpers;

using SecureServerBackupCommon;

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace SecureServerBackup.WinForms
{
	internal sealed partial class MainForm : Form
	{
		private const string BackupJobCardTag = "BackupJobCard";

		private readonly JobManager jobManager = new();
		private ActivityManagementForm? activityManagementView;
		private MountBackupTabForm? mountBackupTabView;
		private ScheduleManagementForm? scheduleManagementView;

		public MainForm()
		{
			InitializeComponent();
			UiDispatcherHelper.Initialize();
			ConfigureMenu();
			ConfigureShell();
			MainMenuStrip = mainMenuStrip;
			WindowPositionManager.RestoreMainWindowPosition(this);
		}

		private void LoadBackupJobs()
		{
			var jobs = jobManager.GetAllJobs();

			backupJobsPanel.SuspendLayout();

			Control actionPanel = backupJobsPanel.Controls[0];
			backupJobsPanel.Controls.Clear();
			backupJobsPanel.Controls.Add(actionPanel);

			foreach (BackupJob job in jobs.OrderBy(job => job.Name))
			{
				backupJobsPanel.Controls.Add(CreateBackupJobCard(job));
			}

			emptyBackupJobsLabel.Visible = jobs.Count == 0;
			if (jobs.Count == 0)
			{
				backupJobsPanel.Controls.Add(emptyBackupJobsLabel);
			}

			backupJobsPanel.ResumeLayout();
			ResizeBackupJobCards();
		}

		private Control CreateBackupJobCard(BackupJob job)
		{
			var card = new Panel
			{
				Height = 188,
				BackColor = WinFormsThemeManager.PanelBackground,
				Margin = new Padding(0, 0, 0, 12),
				Padding = new Padding(14),
				Tag = BackupJobCardTag
			};
			card.Paint += (_, e) =>
			{
				ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle, WinFormsThemeManager.CardBorderColor, ButtonBorderStyle.Solid);
			};

			var root = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				ColumnCount = 2,
				RowCount = 1,
				Margin = new Padding(0)
			};
			root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));

			var infoLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				ColumnCount = 2,
				RowCount = 6,
				Margin = new Padding(0)
			};
			infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
			infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			infoLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			infoLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			infoLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			infoLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			infoLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			infoLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			var nameLabel = new Label
			{
				Text = job.Name,
				Font = new Font(Font, System.Drawing.FontStyle.Bold),
				AutoSize = true,
				Margin = new Padding(0, 0, 0, 10)
			};
			infoLayout.Controls.Add(nameLabel, 0, 0);
			infoLayout.SetColumnSpan(nameLabel, 2);

			AddInfoRow(infoLayout, 1, "Type:", GetBackupTypeDisplay(job), false);
			AddInfoRow(infoLayout, 2, "Source:", GetSourceSummary(job), true);
			AddInfoRow(infoLayout, 3, "Destination:", string.IsNullOrWhiteSpace(job.DestinationPath) ? "No destination selected" : job.DestinationPath, true);
			AddInfoRow(infoLayout, 4, "Schedule:", GetScheduleSummary(job), false);
			AddNextRunRow(infoLayout, 5, job);

			var actionsPanel = new FlowLayoutPanel
			{
				Dock = DockStyle.Top,
				FlowDirection = System.Windows.Forms.FlowDirection.TopDown,
				WrapContents = false,
				AutoSize = true,
				Margin = new Padding(10, 18, 0, 0)
			};

			actionsPanel.Controls.Add(CreateJobActionButton("Run Now", WinFormsThemeManager.ButtonBackground, async (_, _) => await RunBackupJobHelper.RunBackupJobAsync(job, this)));
			actionsPanel.Controls.Add(CreateJobActionButton("Edit", WinFormsThemeManager.ButtonBackground, (_, _) => EditBackupJob(job)));
			actionsPanel.Controls.Add(CreateJobActionButton("Delete", WinFormsThemeManager.DangerColor, (_, _) => DeleteBackupJob(job)));

			root.Controls.Add(infoLayout, 0, 0);
			root.Controls.Add(actionsPanel, 1, 0);

			card.Controls.Add(root);
			return card;
		}

		private static void AddInfoRow(TableLayoutPanel layout, int rowIndex, string labelText, string valueText, bool allowWrap)
		{
			var label = new Label
			{
				Text = labelText,
				AutoSize = true,
				Font = CreateMessageBoxFont(System.Drawing.FontStyle.Regular),
				Margin = new Padding(0, 0, 8, 8)
			};

			var value = new Label
			{
				Text = valueText,
				AutoSize = true,
				Font = CreateMessageBoxFont(System.Drawing.FontStyle.Bold),
				Margin = new Padding(0, 0, 0, 8),
				MaximumSize = allowWrap ? new System.Drawing.Size(560, 0) : System.Drawing.Size.Empty
			};

			layout.Controls.Add(label, 0, rowIndex);
			layout.Controls.Add(value, 1, rowIndex);
		}

		private static void AddNextRunRow(TableLayoutPanel layout, int rowIndex, BackupJob job)
		{
			var label = new Label
			{
				Text = "Next Run:",
				AutoSize = true,
				Margin = new Padding(0, 0, 8, 0)
			};

			var flow = new FlowLayoutPanel
			{
				AutoSize = true,
				WrapContents = false,
				FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
				Margin = new Padding(0)
			};

			flow.Controls.Add(new Label
			{
				Text = GetNextRunSummary(job),
				AutoSize = true,
				Font = CreateMessageBoxFont(System.Drawing.FontStyle.Bold),
				Margin = new Padding(0, 0, 18, 0)
			});

			flow.Controls.Add(new Label
			{
				Text = "Status:",
				AutoSize = true,
				Margin = new Padding(0, 0, 6, 0)
			});

			flow.Controls.Add(new Label
			{
				Text = job.IsCurrentlyRunning ? "Running" : "Idle",
				AutoSize = true
			});

			layout.Controls.Add(label, 0, rowIndex);
			layout.Controls.Add(flow, 1, rowIndex);
		}

		private static Font GetMessageBoxFont()
		{
			Font? font = System.Drawing.SystemFonts.MessageBoxFont ?? System.Drawing.SystemFonts.DefaultFont;
			if (font is null)
			{
				throw new InvalidOperationException("A default system font is required.");
			}

			return font;
		}

		private static Font CreateMessageBoxFont(System.Drawing.FontStyle style)
		{
			Font font = GetMessageBoxFont();
			return new Font(font.FontFamily, font.Size, style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
		}

		private void EditBackupJob(BackupJob job)
		{
			using var form = new BackupNewEditForm(job);
			form.ShowDialog(this);
			LoadBackupJobs();
			LoadVerifyBackups();
			LoadRestoreBackups();
		}

		private void DeleteBackupJob(BackupJob job)
		{
			CustomDialogResult result = CustomDialogService.ShowQuestion(this, 
					$"Are you sure you want to delete the backup job '{job.Name}'?",
					"Confirm Delete");

			if (result != CustomDialogResult.Yes)
			{
				return;
			}

			try
			{
				jobManager.DeleteJob(job.Id);
				LoadBackupJobs();
				LoadVerifyBackups();
				LoadRestoreBackups();

				CustomDialogService.ShowInfo(this,"Backup job deleted successfully.","Success");
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowInfo(this,$"Failed to delete backup job: {ex.Message}","Delete Error");
			}
		}
		public void ShowActivityTab()
		{
			OpenActivityManagement();
		}

		private void LoadVersion()
		{
			versionStatusLabel.Text = VersionClass.GetVersion();
		}

		private void ConfigureMenu()
		{
			mainMenuStrip.Items.Clear();
			mainMenuStrip.BackColor = WinFormsThemeManager.HeaderBackground;
			mainMenuStrip.RenderMode = ToolStripRenderMode.System;

			var fileMenu = new ToolStripMenuItem("&File");
			fileMenu.DropDownItems.Add(new ToolStripMenuItem("E&xit", null, (_, _) => Close()));

			var backupMenu = new ToolStripMenuItem("&Backup");
			backupMenu.DropDownItems.Add(new ToolStripMenuItem("&New Backup...", null, (_, _) => OpenNewBackup()));
			backupMenu.DropDownItems.Add(new ToolStripMenuItem("&Restore...", null, (_, _) => OpenRestore()));
			backupMenu.DropDownItems.Add(new ToolStripMenuItem("&Import Backup...", null, (_, _) => OpenImportBackup()));

			var serviceStatusMenu = new ToolStripMenuItem("Service &Status");
			serviceStatusMenu.DropDownItems.Add(new ToolStripMenuItem("&Manage Schedules...", null, (_, _) => OpenSchedules()));

			var activityMenu = new ToolStripMenuItem("&Activity");
			activityMenu.DropDownItems.Add(new ToolStripMenuItem("&Activity Management...", null, (_, _) => OpenActivityManagement()));

			var serviceMenu = new ToolStripMenuItem("Ser&vice");
			serviceMenu.DropDownItems.Add(new ToolStripMenuItem("Service &Management...", null, (_, _) => OpenServiceManagement()));
			serviceMenu.DropDownItems.Add(new ToolStripSeparator());
			serviceMenu.DropDownItems.Add(new ToolStripMenuItem("&Recovery Environment Creator...", null, (_, _) => OpenRecoveryEnvironment()));

			var helpMenu = new ToolStripMenuItem("&Help");
			helpMenu.DropDownItems.Add(new ToolStripMenuItem("&About...", null, (_, _) => OpenAbout()));

			mainMenuStrip.Items.AddRange(
			[
				fileMenu,
				backupMenu,
				serviceStatusMenu,
				activityMenu,
				serviceMenu,
				helpMenu
			]);

		}

		private void ConfigureShell()
		{
			BackColor = WinFormsThemeManager.WindowBackground;
			mainStatusStrip.BackColor = WinFormsThemeManager.StatusBarBackground;
			backupTabPage.BackColor = WinFormsThemeManager.WindowBackground;
			activityTabPage.BackColor = WinFormsThemeManager.WindowBackground;
			mountBackupsTabPage.BackColor = WinFormsThemeManager.WindowBackground;
			verifyTabPage.BackColor = WinFormsThemeManager.WindowBackground;
			restoreTabPage.BackColor = WinFormsThemeManager.WindowBackground;
			schedulesTabPage.BackColor = WinFormsThemeManager.WindowBackground;
			backupJobsPanel.BackColor = WinFormsThemeManager.PanelBackground;
			activityTabPanel.BackColor = WinFormsThemeManager.PanelBackground;
			restoreRootLayout.BackColor = WinFormsThemeManager.PanelBackground;
			versionStatusLabel.Text = "Version: Loading...";
			restoreStatusLabel.MaximumSize = new System.Drawing.Size(900, 0);

			ConfigurePlaceholderDataGridView(verifyBackupsDataGridiew);
			ConfigurePlaceholderDataGridView(restoreBackupsDataGridView);
			ConfigureBackupDataGridView(verifyBackupsDataGridiew, "Verify");
			ConfigureBackupDataGridView(restoreBackupsDataGridView, "Restore");
			verifyBackupsDataGridiew.CellContentClick += VerifyBackupsListView_CellContentClick;
			restoreBackupsDataGridView.CellContentClick += RestoreBackupsListView_CellContentClick;

			backupJobsPanel.Controls.Clear();
			backupJobsPanel.Controls.Add(BuildBackupActionPanel());
			backupJobsPanel.Controls.Add(emptyBackupJobsLabel);

			activityTabPanel.Controls.Clear();

			restoreActionsPanel.Controls.Clear();
			restoreActionsPanel.Controls.Add(CreateActionButton("Refresh", (_, _) => LoadRestoreBackups()));
			restoreActionsPanel.Controls.Add(CreateActionButton("Browse .ssb...", (_, _) => BrowseRestoreBackup()));
			restoreActionsPanel.Controls.Add(CreateActionButton("Restore Selected", (_, _) => OpenSelectedRestoreBackup()));
			
			verifyActionsPanel.Controls.Clear();
			verifyActionsPanel.Controls.Add(CreateActionButton("Refresh", (_, _) => LoadVerifyBackups()));
			verifyActionsPanel.Controls.Add(CreateActionButton("Browse .ssb...", (_, _) => BrowseVerifyBackup()));
			verifyActionsPanel.Controls.Add(CreateActionButton("Verify Selected", async (_, _) => await VerifySelectedBackupAsync()));
		}

		private Control BuildBackupActionPanel()
		{
			var panel = new TableLayoutPanel
			{
				AutoSize = true,
				ColumnCount = 2,
				RowCount = 1,
				Margin = new Padding(0, 0, 0, 12),
				Padding = new Padding(10, 10, 10, 8),
				BackColor = WinFormsThemeManager.PanelBackground
			};

			panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

			var titleLabel = new Label
			{
				Text = "Backup Jobs",
				Font = new Font(Font, System.Drawing.FontStyle.Bold),
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 8, 18, 0)
			};

			var buttonsPanel = new FlowLayoutPanel
			{
				AutoSize = true,
				WrapContents = false,
				FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0)
			};

			buttonsPanel.Controls.Add(CreateHeaderActionButton("New Backup...", (_, _) => OpenNewBackup(), 130));
			buttonsPanel.Controls.Add(CreateHeaderActionButton("Import Backup...", (_, _) => OpenImportBackup(), 130));
			buttonsPanel.Controls.Add(CreateHeaderActionButton("Refresh", (_, _) =>
			{
				LoadBackupJobs();
				LoadVerifyBackups();
				LoadRestoreBackups();
			}, 100));

			panel.Controls.Add(titleLabel, 0, 0);
			panel.Controls.Add(buttonsPanel, 1, 0);

			return panel;
		}

		private void ActivityTabPage_Enter(object? sender, EventArgs e)
		{
			EnsureActivityManagementLoaded();
			EnsureSelectedTabFits();
		}

		private void MainTabControl_SelectedIndexChanged(object? sender, EventArgs e)
		{
			if (mainTabControl.SelectedTab == backupTabPage)
			{
				LoadBackupJobs();
			}
			else if (mainTabControl.SelectedTab == activityTabPage)
			{
				EnsureActivityManagementLoaded();
			}
			else if (mainTabControl.SelectedTab == mountBackupsTabPage)
			{
				EnsureMountBackupTabLoaded();
			}
			else if (mainTabControl.SelectedTab == verifyTabPage)
			{
				LoadVerifyBackups();
			}
			else if (mainTabControl.SelectedTab == restoreTabPage)
			{
				LoadRestoreBackups();
			}
			else if (mainTabControl.SelectedTab == schedulesTabPage)
			{
				EnsureScheduleManagementLoaded();
			}

			EnsureSelectedTabFits();
		}

		private void EnsureMountBackupTabLoaded()
		{
			if (mountBackupTabView != null && !mountBackupTabView.IsDisposed && mountBackupsTabPage.Controls.Contains(mountBackupTabView))
			{
				return;
			}

			mountBackupsTabPage.SuspendLayout();

			try
			{
				mountBackupTabView?.Dispose();
				mountBackupsTabPage.Controls.Clear();

				MountBackupTabForm childForm = new();
				childForm.ConfigureForEmbeddedHost();
				childForm.TopLevel = false;
				childForm.FormBorderStyle = FormBorderStyle.None;
				childForm.Dock = DockStyle.Fill;

				mountBackupTabView = childForm;
				mountBackupsTabPage.Controls.Add(childForm);
				childForm.Show();
			}
			finally
			{
				mountBackupsTabPage.ResumeLayout();
			}

			EnsureSelectedTabFits();
		}

		private void EnsureActivityManagementLoaded()
		{
			if (activityManagementView != null && !activityManagementView.IsDisposed && activityTabPanel.Controls.Contains(activityManagementView))
			{
				return;
			}

			activityTabPanel.SuspendLayout();

			try
			{
				activityManagementView?.Dispose();
				activityTabPanel.Controls.Clear();

				ActivityManagementForm childForm = new();
				childForm.TopLevel = false;
				childForm.FormBorderStyle = FormBorderStyle.None;
				childForm.Dock = DockStyle.Fill;

				activityManagementView = childForm;
				activityTabPanel.Controls.Add(childForm);
				childForm.Show();
			}
			finally
			{
				activityTabPanel.ResumeLayout();
			}

			EnsureSelectedTabFits();
		}

		private void EnsureSelectedTabFits()
		{
			if (mainTabControl.SelectedTab == null)
			{
				return;
			}

			Rectangle displayRectangle = mainTabControl.DisplayRectangle;
			if (displayRectangle.Width <= 0 || displayRectangle.Height <= 0)
			{
				return;
			}

			System.Drawing.Size requiredContentSize = GetRequiredSelectedTabContentSize();
			if (requiredContentSize.Width <= 0 || requiredContentSize.Height <= 0)
			{
				return;
			}

			int additionalWidth = Math.Max(0, requiredContentSize.Width - displayRectangle.Width);
			int additionalHeight = Math.Max(0, requiredContentSize.Height - displayRectangle.Height);
			if (additionalWidth == 0 && additionalHeight == 0)
			{
				return;
			}

			System.Drawing.Size targetClientSize = new(
				ClientSize.Width + additionalWidth,
				ClientSize.Height + additionalHeight);

			System.Drawing.Size targetFormSize = SizeFromClientSize(targetClientSize);
			MinimumSize = new System.Drawing.Size(
				Math.Max(MinimumSize.Width, targetFormSize.Width),
				Math.Max(MinimumSize.Height, targetFormSize.Height));

			if (Width < targetFormSize.Width || Height < targetFormSize.Height)
			{
				Size = new System.Drawing.Size(
					Math.Max(Width, targetFormSize.Width),
					Math.Max(Height, targetFormSize.Height));
			}
		}

		private void EnsureScheduleManagementLoaded()
		{
			if (scheduleManagementView != null && !scheduleManagementView.IsDisposed && schedulesTabPanel.Controls.Contains(scheduleManagementView))
			{
				return;
			}

			schedulesTabPanel.SuspendLayout();

			try
			{
				scheduleManagementView?.Dispose();
				schedulesTabPanel.Controls.Clear();

				ScheduleManagementForm childForm = new();
				childForm.ConfigureForEmbeddedHost();
				childForm.TopLevel = false;
				childForm.FormBorderStyle = FormBorderStyle.None;
				childForm.Dock = DockStyle.Fill;

				scheduleManagementView = childForm;
				schedulesTabPanel.Controls.Add(childForm);
				childForm.Show();
			}
			finally
			{
				schedulesTabPanel.ResumeLayout();
			}
		}

		private System.Drawing.Size GetRequiredSelectedTabContentSize()
		{
			if (mainTabControl.SelectedTab == activityTabPage && activityManagementView != null && !activityManagementView.IsDisposed)
			{
				return new System.Drawing.Size(
					activityManagementView.MinimumSize.Width + activityTabPanel.Padding.Horizontal + activityTabPage.Padding.Horizontal,
					activityManagementView.MinimumSize.Height + activityTabPanel.Padding.Vertical + activityTabPage.Padding.Vertical);
			}

			if (mainTabControl.SelectedTab == schedulesTabPage && scheduleManagementView != null && !scheduleManagementView.IsDisposed)
			{
				return new System.Drawing.Size(
					scheduleManagementView.MinimumSize.Width + schedulesTabPanel.Padding.Horizontal + schedulesTabPage.Padding.Horizontal,
					scheduleManagementView.MinimumSize.Height + schedulesTabPanel.Padding.Vertical + schedulesTabPage.Padding.Vertical);
			}

			if (mainTabControl.SelectedTab == mountBackupsTabPage && mountBackupTabView != null && !mountBackupTabView.IsDisposed)
			{
				System.Drawing.Size hostedSize = mountBackupTabView.MinimumSize;
				if (hostedSize.Width <= 0 || hostedSize.Height <= 0)
				{
					hostedSize = mountBackupTabView.Size;
				}

				return new System.Drawing.Size(
					hostedSize.Width + mountBackupsTabPage.Padding.Horizontal,
					hostedSize.Height + mountBackupsTabPage.Padding.Vertical);
			}

			return System.Drawing.Size.Empty;
		}

		private static void ConfigurePlaceholderDataGridView(DataGridView dataGridView)
		{
			dataGridView.Dock = DockStyle.Fill;
			dataGridView.AllowUserToAddRows = false;
			dataGridView.AllowUserToDeleteRows = false;
			dataGridView.AllowUserToResizeRows = false;
			dataGridView.AutoGenerateColumns = false;
			dataGridView.MultiSelect = false;
			dataGridView.ReadOnly = true;
			dataGridView.RowHeadersVisible = false;
			dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView.BackgroundColor = WinFormsThemeManager.VeryLightTurquoise;
			dataGridView.GridColor = WinFormsThemeManager.BorderColor;
			dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridView.EnableHeadersVisualStyles = false;
			dataGridView.Font = CreateMessageBoxFont(System.Drawing.FontStyle.Regular);
		}

		private static void ConfigureBackupDataGridView(DataGridView dataGridView, string actionText)
		{
			dataGridView.Columns.Clear();
			dataGridView.Columns.Add("BackupName", "Backup Name");
			dataGridView.Columns["BackupName"].Width = 180;
			dataGridView.Columns.Add("Type", "Type");
			dataGridView.Columns["Type"].Width = 110;
			dataGridView.Columns.Add("Date", "Date");
			dataGridView.Columns["Date"].Width = 140;
			dataGridView.Columns.Add("Encrypted", "Encrypted");
			dataGridView.Columns["Encrypted"].Width = 90;
			dataGridView.Columns.Add("Path", "Path");
			dataGridView.Columns["Path"].Width = 200;

			var actionColumn = new DataGridViewButtonColumn
			{
				Name = "Action",
				HeaderText = "Action",
				Text = actionText,
				UseColumnTextForButtonValue = true,
				Width = 90,
				ReadOnly = true
			};
			dataGridView.Columns.Add(actionColumn);
		}

		private static Button CreateActionButton(string text, EventHandler onClick)
		{
			var button = new Button
			{
				Text = text,
				AutoSize = true,
				MinimumSize = new System.Drawing.Size(130, 32),
				Margin = new Padding(0, 0, 8, 8)
			};

			button.Click += onClick;
			return button;
		}

		private static AvailableBackupInfo CreateAvailableBackupInfo(BackupJob job, string backupPath)
		{
			ArgumentNullException.ThrowIfNull(job);
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);

			DateTime backupDate = File.Exists(backupPath)
				? new FileInfo(backupPath).LastWriteTime
				: Directory.GetLastWriteTime(backupPath);

			return new AvailableBackupInfo
			{
				BackupName = job.Name,
				BackupType = job.Type.ToString(),
				BackupDate = backupDate,
				BackupPath = backupPath,
				IsEncrypted = File.Exists(backupPath) && BackupEncryptionService.IsEncryptedBackupFile(backupPath),
				ProtectedEncryptionPassword = job.ProtectedEncryptionPassword
			};
		}

		private static AvailableBackupInfo CreateAdHocBackupInfo(string backupPath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);

			var fileInfo = new FileInfo(backupPath);
			string backupName = Path.GetFileNameWithoutExtension(backupPath);

			return new AvailableBackupInfo
			{
				BackupName = backupName,
				BackupType = GetBackupTypeFromFilename(backupName),
				BackupDate = fileInfo.LastWriteTime,
				BackupPath = backupPath,
				IsEncrypted = BackupEncryptionService.IsEncryptedBackupFile(backupPath)
			};
		}

		private System.Collections.Generic.List<AvailableBackupInfo> GetAvailableBackups()
		{
			var backups = new System.Collections.Generic.List<AvailableBackupInfo>();

			foreach (BackupJob job in jobManager.GetAllJobs())
			{
				if (string.IsNullOrWhiteSpace(job.DestinationPath) || !Directory.Exists(job.DestinationPath))
				{
					continue;
				}

				System.Collections.Generic.IEnumerable<string> backupEntries = Directory.EnumerateFileSystemEntries(job.DestinationPath, "*.ssb", SearchOption.AllDirectories);
				foreach (string backupPath in GroupRestoreBackupEntries(job, backupEntries))
				{
					backups.Add(CreateAvailableBackupInfo(job, backupPath));
				}
			}

			return backups
				.OrderByDescending(backup => backup.BackupDate)
				.ThenBy(backup => backup.BackupName, StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		private static void PopulateBackupListView(
			DataGridView dataGridView,
			System.Collections.Generic.IEnumerable<AvailableBackupInfo> backups,
			Func<AvailableBackupInfo, string>? statusSelector = null,
			Func<AvailableBackupInfo, string>? detailsSelector = null,
			string? actionLabel = null)
		{
			ArgumentNullException.ThrowIfNull(dataGridView);
			ArgumentNullException.ThrowIfNull(backups);

			dataGridView.SuspendLayout();
			dataGridView.Rows.Clear();

			foreach (AvailableBackupInfo backup in backups)
			{
				int rowIndex = dataGridView.Rows.Add();
				DataGridViewRow row = dataGridView.Rows[rowIndex];
				row.Tag = backup;
				row.Cells["BackupName"].Value = backup.BackupName;
				row.Cells["Type"].Value = backup.BackupType;
				row.Cells["Date"].Value = backup.BackupDate.ToString("g");
				row.Cells["Encrypted"].Value = backup.EncryptionStatus;

				if (statusSelector != null)
				{
					row.Cells["Status"].Value = statusSelector(backup);
				}

				if (detailsSelector != null)
				{
					row.Cells["Details"].Value = detailsSelector(backup);
				}

				row.Cells["Path"].Value = backup.BackupPath;
				row.Cells["Action"].Value = actionLabel ?? string.Empty;
			}

			dataGridView.ResumeLayout();
		}

		private static AvailableBackupInfo? GetSelectedBackup(DataGridView dataGridView)
		{
			ArgumentNullException.ThrowIfNull(dataGridView);

			return dataGridView.SelectedRows.Count > 0 && dataGridView.SelectedRows[0].Tag is AvailableBackupInfo backup
				? backup
				: null;
		}

		private static void AddBackupToListView(
			DataGridView dataGridView,
			AvailableBackupInfo backup,
			Func<AvailableBackupInfo, string>? statusSelector = null,
			Func<AvailableBackupInfo, string>? detailsSelector = null)
		{
			ArgumentNullException.ThrowIfNull(dataGridView);
			ArgumentNullException.ThrowIfNull(backup);

			int rowIndex = dataGridView.Rows.Add();
			DataGridViewRow row = dataGridView.Rows[rowIndex];
			row.Tag = backup;
			row.Cells["BackupName"].Value = backup.BackupName;
			row.Cells["Type"].Value = backup.BackupType;
			row.Cells["Date"].Value = backup.BackupDate.ToString("g");
			row.Cells["Encrypted"].Value = backup.EncryptionStatus;
			row.Cells["Path"].Value = backup.BackupPath;
			row.Cells["Action"].Value = "Verify";

			if (statusSelector != null)
			{
				row.Cells["Status"].Value = statusSelector(backup);
			}

			if (detailsSelector != null)
			{
				row.Cells["Details"].Value = detailsSelector(backup);
			}
		}

		private static bool TrySelectBackupByPath(DataGridView dataGridView, string backupPath)
		{
			ArgumentNullException.ThrowIfNull(dataGridView);
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);

			foreach (DataGridViewRow row in dataGridView.Rows)
			{
				if (row.Tag is AvailableBackupInfo backup && string.Equals(backup.BackupPath, backupPath, StringComparison.OrdinalIgnoreCase))
				{
					row.Selected = true;
					dataGridView.FirstDisplayedScrollingRowIndex = row.Index;
					return true;
				}
			}

			return false;
		}

		private void LoadVerifyBackups()
		{
			try
			{
				System.Collections.Generic.List<AvailableBackupInfo> backups = GetAvailableBackups();
				PopulateBackupListView(verifyBackupsDataGridiew, backups, actionLabel: "Verify");
				verifyStatusLabel.Text = backups.Count == 0
					? "No backups were found. Create a backup first, then return here to verify it."
					: "Select a backup and click Verify Selected to validate the archive before restoring it.";
			}
			catch (Exception ex)
			{
				verifyBackupsDataGridiew.Rows.Clear();
				verifyStatusLabel.Text = $"Error loading verify backups: {ex.Message}";
			}
		}

		private void BrowseVerifyBackup()
		{
			using var openFileDialog = new OpenFileDialog
			{
				Title = "Select Backup File to Verify",
				Filter = "Secure Server Backup Files (*.ssb)|*.ssb|All Files (*.*)|*.*",
				DefaultExt = "ssb",
				Multiselect = false,
				CheckFileExists = true
			};

			if (openFileDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			string selectedFile = openFileDialog.FileName;
			if (TrySelectBackupByPath(verifyBackupsDataGridiew, selectedFile))
			{
				verifyStatusLabel.Text = "Selected the existing backup entry. Click Verify Selected to validate it.";
				return;
			}

			AvailableBackupInfo backupInfo = CreateAdHocBackupInfo(selectedFile);
			AddBackupToListView(verifyBackupsDataGridiew, backupInfo);
			TrySelectBackupByPath(verifyBackupsDataGridiew, selectedFile);
			verifyStatusLabel.Text = $"Backup file added for verify: {Path.GetFileName(selectedFile)}";
		}

		private async System.Threading.Tasks.Task VerifySelectedBackupAsync()
		{
			AvailableBackupInfo? backup = GetSelectedBackup(verifyBackupsDataGridiew);
			if (backup == null)
			{
				CustomDialogService.ShowWarning(this, "Please select a backup to verify.", "No Selection");
				return;
			}
			VerifyBackupHelper vh = new VerifyBackupHelper(backup);
			await vh.VerifyBackup(this, verifyStatusLabel);
			/*
			PreparedBackupFile? preparedBackup = null;

			try
			{
				verifyStatusLabel.Text = $"Preparing {backup.BackupName} for verify...";
				preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms(this, backup.BackupPath, backup.BackupName, backup.ProtectedEncryptionPassword);
				(bool success, string message) = await BackupValidator.ValidateBackupAsync(preparedBackup.WorkingPath, $"{backup.BackupName} [Verify]", backupSucceeded: true);

				verifyStatusLabel.Text = success
					? $"Verify completed successfully for {backup.BackupName}."
					: $"Verify failed for {backup.BackupName}.";

				if (success)
				{
					CustomDialogService.ShowSuccess(this, message, "Verify Complete");
					return;
				}

				CustomDialogService.ShowError(this, message, "Verify Failed");
			}
			catch (OperationCanceledException)
			{
				verifyStatusLabel.Text = "Verify cancelled.";
			}
			catch (Exception ex)
			{
				verifyStatusLabel.Text = $"Verify failed for {backup.BackupName}.";
				CustomDialogService.ShowError(this, $"Failed to verify backup:{Environment.NewLine}{ex.Message}", "Verify Failed");
			}
			finally
			{
				preparedBackup?.Dispose();
			}*/
		}

		private Button CreateHeaderActionButton(string text, EventHandler onClick, int width)
		{
			var button = new Button
			{
				Text = text,
				Width = width,
				Height = 32,
				Margin = new Padding(0, 0, 10, 0)
			};

			WinFormsThemeManager.ApplyButtonTheme(button, WinFormsThemeManager.ButtonBackground);
			button.Click += onClick;
			return button;
		}

		private Button CreateJobActionButton(string text, Color backColor, EventHandler onClick)
		{
			var button = new Button
			{
				Text = text,
				Width = 130,
				Height = 34,
				Margin = new Padding(0, 0, 0, 10)
			};

			WinFormsThemeManager.ApplyButtonTheme(button, backColor);
			button.Click += onClick;
			return button;
		}

		private void MainTabControl_DrawItem(object? sender, DrawItemEventArgs e)
		{
			TabPage page = mainTabControl.TabPages[e.Index];
			bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
			Rectangle bounds = e.Bounds;

			using var backBrush = new SolidBrush(isSelected ? WinFormsThemeManager.MediumTurquoise : WinFormsThemeManager.VeryLightTurquoise);
			using var borderPen = new Pen(WinFormsThemeManager.BorderColor);
			Font tabFont = isSelected ? new Font(Font, System.Drawing.FontStyle.Bold) : Font;

			e.Graphics.FillRectangle(backBrush, bounds);
			e.Graphics.DrawRectangle(borderPen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

			TextRenderer.DrawText(
				e.Graphics,
				page.Text,
				tabFont,
				bounds,
				WinFormsThemeManager.PrimaryText,
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

			if (isSelected)
			{
				tabFont.Dispose();
			}
		}

		private void ResizeBackupJobCards()
		{
			int width = Math.Max(backupJobsPanel.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 10, 320);

			foreach (Control control in backupJobsPanel.Controls)
			{
				if (control is Panel card && string.Equals(card.Tag as string, BackupJobCardTag, StringComparison.Ordinal))
				{
					card.Width = width;
				}
			}
		}

		private static string GetBackupTypeDisplay(BackupJob job)
		{
			return job.Type switch
			{
				BackupType.Full => "Full",
				BackupType.Incremental => "Full then Incremental",
				BackupType.Differential => "Full then Differential",
				BackupType.SelectedFilesAndFolders => "Selected Files & Folder",
				BackupType.CloneToDisk => "Clone to Disk",
				BackupType.CloneToVirtualDisk => "Clone to Virtual Disk (Hyper-V)",
				BackupType.CloneHyperVSystem => "Clone Hyper-V System",
				BackupType.ExportHyperVSystem => "Export Hyper-V System",
				_ => job.Type.ToString()
			};
		}

		private static string GetSourceSummary(BackupJob job)
		{
			string[] hyperVNames = (job.HyperVMachines ?? new System.Collections.Generic.List<string>())
				.Where(name => !string.IsNullOrWhiteSpace(name))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();

			if (hyperVNames.Length > 0)
			{
				return string.Join(", ", hyperVNames);
			}

			string[] sources = (job.SourcePaths ?? new System.Collections.Generic.List<string>())
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();

			if (sources.Length == 0)
			{
				return "No source selected";
			}

			if (sources.Length == 1)
			{
				return sources[0];
			}

			return $"{sources[0]} (+{sources.Length - 1} more)";
		}

		private static string GetScheduleSummary(BackupJob job)
		{
			if (job.Schedule == null || !job.Schedule.Enabled)
			{
				return "No schedule (manual only)";
			}

			string timeText = DateTime.Today.Add(job.Schedule.Time).ToString("h:mm tt");

			return job.Schedule.Frequency switch
			{
				ScheduleFrequency.Daily => $"Daily at {timeText}",
				ScheduleFrequency.Weekly => job.Schedule.DaysOfWeek == null || job.Schedule.DaysOfWeek.Count == 0
					? $"Weekly at {timeText}"
					: $"Weekly on {string.Join(", ", job.Schedule.DaysOfWeek)} at {timeText}",
				ScheduleFrequency.Monthly => $"Monthly on day {job.Schedule.DayOfMonth} at {timeText}",
				ScheduleFrequency.Once => $"Once at {timeText}",
				_ => "Scheduled"
			};
		}

		private static string GetNextRunSummary(BackupJob job)
		{
			DateTime? nextRun = job.NextScheduledRun ?? job.Schedule?.NextRunTime;
			return nextRun.HasValue ? nextRun.Value.ToString("g") : "Not scheduled";
		}

		private void OpenNewBackup()
		{
			using var form = new BackupNewEditForm();
			form.ShowDialog(this);
			LoadBackupJobs();
			LoadVerifyBackups();
			LoadRestoreBackups();
		}

		private void OpenRestore()
		{
			if (restoreBackupsDataGridView.SelectedRows.Count > 0)
			{
				OpenSelectedRestoreBackup();
				return;
			}

			LoadRestoreBackups();
			mainTabControl.SelectedTab = mainTabControl.TabPages.Cast<TabPage>().FirstOrDefault(page => page.Text == "Restore") ?? mainTabControl.SelectedTab;
		}

		private void LoadRestoreBackups()
		{
			try
			{
				System.Collections.Generic.List<AvailableBackupInfo> backups = GetAvailableBackups();
				PopulateBackupListView(restoreBackupsDataGridView, backups, actionLabel: "Restore");

				emptyRestoreBackupsLabel.Visible = backups.Count == 0;
				restoreStatusLabel.Text = backups.Count == 0
					? "No backups were found. Create a backup first, then return here to restore it."
					: "Select a backup to open the restore workflow. Restoring the boot/system drive requires the recovery environment.";
			}
			catch (Exception ex)
			{
				restoreBackupsDataGridView.Rows.Clear();
				emptyRestoreBackupsLabel.Visible = true;
				restoreStatusLabel.Text = $"Error loading restore backups: {ex.Message}";
			}
		}

		private void BrowseRestoreBackup()
		{
			using var openFileDialog = new OpenFileDialog
			{
				Title = "Select Backup File to Restore",
				Filter = "Secure Server Backup Files (*.ssb)|*.ssb|All Files (*.*)|*.*",
				DefaultExt = "ssb",
				Multiselect = false,
				CheckFileExists = true
			};

			if (openFileDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			string selectedFile = openFileDialog.FileName;
			bool exists = restoreBackupsDataGridView.Rows.Cast<DataGridViewRow>()
				.Any(row => row.Tag is AvailableBackupInfo backup && backup.BackupPath.Equals(selectedFile, StringComparison.OrdinalIgnoreCase));
			if (exists)
			{
				CustomDialogService.ShowInfo(this, "This backup is already in the list.", "Already Added");
				return;
			}

			AvailableBackupInfo backupInfo = CreateAdHocBackupInfo(selectedFile);
			int rowIndex = restoreBackupsDataGridView.Rows.Add();
			DataGridViewRow row = restoreBackupsDataGridView.Rows[rowIndex];
			row.Tag = backupInfo;
			row.Cells["BackupName"].Value = backupInfo.BackupName;
			row.Cells["Type"].Value = backupInfo.BackupType;
			row.Cells["Date"].Value = backupInfo.BackupDate.ToString("g");
			row.Cells["Encrypted"].Value = backupInfo.EncryptionStatus;
			row.Cells["Path"].Value = backupInfo.BackupPath;
			row.Cells["Action"].Value = "Restore";
			emptyRestoreBackupsLabel.Visible = false;
			restoreStatusLabel.Text = "Select a backup to open the restore workflow. Restoring the boot/system drive requires the recovery environment.";
			CustomDialogService.ShowInfo(this, $"Backup file added for restore: {Path.GetFileName(selectedFile)}", "Backup Added");
		}

		private void OpenSelectedRestoreBackup()
		{
			if (restoreBackupsDataGridView.SelectedRows.Count == 0 || restoreBackupsDataGridView.SelectedRows[0].Tag is not AvailableBackupInfo backup)
			{
				CustomDialogService.ShowWarning(this, "Please select a valid backup to restore.", "Restore Error");
				return;
			}

			OpenRestoreWorkflow(backup);
		}

		private async void OpenRestoreWorkflow(AvailableBackupInfo backup)
		{
			try
			{
				CheckBackupServiceHelper cbsh = new(this);
				bool serviceOk = await cbsh.CheckBackupServiceAsync();
				if (!serviceOk)  return;
				bool requireAlternateDestination = false;
				if (IsBootRelatedBackup(backup))
				{
					CustomDialogResult bootRestoreDecision = CustomDialogService.Show(this,
						"This backup includes the currently booted disk/volume.\n\nDo you want to restore it to a non-boot disk/volume from Windows?",
						"Boot Drive Restore Detected",
						DialogButtons.YesNoCancel,
						DialogIcon.Warning);

					if (bootRestoreDecision == CustomDialogResult.No)
					{
						CustomDialogService.ShowWarning(this, "To restore the currently booted disk/volume in place, boot from the recovery disk and perform the restore from there.", "Recovery Disk Required");
						return;
					}

					if (bootRestoreDecision != CustomDialogResult.Yes)
					{
						return;
					}

					requireAlternateDestination = true;
				}

				RestoreSelectionContext? restoreSelection = PromptForRestoreSelection(backup, requireAlternateDestination);
				if (restoreSelection == null)
				{
					return;
				}

				if (RestoreWorkflowHelper.DetermineRestoreTargetKind(
					restoreSelection.RestorePoint.BackupType,
					restoreSelection.RestorePoint.FilePath,
					restoreSelection.ScopeKind == RestoreScopeKind.SelectedItems && restoreSelection.SelectedItems.Count > 0
						? restoreSelection.SelectedItems
						: RestoreWorkflowHelper.GetBackupItemsForRestorePoint(restoreSelection.RestorePoint)) == RestoreTargetKind.FileOrFolder)
				{
					using var restoreForm = new RestoreFileSelectionForm(restoreSelection);
					restoreForm.ShowDialog(this);
					return;
				}

				using var restoreImageForm = new RestoreImageSelectionForm(restoreSelection);
				restoreImageForm.ShowDialog(this);
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Error opening restore workflow: {ex.Message}", "Restore Error");
			}
		}

		private RestoreSelectionContext? PromptForRestoreSelection(AvailableBackupInfo backup, bool requireAlternateDestination)
		{
			using var restorePointForm = new RestorePointSelectionForm(backup);
			if (restorePointForm.ShowDialog(this) != DialogResult.OK || restorePointForm.SelectedRestorePoint == null)
			{
				return null;
			}

			System.Collections.Generic.IReadOnlyList<RestorePoint> restorePoints = RestoreWorkflowHelper.GetRestorePointsForBackup(backup.BackupPath);
			RestorePoint selectedRestorePoint = restorePointForm.SelectedRestorePoint;
			bool isGroupedRestore = restorePoints.Count == 1;
			if (!isGroupedRestore)
			{
				return new RestoreSelectionContext
				{
					Backup = backup,
					RestorePoint = selectedRestorePoint,
					RequireAlternateDestination = requireAlternateDestination,
					ScopeKind = RestoreScopeKind.All
				};
			}

			CustomDialogResult restoreAllDecision = CustomDialogService.Show(this,
				"Do you want to restore all files or volumes from the selected restore point?\n\nChoose Yes to restore everything, No to choose specific items, or Cancel to stop.",
				"Restore Scope",
				DialogButtons.YesNoCancel,
				DialogIcon.Question);

			if (restoreAllDecision == CustomDialogResult.Cancel)
			{
				return null;
			}

			if (restoreAllDecision == CustomDialogResult.Yes)
			{
				return new RestoreSelectionContext
				{
					Backup = backup,
					RestorePoint = selectedRestorePoint,
					RequireAlternateDestination = requireAlternateDestination,
					ScopeKind = RestoreScopeKind.All
				};
			}

			System.Collections.Generic.IReadOnlyList<VolumeInfo> volumes = RestoreWorkflowHelper.GetBackupVolumesForRestorePoint(selectedRestorePoint);
			if (volumes.Count > 1)
			{
				using var volumeForm = new RestoreVolumeSelectionForm(volumes, true, "Select the volume or full set of volumes to restore from this restore point.");
				if (volumeForm.ShowDialog(this) != DialogResult.OK || !volumeForm.Confirmed)
				{
					return null;
				}

				System.Collections.Generic.IReadOnlyList<VolumeInfo> selectedVolumes = volumeForm.SelectedDiskGroup ??
					(volumeForm.SelectedVolume != null ? new[] { volumeForm.SelectedVolume } : Array.Empty<VolumeInfo>());
				if (selectedVolumes.Count == 0)
				{
					return null;
				}

				return new RestoreSelectionContext
				{
					Backup = backup,
					RestorePoint = selectedRestorePoint,
					RequireAlternateDestination = requireAlternateDestination,
					ScopeKind = RestoreScopeKind.SelectedVolumes,
					SelectedVolumes = selectedVolumes
				};
			}

			System.Collections.Generic.IReadOnlyList<string> items = RestoreWorkflowHelper.GetBackupItemsForRestorePoint(selectedRestorePoint);
			if (items.Count == 0)
			{
				CustomDialogService.ShowWarning(this, "No selectable files or folders were found for the selected restore point.", "Restore Items Unavailable");
				return null;
			}

			using var itemForm = new RestoreItemSelectionForm(backup, selectedRestorePoint, items);
			if (itemForm.ShowDialog(this) != DialogResult.OK || itemForm.SelectedItems.Count == 0)
			{
				return null;
			}

			return new RestoreSelectionContext
			{
				Backup = backup,
				RestorePoint = selectedRestorePoint,
				RequireAlternateDestination = requireAlternateDestination,
				ScopeKind = RestoreScopeKind.SelectedItems,
				SelectedItems = itemForm.SelectedItems
			};
		}

		private bool IsBootRelatedBackup(AvailableBackupInfo backup)
		{
			BackupJob? matchingJob = jobManager.GetAllJobs().FirstOrDefault(j =>
				string.Equals(j.Name, backup.BackupName, StringComparison.OrdinalIgnoreCase) &&
				!string.IsNullOrWhiteSpace(j.DestinationPath) &&
				backup.BackupPath.StartsWith(j.DestinationPath, StringComparison.OrdinalIgnoreCase));

			if (matchingJob == null)
			{
				return false;
			}

			if (matchingJob.Target == BackupTarget.Disk)
			{
				int bootDiskNumber = GetBootDiskNumber();
				if (bootDiskNumber < 0)
				{
					return false;
				}

				string bootDevicePath = $@"\\.\PHYSICALDRIVE{bootDiskNumber}";
				foreach (string sourcePath in matchingJob.SourcePaths)
				{
					if (string.IsNullOrWhiteSpace(sourcePath))
					{
						continue;
					}

					string normalized = sourcePath.TrimEnd('\\');
					if (string.Equals(normalized, bootDevicePath, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (int.TryParse(normalized, out int diskIndex) && diskIndex == bootDiskNumber)
					{
						return true;
					}
				}

				return false;
			}

			foreach (string sourcePath in matchingJob.SourcePaths)
			{
				if (string.IsNullOrWhiteSpace(sourcePath))
				{
					continue;
				}

				try
				{
					if (sourcePath.StartsWith(@"\\?\", StringComparison.OrdinalIgnoreCase) ||
						sourcePath.EndsWith(":", StringComparison.OrdinalIgnoreCase) ||
						sourcePath.EndsWith(@":\", StringComparison.OrdinalIgnoreCase))
					{
						int result = SecureServerBackup.Services.BackupEngineInterop.IsBootVolume(sourcePath, out bool isBootVolume);
						if (result == 0 && isBootVolume)
						{
							return true;
						}
					}
					else
					{
						string systemRoot = Path.GetPathRoot(Environment.SystemDirectory)?.TrimEnd('\\') ?? string.Empty;
						string sourceRoot = Path.GetPathRoot(sourcePath)?.TrimEnd('\\') ?? string.Empty;
						if (!string.IsNullOrWhiteSpace(systemRoot) && string.Equals(systemRoot, sourceRoot, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}

			return false;
		}

		private static int GetBootDiskNumber()
		{
			try
			{
				using var searcher = new System.Management.ManagementObjectSearcher("SELECT DiskIndex FROM Win32_DiskPartition WHERE BootPartition = TRUE");
				foreach (System.Management.ManagementObject partition in searcher.Get())
				{
					if (int.TryParse(partition["DiskIndex"]?.ToString(), out int diskIndex))
					{
						return diskIndex;
					}
				}
			}
			catch
			{
			}

			return -1;
		}

		internal static System.Collections.Generic.IReadOnlyList<string> GroupRestoreBackupEntries(BackupJob job, System.Collections.Generic.IEnumerable<string> backupEntries)
		{
			string[] entries = backupEntries
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();

			if (job.Type != BackupType.SelectedFilesAndFolders || job.Target != BackupTarget.FilesAndFolders)
			{
				return entries
					.OrderByDescending(GetRestoreBackupEntryTimestamp)
					.ThenByDescending(path => path, StringComparer.OrdinalIgnoreCase)
					.ToArray();
			}

			return entries
				.GroupBy(path => GetRestoreBackupEntryTimestamp(path).Date)
				.Select(group => group
					.OrderByDescending(GetRestoreBackupEntryTimestamp)
					.ThenByDescending(path => path, StringComparer.OrdinalIgnoreCase)
					.First())
				.OrderByDescending(GetRestoreBackupEntryTimestamp)
				.ThenByDescending(path => path, StringComparer.OrdinalIgnoreCase)
				.ToArray();
		}

		private static DateTime GetRestoreBackupEntryTimestamp(string backupPath)
		{
			return File.Exists(backupPath)
				? new FileInfo(backupPath).LastWriteTime
				: Directory.GetLastWriteTime(backupPath);
		}

		private static string GetBackupTypeFromFilename(string filenameWithoutExt)
		{
			string lower = filenameWithoutExt.ToLowerInvariant();
			if (lower.Contains("full"))
			{
				return "Full";
			}

			if (lower.Contains("incremental") || lower.Contains("incr"))
			{
				return "Incremental";
			}

			if (lower.Contains("differential") || lower.Contains("diff"))
			{
				return "Differential";
			}

			return "Full";
		}

		private void OpenImportBackup()
		{
			using var form = new ImportBackupForm();
			form.ShowDialog(this);
			LoadBackupJobs();
			LoadVerifyBackups();
			LoadRestoreBackups();
		}

		private void OpenSchedules()
		{
			mainTabControl.SelectedTab = schedulesTabPage;
			EnsureScheduleManagementLoaded();
			EnsureSelectedTabFits();
		}

		private void OpenActivityManagement()
		{
			mainTabControl.SelectedTab = activityTabPage;
			EnsureActivityManagementLoaded();
		}

		private void OpenServiceManagement()
		{
			using var form = new ServiceManagementForm();
			form.ShowDialog(this);
		}

		private void OpenRecoveryEnvironment()
		{
			using var form = new RecoveryEnvironmentForm();
			form.ShowDialog(this);
		}

		private void OpenAbout()
		{
			using var form = new AboutForm();
			form.ShowDialog(this);
		}

		private void MainForm_Load(object? sender, EventArgs e)
		{
			LoadVersion();
			LoadBackupJobs();
			LoadVerifyBackups();
			LoadRestoreBackups();
			EnsureSelectedTabFits();
			if (SynchronizationContext.Current != null)
			{
				NotificationService.ConfigureUiContext(SynchronizationContext.Current, ShowActivityTab);
			}
		}

		private void MainForm_Resize(object? sender, EventArgs e)
		{
			ResizeBackupJobCards();
			ResizeRestoreColumns();
			ResizeVerifyColumns();
		}

		// Resize the Path column to fill available width; Action column stays fixed.
		private void ResizeRestoreColumns()
		{
			if (restoreBackupsDataGridView.Columns.Count < 6)
				return;

			int fixWidth = restoreBackupsDataGridView.Columns["BackupName"].Width
				+ restoreBackupsDataGridView.Columns["Type"].Width
				+ restoreBackupsDataGridView.Columns["Date"].Width
				+ restoreBackupsDataGridView.Columns["Encrypted"].Width
				+ restoreBackupsDataGridView.Columns["Action"].Width;
			int available = restoreBackupsDataGridView.ClientSize.Width - fixWidth - 30;
			restoreBackupsDataGridView.Columns["Path"].Width = Math.Max(120, available);
		}

		private void ResizeVerifyColumns()
		{
			if (verifyBackupsDataGridiew.Columns.Count < 6)
				return;

			int fixWidth = verifyBackupsDataGridiew.Columns["BackupName"].Width
				+ verifyBackupsDataGridiew.Columns["Type"].Width
				+ verifyBackupsDataGridiew.Columns["Date"].Width
				+ verifyBackupsDataGridiew.Columns["Encrypted"].Width
				+ verifyBackupsDataGridiew.Columns["Action"].Width;
			int available = verifyBackupsDataGridiew.ClientSize.Width - fixWidth - 30;
			verifyBackupsDataGridiew.Columns["Path"].Width = Math.Max(120, available);
		}

		private async void VerifyBackupsListView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex != verifyBackupsDataGridiew.Columns["Action"].Index)
				return;

			DataGridViewRow row = verifyBackupsDataGridiew.Rows[e.RowIndex];
			if (row.Tag is not AvailableBackupInfo backup)
				return;

			row.Selected = true;
			try
			{
				await VerifySelectedBackupAsync();
			}
			catch (Exception ex)
			{
				CustomDialogService.ShowError(this, $"Failed to verify the selected backup:{Environment.NewLine}{ex.Message}", "Verify Backup");
			}
		}

		private void RestoreBackupsListView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex != restoreBackupsDataGridView.Columns["Action"].Index)
				return;

			DataGridViewRow row = restoreBackupsDataGridView.Rows[e.RowIndex];
			if (row.Tag is not AvailableBackupInfo backup)
				return;

			row.Selected = true;
			OpenRestoreWorkflow(backup);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			WindowPositionManager.SaveMainWindowPosition(this);
			base.OnFormClosing(e);
		}

		private void RestoreBackupsListView_DoubleClick(object? sender, EventArgs e)
		{
			OpenSelectedRestoreBackup();
		}

	}
}