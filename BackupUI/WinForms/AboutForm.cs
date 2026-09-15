using System;
using System.ComponentModel;
using System.Drawing;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Services;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class AboutForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		public AboutForm()
		{
			InitializeComponent();
			mainVersionLabel.Text = "Version 0.0.0.0";
			uiVersionLabel.Text = "UI Version: 0.0.0.0";
			engineVersionLabel.Text = "Engine Version: 0.0.0.0";
			serviceVersionLabel.Text = "Service Version: Loading...";
			serviceWarningLabel.Text = string.Empty;
		}

		private async void AboutForm_Load(object? sender, EventArgs e)
		{
			if (IsInDesignMode)
			{
				return;
			}

			try
			{
				await LoadVersionsAsync();
			}
			catch (InvalidOperationException)
			{
				serviceVersionLabel.Text = "Service Version: Not Installed";
				serviceWarningLabel.Text = "Not installed";
			}
		}

		private async Task LoadVersionsAsync()
		{
			var uiVersion = VersionClass.GetAssemblyVersion();
			mainVersionLabel.Text = $"Version {uiVersion}";
			uiVersionLabel.Text = $"UI Version: {uiVersion}";
			engineVersionLabel.Text = $"Engine Version: {uiVersion}";
			await LoadServiceVersionAsync(uiVersion);
		}

		private async Task LoadServiceVersionAsync(string uiVersion)
		{
			serviceVersionLabel.Text = "Service Version: Checking...";
			serviceWarningLabel.Text = string.Empty;

			try
			{
				using var service = new ServiceController("SecureServerBackupService");
				if (service.Status == ServiceControllerStatus.Running)
				{
					var serviceClient = new BackupServiceClient();
					var versionTask = serviceClient.GetServiceVersionAsync();
					var completedTask = await Task.WhenAny(versionTask, Task.Delay(3000));
					var serviceVersion = completedTask == versionTask ? await versionTask : null;

					if (!string.IsNullOrWhiteSpace(serviceVersion))
					{
						serviceVersionLabel.Text = $"Service Version: {serviceVersion}";
						serviceWarningLabel.Text = serviceVersion != uiVersion ? "Version mismatch" : string.Empty;
						serviceWarningLabel.ForeColor = serviceVersion != uiVersion ? Color.DarkRed : Color.DarkGreen;
					}
					else
					{
						serviceVersionLabel.Text = "Service Version: Unknown (old version)";
						serviceWarningLabel.Text = "Reinstall required";
					}
				}
				else
				{
					serviceVersionLabel.Text = $"Service Version: N/A ({service.Status})";
					serviceWarningLabel.Text = service.Status.ToString();
				}
			}
			catch (InvalidOperationException)
			{
				serviceVersionLabel.Text = "Service Version: Not Installed";
				serviceWarningLabel.Text = "Not installed";
			}
			catch (Exception ex)
			{
				serviceVersionLabel.Text = $"Service Version: Error: {ex.Message}";
				serviceWarningLabel.Text = string.Empty;
			}
		}

	}
}
