using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed class ServiceManagementForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly BackupServiceManager serviceManager = new();
		private readonly Label statusValueLabel;
		private readonly Label installedValueLabel;
		private readonly Label uiVersionValueLabel;
		private readonly Label serviceVersionValueLabel;
		private readonly Label versionWarningLabel;
		private readonly Button startButton;
		private readonly Button stopButton;
		private readonly Button restartButton;
		private readonly Button installButton;
		private readonly Button uninstallButton;

		public ServiceManagementForm()
		{
			Font baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
			Text = "Service Management";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(620, 500);
			ClientSize = new Size(620, 500);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Backup Service Management",
				Font = new Font(baseFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(16, 16)
			};

			var statusGroup = new GroupBox
			{
				Text = "Service Status",
				Location = new Point(16, 52),
				Size = new Size(580, 190)
			};

			statusGroup.Controls.Add(CreateCaptionLabel("Status:", 16, 32));
			statusValueLabel = CreateValueLabel("Unknown", 140, 32);
			statusGroup.Controls.Add(statusValueLabel);

			statusGroup.Controls.Add(CreateCaptionLabel("Installed:", 16, 60));
			installedValueLabel = CreateValueLabel("Unknown", 140, 60);
			statusGroup.Controls.Add(installedValueLabel);

			statusGroup.Controls.Add(CreateCaptionLabel("UI Version:", 16, 98));
			uiVersionValueLabel = CreateValueLabel(VersionClass.GetAssemblyVersion(), 140, 98);
			statusGroup.Controls.Add(uiVersionValueLabel);

			statusGroup.Controls.Add(CreateCaptionLabel("Service Version:", 16, 126));
			serviceVersionValueLabel = CreateValueLabel("Unknown", 140, 126);
			statusGroup.Controls.Add(serviceVersionValueLabel);

			versionWarningLabel = new Label
			{
				AutoSize = true,
				Location = new Point(280, 126),
				ForeColor = Color.DarkRed,
				Visible = false
			};
			statusGroup.Controls.Add(versionWarningLabel);

			var refreshStatusButton = new Button
			{
				Text = "Refresh Status",
				Location = new Point(16, 154),
				Size = new Size(130, 28)
			};
			refreshStatusButton.Click += async (_, _) => await RefreshStatusAsync();
			statusGroup.Controls.Add(refreshStatusButton);

			var abortRetriesButton = new Button
			{
				Text = "Abort Failed Retries",
				Location = new Point(156, 154),
				Size = new Size(150, 28)
			};
			abortRetriesButton.Click += async (_, _) => await AbortFailedRetriesAsync();
			statusGroup.Controls.Add(abortRetriesButton);

			var controlGroup = new GroupBox
			{
				Text = "Service Control",
				Location = new Point(16, 252),
				Size = new Size(580, 140)
			};

			startButton = new Button { Text = "Start Service", Location = new Point(16, 30), Size = new Size(120, 30) };
			startButton.Click += async (_, _) => await StartServiceAsync();
			stopButton = new Button { Text = "Stop Service", Location = new Point(146, 30), Size = new Size(120, 30) };
			stopButton.Click += async (_, _) => await StopServiceAsync();
			restartButton = new Button { Text = "Restart Service", Location = new Point(276, 30), Size = new Size(120, 30) };
			restartButton.Click += async (_, _) => await RestartServiceAsync();
			installButton = new Button { Text = "Install and Start Service", Location = new Point(16, 78), Size = new Size(170, 30) };
			installButton.Click += async (_, _) => await InstallServiceAsync();
			uninstallButton = new Button { Text = "Uninstall Service", Location = new Point(196, 78), Size = new Size(130, 30) };
			uninstallButton.Click += async (_, _) => await UninstallServiceAsync();

			controlGroup.Controls.Add(startButton);
			controlGroup.Controls.Add(stopButton);
			controlGroup.Controls.Add(restartButton);
			controlGroup.Controls.Add(installButton);
			controlGroup.Controls.Add(uninstallButton);

			var closeButton = new Button
			{
				Text = "Close",
				Location = new Point(496, 410),
				Size = new Size(100, 32),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.OK
			};

			Controls.Add(titleLabel);
			Controls.Add(statusGroup);
			Controls.Add(controlGroup);
			Controls.Add(closeButton);

			if (IsInDesignMode)
			{
				LoadDesignTimeStatus();
			}
			else
			{
				Load += async (_, _) => await RefreshStatusAsync();
			}
		}

		private void LoadDesignTimeStatus()
		{
			installedValueLabel.Text = "Yes";
			statusValueLabel.Text = "Running";
			serviceVersionValueLabel.Text = VersionClass.GetAssemblyVersion();
			versionWarningLabel.Text = string.Empty;
			versionWarningLabel.Visible = false;
			startButton.Enabled = false;
			stopButton.Enabled = true;
			restartButton.Enabled = true;
			installButton.Enabled = false;
			uninstallButton.Enabled = true;
		}

		private static Label CreateCaptionLabel(string text, int x, int y)
		{
			Font baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
			return new Label { Text = text, AutoSize = true, Location = new Point(x, y), Font = new Font(baseFont, FontStyle.Bold) };
		}

		private static Label CreateValueLabel(string text, int x, int y)
		{
			return new Label { Text = text, AutoSize = true, Location = new Point(x, y) };
		}

		private async Task RefreshStatusAsync()
		{
			try
			{
				bool isInstalled = await serviceManager.IsServiceInstalledAsync();
				installedValueLabel.Text = isInstalled ? "Yes" : "No";

				if (!isInstalled)
				{
					statusValueLabel.Text = "Not Installed";
					serviceVersionValueLabel.Text = "N/A (not installed)";
					versionWarningLabel.Visible = false;
					startButton.Enabled = false;
					stopButton.Enabled = false;
					restartButton.Enabled = false;
					installButton.Enabled = true;
					uninstallButton.Enabled = false;
					return;
				}

				ServiceControllerStatus? status = await serviceManager.GetServiceStatusAsync();
				statusValueLabel.Text = status.HasValue ? FormatServiceStatus(status.Value) : "Unknown";
				startButton.Enabled = status != ServiceControllerStatus.Running;
				stopButton.Enabled = status == ServiceControllerStatus.Running;
				restartButton.Enabled = status == ServiceControllerStatus.Running;
				installButton.Enabled = false;
				uninstallButton.Enabled = true;

				if (status == ServiceControllerStatus.Running)
				{
					await CheckServiceVersionAsync();
				}
				else
				{
					serviceVersionValueLabel.Text = "N/A (service not running)";
					versionWarningLabel.Text = "Not Running";
					versionWarningLabel.ForeColor = Color.DarkOrange;
					versionWarningLabel.Visible = true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Failed to refresh status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async Task CheckServiceVersionAsync()
		{
			try
			{
				var serviceClient = new BackupServiceClient();
				Task<string?> versionTask = serviceClient.GetServiceVersionAsync();
				Task completedTask = await Task.WhenAny(versionTask, Task.Delay(3000));
				string? serviceVersion = completedTask == versionTask ? await versionTask : null;

				if (!string.IsNullOrWhiteSpace(serviceVersion))
				{
					serviceVersionValueLabel.Text = serviceVersion;
					string uiVersion = VersionClass.GetAssemblyVersion();
					if (!string.Equals(serviceVersion, uiVersion, StringComparison.Ordinal))
					{
						versionWarningLabel.Text = "VERSION MISMATCH";
						versionWarningLabel.ForeColor = Color.DarkRed;
						versionWarningLabel.Visible = true;
					}
					else
					{
						versionWarningLabel.Visible = false;
					}
				}
				else
				{
					serviceVersionValueLabel.Text = "Unknown (old version)";
					versionWarningLabel.Text = "Reinstall Required";
					versionWarningLabel.ForeColor = Color.DarkOrange;
					versionWarningLabel.Visible = true;
				}
			}
			catch
			{
				serviceVersionValueLabel.Text = "Unknown (check failed)";
				versionWarningLabel.Text = "Check Failed";
				versionWarningLabel.ForeColor = Color.DarkOrange;
				versionWarningLabel.Visible = true;
			}
		}

		private async Task StartServiceAsync()
		{
			bool success = await serviceManager.StartServiceAsync();
			MessageBox.Show(this, success ? "Service started successfully." : "Failed to start service.", success ? "Success" : "Error", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			await RefreshStatusAsync();
		}

		private async Task StopServiceAsync()
		{
			bool success = await serviceManager.StopServiceAsync();
			MessageBox.Show(this, success ? "Service stopped successfully." : "Failed to stop service.", success ? "Success" : "Error", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			await RefreshStatusAsync();
		}

		private async Task RestartServiceAsync()
		{
			bool success = await serviceManager.RestartServiceAsync();
			MessageBox.Show(this, success ? "Service restarted successfully." : "Failed to restart service.", success ? "Success" : "Error", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			await RefreshStatusAsync();
		}

		private async Task InstallServiceAsync()
		{
			string? servicePath = ServiceInstaller.GetServiceExecutablePath();
			if (string.IsNullOrWhiteSpace(servicePath) || !File.Exists(servicePath))
			{
				MessageBox.Show(this, $"Service executable not found in: {AppDomain.CurrentDomain.BaseDirectory}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			bool installSuccess = await serviceManager.InstallServiceAsync(servicePath);
			if (!installSuccess)
			{
				MessageBox.Show(this, "Failed to install service. Make sure you run as Administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			await Task.Delay(1000);
			bool startSuccess = await serviceManager.StartServiceAsync();
			MessageBox.Show(this, startSuccess ? "Service installed and started successfully!" : "Service installed successfully, but failed to start automatically.", startSuccess ? "Success" : "Partial Success", MessageBoxButtons.OK, startSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
			await RefreshStatusAsync();
		}

		private async Task UninstallServiceAsync()
		{
			if (MessageBox.Show(this, "Are you sure you want to uninstall the service?", "Confirm Uninstall", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			bool success = await serviceManager.UninstallServiceAsync();
			MessageBox.Show(this, success ? "Service uninstalled successfully." : "Failed to uninstall service. Make sure you run as Administrator.", success ? "Success" : "Error", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			await RefreshStatusAsync();
		}

		private async Task AbortFailedRetriesAsync()
		{
			try
			{
				if (MessageBox.Show(this,
					"This will cancel all pending retry attempts for failed backups.\n\nFailed jobs will be rescheduled for their next normal run time.\n\nDo you want to continue?",
					"Abort Failed Retries",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return;
				}

				string jobsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SecureServerBackupService", "jobs.json");
				if (!File.Exists(jobsFilePath))
				{
					MessageBox.Show(this, "No backup jobs found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				string json = await File.ReadAllTextAsync(jobsFilePath);
				var jobs = JsonSerializer.Deserialize<System.Collections.Generic.List<BackupJob>>(json);
				if (jobs == null || jobs.Count == 0)
				{
					MessageBox.Show(this, "No backup jobs found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				int abortedCount = 0;
				DateTime now = DateTime.Now;
				foreach (BackupJob job in jobs)
				{
					if (job.Schedule?.NextRunTime == null)
					{
						continue;
					}

					TimeSpan timeUntilRun = job.Schedule.NextRunTime.Value - now;
					bool isInRetryMode = job.Schedule.NextRunTime.Value < now || timeUntilRun.TotalMinutes <= 20;
					if (!isInRetryMode)
					{
						continue;
					}

					RecalculateNextRunTime(job);
					abortedCount++;
				}

				if (abortedCount == 0)
				{
					MessageBox.Show(this, "No jobs found in retry mode.\n\nAll jobs are already scheduled for their normal run times.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				json = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
				await File.WriteAllTextAsync(jobsFilePath, json);

				if (MessageBox.Show(this,
					$"Aborted retry attempts for {abortedCount} job(s).\n\nJobs have been rescheduled for their next normal run time.\n\nDo you want to restart the service now to immediately stop all retries?",
					"Success - Restart Service?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) == DialogResult.Yes)
				{
					bool stopped = await serviceManager.StopServiceAsync();
					if (stopped)
					{
						await Task.Delay(2000);
						bool started = await serviceManager.StartServiceAsync();
						MessageBox.Show(this, started ? "Service restarted successfully.\nAll retries have been stopped." : "Service stopped but failed to restart.\nPlease start it manually.", started ? "Service Restarted" : "Restart Failed", MessageBoxButtons.OK, started ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
					}
					else
					{
						MessageBox.Show(this, "Failed to stop service. Changes saved but may take effect on next service restart.", "Restart Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}

				await RefreshStatusAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Failed to abort retries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private static void RecalculateNextRunTime(BackupJob job)
		{
			if (job.Schedule == null)
			{
				return;
			}

			DateTime now = DateTime.Now;
			DateTime scheduledTime = now.Date.Add(job.Schedule.Time);
			switch (job.Schedule.Frequency)
			{
				case ScheduleFrequency.Daily:
					job.Schedule.NextRunTime = scheduledTime > now ? scheduledTime : scheduledTime.AddDays(1);
					break;
				case ScheduleFrequency.Weekly:
					DateTime nextRun = scheduledTime > now ? scheduledTime : scheduledTime.AddDays(1);
					while (!job.Schedule.DaysOfWeek.Contains(nextRun.DayOfWeek))
					{
						nextRun = nextRun.AddDays(1);
					}
					job.Schedule.NextRunTime = nextRun;
					break;
				case ScheduleFrequency.Monthly:
					DateTime nextMonth = new(now.Year, now.Month, job.Schedule.DayOfMonth, job.Schedule.Time.Hours, job.Schedule.Time.Minutes, 0);
					if (nextMonth <= now)
					{
						nextMonth = nextMonth.AddMonths(1);
					}
					job.Schedule.NextRunTime = nextMonth;
					break;
				case ScheduleFrequency.Once:
					job.Schedule.NextRunTime = null;
					job.Schedule.Enabled = false;
					break;
			}
		}

		private static string FormatServiceStatus(ServiceControllerStatus status)
		{
			return status switch
			{
				ServiceControllerStatus.Running => "Running",
				ServiceControllerStatus.Stopped => "Stopped",
				ServiceControllerStatus.Paused => "Paused",
				ServiceControllerStatus.StartPending => "Start Pending",
				ServiceControllerStatus.StopPending => "Stop Pending",
				ServiceControllerStatus.ContinuePending => "Continue Pending",
				ServiceControllerStatus.PausePending => "Pause Pending",
				_ => status.ToString()
			};
		}
	}
}
