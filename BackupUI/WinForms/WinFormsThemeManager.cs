using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal static class WinFormsThemeManager
	{
		internal static Color PrimaryTurquoise => ColorTranslator.FromHtml("#20B2AA");
		internal static Color DarkTurquoise => ColorTranslator.FromHtml("#00CED1");
		internal static Color MediumTurquoise => ColorTranslator.FromHtml("#48D1CC");
		internal static Color LightTurquoise => ColorTranslator.FromHtml("#AFEEEE");
		internal static Color VeryLightTurquoise => ColorTranslator.FromHtml("#E0F7F7");
		internal static Color ButtonBackground => ColorTranslator.FromHtml("#008B8B");
		internal static Color ButtonForeground => Color.Black;
		internal static Color ButtonHover => ColorTranslator.FromHtml("#20B2AA");
		internal static Color ButtonPressed => ColorTranslator.FromHtml("#5F9EA0");
		internal static Color PrimaryText => Color.Black;
		internal static Color SecondaryText => ColorTranslator.FromHtml("#333333");
		internal static Color ErrorText => ColorTranslator.FromHtml("#8B0000");
		internal static Color WarningText => ColorTranslator.FromHtml("#FF8C00");
		internal static Color SuccessText => ColorTranslator.FromHtml("#006400");
		internal static Color InfoText => ColorTranslator.FromHtml("#000080");
		internal static Color WindowBackground => ColorTranslator.FromHtml("#F5FFFF");
		internal static Color PanelBackground => ColorTranslator.FromHtml("#E0F7F7");
		internal static Color AlternateRowBackground => ColorTranslator.FromHtml("#5F9EA0");
		internal static Color HeaderBackground => ColorTranslator.FromHtml("#B0E0E6");
		internal static Color StatusBarBackground => ColorTranslator.FromHtml("#AFEEEE");
		internal static Color BorderColor => ColorTranslator.FromHtml("#5F9EA0");
		internal static Color LightBorderColor => ColorTranslator.FromHtml("#B0E0E6");
		internal static Color SelectionBackground => ColorTranslator.FromHtml("#008B8B");
		internal static Color SelectionForeground => Color.White;
		internal static Color SuccessBackground => ColorTranslator.FromHtml("#E6F4EA");
		internal static Color WarningBackground => ColorTranslator.FromHtml("#FFF8DC");
		internal static Color ErrorBackground => ColorTranslator.FromHtml("#FFE4E1");
		internal static Color InfoBackground => ColorTranslator.FromHtml("#E0F7F7");
		internal static Color HelpHeaderBackground => ColorTranslator.FromHtml("#2C3E50");
		internal static Color CardBorderColor => ColorTranslator.FromHtml("#56D6D6");
		internal static Color DangerColor => ColorTranslator.FromHtml("#8B0000");

		private static readonly HashSet<IntPtr> ThemedForms = new();
		private static readonly HashSet<Control> ThemedControls = new();
		private static bool isInitialized;

		internal static void Initialize()
		{
			if (isInitialized)
			{
				return;
			}

			ToolStripManager.Renderer = new ToolStripProfessionalRenderer(new TurquoiseProfessionalColorTable());
			Application.Idle += OnApplicationIdle;
			isInitialized = true;
		}

		internal static void ApplyTheme(Form form)
		{
			ArgumentNullException.ThrowIfNull(form);

			ApplyThemeToControl(form);
			RegisterForm(form);
		}

		private static void OnApplicationIdle(object? sender, EventArgs e)
		{
			foreach (Form form in Application.OpenForms)
			{
				if (form.IsDisposed)
				{
					continue;
				}

				ApplyTheme(form);
			}
		}

		private static void RegisterForm(Form form)
		{
			if (ThemedForms.Add(form.Handle))
			{
				form.HandleDestroyed += (_, _) => ThemedForms.Remove(form.Handle);
				form.ControlAdded += OnControlAdded;
			}
		}

		private static void OnControlAdded(object? sender, ControlEventArgs e)
		{
			if (e.Control is null)
			{
				return;
			}

			ApplyThemeToControl(e.Control);
		}

		private static void ApplyThemeToControl(Control control)
		{
			ArgumentNullException.ThrowIfNull(control);

			if (!ThemedControls.Add(control))
			{
				return;
			}

			control.Disposed += (_, _) => ThemedControls.Remove(control);
			control.ControlAdded += OnControlAdded;

			if (control is Form form)
			{
				form.BackColor = WindowBackground;
				if (ShouldApplyForeground(form.ForeColor))
				{
					form.ForeColor = PrimaryText;
				}
			}
			else if (control is TabPage)
			{
				control.BackColor = WindowBackground;
				if (ShouldApplyForeground(control.ForeColor))
				{
					control.ForeColor = PrimaryText;
				}
			}
			else if (control is GroupBox)
			{
				control.BackColor = VeryLightTurquoise;

				if (ShouldApplyForeground(control.ForeColor))
				{
					control.ForeColor = PrimaryText;
				}
			}
			else if (control is Panel or FlowLayoutPanel or TableLayoutPanel or SplitContainer)
			{
				if (ShouldApplyContainerBackground(control.BackColor))
				{
					control.BackColor = PanelBackground;
				}

				if (ShouldApplyForeground(control.ForeColor))
				{
					control.ForeColor = PrimaryText;
				}
			}
			else if (control is Label)
			{
				if (control.BackColor == default || control.BackColor == Color.Transparent)
				{
					control.BackColor = Color.Transparent;
				}

				if (ShouldApplyForeground(control.ForeColor))
				{
					control.ForeColor = PrimaryText;
				}
			}
			else if (control is Button button)
			{
				Color buttonBackColor = ShouldApplyButtonBackground(button.BackColor)
					? ButtonBackground
					: button.BackColor;

				ApplyButtonTheme(button, buttonBackColor);
			}
			else if (control is CheckBox or RadioButton)
			{
				if (ShouldApplyContainerBackground(control.BackColor))
				{
					control.BackColor = PanelBackground;
				}

				if (ShouldApplyForeground(control.ForeColor))
				{
					control.ForeColor = PrimaryText;
				}
			}
			else if (control is TextBoxBase or ComboBox or NumericUpDown or DateTimePicker or ListBox or CheckedListBox)
			{
				control.BackColor = VeryLightTurquoise;

				if (ShouldApplyForeground(control.ForeColor))
				{
					control.ForeColor = PrimaryText;
				}
			}
			else if (control is ListView listView)
			{
				listView.BackColor = VeryLightTurquoise;

				if (ShouldApplyForeground(listView.ForeColor))
				{
					listView.ForeColor = PrimaryText;
				}
			}
			else if (control is TreeView treeView)
			{
				treeView.BackColor = VeryLightTurquoise;

				if (ShouldApplyForeground(treeView.ForeColor))
				{
					treeView.ForeColor = PrimaryText;
				}
				try
				{
					treeView.LineColor = BorderColor;
				}
				catch (InvalidOperationException)
				{
				}
			}
			else if (control is TabControl tabControl)
			{
				if (ShouldApplyContainerBackground(tabControl.BackColor))
				{
					tabControl.BackColor = WindowBackground;
				}

				if (ShouldApplyForeground(tabControl.ForeColor))
				{
					tabControl.ForeColor = PrimaryText;
				}
			}
			else if (control is DataGridView dataGridView)
			{
				ApplyThemeToDataGridView(dataGridView);
			}
			else if (control is ProgressBar progressBar)
			{
				progressBar.BackColor = VeryLightTurquoise;
				progressBar.ForeColor = ButtonBackground;
			}

			if (control is MenuStrip menuStrip)
			{
				menuStrip.BackColor = HeaderBackground;
				if (ShouldApplyForeground(menuStrip.ForeColor))
				{
					menuStrip.ForeColor = PrimaryText;
				}
			}
			else if (control is StatusStrip statusStrip)
			{
				statusStrip.BackColor = StatusBarBackground;
				if (ShouldApplyForeground(statusStrip.ForeColor))
				{
					statusStrip.ForeColor = PrimaryText;
				}
			}
			else if (control is ToolStrip toolStrip)
			{
				toolStrip.BackColor = HeaderBackground;
				if (ShouldApplyForeground(toolStrip.ForeColor))
				{
					toolStrip.ForeColor = PrimaryText;
				}
			}

			foreach (Control child in control.Controls)
			{
				ApplyThemeToControl(child);
			}
		}

		private static void ApplyThemeToDataGridView(DataGridView dataGridView)
		{
			dataGridView.BackgroundColor = VeryLightTurquoise;
			dataGridView.GridColor = DarkTurquoise;
			dataGridView.BorderStyle = BorderStyle.FixedSingle;
			dataGridView.EnableHeadersVisualStyles = false;
			dataGridView.ColumnHeadersDefaultCellStyle.BackColor = HeaderBackground;
			dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = PrimaryText;
			dataGridView.RowHeadersDefaultCellStyle.BackColor = HeaderBackground;
			dataGridView.RowHeadersDefaultCellStyle.ForeColor = PrimaryText;
			dataGridView.DefaultCellStyle.BackColor = VeryLightTurquoise;
			dataGridView.DefaultCellStyle.ForeColor = PrimaryText;
			dataGridView.DefaultCellStyle.SelectionBackColor = SelectionBackground;
			dataGridView.DefaultCellStyle.SelectionForeColor = SelectionForeground;
			dataGridView.AlternatingRowsDefaultCellStyle.BackColor = AlternateRowBackground;
			dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = PrimaryText;
		}

		internal static void ApplyButtonTheme(Button button, Color backColor)
		{
			ArgumentNullException.ThrowIfNull(button);

			button.UseVisualStyleBackColor = false;
			button.BackColor = backColor;
			button.ForeColor = GetButtonForeground(backColor);
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderColor = ControlPaint.Dark(backColor);
			button.FlatAppearance.MouseOverBackColor = GetHoverColor(backColor);
			button.FlatAppearance.MouseDownBackColor = GetPressedColor(backColor);
		}

		private static Color GetButtonForeground(Color backColor)
		{
			return backColor == DangerColor || backColor == WarningText
				? VeryLightTurquoise
				: ButtonForeground;
		}

		private static Color GetHoverColor(Color backColor)
		{
			return backColor == ButtonBackground ? ButtonHover : ControlPaint.Light(backColor);
		}

		private static Color GetPressedColor(Color backColor)
		{
			return backColor == ButtonBackground ? ButtonPressed : ControlPaint.Dark(backColor);
		}

		private static bool ShouldApplyButtonBackground(Color backColor)
		{
			return backColor.IsEmpty ||
				backColor == Color.Transparent ||
				backColor.ToArgb() == SystemColors.Control.ToArgb() ||
				backColor.ToArgb() == SystemColors.ButtonFace.ToArgb() ||
				backColor.ToArgb() == Color.White.ToArgb();
		}

		private static bool ShouldApplyContainerBackground(Color backColor)
		{
			return backColor.IsEmpty ||
				backColor == Color.Transparent ||
				backColor.ToArgb() == SystemColors.Control.ToArgb() ||
				backColor.ToArgb() == SystemColors.ButtonFace.ToArgb() ||
				backColor.ToArgb() == Color.White.ToArgb();
		}

		private static bool ShouldApplyForeground(Color foreColor)
		{
			return foreColor.IsEmpty ||
				foreColor.ToArgb() == SystemColors.ControlText.ToArgb() ||
				foreColor.ToArgb() == SystemColors.WindowText.ToArgb();
		}

		private sealed class TurquoiseProfessionalColorTable : ProfessionalColorTable
		{
			public override Color MenuStripGradientBegin => HeaderBackground;
			public override Color MenuStripGradientEnd => HeaderBackground;
			public override Color ToolStripDropDownBackground => VeryLightTurquoise;
			public override Color ImageMarginGradientBegin => VeryLightTurquoise;
			public override Color ImageMarginGradientMiddle => VeryLightTurquoise;
			public override Color ImageMarginGradientEnd => VeryLightTurquoise;
			public override Color MenuItemSelected => LightTurquoise;
			public override Color MenuItemBorder => BorderColor;
			public override Color MenuItemPressedGradientBegin => VeryLightTurquoise;
			public override Color MenuItemPressedGradientMiddle => VeryLightTurquoise;
			public override Color MenuItemPressedGradientEnd => VeryLightTurquoise;
			public override Color ButtonSelectedBorder => BorderColor;
			public override Color ButtonSelectedHighlight => ButtonHover;
			public override Color ButtonPressedHighlight => ButtonPressed;
			public override Color ButtonPressedBorder => BorderColor;
			public override Color ToolStripBorder => BorderColor;
			public override Color StatusStripGradientBegin => StatusBarBackground;
			public override Color StatusStripGradientEnd => StatusBarBackground;
		}
	}
}
