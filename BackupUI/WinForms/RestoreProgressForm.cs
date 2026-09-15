using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Services;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestoreProgressForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly Func<BackupEngineInterop.ProgressCallback, Task> restoreOperation;
		private readonly bool keepWindowOpen;
		private bool isCompleted;

		public RestoreProgressForm()
			: this("Sample Restore", true, _ => Task.CompletedTask)
		{
		}

		public RestoreProgressForm(string restoreName, bool keepWindowOpen, Func<BackupEngineInterop.ProgressCallback, Task> restoreOperation)
		{
			ArgumentNullException.ThrowIfNull(restoreOperation);
			this.restoreOperation = restoreOperation;
			this.keepWindowOpen = keepWindowOpen;

			InitializeComponent();
			Text = $"Restore Progress: {restoreName}";
		}

		public bool RestoreSucceeded { get; private set; }

		private async Task RunRestoreAsync()
		{
			try
			{
				await restoreOperation(UpdateProgress);
				RestoreSucceeded = true;
				isCompleted = true;
				progressBar.Value = 100;
				percentageLabel.Text = "100%";
				progressLabel.Text = "Restore completed successfully!";
				closeButton.Text = "Close";

				using var completionDialog = new RestoreCompletionDialogForm();
				completionDialog.ConfigureRestoreSuccess(!keepWindowOpen);
				completionDialog.ShowDialog(this);
				if (!keepWindowOpen && completionDialog.WasAutoClose)
				{
					Close();
				}
			}
			catch (Exception ex)
			{
				isCompleted = true;
				progressLabel.Text = $"Restore failed: {ex.Message}";
				closeButton.Text = "Close";
				MessageBox.Show(this, $"Restore failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateProgress(int percentage, string? message)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => UpdateProgress(percentage, message)));
				return;
			}

			if (percentage >= 0)
			{
				progressBar.Value = Math.Max(progressBar.Minimum, Math.Min(progressBar.Maximum, percentage));
				percentageLabel.Text = $"{percentage}%";
			}

			if (!string.IsNullOrWhiteSpace(message))
			{
				progressLabel.Text = message;
				currentItemTextBox.Text = message;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!isCompleted)
			{
				DialogResult result = MessageBox.Show(this,
					"Restore is still running in the background.\n\nClosing this window will not stop the restore.",
					"Restore Still Running",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Information);
				if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
			}

			base.OnFormClosing(e);
		}

		private async void RestoreProgressForm_Load(object? sender, EventArgs e)
		{
			if (IsInDesignMode)
			{
				return;
			}

			await RunRestoreAsync();
		}

		private void CloseButton_Click(object? sender, EventArgs e)
		{
			Close();
		}
	}
}
