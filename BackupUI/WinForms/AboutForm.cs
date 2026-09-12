using System;
using System.Drawing;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Services;

namespace SecureServerBackup.WinForms
{
	internal sealed class AboutForm : Form
	{
		private readonly Label mainVersionLabel;
		private readonly Label uiVersionLabel;
		private readonly Label engineVersionLabel;
		private readonly Label serviceVersionLabel;
		private readonly Label serviceWarningLabel;

		public AboutForm()
		{
			var baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;

			Text = "About Secure Server Backup";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			StartPosition = FormStartPosition.CenterParent;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(420, 260);

			mainVersionLabel = CreateLabel(new Point(20, 20), baseFont, bold: true);
			uiVersionLabel = CreateLabel(new Point(20, 70), baseFont);
			engineVersionLabel = CreateLabel(new Point(20, 105), baseFont);
			serviceVersionLabel = CreateLabel(new Point(20, 140), baseFont);
			serviceWarningLabel = CreateLabel(new Point(20, 175), baseFont);
			serviceWarningLabel.ForeColor = Color.DarkOrange;

			var okButton = new Button
			{
				Text = "OK",
				DialogResult = DialogResult.OK,
				Location = new Point(310, 210),
				Size = new Size(80, 30)
			};

			Controls.Add(mainVersionLabel);
			Controls.Add(uiVersionLabel);
			Controls.Add(engineVersionLabel);
			Controls.Add(serviceVersionLabel);
			Controls.Add(serviceWarningLabel);
			Controls.Add(okButton);

			AcceptButton = okButton;
			Load += async (_, _) => await LoadVersionsAsync();
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

		private static Label CreateLabel(Point location, Font baseFont, bool bold = false)
		{
			return new Label
			{
				AutoSize = true,
				Location = location,
				Font = bold ? new Font(baseFont, FontStyle.Bold) : baseFont
			};
		}
	}
}
