using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class RestoreCompletionDialogForm : Form
	{
		private const int AutoCloseMinutes = 15;
		private readonly Label countdownLabel;
		private readonly Timer countdownTimer;
		private TimeSpan remaining;
		private bool autoCloseEnabled = true;

		public bool WasAutoClose { get; private set; }

		public RestoreCompletionDialogForm()
		{
			Font baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
			Text = "Restore Complete";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			TopMost = true;
			ClientSize = new Size(480, 220);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Restore Complete",
				Font = new Font(baseFont, FontStyle.Bold),
				ForeColor = Color.DarkGreen,
				AutoSize = true,
				Location = new Point(86, 24)
			};

			var iconLabel = new Label
			{
				Text = "✅",
				Font = new Font(baseFont.FontFamily, 22F, FontStyle.Regular),
				AutoSize = true,
				Location = new Point(28, 16)
			};

			var messageLabel = new Label
			{
				Text = "Restore completed successfully!",
				AutoSize = false,
				Location = new Point(28, 76),
				Size = new Size(420, 40)
			};

			countdownLabel = new Label
			{
				AutoSize = false,
				Location = new Point(28, 128),
				Size = new Size(420, 24)
			};

			var okButton = new Button
			{
				Text = "OK",
				Size = new Size(100, 32),
				Location = new Point(348, 170),
				DialogResult = DialogResult.OK
			};
			okButton.Click += (_, _) =>
			{
				WasAutoClose = false;
				Close();
			};

			Controls.Add(titleLabel);
			Controls.Add(iconLabel);
			Controls.Add(messageLabel);
			Controls.Add(countdownLabel);
			Controls.Add(okButton);

			remaining = TimeSpan.FromMinutes(AutoCloseMinutes);
			countdownTimer = new Timer { Interval = 1000 };
			countdownTimer.Tick += CountdownTimer_Tick;
			Load += (_, _) => countdownTimer.Start();
			FormClosed += (_, _) => countdownTimer.Stop();
		}

		public void ConfigureRestoreSuccess(bool enableAutoClose)
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
