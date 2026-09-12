using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class CustomDialogForm : Form
	{
		private readonly Label titleLabel;
		private readonly Label iconLabel;
		private readonly TextBox messageTextBox;
		private readonly FlowLayoutPanel buttonPanel;

		public CustomDialogResult Result { get; private set; } = CustomDialogResult.None;

		public CustomDialogForm()
		{
			var baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;

			Text = "Message";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			StartPosition = FormStartPosition.CenterParent;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			TopMost = true;
			ClientSize = new Size(520, 240);
			BackColor = Color.White;

			titleLabel = new Label
			{
				AutoSize = true,
				Font = new Font(baseFont, FontStyle.Bold),
				ForeColor = Color.FromArgb(32, 178, 170),
				Location = new Point(70, 20)
			};

			iconLabel = new Label
			{
				AutoSize = false,
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font(baseFont.FontFamily, 20F, FontStyle.Regular),
				Location = new Point(20, 16),
				Size = new Size(36, 36)
			};

			messageTextBox = new TextBox
			{
				BorderStyle = BorderStyle.None,
				Multiline = true,
				ReadOnly = true,
				BackColor = Color.White,
				Location = new Point(20, 68),
				Size = new Size(480, 106),
				TabStop = false
			};

			buttonPanel = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.RightToLeft,
				Dock = DockStyle.Bottom,
				Height = 54,
				Padding = new Padding(12, 10, 12, 10)
			};

			Controls.Add(titleLabel);
			Controls.Add(iconLabel);
			Controls.Add(messageTextBox);
			Controls.Add(buttonPanel);
		}

		public void Configure(string message, string title, DialogButtons buttons, DialogIcon icon, string? primaryButtonText = null, string? secondaryButtonText = null, string? tertiaryButtonText = null)
		{
			Text = title;
			titleLabel.Text = title;
			messageTextBox.Text = message;
			ConfigureIcon(icon);
			ConfigureButtons(buttons, primaryButtonText, secondaryButtonText, tertiaryButtonText);
		}

		private void ConfigureIcon(DialogIcon icon)
		{
			iconLabel.Visible = icon != DialogIcon.None;
			(iconLabel.Text, iconLabel.ForeColor) = icon switch
			{
				DialogIcon.Information => ("ℹ️", Color.Navy),
				DialogIcon.Warning => ("⚠️", Color.DarkOrange),
				DialogIcon.Error => ("❌", Color.DarkRed),
				DialogIcon.Question => ("❓", Color.LightSeaGreen),
				DialogIcon.Success => ("✅", Color.DarkGreen),
				_ => (string.Empty, Color.Transparent)
			};
		}

		private void ConfigureButtons(DialogButtons buttons, string? primaryButtonText, string? secondaryButtonText, string? tertiaryButtonText)
		{
			buttonPanel.Controls.Clear();

			switch (buttons)
			{
				case DialogButtons.OK:
					buttonPanel.Controls.Add(CreateButton(primaryButtonText ?? "OK", CustomDialogResult.OK));
					break;
				case DialogButtons.OKCancel:
					buttonPanel.Controls.Add(CreateButton(secondaryButtonText ?? "Cancel", CustomDialogResult.Cancel));
					buttonPanel.Controls.Add(CreateButton(primaryButtonText ?? "OK", CustomDialogResult.OK));
					break;
				case DialogButtons.YesNo:
					buttonPanel.Controls.Add(CreateButton(secondaryButtonText ?? "No", CustomDialogResult.No));
					buttonPanel.Controls.Add(CreateButton(primaryButtonText ?? "Yes", CustomDialogResult.Yes));
					break;
				case DialogButtons.YesNoCancel:
					buttonPanel.Controls.Add(CreateButton(tertiaryButtonText ?? "No", CustomDialogResult.No));
					buttonPanel.Controls.Add(CreateButton(secondaryButtonText ?? "Cancel", CustomDialogResult.Cancel));
					buttonPanel.Controls.Add(CreateButton(primaryButtonText ?? "Yes", CustomDialogResult.Yes));
					break;
			}
		}

		private Button CreateButton(string text, CustomDialogResult result)
		{
			var button = new Button
			{
				Text = text,
				AutoSize = true,
				MinimumSize = new Size(90, 30),
				Margin = new Padding(8, 0, 0, 0)
			};
			button.Click += (_, _) => CloseWithResult(result);
			return button;
		}

		private void CloseWithResult(CustomDialogResult result)
		{
			Result = result;
			DialogResult = result is CustomDialogResult.OK or CustomDialogResult.Yes
				? DialogResult.OK
				: DialogResult.Cancel;
			Close();
		}
	}
}
