using System;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class Win32WindowAdapter : IWin32Window
	{
		public Win32WindowAdapter(IntPtr handle)
		{
			Handle = handle;
		}

		public IntPtr Handle { get; }
	}
}
