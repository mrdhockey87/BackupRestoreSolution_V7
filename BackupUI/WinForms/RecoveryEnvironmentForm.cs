using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RecoveryEnvironmentForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private string isoPath = string.Empty;

		public RecoveryEnvironmentForm()
		{
			InitializeComponent();
		}

		private void RecoveryEnvironmentForm_Load(object? sender, EventArgs e)
		{
			if (IsInDesignMode)
			{
				return;
			}

			CheckIsoFile();
		}

		private void CheckIsoFile()
		{
			try
			{
				string appDir = AppDomain.CurrentDomain.BaseDirectory;
				string linuxRestoreDir = Path.Combine(appDir, "LinuxRestore");
				isoPath = Path.Combine(linuxRestoreDir, "BackupRestore_Recovery.iso");
				isoPathTextBox.Text = isoPath;

				if (File.Exists(isoPath))
				{
					var fileInfo = new FileInfo(isoPath);
					isoStatusLabel.Text = $"ISO Found ({FormatBytes(fileInfo.Length)})";
					isoStatusLabel.ForeColor = WinFormsThemeManager.SuccessText;
					openIsoLocationButton.Enabled = true;
					isoNoteLabel.Text = string.Empty;
				}
				else
				{
					isoStatusLabel.Text = "ISO file not found";
					isoStatusLabel.ForeColor = WinFormsThemeManager.ErrorText;
					openIsoLocationButton.Enabled = false;
					isoNoteLabel.Text = "The ISO should be deployed in the LinuxRestore folder. If it is missing, build it with the LinuxRestore BUILD-AND-CREATE-ISO.ps1 script.";
				}
			}
			catch (Exception ex)
			{
				isoStatusLabel.Text = $"Error: {ex.Message}";
				isoStatusLabel.ForeColor = WinFormsThemeManager.ErrorText;
				openIsoLocationButton.Enabled = false;
			}
		}

		private static string FormatBytes(long bytes)
		{
			string[] sizes = ["B", "KB", "MB", "GB"];
			double len = bytes;
			int order = 0;
			while (len >= 1024 && order < sizes.Length - 1)
			{
				order++;
				len /= 1024;
			}

			return $"{len:0.##} {sizes[order]}";
		}

		private void OpenRufusWebsite()
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://rufus.ie",
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Failed to open website: {ex.Message}\n\nPlease manually visit: https://rufus.ie", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OpenIsoLocation()
		{
			try
			{
				if (!File.Exists(isoPath))
				{
					MessageBox.Show(this, "ISO file not found. Please ensure the file exists at the specified location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				Process.Start("explorer.exe", $"/select,\"{isoPath}\"");
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Failed to open folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void PrintInstructions()
		{
			try
			{
				string tempFile = Path.Combine(Path.GetTempPath(), "RecoveryUSB_Instructions.html");
				File.WriteAllText(tempFile, GenerateInstructionsHtml());
				Process.Start(new ProcessStartInfo
				{
					FileName = tempFile,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Failed to create printable instructions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OpenIsoLocationButton_Click(object? sender, EventArgs e)
		{
			OpenIsoLocation();
		}

		private void OpenRufusButton_Click(object? sender, EventArgs e)
		{
			OpenRufusWebsite();
		}

		private void PrintInstructionsButton_Click(object? sender, EventArgs e)
		{
			PrintInstructions();
		}

		private string GenerateInstructionsHtml()
		{
			return $@"<!DOCTYPE html>
<html>
<head><title>Recovery USB Creation Instructions</title></head>
<body style='font-family: Arial, sans-serif; margin: 40px; line-height: 1.6;'>
<h1>Recovery USB Creation Instructions</h1>
<p>ISO path:</p>
<p><code>{isoPath}</code></p>
<ol>
<li>Download Rufus from https://rufus.ie</li>
<li>Select the recovery ISO file shown above.</li>
<li>Create a bootable USB drive.</li>
<li>Boot the target system from that USB drive.</li>
</ol>
<p>Restore commands on the recovery media:</p>
<ul>
<li>./restore_gui</li>
<li>./restore_tui</li>
<li>./restore_cli &lt;backup&gt; &lt;destination&gt;</li>
</ul>
</body>
</html>";
		}
	}
}
