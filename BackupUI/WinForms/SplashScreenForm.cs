using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class SplashScreenForm : Form
	{
		private readonly PictureBox logoPictureBox;
		private readonly Label statusLabel;
		private readonly Label versionLabel;

		public SplashScreenForm()
		{
			var baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;

			FormBorderStyle = FormBorderStyle.None;
			StartPosition = FormStartPosition.Manual;
			ShowInTaskbar = false;
			TopMost = true;
			ClientSize = new Size(600, 400);
			BackColor = WinFormsThemeManager.LightTurquoise;

			logoPictureBox = new PictureBox
			{
				Location = new Point(200, 20),
				Size = new Size(200, 200),
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = Color.Transparent
			};

			var titleLabel = new Label
			{
				Text = "Secure Server Backup",
				Font = new Font(baseFont.FontFamily, 18F, FontStyle.Bold),
				ForeColor = WinFormsThemeManager.PrimaryTurquoise,
				AutoSize = true,
				Location = new Point(160, 235)
			};

			statusLabel = new Label
			{
				Text = "Starting...",
				Font = baseFont,
				ForeColor = WinFormsThemeManager.SecondaryText,
				AutoSize = true,
				Location = new Point(250, 305)
			};

			versionLabel = new Label
			{
				Text = "Version: Loading...",
				Font = baseFont,
				ForeColor = WinFormsThemeManager.SecondaryText,
				AutoSize = true,
				Location = new Point(240, 275)
			};

			Controls.Add(logoPictureBox);
			Controls.Add(titleLabel);
			Controls.Add(statusLabel);
			Controls.Add(versionLabel);

			LoadSavedPosition();
			LoadLogo();
			LoadVersion();
		}

		public void UpdateStatus(string status)
		{
			if (InvokeRequired)
			{
				Invoke(() => statusLabel.Text = status);
				return;
			}

			statusLabel.Text = status;
		}

		public static async Task<SplashScreenForm> ShowAsync()
		{
			var splash = new SplashScreenForm();
			splash.Show();
			splash.Refresh();
			await Task.Delay(100);
			return splash;
		}

		public async Task CloseAsync()
		{
			if (IsDisposed)
			{
				return;
			}

			await Task.Delay(150);
			if (!IsDisposed)
			{
				Close();
			}
		}

		private void LoadLogo()
		{
			foreach (string logoPath in GetPreferredLogoPaths())
			{
				if (TryLoadLogo(logoPath))
				{
					return;
				}
			}
		}

		private string[] GetPreferredLogoPaths()
		{
			float scaleFactor = DeviceDpi / 96F;
			if (scaleFactor >= 2F)
			{
				return
				[
					"Assets/logo_large.png",
					"Assets/logo_medium.png",
					"Assets/logo_small.png"
				];
			}

			if (scaleFactor >= 1.5F)
			{
				return
				[
					"Assets/logo_medium.png",
					"Assets/logo_large.png",
					"Assets/logo_small.png"
				];
			}

			return
			[
				"Assets/logo_small.png",
				"Assets/logo_medium.png",
				"Assets/logo_large.png"
			];
		}

		private bool TryLoadLogo(string relativeResourcePath)
		{
			try
			{
				string normalizedRelativePath = relativeResourcePath.Replace('/', Path.DirectorySeparatorChar);
				string logoPath = Path.Combine(AppContext.BaseDirectory, normalizedRelativePath);
				if (!File.Exists(logoPath))
				{
					return false;
				}

				using Image loadedImage = Image.FromFile(logoPath);
				logoPictureBox.Image = new Bitmap(loadedImage);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void LoadVersion()
		{
			try
			{
				versionLabel.Text = VersionClass.GetVersion();
			}
			catch
			{
				versionLabel.Text = "Version: Unknown";
			}
		}

		private void LoadSavedPosition()
		{
			try
			{
				var settingsPath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"BackupRestoreApp",
					"window-position.json");

				if (!File.Exists(settingsPath))
				{
					CenterOnPrimaryScreen();
					return;
				}

				var json = File.ReadAllText(settingsPath);
				var position = JsonSerializer.Deserialize<SavedWindowPosition>(json);
				if (position == null)
				{
					CenterOnPrimaryScreen();
					return;
				}

				Left = (int)(position.Left + (position.Width / 2) - (Width / 2));
				Top = (int)(position.Top + (position.Height / 2) - (Height / 2));
			}
			catch
			{
				CenterOnPrimaryScreen();
			}
		}

		private void CenterOnPrimaryScreen()
		{
			var primaryScreen = Screen.PrimaryScreen;
			if (primaryScreen == null)
			{
				return;
			}

			Left = primaryScreen.WorkingArea.Left + (primaryScreen.WorkingArea.Width - Width) / 2;
			Top = primaryScreen.WorkingArea.Top + (primaryScreen.WorkingArea.Height - Height) / 2;
		}

		private sealed class SavedWindowPosition
		{
			public double Left { get; set; }
			public double Top { get; set; }
			public double Width { get; set; }
			public double Height { get; set; }
		}
	}
}
