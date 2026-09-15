using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class AboutForm
	{
		private IContainer components;
		private Label mainVersionLabel;
		private Label uiVersionLabel;
		private Label engineVersionLabel;
		private Label serviceVersionLabel;
		private Label serviceWarningLabel;
		private Button okButton;

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
			mainVersionLabel = new Label();
			uiVersionLabel = new Label();
			engineVersionLabel = new Label();
			serviceVersionLabel = new Label();
			serviceWarningLabel = new Label();
			okButton = new Button();
			SuspendLayout();
			mainVersionLabel.AutoSize = true;
			mainVersionLabel.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			mainVersionLabel.Location = new Point(20, 20);
			mainVersionLabel.Name = "mainVersionLabel";
			mainVersionLabel.Size = new Size(127, 20);
			mainVersionLabel.TabIndex = 0;
			mainVersionLabel.Text = "Version 0.0.0.0";
			uiVersionLabel.AutoSize = true;
			uiVersionLabel.Location = new Point(20, 70);
			uiVersionLabel.Name = "uiVersionLabel";
			uiVersionLabel.Size = new Size(146, 20);
			uiVersionLabel.TabIndex = 1;
			uiVersionLabel.Text = "UI Version: 0.0.0.0";
			engineVersionLabel.AutoSize = true;
			engineVersionLabel.Location = new Point(20, 105);
			engineVersionLabel.Name = "engineVersionLabel";
			engineVersionLabel.Size = new Size(177, 20);
			engineVersionLabel.TabIndex = 2;
			engineVersionLabel.Text = "Engine Version: 0.0.0.0";
			serviceVersionLabel.AutoSize = true;
			serviceVersionLabel.Location = new Point(20, 140);
			serviceVersionLabel.Name = "serviceVersionLabel";
			serviceVersionLabel.Size = new Size(174, 20);
			serviceVersionLabel.TabIndex = 3;
			serviceVersionLabel.Text = "Service Version: Loading";
			serviceWarningLabel.AutoSize = true;
			serviceWarningLabel.ForeColor = Color.DarkOrange;
			serviceWarningLabel.Location = new Point(20, 175);
			serviceWarningLabel.Name = "serviceWarningLabel";
			serviceWarningLabel.Size = new Size(0, 20);
			serviceWarningLabel.TabIndex = 4;
			okButton.DialogResult = DialogResult.OK;
			okButton.Location = new Point(310, 210);
			okButton.Name = "okButton";
			okButton.Size = new Size(80, 30);
			okButton.TabIndex = 5;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(420, 260);
			Controls.Add(okButton);
			Controls.Add(serviceWarningLabel);
			Controls.Add(serviceVersionLabel);
			Controls.Add(engineVersionLabel);
			Controls.Add(uiVersionLabel);
			Controls.Add(mainVersionLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "AboutForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "About Secure Server Backup";
			Load += AboutForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
