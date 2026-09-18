using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Services;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class ScheduleManagementForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly JobManager jobManager = new();

		public ScheduleManagementForm()
		{
			InitializeComponent();

			if (IsInDesignMode)
			{
				LoadDesignTimeJobs();
			}
			else
			{
				Load += ScheduleManagementForm_Load;
			}
		}

		private void ScheduleManagementForm_Load(object? sender, EventArgs e)
		{
			LoadJobs();
		}

		private void LoadDesignTimeJobs()
		{
			jobsGrid.DataSource = new[]
			{
				new BackupJob
				{
					Name = "Daily Files",
					Type = BackupType.Full,
					DestinationPath = @"D:\Backups",
					IsCurrentlyRunning = false,
					LastRunTime = DateTime.Now.AddDays(-1),
					NextScheduledRun = DateTime.Now.AddHours(6),
					Schedule = new BackupSchedule { Enabled = true, Frequency = ScheduleFrequency.Daily, Time = new TimeSpan(2, 0, 0) }
				},
				new BackupJob
				{
					Name = "Weekly Clone",
					Type = BackupType.CloneToVirtualDisk,
					DestinationPath = @"E:\Clones",
					IsCurrentlyRunning = false,
					LastRunTime = DateTime.Now.AddDays(-7),
					NextScheduledRun = DateTime.Now.AddDays(1),
					Schedule = new BackupSchedule { Enabled = true, Frequency = ScheduleFrequency.Weekly, Time = new TimeSpan(3, 0, 0), DaysOfWeek = { DayOfWeek.Sunday } }
				}
			};
		}

		private void RefreshButton_Click(object? sender, EventArgs e)
		{
			LoadJobs();
		}

		private void EditNextRunButton_Click(object? sender, EventArgs e)
		{
			EditNextRun();
		}

		private void EditJobButton_Click(object? sender, EventArgs e)
		{
			EditJob();
		}

		private void DeleteJobButton_Click(object? sender, EventArgs e)
		{
			DeleteJob();
		}

		private async void RunNowButton_Click(object? sender, EventArgs e)
		{
			await RunNowAsync();
		}

		private void JobsGrid_DoubleClick(object? sender, EventArgs e)
		{
			EditJob();
		}

		private void LoadJobs()
		{
			jobsGrid.DataSource = jobManager.GetScheduledJobs().ToList();
		}

		private BackupJob? GetSelectedJob()
		{
			return jobsGrid.CurrentRow?.DataBoundItem as BackupJob;
		}

		private void EditNextRun()
		{
			BackupJob? job = GetSelectedJob();
			if (job == null)
			{
				CustomDialogService.ShowWarning(this, "Please select a valid scheduled job.", "No Selection");
				return;
			}

			if (job.Schedule == null || !job.Schedule.Enabled || !job.NextScheduledRun.HasValue)
			{
				CustomDialogService.ShowWarning(this, "This job does not have an editable next run time.", "Edit Next Run");
				return;
			}

			DateTime currentNextRun = job.NextScheduledRun.Value;
			DateTime? latestAllowedRun = GetLatestAllowedNextRun(job.Schedule, currentNextRun);
			if (!latestAllowedRun.HasValue || latestAllowedRun.Value < currentNextRun)
			{
				CustomDialogService.ShowWarning(this, "Unable to determine the valid edit range for this schedule.", "Edit Next Run");
				return;
			}

			CustomDialogService.ShowInfo(this,
				$"You can edit the next run for '{job.Name}'.{Environment.NewLine}{Environment.NewLine}This is a one time change only. After this edited next run is used, the job will return to its normal schedule.{Environment.NewLine}{Environment.NewLine}Current next run: {currentNextRun:yyyy-MM-dd hh:mm tt}{Environment.NewLine}Latest allowed value: {latestAllowedRun.Value:yyyy-MM-dd hh:mm tt}",
				"Edit Next Run");

			using var form = new NextRunTimeEditForm(job, currentNextRun, latestAllowedRun.Value);
			if (form.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			job.NextScheduledRun = form.SelectedNextRun;
			jobManager.UpdateJob(job);
			BackupLogger.LogInfo(job.Name, $"Next scheduled run manually edited to {form.SelectedNextRun:yyyy-MM-dd HH:mm:ss}");
			LoadJobs();
			CustomDialogService.ShowSuccess(this, $"Next run updated to {form.SelectedNextRun:yyyy-MM-dd hh:mm tt}.", "Next Run Updated");
		}

		private static DateTime? GetLatestAllowedNextRun(BackupSchedule schedule, DateTime currentNextRun)
		{
			return schedule.Frequency switch
			{
				ScheduleFrequency.Daily => currentNextRun.Date.Add(schedule.Time).AddDays(1),
				ScheduleFrequency.Weekly => GetNextWeeklyOccurrence(schedule, currentNextRun),
				ScheduleFrequency.Monthly => GetNextMonthlyOccurrence(schedule, currentNextRun),
				ScheduleFrequency.Once => currentNextRun,
				_ => null
			};
		}

		private static DateTime? GetNextWeeklyOccurrence(BackupSchedule schedule, DateTime currentNextRun)
		{
			if (schedule.DaysOfWeek.Count == 0)
			{
				return null;
			}

			DateTime candidate = currentNextRun.Date.AddDays(1).Add(schedule.Time);
			while (!schedule.DaysOfWeek.Contains(candidate.DayOfWeek))
			{
				candidate = candidate.AddDays(1);
			}

			return candidate;
		}

		private static DateTime GetNextMonthlyOccurrence(BackupSchedule schedule, DateTime currentNextRun)
		{
			DateTime nextMonth = new(currentNextRun.Year, currentNextRun.Month, 1);
			nextMonth = nextMonth.AddMonths(1);
			int day = Math.Max(1, Math.Min(schedule.DayOfMonth, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)));
			return new DateTime(nextMonth.Year, nextMonth.Month, day, schedule.Time.Hours, schedule.Time.Minutes, 0);
		}

		private void EditJob()
		{
			BackupJob? job = GetSelectedJob();
			if (job == null)
			{
				CustomDialogService.ShowWarning(this, "Please select a job to edit.", "No Selection");
				return;
			}

			using var form = new BackupWindowNewForm(job);
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				LoadJobs();
			}
		}

		private void DeleteJob()
		{
			BackupJob? job = GetSelectedJob();
			if (job == null)
			{
				CustomDialogService.ShowWarning(this, "Please select a job to delete.", "No Selection");
				return;
			}

			CustomDialogResult result = CustomDialogService.ShowQuestion(this, $"Are you sure you want to delete the job '{job.Name}'?", "Confirm Delete");
			if (result != CustomDialogResult.Yes)
			{
				return;
			}

			jobManager.DeleteJob(job.Id);
			LoadJobs();
			CustomDialogService.ShowSuccess(this, "Job deleted successfully.", "Success");
		}

		private async Task RunNowAsync()
		{
			BackupJob? job = GetSelectedJob();
			if (job == null)
			{
				CustomDialogService.ShowWarning(this, "Please select a job to run.", "No Selection");
				return;
			}

			if (!CheckBackupService())
			{
				return;
			}

			CustomDialogResult result = CustomDialogService.ShowQuestion(this,
				$"Run backup job '{job.Name}' now?{Environment.NewLine}{Environment.NewLine}The backup will run in the background service and continue even if you close this window.",
				"Run Backup");
			if (result != CustomDialogResult.Yes)
			{
				return;
			}

			BackupLogger.LogInfo(job.Name, "User initiated manual backup from Schedule Management (Run Now clicked)");
			var serviceClient = new BackupServiceClient();
			bool success = await serviceClient.RunBackupNowAsync(job.Id);
			if (!success)
			{
				BackupLogger.LogError(job.Name, "Failed to communicate with Secure Server Backup Service - backup was not started");
				CustomDialogService.ShowError(this,
					"Failed to start backup. The service may be busy or not responding.\n\nTry again in a few moments, or restart the Secure Server Backup Service from Windows Services.",
					"Service Error");
				return;
			}

			BackupLogger.LogInfo(job.Name, "Service accepted backup request - backup is starting");
			var progressForm = new BackupProgressForm(job.Id, job.Name);
			progressForm.Show(this);
		}

		private bool CheckBackupService()
		{
			try
			{
				using var service = new ServiceController("SecureServerBackupService");
				if (service.Status == ServiceControllerStatus.Running)
				{
					return true;
				}

				BackupLogger.LogWarning("System", $"Secure Server Backup Service is not running (Status: {service.Status})");
				CustomDialogResult result = CustomDialogService.ShowQuestion(this,
					$"The Secure Server Backup Service is not running (Status: {service.Status}).{Environment.NewLine}{Environment.NewLine}Would you like to start it now?{Environment.NewLine}{Environment.NewLine}Note: You may need to run this application as Administrator to start the service.",
					"Service Not Running");
				if (result != CustomDialogResult.Yes)
				{
					return false;
				}

				try
				{
					BackupLogger.LogInfo("System", "Attempting to start Secure Server Backup Service...");
					service.Start();
					service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
					BackupLogger.LogInfo("System", "Secure Server Backup Service started successfully");
					CustomDialogService.ShowSuccess(this, "Secure Server Backup Service started successfully.", "Service Started");
					return true;
				}
				catch (Exception ex)
				{
					BackupLogger.LogError("System", $"Failed to start Secure Server Backup Service: {ex.Message}");
					CustomDialogService.ShowError(this,
						$"Failed to start service: {ex.Message}{Environment.NewLine}{Environment.NewLine}Please start the service manually from Windows Services (services.msc) or run this application as Administrator.",
						"Service Start Failed");
					return false;
				}
			}
			catch (InvalidOperationException)
			{
				BackupLogger.LogError("System", "Secure Server Backup Service is not installed on this system");
				CustomDialogResult result = CustomDialogService.ShowQuestion(this,
					"The Secure Server Backup Service is not installed on this system.\n\nThe service must be installed before backups can run.\n\nTo install the service:\n1. Open PowerShell as Administrator\n2. Navigate to the solution folder\n3. Run: .\\Install-BackupService.ps1\n\nWould you like to open the solution folder now?",
					"Service Not Installed");
				if (result == CustomDialogResult.Yes)
				{
					try
					{
						string solutionDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
						solutionDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(solutionDir, "..", "..", ".."));
						Process.Start("explorer.exe", solutionDir);
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
				CustomDialogService.ShowError(this, $"Error checking service status: {ex.Message}", "Service Check Error");
				return false;
			}
		}
	}
}
