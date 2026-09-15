using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class BackupPasswordPromptForm
	{
		private IContainer components;
		private Label instructionLabel;
		private TextBox passwordTextBox;
		private CheckBox showPasswordCheckBox;
		private Label errorLabel;
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
			instructionLabel = new Label();
			passwordTextBox = new TextBox();
			showPasswordCheckBox = new CheckBox();
			errorLabel = new Label();
			okButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			instructionLabel.AutoSize = true;
			instructionLabel.Location = new Point(16, 20);
			instructionLabel.Name = "instructionLabel";
			instructionLabel.Size = new Size(176, 20);
			instructionLabel.TabIndex = 0;
			instructionLabel.Text = "Enter the backup password:";
			passwordTextBox.Location = new Point(16, 52);
			passwordTextBox.Name = "passwordTextBox";
			passwordTextBox.Size = new Size(372, 27);
			passwordTextBox.TabIndex = 1;
			passwordTextBox.UseSystemPasswordChar = true;
			showPasswordCheckBox.AutoSize = true;
			showPasswordCheckBox.Location = new Point(16, 84);
			showPasswordCheckBox.Name = "showPasswordCheckBox";
			showPasswordCheckBox.Size = new Size(129, 24);
			showPasswordCheckBox.TabIndex = 2;
			showPasswordCheckBox.Text = "Show password";
			showPasswordCheckBox.UseVisualStyleBackColor = true;
			showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;
			errorLabel.ForeColor = Color.DarkRed;
			errorLabel.Location = new Point(16, 112);
			errorLabel.Name = "errorLabel";
			errorLabel.Size = new Size(372, 34);
			errorLabel.TabIndex = 3;
			errorLabel.Visible = false;
			okButton.Location = new Point(208, 150);
			okButton.Name = "okButton";
			okButton.Size = new Size(90, 30);
			okButton.TabIndex = 4;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			okButton.Click += OkButton_Click;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(298, 150);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(90, 30);
			cancelButton.TabIndex = 5;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(420, 190);
			Controls.Add(cancelButton);
			Controls.Add(okButton);
			Controls.Add(errorLabel);
			Controls.Add(showPasswordCheckBox);
			Controls.Add(passwordTextBox);
			Controls.Add(instructionLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "BackupPasswordPromptForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Backup Password";
			Shown += BackupPasswordPromptForm_Shown;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
