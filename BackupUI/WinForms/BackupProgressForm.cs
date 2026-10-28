using SecureServerBackup.Enums;
using SecureServerBackup.Services;

using SecureServerBackupCommon;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class BackupProgressForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly Guid jobId;
		private readonly string jobName;
		private readonly BackupServiceClient serviceClient;
		private bool isCompleted;
		private bool abortRequested;
		private DateTime? waitingStartTime;
		private readonly Form owner;
		/*public BackupProgressForm(null, Guid.Empty, "Sample Backup")
		{
		}
		
		public BackupProgressForm(Guid jobId, string jobName)
		{
			this.jobId = jobId;
			this.jobName = jobName;
			serviceClient = new BackupServiceClient();
			InitializeComponent();
			Text = $"Backup Progress: {jobName}";
		}*/

		public BackupProgressForm(Form _owner, Guid jobId, string jobName)
		{
			owner = _owner;
			this.jobId = jobId;
			this.jobName = jobName;
			StartPosition = FormStartPosition.CenterParent;
			serviceClient = new BackupServiceClient();
			InitializeComponent();
			Text = $"Backup Progress: {jobName}";
		}
		public bool WasClosedWhileBackupRunning => !isCompleted && !abortRequested;

		private async Task UpdateProgressAsync()
		{
			try
			{
				BackupProgress? progress = await serviceClient.GetProgressAsync(jobId);
				if (progress == null)
				{
					waitingStartTime ??= DateTime.Now;
					progressLabel.Text = DateTime.Now - waitingStartTime.Value < TimeSpan.FromSeconds(5)
						? "Initializing backup..."
						: "Waiting for backup to start...";
					return;
				}

				waitingStartTime = null;
				progressBar.Value = Math.Max(progressBar.Minimum, Math.Min(progressBar.Maximum, progress.Percentage));
				progressLabel.Text = progress.Message;
				percentageLabel.Text = $"{progress.Percentage}%";
				currentFileTextBox.Text = progress.CurrentFile ?? string.Empty;

				if (progress.IsVerifying)
				{
					Text = $"Verification Progress: {jobName}";
				}
				else if (progress.IsRunning)
				{
					Text = $"Backup Progress: {jobName}";
				}

				if (progress.IsRunning)
				{
					return;
				}

				progressTimer.Stop();
				isCompleted = true;
				abortButton.Enabled = false;

				if (progress.Success)
				{
					progressBar.Value = 100;
					percentageLabel.Text = "100%";
					progressLabel.Text = "Backup completed successfully!";
				}
				else
				{
					progressLabel.Text = $"Backup failed: {progress.ErrorMessage ?? "Unknown error"}";
				}

				hideCloseButton.Text = "Close";

				using var completionForm = new BackupCompletionForm();
				if (progress.Success)
				{
					completionForm.ConfigureSuccess(jobName);
				}
				else
				{
					completionForm.ConfigureFailure(jobName, progress.ErrorMessage);
				}

				completionForm.ShowDialog(this);
				if (completionForm.WasAutoClose)
				{
					Close();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error updating progress: {ex.Message}");
			}
		}

		private async Task AbortBackupAsync()
		{
			CustomDialogResult result = CustomDialogService.Show(this,
				$"Are you sure you want to abort the backup '{jobName}'?{Environment.NewLine}{Environment.NewLine}This cannot be undone.",
				"Abort Backup",
				DialogButtons.YesNo,
				DialogIcon.Warning);

			if (result != CustomDialogResult.Yes)
			{
				return;
			}

			abortButton.Enabled = false;
			progressLabel.Text = "Aborting backup...";
			abortRequested = true;

			bool success = await serviceClient.AbortBackupAsync(jobId);
			if (success)
			{
				isCompleted = true;
				progressTimer.Stop();
				CustomDialogService.ShowWarning(this,
					"Backup abort has been requested.\n\nIMPORTANT: The backup process may continue running in the background for a short time while it safely stops the current operation.\n\nThe backup file may be incomplete and should be deleted.",
					"Backup Abort Requested");
				Close();
				return;
			}

			CustomDialogService.ShowWarning(this,
				"Failed to send abort request. Please try again or check if the service is running.",
				"Abort Failed");
			abortButton.Enabled = true;
			abortRequested = false;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!isCompleted && !abortRequested)
			{
				CustomDialogResult result = CustomDialogService.Show(this,
					"Backup is still running in the background.\n\nClosing this window will not stop the backup.\n\nYou can reopen this window from the main window to view progress again.",
					"Backup Still Running",
					DialogButtons.OKCancel,
					DialogIcon.Information);
				if (result == CustomDialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
			}

			progressTimer.Stop();
			base.OnClosing(e);
		}

		private async void BackupProgressForm_Load(object? sender, EventArgs e)
		{
			if (IsInDesignMode)
			{
				return;
			}

			progressTimer.Start();

			try
			{
				await UpdateProgressAsync();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading backup progress: {ex.Message}");
			}
		}

		private async void ProgressTimer_Tick(object? sender, EventArgs e)
		{
			if (IsInDesignMode)
			{
				return;
			}

			await UpdateProgressAsync();
		}

		private async void AbortButton_Click(object? sender, EventArgs e)
		{
			if (IsInDesignMode)
			{
				return;
			}

			await AbortBackupAsync();
		}

		private void HideCloseButton_Click(object? sender, EventArgs e)
		{
			Close();
		}
	}
}
