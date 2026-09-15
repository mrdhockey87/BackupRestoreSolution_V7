using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed class ExclusionsManagementForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly ListBox exclusionsListBox;
		private readonly TextBox extensionPatternTextBox;
		private readonly Label noExclusionsLabel;
		private readonly Label statusLabel;

		public ExclusionsManagementForm()
			: this([@"*.tmp", @"C:\Temp", @"C:\Users\Admin\Documents\draft.txt"])
		{
		}

		public ExclusionsManagementForm(List<string> currentExclusions)
		{
			Exclusions = new List<string>(currentExclusions ?? new List<string>());

			Text = "Manage Exclusions";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(720, 520);
			ClientSize = new Size(720, 520);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Custom Backup Exclusions",
				Font = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(16, 16)
			};

			var buttonPanel = new FlowLayoutPanel
			{
				Location = new Point(16, 48),
				Size = new Size(680, 36),
				WrapContents = true
			};
			buttonPanel.Controls.Add(CreateButton("Add File...", (_, _) => BrowseFile()));
			buttonPanel.Controls.Add(CreateButton("Add Folder...", (_, _) => BrowseFolder()));
			buttonPanel.Controls.Add(CreateButton("Remove Selected", (_, _) => RemoveSelected()));
			buttonPanel.Controls.Add(CreateButton("Clear All", (_, _) => ClearAll()));

			var patternLabel = new Label
			{
				Text = "Extension or wildcard pattern:",
				AutoSize = true,
				Location = new Point(16, 96)
			};

			extensionPatternTextBox = new TextBox
			{
				Location = new Point(16, 122),
				Size = new Size(520, 24)
			};
			extensionPatternTextBox.KeyDown += (_, e) =>
			{
				if (e.KeyCode == Keys.Enter)
				{
					AddPatternFromTextBox();
					e.SuppressKeyPress = true;
				}
			};

			var addPatternButton = new Button
			{
				Text = "Add Pattern",
				Size = new Size(120, 28),
				Location = new Point(552, 120)
			};
			addPatternButton.Click += (_, _) => AddPatternFromTextBox();

			exclusionsListBox = new ListBox
			{
				Location = new Point(16, 160),
				Size = new Size(680, 270),
				SelectionMode = SelectionMode.MultiExtended,
				HorizontalScrollbar = true
			};

			noExclusionsLabel = new Label
			{
				Text = "No custom exclusions defined.",
				AutoSize = true,
				Location = new Point(24, 168),
				ForeColor = Color.DimGray,
				BackColor = Color.Transparent
			};

			statusLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 440),
				Size = new Size(680, 40),
				ForeColor = Color.DimGray
			};

			var okButton = new Button
			{
				Text = "OK",
				Size = new Size(90, 30),
				Location = new Point(516, 486),
				DialogResult = DialogResult.OK
			};

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(90, 30),
				Location = new Point(606, 486),
				DialogResult = DialogResult.Cancel
			};

			Controls.Add(titleLabel);
			Controls.Add(buttonPanel);
			Controls.Add(patternLabel);
			Controls.Add(extensionPatternTextBox);
			Controls.Add(addPatternButton);
			Controls.Add(exclusionsListBox);
			Controls.Add(noExclusionsLabel);
			Controls.Add(statusLabel);
			Controls.Add(okButton);
			Controls.Add(cancelButton);

			if (IsInDesignMode)
			{
				LoadExclusions();
				UpdateStatus();
			}
			else
			{
				Load += (_, _) =>
				{
					LoadExclusions();
					UpdateStatus();
				};
			}
		}

		public List<string> Exclusions { get; }

		private static Button CreateButton(string text, EventHandler handler)
		{
			var button = new Button
			{
				Text = text,
				AutoSize = true,
				MinimumSize = new Size(120, 30),
				Margin = new Padding(0, 0, 8, 0)
			};
			button.Click += handler;
			return button;
		}

		private void LoadExclusions()
		{
			exclusionsListBox.Items.Clear();
			foreach (string exclusion in Exclusions)
			{
				exclusionsListBox.Items.Add($"{GetIconForExclusion(exclusion)} {exclusion}");
			}

			noExclusionsLabel.Visible = Exclusions.Count == 0;
		}

		private static string GetIconForExclusion(string exclusion)
		{
			if (exclusion.StartsWith("*", StringComparison.Ordinal))
			{
				return "📄";
			}
			if (Directory.Exists(exclusion))
			{
				return "📁";
			}
			if (File.Exists(exclusion))
			{
				return "📝";
			}
			if (exclusion.Contains('*') || exclusion.Contains('?'))
			{
				return "📄";
			}
			return "❓";
		}

		private void BrowseFile()
		{
			using var dialog = new OpenFileDialog
			{
				Title = "Select File to Exclude",
				Multiselect = true,
				CheckFileExists = true,
				Filter = "All Files (*.*)|*.*"
			};

			if (dialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (string file in dialog.FileNames)
			{
				AddExclusion(file);
			}
		}

		private void BrowseFolder()
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select Folder to Exclude",
				ShowNewFolderButton = false
			};

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				AddExclusion(dialog.SelectedPath);
			}
		}

		private void AddPatternFromTextBox()
		{
			string pattern = extensionPatternTextBox.Text.Trim();
			if (string.IsNullOrWhiteSpace(pattern))
			{
				MessageBox.Show(this, "Please enter a file extension pattern.", "No Pattern", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (!pattern.StartsWith("*", StringComparison.Ordinal))
			{
				pattern = "*" + pattern;
			}

			if (!pattern.Contains('.') && !pattern.Contains('?'))
			{
				DialogResult result = MessageBox.Show(this,
					$"The pattern '{pattern}' doesn't contain a file extension.\n\nDid you mean to add '.' (e.g., '*.tmp' instead of '*tmp')?\n\nAdd it anyway?",
					"Confirm Pattern",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);
				if (result != DialogResult.Yes)
				{
					return;
				}
			}

			AddExclusion(pattern);
			extensionPatternTextBox.Clear();
		}

		private void AddExclusion(string exclusion)
		{
			exclusion = exclusion.Replace("/", "\\", StringComparison.Ordinal);
			if (Exclusions.Any(existing => existing.Equals(exclusion, StringComparison.OrdinalIgnoreCase)))
			{
				statusLabel.Text = $"Exclusion already exists: {exclusion}";
				return;
			}

			Exclusions.Add(exclusion);
			LoadExclusions();
			UpdateStatus();
			statusLabel.Text = $"Added: {exclusion}";
		}

		private void RemoveSelected()
		{
			if (exclusionsListBox.SelectedIndices.Count == 0)
			{
				return;
			}

			DialogResult result = MessageBox.Show(this,
				$"Remove {exclusionsListBox.SelectedIndices.Count} exclusion(s)?",
				"Confirm Removal",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			if (result != DialogResult.Yes)
			{
				return;
			}

			List<int> indices = exclusionsListBox.SelectedIndices.Cast<int>().OrderByDescending(index => index).ToList();
			foreach (int index in indices)
			{
				Exclusions.RemoveAt(index);
			}

			LoadExclusions();
			UpdateStatus();
			statusLabel.Text = $"Removed {indices.Count} exclusion(s).";
		}

		private void ClearAll()
		{
			if (Exclusions.Count == 0)
			{
				return;
			}

			DialogResult result = MessageBox.Show(this,
				$"Remove ALL {Exclusions.Count} custom exclusions?\n\nSystem exclusions will remain and cannot be removed.",
				"Confirm Clear All",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);
			if (result != DialogResult.Yes)
			{
				return;
			}

			Exclusions.Clear();
			LoadExclusions();
			UpdateStatus();
			statusLabel.Text = "All custom exclusions removed.";
		}

		private void UpdateStatus()
		{
			if (Exclusions.Count == 0)
			{
				statusLabel.Text = "No custom exclusions defined. System exclusions still apply.";
				return;
			}

			int fileCount = Exclusions.Count(File.Exists);
			int folderCount = Exclusions.Count(Directory.Exists);
			int patternCount = Exclusions.Count(exclusion => exclusion.Contains('*') || exclusion.Contains('?'));
			statusLabel.Text = $"{Exclusions.Count} exclusion(s): {fileCount} file(s), {folderCount} folder(s), {patternCount} pattern(s)";
		}
	}
}
