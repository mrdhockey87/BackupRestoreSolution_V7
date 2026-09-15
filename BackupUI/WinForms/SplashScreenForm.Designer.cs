using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class SplashScreenForm
	{
		private IContainer components;
		private PictureBox logoPictureBox;
		private Label titleLabel;
		private Label versionLabel;
		private Label statusLabel;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			logoPictureBox = new PictureBox();
			titleLabel = new Label();
			versionLabel = new Label();
			statusLabel = new Label();
			((ISupportInitialize)logoPictureBox).BeginInit();
			SuspendLayout();
			logoPictureBox.BackColor = Color.Transparent;
			logoPictureBox.Location = new Point(200, 20);
			logoPictureBox.Name = "logoPictureBox";
			logoPictureBox.Size = new Size(200, 200);
			logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			logoPictureBox.TabIndex = 0;
			logoPictureBox.TabStop = false;
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 18F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.ForeColor = WinFormsThemeManager.PrimaryTurquoise;
			titleLabel.Location = new Point(160, 235);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(239, 35);
			titleLabel.TabIndex = 1;
			titleLabel.Text = "Secure Server Backup";
			versionLabel.AutoSize = true;
			versionLabel.ForeColor = WinFormsThemeManager.SecondaryText;
			versionLabel.Location = new Point(240, 275);
			versionLabel.Name = "versionLabel";
			versionLabel.Size = new Size(122, 20);
			versionLabel.TabIndex = 2;
			versionLabel.Text = "Version: Loading...";
			statusLabel.AutoSize = true;
			statusLabel.ForeColor = WinFormsThemeManager.SecondaryText;
			statusLabel.Location = new Point(250, 305);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(73, 20);
			statusLabel.TabIndex = 3;
			statusLabel.Text = "Starting...";
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = WinFormsThemeManager.LightTurquoise;
			ClientSize = new Size(600, 400);
			Controls.Add(statusLabel);
			Controls.Add(versionLabel);
			Controls.Add(titleLabel);
			Controls.Add(logoPictureBox);
			FormBorderStyle = FormBorderStyle.None;
			Name = "SplashScreenForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			TopMost = true;
			((ISupportInitialize)logoPictureBox).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
