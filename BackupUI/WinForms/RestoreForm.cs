using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SecureServerBackup.WinForm
{
	public partial class RestoreForm : Form
	{
		// Subscribe to these from your backup/restore application code.
		public event EventHandler? BackupLoadRequested;
		public event EventHandler? RestoreRequested;

		public RestoreForm()
		{
			InitializeComponent();
			ShowStep(1);
		}

		public string BackupSource => txtBackupSource.Text ?? string.Empty;

		public string? SelectedBackupDate =>
			lstBackupDates.SelectedItems.Count == 0
				? null
				: lstBackupDates.SelectedItems[0].Text;

		public string? RestoreDestination =>
			rbNewLocation.Checked ? txtRestoreDestination.Text : null;

		public bool OverwriteExistingFiles => chkOverwrite.Checked;
		public bool PreservePermissions => chkPreservePermissions.Checked;
		public bool RestoreSystemState => chkRestoreSystemState.Checked;
		public bool RestoreAsHyperV => chkRestoreAsHyperV.Checked;
		public string VMName => txtVMName.Text ?? string.Empty;
		public string VMStoragePath => txtVMStorage.Text ?? string.Empty;
		public bool StartVMAfterRestore => chkStartVM.Checked;

		public void SetBackupInfo(string message)
		{
			txtBackupInfo.Text = message;
		}

		public void ClearBackupDates()
		{
			lstBackupDates.Items.Clear();
		}

		public void AddBackupDate(string displayDate, string backupType, string size)
		{
			var item = new ListViewItem(displayDate);
			item.SubItems.Add(backupType);
			item.SubItems.Add(size);
			lstBackupDates.Items.Add(item);
		}

		public void ClearRestoreContents()
		{
			treeRestoreContents.Nodes.Clear();
		}

		public TreeNodeCollection RestoreContentNodes => treeRestoreContents.Nodes;

		public IEnumerable<TreeNode> CheckedRestoreNodes
		{
			get
			{
				foreach (TreeNode root in treeRestoreContents.Nodes)
					foreach (TreeNode node in GetCheckedNodes(root))
						yield return node;
			}
		}

		public void SetProgress(int percent, string message)
		{
			progressBar.Visible = true;
			txtProgress.Visible = true;
			progressBar.Value = Math.Max(0, Math.Min(100, percent));
			txtProgress.Text = message;
		}

		private static IEnumerable<TreeNode> GetCheckedNodes(TreeNode node)
		{
			if (node.Checked)
				yield return node;

			foreach (TreeNode child in node.Nodes)
				foreach (TreeNode checkedNode in GetCheckedNodes(child))
					yield return checkedNode;
		}

		private void ShowStep(int step)
		{
			pnlStep1.Visible = step == 1;
			pnlStep2.Visible = step == 2;
			pnlStep3.Visible = step == 3;

			if (step == 1) pnlStep1.BringToFront();
			if (step == 2) pnlStep2.BringToFront();
			if (step == 3) pnlStep3.BringToFront();
		}

		private static void SetCheckedRecursive(TreeNode node, bool isChecked)
		{
			node.Checked = isChecked;

			foreach (TreeNode child in node.Nodes)
				SetCheckedRecursive(child, isChecked);
		}

		private static string BrowseForFolder(string currentPath)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				if (Directory.Exists(currentPath))
					dialog.SelectedPath = currentPath;

				return dialog.ShowDialog() == DialogResult.OK
					? dialog.SelectedPath
					: currentPath;
			}
		}

		private void BrowseBackup_Click(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog())
			{
				dialog.Title = "Select backup file";
				dialog.Filter = "All files (*.*)|*.*";

				if (dialog.ShowDialog(this) == DialogResult.OK)
					txtBackupSource.Text = dialog.FileName;
			}

			// A folder path can also be entered directly in txtBackupSource.
		}

		private void LoadBackup_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtBackupSource.Text))
			{
				MessageBox.Show(this, "Select a backup source first.");
				return;
			}

			if (BackupLoadRequested == null)
			{
				MessageBox.Show(this,
					"Connect BackupLoadRequested to your backup-loading code.");
				return;
			}

			BackupLoadRequested(this, EventArgs.Empty);
		}

		private void Step1Next_Click(object sender, EventArgs e)
		{
			if (lstBackupDates.SelectedItems.Count == 0)
			{
				MessageBox.Show(this, "Select a backup date first.");
				return;
			}

			ShowStep(2);
		}

		private void ExpandAll_Click(object sender, EventArgs e) =>
			treeRestoreContents.ExpandAll();

		private void CollapseAll_Click(object sender, EventArgs e) =>
			treeRestoreContents.CollapseAll();

		private void SelectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in treeRestoreContents.Nodes)
				SetCheckedRecursive(node, true);
		}

		private void UnselectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in treeRestoreContents.Nodes)
				SetCheckedRecursive(node, false);
		}

		private void Step2Back_Click(object sender, EventArgs e) => ShowStep(1);

		private void Step2Next_Click(object sender, EventArgs e)
		{
			using (var nodes = CheckedRestoreNodes.GetEnumerator())
			{
				if (!nodes.MoveNext())
				{
					MessageBox.Show(this, "Check at least one item to restore.");
					return;
				}
			}

			ShowStep(3);
		}

		private void RestoreLocation_Changed(object sender, EventArgs e)
		{
			pnlNewLocation.Visible = rbNewLocation.Checked;
		}

		private void BrowseDestination_Click(object sender, EventArgs e)
		{
			txtRestoreDestination.Text =
				BrowseForFolder(txtRestoreDestination.Text);
		}

		private void HyperV_CheckedChanged(object sender, EventArgs e)
		{
			pnlHyperV.Visible = chkRestoreAsHyperV.Checked;
		}

		private void BrowseVMStorage_Click(object sender, EventArgs e)
		{
			txtVMStorage.Text = BrowseForFolder(txtVMStorage.Text);
		}

		private void Step3Back_Click(object sender, EventArgs e) => ShowStep(2);

		private void StartRestore_Click(object sender, EventArgs e)
		{
			if (rbNewLocation.Checked &&
				string.IsNullOrWhiteSpace(txtRestoreDestination.Text))
			{
				MessageBox.Show(this, "Select a new restore path.");
				return;
			}

			if (chkRestoreAsHyperV.Checked &&
				(string.IsNullOrWhiteSpace(txtVMName.Text) ||
				 string.IsNullOrWhiteSpace(txtVMStorage.Text)))
			{
				MessageBox.Show(this, "Enter a VM name and storage path.");
				return;
			}

			if (RestoreRequested == null)
			{
				MessageBox.Show(this,
					"Connect RestoreRequested to your restore code.");
				return;
			}

			RestoreRequested(this, EventArgs.Empty);
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}