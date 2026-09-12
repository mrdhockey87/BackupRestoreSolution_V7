using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class RecoveryEnvironmentForm : Form
	{
		private readonly TextBox isoPathTextBox;
		private readonly Label isoStatusLabel;
		private readonly Label isoNoteLabel;
		private readonly Button openIsoLocationButton;
		private string isoPath = string.Empty;

		public RecoveryEnvironmentForm()
		{
			Font baseFont = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
			Text = "Recovery Environment Creator";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(760, 560);
			ClientSize = new Size(760, 560);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Recovery USB Creation",
				Font = new Font(baseFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(16, 16)
			};

			var isoPathHeaderLabel = new Label
			{
				Text = "Recovery ISO Path:",
				AutoSize = true,
				Location = new Point(16, 54)
			};

			isoPathTextBox = new TextBox
			{
				Location = new Point(16, 76),
				Size = new Size(710, 24),
				ReadOnly = true
			};

			isoStatusLabel = new Label
			{
				AutoSize = true,
				Location = new Point(16, 110)
			};

			isoNoteLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 136),
				Size = new Size(710, 54)
			};

			openIsoLocationButton = new Button
			{
				Text = "Open ISO Location",
				Location = new Point(16, 198),
				Size = new Size(140, 32)
			};
			openIsoLocationButton.Click += (_, _) => OpenIsoLocation();

			var openRufusButton = new Button
			{
				Text = "Open Rufus Website",
				Location = new Point(166, 198),
				Size = new Size(150, 32)
			};
			openRufusButton.Click += (_, _) => OpenRufusWebsite();

			var printInstructionsButton = new Button
			{
				Text = "Print Instructions",
				Location = new Point(326, 198),
				Size = new Size(140, 32)
			};
			printInstructionsButton.Click += (_, _) => PrintInstructions();

			var instructionsTextBox = new TextBox
			{
				Location = new Point(16, 246),
				Size = new Size(710, 260),
				Multiline = true,
				ReadOnly = true,
				ScrollBars = ScrollBars.Vertical,
				Text = "1. Download Rufus from https://rufus.ie\r\n" +
					   "2. Select the recovery ISO shown above.\r\n" +
					   "3. Create a bootable USB drive in Rufus.\r\n" +
					   "4. Boot from the USB drive on the target machine.\r\n\r\n" +
					   "Restore options on the recovery media:\r\n" +
					   "- restore_gui: graphical interface\r\n" +
					   "- restore_tui: terminal UI\r\n" +
					   "- restore_cli: direct command-line restore"
			};

			var closeButton = new Button
			{
				Text = "Close",
				Size = new Size(100, 32),
				Location = new Point(626, 516),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.OK
			};

			Controls.Add(titleLabel);
			Controls.Add(isoPathHeaderLabel);
			Controls.Add(isoPathTextBox);
			Controls.Add(isoStatusLabel);
			Controls.Add(isoNoteLabel);
			Controls.Add(openIsoLocationButton);
			Controls.Add(openRufusButton);
			Controls.Add(printInstructionsButton);
			Controls.Add(instructionsTextBox);
			Controls.Add(closeButton);

			Load += (_, _) => CheckIsoFile();
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
					isoStatusLabel.ForeColor = Color.DarkGreen;
					openIsoLocationButton.Enabled = true;
					isoNoteLabel.Text = string.Empty;
				}
				else
				{
					isoStatusLabel.Text = "ISO file not found";
					isoStatusLabel.ForeColor = Color.DarkRed;
					openIsoLocationButton.Enabled = false;
					isoNoteLabel.Text = "The ISO should be deployed in the LinuxRestore folder. If it is missing, build it with the LinuxRestore BUILD-AND-CREATE-ISO.ps1 script.";
				}
			}
			catch (Exception ex)
			{
				isoStatusLabel.Text = $"Error: {ex.Message}";
				isoStatusLabel.ForeColor = Color.DarkRed;
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
