using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class BackupCompletionForm : Form
	{
		private const int AutoCloseMinutes = 15;
		private TimeSpan remaining;
		private bool autoCloseEnabled = true;

		public BackupCompletionForm()
		{
			InitializeComponent();
			remaining = TimeSpan.FromMinutes(AutoCloseMinutes);
		}

		public bool WasAutoClose { get; private set; }

		public void ConfigureSuccess(string jobName)
		{
			ConfigureAutoClose(true);
			titleLabel.Text = "Backup Complete";
			iconLabel.Text = "✅";
			iconLabel.ForeColor = Color.DarkGreen;
			messageLabel.Text = $"Backup job '{jobName}' completed successfully!";
			okButton.BackColor = SystemColors.Control;
		}

		public void ConfigureFailure(string jobName, string? errorMessage)
		{
			ConfigureAutoClose(true);
			titleLabel.Text = "Backup Failed";
			iconLabel.Text = "❌";
			iconLabel.ForeColor = Color.DarkRed;
			messageLabel.Text = $"Backup job '{jobName}' failed!{Environment.NewLine}{Environment.NewLine}Error: {errorMessage ?? "Unknown error"}{Environment.NewLine}{Environment.NewLine}Check Activity log for details.";
			okButton.BackColor = Color.MistyRose;
		}

		private void ConfigureAutoClose(bool enableAutoClose)
		{
			autoCloseEnabled = enableAutoClose;
			remaining = TimeSpan.FromMinutes(AutoCloseMinutes);
			countdownLabel.Text = enableAutoClose
				? $"This dialog will close automatically in {remaining:m\\:ss}"
				: "This dialog will remain open until you close it.";
		}

		private void CountdownTimer_Tick(object? sender, EventArgs e)
		{
			if (!autoCloseEnabled)
			{
				return;
			}

			remaining = remaining.Subtract(TimeSpan.FromSeconds(1));
			if (remaining <= TimeSpan.Zero)
			{
				WasAutoClose = true;
				Close();
				return;
			}

			countdownLabel.Text = $"This dialog will close automatically in {remaining:m\\:ss}";
		}

		private void BackupCompletionForm_Load(object? sender, EventArgs e)
		{
			countdownTimer.Start();
		}

		private void BackupCompletionForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			countdownTimer.Stop();
		}

		private void OkButton_Click(object? sender, EventArgs e)
		{
			WasAutoClose = false;
			Close();
		}
	}
}
