using SecureServerBackup.Services;

using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureServerBackup.Helpers
{
	//Check to make sure the service is installed and running. If not, install and start it automatically. mdail 10/21/2026
	internal class CheckBackupServiceHelper
	{
		private Form anyForm;
		public CheckBackupServiceHelper(Form _anyForm)
		{
			anyForm = _anyForm;
		}

		public async Task<bool> CheckBackupServiceAsync()
		{
			try
			{
				if (!ServiceInstaller.IsServiceInstalled())
				{
					BackupLogger.LogServiceInfo("Secure Server Backup Service not installed - installing automatically...");
					(bool success, string message) = await ServiceInstaller.InstallAndStartServiceAsync();
					if (!success)
					{
						BackupLogger.LogServiceError($"Failed to install service: {message}");
						CustomDialogService.ShowError(anyForm, $"Failed to install service:\n\n{message}\n\nPlease ensure the application has Administrator privileges.", "Installation Failed");
						return false;
					}

					BackupLogger.LogServiceInfo("Secure Server Backup Service installed and started successfully");
					return true;
				}

				System.ServiceProcess.ServiceControllerStatus? status = ServiceInstaller.GetServiceStatus();
				if (status != System.ServiceProcess.ServiceControllerStatus.Running)
				{
					BackupLogger.LogServiceInfo($"Secure Server Backup Service not running (Status: {status}) - starting automatically...");
					(bool success, string message) = await ServiceInstaller.StartServiceAsync();
					if (!success)
					{
						BackupLogger.LogServiceError($"Failed to start service: {message}");
						CustomDialogService.ShowError(anyForm, $"Failed to start service:\n\n{message}", "Start Failed");
						return false;
					}

					BackupLogger.LogServiceInfo("Secure Server Backup Service started successfully");
				}

				return true;
			}
			catch (Exception ex)
			{
				BackupLogger.LogServiceError($"Error checking service status: {ex.Message}");
				CustomDialogService.ShowError(anyForm, $"Error checking service status:\n\n{ex.Message}", "Service Check Error");
				return false;
			}
		}

	}
}
