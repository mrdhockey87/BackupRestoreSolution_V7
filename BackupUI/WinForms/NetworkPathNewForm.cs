//NetworkPathForm.cs
using SecureServerBackup.Services;

using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	public partial class NetworkPathNewForm : Form
	{
		public NetworkPathNewForm()
		{
			InitializeComponent();
		}

		public string NetworkPath => txtPath.Text.Trim();

		private void btnOk_Click(object? sender, System.EventArgs e)
		{
			if (!Regex.IsMatch(NetworkPath, @"^\\\$^\$+\$^\$+"))
			{
				CustomDialogService.ShowWarning(this,
					@"Enter a UNC path such as \\server\share.",
					"Invalid Path");
				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
