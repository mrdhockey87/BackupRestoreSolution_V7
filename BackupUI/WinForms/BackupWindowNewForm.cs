using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;
using Svg;
using SecureServerBackup.Services;
using SecureServerBackup.Windows;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	public sealed partial class BackupWindowNewForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly BackupJob? existingJob;
		private readonly JobManager jobManager = new();
		private readonly List<string> nativeSourcePaths = new();
		private readonly List<string> nativeUserExclusions = new();
		private readonly HashSet<TreeNode> loadingVolumeNodes = [];

		private BackupJob? currentJob;
		private bool hasSavedEncryptionPassword;
		private string savedProtectedPassword = string.Empty;
		private bool suppressPasswordSync;
		private bool suppressTreeCheckSync;
		private int volumeAnimationFrame;
		private Control[] settingsWidthControls = [];
		private const int VolumeAnimationFrameCount = 6;

		private enum SourceTreeNodeKind
		{
			Disk,
			Volume,
			Directory,
			File,
			Partition
		}

		private sealed class SourceTreeNodeData
		{
			public required SourceTreeNodeKind Kind { get; init; }
			public int DiskNumber { get; init; }
			public string SelectionPath { get; init; } = string.Empty;
			public string FileSystemPath { get; init; } = string.Empty;
		}

		private sealed class FileSystemNodeEntry
		{
			public required SourceTreeNodeKind Kind { get; init; }
			public required string Text { get; init; }
			public required string SelectionPath { get; init; }
			public required string FileSystemPath { get; init; }
		}

		public BackupWindowNewForm()
			: this(null)
		{
		}

		public BackupWindowNewForm(BackupJob? job)
		{
			existingJob = job;
			currentJob = job;
			InitializeComponent();

			Text = job == null ? "Create Backup" : $"Edit Backup - {job.Name}";
			headerLabel.Text = Text;

			settingsWidthControls = [basicGroup, retentionPanel, selectedFilesRetentionPanel, cloneRetentionPanel, exclusionsGroup, encryptionGroup, scheduleGroup];
			selectedFilesRetentionComboBox.Items.Clear();
			cloneRetentionComboBox.Items.Clear();
			for (int i = 1; i <= 30; i++)
			{
				selectedFilesRetentionComboBox.Items.Add(i.ToString());
				cloneRetentionComboBox.Items.Add(i.ToString());
			}

			selectedFilesRetentionComboBox.Text = "7";
			cloneRetentionComboBox.Text = "7";
			retainCountTextBox.Text = "1";

			InitializeDriveTreeResources();
			ResizeSettingsWidth();

			InitializeScheduleControls();

			if (IsInDesignMode)
			{
				ApplyDesignTimeState();
				return;
			}

			PopulateDriveTree();
			LoadExistingJob();
			UpdateBackupTypeUi();
			UpdateEncryptionUi();
			UpdateScheduleUi();
			RefreshNativeSourceListBox();
			UpdateAdvancedStateSummary();
		}

		private void InitializeDriveTreeResources()
		{
			driveImageList.Images.Clear();

			try
			{
				driveImageList.Images.Add("drive", LoadTreeBitmapFromSvg("Assets\\hard_drive.svg", 16, 16) ?? SystemIcons.WinLogo.ToBitmap());
				driveImageList.Images.Add("folder", LoadTreeBitmapFromSvg("Assets\\folder_color.svg", 16, 16) ?? SystemIcons.Application.ToBitmap());
				driveImageList.Images.Add("file", LoadTreeBitmapFromSvg("Assets\\File_color.svg", 16, 16) ?? SystemIcons.Information.ToBitmap());

				for (int frame = 0; frame < VolumeAnimationFrameCount; frame++)
				{
					driveImageList.Images.Add(GetVolumeAnimationImageKey(frame), CreateVolumeProgressBitmap(frame, VolumeAnimationFrameCount, driveImageList.ImageSize));
				}
			}
			catch
			{
			}
		}

		private void ResizeSettingsWidth()
		{
			int width = Math.Max(320, settingsScroll.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 6);
			settingsStack.Width = width;

			foreach (Control control in settingsWidthControls)
			{
				control.Width = width;
			}
		}

		private void BackupWindowNewForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			volumeAnimationTimer.Stop();
		}

		private void BackupTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			UpdateBackupTypeUi();
		}

		private void RefreshDriveTreeButton_Click(object? sender, EventArgs e)
		{
			PopulateDriveTree();
		}

		private void ExpandTreeButton_Click(object? sender, EventArgs e)
		{
			driveTree.ExpandAll();
		}

		private void CollapseTreeButton_Click(object? sender, EventArgs e)
		{
			driveTree.CollapseAll();
		}

		private void ShowHiddenPartitionsCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			PopulateDriveTree();
		}

		private void NativeSourceListBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			UpdateNativeSourceUi();
		}

		private void EncryptCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			UpdateEncryptionUi();
		}

		private void ShowPasswordCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			UpdatePasswordVisibility();
		}

		private void EnableScheduleCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			UpdateScheduleUi();
		}

		private void FrequencyComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			UpdateScheduleFrequencyUi();
		}

		private void SettingsScroll_Resize(object? sender, EventArgs e)
		{
			ResizeSettingsWidth();
		}

		private void ApplyDesignTimeState()
		{
			backupNameTextBox.Text = "Sample Backup Job";
			destinationTextBox.Text = @"D:\Backups";
			backupTypeComboBox.SelectedIndex = 0;
			encryptCheckBox.Checked = true;
			encryptionPasswordTextBox.Text = "password";
			verifyEncryptionPasswordTextBox.Text = "password";
			enableScheduleCheckBox.Checked = true;
			nativeSourcePaths.Clear();
			nativeSourcePaths.Add(@"C:\Users\Admin\Documents");
			nativeSourcePaths.Add(@"D:\Projects");
			RefreshNativeSourceListBox();

			driveTree.Nodes.Clear();
			var diskNode = new TreeNode("Disk 0 - Sample SSD (512 GB)");
			diskNode.Nodes.Add(new TreeNode(@"C:\ (NTFS, 240 GB)"));
			diskNode.Nodes.Add(new TreeNode(@"D:\ (NTFS, 180 GB)"));
			driveTree.Nodes.Add(diskNode);
			driveTree.ExpandAll();

			UpdateBackupTypeUi();
			UpdateEncryptionUi();
			UpdateScheduleUi();
			UpdateAdvancedStateSummary();
		}

		private static GroupBox CreateGroupBox(string title)
		{
			return new GroupBox
			{
				Text = title,
				Dock = DockStyle.Top,
				AutoSize = true,
				Margin = new Padding(0, 0, 0, 12),
				BackColor = Color.FromArgb(223, 242, 242),
				Padding = new Padding(6)
			};
		}

		private void AddBackupTypeRadioButton(TableLayoutPanel panel, int column, int row, string text, int comboIndex)
		{
			ArgumentNullException.ThrowIfNull(panel);

			while (panel.RowStyles.Count <= row)
			{
				panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}

			RadioButton radioButton = new()
			{
				Text = text,
				AutoSize = true,
				Margin = new Padding(0, 2, 8, 4),
				Anchor = AnchorStyles.Left
			};

			radioButton.CheckedChanged += (_, _) =>
			{
				if (radioButton.Checked && backupTypeComboBox.SelectedIndex != comboIndex)
				{
					backupTypeComboBox.SelectedIndex = comboIndex;
				}
			};

			backupTypeComboBox.SelectedIndexChanged += (_, _) =>
			{
				if (backupTypeComboBox.SelectedIndex == comboIndex && !radioButton.Checked)
				{
					radioButton.Checked = true;
				}
			};

			panel.Controls.Add(radioButton, column, row);
		}

		private static TableLayoutPanel CreateDetailsLayout()
		{
			var panel = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				Padding = new Padding(8),
				ColumnCount = 3
			};
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			return panel;
		}

		private static TextBox AddLabeledTextBox(TableLayoutPanel panel, int row, string label)
		{
			EnsureRow(panel, row);
			panel.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 4, 8, 4) }, 0, row);
			var textBox = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 2, 4, 2) };
			panel.Controls.Add(textBox, 1, row);
			return textBox;
		}

		private static ComboBox AddLabeledComboBox(TableLayoutPanel panel, int row, string label)
		{
			EnsureRow(panel, row);
			panel.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 4, 8, 4) }, 0, row);
			var comboBox = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 2, 4, 2) };
			panel.Controls.Add(comboBox, 1, row);
			return comboBox;
		}

		private static CheckBox AddLabeledCheckBox(TableLayoutPanel panel, int row, string text, bool isChecked)
		{
			EnsureRow(panel, row);
			panel.Controls.Add(new Label { AutoSize = true }, 0, row);
			var checkBox = new CheckBox { Text = text, AutoSize = true, Checked = isChecked, Margin = new Padding(0, 4, 4, 4) };
			panel.Controls.Add(checkBox, 1, row);
			return checkBox;
		}

		private static void EnsureRow(TableLayoutPanel panel, int row)
		{
			while (panel.RowStyles.Count <= row)
			{
				panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}

			panel.RowCount = Math.Max(panel.RowCount, row + 1);
		}

		private static Panel CreateRetentionPanel(string title, string prefix, out TextBox textBox, string suffix)
		{
			var panel = new Panel { Dock = DockStyle.Top, AutoSize = true, Margin = new Padding(0, 0, 0, 12) };
			var group = CreateGroupBox(title);
			var layout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				AutoSize = true,
				Padding = new Padding(10)
			};
			layout.Controls.Add(new Label { Text = prefix, AutoSize = true, Margin = new Padding(0, 8, 6, 0) });
			textBox = new TextBox { Width = 60, TextAlign = HorizontalAlignment.Center };
			layout.Controls.Add(textBox);
			layout.Controls.Add(new Label { Text = suffix, AutoSize = true, Margin = new Padding(6, 8, 0, 0) });
			group.Controls.Add(layout);
			panel.Controls.Add(group);
			return panel;
		}

		private static Panel CreateRetentionPanel(string title, string prefix, out ComboBox comboBox, string suffix)
		{
			var panel = new Panel { Dock = DockStyle.Top, AutoSize = true, Margin = new Padding(0, 0, 0, 12) };
			var group = CreateGroupBox(title);
			var layout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				AutoSize = true,
				Padding = new Padding(10)
			};
			layout.Controls.Add(new Label { Text = prefix, AutoSize = true, Margin = new Padding(0, 8, 6, 0) });
			comboBox = new ComboBox { Width = 80, DropDownStyle = ComboBoxStyle.DropDown, Text = "7" };
			layout.Controls.Add(comboBox);
			layout.Controls.Add(new Label { Text = suffix, AutoSize = true, Margin = new Padding(6, 8, 0, 0) });
			group.Controls.Add(layout);
			panel.Controls.Add(group);
			return panel;
		}

		private static Label CreateActionInfoBox()
		{
			return new Label
			{
				Text = "Advanced migration status\n\n- Native: common settings, encryption, schedule, retention\n- Legacy advanced editor: source tree, Hyper-V, clone/export, immediate execution",
				AutoSize = true,
				MaximumSize = new Size(220, 0),
				BorderStyle = BorderStyle.FixedSingle,
				Padding = new Padding(10),
				Margin = new Padding(0, 0, 0, 10)
			};
		}

		private void InitializeScheduleControls()
		{
			for (int i = 1; i <= 12; i++)
			{
				hourComboBox.Items.Add(i.ToString());
			}
			hourComboBox.SelectedIndex = 1;

			for (int i = 0; i < 60; i++)
			{
				minuteComboBox.Items.Add(i.ToString("D2"));
			}
			minuteComboBox.Text = "00";

			amPmComboBox.Items.AddRange(new object[] { "AM", "PM" });
			amPmComboBox.SelectedIndex = 0;

			for (int i = 1; i <= 31; i++)
			{
				dayOfMonthComboBox.Items.Add(i.ToString());
			}
			dayOfMonthComboBox.SelectedIndex = 0;
			frequencyComboBox.SelectedIndex = 0;
		}

		private void LoadExistingJob()
		{
			BackupJob? job = currentJob ?? existingJob;
			if (job == null)
			{
				nativeSourcePaths.Clear();
				nativeUserExclusions.Clear();
				UpdateExclusionsButtonText();
				RefreshNativeSourceListBox();
				backupTypeComboBox.SelectedIndex = 0;
				return;
			}

			backupNameTextBox.Text = job.Name;
			destinationTextBox.Text = job.DestinationPath;
			compressCheckBox.Checked = job.CompressData;
			verifyCheckBox.Checked = job.VerifyAfterBackup;
			encryptCheckBox.Checked = job.EncryptBackup;
			hasSavedEncryptionPassword = job.EncryptBackup && !string.IsNullOrWhiteSpace(job.ProtectedEncryptionPassword);
			savedProtectedPassword = job.ProtectedEncryptionPassword;

			if (hasSavedEncryptionPassword)
			{
				encryptionPasswordTextBox.Text = "********";
				verifyEncryptionPasswordTextBox.Text = "********";
			}
			else
			{
				encryptionPasswordTextBox.Clear();
				verifyEncryptionPasswordTextBox.Clear();
			}

			backupTypeComboBox.SelectedIndex = job.Type switch
			{
				BackupType.Full => 0,
				BackupType.Incremental => 1,
				BackupType.Differential => 2,
				BackupType.SelectedFilesAndFolders => 3,
				BackupType.CloneToDisk => 4,
				BackupType.CloneToVirtualDisk => 5,
				BackupType.CloneHyperVSystem => 6,
				BackupType.ExportHyperVSystem => 7,
				_ => 0
			};

			retainCountTextBox.Text = Math.Max(1, job.RetainFullBackupCount).ToString();
			selectedFilesRetentionComboBox.Text = Math.Clamp(job.SelectedFilesRetentionCount, 1, 30).ToString();
			cloneRetentionComboBox.Text = Math.Clamp(job.CloneRetentionCount, 1, 30).ToString();
			LoadNativeSourcePaths(job);
			LoadNativeUserExclusions(job);

			for (int i = 0; i < weeklyDaysCheckedListBox.Items.Count; i++)
			{
				weeklyDaysCheckedListBox.SetItemChecked(i, false);
			}

			if (job.Schedule != null && job.Schedule.Enabled)
			{
				enableScheduleCheckBox.Checked = true;
				frequencyComboBox.SelectedIndex = (int)job.Schedule.Frequency;

				int hour24 = job.Schedule.Time.Hours;
				int hour12 = hour24 % 12;
				if (hour12 == 0)
				{
					hour12 = 12;
				}

				hourComboBox.Text = hour12.ToString();
				minuteComboBox.Text = job.Schedule.Time.Minutes.ToString("D2");
				amPmComboBox.SelectedItem = hour24 >= 12 ? "PM" : "AM";

				if (job.Schedule.Frequency == ScheduleFrequency.Weekly)
				{
					for (int i = 0; i < weeklyDaysCheckedListBox.Items.Count; i++)
					{
						DayOfWeek day = (DayOfWeek)(((i + 1) % 7));
						if (job.Schedule.DaysOfWeek.Contains(day))
						{
							weeklyDaysCheckedListBox.SetItemChecked(i, true);
						}
					}
				}
				else if (job.Schedule.Frequency == ScheduleFrequency.Monthly)
				{
					dayOfMonthComboBox.Text = Math.Max(1, job.Schedule.DayOfMonth).ToString();
				}
			}
			else
			{
				enableScheduleCheckBox.Checked = false;
				frequencyComboBox.SelectedIndex = 0;
				hourComboBox.SelectedIndex = 1;
				minuteComboBox.Text = "00";
				amPmComboBox.SelectedIndex = 0;
				dayOfMonthComboBox.SelectedIndex = 0;
			}
		}

		private void LoadNativeUserExclusions(BackupJob job)
		{
			nativeUserExclusions.Clear();
			if (job.UserExclusions != null)
			{
				nativeUserExclusions.AddRange(job.UserExclusions
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase));
			}

			UpdateExclusionsButtonText();
		}

		private void UpdateExclusionsButtonText()
		{
			manageExclusionsButton.Text = nativeUserExclusions.Count > 0
				? $"Manage Exclusions... ({nativeUserExclusions.Count})"
				: "Manage Exclusions...";
		}

		private void ManageExclusions_Click(object? sender, EventArgs e)
		{
			try
			{
				using var exclusionsForm = new ExclusionsManagementForm(new List<string>(nativeUserExclusions));
				if (exclusionsForm.ShowDialog(this) == DialogResult.OK)
				{
					nativeUserExclusions.Clear();
					nativeUserExclusions.AddRange(exclusionsForm.Exclusions
						.Where(path => !string.IsNullOrWhiteSpace(path))
						.Distinct(StringComparer.OrdinalIgnoreCase));
					UpdateExclusionsButtonText();
				}
			}
			catch (Exception ex)
			{
				global::System.Windows.Forms.MessageBox.Show(this, $"Failed to open exclusions management.\n\n{ex.Message}", "Exclusions Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		private void LoadNativeSourcePaths(BackupJob job)
		{
			nativeSourcePaths.Clear();
			if (job.Type == BackupType.SelectedFilesAndFolders && job.Target == BackupTarget.FilesAndFolders)
			{
				IReadOnlyList<string> persistedPaths = SelectedFileListStore.Load(job.Name);
				IEnumerable<string> selectedPaths = persistedPaths.Count > 0 ? persistedPaths : job.SourcePaths;
				nativeSourcePaths.AddRange(selectedPaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase));
			}
			else if (CanUseNativeSourceSelection(job.Type, job.Target))
			{
				nativeSourcePaths.AddRange(job.SourcePaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase));
			}

			RefreshNativeSourceListBox();
		}

		private void RefreshNativeSourceListBox()
		{
			nativeSourceListBox.Items.Clear();
			foreach (string path in nativeSourcePaths)
			{
				nativeSourceListBox.Items.Add(path);
			}

			if (nativeSourcePaths.Count == 0)
			{
				nativeSourceListBox.Items.Add("No native file or folder sources selected.");
				nativeSourceListBox.Enabled = false;
				nativeSourceListBox.SelectedIndex = -1;
			}
			else
			{
				nativeSourceListBox.Enabled = true;
			}

			UpdateLoadedTreeSelectionStates();
			UpdateNativeSourceUi();
		}

		private void UpdateNativeSourceUi()
		{
			bool nativeSelectionSupported = IsNativeSourceSelectionSupportedForCurrentState();
			addFolderSourceButton.Enabled = nativeSelectionSupported;
			addFileSourceButton.Enabled = nativeSelectionSupported;
			removeSourceButton.Enabled = nativeSelectionSupported && nativeSourcePaths.Count > 0 && nativeSourceListBox.Enabled && nativeSourceListBox.SelectedIndex >= 0;
		}

		private void PopulateDriveTree()
		{
			if (driveTree == null)
			{
				return;
			}

			driveTree.BeginUpdate();
			try
			{
				driveTree.Nodes.Clear();
				bool loadedPhysicalDisks = PopulateDriveTreeFromPhysicalDisks();

				if (!loadedPhysicalDisks || driveTree.Nodes.Count == 0)
				{
					PopulateDriveTreeFromLogicalVolumes();
				}

				if (driveTree.Nodes.Count == 0)
				{
					driveTree.Nodes.Add(new TreeNode("No disks or volumes were detected."));
				}
			}
			catch
			{
				if (driveTree.Nodes.Count == 0)
				{
					driveTree.Nodes.Add(new TreeNode("No disks or volumes were detected."));
				}
			}
			finally
			{
				driveTree.EndUpdate();
			}
		}

		private bool PopulateDriveTreeFromPhysicalDisks()
		{
			try
			{
				using var diskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive ORDER BY Index");
				foreach (ManagementObject disk in diskSearcher.Get())
				{
					if (!int.TryParse(disk["Index"]?.ToString(), out int diskIndex))
					{
						continue;
					}

					TreeNode diskNode = CreateDiskNode(
						diskIndex,
						disk["Model"]?.ToString()?.Trim(),
						TryReadInt64(disk["Size"]));

					driveTree.Nodes.Add(diskNode);
				}

				return driveTree.Nodes.Count > 0;
			}
			catch
			{
				return false;
			}
		}

		private void PopulateDriveTreeFromLogicalVolumes()
		{
			Dictionary<int, TreeNode> diskNodes = [];

			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (!driveInfo.IsReady)
				{
					continue;
				}

				if (driveInfo.DriveType != DriveType.Fixed && driveInfo.DriveType != DriveType.Removable)
				{
					continue;
				}

				int? diskNumber = TryGetDiskNumberForDrive(driveInfo.Name);
				if (!diskNumber.HasValue)
				{
					continue;
				}

				if (!diskNodes.TryGetValue(diskNumber.Value, out TreeNode? diskNode))
				{
					(string? model, long size) = GetDiskDisplayInfo(diskNumber.Value);
					diskNode = CreateDiskNode(diskNumber.Value, model, size);
					diskNodes.Add(diskNumber.Value, diskNode);
					driveTree.Nodes.Add(diskNode);
				}
			}
		}

		private TreeNode CreateDiskNode(int diskIndex, string? model, long diskSize)
		{
			string diskModel = string.IsNullOrWhiteSpace(model)
				? $"PhysicalDrive{diskIndex}"
				: model;

			TreeNode diskNode = new($"Disk {diskIndex} - {diskModel} ({FormatStorageSize(diskSize)})")
			{
				Tag = new SourceTreeNodeData
				{
					Kind = SourceTreeNodeKind.Disk,
					DiskNumber = diskIndex,
					SelectionPath = $@"\\.\PHYSICALDRIVE{diskIndex}"
				},
				ImageKey = driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
				SelectedImageKey = driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty
			};

			diskNode.Nodes.Add(CreatePlaceholderNode());
			ApplySelectionStateToNode(diskNode);
			return diskNode;
		}

		private static (string? Model, long Size) GetDiskDisplayInfo(int diskIndex)
		{
			try
			{
				using var diskSearcher = new ManagementObjectSearcher($"SELECT Model, Caption, Manufacturer, Size FROM Win32_DiskDrive WHERE Index = {diskIndex}");
				foreach (ManagementObject disk in diskSearcher.Get())
				{
					string? model = FirstNonEmpty(
						disk["Model"]?.ToString(),
						disk["Caption"]?.ToString(),
						disk["Manufacturer"]?.ToString());

					return (model?.Trim(), TryReadInt64(disk["Size"]));
				}
			}
			catch
			{
			}

			return (null, 0);
		}

		private static string? FirstNonEmpty(params string?[] values)
		{
			foreach (string? value in values)
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					return value;
				}
			}

			return null;
		}

		private static string FormatVolumeNodeText(DriveInfo driveInfo)
		{
			ArgumentNullException.ThrowIfNull(driveInfo);

			string driveLetter = driveInfo.Name.TrimEnd('\\');
			string volumeName = string.IsNullOrWhiteSpace(driveInfo.VolumeLabel)
				? "Local Disk"
				: driveInfo.VolumeLabel;
			string bootSuffix = IsBootVolume(driveInfo) ? " [Boot Volume]" : string.Empty;

			return $"{driveLetter} ({volumeName}) ({FormatStorageSize(driveInfo.TotalSize)}){bootSuffix}";
		}

		private void VolumeAnimationTimer_Tick(object? sender, EventArgs e)
		{
			if (IsDisposed || loadingVolumeNodes.Count == 0)
			{
				volumeAnimationTimer.Stop();
				return;
			}

			volumeAnimationFrame = (volumeAnimationFrame + 1) % VolumeAnimationFrameCount;
			UpdateVolumeNodeAnimation();
		}

		private void UpdateVolumeNodeAnimation()
		{
			if (driveTree.IsDisposed)
			{
				return;
			}

			driveTree.BeginUpdate();
			try
			{
				foreach (TreeNode loadingNode in loadingVolumeNodes.ToArray())
				{
					if (loadingNode.TreeView is not null && !loadingNode.TreeView.IsDisposed)
					{
						string imageKey = GetCurrentVolumeAnimationImageKey();
						loadingNode.ImageKey = imageKey;
						loadingNode.SelectedImageKey = imageKey;
					}
				}
			}
			finally
			{
				driveTree.EndUpdate();
			}
		}

		private void BeginNodeLoading(TreeNode node, bool animateNode)
		{
			ArgumentNullException.ThrowIfNull(node);

			node.Nodes.Clear();
			node.Nodes.Add(CreatePlaceholderNode());

			if (!animateNode)
			{
				return;
			}

			loadingVolumeNodes.Add(node);
			string imageKey = GetCurrentVolumeAnimationImageKey();
			node.ImageKey = imageKey;
			node.SelectedImageKey = imageKey;
			if (!volumeAnimationTimer.Enabled)
			{
				volumeAnimationTimer.Start();
			}
		}

		private void EndNodeLoading(TreeNode node, bool animateNode, SourceTreeNodeKind nodeKind)
		{
			ArgumentNullException.ThrowIfNull(node);

			if (!animateNode)
			{
				return;
			}

			loadingVolumeNodes.Remove(node);
			string imageKey = GetTreeImageKey(nodeKind);
			node.ImageKey = imageKey;
			node.SelectedImageKey = imageKey;

			if (loadingVolumeNodes.Count == 0)
			{
				volumeAnimationTimer.Stop();
			}
		}

		private string GetTreeImageKey(SourceTreeNodeKind nodeKind) => nodeKind switch
		{
			SourceTreeNodeKind.Disk => driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
			SourceTreeNodeKind.Volume => driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
			SourceTreeNodeKind.Partition => driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
			SourceTreeNodeKind.File => driveImageList.Images.ContainsKey("file") ? "file" : string.Empty,
			_ => driveImageList.Images.ContainsKey("folder") ? "folder" : string.Empty
		};

		private string GetCurrentVolumeAnimationImageKey() => GetVolumeAnimationImageKey(volumeAnimationFrame);

		private static string GetVolumeAnimationImageKey(int frame) => $"volume-progress-{frame}";

		private static Bitmap CreateVolumeProgressBitmap(int frame, int frameCount, Size imageSize)
		{
			Bitmap bitmap = new(imageSize.Width, imageSize.Height);
			using Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.Transparent);

			Rectangle outerBounds = new(1, 4, imageSize.Width - 3, imageSize.Height - 8);
			using Pen borderPen = new(Color.FromArgb(82, 104, 112));
			using SolidBrush backgroundBrush = new(Color.FromArgb(224, 236, 238));
			using SolidBrush fillBrush = new(Color.FromArgb(45, 197, 192));
			graphics.FillRectangle(backgroundBrush, outerBounds);
			graphics.DrawRectangle(borderPen, outerBounds);

			int innerWidth = Math.Max(1, outerBounds.Width - 2);
			int filledWidth = frameCount <= 1
				? innerWidth
				: (int)Math.Round(innerWidth * (frame + 1D) / frameCount);

			graphics.FillRectangle(fillBrush, outerBounds.X + 1, outerBounds.Y + 1, Math.Min(innerWidth, filledWidth), Math.Max(1, outerBounds.Height - 1));
			return bitmap;
		}

		private static bool IsBootVolume(DriveInfo driveInfo)
		{
			ArgumentNullException.ThrowIfNull(driveInfo);

			string systemRoot = (Path.GetPathRoot(Environment.SystemDirectory) ?? string.Empty).TrimEnd('\\');
			return driveInfo.RootDirectory.FullName.TrimEnd('\\').Equals(systemRoot, StringComparison.OrdinalIgnoreCase);
		}

		private static Bitmap? LoadTreeBitmapFromSvg(string assetRelativePath, int width, int height)
		{
			if (string.IsNullOrWhiteSpace(assetRelativePath) || width <= 0 || height <= 0)
			{
				return null;
			}

			try
			{
				string assetPath = Path.Combine(AppContext.BaseDirectory, assetRelativePath);
				if (!File.Exists(assetPath))
				{
					return null;
				}

				SvgDocument? svgDocument = SvgDocument.Open<SvgDocument>(assetPath);
				if (svgDocument == null)
				{
					return null;
				}

				svgDocument.Width = width;
				svgDocument.Height = height;
				return svgDocument.Draw(width, height);
			}
			catch
			{
				return null;
			}
		}

		private static int? TryGetDiskNumberForDrive(string driveRoot)
		{
			if (string.IsNullOrWhiteSpace(driveRoot))
			{
				return null;
			}

			string logicalDiskId = driveRoot.TrimEnd('\\');
			try
			{
				using var partitionSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{logicalDiskId}'}} WHERE AssocClass=Win32_LogicalDiskToPartition");
				foreach (ManagementObject partition in partitionSearcher.Get())
				{
					string? partitionId = partition["DeviceID"]?.ToString();
					if (string.IsNullOrWhiteSpace(partitionId))
					{
						continue;
					}

					using var diskSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionId}'}} WHERE AssocClass=Win32_DiskDriveToDiskPartition");
					foreach (ManagementObject disk in diskSearcher.Get())
					{
						if (int.TryParse(disk["Index"]?.ToString(), out int diskNumber))
						{
							return diskNumber;
						}
					}
				}
			}
			catch
			{
			}

			return null;
		}

		private void DriveTree_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
		{
			ArgumentNullException.ThrowIfNull(e);
			if (e.Node == null)
			{
				return;
			}

			if (e.Node.Nodes.Count != 1 || !IsPlaceholderNode(e.Node.Nodes[0]))
			{
				return;
			}

			if (e.Node.Tag is not SourceTreeNodeData nodeData)
			{
				return;
			}

			e.Node.Nodes.Clear();

			switch (nodeData.Kind)
			{
				case SourceTreeNodeKind.Disk:
					PopulateVolumeNodes(e.Node, nodeData.DiskNumber);
					break;
				case SourceTreeNodeKind.Volume:
					e.Cancel = true;
					_ = LoadNodeFileSystemChildrenAsync(e.Node, nodeData, animateNode: true);
					break;
				case SourceTreeNodeKind.Directory:
					e.Cancel = true;
					_ = LoadNodeFileSystemChildrenAsync(e.Node, nodeData, animateNode: false);
					break;
			}
		}

		private void PopulateVolumeNodes(TreeNode diskNode, int diskIndex)
		{
			ArgumentNullException.ThrowIfNull(diskNode);

			bool loadedVolumeNodes = false;
			try
			{
				using var partSearcher = new ManagementObjectSearcher($"SELECT DeviceID, Size, Type FROM Win32_DiskPartition WHERE DiskIndex = {diskIndex}");
				foreach (ManagementObject partition in partSearcher.Get())
				{
					string? partitionDeviceId = partition["DeviceID"]?.ToString();
					if (string.IsNullOrWhiteSpace(partitionDeviceId))
					{
						continue;
					}

					bool hasLogicalVolume = false;
					using var logicalSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionDeviceId}'}} WHERE AssocClass=Win32_LogicalDiskToPartition");
					foreach (ManagementObject logical in logicalSearcher.Get())
					{
						hasLogicalVolume = true;
						string? driveLetter = logical["DeviceID"]?.ToString();
						if (string.IsNullOrWhiteSpace(driveLetter))
						{
							continue;
						}

						try
						{
							DriveInfo driveInfo = new(driveLetter);
							if (!driveInfo.IsReady)
							{
								continue;
							}

							TreeNode volumeNode = new(FormatVolumeNodeText(driveInfo))
							{
								Tag = new SourceTreeNodeData
								{
									Kind = SourceTreeNodeKind.Volume,
									DiskNumber = diskIndex,
									SelectionPath = driveInfo.Name,
									FileSystemPath = driveInfo.RootDirectory.FullName
								},
								ImageKey = GetTreeImageKey(SourceTreeNodeKind.Volume),
								SelectedImageKey = GetTreeImageKey(SourceTreeNodeKind.Volume)
							};

							volumeNode.Nodes.Add(CreatePlaceholderNode());

							ApplySelectionStateToNode(volumeNode);
							diskNode.Nodes.Add(volumeNode);
							loadedVolumeNodes = true;
						}
						catch
						{
						}
					}

					if (!hasLogicalVolume && showHiddenPartitionsCheckBox.Checked)
					{
						long partitionSize = TryReadInt64(partition["Size"]);
						string partitionType = partition["Type"]?.ToString()?.Trim() ?? "System Reserved";
						TreeNode partitionNode = new($"(No Letter) {partitionType} ({FormatStorageSize(partitionSize)})")
						{
							Tag = new SourceTreeNodeData
							{
								Kind = SourceTreeNodeKind.Partition,
								DiskNumber = diskIndex,
								SelectionPath = partitionDeviceId
							},
							ImageKey = GetTreeImageKey(SourceTreeNodeKind.Partition),
							SelectedImageKey = GetTreeImageKey(SourceTreeNodeKind.Partition)
						};

						ApplySelectionStateToNode(partitionNode);
						diskNode.Nodes.Add(partitionNode);
						loadedVolumeNodes = true;
					}
				}
			}
			catch
			{
			}

			if (!loadedVolumeNodes)
			{
				PopulateVolumeNodesFromReadyDrives(diskNode, diskIndex);
			}

			if (diskNode.Nodes.Count == 0)
			{
				diskNode.Nodes.Add(new TreeNode("No volumes detected."));
			}
		}

		private void PopulateVolumeNodesFromReadyDrives(TreeNode diskNode, int diskIndex)
		{
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				try
				{
					if (!driveInfo.IsReady)
					{
						continue;
					}

					if (driveInfo.DriveType != DriveType.Fixed && driveInfo.DriveType != DriveType.Removable)
					{
						continue;
					}

					int? mappedDiskNumber = TryGetDiskNumberForDrive(driveInfo.Name);
					if (mappedDiskNumber != diskIndex)
					{
						continue;
					}

					TreeNode volumeNode = new(FormatVolumeNodeText(driveInfo))
					{
						Tag = new SourceTreeNodeData
						{
							Kind = SourceTreeNodeKind.Volume,
							DiskNumber = diskIndex,
							SelectionPath = driveInfo.Name,
							FileSystemPath = driveInfo.RootDirectory.FullName
						},
						ImageKey = GetTreeImageKey(SourceTreeNodeKind.Volume),
						SelectedImageKey = GetTreeImageKey(SourceTreeNodeKind.Volume)
					};

					volumeNode.Nodes.Add(CreatePlaceholderNode());

					ApplySelectionStateToNode(volumeNode);
					diskNode.Nodes.Add(volumeNode);
				}
				catch
				{
				}
			}
		}

		private async Task LoadNodeFileSystemChildrenAsync(TreeNode parentNode, SourceTreeNodeData nodeData, bool animateNode)
		{
			ArgumentNullException.ThrowIfNull(parentNode);
			ArgumentNullException.ThrowIfNull(nodeData);

			if (loadingVolumeNodes.Contains(parentNode) || string.IsNullOrWhiteSpace(nodeData.FileSystemPath))
			{
				return;
			}

			bool showHidden = showHiddenPartitionsCheckBox.Checked;
			BeginNodeLoading(parentNode, animateNode);
			try
			{
				List<FileSystemNodeEntry> entries = await Task.Run(() => EnumerateFileSystemEntries(nodeData.FileSystemPath, showHidden));

				if (IsDisposed || driveTree.IsDisposed)
				{
					return;
				}

				parentNode.Nodes.Clear();
				foreach (FileSystemNodeEntry entry in entries)
				{
					TreeNode childNode = new(entry.Text)
					{
						Tag = new SourceTreeNodeData
						{
							Kind = entry.Kind,
							SelectionPath = entry.SelectionPath,
							FileSystemPath = entry.FileSystemPath
						},
						ImageKey = entry.Kind == SourceTreeNodeKind.File
							? (driveImageList.Images.ContainsKey("file") ? "file" : string.Empty)
							: (driveImageList.Images.ContainsKey("folder") ? "folder" : string.Empty),
						SelectedImageKey = entry.Kind == SourceTreeNodeKind.File
							? (driveImageList.Images.ContainsKey("file") ? "file" : string.Empty)
							: (driveImageList.Images.ContainsKey("folder") ? "folder" : string.Empty)
					};

					if (entry.Kind == SourceTreeNodeKind.Directory)
					{
						childNode.Nodes.Add(CreatePlaceholderNode());
					}

					ApplySelectionStateToNode(childNode);
					parentNode.Nodes.Add(childNode);
				}

				if (parentNode.Nodes.Count == 0)
				{
					parentNode.Nodes.Add(new TreeNode("No files or folders detected."));
				}

				parentNode.Expand();
			}
			catch
			{
				if (!IsDisposed && !driveTree.IsDisposed)
				{
					parentNode.Nodes.Clear();
					parentNode.Nodes.Add(new TreeNode("Unable to load files or folders."));
					parentNode.Expand();
				}
			}
			finally
			{
				EndNodeLoading(parentNode, animateNode, nodeData.Kind);
			}
		}

		private List<FileSystemNodeEntry> EnumerateFileSystemEntries(string rootPath, bool showHidden)
		{
			List<FileSystemNodeEntry> entries = [];
			foreach (string directoryPath in Directory.EnumerateDirectories(rootPath))
			{
				try
				{
					DirectoryInfo directoryInfo = new(directoryPath);
					if (!showHidden && directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
					{
						continue;
					}

					entries.Add(new FileSystemNodeEntry
					{
						Kind = SourceTreeNodeKind.Directory,
						Text = directoryInfo.Name,
						SelectionPath = directoryInfo.FullName,
						FileSystemPath = directoryInfo.FullName
					});
				}
				catch
				{
				}
			}

			foreach (string filePath in Directory.EnumerateFiles(rootPath))
			{
				try
				{
					FileInfo fileInfo = new(filePath);
					if (!showHidden && fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
					{
						continue;
					}

					entries.Add(new FileSystemNodeEntry
					{
						Kind = SourceTreeNodeKind.File,
						Text = fileInfo.Name,
						SelectionPath = fileInfo.FullName,
						FileSystemPath = fileInfo.FullName
					});
				}
				catch
				{
				}
			}

			return entries;
		}

		private static TreeNode CreatePlaceholderNode() => new("Loading...");

		private static bool IsPlaceholderNode(TreeNode node)
		{
			ArgumentNullException.ThrowIfNull(node);
			return node.Tag is null && string.Equals(node.Text, "Loading...", StringComparison.Ordinal);
		}

		private static long TryReadInt64(object? value)
		{
			try
			{
				return value == null ? 0 : Convert.ToInt64(value);
			}
			catch
			{
				return 0;
			}
		}

		private static string FormatStorageSize(long bytes)
		{
			if (bytes <= 0)
			{
				return "Unknown size";
			}

			double value = bytes;
			string[] units = ["B", "KB", "MB", "GB", "TB", "PB"];
			int unitIndex = 0;

			while (value >= 1024 && unitIndex < units.Length - 1)
			{
				value /= 1024;
				unitIndex++;
			}

			return $"{value:0.##} {units[unitIndex]}";
		}

		private void DriveTree_AfterCheck(object? sender, TreeViewEventArgs e)
		{
			if (e.Node == null || suppressTreeCheckSync || e.Node.Tag is not SourceTreeNodeData nodeData)
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(nodeData.SelectionPath))
			{
				return;
			}

			if (e.Node.Checked)
			{
				if (!nativeSourcePaths.Any(existingPath => string.Equals(existingPath, nodeData.SelectionPath, StringComparison.OrdinalIgnoreCase)))
				{
					nativeSourcePaths.Add(nodeData.SelectionPath);
				}
			}
			else
			{
				nativeSourcePaths.RemoveAll(existingPath => string.Equals(existingPath, nodeData.SelectionPath, StringComparison.OrdinalIgnoreCase));
			}

			RefreshNativeSourceListBox();
			UpdateAdvancedStateSummary();
		}

		private void UpdateLoadedTreeSelectionStates()
		{
			if (driveTree == null)
			{
				return;
			}

			suppressTreeCheckSync = true;
			try
			{
				foreach (TreeNode node in driveTree.Nodes)
				{
					ApplySelectionStateRecursively(node);
				}
			}
			finally
			{
				suppressTreeCheckSync = false;
			}
		}

		private void ApplySelectionStateRecursively(TreeNode node)
		{
			ArgumentNullException.ThrowIfNull(node);

			ApplySelectionStateToNode(node);
			foreach (TreeNode childNode in node.Nodes)
			{
				if (!IsPlaceholderNode(childNode))
				{
					ApplySelectionStateRecursively(childNode);
				}
			}
		}

		private void ApplySelectionStateToNode(TreeNode node)
		{
			ArgumentNullException.ThrowIfNull(node);

			if (node.Tag is not SourceTreeNodeData nodeData || string.IsNullOrWhiteSpace(nodeData.SelectionPath))
			{
				node.Checked = false;
				return;
			}

			node.Checked = nativeSourcePaths.Any(existingPath =>
				string.Equals(existingPath, nodeData.SelectionPath, StringComparison.OrdinalIgnoreCase));
		}

		private void AddFolderSource_Click(object? sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select a folder to back up"
			};

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				AddNativeSourcePath(dialog.SelectedPath);
			}
		}

		private void AddFileSource_Click(object? sender, EventArgs e)
		{
			using var dialog = new OpenFileDialog
			{
				Title = "Select file(s) to back up",
				Multiselect = true,
				CheckFileExists = true,
				Filter = "All Files (*.*)|*.*"
			};

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				foreach (string fileName in dialog.FileNames)
				{
					AddNativeSourcePath(fileName);
				}
			}
		}

		private void RemoveSelectedSource_Click(object? sender, EventArgs e)
		{
			if (!nativeSourceListBox.Enabled || nativeSourceListBox.SelectedItem is not string selectedPath)
			{
				return;
			}

			nativeSourcePaths.RemoveAll(path => string.Equals(path, selectedPath, StringComparison.OrdinalIgnoreCase));
			RefreshNativeSourceListBox();
			UpdateAdvancedStateSummary();
		}

		private void AddNativeSourcePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}

			if (!nativeSourcePaths.Any(existingPath => string.Equals(existingPath, path, StringComparison.OrdinalIgnoreCase)))
			{
				nativeSourcePaths.Add(path);
				RefreshNativeSourceListBox();
				UpdateAdvancedStateSummary();
			}
		}

		private bool IsNativeSourceSelectionSupportedForCurrentState()
		{
			return CanUseNativeSourceSelection(GetSelectedBackupType(), GetEffectiveSourceTargetForCurrentState());
		}

		private BackupTarget GetEffectiveSourceTargetForCurrentState()
		{
			BackupTarget existingTarget = currentJob?.Target ?? existingJob?.Target ?? 0;
			if (existingTarget == BackupTarget.Disk || existingTarget == BackupTarget.Volume || existingTarget == BackupTarget.HyperV)
			{
				return existingTarget;
			}

			return BackupTarget.FilesAndFolders;
		}

		private static bool CanUseNativeSourceSelection(BackupType backupType, BackupTarget target)
		{
			if (backupType == BackupType.SelectedFilesAndFolders)
			{
				return target != BackupTarget.Disk && target != BackupTarget.Volume && target != BackupTarget.HyperV;
			}

			if (backupType == BackupType.CloneToDisk ||
				backupType == BackupType.CloneToVirtualDisk ||
				backupType == BackupType.CloneHyperVSystem ||
				backupType == BackupType.ExportHyperVSystem ||
				backupType == BackupType.SelectedFilesAndFolders)
			{
				return false;
			}

			if (target == BackupTarget.Disk || target == BackupTarget.Volume || target == BackupTarget.HyperV)
			{
				return false;
			}

			return true;
		}

		private void UpdateAdvancedStateSummary()
		{
			advancedSelectionListBox.Items.Clear();

			BackupJob? job = currentJob ?? existingJob;
			if (job == null)
			{
				if (nativeSourcePaths.Count > 0)
				{
					advancedStateLabel.Text = $"Native file/folder sources staged for save: {nativeSourcePaths.Count} item(s).";
					foreach (string path in nativeSourcePaths.Take(25))
					{
						advancedSelectionListBox.Items.Add($"Source: {path}");
					}

					if (nativeSourcePaths.Count > 25)
					{
						advancedSelectionListBox.Items.Add($"...and {nativeSourcePaths.Count - 25} more item(s)");
					}
				}
				else
				{
					advancedStateLabel.Text = "No source selection has been configured yet. Add native file/folder sources or open Advanced Editor for disk, volume, clone, or Hyper-V selection.";
					advancedSelectionListBox.Items.Add("No sources selected yet.");
				}

				return;
			}

			List<string> summaryItems = BuildAdvancedSummaryItems(job);
			advancedStateLabel.Text = BuildAdvancedStateMessage(job, summaryItems.Count);
			if (summaryItems.Count == 0)
			{
				advancedSelectionListBox.Items.Add("No advanced sources are currently attached.");
				return;
			}

			foreach (string item in summaryItems.Take(25))
			{
				advancedSelectionListBox.Items.Add(item);
			}

			if (summaryItems.Count > 25)
			{
				advancedSelectionListBox.Items.Add($"...and {summaryItems.Count - 25} more item(s)");
			}
		}

		private static string BuildAdvancedStateMessage(BackupJob job, int itemCount)
		{
			if (job.Type == BackupType.SelectedFilesAndFolders && job.Target == BackupTarget.FilesAndFolders)
			{
				string selectionText = itemCount == 1 ? "1 selected item" : $"{itemCount} selected items";
				return $"Selected file and folder backup currently staged natively: {selectionText}. You can save and start this backup directly from WinForms.";
			}

			string targetText;
			switch (job.Target)
			{
				case BackupTarget.Disk:
					targetText = "Disk sources";
					break;
				case BackupTarget.Volume:
					targetText = "Volume sources";
					break;
				case BackupTarget.FilesAndFolders:
					targetText = "File/folder sources";
					break;
				case BackupTarget.HyperV:
					targetText = "Hyper-V sources";
					break;
				default:
					targetText = "Advanced sources";
					break;
			}

			string countText = itemCount == 1 ? "1 item" : $"{itemCount} items";
			return $"{targetText} currently attached to this job: {countText}. Use Advanced Editor to change source-tree, clone, or Hyper-V selections.";
		}

		private static List<string> BuildAdvancedSummaryItems(BackupJob job)
		{
			var summaryItems = new List<string>();
			if (job.HyperVMachines.Count > 0)
			{
				summaryItems.AddRange(job.HyperVMachines
					.Where(name => !string.IsNullOrWhiteSpace(name))
					.Select(name => $"Hyper-V: {name}"));
			}

			IEnumerable<string> paths = job.Type == BackupType.SelectedFilesAndFolders
				? job.SelectedFilesSourceRoots.Concat(job.SourcePaths)
				: job.SourcePaths;
			summaryItems.AddRange(paths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.Select(path => $"Source: {path}"));

			if (job.Type == BackupType.CloneHyperVSystem && job.RenameHyperVSystem && !string.IsNullOrWhiteSpace(job.RenameHyperVSystemName))
			{
				summaryItems.Add($"Rename Hyper-V System: {job.RenameHyperVSystemName}");
			}

			return summaryItems;
		}

		private void UpdateBackupTypeUi()
		{
			BackupType backupType = GetSelectedBackupType();
			bool isNativeSaveSupported = backupType == BackupType.Full || backupType == BackupType.Incremental || backupType == BackupType.Differential || backupType == BackupType.SelectedFilesAndFolders;
			bool isSelectedFiles = backupType == BackupType.SelectedFilesAndFolders;
			bool isCloneOrExport = backupType == BackupType.CloneToVirtualDisk || backupType == BackupType.CloneHyperVSystem || backupType == BackupType.ExportHyperVSystem;
			bool nativeSourceSelectionSupported = IsNativeSourceSelectionSupportedForCurrentState();

			retentionPanel.Visible = backupType == BackupType.Full;
			selectedFilesRetentionPanel.Visible = isSelectedFiles;
			cloneRetentionPanel.Visible = isCloneOrExport;
			saveJobButton.Enabled = isNativeSaveSupported;
			startBackupButton.Enabled = true;
			openAdvancedEditorButton.Enabled = !isNativeSaveSupported || !nativeSourceSelectionSupported;
			openAdvancedEditorButton.Visible = !isSelectedFiles;
			startBackupButton.Text = "Start Backup";
			actionInfoLabel.Text = isSelectedFiles
				? "Advanced migration status\n\n- Native: common settings, encryption, schedule, retention, selected file/folder source lists, immediate execution\n- Remaining legacy-only areas: disk, volume, clone/export, and Hyper-V authoring"
				: "Advanced migration status\n\n- Native: common settings, encryption, schedule, retention\n- Legacy advanced editor: source tree, Hyper-V, clone/export, immediate execution";
			actionHelpLabel.Text = isSelectedFiles
				? "Selected file and folder jobs can now be saved and started directly from this WinForms screen. Use the Add Folder and Add File buttons to build the selection list."
				: "Save Native Settings stores common backup metadata now. To select sources, clones, Hyper-V systems, or to run the job immediately, use Advanced Editor until those sections are migrated.";
			UpdateNativeSourceUi();
			UpdateAdvancedStateSummary();

			nativeCoverageLabel.Text = isNativeSaveSupported
				? nativeSourceSelectionSupported
					? "This native WinForms screen can now save standard file/folder full, incremental, and differential jobs, including ordinary file and folder source paths. Use Advanced Editor only for disk, volume, clone/export, Hyper-V, and immediate execution flows."
					: "This job currently uses a disk, volume, or Hyper-V source selection that remains on the legacy advanced editor. You can still update common settings here, but source changes stay in Advanced Editor for now."
				: "This backup type still depends on advanced source or clone/Hyper-V configuration. Use the advanced editor for full functionality while the remaining sections are migrated to WinForms.";
		}

		private void UpdateEncryptionUi()
		{
			bool enabled = encryptCheckBox.Checked;
			encryptionPanel.Visible = enabled;
			bool passwordLocked = enabled && hasSavedEncryptionPassword;
			encryptionPasswordTextBox.ReadOnly = passwordLocked;
			verifyEncryptionPasswordTextBox.ReadOnly = passwordLocked;
			verifyEncryptionPasswordTextBox.Visible = enabled && !hasSavedEncryptionPassword;
			foreach (Control control in encryptionPanel.Controls)
			{
				ToggleVerifyPasswordVisibility(control, enabled && !hasSavedEncryptionPassword);
			}
			showPasswordCheckBox.Enabled = enabled;
			if (!enabled)
			{
				hasSavedEncryptionPassword = false;
				savedProtectedPassword = string.Empty;
				suppressPasswordSync = true;
				encryptionPasswordTextBox.Clear();
				verifyEncryptionPasswordTextBox.Clear();
				showPasswordCheckBox.Checked = false;
				suppressPasswordSync = false;
			}
			UpdatePasswordVisibility();
		}

		private static void ToggleVerifyPasswordVisibility(Control control, bool visible)
		{
			if (control is TableLayoutPanel table)
			{
				TextBox? verifyTextBox = table.Controls.OfType<TextBox>().Skip(1).FirstOrDefault();
				foreach (Control child in table.Controls)
				{
					if (child.Name == "VerifyPasswordLabel" || child == verifyTextBox)
					{
						child.Visible = visible;
					}
				}
			}
		}

		private async void StartBackup_Click(object? sender, EventArgs e)
		{
			BackupType backupType = GetSelectedBackupType();
			bool nativeRunSupported = backupType == BackupType.Full || backupType == BackupType.Incremental || backupType == BackupType.Differential;
			if (!nativeRunSupported)
			{
				OpenAdvancedEditor_Click(sender, e);
				return;
			}

			if (!ValidateNativeInputs())
			{
				return;
			}

			try
			{
				Enabled = false;
				UseWaitCursor = true;

				BackupJob job = SaveOrUpdateNativeJob(showSuccessMessage: false);
				bool serviceOk = await CheckBackupServiceAsync();
				if (!serviceOk)
				{
					return;
				}

				BackupLogger.LogInfo(job.Name, "User initiated manual backup from WinForms BackupWindowNew");
				BackupServiceClient backupServiceClient = new();
				bool started = await backupServiceClient.RunBackupNowAsync(job.Id);
				if (!started)
				{
					BackupLogger.LogError(job.Name, "Failed to communicate with Secure Server Backup Service - backup was not started");
					global::System.Windows.Forms.MessageBox.Show(
						this,
						"Failed to start backup. The service may be busy or not responding.\n\nTry again in a few moments, or restart the Secure Server Backup Service from Windows Services.",
						"Service Error",
						global::System.Windows.Forms.MessageBoxButtons.OK,
						global::System.Windows.Forms.MessageBoxIcon.Error);
					return;
				}

				BackupLogger.LogInfo(job.Name, "Service accepted backup request - backup is starting");
				var progressForm = new BackupProgressForm(job.Id, job.Name);
				progressForm.Show(this);
				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				global::System.Windows.Forms.MessageBox.Show(this, $"Failed to start backup now.\n\n{ex.Message}", "Start Backup Failed", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Error);
			}
			finally
			{
				UseWaitCursor = false;
				Enabled = true;
			}
		}

		private void UpdatePasswordVisibility()
		{
			bool usePasswordChars = !showPasswordCheckBox.Checked;
			encryptionPasswordTextBox.UseSystemPasswordChar = usePasswordChars;
			verifyEncryptionPasswordTextBox.UseSystemPasswordChar = usePasswordChars;
		}

		private void UpdateScheduleUi()
		{
			schedulePanel.Enabled = enableScheduleCheckBox.Checked;
			UpdateScheduleFrequencyUi();
		}

		private void UpdateScheduleFrequencyUi()
		{
			ScheduleFrequency frequency = GetSelectedFrequency();
			weeklyPanel.Visible = frequency == ScheduleFrequency.Weekly;
			monthlyPanel.Visible = frequency == ScheduleFrequency.Monthly;
		}

		private void EncryptionPasswordTextBox_TextChanged(object? sender, EventArgs e)
		{
			if (suppressPasswordSync || hasSavedEncryptionPassword)
			{
				return;
			}
		}

		private void BrowseDestination_Click(object? sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select backup destination"
			};
			if (!string.IsNullOrWhiteSpace(destinationTextBox.Text))
			{
				dialog.SelectedPath = destinationTextBox.Text;
			}
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				destinationTextBox.Text = dialog.SelectedPath;
			}
		}

		private void OpenAdvancedEditor_Click(object? sender, EventArgs e)
		{
			global::System.Windows.Forms.MessageBox.Show(
				this,
				"This backup type is not yet available in the WinForms backup editor.\n\nSelected file and folder jobs can now be created here, but disk, volume, clone/export, and Hyper-V authoring still need native WinForms replacements before WPF can be removed completely.",
				"WinForms Migration In Progress",
				global::System.Windows.Forms.MessageBoxButtons.OK,
				global::System.Windows.Forms.MessageBoxIcon.Information);
		}

		private void SaveJob_Click(object? sender, EventArgs e)
		{
			if (!ValidateNativeInputs())
			{
				return;
			}

			try
			{
				SaveOrUpdateNativeJob(showSuccessMessage: true);
				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				global::System.Windows.Forms.MessageBox.Show(this, $"ERROR: Failed to save backup job!\n\n{ex.Message}", "Save Failed", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		private BackupJob SaveOrUpdateNativeJob(bool showSuccessMessage)
		{
			string? previousJobName = currentJob?.Name ?? existingJob?.Name;
			BackupJob job = CreateNativeJobFromInput();
			bool nativeSourceSelectionSupported = IsNativeSourceSelectionSupportedForCurrentState();
			if (currentJob != null)
			{
				job.Id = currentJob.Id;
				if (!nativeSourceSelectionSupported)
				{
					job.SourcePaths = currentJob.SourcePaths.ToList();
					job.SelectedFilesSourceRoots = currentJob.SelectedFilesSourceRoots.ToList();
					job.HyperVMachines = currentJob.HyperVMachines.ToList();
					job.Target = currentJob.Target;
					job.IsHyperVBackup = currentJob.IsHyperVBackup;
				}

				job.RenameHyperVSystem = currentJob.RenameHyperVSystem;
				job.RenameHyperVSystemName = currentJob.RenameHyperVSystemName;
				job.UserExclusions = new List<string>(nativeUserExclusions);
				jobManager.UpdateJob(job);
				PersistSelectedFileList(previousJobName, job);

				if (showSuccessMessage)
				{
					global::System.Windows.Forms.MessageBox.Show(
						this,
						nativeSourceSelectionSupported
							? $"Backup job '{job.Name}' updated successfully!\n\nNative file/folder sources were saved with the job. Open the advanced editor only when you need disk, volume, clone/export, or Hyper-V settings."
							: $"Backup job '{job.Name}' updated successfully!\n\nAdvanced source selection remains unchanged. Open the advanced editor when you need to change sources, clone/export options, or Hyper-V settings.",
						"Success",
						global::System.Windows.Forms.MessageBoxButtons.OK,
						global::System.Windows.Forms.MessageBoxIcon.Information);
				}
			}
			else
			{
				jobManager.AddJob(job);
				PersistSelectedFileList(previousJobName, job);

				if (showSuccessMessage)
				{
					global::System.Windows.Forms.MessageBox.Show(
						this,
						nativeSourceSelectionSupported
							? $"Backup job '{job.Name}' created successfully!\n\nNative file/folder sources were saved with the job. Use Advanced Editor only if you need disk, volume, clone/export, or Hyper-V options."
							: $"Backup job '{job.Name}' created successfully!\n\nNext step: open the advanced editor to choose sources and any clone or Hyper-V options until full WinForms parity is finished.",
						"Success",
						global::System.Windows.Forms.MessageBoxButtons.OK,
						global::System.Windows.Forms.MessageBoxIcon.Information);
				}
			}

			currentJob = jobManager.GetJob(job.Id) ?? job;
			hasSavedEncryptionPassword = job.EncryptBackup && !string.IsNullOrWhiteSpace(job.ProtectedEncryptionPassword);
			savedProtectedPassword = job.ProtectedEncryptionPassword;
			LoadNativeUserExclusions(currentJob);
			UpdateAdvancedStateSummary();
			return currentJob;
		}

		private async Task<bool> CheckBackupServiceAsync()
		{
			try
			{
				if (!ServiceInstaller.IsServiceInstalled())
				{
					BackupLogger.LogServiceInfo("Secure Server Backup Service not installed - installing automatically...");
					(bool success, string message) = await ServiceInstaller.InstallAndStartServiceAsync();
					if (!success)
					{
						BackupLogger.LogServiceError($"Failed to install service: {message}");
						global::System.Windows.Forms.MessageBox.Show(this, $"Failed to install service:\n\n{message}\n\nPlease ensure the application has Administrator privileges.", "Installation Failed", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Error);
						return false;
					}

					BackupLogger.LogServiceInfo("Secure Server Backup Service installed and started successfully");
					return true;
				}

				System.ServiceProcess.ServiceControllerStatus? status = ServiceInstaller.GetServiceStatus();
				if (status != System.ServiceProcess.ServiceControllerStatus.Running)
				{
					BackupLogger.LogServiceInfo($"Secure Server Backup Service not running (Status: {status}) - starting automatically...");
					(bool success, string message) = await ServiceInstaller.StartServiceAsync();
					if (!success)
					{
						BackupLogger.LogServiceError($"Failed to start service: {message}");
						global::System.Windows.Forms.MessageBox.Show(this, $"Failed to start service:\n\n{message}", "Start Failed", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Error);
						return false;
					}

					BackupLogger.LogServiceInfo("Secure Server Backup Service started successfully");
				}

				return true;
			}
			catch (Exception ex)
			{
				BackupLogger.LogServiceError($"Error checking service status: {ex.Message}");
				global::System.Windows.Forms.MessageBox.Show(this, $"Error checking service status:\n\n{ex.Message}", "Service Check Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}
		}

		private bool ValidateNativeInputs()
		{
			if (string.IsNullOrWhiteSpace(backupNameTextBox.Text))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please enter a backup name.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (string.IsNullOrWhiteSpace(destinationTextBox.Text))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please select a backup destination.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			BackupType backupType = GetSelectedBackupType();
			if (backupType != BackupType.Full && backupType != BackupType.Incremental && backupType != BackupType.Differential && backupType != BackupType.SelectedFilesAndFolders)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "This backup type still requires the advanced editor in the current migration stage. Use 'Open Advanced Editor...' for this backup type.", "Native Form Limitation", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (IsNativeSourceSelectionSupportedForCurrentState() && nativeSourcePaths.Count == 0)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Add at least one file or folder source before saving this native file/folder backup job.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (enableScheduleCheckBox.Checked && !TryGetScheduledTime(out _, out _))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please enter a valid scheduled time. Minutes must be between 00 and 59.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (!int.TryParse(retainCountTextBox.Text, out int retainCount) || retainCount < 1)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please enter a full backup retention value of 1 or greater.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (encryptCheckBox.Checked && !hasSavedEncryptionPassword)
			{
				if (string.IsNullOrWhiteSpace(encryptionPasswordTextBox.Text))
				{
					global::System.Windows.Forms.MessageBox.Show(this, "Please enter a password for backup encryption.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
					return false;
				}

				if (!string.Equals(encryptionPasswordTextBox.Text, verifyEncryptionPasswordTextBox.Text, StringComparison.Ordinal))
				{
					global::System.Windows.Forms.MessageBox.Show(this, "The encryption passwords do not match.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
					return false;
				}
			}

			return true;
		}

		private BackupJob CreateNativeJobFromInput()
		{
			BackupType backupType = GetSelectedBackupType();
			BackupJob? sourceJob = currentJob ?? existingJob;
			BackupTarget effectiveTarget = GetEffectiveSourceTargetForCurrentState();
			var job = new BackupJob
			{
				Id = sourceJob?.Id ?? Guid.NewGuid(),
				Name = backupNameTextBox.Text.Trim(),
				Type = backupType,
				DestinationPath = destinationTextBox.Text.Trim(),
				CompressData = compressCheckBox.Checked,
				VerifyAfterBackup = verifyCheckBox.Checked,
				EncryptBackup = encryptCheckBox.Checked,
				ProtectedEncryptionPassword = GetProtectedEncryptionPassword(),
				RetainFullBackupCount = int.TryParse(retainCountTextBox.Text, out int retainCount) ? Math.Max(1, retainCount) : 1,
				SelectedFilesRetentionCount = ParseRangedValue(selectedFilesRetentionComboBox.Text, 7),
				CloneRetentionCount = ParseRangedValue(cloneRetentionComboBox.Text, 7),
				SourcePaths = sourceJob?.SourcePaths?.ToList() ?? new List<string>(),
				SelectedFilesSourceRoots = sourceJob?.SelectedFilesSourceRoots?.ToList() ?? new List<string>(),
				HyperVMachines = sourceJob?.HyperVMachines?.ToList() ?? new List<string>(),
				Target = sourceJob?.Target ?? BackupTarget.FilesAndFolders,
				IsHyperVBackup = sourceJob?.IsHyperVBackup ?? false,
				RenameHyperVSystem = sourceJob?.RenameHyperVSystem ?? false,
				RenameHyperVSystemName = sourceJob?.RenameHyperVSystemName ?? string.Empty,
				UserExclusions = sourceJob?.UserExclusions?.ToList() ?? new List<string>()
			};

			job.UserExclusions = new List<string>(nativeUserExclusions);

			if (backupType == BackupType.SelectedFilesAndFolders)
			{
				job.Target = BackupTarget.FilesAndFolders;
				job.IsHyperVBackup = false;
				job.HyperVMachines.Clear();
				job.SourcePaths = nativeSourcePaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();
				job.SelectedFilesSourceRoots = GetSelectedFilesSourceRoots(job.SourcePaths);
			}
			else if (CanUseNativeSourceSelection(backupType, effectiveTarget))
			{
				job.Target = BackupTarget.FilesAndFolders;
				job.IsHyperVBackup = false;
				job.HyperVMachines.Clear();
				job.SelectedFilesSourceRoots.Clear();
				job.SourcePaths = nativeSourcePaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();
			}

			if (enableScheduleCheckBox.Checked && TryGetScheduledTime(out int hour24, out int minute))
			{
				job.Schedule = new BackupSchedule
				{
					JobId = job.Id,
					Enabled = true,
					Frequency = GetSelectedFrequency(),
					Time = new TimeSpan(hour24, minute, 0)
				};

				if (job.Schedule.Frequency == ScheduleFrequency.Weekly)
				{
					foreach (object? checkedItem in weeklyDaysCheckedListBox.CheckedItems)
					{
						if (checkedItem is string dayName && Enum.TryParse(dayName, out DayOfWeek dayOfWeek))
						{
							job.Schedule.DaysOfWeek.Add(dayOfWeek);
						}
					}
				}
				else if (job.Schedule.Frequency == ScheduleFrequency.Monthly)
				{
					job.Schedule.DayOfMonth = int.TryParse(dayOfMonthComboBox.Text, out int dayOfMonth) ? dayOfMonth : 1;
				}
			}

			return job;
		}

		private static List<string> GetSelectedFilesSourceRoots(IEnumerable<string> selectedPaths)
		{
			ArgumentNullException.ThrowIfNull(selectedPaths);

			return selectedPaths
				.Where(selectedPath => !string.IsNullOrWhiteSpace(selectedPath))
				.Select(selectedPath => System.IO.Path.GetPathRoot(selectedPath.Trim())?.TrimEnd('\\'))
				.Where(rootPath => !string.IsNullOrWhiteSpace(rootPath))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList()!;
		}

		private void PersistSelectedFileList(string? previousJobName, BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			bool isSelectedFilesJob = job.Type == BackupType.SelectedFilesAndFolders && job.Target == BackupTarget.FilesAndFolders;
			if (!isSelectedFilesJob)
			{
				if (!string.IsNullOrWhiteSpace(previousJobName))
				{
					SelectedFileListStore.Delete(previousJobName);
				}

				SelectedFileListStore.Delete(job.Name);
				return;
			}

			if (!string.IsNullOrWhiteSpace(previousJobName) &&
				!string.Equals(previousJobName, job.Name, StringComparison.OrdinalIgnoreCase))
			{
				SelectedFileListStore.Delete(previousJobName);
			}

			SelectedFileListStore.Save(job.Name, job.SourcePaths);
		}

		private string GetProtectedEncryptionPassword()
		{
			if (!encryptCheckBox.Checked)
			{
				return string.Empty;
			}

			if (hasSavedEncryptionPassword && !string.IsNullOrWhiteSpace(savedProtectedPassword))
			{
				return savedProtectedPassword;
			}

			return BackupEncryptionService.ProtectPassword(encryptionPasswordTextBox.Text);
		}

		private static int ParseRangedValue(string? text, int defaultValue)
		{
			return int.TryParse(text?.Trim(), out int parsed)
				? Math.Clamp(parsed, 1, 30)
				: defaultValue;
		}

		private BackupType GetSelectedBackupType()
		{
			switch (backupTypeComboBox.SelectedIndex)
			{
				case 0:
					return BackupType.Full;
				case 1:
					return BackupType.Incremental;
				case 2:
					return BackupType.Differential;
				case 3:
					return BackupType.SelectedFilesAndFolders;
				case 4:
					return BackupType.CloneToDisk;
				case 5:
					return BackupType.CloneToVirtualDisk;
				case 6:
					return BackupType.CloneHyperVSystem;
				case 7:
					return BackupType.ExportHyperVSystem;
				default:
					return BackupType.Full;
			}
		}

		private ScheduleFrequency GetSelectedFrequency()
		{
			switch (frequencyComboBox.SelectedIndex)
			{
				case 1:
					return ScheduleFrequency.Weekly;
				case 2:
					return ScheduleFrequency.Monthly;
				case 3:
					return ScheduleFrequency.Once;
				default:
					return ScheduleFrequency.Daily;
			}
		}

		private bool TryGetScheduledTime(out int hour24, out int minute)
		{
			hour24 = 0;
			minute = 0;

			if (!int.TryParse(hourComboBox.Text, out int hour12) || hour12 < 1 || hour12 > 12)
			{
				return false;
			}

			if (!int.TryParse(minuteComboBox.Text, out minute) || minute < 0 || minute > 59)
			{
				return false;
			}

			string amPm = amPmComboBox.SelectedItem?.ToString() ?? "AM";
			if (string.Equals(amPm, "AM", StringComparison.OrdinalIgnoreCase))
			{
				hour24 = hour12 == 12 ? 0 : hour12;
			}
			else
			{
				hour24 = hour12 == 12 ? 12 : hour12 + 12;
			}

			minuteComboBox.Text = minute.ToString("D2");
			return true;
		}
	}
}
