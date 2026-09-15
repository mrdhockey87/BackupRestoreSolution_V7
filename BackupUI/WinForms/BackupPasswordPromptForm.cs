using System;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class BackupPasswordPromptForm : Form
	{
		public BackupPasswordPromptForm()
			: this("Sample Backup")
		{
		}

		public BackupPasswordPromptForm(string backupName)
		{
			InitializeComponent();
			Text = $"Backup Password - {backupName}";
			errorLabel.Visible = false;
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

		private void ShowPasswordCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			passwordTextBox.UseSystemPasswordChar = !showPasswordCheckBox.Checked;
		}

		private void OkButton_Click(object? sender, EventArgs e)
		{
			Confirm();
		}

		private void BackupPasswordPromptForm_Shown(object? sender, EventArgs e)
		{
			passwordTextBox.Focus();
		}
	}
}
