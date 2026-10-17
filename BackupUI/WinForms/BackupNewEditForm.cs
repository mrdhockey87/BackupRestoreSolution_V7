//BackupNewEditForm.cs
using SecureServerBackup.Enums;
using SecureServerBackup.Helpers;
using SecureServerBackup.Models;
using SecureServerBackup.Services;

using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using BackupEngineInterop = SecureServerBackup.Services.BackupEngineInterop;


namespace SecureServerBackup.WinForms
{
	public partial class BackupNewEditForm : Form
	{
		private const int MaxRetentionValue = 30;

		private readonly List<DriveTreeItem> driveItems = new();
		private readonly JobManager jobManager = new();
		private BackupJob? existingJob;
		private BackupJob? _editingJob;
		private List<string>? _pathsToPreselect;
		private List<string>? _tempUserExclusions;
		private List<string> _missingSavedSelectionPaths = new();
		private HashSet<string> _pendingSelectionPaths = new(StringComparer.OrdinalIgnoreCase);

		private bool _hasSavedEncryptionPassword;
		private string? _decryptedEncryptionPassword;
		private bool _isLoadingDrives;
		private bool _reloadDrivesAfterLoad;
		private bool _isInitializingJobData;
		private bool _hasCompletedInitialDriveLoad;
		private bool _syncingTree;
		private bool _showHiddenPartitions;
		private readonly CancellationTokenSource _shutdownCts = new();
		private bool hasSourceSelected;
		private bool hasTargetSelected;
		private bool volumeConfigShown;

		private CancellationToken ShutdownToken => _shutdownCts.Token;

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_shutdownCts.Cancel();
			base.OnFormClosed(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_shutdownCts.Cancel();
			base.OnFormClosing(e);
		}

		public BackupNewEditForm()
		{
			InitializeComponent();
			foreach (CheckBox day in new[] { chkMonday, chkTuesday, chkWednesday, chkThursday, chkFriday, chkSaturday, chkSunday })
			{
				day.AutoSize = true;
				day.Margin = new Padding(0, 0, 10, 4);
			}
			PopulateStaticCombos();
			UpdateDynamicPanels();
		}

		public BackupNewEditForm(BackupJob job) : this()
		{
			existingJob = job;
			LoadJobData(job);
		}

		// =====================================================================
		// Lifecycle and layout
		// =====================================================================

		protected override async void OnShown(EventArgs e)
		{
			base.OnShown(e);
			if (DesignMode)
			{
				return;
			}

			FitToContent(onlyGrow: false);

			try
			{
				await LoadDrives(ShutdownToken);
				_hasCompletedInitialDriveLoad = true;

				if (ShutdownToken.IsCancellationRequested || IsDisposed || Disposing)
				{
					return;
				}

				if (_pathsToPreselect is { Count: > 0 } paths)
				{
					await PreSelectItemsAsync(paths, ShutdownToken);
				}

				if (ShutdownToken.IsCancellationRequested || IsDisposed || Disposing)
				{
					return;
				}

				ShowMissingSavedSelectionsWarning();
				UpdateDynamicPanels();
			}
			catch (OperationCanceledException) when (ShutdownToken.IsCancellationRequested || IsDisposed || Disposing)
			{
				// The form closed before startup work completed.
			}
			catch (Exception ex) when (!IsDisposed && !Disposing && !ShutdownToken.IsCancellationRequested)
			{
				ShowError($"Error loading drives: {ex.Message}", "Error");
			}
		}

		private void PopulateStaticCombos()
		{
			PopulateNumberCombo(cmbSelectedRetention, 1, MaxRetentionValue, 7);
			PopulateNumberCombo(cmbCloneRetention, 1, MaxRetentionValue, 7);

			cmbHour.Items.Clear();
			for (int i = 1; i <= 12; i++) cmbHour.Items.Add(i.ToString());
			cmbHour.SelectedIndex = 1;

			cmbMinute.Items.Clear();
			for (int i = 0; i < 60; i++) cmbMinute.Items.Add(i.ToString("D2"));
			cmbMinute.Text = "00";

			cmbDayOfMonth.Items.Clear();
			for (int i = 1; i <= 31; i++) cmbDayOfMonth.Items.Add(i.ToString());
			cmbDayOfMonth.SelectedIndex = 0;
		}

		private static void PopulateNumberCombo(ComboBox combo, int min, int max, int selected)
		{
			combo.Items.Clear();
			for (int i = min; i <= max; i++) combo.Items.Add(i.ToString());
			combo.Text = selected.ToString();
		}

		/// <summary>
		/// Shows or hides panels based on the current selections, then grows the form if the content no longer fits.
		/// </summary>
		private void UpdateDynamicPanels()
		{
			if (DesignMode) return;

			tlpRoot.SuspendLayout();
			try
			{
				bool cloneVirtualOrHyperV = rbCloneVirtual.Checked || rbCloneHyperV.Checked || rbExportHyperV.Checked;

				pnlFullRetention.Visible = rbFullBackup.Checked;
				pnlSelectedRetention.Visible = rbSelectedFiles.Checked;
				pnlCloneRetention.Visible = cloneVirtualOrHyperV;
				UpdateCloneRetentionText();

				pnlCloneDestination.Visible = rbCloneDisk.Checked;
				pnlDestination.Visible = !rbCloneDisk.Checked;

				pnlRenameHyperV.Visible = rbCloneHyperV.Checked;
				if (!rbCloneHyperV.Checked && chkRenameHyperVSystem.Checked)
				{
					chkRenameHyperVSystem.Checked = false;
				}
				pnlRenameHyperVName.Visible = rbCloneHyperV.Checked && chkRenameHyperVSystem.Checked;

				pnlEncryption.Visible = chkEncryptBackup.Checked;
				UpdateEncryptionUiState();

				pnlSchedule.Enabled = chkEnableSchedule.Checked;
				pnlWeekly.Visible = cmbFrequency.SelectedIndex == 1;
				pnlMonthly.Visible = cmbFrequency.SelectedIndex == 2;
			}
			finally
			{
				tlpRoot.ResumeLayout(true);
			}

			FitToContent(onlyGrow: true);
		}

