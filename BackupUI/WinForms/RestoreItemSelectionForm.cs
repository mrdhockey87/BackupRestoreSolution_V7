using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Windows;

namespace SecureServerBackup.WinForms
{
	internal sealed class RestoreItemSelectionForm : Form
	{
		private readonly ListBox itemsListBox;

		public RestoreItemSelectionForm(AvailableBackupInfo backup, RestorePoint restorePoint, IReadOnlyList<string> items)
		{
			ArgumentNullException.ThrowIfNull(backup);
			ArgumentNullException.ThrowIfNull(restorePoint);
			ArgumentNullException.ThrowIfNull(items);

			Backup = backup;
			RestorePoint = restorePoint;

			Text = "Select Restore Items";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(760, 430);
			MinimumSize = new Size(760, 430);
			BackColor = Color.White;

			var summaryLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 16),
				Size = new Size(720, 48),
				Text = $"Restore point: {restorePoint.DisplayName}{Environment.NewLine}Source: {restorePoint.FilePath}"
			};

			itemsListBox = new ListBox
			{
				Location = new Point(16, 74),
				Size = new Size(720, 280),
				SelectionMode = SelectionMode.MultiExtended,
				Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
			};

			foreach (string item in items.Where(item => !string.IsNullOrWhiteSpace(item)))
			{
				itemsListBox.Items.Add(item);
			}

			var helpLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 362),
				Size = new Size(720, 24),
				Text = "Select one or more files or folders to restore, then click Next."
			};

			var nextButton = new Button
			{
				Text = "Next",
				Size = new Size(96, 32),
				Location = new Point(540, 392),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				Enabled = itemsListBox.Items.Count > 0
			};
			nextButton.Click += (_, _) => ConfirmSelection();

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(96, 32),
				Location = new Point(640, 392),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.Cancel
			};

			AcceptButton = nextButton;
			CancelButton = cancelButton;

			Controls.Add(summaryLabel);
			Controls.Add(itemsListBox);
			Controls.Add(helpLabel);
			Controls.Add(nextButton);
			Controls.Add(cancelButton);
		}

		public AvailableBackupInfo Backup { get; }

		public RestorePoint RestorePoint { get; }

		public IReadOnlyList<string> SelectedItems { get; private set; } = Array.Empty<string>();

		private void ConfirmSelection()
		{
			List<string> selectedItems = itemsListBox.SelectedItems.Cast<object>()
				.Select(item => item?.ToString() ?? string.Empty)
				.Where(item => !string.IsNullOrWhiteSpace(item))
				.ToList();

			if (selectedItems.Count == 0)
			{
				MessageBox.Show(this, "Please select at least one file or folder to continue.", "Restore Items Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			SelectedItems = selectedItems;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
