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
				MessageBox.Show("Please enter a network path.", "Validation Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (!path.StartsWith("\\\\"))
			{
				MessageBox.Show("Network path must start with \\\\ (UNC format).\n\nExample: \\\\server\\share",
					"Validation Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Check if path is accessible
			try
			{
				if (!Directory.Exists(path))
				{
					var result = MessageBox.Show(
						$"Cannot access network path:\n{path}\n\nThe path may not exist or you may not have permissions.\n\nAdd anyway?",
						"Network Path Not Accessible",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning);

					if (result != DialogResult.Yes)
						return;
				}
			}
			catch (Exception ex)
			{
				var result = MessageBox.Show(
					$"Error checking network path:\n{ex.Message}\n\nAdd anyway?",
					"Network Path Error",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);

				if (result != DialogResult.Yes)
					return;
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
