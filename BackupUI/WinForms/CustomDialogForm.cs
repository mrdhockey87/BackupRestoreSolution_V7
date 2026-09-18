using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class CustomDialogForm : Form
	{
		public CustomDialogResult Result { get; private set; } = CustomDialogResult.None;

		public CustomDialogForm()
		{
			InitializeComponent();
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
				DialogIcon.Information => ("ℹ️", WinFormsThemeManager.InfoText),
				DialogIcon.Warning => ("⚠️", WinFormsThemeManager.WarningText),
				DialogIcon.Error => ("❌", WinFormsThemeManager.ErrorText),
				DialogIcon.Question => ("❓", WinFormsThemeManager.PrimaryTurquoise),
				DialogIcon.Success => ("✅", WinFormsThemeManager.SuccessText),
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
