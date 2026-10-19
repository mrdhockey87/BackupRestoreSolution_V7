using System;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	//I think this class can be deleted. mdail 10/19/2026
	internal sealed class Win32WindowAdapter : IWin32Window
	{
		public Win32WindowAdapter(IntPtr handle)
		{
			Handle = handle;
		}

		public IntPtr Handle { get; }
	}
}
