using System;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestoreCompletionDialogForm : Form
	{
		private const int AutoCloseMinutes = 15;
		private TimeSpan remaining;
		private bool autoCloseEnabled = true;

		public bool WasAutoClose { get; private set; }

		public RestoreCompletionDialogForm()
		{
			InitializeComponent();
			remaining = TimeSpan.FromMinutes(AutoCloseMinutes);
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

		private void RestoreCompletionDialogForm_Load(object? sender, EventArgs e)
		{
			countdownTimer.Start();
		}

		private void RestoreCompletionDialogForm_FormClosed(object? sender, FormClosedEventArgs e)
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
