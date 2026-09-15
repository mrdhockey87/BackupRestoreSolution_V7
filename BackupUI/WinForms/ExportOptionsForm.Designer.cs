using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class ExportOptionsForm
	{
		private IContainer components;
		private Label titleLabel;
		private RadioButton csvRadioButton;
		private RadioButton textRadioButton;
		private Button okButton;
		private Button cancelButton;

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
			csvRadioButton = new RadioButton();
			textRadioButton = new RadioButton();
			okButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			titleLabel.AutoSize = true;
			titleLabel.Location = new Point(20, 20);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(151, 20);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Choose export format:";
			csvRadioButton.AutoSize = true;
			csvRadioButton.Checked = true;
			csvRadioButton.Location = new Point(24, 56);
			csvRadioButton.Name = "csvRadioButton";
			csvRadioButton.Size = new Size(57, 24);
			csvRadioButton.TabIndex = 1;
			csvRadioButton.TabStop = true;
			csvRadioButton.Text = "CSV";
			csvRadioButton.UseVisualStyleBackColor = true;
			textRadioButton.AutoSize = true;
			textRadioButton.Location = new Point(24, 84);
			textRadioButton.Name = "textRadioButton";
			textRadioButton.Size = new Size(58, 24);
			textRadioButton.TabIndex = 2;
			textRadioButton.Text = "Text";
			textRadioButton.UseVisualStyleBackColor = true;
			okButton.DialogResult = DialogResult.OK;
			okButton.Location = new Point(116, 122);
			okButton.Name = "okButton";
			okButton.Size = new Size(90, 30);
			okButton.TabIndex = 3;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(216, 122);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(90, 30);
			cancelButton.TabIndex = 4;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(320, 170);
			Controls.Add(cancelButton);
			Controls.Add(okButton);
			Controls.Add(textRadioButton);
			Controls.Add(csvRadioButton);
			Controls.Add(titleLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ExportOptionsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Export Options";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
