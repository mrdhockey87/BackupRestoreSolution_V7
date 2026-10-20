using SecureServerBackup.Enums;
using SecureServerBackup.Services;
using SecureServerBackup.WinForms;

using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureServerBackup.Helpers
{
	public static class RunBackupJobHelper
	{
		public static async System.Threading.Tasks.Task RunBackupJobAsync(BackupJob job, Form anyForm)
		{
			if (!CheckBackupService(anyForm))
			{
				return;
			}

			CustomDialogResult result = CustomDialogService.ShowQuestion(
				anyForm,
				$"Run backup job '{job.Name}' now?\n\nThe backup will run in the background service and continue even if you close this window.",
				"Run Backup");

			if (result != CustomDialogResult.Yes)
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

					var progressWindow = new BackupProgressForm(job.Id, job.Name);
					progressWindow.Show();
					return;
				}

				BackupLogger.LogError(job.Name, "Failed to communicate with Secure Server Backup Service - backup was not started");

				CustomDialogService.ShowError(
					anyForm,
					"Failed to start backup. The service may be busy or not responding.\n\nTry again in a few moments, or restart the Secure Server Backup Service from Windows Services.",
					"Service Error");
			}
			catch (Exception ex)
			{
				BackupLogger.LogError(job.Name, $"Error starting backup job from the Backup tab: {ex.Message}");

				CustomDialogService.ShowError(
					anyForm,
					$"Error starting backup job: {ex.Message}",
					"Run Backup Error");
			}
		}

		public static bool CheckBackupService(Form anyForm)
		{
			try
			{
				using var service = new System.ServiceProcess.ServiceController("SecureServerBackupService");

				if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running)
				{
					BackupLogger.LogWarning("System", $"Secure Server Backup Service is not running (Status: {service.Status})");

					CustomDialogResult result = CustomDialogService.ShowQuestion(
						anyForm,
						$"The Secure Server Backup Service is not running (Status: {service.Status}).\n\nWould you like to start it now?\n\nNote: You may need to run this application as Administrator to start the service.",
						"Service Not Running");

					if (result == CustomDialogResult.Yes)
					{
						try
						{
							BackupLogger.LogInfo("System", "Attempting to start Secure Server Backup Service...");
							service.Start();
							service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
							BackupLogger.LogInfo("System", "Secure Server Backup Service started successfully");

							CustomDialogService.ShowInfo(
								anyForm,
								"Secure Server Backup Service started successfully.",
								"Service Started");

							return true;
						}
						catch (Exception ex)
						{
							BackupLogger.LogError("System", $"Failed to start Secure Server Backup Service: {ex.Message}");

							CustomDialogService.ShowError(
								anyForm,
								$"Failed to start service: {ex.Message}\n\nPlease start the service manually from Windows Services (services.msc) or run this application as Administrator.",
								"Service Start Failed");

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

				CustomDialogResult result = CustomDialogService.ShowQuestion(anyForm,
					"The Secure Server Backup Service is not installed on this system.\n\nThe service must be installed before backups can run.\n\nTo install the service:\n1. Open PowerShell as Administrator\n2. Navigate to the solution folder\n3. Run: .\\Install-BackupService.ps1\n\nWould you like to open the solution folder now?",
					"Service Not Installed");

				if (result == CustomDialogResult.Yes)
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

				CustomDialogService.ShowError(
					anyForm,
					$"Error checking service status: {ex.Message}",
					"Service Check Error");

				return false;
			}
		}

	}
}
