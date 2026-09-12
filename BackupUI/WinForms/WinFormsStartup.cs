using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using SecureServerBackup.Services;

namespace SecureServerBackup.WinForms
{
	internal static class WinFormsStartup
	{
		private const string MutexName = "SecureServerBackup_SingleInstance_Mutex";
		private const int SW_RESTORE = 9;

		private static Mutex? singleInstanceMutex;

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		private static extern bool IsIconic(IntPtr hWnd);
				
		public static void Run()
		{
			if (!TryAcquireSingleInstance())
			{
				return;
			}

			Application.ApplicationExit += (_, _) => ReleaseSingleInstance();

			try
			{
				SetNormalPriority();
				WinFormsThemeManager.Initialize();

				if (!EnsureAdministrator())
				{
					return;
				}

				var splash = new SplashScreenForm();
				WinFormsThemeManager.ApplyTheme(splash);

				var appContext = new ApplicationContext(splash);

				// 3. Attach the initialization step, passing BOTH the form and the context
				splash.Shown += async (s, e) => await InitializeApplicationAsync(splash, appContext);

				// 4. Start EXACTLY ONE message loop. This will run until the appContext tells it to stop.
				Application.Run(appContext);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Error during application startup: {ex.Message}",
					"Startup Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
			finally
			{
				ReleaseSingleInstance();
			}
		}
		private static async Task InitializeApplicationAsync(SplashScreenForm splash, ApplicationContext context)
		{
			try
			{
				splash.UpdateStatus("Checking components...");
				await Task.Delay(150); // 👈 Replaced Thread.Sleep/DoEvents with true asynchronous delays

				splash.UpdateStatus("Verifying SecureServerBackupEngine.dll...");
				CheckBackupEngineDll();
				await Task.Delay(150);

				splash.UpdateStatus("Initializing services...");
				NotificationService.Initialize();
				await Task.Delay(150);

				splash.UpdateStatus("Loading main window...");

				// 6. Instantiate the Main Window. We do NOT use a 'using' statement here
				// because Application.Run(mainForm) takes over responsibility for disposing it.
				var mainForm = new MainForm();
				WinFormsThemeManager.ApplyTheme(mainForm);
				await Task.Delay(150);

				splash.UpdateStatus("Ready!");
				await Task.Delay(100);

				// 7. Hide the splash screen first for a smooth visual transition
				splash.Hide();

				context.MainForm = mainForm;

				// 6. Show the main form to the user
				mainForm.Show();

				// 7. Close the splash screen. Because we swapped the context.MainForm to 
				// the mainForm above, closing the splash screen will NOT shut down the app.
				splash.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Initialization failed: {ex.Message}",
					"Startup Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
			finally
			{
				// 9. Close the splash screen instance, which safely tears down its resources
				if (!splash.IsDisposed)
				{
					splash.Close();
				}
			}
		}

		private static bool TryAcquireSingleInstance()
		{
			singleInstanceMutex = new Mutex(initiallyOwned: true, MutexName, out bool createdNew);
			if (createdNew)
			{
				return true;
			}

			ActivateExistingInstance();
			singleInstanceMutex.Dispose();
			singleInstanceMutex = null;
			return false;
		}

		private static void ReleaseSingleInstance()
		{
			if (singleInstanceMutex == null)
			{
				return;
			}

			try
			{
				singleInstanceMutex.ReleaseMutex();
			}
			catch (ApplicationException)
			{
			}
			finally
			{
				singleInstanceMutex.Dispose();
				singleInstanceMutex = null;
			}
		}

		private static void ActivateExistingInstance()
		{
			try
			{
				string? exeName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule?.FileName);
				if (string.IsNullOrEmpty(exeName))
				{
					return;
				}

				foreach (Process proc in Process.GetProcessesByName(exeName))
				{
					if (proc.Id == Environment.ProcessId)
					{
						continue;
					}

					IntPtr hWnd = proc.MainWindowHandle;
					if (hWnd == IntPtr.Zero)
					{
						continue;
					}

					if (IsIconic(hWnd))
					{
						ShowWindow(hWnd, SW_RESTORE);
					}

					SetForegroundWindow(hWnd);
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[WinFormsStartup.ActivateExistingInstance] {ex.Message}");
			}
		}

		private static void SetNormalPriority()
		{
			try
			{
				Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
				Debug.WriteLine("[WinFormsStartup] Process priority set to Normal");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[WinFormsStartup] Warning: Failed to set process priority: {ex.Message}");
			}
		}

		private static bool EnsureAdministrator()
		{
			if (IsRunningAsAdministrator())
			{
				return true;
			}

			try
			{
				var processInfo = new ProcessStartInfo
				{
					UseShellExecute = true,
					WorkingDirectory = Environment.CurrentDirectory,
					FileName = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty,
					Verb = "runas"
				};

				ReleaseSingleInstance();
				Process.Start(processInfo);
			}
			catch (Exception)
			{
				MessageBox.Show(
					"This application requires administrator privileges to access backup services, VSS snapshots, and Hyper-V.\n\nPlease run as Administrator.",
					"Administrator Rights Required",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}

			return false;
		}

		private static void CheckBackupEngineDll()
		{
			var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SecureServerBackupEngine.dll");
			if (!File.Exists(dllPath))
			{
				throw new FileNotFoundException(
					$"SecureServerBackupEngine.dll was not found. Expected location: {dllPath}",
					dllPath);
			}
		}

		private static bool IsRunningAsAdministrator()
		{
			try
			{
				var identity = WindowsIdentity.GetCurrent();
				var principal = new WindowsPrincipal(identity);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
			catch
			{
				return false;
			}
		}
	}
}
