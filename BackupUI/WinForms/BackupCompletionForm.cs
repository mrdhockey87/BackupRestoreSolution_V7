using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class BackupCompletionForm : Form
	{
		private const int AutoCloseMinutes = 15;
		private readonly Label titleLabel;
		private readonly Label iconLabel;
		private readonly Label messageLabel;
		private readonly Label countdownLabel;
		private readonly Button okButton;
		private readonly Timer countdownTimer;
		private TimeSpan remaining;
		private bool autoCloseEnabled = true;

		public BackupCompletionForm()
		{
			Font baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
			Text = "Backup Complete";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			TopMost = true;
			ClientSize = new Size(520, 250);
			BackColor = Color.White;

			iconLabel = new Label
			{
				Font = new Font(baseFont.FontFamily, 22F, FontStyle.Regular),
				AutoSize = true,
				Location = new Point(24, 18)
			};

			titleLabel = new Label
			{
				Font = new Font(baseFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(82, 26)
			};

			messageLabel = new Label
			{
				AutoSize = false,
				Location = new Point(24, 72),
				Size = new Size(472, 96)
			};

			countdownLabel = new Label
			{
				AutoSize = false,
				Location = new Point(24, 176),
				Size = new Size(472, 24)
			};

			okButton = new Button
			{
				Text = "OK",
				Size = new Size(100, 32),
				Location = new Point(396, 208),
				DialogResult = DialogResult.OK
			};
			okButton.Click += (_, _) =>
			{
				WasAutoClose = false;
				Close();
			};

			Controls.Add(iconLabel);
			Controls.Add(titleLabel);
			Controls.Add(messageLabel);
			Controls.Add(countdownLabel);
			Controls.Add(okButton);

			remaining = TimeSpan.FromMinutes(AutoCloseMinutes);
			countdownTimer = new Timer { Interval = 1000 };
			countdownTimer.Tick += CountdownTimer_Tick;
			Load += (_, _) => countdownTimer.Start();
			FormClosed += (_, _) => countdownTimer.Stop();
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
	}
}
