using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Services;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed class BackupProgressForm : Form
	{
		private readonly Guid jobId;
		private readonly string jobName;
		private readonly BackupServiceClient serviceClient;
		private readonly Timer progressTimer;
		private readonly ProgressBar progressBar;
		private readonly Label progressLabel;
		private readonly Label percentageLabel;
		private readonly TextBox currentFileTextBox;
		private readonly Button abortButton;
		private readonly Button hideCloseButton;
		private bool isCompleted;
		private bool abortRequested;
		private DateTime? waitingStartTime;

		public BackupProgressForm(Guid jobId, string jobName)
		{
			this.jobId = jobId;
			this.jobName = jobName;
			serviceClient = new BackupServiceClient();

			Text = $"Backup Progress: {jobName}";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(660, 320);
			ClientSize = new Size(660, 320);
			BackColor = Color.White;

			progressLabel = new Label
			{
				Text = "Initializing backup...",
				AutoSize = false,
				Location = new Point(20, 20),
				Size = new Size(600, 24)
			};

			progressBar = new ProgressBar
			{
				Location = new Point(20, 56),
				Size = new Size(600, 24),
				Minimum = 0,
				Maximum = 100
			};

			percentageLabel = new Label
			{
				Text = "0%",
				AutoSize = true,
				Location = new Point(20, 90)
			};

			var currentFileLabel = new Label
			{
				Text = "Current File:",
				AutoSize = true,
				Location = new Point(20, 122)
			};

			currentFileTextBox = new TextBox
			{
				Location = new Point(20, 146),
				Size = new Size(600, 96),
				ReadOnly = true,
				Multiline = true,
				ScrollBars = ScrollBars.Vertical
			};

			abortButton = new Button
			{
				Text = "Abort",
				Size = new Size(100, 32),
				Location = new Point(410, 262),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right
			};
			abortButton.Click += async (_, _) => await AbortBackupAsync();

			hideCloseButton = new Button
			{
				Text = "Hide",
				Size = new Size(100, 32),
				Location = new Point(520, 262),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right
			};
			hideCloseButton.Click += (_, _) => Close();

			Controls.Add(progressLabel);
			Controls.Add(progressBar);
			Controls.Add(percentageLabel);
			Controls.Add(currentFileLabel);
			Controls.Add(currentFileTextBox);
			Controls.Add(abortButton);
			Controls.Add(hideCloseButton);

			progressTimer = new Timer { Interval = 1000 };
			progressTimer.Tick += async (_, _) => await UpdateProgressAsync();
			Load += async (_, _) =>
			{
				progressTimer.Start();
				await UpdateProgressAsync();
			};
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
			DialogResult result = MessageBox.Show(this,
				$"Are you sure you want to abort the backup '{jobName}'?{Environment.NewLine}{Environment.NewLine}This cannot be undone.",
				"Abort Backup",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if (result != DialogResult.Yes)
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
				MessageBox.Show(this,
					"Backup abort has been requested.\n\nIMPORTANT: The backup process may continue running in the background for a short time while it safely stops the current operation.\n\nThe backup file may be incomplete and should be deleted.",
					"Backup Abort Requested",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				Close();
				return;
			}

			MessageBox.Show(this,
				"Failed to send abort request. Please try again or check if the service is running.",
				"Abort Failed",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			abortButton.Enabled = true;
			abortRequested = false;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!isCompleted && !abortRequested)
			{
				DialogResult result = MessageBox.Show(this,
					"Backup is still running in the background.\n\nClosing this window will not stop the backup.\n\nYou can reopen this window from the main window to view progress again.",
					"Backup Still Running",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Information);
				if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
			}

			progressTimer.Stop();
			base.OnClosing(e);
		}
	}
}
