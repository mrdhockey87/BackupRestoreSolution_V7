using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class BackupPasswordPromptForm : Form
	{
		private readonly TextBox passwordTextBox;
		private readonly CheckBox showPasswordCheckBox;
		private readonly Label errorLabel;

		public BackupPasswordPromptForm(string backupName)
		{
			Text = $"Backup Password - {backupName}";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(420, 190);
			BackColor = Color.White;

			var instructionLabel = new Label
			{
				Text = "Enter the backup password:",
				AutoSize = true,
				Location = new Point(16, 20)
			};

			passwordTextBox = new TextBox
			{
				Location = new Point(16, 52),
				Size = new Size(372, 24),
				UseSystemPasswordChar = true
			};

			showPasswordCheckBox = new CheckBox
			{
				Text = "Show password",
				AutoSize = true,
				Location = new Point(16, 84)
			};
			showPasswordCheckBox.CheckedChanged += (_, _) => passwordTextBox.UseSystemPasswordChar = !showPasswordCheckBox.Checked;

			errorLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 112),
				Size = new Size(372, 34),
				ForeColor = Color.DarkRed,
				Visible = false
			};

			var okButton = new Button
			{
				Text = "OK",
				Size = new Size(90, 30),
				Location = new Point(208, 150)
			};
			okButton.Click += (_, _) => Confirm();

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(90, 30),
				Location = new Point(298, 150),
				DialogResult = DialogResult.Cancel
			};

			Controls.Add(instructionLabel);
			Controls.Add(passwordTextBox);
			Controls.Add(showPasswordCheckBox);
			Controls.Add(errorLabel);
			Controls.Add(okButton);
			Controls.Add(cancelButton);

			AcceptButton = okButton;
			CancelButton = cancelButton;
			Shown += (_, _) => passwordTextBox.Focus();
		}

		public string EnteredPassword { get; private set; } = string.Empty;

		public void SetError(string message)
		{
			errorLabel.Text = message;
			errorLabel.Visible = !string.IsNullOrWhiteSpace(message);
		}

		private void Confirm()
		{
			string password = passwordTextBox.Text;
			if (string.IsNullOrWhiteSpace(password))
			{
				SetError("Please enter the backup password.");
				return;
			}

			EnteredPassword = password;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
