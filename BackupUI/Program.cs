using System;
using System.Windows.Forms;
using SecureServerBackup.WinForms;

namespace SecureServerBackup
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			ApplicationConfiguration.Initialize();
			WinFormsStartup.Run();//.GetAwaiter().GetResult();
		}
	}
}