		/// <summary>
		/// Sizes the form to its content. Only grows when onlyGrow is true, so a user's manual sizing is respected.
		/// </summary>
		private void FitToContent(bool onlyGrow)
		{
			if (DesignMode || IsDisposed || Disposing || WindowState != FormWindowState.Normal || !IsHandleCreated)
			{
				return;
			}

			Size chrome = Size - ClientSize;
			Size needed = new Size(tlpRoot.PreferredSize.Width + 24, tlpRoot.PreferredSize.Height + 24) + chrome;

			Rectangle workArea = Screen.FromControl(this).WorkingArea;
			int width = Math.Min(needed.Width, workArea.Width);
			int height = Math.Min(needed.Height, workArea.Height);

			Size target = onlyGrow
				? new Size(Math.Max(Width, width), Math.Max(Height, height))
				: new Size(width, height);

			Size = target;

			if (Top + Height > workArea.Bottom)
			{
				Top = Math.Max(workArea.Top, workArea.Bottom - Height);
			}
		}

		private bool IsFormClosingOrDisposed()
		{
			return IsDisposed || Disposing || ShutdownToken.IsCancellationRequested;
		}

		private void SetBusy(bool busy)
		{
			if (IsFormClosingOrDisposed())
			{
				return;
			}

			progressBar.Visible = busy;
			lblProgress.Visible = busy;
			pnlProgress.Visible = busy;
			btnStartBackup.Enabled = !busy;
			btnSaveJob.Enabled = !busy;
			UseWaitCursor = busy;
			if (!busy) progressBar.Value = 0;
			FitToContent(onlyGrow: true);
		}

		private void ReportProgress(int percentage, string? message)
		{
			if (IsFormClosingOrDisposed() || !IsHandleCreated) return;

			BeginInvoke(new Action(() =>
			{
				if (IsFormClosingOrDisposed() || !IsHandleCreated)
				{
					return;
				}

				progressBar.Value = Math.Clamp(percentage, 0, 100);
				lblProgress.Text = message ?? $"Progress: {percentage}%";
			}));
		}

		private void ReportStatus(string message)
		{
			if (IsFormClosingOrDisposed() || !IsHandleCreated) return;

			BeginInvoke(new Action(() =>
			{
				if (IsFormClosingOrDisposed() || !IsHandleCreated)
				{
					return;
				}

				lblProgress.Text = message;
			}));
		}

		// =====================================================================
		// Loading an existing job
		// =====================================================================

		private void LoadJobData(BackupJob job)
		{
			_isInitializingJobData = true;
			try
			{
				Text = $"Edit Backup - {job.Name}";
				_editingJob = job;

				txtBackupName.Text = job.Name;
				txtDestination.Text = job.DestinationPath;

				if (job.UserExclusions is { Count: > 0 })
				{
					btnManageExclusions.Text = $"Manage Exclusions... ({job.UserExclusions.Count})";
				}

				GetBackupTypeRadio(job.Type).Checked = true;

				chkCompress.Checked = job.CompressData;
				chkVerify.Checked = job.VerifyAfterBackup;
				txtRetainCount.Text = job.RetainFullBackupCount.ToString();
				cmbSelectedRetention.Text = Math.Clamp(job.SelectedFilesRetentionCount, 1, MaxRetentionValue).ToString();
				cmbCloneRetention.Text = Math.Clamp(job.CloneRetentionCount, 1, MaxRetentionValue).ToString();

				chkEncryptBackup.Checked = job.EncryptBackup;
				_hasSavedEncryptionPassword = job.EncryptBackup && !string.IsNullOrWhiteSpace(job.ProtectedEncryptionPassword);

				chkRenameHyperVSystem.Checked = job.RenameHyperVSystem;
				txtRenameHyperVSystemName.Text = job.RenameHyperVSystemName;

				if (_hasSavedEncryptionPassword)
				{
					txtEncryptionPassword.Text = "********";
					txtVerifyEncryptionPassword.Text = string.Empty;
				}

				IReadOnlyList<string> replayPaths = PersistenceHelpers.GetReplayPathsForJob(job);
				if (replayPaths.Count > 0)
				{
					_pathsToPreselect = new List<string>(replayPaths);
				}

				if (job.Schedule != null)
				{
					chkEnableSchedule.Checked = job.Schedule.Enabled;
					cmbFrequency.SelectedIndex = (int)job.Schedule.Frequency;

					int hour24 = job.Schedule.Time.Hours;
					int hour12 = hour24 % 12 == 0 ? 12 : hour24 % 12;
					cmbHour.Text = hour12.ToString();
					cmbMinute.Text = job.Schedule.Time.Minutes.ToString("D2");
					cmbAmPm.SelectedIndex = hour24 < 12 ? 0 : 1;

					if (job.Schedule.Frequency == ScheduleFrequency.Monthly)
					{
						cmbDayOfMonth.Text = job.Schedule.DayOfMonth.ToString();
					}
					else if (job.Schedule.Frequency == ScheduleFrequency.Weekly)
					{
						for (int i = 0; i < 7; i++)
						{
							DayCheckBoxes[i].Checked = job.Schedule.DaysOfWeek.Contains(DayOfWeekFor(i));
						}
					}
				}
			}
			finally
			{
				_isInitializingJobData = false;
			}

			UpdateDynamicPanels();
		}

		private RadioButton GetBackupTypeRadio(BackupType type) => type switch
		{
			BackupType.Incremental => rbIncremental,
			BackupType.Differential => rbDifferential,
			BackupType.SelectedFilesAndFolders => rbSelectedFiles,
			BackupType.CloneToDisk => rbCloneDisk,
			BackupType.CloneToVirtualDisk => rbCloneVirtual,
			BackupType.CloneHyperVSystem => rbCloneHyperV,
			BackupType.ExportHyperVSystem => rbExportHyperV,
			_ => rbFullBackup
		};

		private BackupType GetSelectedBackupType()
		{
			if (rbIncremental.Checked) return BackupType.Incremental;
			if (rbDifferential.Checked) return BackupType.Differential;
			if (rbSelectedFiles.Checked) return BackupType.SelectedFilesAndFolders;
			if (rbCloneDisk.Checked) return BackupType.CloneToDisk;
			if (rbCloneVirtual.Checked) return BackupType.CloneToVirtualDisk;
			if (rbCloneHyperV.Checked) return BackupType.CloneHyperVSystem;
			if (rbExportHyperV.Checked) return BackupType.ExportHyperVSystem;
			return BackupType.Full;
		}

