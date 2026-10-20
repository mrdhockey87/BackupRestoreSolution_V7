using SecureServerBackup.Enums;
using SecureServerBackup.Services;

using System;
using System.IO;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	public partial class NetworkPathForm : Form
	{
		public string NetworkPath { get; private set; } = string.Empty;

		public NetworkPathForm()
		{
			InitializeComponent();
		}

		private void NetworkPathForm_Load(object sender, EventArgs e)
		{
			txtNetworkPath.Focus();
			txtNetworkPath.SelectAll();
		}

		private void Browse_Click(object sender, EventArgs e)
		{
			using FolderBrowserDialog dialog = new()
			{
				Description = "Browse available network locations and select a shared folder"
			};

			if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
			{
				return;
			}

			txtNetworkPath.Text = dialog.SelectedPath;
			txtNetworkPath.SelectionStart = txtNetworkPath.Text.Length;
			txtNetworkPath.Focus();
		}

		private void OK_Click(object sender, EventArgs e)
		{
			var path = txtNetworkPath.Text.Trim();

			// Validate UNC path format
			if (string.IsNullOrWhiteSpace(path))
			{
				CustomDialogService.ShowWarning("Please enter a network path.", "Validation Error");
				return;
			}

			if (!path.StartsWith("\\\\"))
			{
				CustomDialogService.ShowWarning("Network path must start with \\\\ (UNC format).\n\nExample: \\\\server\\share",
					"Validation Error");
				return;
			}

			// Check if path is accessible
			try
			{
				if (!Directory.Exists(path))
				{
					CustomDialogResult result = CustomDialogService.ShowQuestion(this,
						$"Cannot access network path:\n{path}\n\nThe path may not exist or you may not have permissions.\n\nAdd anyway?",
						"Network Path Not Accessible");

					if (result != CustomDialogResult.Yes)
					{
						return;
					}
					else
					{
						DialogResult = DialogResult.Cancel;
						Close();
					}
				}
			}
			catch (Exception ex)
			{
				CustomDialogResult result = CustomDialogService.Show(this,
					$"Error checking network path:\n{ex.Message}\n\nAdd anyway?",
					"Network Path Error",
					DialogButtons.YesNo,
					DialogIcon.Warning);

				if (result != CustomDialogResult.Yes)
				{
					return;
				}
				else
				{
					DialogResult = DialogResult.Cancel;
					Close();
				}
			}

			NetworkPath = path;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
