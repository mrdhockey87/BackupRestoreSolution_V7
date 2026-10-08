using SecureServerBackup.WinForms;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.Controls
{
	/// <summary>
	/// TabControl whose strip background (behind and between the tabs) can be themed.
	/// Tabs are drawn with a small gap, a border, and a distinct selected-tab color.
	/// Assumes tabs on the top (or bottom) edge.
	/// </summary>
	public partial class CustomTabControl : TabControl
	{
		private const int TabGap = 2;

		private Color _tabStripBackColor = Color.Empty;
		private Color _tabBackColor = Color.Empty;
		private Color _selectedTabBackColor = Color.Empty;
		private Color _tabBorderColor = Color.Empty;
		private Color _tabTextColor = Color.Empty;

		public CustomTabControl()
		{
			SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw,
				true);
		}

		#region Theme properties

		/// <summary>Color of the space behind the tab control and between/after the tabs.</summary>
		[Category("Appearance")]
		[Description("Color of the space behind the tabs and between them. Defaults to BackColor.")]
		public Color TabStripBackColor
		{
			get => _tabStripBackColor; //.IsEmpty ? BackColor : WinFormsThemeManager.LightTurquoise; // _tabStripBackColor;
			set { _tabStripBackColor = value; Invalidate(); }
		}
		public Color TabTextColor
		{
			get => _tabTextColor; //.IsEmpty ? SystemColors.GrayText : WinFormsThemeManager.PrimaryText; // _tabStripBackColor;
			set { _tabTextColor = value; Invalidate(); }
		}
		private bool ShouldSerializeTabStripBackColor() => !_tabStripBackColor.IsEmpty;
		private void ResetTabStripBackColor() => TabStripBackColor = Color.Empty;

		/// <summary>Fill color of unselected tabs.</summary>
		[Category("Appearance")]
		[Description("Fill color of unselected tabs. Defaults to SystemColors.ControlLight.")]
		public Color TabBackColor
		{
			get => _tabBackColor;//.IsEmpty ? SystemColors.ControlLight : WinFormsThemeManager.MediumTurquoise;
			set { _tabBackColor = value; Invalidate(); }
		}
		private bool ShouldSerializeTabBackColor() => !_tabBackColor.IsEmpty;
		private void ResetTabBackColor() => TabBackColor = Color.Empty;

		/// <summary>Fill color of the selected tab. Defaults to the selected page's BackColor.</summary>
		[Category("Appearance")]
		[Description("Fill color of the selected tab. Defaults to the selected page's BackColor.")]
		public Color SelectedTabBackColor
		{
			get => _selectedTabBackColor; //WinFormsThemeManager.MediumTurquoise //_selectedTabBackColor.IsEmpty
				//? (SelectedTab?.BackColor ?? WinFormsThemeManager.MediumTurquoise) //SystemColors.Window)
				//: _selectedTabBackColor;
			set { _selectedTabBackColor = value; Invalidate(); }
		}
		private bool ShouldSerializeSelectedTabBackColor() => !_selectedTabBackColor.IsEmpty;
		private void ResetSelectedTabBackColor() => SelectedTabBackColor = Color.Empty;

		/// <summary>Color of the tab outlines and the border around the page area.</summary>
		[Category("Appearance")]
		[Description("Color of the tab outlines and the border around the page area.")]
		public Color TabBorderColor
		{
			get => _tabBorderColor; //	.IsEmpty ? SystemColors.ControlDark : WinFormsThemeManager.DarkTurquoise;
			set { _tabBorderColor = value; Invalidate(); }
		}
		private bool ShouldSerializeTabBorderColor() => !_tabBorderColor.IsEmpty;
		private void ResetTabBorderColor() => TabBorderColor = Color.Empty;

		#endregion

		#region Repaint triggers

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			Invalidate();
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			Invalidate();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			Invalidate();
		}

		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			// 1. Everything behind the tabs (strip, gaps, area around the page)
			using (var stripBrush = new SolidBrush(TabStripBackColor))
			{
				g.FillRectangle(stripBrush, ClientRectangle);
			}

			if (TabCount == 0)
				return;

			bool bottom = Alignment == TabAlignment.Bottom;

			// 2. Border around the page area
			Rectangle pageBorder = DisplayRectangle;
			pageBorder.Inflate(1, 1);
			using (var borderPen = new Pen(TabBorderColor))
			{
				g.DrawRectangle(borderPen, pageBorder.X, pageBorder.Y, pageBorder.Width - 1, pageBorder.Height - 1);
			}

			// 3. Tabs (unselected first, selected last so it sits on top of the border)
			for (int i = 0; i < TabCount; i++)
			{
				if (i != SelectedIndex)
					DrawTab(g, i, false, bottom);
			}

			if (SelectedIndex >= 0 && SelectedIndex < TabCount)
				DrawTab(g, SelectedIndex, true, bottom);
		}

		private void DrawTab(Graphics g, int index, bool selected, bool bottom)
		{
			TabPage page = TabPages[index];
			Rectangle r = GetTabRect(index);

			// Leave a gap so the strip color shows between tabs
			r.Inflate(-TabGap / 2, 0);

			// Selected tab overlaps the page border by one pixel so it "joins" the page
			if (selected)
			{
				if (bottom)
				{
					r.Y -= 1;
					r.Height += 1;
				}
				else
				{
					r.Height += 1;
				}
			}

			Color fill = selected ? SelectedTabBackColor : TabBackColor;
			using (var fillBrush = new SolidBrush(fill))
			using (var borderPen = new Pen(TabBorderColor))
			{
				g.FillRectangle(fillBrush, r);

				if (selected)
				{
					// Outline without the edge that touches the page
					Point[] outline = bottom
						? new[]
						{
							new Point(r.Left, r.Top),
							new Point(r.Left, r.Bottom - 1),
							new Point(r.Right - 1, r.Bottom - 1),
							new Point(r.Right - 1, r.Top)
						}
						: new[]
						{
							new Point(r.Left, r.Bottom - 1),
							new Point(r.Left, r.Top),
							new Point(r.Right - 1, r.Top),
							new Point(r.Right - 1, r.Bottom - 1)
						};
					g.DrawLines(borderPen, outline);
				}
				else
				{
					g.DrawRectangle(borderPen, r.X, r.Y, r.Width - 1, r.Height - 1);
				}
			}

			// Optional image
			Rectangle textRect = r;
			if (ImageList != null && page.ImageIndex >= 0 && page.ImageIndex < ImageList.Images.Count)
			{
				Image img = ImageList.Images[page.ImageIndex];
				int y = r.Y + (r.Height - img.Height) / 2;
				g.DrawImage(img, r.X + 6, y);
				textRect.X += img.Width + 8;
				textRect.Width -= img.Width + 8;
			}

			// Text
			Color textColor = page.Enabled ? ForeColor : TabTextColor;// SystemColors.GrayText;
			TextRenderer.DrawText(
				g,
				page.Text,
				Font,
				textRect,
				textColor,
				TextFormatFlags.HorizontalCenter |
				TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine |
				TextFormatFlags.EndEllipsis |
				TextFormatFlags.NoPrefix);
		}
	}
}