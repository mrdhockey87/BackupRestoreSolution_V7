using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class BackupCompletionForm
	{
		private IContainer components;
		private Label titleLabel;
		private Label iconLabel;
		private Label messageLabel;
		private Label countdownLabel;
		private Button okButton;
		private Timer countdownTimer;

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
			countdownTimer = new Timer(components);
			titleLabel = new Label();
			iconLabel = new Label();
			messageLabel = new Label();
			countdownLabel = new Label();
			okButton = new Button();
			SuspendLayout();
			countdownTimer.Interval = 1000;
			countdownTimer.Tick += CountdownTimer_Tick;
			iconLabel.AutoSize = true;
			iconLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 22F, FontStyle.Regular, GraphicsUnit.Point);
			iconLabel.Location = new Point(24, 18);
			iconLabel.Name = "iconLabel";
			iconLabel.Size = new Size(33, 42);
			iconLabel.TabIndex = 0;
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			titleLabel.Location = new Point(82, 26);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(121, 20);
			titleLabel.TabIndex = 1;
			titleLabel.Text = "Backup Status";
			messageLabel.Location = new Point(24, 72);
			messageLabel.Name = "messageLabel";
			messageLabel.Size = new Size(472, 96);
			messageLabel.TabIndex = 2;
			countdownLabel.Location = new Point(24, 176);
			countdownLabel.Name = "countdownLabel";
			countdownLabel.Size = new Size(472, 24);
			countdownLabel.TabIndex = 3;
			okButton.DialogResult = DialogResult.OK;
			okButton.Location = new Point(396, 208);
			okButton.Name = "okButton";
			okButton.Size = new Size(100, 32);
			okButton.TabIndex = 4;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			okButton.Click += OkButton_Click;
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(520, 250);
			Controls.Add(okButton);
			Controls.Add(countdownLabel);
			Controls.Add(messageLabel);
			Controls.Add(titleLabel);
			Controls.Add(iconLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "BackupCompletionForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Backup Complete";
			TopMost = true;
			FormClosed += BackupCompletionForm_FormClosed;
			Load += BackupCompletionForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
