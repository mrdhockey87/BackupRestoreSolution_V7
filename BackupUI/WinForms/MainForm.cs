using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackup.Windows; // Keep for remaining staged WPF windows still launched from this WinForms shell.

using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class MainForm : Form
	{
		private const string BackupJobCardTag = "BackupJobCard";

		private readonly JobManager jobManager = new();
		private ActivityManagementForm? activityManagementView;

		public MainForm()
		{
			InitializeComponent();
			ConfigureMenu();
			ConfigureShell();
			MainMenuStrip = mainMenuStrip;
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
				Font = new Font(Font, FontStyle.Bold),
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
				FlowDirection = FlowDirection.TopDown,
				WrapContents = false,
				AutoSize = true,
				Margin = new Padding(10, 18, 0, 0)
			};

			actionsPanel.Controls.Add(CreateJobActionButton("Run Now", WinFormsThemeManager.ButtonBackground, async (_, _) => await RunBackupJobAsync(job)));
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
				Font = CreateMessageBoxFont(FontStyle.Regular),
				Margin = new Padding(0, 0, 8, 8)
			};

			var value = new Label
			{
				Text = valueText,
				AutoSize = true,
				Font = CreateMessageBoxFont(FontStyle.Bold),
				Margin = new Padding(0, 0, 0, 8),
				MaximumSize = allowWrap ? new Size(560, 0) : Size.Empty
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
				FlowDirection = FlowDirection.LeftToRight,
				Margin = new Padding(0)
			};

			flow.Controls.Add(new Label
			{
				Text = GetNextRunSummary(job),
				AutoSize = true,
				Font = CreateMessageBoxFont(FontStyle.Bold),
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
			Font? font = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
			if (font is null)
			{
				throw new InvalidOperationException("A default system font is required.");
			}

			return font;
		}

		private static Font CreateMessageBoxFont(FontStyle style)
		{
			Font font = GetMessageBoxFont();
			return new Font(font.FontFamily, font.Size, style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
		}

		private void EditBackupJob(BackupJob job)
		{
			using var form = new BackupWindowNewForm(job);
			form.ShowDialog(this);
			LoadBackupJobs();
			LoadRestoreBackups();
		}

		private void DeleteBackupJob(BackupJob job)
		{
			DialogResult result = global::System.Windows.Forms.MessageBox.Show(
				this,
				$"Are you sure you want to delete the backup job '{job.Name}'?",
				"Confirm Delete",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (result != DialogResult.Yes)
			{
				return;
			}

			try
			{
				jobManager.DeleteJob(job.Id);
				LoadBackupJobs();
				LoadRestoreBackups();

				global::System.Windows.Forms.MessageBox.Show(
					this,
					"Backup job deleted successfully.",
					"Success",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				global::System.Windows.Forms.MessageBox.Show(
					this,
					$"Failed to delete backup job: {ex.Message}",
					"Delete Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private async System.Threading.Tasks.Task RunBackupJobAsync(BackupJob job)
		{
			if (!CheckBackupService())
			{
				return;
			}

			DialogResult result = global::System.Windows.Forms.MessageBox.Show(
				this,
				$"Run backup job '{job.Name}' now?\n\nThe backup will run in the background service and continue even if you close this window.",
				"Run Backup",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (result != DialogResult.Yes)
			{
				return;
			}

			try
			{
				BackupLogger.LogInfo(job.Name, "User initiated manual backup from the Backup tab (Run Now clicked)");

				var serviceClient = new BackupServiceClient();
				bool success = await serviceClient.RunBackupNowAsync(job.Id);

				if (success)
				{
					BackupLogger.LogInfo(job.Name, "Service accepted backup request - backup is starting");

					var progressWindow = new BackupProgressWindow(job.Id, job.Name);
					progressWindow.Show();
					return;
				}

				BackupLogger.LogError(job.Name, "Failed to communicate with Secure Server Backup Service - backup was not started");

				global::System.Windows.Forms.MessageBox.Show(
					this,
					"Failed to start backup. The service may be busy or not responding.\n\nTry again in a few moments, or restart the Secure Server Backup Service from Windows Services.",
					"Service Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				BackupLogger.LogError(job.Name, $"Error starting backup job from the Backup tab: {ex.Message}");

				global::System.Windows.Forms.MessageBox.Show(
					this,
					$"Error starting backup job: {ex.Message}",
					"Run Backup Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private bool CheckBackupService()
		{
			try
			{
				using var service = new System.ServiceProcess.ServiceController("SecureServerBackupService");

				if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running)
				{
					BackupLogger.LogWarning("System", $"Secure Server Backup Service is not running (Status: {service.Status})");

					DialogResult result = global::System.Windows.Forms.MessageBox.Show(
						this,
						$"The Secure Server Backup Service is not running (Status: {service.Status}).\n\nWould you like to start it now?\n\nNote: You may need to run this application as Administrator to start the service.",
						"Service Not Running",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning);

					if (result == DialogResult.Yes)
					{
						try
						{
							BackupLogger.LogInfo("System", "Attempting to start Secure Server Backup Service...");
							service.Start();
							service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
							BackupLogger.LogInfo("System", "Secure Server Backup Service started successfully");

							global::System.Windows.Forms.MessageBox.Show(
								this,
								"Secure Server Backup Service started successfully.",
								"Service Started",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);

							return true;
						}
						catch (Exception ex)
						{
							BackupLogger.LogError("System", $"Failed to start Secure Server Backup Service: {ex.Message}");

							global::System.Windows.Forms.MessageBox.Show(
								this,
								$"Failed to start service: {ex.Message}\n\nPlease start the service manually from Windows Services (services.msc) or run this application as Administrator.",
								"Service Start Failed",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);

							return false;
						}
					}

					return false;
				}

				return true;
			}
			catch (System.InvalidOperationException)
			{
				BackupLogger.LogError("System", "Secure Server Backup Service is not installed on this system");

				DialogResult result = global::System.Windows.Forms.MessageBox.Show(
					this,
					"The Secure Server Backup Service is not installed on this system.\n\nThe service must be installed before backups can run.\n\nTo install the service:\n1. Open PowerShell as Administrator\n2. Navigate to the solution folder\n3. Run: .\\Install-BackupService.ps1\n\nWould you like to open the solution folder now?",
					"Service Not Installed",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Error);

				if (result == DialogResult.Yes)
				{
					try
					{
						string solutionDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
						solutionDir = Path.GetFullPath(Path.Combine(solutionDir, "..", "..", ".."));
						System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", solutionDir)
						{
							UseShellExecute = true
						});
					}
					catch
					{
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				BackupLogger.LogError("System", $"Error checking Secure Server Backup Service status: {ex.Message}");

				global::System.Windows.Forms.MessageBox.Show(
					this,
					$"Error checking service status: {ex.Message}",
					"Service Check Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);

				return false;
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
			restoreStatusLabel.MaximumSize = new Size(900, 0);

			ConfigurePlaceholderListView(mountBackupsListView);
			ConfigurePlaceholderListView(verifyBackupsListView);
			ConfigurePlaceholderListView(restoreBackupsListView);
			ConfigurePlaceholderListView(schedulesListView);

			restoreBackupsListView.Columns.Clear();
			restoreBackupsListView.Columns.Add("Backup Name", 220);
			restoreBackupsListView.Columns.Add("Type", 120);
			restoreBackupsListView.Columns.Add("Date", 160);
			restoreBackupsListView.Columns.Add("Encrypted", 110);
			restoreBackupsListView.Columns.Add("Path", 320);

			backupJobsPanel.Controls.Clear();
			backupJobsPanel.Controls.Add(BuildBackupActionPanel());
			backupJobsPanel.Controls.Add(emptyBackupJobsLabel);

			activityTabPanel.Controls.Clear();
			activityTabPanel.Controls.Add(BuildActivityTabContent());

			restoreActionsPanel.Controls.Clear();
			restoreActionsPanel.Controls.Add(CreateActionButton("Refresh", (_, _) => LoadRestoreBackups()));
			restoreActionsPanel.Controls.Add(CreateActionButton("Browse .ssb...", (_, _) => BrowseRestoreBackup()));
			restoreActionsPanel.Controls.Add(CreateActionButton("Restore Selected", (_, _) => OpenSelectedRestoreBackup()));
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
				Font = new Font(Font, FontStyle.Bold),
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 8, 18, 0)
			};

			var buttonsPanel = new FlowLayoutPanel
			{
				AutoSize = true,
				WrapContents = false,
				FlowDirection = FlowDirection.LeftToRight,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0)
			};

			buttonsPanel.Controls.Add(CreateHeaderActionButton("New Backup...", (_, _) => OpenNewBackup(), 130));
			buttonsPanel.Controls.Add(CreateHeaderActionButton("Import Backup...", (_, _) => OpenImportBackup(), 130));
			buttonsPanel.Controls.Add(CreateHeaderActionButton("Refresh", (_, _) =>
			{
				LoadBackupJobs();
				LoadRestoreBackups();
			}, 100));

			panel.Controls.Add(titleLabel, 0, 0);
			panel.Controls.Add(buttonsPanel, 1, 0);

			return panel;
		}

		private Control BuildActivityTabContent()
		{
			var panel = new Panel
			{
				Dock = DockStyle.Fill,
				BackColor = WinFormsThemeManager.PanelBackground,
				Padding = new Padding(0)
			};

			activityManagementView?.Dispose();
			activityManagementView = new ActivityManagementForm
			{
				TopLevel = false,
				FormBorderStyle = FormBorderStyle.None,
				Dock = DockStyle.Fill,
				Visible = false
			};

			panel.Controls.Add(activityManagementView);
			activityManagementView.Show();

			return panel;
		}

		private static Label CreatePlaceholderLabel(string text)
		{
			return new Label
			{
				Text = text,
				AutoSize = true,
				ForeColor = WinFormsThemeManager.SecondaryText,
				Margin = new Padding(10, 8, 10, 8),
				BackColor = WinFormsThemeManager.PanelBackground
			};
		}

		private static void ConfigurePlaceholderListView(ListView listView)
		{
			listView.Dock = DockStyle.Fill;
			listView.View = View.Details;
			listView.FullRowSelect = true;
			listView.GridLines = true;
			listView.HideSelection = false;
			listView.BackColor = Color.White;
		}

		private static Button CreateActionButton(string text, EventHandler onClick)
		{
			var button = new Button
			{
				Text = text,
				AutoSize = true,
				MinimumSize = new Size(130, 32),
				Margin = new Padding(0, 0, 8, 8)
			};

			button.Click += onClick;
			return button;
		}

		private static ListView CreatePlaceholderListView()
		{
			return new ListView
			{
				Dock = DockStyle.Fill,
				View = View.Details,
				FullRowSelect = true,
				GridLines = true,
				HideSelection = false,
				BackColor = Color.White
			};
		}

		private Button CreateHeaderActionButton(string text, EventHandler onClick, int width)
		{
			var button = new Button
			{
				Text = text,
				Width = width,
				Height = 32,
				Margin = new Padding(0, 0, 10, 0),
				BackColor = WinFormsThemeManager.ButtonBackground,
				ForeColor = WinFormsThemeManager.PrimaryText,
				FlatStyle = FlatStyle.Flat
			};

			button.FlatAppearance.BorderColor = WinFormsThemeManager.BorderColor;
			button.FlatAppearance.BorderSize = 1;
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
				Margin = new Padding(0, 0, 0, 10),
				BackColor = backColor,
				ForeColor = WinFormsThemeManager.PrimaryText,
				FlatStyle = FlatStyle.Flat
			};

			button.FlatAppearance.BorderSize = 1;
			button.FlatAppearance.BorderColor = ControlPaint.Dark(backColor);
			button.Click += onClick;
			return button;
		}

		private void MainTabControl_DrawItem(object? sender, DrawItemEventArgs e)
		{
			TabPage page = mainTabControl.TabPages[e.Index];
			bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
			Rectangle bounds = e.Bounds;

			using var backBrush = new SolidBrush(isSelected ? WinFormsThemeManager.LightTurquoise : Color.White);
			using var borderPen = new Pen(WinFormsThemeManager.BorderColor);

			e.Graphics.FillRectangle(backBrush, bounds);
			e.Graphics.DrawRectangle(borderPen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

			TextRenderer.DrawText(
				e.Graphics,
				page.Text,
				Font,
				bounds,
				WinFormsThemeManager.PrimaryText,
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
			using var form = new BackupWindowNewForm();
			form.ShowDialog(this);
			LoadBackupJobs();
			LoadRestoreBackups();
		}

		private void OpenRestore()
		{
			if (restoreBackupsListView.SelectedItems.Count > 0)
			{
				OpenSelectedRestoreBackup();
				return;
			}

			LoadRestoreBackups();
			mainTabControl.SelectedTab = mainTabControl.TabPages.Cast<TabPage>().FirstOrDefault(page => page.Text == "Restore") ?? mainTabControl.SelectedTab;
		}

		private void LoadRestoreBackups()
		{
			var backups = new System.Collections.Generic.List<AvailableBackupInfo>();

			try
			{
				var jobs = jobManager.GetAllJobs();
				foreach (BackupJob job in jobs)
				{
					string destPath = job.DestinationPath;
					if (!Directory.Exists(destPath))
					{
						continue;
					}

					var backupEntries = Directory.EnumerateFileSystemEntries(destPath, "*.ssb", SearchOption.AllDirectories);
					foreach (string ssb in GroupRestoreBackupEntries(job, backupEntries))
					{
						DateTime backupDate = File.Exists(ssb)
							? new FileInfo(ssb).LastWriteTime
							: Directory.GetLastWriteTime(ssb);

						bool isEncrypted = File.Exists(ssb) && BackupEncryptionService.IsEncryptedBackupFile(ssb);
						backups.Add(new AvailableBackupInfo
						{
							BackupName = job.Name,
							BackupType = job.Type.ToString(),
							BackupDate = backupDate,
							BackupPath = ssb,
							IsEncrypted = isEncrypted,
							ProtectedEncryptionPassword = job.ProtectedEncryptionPassword
						});
					}
				}

				restoreBackupsListView.BeginUpdate();
				restoreBackupsListView.Items.Clear();
				foreach (AvailableBackupInfo backup in backups.OrderByDescending(backup => backup.BackupDate))
				{
					var item = new ListViewItem(backup.BackupName);
					item.SubItems.Add(backup.BackupType);
					item.SubItems.Add(backup.BackupDate.ToString("g"));
					item.SubItems.Add(backup.EncryptionStatus);
					item.SubItems.Add(backup.BackupPath);
					item.Tag = backup;
					restoreBackupsListView.Items.Add(item);
				}
				restoreBackupsListView.EndUpdate();

				emptyRestoreBackupsLabel.Visible = backups.Count == 0;
				restoreStatusLabel.Text = backups.Count == 0
					? "No backups were found. Create a backup first, then return here to restore it."
					: "Select a backup to open the restore workflow. Restoring the boot/system drive requires the recovery environment.";
			}
			catch (Exception ex)
			{
				restoreBackupsListView.Items.Clear();
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
			bool exists = restoreBackupsListView.Items.Cast<ListViewItem>()
				.Any(item => item.Tag is AvailableBackupInfo backup && backup.BackupPath.Equals(selectedFile, StringComparison.OrdinalIgnoreCase));
			if (exists)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "This backup is already in the list.", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var fileInfo = new FileInfo(selectedFile);
			var backupInfo = new AvailableBackupInfo
			{
				BackupName = Path.GetFileNameWithoutExtension(selectedFile),
				BackupType = GetBackupTypeFromFilename(Path.GetFileNameWithoutExtension(selectedFile)),
				BackupDate = fileInfo.LastWriteTime,
				BackupPath = selectedFile,
				IsEncrypted = BackupEncryptionService.IsEncryptedBackupFile(selectedFile)
			};

			var item = new ListViewItem(backupInfo.BackupName);
			item.SubItems.Add(backupInfo.BackupType);
			item.SubItems.Add(backupInfo.BackupDate.ToString("g"));
			item.SubItems.Add(backupInfo.EncryptionStatus);
			item.SubItems.Add(backupInfo.BackupPath);
			item.Tag = backupInfo;
			restoreBackupsListView.Items.Add(item);
			emptyRestoreBackupsLabel.Visible = false;
			restoreStatusLabel.Text = "Select a backup to open the restore workflow. Restoring the boot/system drive requires the recovery environment.";
			global::System.Windows.Forms.MessageBox.Show(this, $"Backup file added for restore: {Path.GetFileName(selectedFile)}", "Backup Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void OpenSelectedRestoreBackup()
		{
			if (restoreBackupsListView.SelectedItems.Count == 0 || restoreBackupsListView.SelectedItems[0].Tag is not AvailableBackupInfo backup)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please select a valid backup to restore.", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			OpenRestoreWorkflow(backup);
		}

		private void OpenRestoreWorkflow(AvailableBackupInfo backup)
		{
			try
			{
				bool requireAlternateDestination = false;
				if (IsBootRelatedBackup(backup))
				{
					DialogResult bootRestoreDecision = global::System.Windows.Forms.MessageBox.Show(
						this,
						"This backup includes the currently booted disk/volume.\n\nDo you want to restore it to a non-boot disk/volume from Windows?",
						"Boot Drive Restore Detected",
						MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Warning);

					if (bootRestoreDecision == DialogResult.No)
					{
						global::System.Windows.Forms.MessageBox.Show(this, "To restore the currently booted disk/volume in place, boot from the recovery disk and perform the restore from there.", "Recovery Disk Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}

					if (bootRestoreDecision != DialogResult.Yes)
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
				global::System.Windows.Forms.MessageBox.Show(this, $"Error opening restore workflow: {ex.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

			DialogResult restoreAllDecision = global::System.Windows.Forms.MessageBox.Show(
				this,
				"Do you want to restore all files or volumes from the selected restore point?\n\nChoose Yes to restore everything, No to choose specific items, or Cancel to stop.",
				"Restore Scope",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question);

			if (restoreAllDecision == DialogResult.Cancel)
			{
				return null;
			}

			if (restoreAllDecision == DialogResult.Yes)
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
				global::System.Windows.Forms.MessageBox.Show(this, "No selectable files or folders were found for the selected restore point.", "Restore Items Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
			LoadRestoreBackups();
		}

		private void OpenSchedules()
		{
			using var form = new ScheduleManagementForm();
			form.ShowDialog(this);
			LoadBackupJobs();
		}

		private void OpenActivityManagement()
		{
			using var form = new ActivityManagementForm();
			form.ShowDialog(this);
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
			LoadRestoreBackups();
			if (SynchronizationContext.Current != null)
			{
				NotificationService.ConfigureUiContext(SynchronizationContext.Current, ShowActivityTab);
			}
		}

		private void MainForm_Resize(object? sender, EventArgs e)
		{
			ResizeBackupJobCards();
		}

		private void RestoreBackupsListView_DoubleClick(object? sender, EventArgs e)
		{
			OpenSelectedRestoreBackup();
		}
	}
}