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
		internal static Color ButtonHover => ColorTranslator.FromHtml("#20B2AA");
		internal static Color ButtonPressed => ColorTranslator.FromHtml("#5F9EA0");
		internal static Color PrimaryText => Color.Black;
		internal static Color SecondaryText => ColorTranslator.FromHtml("#333333");
		internal static Color ErrorText => ColorTranslator.FromHtml("#8B0000");
		internal static Color WindowBackground => ColorTranslator.FromHtml("#F5FFFF");
		internal static Color PanelBackground => ColorTranslator.FromHtml("#E0F7F7");
		internal static Color AlternateRowBackground => ColorTranslator.FromHtml("#5F9EA0");
		internal static Color HeaderBackground => ColorTranslator.FromHtml("#B0E0E6");
		internal static Color StatusBarBackground => ColorTranslator.FromHtml("#AFEEEE");
		internal static Color BorderColor => ColorTranslator.FromHtml("#5F9EA0");
		internal static Color LightBorderColor => ColorTranslator.FromHtml("#B0E0E6");
		internal static Color SelectionBackground => ColorTranslator.FromHtml("#008B8B");
		internal static Color SelectionForeground => Color.White;
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

			if (control is Form form)
			{
				form.BackColor = WindowBackground;
				form.ForeColor = PrimaryText;
			}
			else if (control is TabPage)
			{
				control.BackColor = WindowBackground;
				control.ForeColor = PrimaryText;
			}
			else if (control is Panel or FlowLayoutPanel or TableLayoutPanel or SplitContainer or GroupBox)
			{
				if (ShouldApplyBackground(control.BackColor))
				{
					control.BackColor = PanelBackground;
				}

				control.ForeColor = PrimaryText;
			}
			else if (control is Label)
			{
				if (control.BackColor == default || control.BackColor == Color.Transparent)
				{
					control.BackColor = Color.Transparent;
				}

				control.ForeColor = PrimaryText;
			}
			else if (control is Button button)
			{
				button.UseVisualStyleBackColor = false;
				if (ShouldApplyBackground(button.BackColor))
				{
					button.BackColor = ButtonBackground;
				}

				button.ForeColor = PrimaryText;
				button.FlatStyle = FlatStyle.Flat;
				button.FlatAppearance.BorderColor = BorderColor;
				button.FlatAppearance.MouseOverBackColor = ButtonHover;
				button.FlatAppearance.MouseDownBackColor = ButtonPressed;
			}
			else if (control is CheckBox or RadioButton)
			{
				if (ShouldApplyBackground(control.BackColor))
				{
					control.BackColor = PanelBackground;
				}

				control.ForeColor = PrimaryText;
			}
			else if (control is TextBoxBase or ComboBox or NumericUpDown or DateTimePicker or ListBox or CheckedListBox)
			{
				if (ShouldApplyBackground(control.BackColor))
				{
					control.BackColor = Color.White;
				}

				control.ForeColor = PrimaryText;
			}
			else if (control is ListView listView)
			{
				if (ShouldApplyBackground(listView.BackColor))
				{
					listView.BackColor = VeryLightTurquoise;
				}

				listView.ForeColor = PrimaryText;
			}
			else if (control is TreeView treeView)
			{
				if (ShouldApplyBackground(treeView.BackColor))
				{
					treeView.BackColor = VeryLightTurquoise;
				}

				treeView.ForeColor = PrimaryText;
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
				if (ShouldApplyBackground(tabControl.BackColor))
				{
					tabControl.BackColor = WindowBackground;
				}

				tabControl.ForeColor = PrimaryText;
			}
			else if (control is DataGridView dataGridView)
			{
				ApplyThemeToDataGridView(dataGridView);
			}

			if (control is MenuStrip menuStrip)
			{
				menuStrip.BackColor = HeaderBackground;
				menuStrip.ForeColor = PrimaryText;
			}
			else if (control is StatusStrip statusStrip)
			{
				statusStrip.BackColor = StatusBarBackground;
				statusStrip.ForeColor = PrimaryText;
			}
			else if (control is ToolStrip toolStrip)
			{
				toolStrip.BackColor = HeaderBackground;
				toolStrip.ForeColor = PrimaryText;
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

		private static bool ShouldApplyBackground(Color backColor)
		{
			return backColor.IsEmpty ||
				backColor == Color.Transparent ||
				backColor.ToArgb() == SystemColors.Control.ToArgb() ||
				backColor.ToArgb() == SystemColors.ButtonFace.ToArgb();
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