		private string GetBackupTypeName()
		{
			if (rbFullBackup.Checked) return "Full Backup";
			if (rbIncremental.Checked) return "Incremental Backup";
			if (rbDifferential.Checked) return "Differential Backup";
			if (rbSelectedFiles.Checked) return "Selected Files & Folder";
			if (rbCloneDisk.Checked) return "Clone to Disk";
			if (rbCloneVirtual.Checked) return "Clone to Virtual Disk";
			if (rbCloneHyperV.Checked) return "Clone Hyper-V System";
			if (rbExportHyperV.Checked) return "Export Hyper-V System";
			return "This backup type";
		}

		// =====================================================================
		// Backup type / settings events
		// =====================================================================

		private void BackupType_CheckedChanged(object? sender, EventArgs e)
		{
			if (sender is RadioButton { Checked: false }) return;

			UpdateDynamicPanels();
			ApplyBackupTypeSelectionRestrictions();
			SyncNodeCheckStates(treeDrives.Nodes);

			if ((rbCloneHyperV.Checked || rbExportHyperV.Checked) && !_isInitializingJobData && _hasCompletedInitialDriveLoad)
			{
				_ = ReloadDriveTreeForBackupTypeAsync();
			}
		}

		private void RenameHyperVSystem_CheckedChanged(object? sender, EventArgs e) => UpdateDynamicPanels();

		private void UpdateCloneRetentionText()
		{
			bool isExport = rbExportHyperV.Checked;
			lblCloneRetHeader.Text = isExport ? "Export Retention:" : "Clone Retention:";
			lblCloneRetDesc.Text = isExport
				? "Keep the last X exports when scheduled or manual exports run. Older exports will be automatically deleted."
				: "Keep the last X clones when scheduled or manual clones run. Older clones will be automatically deleted.";
			lblCloneRetSuffix.Text = isExport ? "export(s)" : "clone(s)";
		}

		private async Task ReloadDriveTreeForBackupTypeAsync()
		{
			if (IsFormClosingOrDisposed())
			{
				return;
			}

			try
			{
				await LoadDrives();
			}
			catch (Exception ex)
			{
				if (!IsFormClosingOrDisposed())
				{
					ShowError($"Error reloading drives: {ex.Message}", "Error");
				}
			}
		}

