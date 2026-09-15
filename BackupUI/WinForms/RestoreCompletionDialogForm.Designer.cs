using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class RestoreCompletionDialogForm
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
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			titleLabel.ForeColor = Color.DarkGreen;
			titleLabel.Location = new Point(86, 24);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(123, 20);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Restore Complete";
			iconLabel.AutoSize = true;
			iconLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 22F, FontStyle.Regular, GraphicsUnit.Point);
			iconLabel.Location = new Point(28, 16);
			iconLabel.Name = "iconLabel";
			iconLabel.Size = new Size(33, 42);
			iconLabel.TabIndex = 1;
			iconLabel.Text = "✅";
			messageLabel.Location = new Point(28, 76);
			messageLabel.Name = "messageLabel";
			messageLabel.Size = new Size(420, 40);
			messageLabel.TabIndex = 2;
			messageLabel.Text = "Restore completed successfully!";
			countdownLabel.Location = new Point(28, 128);
			countdownLabel.Name = "countdownLabel";
			countdownLabel.Size = new Size(420, 24);
			countdownLabel.TabIndex = 3;
			okButton.DialogResult = DialogResult.OK;
			okButton.Location = new Point(348, 170);
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
			ClientSize = new Size(480, 220);
			Controls.Add(okButton);
			Controls.Add(countdownLabel);
			Controls.Add(messageLabel);
			Controls.Add(iconLabel);
			Controls.Add(titleLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "RestoreCompletionDialogForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore Complete";
			TopMost = true;
			FormClosed += RestoreCompletionDialogForm_FormClosed;
			Load += RestoreCompletionDialogForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
