using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class ExportOptionsForm : Form
	{
		private readonly RadioButton csvRadioButton;
		private readonly RadioButton textRadioButton;

		public ExportOptionsForm()
		{
			Text = "Export Options";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(320, 170);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Choose export format:",
				AutoSize = true,
				Location = new Point(20, 20)
			};

			csvRadioButton = new RadioButton
			{
				Text = "CSV",
				AutoSize = true,
				Location = new Point(24, 56),
				Checked = true
			};

			textRadioButton = new RadioButton
			{
				Text = "Text",
				AutoSize = true,
				Location = new Point(24, 84)
			};

			var okButton = new Button
			{
				Text = "OK",
				Size = new Size(90, 30),
				Location = new Point(116, 122),
				DialogResult = DialogResult.OK
			};

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(90, 30),
				Location = new Point(216, 122),
				DialogResult = DialogResult.Cancel
			};

			Controls.Add(titleLabel);
			Controls.Add(csvRadioButton);
			Controls.Add(textRadioButton);
			Controls.Add(okButton);
			Controls.Add(cancelButton);

			AcceptButton = okButton;
			CancelButton = cancelButton;
		}

		public string ExportFormat => csvRadioButton.Checked ? "CSV" : "Text";
	}
}
