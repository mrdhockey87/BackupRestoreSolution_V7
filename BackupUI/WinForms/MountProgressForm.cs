using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	public partial class MountProgressForm : Form
	{
		private bool _isClosed = false;

		private static readonly Color BorderColor = Color.FromArgb(200, 200, 200);

		public MountProgressForm()
		{
			InitializeComponent();
			this.Paint += MountProgressForm_Paint;
		}

		private void MountProgressForm_Paint(object sender, PaintEventArgs e)
		{
			using var pen = new Pen(BorderColor, 1);
			e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
		}

		/// <summary>
		/// Update backup name
		/// </summary>
		public void SetBackupName(string name)
		{
			if (_isClosed || txtBackupName == null) return;

			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => SetBackupName(name)));
				return;
			}

			txtBackupName.Text = $"Backup: {name}";
		}

		/// <summary>
		/// Update status message and optionally progress percentage
		/// </summary>
		public void SetStatus(string status, int percentage = -1)
		{
			if (_isClosed || txtStatus == null) return;

			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => SetStatus(status, percentage)));
				return;
			}

			// Parse message to distinguish file-level vs general progress (v6.0.1.19)
			if (status.Contains("Processing:") || status.Contains("Backing up:"))
			{
				if (txtCurrentFile != null)
				{
					txtCurrentFile.Text = status;
				}
			}
			else
			{
				txtStatus.Text = status;

				if (txtCurrentFile != null &&
					!status.Contains("Capturing") &&
					!status.Contains("Mounting") &&
					!status.Contains("Processing"))
				{
					txtCurrentFile.Text = "";
				}
			}

			if (percentage >= 0)
			{
				SetProgress(percentage);
			}
		}

		/// <summary>
		/// Set progress percentage (0-100), or -1 for indeterminate
		/// </summary>
		public void SetProgress(int percentage)
		{
			if (_isClosed || progressBar == null) return;

			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => SetProgress(percentage)));
				return;
			}

			if (percentage < 0)
			{
				if (progressBar.Style != ProgressBarStyle.Marquee)
				{
					progressBar.Style = ProgressBarStyle.Marquee;
					progressBar.MarqueeAnimationSpeed = 30;
				}
			}
			else
			{
				if (progressBar.Style != ProgressBarStyle.Continuous)
				{
					progressBar.Style = ProgressBarStyle.Continuous;
					progressBar.MarqueeAnimationSpeed = 0;
				}
				progressBar.Maximum = 100;
				progressBar.Value = Math.Max(0, Math.Min(100, percentage));
			}
		}

		/// <summary>
		/// Close the progress window
		/// </summary>
		public void CloseProgress()
		{
			if (_isClosed) return;
			_isClosed = true;

			if (InvokeRequired)
			{
				BeginInvoke(new Action(() =>
				{
					try { Close(); } catch { /* already closed */ }
				}));
				return;
			}

			try { Close(); } catch { /* already closed */ }
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_isClosed = true;
			base.OnFormClosed(e);
		}
	}
}