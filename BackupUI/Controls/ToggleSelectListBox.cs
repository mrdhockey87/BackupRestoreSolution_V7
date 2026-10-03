using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.Controls
{
	/// <summary>
	/// Single-select ListBox where clicking the already-selected item clears the selection
	/// (replaces the WPF PreviewMouseLeftButtonDown trick on the restore-point list).
	/// Behaves like a normal ListBox in the Visual Studio designer.
	/// </summary>
	[ToolboxItem(true)]
	[DefaultEvent(nameof(SelectedIndexChanged))]
	[DesignerCategory("Code")]
	[Description("A ListBox that clears its selection when the selected item is clicked again.")]
	public partial class ToggleSelectListBox : ListBox
	{
		private const int WM_LBUTTONDOWN = 0x0201;

		public ToggleSelectListBox()
		{
		}

		/// <summary>
		/// When true (default) and SelectionMode is One, clicking the selected item again deselects it.
		/// </summary>
		[Category("Behavior")]
		[Description("Clicking the selected item again clears the selection (single-select mode only).")]
		[DefaultValue(true)]
		public bool ToggleOnReselect { get; set; } = true;

		private bool IsRuntime =>
			!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime;

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_LBUTTONDOWN &&
				IsRuntime &&
				ToggleOnReselect &&
				SelectionMode == SelectionMode.One &&
				IsHandleCreated)
			{
				int lp = unchecked((int)m.LParam.ToInt64());
				var pt = new Point((short)(lp & 0xFFFF), (short)((lp >> 16) & 0xFFFF));

				int clicked = IndexFromPoint(pt);
				int before = SelectedIndex;

				base.WndProc(ref m);

				if (clicked >= 0 && clicked == before && SelectedIndex == clicked)
				{
					SelectedIndex = -1;   // raises SelectedIndexChanged
				}

				return;
			}

			base.WndProc(ref m);
		}
	}
}
