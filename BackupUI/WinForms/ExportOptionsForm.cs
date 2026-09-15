using System;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class ExportOptionsForm : Form
	{
		public ExportOptionsForm()
		{
			InitializeComponent();
		}

		public string ExportFormat => csvRadioButton.Checked ? "CSV" : "Text";
	}
}
