using SecureServerBackup.Enums;
using SecureServerBackup.Helpers;
using SecureServerBackup.Models;
using SecureServerBackup.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureServerBackup.WinForm
{
	public partial class RestoreFormNew : Form
	{
#pragma warning disable CS0169
		private const string RestoreLogJobName = "[Restore]";
		private ObservableCollection<RestorePoint> restorePoints = new();
		private List<string> backupFiles = new();
		private readonly AvailableBackupInfo? _preloadedBackup;
		private readonly RestoreSelectionContext? _preselectedRestore;
		private readonly bool _requireAlternateDestination;
		private readonly List<string> _bootProtectedTargets = new();
		private string? _selectedTargetPath;
		private int? _selectedTargetDiskNumber;
		private NativeBackupMountManager.RestoreDiskPlan? _diskRestorePlan;
		private bool _isHyperVBackupPoint;
		private bool _suppressRestorePointSelectionChanged;
		private RestorePoint? _activeRestorePoint;

		// Restore target drive tree items
		private readonly ObservableCollection<DriveTreeItem> _restoreTargetItems = new();
		private bool _showHiddenPartitionsTarget;
		private bool _isLoadingTargets;
		private bool _reloadRestoreTargetsAfterLoad;

		// Selected volumes/disk-group from the restore-volume selection dialog
		private VolumeInfo? _selectedRestoreVolume;
		private IReadOnlyList<VolumeInfo>? _selectedRestoreDiskGroup;
		public event EventHandler? ScanBackupRequested;
		public event EventHandler? RefreshTargetsRequested;
		public event EventHandler? RestoreRequested;

#pragma warning restore CS0169 // Restore warning behavior for the rest of the file

#pragma warning disable CS0414
		private RestoreTargetKind _lastBuiltTargetKind = RestoreTargetKind.FileOrFolder;
		private RestoreTargetKind _restoreTargetKind = RestoreTargetKind.FileOrFolder;
#pragma warning restore CS0414 // Restore warning behavior for the rest of the file
		public RestoreFormNew()
		{
			InitializeComponent();

			cmbHyperVRestoreTarget.SelectedIndex = 0;
			cmbNewHyperVGeneration.SelectedIndex = 1;

			// Keep the options panel sized to its scroll viewport.
			optionsScroll.ClientSizeChanged += (sender, args) =>
			{
				optionsContent.Width =
					Math.Max(475, optionsScroll.ClientSize.Width - 20);
			};

			UpdateOptionVisibility();
		}
		/*
		private void init_vars(){
			_restoreTargetKind = RestoreTargetKind.FileOrFolder;
			restorePoints = new ObservableCollection<RestorePoint>();
			backupFiles = new List<string>();
			_restoreTargetItems.Clear();
			_bootProtectedTargets.Clear();
			_selectedTargetPath = "";
			_selectedTargetDiskNumber = -1;
			_diskRestorePlan = new NativeBackupMountManager.RestoreDiskPlan();
			_isHyperVBackupPoint = false;
			_suppressRestorePointSelectionChanged = false;
			_activeRestorePoint = new RestorePoint();
			_showHiddenPartitionsTarget = false;
			_isLoadingTargets = false;
			_reloadRestoreTargetsAfterLoad = false;
			_lastBuiltTargetKind = RestoreTargetKind.FileOrFolder;
			_selectedRestoreVolume = new VolumeInfo();
			_selectedRestoreDiskGroup = new List<VolumeInfo>();
		}*/

		public string BackupSource => txtBackupSource.Text ?? string.Empty;

		public RestorePoint? SelectedRestorePoint =>
			lstRestorePoints.SelectedItem as RestorePoint;

		public string FolderDestination =>
			txtFolderRestoreDestination.Text.Trim();

		public bool OverwriteExisting => chkOverwrite.Checked;
		public bool PreservePermissions => chkPreservePermissions.Checked;
		public bool VerifyAfterRestore => chkVerifyAfterRestore.Checked;

		public void SetBackupSource(string path)
		{
			txtBackupSource.Text = path ?? string.Empty;
		}

		public void SetBackupSummary(
			string fileCount, string totalSize, string restorePointCount)
		{
			txtBackupFileCount.Text = fileCount;
			txtBackupTotalSize.Text = totalSize;
			txtBackupRestorePointCount.Text = restorePointCount;
			pnlBackupInfo.Visible = true;
			grpRestoreOptions.Enabled = true;
		}

		public void AddRestorePoint(RestorePoint point)
		{
			lstRestorePoints.Items.Add(point);
		}

		public void ClearRestorePoints()
		{
			lstRestorePoints.Items.Clear();
		}

		public void AddBackupItem(string item)
		{
			lstBackupItems.Items.Add(item);
		}

		public void ClearBackupItems()
		{
			lstBackupItems.Items.Clear();
		}

		public TreeNodeCollection RestoreTargetNodes =>
			treeViewRestoreTarget.Nodes;

		public void SetTargetsLoading(bool loading)
		{
			loadingTargetOverlay.Visible = loading;
			if (loading)
				loadingTargetOverlay.BringToFront();
		}

		private void UpdateOptionVisibility()
		{
			pnlItemSelection.Visible = rbRestoreSelected.Checked;

			bool hyperVPoint = pnlHyperVRestoreMode.Visible;
			bool vmMode = hyperVPoint &&
						  cmbHyperVRestoreTarget.SelectedIndex == 3;

			pnlHyperVVmOptions.Visible = vmMode;
			pnlHyperVReplaceExistingOptions.Visible =
				vmMode && rbHyperVReplaceExisting.Checked;
			pnlHyperVDirectoryOptions.Visible =
				vmMode && rbHyperVRestoreToDirectory.Checked;

			pnlRegularHyperVRestore.Visible =
				chkRestoreToHyperVDisk.Visible &&
				chkRestoreToHyperVDisk.Checked;

			pnlExistingHyperVVmOptions.Visible =
				pnlRegularHyperVRestore.Visible &&
				chkAttachToExistingHyperVVm.Checked;
			pnlNewHyperVVmOptions.Visible =
				pnlRegularHyperVRestore.Visible &&
				rbCreateNewHyperVVm.Checked;

			pnlHyperVCloneDestination.Visible = vmMode;
			pnlHyperVCloneAlternate.Visible =
				vmMode && rbHyperVCloneAlternate.Checked;

			bool filesMode = !vmMode &&
							 (!hyperVPoint ||
							  cmbHyperVRestoreTarget.SelectedIndex == 0) &&
							 !chkRestoreToHyperVDisk.Checked;

			pnlLocationChoice.Visible = filesMode;
			grpRestoreTarget.Visible = !vmMode;
			grpRestoreTarget.Enabled = !filesMode && !vmMode;

			btnRestore.Text =
				hyperVPoint &&
				(cmbHyperVRestoreTarget.SelectedIndex == 1 ||
				 cmbHyperVRestoreTarget.SelectedIndex == 2)
					? "Next"
					: "Start Restore";

			UpdateRestoreActionState();
		}

		private void UpdateRestoreActionState()
		{
			if (btnRestore == null)
				return;

			bool hasPoint = SelectedRestorePoint != null;
			bool needsFolder = pnlLocationChoice.Visible;
			bool needsTree = grpRestoreTarget.Visible &&
							 grpRestoreTarget.Enabled;

			btnRestore.Enabled =
				hasPoint &&
				(!needsFolder ||
				 !string.IsNullOrWhiteSpace(txtFolderRestoreDestination.Text)) &&
				(!needsTree ||
				 treeViewRestoreTarget.SelectedNode?.Tag is RestoreTargetTag tag &&
				 tag.Selectable);
		}

		private void BrowseBackup_Click(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog())
			{
				dialog.Title = "Select Backup File";
				dialog.Filter =
					"Backup files (*.ssb;*.bak;*.backup)|*.ssb;*.bak;*.backup|" +
					"All files (*.*)|*.*";

				if (dialog.ShowDialog(this) == DialogResult.OK)
					txtBackupSource.Text = dialog.FileName;
			}

			// For a folder source, your caller can use SetBackupSource,
			// or add a separate FolderBrowserDialog entry point.
		}

		private void ScanBackup_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtBackupSource.Text))
			{
				MessageBox.Show(this, "Select a backup source first.");
				return;
			}

			ScanBackupRequested?.Invoke(this, EventArgs.Empty);
		}

		private void RestorePoints_SelectionChanged(
			object sender, EventArgs e)
		{
			UpdateRestoreActionState();
		}

		private void RestoreItems_SelectionChanged(
			object sender, EventArgs e)
		{
			UpdateRestoreActionState();
		}

		private void RestoreScope_Changed(object sender, EventArgs e)
		{
			UpdateOptionVisibility();
		}

		private void HyperVRestoreTarget_Changed(
			object sender, EventArgs e)
		{
			UpdateOptionVisibility();
		}

		private void HyperVVmRestoreMode_Changed(
			object sender, EventArgs e)
		{
			UpdateOptionVisibility();
		}

		private void RegularHyperVRestoreOption_Changed(
			object sender, EventArgs e)
		{
			UpdateOptionVisibility();
		}

		private void ExistingHyperVVmAttach_Changed(
			object sender, EventArgs e)
		{
			UpdateOptionVisibility();
		}

		private void HyperVCloneDestination_Changed(
			object sender, EventArgs e)
		{
			UpdateOptionVisibility();
		}

		private void TargetLocation_TextChanged(
			object sender, EventArgs e)
		{
			UpdateRestoreActionState();
		}

		private void BrowseRestoreDestination_Click(
			object sender, EventArgs e)
		{
			BrowseFolderInto(txtFolderRestoreDestination);
		}

		private void BrowseHyperVRestoreDirectory_Click(
			object sender, EventArgs e)
		{
			BrowseFolderInto(txtHyperVRestoreDirectory);
		}

		private void BrowseNewHyperVVmLocation_Click(
			object sender, EventArgs e)
		{
			BrowseFolderInto(txtNewHyperVVmPath);
		}

		private void BrowseHyperVCloneVmFolder_Click(
			object sender, EventArgs e)
		{
			BrowseFolderInto(txtHyperVCloneVmFolder);
		}

		private void BrowseHyperVCloneDiskFolder_Click(
			object sender, EventArgs e)
		{
			BrowseFolderInto(txtHyperVCloneDiskFolder);
		}

		private void BrowseHyperVVirtualDisk_Click(
			object sender, EventArgs e)
		{
			using (var dialog = new SaveFileDialog())
			{
				dialog.Title = "Select Hyper-V virtual disk destination";
				dialog.Filter = "Hyper-V virtual disk (*.vhdx)|*.vhdx";
				dialog.DefaultExt = "vhdx";
				dialog.AddExtension = true;
				dialog.OverwritePrompt = false;

				if (dialog.ShowDialog(this) == DialogResult.OK)
					txtHyperVVirtualDiskPath.Text = dialog.FileName;
			}
		}

		private void BrowseFolderInto(TextBox destination)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.ShowNewFolderButton = true;

				if (Directory.Exists(destination.Text))
					dialog.SelectedPath = destination.Text;

				if (dialog.ShowDialog(this) == DialogResult.OK)
					destination.Text = dialog.SelectedPath;
			}
		}

		private void RefreshRestoreTarget_Click(
			object sender, EventArgs e)
		{
			RefreshTargetsRequested?.Invoke(this, EventArgs.Empty);
		}

		private void ExpandAllTarget_Click(object sender, EventArgs e)
		{
			treeViewRestoreTarget.ExpandAll();
		}

		private void CollapseAllTarget_Click(object sender, EventArgs e)
		{
			treeViewRestoreTarget.CollapseAll();
		}

		private void ShowHiddenPartitionsTarget_Click(
			object sender, EventArgs e)
		{
			RefreshTargetsRequested?.Invoke(this, EventArgs.Empty);
		}

		private void TreeViewRestoreTarget_AfterSelect(
			object sender, TreeViewEventArgs e)
		{
			var target = e.Node?.Tag as RestoreTargetTag;

			txtSelectedTargetLabel.Text =
				target != null && target.Selectable
					? "Selected: " + e.Node?.Text
					: "No target selected";

			UpdateRestoreActionState();
		}

		private void StartRestore_Click(object sender, EventArgs e)
		{
			if (RestoreRequested == null)
			{
				MessageBox.Show(
					this,
					"Connect RestoreRequested to the migrated restore workflow.",
					"Restore workflow not connected");
				return;
			}

			RestoreRequested(this, EventArgs.Empty);
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		// Attach this to a TreeNode.Tag when populating RestoreTargetNodes.
		// Do not mark a boot/system target selectable.
		public sealed class RestoreTargetTag
		{
			public string Path { get; set; } = string.Empty;
			public int? DiskNumber { get; set; }
			public bool Selectable { get; set; }
		}

		private static DateTime GetEntryTimestamp(string path)
		{
			return File.Exists(path)
				? File.GetCreationTime(path)
				: Directory.GetCreationTime(path);
		}

		private static DateTime GetRestorePointTimestamp(string backupPath, IReadOnlyList<RestorePointArchiveImage> archiveImages)
		{
			if (TryGetBackupStartTime(backupPath, archiveImages, out DateTime backupStartTime))
			{
				return backupStartTime;
			}

			return GetEntryTimestamp(backupPath);
		}

		private static bool TryGetBackupStartTime(string backupPath, IReadOnlyList<RestorePointArchiveImage> archiveImages, out DateTime backupStartTime)
		{
			if (TryGetArchiveBackupStartTime(archiveImages, out backupStartTime))
			{
				return true;
			}

			return TryGetFileBackupStartTime(backupPath, out backupStartTime);
		}

		private static bool TryGetArchiveBackupStartTime(IReadOnlyList<RestorePointArchiveImage> archiveImages, out DateTime backupStartTime)
		{
			backupStartTime = default;

			if (archiveImages == null)
			{
				return false;
			}

			DateTime? earliestStartTime = archiveImages
				.Select(image => image.BackupStartTime)
				.Where(timestamp => timestamp.HasValue)
				.OrderBy(timestamp => timestamp)
				.FirstOrDefault();

			if (!earliestStartTime.HasValue)
			{
				return false;
			}

			backupStartTime = earliestStartTime.Value;
			return true;
		}

		private static IReadOnlyList<string> ParseListedBackupItems(string listedContents)
		{
			if (string.IsNullOrWhiteSpace(listedContents))
			{
				return Array.Empty<string>();
			}

			return listedContents
				.Split('\n', StringSplitOptions.RemoveEmptyEntries)
				.Select(item => item.Trim())
				.Where(item => !string.IsNullOrWhiteSpace(item) && !string.Equals(item, "(No files in backup)", StringComparison.OrdinalIgnoreCase))
				.ToArray();
		}

		private static bool TryGetFileBackupStartTime(string backupPath, out DateTime backupStartTime)
		{
			backupStartTime = default;

			string metadataPath = Path.Combine(backupPath, "backup_metadata.dat");
			if (!Directory.Exists(backupPath) || !File.Exists(metadataPath))
			{
				return false;
			}

			foreach (string line in File.ReadLines(metadataPath))
			{
				if (!line.StartsWith("#BACKUP_START_TIME|", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				string timestampText = line["#BACKUP_START_TIME|".Length..].Trim();
				if (DateTime.TryParse(timestampText, out backupStartTime))
				{
					return true;
				}

				return false;
			}

			return false;
		}

		private MountedVirtualDiskScope MountPrimaryHyperVVirtualDisk(string backupPointPath)
		{
			string? virtualDiskPath = HyperVRestorePointHelper.FindPrimaryVirtualDisk(backupPointPath);
			if (string.IsNullOrWhiteSpace(virtualDiskPath))
			{
				throw new InvalidOperationException("No VHD or VHDX guest disk was found in the selected Hyper-V backup point.");
			}

			var mountResult = BackupMountManager.MountVirtualDiskReadOnly(virtualDiskPath);
			if (!mountResult.Success || string.IsNullOrWhiteSpace(mountResult.DriveLetter))
			{
				throw new InvalidOperationException($"Failed to mount the Hyper-V guest disk: {mountResult.Error}");
			}

			string driveRoot = mountResult.DriveLetter.EndsWith(":", StringComparison.Ordinal)
				? mountResult.DriveLetter + "\\"
				: mountResult.DriveLetter;

			return new MountedVirtualDiskScope(virtualDiskPath, driveRoot);
		}

		public RestoreFormNew(AvailableBackupInfo backup, bool requireAlternateDestination)
			: this()
		{
			_preloadedBackup = backup;
			_requireAlternateDestination = requireAlternateDestination;
		}

		public RestoreFormNew(RestoreSelectionContext restoreSelection)
			: this(restoreSelection?.Backup ?? throw new ArgumentNullException(nameof(restoreSelection)), restoreSelection.RequireAlternateDestination)
		{
			_preselectedRestore = restoreSelection;
		}
		/*
		private async Task ScanBackupAsync()
		{
			if (string.IsNullOrWhiteSpace(txtBackupSource.Text))
			{
				return;
			}

			string originalTitle = Title;
			Title = "Restore Backup - Scanning...";
			System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

			try
			{
				await ScanBackupSet(txtBackupSource.Text);

				pnlBackupInfo.Visibility = Visibility.Visible;
				grpRestoreOptions.IsEnabled = true;
				UpdateSelectedRestoreTargetKind();
				UpdateRestoreActionState();
			}
			finally
			{
				Title = originalTitle;
				System.Windows.Input.Mouse.OverrideCursor = null;
			}
		}
		*/
		private long GetTargetDiskCapacityBytes(int targetDiskNumber)
		{
			var diskItem = _restoreTargetItems.FirstOrDefault(item =>
				item.ItemType == DriveTreeItemType.Disk &&
				item.PartitionNumber == targetDiskNumber);
			if (diskItem != null && diskItem.Size > 0)
			{
				return diskItem.Size;
			}

			try
			{
				using var searcher = new ManagementObjectSearcher(
					$"SELECT Size FROM Win32_DiskDrive WHERE Index = {targetDiskNumber}");
				foreach (ManagementObject disk in searcher.Get())
				{
					if (long.TryParse(disk["Size"]?.ToString(), out long size))
					{
						return size;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"GetTargetDiskCapacityBytes: {ex.Message}");
			}

			return -1;
		}
		private static long GetRequestedRestoreSize(VolumeInfo volume)
		{
			ArgumentNullException.ThrowIfNull(volume);
			return volume.TargetSize > 0 ? volume.TargetSize : volume.Size;
		}
		private static bool AreSizesEquivalent(long expectedBytes, long actualBytes)
		{
			if (expectedBytes <= 0 || actualBytes <= 0)
			{
				return false;
			}

			long toleranceBytes = Math.Max(256L * 1024 * 1024, (long)(Math.Max(expectedBytes, actualBytes) * 0.02));
			return Math.Abs(expectedBytes - actualBytes) <= toleranceBytes;
		}
		internal static bool ShouldReuseExistingTargetVolumeLayout(long requestedSizeBytes, long targetVolumeSizeBytes, long targetDiskSizeBytes)
		{
			return AreSizesEquivalent(requestedSizeBytes, targetVolumeSizeBytes) ||
				   AreSizesEquivalent(requestedSizeBytes, targetDiskSizeBytes);
		}
	}

}