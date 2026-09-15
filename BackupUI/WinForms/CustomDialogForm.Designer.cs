using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class CustomDialogForm
	{
		private IContainer components;
		private Label titleLabel;
		private Label iconLabel;
		private TextBox messageTextBox;
		private FlowLayoutPanel buttonPanel;

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
			titleLabel = new Label();
			iconLabel = new Label();
			messageTextBox = new TextBox();
			buttonPanel = new FlowLayoutPanel();
			SuspendLayout();
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			titleLabel.ForeColor = Color.FromArgb(32, 178, 170);
			titleLabel.Location = new Point(70, 20);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(70, 20);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Message";
			iconLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 20F, FontStyle.Regular, GraphicsUnit.Point);
			iconLabel.Location = new Point(20, 16);
			iconLabel.Name = "iconLabel";
			iconLabel.Size = new Size(36, 36);
			iconLabel.TabIndex = 1;
			iconLabel.TextAlign = ContentAlignment.MiddleCenter;
			messageTextBox.BackColor = Color.White;
			messageTextBox.BorderStyle = BorderStyle.None;
			messageTextBox.Location = new Point(20, 68);
			messageTextBox.Multiline = true;
			messageTextBox.Name = "messageTextBox";
			messageTextBox.ReadOnly = true;
			messageTextBox.Size = new Size(480, 106);
			messageTextBox.TabIndex = 2;
			messageTextBox.TabStop = false;
			buttonPanel.Dock = DockStyle.Bottom;
			buttonPanel.FlowDirection = FlowDirection.RightToLeft;
			buttonPanel.Location = new Point(0, 186);
			buttonPanel.Name = "buttonPanel";
			buttonPanel.Padding = new Padding(12, 10, 12, 10);
			buttonPanel.Size = new Size(520, 54);
			buttonPanel.TabIndex = 3;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(520, 240);
			Controls.Add(buttonPanel);
			Controls.Add(messageTextBox);
			Controls.Add(iconLabel);
			Controls.Add(titleLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "CustomDialogForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Message";
			TopMost = true;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
