using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class ExclusionsManagementForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		public ExclusionsManagementForm()
			: this([@"*.tmp", @"C:\Temp", @"C:\Users\Admin\Documents\draft.txt"])
		{
		}

		public ExclusionsManagementForm(List<string> currentExclusions)
		{
			Exclusions = new List<string>(currentExclusions ?? new List<string>());
			InitializeComponent();

			if (IsInDesignMode)
			{
				LoadExclusions();
				UpdateStatus();
			}
			else
			{
				Load += ExclusionsManagementForm_Load;
			}
		}

		public List<string> Exclusions { get; }

		private void ExclusionsManagementForm_Load(object? sender, EventArgs e)
		{
			LoadExclusions();
			UpdateStatus();
		}

		private void AddFileButton_Click(object? sender, EventArgs e)
		{
			BrowseFile();
		}

		private void AddFolderButton_Click(object? sender, EventArgs e)
		{
			BrowseFolder();
		}

		private void RemoveSelectedButton_Click(object? sender, EventArgs e)
		{
			RemoveSelected();
		}

		private void ClearAllButton_Click(object? sender, EventArgs e)
		{
			ClearAll();
		}

		private void ExtensionPatternTextBox_KeyDown(object? sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				AddPatternFromTextBox();
				e.SuppressKeyPress = true;
			}
		}

		private void AddPatternButton_Click(object? sender, EventArgs e)
		{
			AddPatternFromTextBox();
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