		private void btnBrowseDestination_Click(object? sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select backup destination folder",
				UseDescriptionForTitle = true,
				ShowNewFolderButton = true
			};

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				txtDestination.Text = dialog.SelectedPath;
			}
		}

		private void btnBrowseClone_Click(object? sender, EventArgs e)
		{
			if (rbCloneDisk.Checked)
			{
				try
				{
					var diskDialog = new DiskSelectionNewForm(GetSelectedDiskIndexes());
					if (diskDialog.ShowDialog(this) == DialogResult.OK && diskDialog.SelectedDisk != null)
					{
						DiskSelectionNewForm.DiskInfo disk = diskDialog.SelectedDisk;
						txtCloneDestination.Text = $"Disk {disk.DiskIndex}: {disk.Model} ({FormatSize(disk.SizeBytes)})";
						txtCloneDestination.Tag = disk;
						hasTargetSelected = true;
						CheckAndShowVolumeConfiguration();
					}
				}
				catch (Exception ex)
				{
					ShowError($"Error selecting disk: {ex.Message}", "Error");
				}
			}
			else
			{
				using var dialog = new FolderBrowserDialog
				{
					Description = "Select folder for virtual disk clone",
					UseDescriptionForTitle = true,
					ShowNewFolderButton = true
				};

				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					txtCloneDestination.Text = dialog.SelectedPath;
					hasTargetSelected = true;
					CheckAndShowVolumeConfiguration();
				}
			}
		}

		private void btnManageExclusions_Click(object? sender, EventArgs e)
		{
			try
			{
				List<string> currentExclusions = _editingJob?.UserExclusions ?? new List<string>();
				ExclusionsManagementNewForm form = new ExclusionsManagementNewForm(currentExclusions);
				if (form.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				if (_editingJob != null)
				{
					_editingJob.UserExclusions = form.Exclusions;
				}
				else
				{
					_tempUserExclusions = form.Exclusions;
				}

				btnManageExclusions.Text = form.Exclusions.Count > 0
					? $"Manage Exclusions... ({form.Exclusions.Count})"
					: "Manage Exclusions...";
			}
			catch (Exception ex)
			{
				ShowError($"Error managing exclusions: {ex.Message}", "Error");
			}
		}

		private void chkShowHiddenPartitions_Click(object? sender, EventArgs e)
		{
			_showHiddenPartitions = chkShowHiddenPartitions.Checked;
			_ = ReloadDriveTreeForBackupTypeAsync();
		}

		private void btnRefresh_Click(object? sender, EventArgs e) => _ = ReloadDriveTreeForBackupTypeAsync();

		private void btnExpandAll_Click(object? sender, EventArgs e) => treeDrives.ExpandAll();

		private void btnCollapseAll_Click(object? sender, EventArgs e) => treeDrives.CollapseAll();

		private void EncryptBackup_CheckedChanged(object? sender, EventArgs e) => UpdateDynamicPanels();

		private void Schedule_CheckedChanged(object? sender, EventArgs e) => UpdateDynamicPanels();

		private void Frequency_SelectedIndexChanged(object? sender, EventArgs e) => UpdateDynamicPanels();

		private void UpdateEncryptionUiState()
		{
			bool enabled = chkEncryptBackup.Checked;
			bool locked = enabled && _hasSavedEncryptionPassword;

			txtEncryptionPassword.ReadOnly = locked;
			chkShowEncryptionPassword.Enabled = enabled;
			pnlVerifyPassword.Visible = enabled && !_hasSavedEncryptionPassword;

			if (!enabled)
			{
				chkShowEncryptionPassword.Checked = false;
				txtEncryptionPassword.Text = string.Empty;
				txtVerifyEncryptionPassword.Text = string.Empty;
			}
		}

		private void chkShowEncryptionPassword_CheckedChanged(object? sender, EventArgs e)
		{
			bool show = chkShowEncryptionPassword.Checked;

			if (_hasSavedEncryptionPassword)
			{
				if (show)
				{
					if (string.IsNullOrWhiteSpace(_decryptedEncryptionPassword) && _editingJob != null)
					{
						_decryptedEncryptionPassword = BackupEncryptionService.UnprotectPassword(_editingJob.ProtectedEncryptionPassword);
					}

					txtEncryptionPassword.Text = _decryptedEncryptionPassword ?? string.Empty;
				}
				else
				{
					txtEncryptionPassword.Text = "********";
				}
			}

			txtEncryptionPassword.UseSystemPasswordChar = !show;
		}

		private string GetEnteredEncryptionPassword()
		{
			if (_hasSavedEncryptionPassword && _editingJob != null)
			{
				return _editingJob.ProtectedEncryptionPassword;
			}

			return BackupEncryptionService.ProtectPassword(txtEncryptionPassword.Text);
		}

		// =====================================================================
		// Tree: building, syncing and events
		// =====================================================================

		private async Task LoadDrives(CancellationToken cancellationToken = default)
		{
			if (_isLoadingDrives)
			{
				_reloadDrivesAfterLoad = true;
				return;
			}

			if (cancellationToken.IsCancellationRequested || IsDisposed || Disposing)
			{
				return;
			}

			_isLoadingDrives = true;
			HashSet<string> checkedPaths = new(StringComparer.OrdinalIgnoreCase);
			GetCheckedItemsRecursive_FullPaths(driveItems, checkedPaths);

			SetLoading(true);
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				driveItems.Clear();

				await DriveEnumeration.LoadPhysicalDrives(driveItems, _showHiddenPartitions);   // copied from BackupWindowNew (see notes)
				cancellationToken.ThrowIfCancellationRequested();
				await HyperVHelpers.LoadHyperVSystems(driveItems);    // copied from BackupWindowNew (see notes)
				cancellationToken.ThrowIfCancellationRequested();
				await NetworkPathHelpers.LoadNetworkDrives(driveItems);    // copied from BackupWindowNew (see notes)
				cancellationToken.ThrowIfCancellationRequested();

				ApplyBackupTypeSelectionRestrictions();

				if (checkedPaths.Count > 0)
				{
					RestoreCheckedPaths(driveItems, checkedPaths);
				}

				RebuildTreeView();
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested || IsDisposed || Disposing)
			{
				// The form closed while drive loading was in progress.
			}
			catch (Exception ex) when (!IsDisposed && !Disposing && !cancellationToken.IsCancellationRequested)
			{
				ShowError($"Error loading drives: {ex.Message}", "Error");
			}
			finally
			{
				SetLoading(false);
				_isLoadingDrives = false;
			}

			if (_reloadDrivesAfterLoad)
			{
				_reloadDrivesAfterLoad = false;
				if (!cancellationToken.IsCancellationRequested && !IsDisposed && !Disposing)
				{
					await LoadDrives(cancellationToken);
				}
			}
		}

		private void SetLoading(bool loading)
		{
			if (IsDisposed || Disposing || ShutdownToken.IsCancellationRequested)
			{
				return;
			}

			lblLoading.Visible = loading;
			UseWaitCursor = loading;
			FitToContent(onlyGrow: true);
		}

		private void RebuildTreeView()
		{
			if (IsDisposed || Disposing || ShutdownToken.IsCancellationRequested)
			{
				return;
			}

			treeDrives.BeginUpdate();
			try
			{
				treeDrives.Nodes.Clear();
				foreach (DriveTreeItem item in driveItems)
				{
					treeDrives.Nodes.Add(CreateTreeNode(item));
				}

				SyncNodeCheckStates(treeDrives.Nodes);
			}
			finally
			{
				treeDrives.EndUpdate();
			}
		}

		private static TreeNode CreateTreeNode(DriveTreeItem item)
		{
			var node = new TreeNode(item.DisplayName ?? string.Empty)
			{
				Tag = item,
				ToolTipText = item.FullPath ?? string.Empty
			};

			foreach (DriveTreeItem child in item.Children)
			{
				node.Nodes.Add(CreateTreeNode(child));
			}

			return node;
		}

		private static void RebuildNodeChildren(TreeNode node, DriveTreeItem item)
		{
			node.Nodes.Clear();
			foreach (DriveTreeItem child in item.Children)
			{
				node.Nodes.Add(CreateTreeNode(child));
			}
		}

		/// <summary>
		/// Pushes the model's IsChecked state into the TreeNodes without raising AfterCheck.
		/// </summary>
		private void SyncNodeCheckStates(TreeNodeCollection nodes)
		{
			bool previous = _syncingTree;
			_syncingTree = true;
			try
			{
				SyncNodeCheckStatesCore(nodes);
			}
			finally
			{
				_syncingTree = previous;
			}
		}

		private static void SyncNodeCheckStatesCore(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Tag is DriveTreeItem item)
				{
					node.Checked = item.IsChecked == true;
					node.ForeColor = item.IsSelectionEnabled ? SystemColors.WindowText : SystemColors.GrayText;
				}

				SyncNodeCheckStatesCore(node.Nodes);
			}
		}

		private void treeDrives_BeforeCheck(object? sender, TreeViewCancelEventArgs e)
		{
			if (_syncingTree) return;

			if (e.Node?.Tag is not DriveTreeItem item ||
				item.IsLoadingPlaceholder ||
				!item.IsSelectionEnabled ||
				item.ItemType == DriveTreeItemType.NetworkRoot ||
				item.ItemType == DriveTreeItemType.NetworkBrowser)
			{
				e.Cancel = true;
			}
		}

		private void treeDrives_AfterCheck(object? sender, TreeViewEventArgs e)
		{
			if (_syncingTree || e.Node?.Tag is not DriveTreeItem item) return;

			// The model cascades the check to children and parents' states are refreshed from it.
			item.IsChecked = e.Node.Checked;
			for (DriveTreeItem? parent = item.Parent; parent != null; parent = parent.Parent)
			{
				parent.RefreshCheckStateFromChildren();
			}

			SyncNodeCheckStates(treeDrives.Nodes);

			if (item.ItemType is DriveTreeItemType.Volume or DriveTreeItemType.Disk)
			{
				if (e.Node.Checked)
				{
					hasSourceSelected = true;
					volumeConfigShown = false;
					CheckAndShowVolumeConfiguration();
				}
				else
				{
					hasSourceSelected = GetCheckedDriveItems()
						.Any(i => i.ItemType is DriveTreeItemType.Volume or DriveTreeItemType.Disk);
				}
			}
		}

		private void treeDrives_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node?.Tag is DriveTreeItem { ItemType: DriveTreeItemType.NetworkBrowser })
			{
				using var dialog = new NetworkPathNewForm();
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					NetworkPathHelpers.AddNetworkPathToTree(dialog.NetworkPath, driveItems, this, treeDrives);
				}
			}
		}

		private async void treeDrives_AfterExpand(object? sender, TreeViewEventArgs e)
		{
			if (e.Node?.Tag is not DriveTreeItem item || !TreeItemHelpers.RequiresLazyLoad(item)) return;

			try
			{
				if (item.ItemType == DriveTreeItemType.HyperVVirtualDisk)
				{
					await HyperVHelpers.LoadHyperVVirtualDiskChildrenAsync(item, item.DisplayName);   // copied from BackupWindowNew
					item.ChildrenLoaded = item.Children.Count > 0;

					if (item.Children.Count == 0)
					{
						e.Node.Collapse();
					}
				}
				else
				{
					List<DriveTreeItem> childItems = await Task.Run(() => VolumeConfigHelpers.BuildVolumeChildItems(item));
					PreSelectHelpers.ReplaceChildren(item, childItems);   // copied from BackupWindowNew
					item.ChildrenLoaded = true;
				}

				PreSelectHelpers.ApplyPendingSelectionState(item, _pendingSelectionPaths);      // copied from BackupWindowNew
				RebuildNodeChildren(e.Node, item);
				SyncNodeCheckStates(e.Node.Nodes);
			}
			catch (Exception ex)
			{
				ShowError($"Error expanding {item.DisplayName}: {ex.Message}", "Error");
			}
		}

		private void treeContextMenu_Opening(object? sender, CancelEventArgs e)
		{
			var item = treeDrives.SelectedNode?.Tag as DriveTreeItem;
			e.Cancel = item is not { IsRemovableNetworkPath: true };
		}

		private void miRemoveNetworkPath_Click(object? sender, EventArgs e)
		{
			if (treeDrives.SelectedNode?.Tag is not DriveTreeItem item ||
				item.Parent == null ||
				!item.IsRemovableNetworkPath)
			{
				return;
			}

			if (MessageBox.Show(this,
					$"Do you really want to delete this saved network path?\n\n{item.FullPath}",
					"Delete Network Path",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			item.Parent.Children.Remove(item);
		    NetworkPathHelpers.PersistCurrentNetworkPaths(item.Parent);   // copied from BackupWindowNew
			RebuildTreeView();
		}

		private void ApplyBackupTypeSelectionRestrictions()
		{
			bool hyperVSystemsOnly = rbCloneHyperV.Checked || rbExportHyperV.Checked;
			foreach (DriveTreeItem item in driveItems)
			{
				ApplySelectionRestrictionRecursive(item, hyperVSystemsOnly);
			}
		}

		private static void ApplySelectionRestrictionRecursive(DriveTreeItem item, bool hyperVSystemsOnly)
		{
			bool isHyperVSystem = item.ItemType == DriveTreeItemType.HyperVSystem;
			bool isSystemDisk = item.ItemType == DriveTreeItemType.Disk;
			bool isNetworkBrowser = item.ItemType == DriveTreeItemType.NetworkBrowser;

			item.IsSelectionEnabled = !hyperVSystemsOnly || isHyperVSystem || isSystemDisk || isNetworkBrowser;
			if (!item.IsSelectionEnabled)
			{
				item.IsChecked = false;
			}

			foreach (DriveTreeItem child in item.Children)
			{
				ApplySelectionRestrictionRecursive(child, hyperVSystemsOnly);
			}
		}

		private async void treeDrives_LazyLoadPlaceholder_Unused() { }   // remove: placeholder for diff purposes

		// =====================================================================
		// Selection helpers
		// =====================================================================

		private List<DriveTreeItem> GetCheckedDriveItems()
		{
			var checkedItems = new List<DriveTreeItem>();
			foreach (DriveTreeItem item in driveItems)
			{
				CollectChecked(item, checkedItems);
			}

			return checkedItems;
		}

		private static void CollectChecked(DriveTreeItem item, List<DriveTreeItem> checkedItems)
		{
			if (item.IsChecked == true)
			{
				checkedItems.Add(item);
			}

			foreach (DriveTreeItem child in item.Children)
			{
				CollectChecked(child, checkedItems);
			}
		}

		private static void GetCheckedItemsRecursive_FullPaths(IEnumerable<DriveTreeItem> items, HashSet<string> paths)
		{
			foreach (DriveTreeItem item in items)
			{
				if (item.IsChecked == true && !string.IsNullOrWhiteSpace(item.FullPath))
				{
					paths.Add(item.FullPath);
				}

				GetCheckedItemsRecursive_FullPaths(item.Children, paths);
			}
		}

		private static void RestoreCheckedPaths(IEnumerable<DriveTreeItem> items, HashSet<string> paths)
		{
			foreach (DriveTreeItem item in items)
			{
				if (!string.IsNullOrWhiteSpace(item.FullPath) && paths.Contains(item.FullPath))
				{
					item.IsChecked = true;
				}

				RestoreCheckedPaths(item.Children, paths);
			}
		}

		private static bool IsHyperVItem(DriveTreeItem item) =>
			item.ItemType is DriveTreeItemType.HyperVSystem or DriveTreeItemType.HyperVVirtualDisk or DriveTreeItemType.HyperVVolume;

		private static bool IsDescendantOfHyperVItem(DriveTreeItem item)
		{
			for (DriveTreeItem? parent = item.Parent; parent != null; parent = parent.Parent)
			{
				if (IsHyperVItem(parent)) return true;
			}

			return false;
		}

		private List<int> GetSelectedDiskIndexes()
		{
			var indexes = new List<int>();
			foreach (DriveTreeItem item in GetCheckedDriveItems())
			{
				if (item.ItemType == DriveTreeItemType.Disk &&
					int.TryParse(item.Name.Replace("Disk", "").Trim().Split(' ')[0], out int diskIndex))
				{
					indexes.Add(diskIndex);
				}
				else if (item.ItemType == DriveTreeItemType.Volume && item.Parent != null &&
						 int.TryParse(item.Parent.Name.Replace("Disk", "").Trim().Split(' ')[0], out int parentIndex) &&
						 !indexes.Contains(parentIndex))
				{
					indexes.Add(parentIndex);
				}
			}

			return indexes;
		}

		// =====================================================================
		// Pre-selection (editing a job)
		// =====================================================================

		private async Task PreSelectItemsAsync(List<string> pathsToSelect, CancellationToken cancellationToken = default)
		{
			if (cancellationToken.IsCancellationRequested || IsDisposed || Disposing)
			{
				return;
			}

			_missingSavedSelectionPaths.Clear();
			_pendingSelectionPaths = pathsToSelect
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Select(PreSelectHelpers.NormalizeSelectionPath)
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.ToHashSet(StringComparer.OrdinalIgnoreCase);

			foreach (string path in pathsToSelect)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (IsDisposed || Disposing)
				{
					return;
				}

				if (HyperVGuestSelectionPath.TryParse(path, out HyperVGuestSelectionInfo? guestSelection) && guestSelection != null)
				{
					await PreSelectHelpers.PreSelectHyperVGuestItemAsync(guestSelection, driveItems, _pendingSelectionPaths);   // copied from BackupWindowNew
				}
				else if (PreSelectHelpers.PreSelectHyperVSystemByName(path, driveItems))                 // copied from BackupWindowNew
				{
					// matched a saved Hyper-V VM name
				}
				else if (!await PreSelectHelpers.PreSelectStandardItemAsync(path, driveItems, _pendingSelectionPaths))            // copied from BackupWindowNew
				{
					_missingSavedSelectionPaths.Add(path);
				}
			}

			if (cancellationToken.IsCancellationRequested || IsDisposed || Disposing)
			{
				return;
			}

			PreSelectHelpers.RefreshLoadedSelectionStates(driveItems);                                  // copied from BackupWindowNew
			RebuildTreeView();
		}

		private void ShowMissingSavedSelectionsWarning()
		{
			if (IsFormClosingOrDisposed() || _editingJob == null || _missingSavedSelectionPaths.Count == 0) return;

			List<string> missing = _missingSavedSelectionPaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
				.ToList();

			_missingSavedSelectionPaths.Clear();
			if (missing.Count == 0) return;

			CustomDialogService.ShowWarning(BuildMissingSavedSelectionsWarningMessage(_editingJob.Type, missing),
				"Missing Saved Selections");
		}

		private void ShowWarning(string message, string title)
		{
			if (IsFormClosingOrDisposed())
			{
				return;
			}

			MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void ShowError(string message, string title)
		{
			if (IsFormClosingOrDisposed())
			{
				return;
			}

			MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void ShowSuccess(string message, string title)
		{
			if (IsFormClosingOrDisposed())
			{
				return;
			}

			MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		private static string BuildMissingSavedSelectionsWarningMessage(BackupType backupType, IReadOnlyCollection<string> missingSelections)
		{
			ArgumentNullException.ThrowIfNull(missingSelections);

			string joinedSelections = string.Join(Environment.NewLine, missingSelections.Where(selection => !string.IsNullOrWhiteSpace(selection)));
			bool preserveRemainingSelections = backupType == BackupType.SelectedFilesAndFolders;

			string prefix = preserveRemainingSelections
				? "The following saved selections were removed from the current selection list:"
				: "The following saved selections were missing, so the current selection list was cleared:";

			return string.IsNullOrWhiteSpace(joinedSelections)
				? prefix
				: prefix + Environment.NewLine + joinedSelections;
		}
		// =====================================================================
		// Volume configuration (clone flows)
		// =====================================================================

		private void CheckAndShowVolumeConfiguration()
		{
			try
			{
				bool isClone = rbCloneDisk.Checked || rbCloneVirtual.Checked;
				if (!isClone || !hasSourceSelected || !hasTargetSelected || volumeConfigShown) return;

				volumeConfigShown = true;

				List<VolumeInfo> selectedVolumes = VolumeConfigHelpers.GetSelectedVolumesForVolumeConfig(driveItems);   // copied from BackupWindowNew
				if (selectedVolumes.Count == 0)
				{
					ShowWarning("Please select at least one volume to clone.", "No Source Selected");
					volumeConfigShown = false;
					return;
				}

				long targetDiskSize = VolumeConfigHelpers.GetTargetDiskSize(rbCloneDisk, rbCloneVirtual, txtCloneDestination);                                 // copied from BackupWindowNew
				if (targetDiskSize <= 0)
				{
					ShowError("Unable to determine target disk size.", "Invalid Target");
					volumeConfigShown = false;
					return;
				}

				int sourceAus = VolumeConfigHelpers.GetAllocationUnitSize(selectedVolumes[0].FileSystem);
				int targetAus = VolumeConfigHelpers.GetTargetAllocationUnitSize(rbCloneDisk, rbCloneVirtual, txtCloneDestination, txtDestination);

				var configWindow = new VolumeConfigurationNewForm(selectedVolumes, targetDiskSize, sourceAus, targetAus);
				if (configWindow.ShowDialog() != DialogResult.OK)
				{
					hasTargetSelected = false;
					txtCloneDestination.Text = string.Empty;
					volumeConfigShown = false;
				}
			}
			catch (Exception ex)
			{
				volumeConfigShown = false;
				ShowError($"Error showing volume configuration: {ex.Message}", "Error");
			}
		}

		// =====================================================================
		// Validation and job creation
		// =====================================================================

		private bool ValidateInputs()
		{
			if (string.IsNullOrWhiteSpace(txtBackupName.Text))
			{
				ShowWarning("Please enter a backup name.", "Validation Error");
				return false;
			}

			if (rbCloneDisk.Checked)
			{
				if (string.IsNullOrWhiteSpace(txtCloneDestination.Text))
				{
					ShowWarning("Please select a physical disk destination.", "Validation Error");
					return false;
				}
			}
			else if (string.IsNullOrWhiteSpace(txtDestination.Text))
			{
				ShowWarning("Please select a backup destination.", "Validation Error");
				return false;
			}

			List<DriveTreeItem> selectedItems = GetCheckedDriveItems();
			List<DriveTreeItem> selectedHyperV = selectedItems.Where(IsHyperVItem).ToList();
			List<DriveTreeItem> selectedNonHyperV = selectedItems.Where(item =>
				!IsHyperVItem(item) &&
				!IsDescendantOfHyperVItem(item) &&
				item.ItemType != DriveTreeItemType.NetworkRoot &&
				item.ItemType != DriveTreeItemType.NetworkBrowser).ToList();
			List<DriveTreeItem> selectedFiles = rbSelectedFiles.Checked
				? FolderPickerHelper.GetEffectiveSelectedFilesAndFoldersItems(driveItems)
				: new List<DriveTreeItem>();

			if (rbCloneHyperV.Checked || rbExportHyperV.Checked)
			{
				string action = rbExportHyperV.Checked ? "export" : "clone";
				if (selectedHyperV.Count == 0)
				{
					ShowWarning($"Please select at least one Hyper-V system to {action}.", "Validation Error");
					return false;
				}

				if (selectedNonHyperV.Count > 0)
				{
					ShowWarning($"{GetBackupTypeName()} can only use Hyper-V systems.\n\nPlease unselect disks, volumes, and folders.",
						"Validation Error");
					return false;
				}

				if (chkRenameHyperVSystem.Checked)
				{
					string newName = txtRenameHyperVSystemName.Text.Trim();
					if (string.IsNullOrWhiteSpace(newName))
					{
						ShowWarning("A new name is required when Rename Hyper-V System is selected.", "Validation Error");
						return false;
					}

					if (!IsValidWindowsComputerName(newName))
					{
						ShowWarning("The new Hyper-V system name must be a valid Windows computer name (1-15 letters, numbers, or hyphens, and cannot start or end with a hyphen).",
							"Validation Error");
						return false;
					}
				}
			}
			else
			{
				if (rbSelectedFiles.Checked &&
					!FolderPickerHelper.IsSelectedFilesAndFoldersSelectionAllowed(selectedFiles))
				{
					ShowWarning("Selected Files & Folder backups can only include checked files, folders, or network share roots from the tree.",
						"Validation Error");
					return false;
				}

				if (selectedHyperV.Count > 0 && selectedNonHyperV.Count > 0)
				{
					ShowWarning($"{GetBackupTypeName()} cannot combine Hyper-V systems with disks, volumes, or folders.\n\nPlease select only Hyper-V systems or only disks, volumes, and folders.",
						"Validation Error");
					return false;
				}

				string? selectionMessage = GetSelectionValidationMessage(
					rbSelectedFiles.Checked,
					selectedFiles.Count,
					selectedHyperV.Count,
					selectedNonHyperV.Count);
				if (!string.IsNullOrWhiteSpace(selectionMessage))
				{
					ShowWarning(selectionMessage, "Validation Error");
					return false;
				}
			}

			if (chkEnableSchedule.Checked && !TryGetScheduledTime(out _, out _))
			{
				ShowWarning("Please enter a valid scheduled time. Minutes must be between 00 and 59.", "Validation Error");
				return false;
			}

			if (rbSelectedFiles.Checked && !IsRetentionValid(cmbSelectedRetention.Text))
			{
				ShowWarning("Please enter a Selected Files retention value between 1 and 30 versions.", "Validation Error");
				return false;
			}

			if ((rbCloneVirtual.Checked || rbCloneHyperV.Checked || rbExportHyperV.Checked) && !IsRetentionValid(cmbCloneRetention.Text))
			{
				ShowWarning("Please enter a clone/export retention value between 1 and 30 items.", "Validation Error");
				return false;
			}

			if (chkEncryptBackup.Checked && !_hasSavedEncryptionPassword)
			{
				if (string.IsNullOrWhiteSpace(txtEncryptionPassword.Text))
				{
					ShowWarning("Please enter a password for backup encryption.", "Validation Error");
					return false;
				}

				if (txtEncryptionPassword.Text != txtVerifyEncryptionPassword.Text)
				{
					ShowWarning("The encryption passwords do not match.", "Validation Error");
					return false;
				}
			}

			return true;
		}
		internal static string? GetSelectionValidationMessage(bool selectedFilesBackup, int selectedFilesCount, int selectedHyperVCount, int selectedNonHyperVCount)
		{
			if (selectedFilesBackup)
			{
				return selectedFilesCount == 0
					? "Please select at least one file, folder, or network share to back up."
					: null;
			}

			return selectedHyperVCount == 0 && selectedNonHyperVCount == 0
				? "Please select at least one drive, volume, folder, or Hyper-V system to backup."
				: null;
		}

		private static bool IsRetentionValid(string? text) =>
			int.TryParse(text?.Trim(), out int value) && value >= 1 && value <= MaxRetentionValue;

		private BackupJob CreateJobFromInput()
		{
			BackupType backupType = GetSelectedBackupType();
			bool renameHyperVSystem = backupType == BackupType.CloneHyperVSystem && chkRenameHyperVSystem.Checked;

			var job = new BackupJob
			{
				Id = Guid.NewGuid(),
				Name = txtBackupName.Text,
				Type = backupType,
				DestinationPath = rbCloneDisk.Checked ? txtCloneDestination.Text : txtDestination.Text,
				CompressData = chkCompress.Checked,
				VerifyAfterBackup = chkVerify.Checked,
				EncryptBackup = chkEncryptBackup.Checked,
				ProtectedEncryptionPassword = chkEncryptBackup.Checked ? GetEnteredEncryptionPassword() : string.Empty,
				RetainFullBackupCount = int.TryParse(txtRetainCount.Text, out int retain) ? Math.Max(1, retain) : 1,
				SelectedFilesRetentionCount = int.TryParse(cmbSelectedRetention.Text, out int selRet) ? Math.Clamp(selRet, 1, MaxRetentionValue) : 7,
				CloneRetentionCount = int.TryParse(cmbCloneRetention.Text, out int cloneRet) ? Math.Clamp(cloneRet, 1, MaxRetentionValue) : 7,
				RenameHyperVSystem = renameHyperVSystem,
				RenameHyperVSystemName = renameHyperVSystem ? txtRenameHyperVSystemName.Text.Trim() : string.Empty
			};

			if (backupType is BackupType.CloneHyperVSystem or BackupType.ExportHyperVSystem)
			{
				job.IsHyperVBackup = true;
			    SelectionHelpers.CollectSelectedHyperVMachines(job, driveItems);   // copied from BackupWindowNew
			}
			else
			{
				SelectionHelpers.CollectSelectedItems(job, driveItems);            // copied from BackupWindowNew
			}

			if (backupType == BackupType.SelectedFilesAndFolders)
			{
				job.SelectedFilesSourceRoots = SelectionHelpers.GetSelectedFilesSourceRoots(driveItems);   // copied from BackupWindowNew
			}

			if (chkEnableSchedule.Checked)
			{
				if (!TryGetScheduledTime(out int hour24, out int minute))
				{
					throw new InvalidOperationException("The scheduled time is invalid.");
				}

				job.Schedule = new BackupSchedule
				{
					JobId = job.Id,
					Enabled = true,
					Frequency = (ScheduleFrequency)cmbFrequency.SelectedIndex,
					Time = new TimeSpan(hour24, minute, 0)
				};

				if (job.Schedule.Frequency == ScheduleFrequency.Weekly)
				{
					for (int i = 0; i < 7; i++)
					{
						if (DayCheckBoxes[i].Checked)
						{
							job.Schedule.DaysOfWeek.Add(DayOfWeekFor(i));
						}
					}
				}
				else if (job.Schedule.Frequency == ScheduleFrequency.Monthly)
				{
					job.Schedule.DayOfMonth = int.Parse(cmbDayOfMonth.Text);
				}
			}

			if (_editingJob?.UserExclusions != null)
			{
				job.UserExclusions = new List<string>(_editingJob.UserExclusions);
			}
			else if (_tempUserExclusions != null)
			{
				job.UserExclusions = new List<string>(_tempUserExclusions);
			}
			else
			{
				job.UserExclusions = new List<string>();
			}

			return job;
		}

		private CheckBox[] DayCheckBoxes => new[]
		{
			chkMonday, chkTuesday, chkWednesday, chkThursday, chkFriday, chkSaturday, chkSunday
		};

		/// <summary>Index 0 = Monday ... index 6 = Sunday.</summary>
		private static DayOfWeek DayOfWeekFor(int index) => (DayOfWeek)((index + 1) % 7);

		private bool TryGetScheduledTime(out int hour24, out int minute)
		{
			hour24 = 0;
			minute = 0;

			if (!int.TryParse(cmbHour.Text, out int hour12) || hour12 < 1 || hour12 > 12) return false;
			if (!int.TryParse(cmbMinute.Text, out minute) || minute < 0 || minute > 59) return false;

			bool am = cmbAmPm.Text != "PM";
			hour24 = am ? (hour12 == 12 ? 0 : hour12) : (hour12 == 12 ? 12 : hour12 + 12);
			cmbMinute.Text = minute.ToString("D2");
			return true;
		}

		// =====================================================================
		// Buttons
		// =====================================================================

		private async void btnStartBackup_Click(object? sender, EventArgs e)
		{
			if (!ValidateInputs()) return;

			SetBusy(true);
			try
			{
				BackupJob job = CreateJobFromInput();
				await Task.Run(() => RunBackupJobHelper.RunBackupJobAsync(job, this));

				ShowSuccess("Backup completed successfully!", "Success");
				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				ShowError($"Backup failed: {ex.Message}", "Error");
			}
			finally
			{
				SetBusy(false);
			}
		}

		private void btnSaveJob_Click(object? sender, EventArgs e)
		{
			if (!ValidateInputs()) return;

			try
			{
				BackupJob job = CreateJobFromInput();

				if (existingJob != null)
				{
					job.Id = existingJob.Id;
					jobManager.UpdateJob(job);
				    PersistenceHelpers.PersistSelectedFileList(existingJob.Name, job);   // copied from BackupWindowNew
					ShowSuccess($"Backup job '{job.Name}' updated successfully!\n\nJob saved to:\nC:\\ProgramData\\SecureServerBackupService\\jobs.json", "Success");
				}
				else
				{
					jobManager.AddJob(job);
					PersistenceHelpers.PersistSelectedFileList(null, job);
					ShowSuccess($"Backup job '{job.Name}' created successfully!\n\nJob saved to:\nC:\\ProgramData\\SecureServerBackupService\\jobs.json", "Success");
				}

				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				ShowError($"ERROR: Failed to save backup job!\n\n{ex.Message}\n\nPlease check:\n" +
						  "1. You have administrator rights\n" +
						  "2. C:\\ProgramData folder is accessible\n" +
						  "3. Antivirus is not blocking the save\n\n" +
						  $"Technical details:\n{ex.InnerException?.Message}", "Save Failed");
			}
		}

		private void btnCancel_Click(object? sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		// =====================================================================
		// Small UI helpers
		// =====================================================================

		/// <summary>
		/// Shows a WPF dialog modally, owned by this WinForms form.
		/// </summary>
		private bool? ShowWpfDialog(System.Windows.Window window)
		{
			_ = new System.Windows.Interop.WindowInteropHelper(window) { Owner = Handle };
			return window.ShowDialog();
		}

		private static string FormatSize(long bytes)
		{
			string[] sizes = { "B", "KB", "MB", "GB", "TB" };
			double len = bytes;
			int order = 0;
			while (len >= 1024 && order < sizes.Length - 1)
			{
				order++;
				len /= 1024;
			}

			return $"{len:0.##} {sizes[order]}";
		}
		internal static bool IsValidWindowsComputerName(string? computerName)
		{
			if (string.IsNullOrWhiteSpace(computerName))
			{
				return false;
			}

			string trimmedName = computerName.Trim();
			if (trimmedName.Length is < 1 or > 15)
			{
				return false;
			}

			if (trimmedName.StartsWith("-", StringComparison.Ordinal) ||
				trimmedName.EndsWith("-", StringComparison.Ordinal))
			{
				return false;
			}

			foreach (char ch in trimmedName)
			{
				if (!(char.IsLetterOrDigit(ch) || ch == '-'))
				{
					return false;
				}
			}

			return true;
		}

		// These route through one place so the rest of the port reads like the WPF original.
		// Swap the bodies for CustomDialogService if it is usable from WinForms.
	}
}
