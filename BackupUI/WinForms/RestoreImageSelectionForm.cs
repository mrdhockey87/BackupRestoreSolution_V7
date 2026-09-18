using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackup.Windows;
using SecureServerBackupCommon;
using BackupEngineInterop = SecureServerBackup.Services.BackupEngineInterop;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestoreImageSelectionForm : Form
	{
		private const string RestoreLogJobName = "[Restore]";
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

		private readonly RestoreSelectionContext restoreSelection;

		private readonly List<TargetChoice> restoreTargets = new();
		private RestoreTargetKind restoreTargetKind;
		private NativeBackupMountManager.RestoreDiskPlan? diskRestorePlan;
		private bool isHyperVBackupPoint;
		private bool showHiddenPartitions;
		private string? selectedTargetPath;
		private int? selectedTargetDiskNumber;
		private VolumeInfo? selectedRestoreVolume;
		private IReadOnlyList<VolumeInfo>? selectedRestoreDiskGroup;

		public RestoreImageSelectionForm()
			: this(CreateDesignTimeRestoreSelection())
		{
		}

		public RestoreImageSelectionForm(RestoreSelectionContext restoreSelection)
		{
			ArgumentNullException.ThrowIfNull(restoreSelection);
			ArgumentNullException.ThrowIfNull(restoreSelection.Backup);
			ArgumentNullException.ThrowIfNull(restoreSelection.RestorePoint);

			this.restoreSelection = restoreSelection;
			selectedRestoreDiskGroup = restoreSelection.SelectedVolumes.Count > 1 ? restoreSelection.SelectedVolumes : null;
			selectedRestoreVolume = restoreSelection.SelectedVolumes.Count == 1 ? restoreSelection.SelectedVolumes[0] : null;
			InitializeComponent();

			summaryLabel.Text = BuildSummaryText();

			if (IsInDesignMode)
			{
				PopulateDesignTimeState();
			}
			else
			{
				Load += RestoreImageSelectionForm_Load;
			}
		}

		private async void RestoreImageSelectionForm_Load(object? sender, EventArgs e)
		{
			try
			{
				await InitializeAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void HyperVVmActionComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			UpdateHyperVUiState();
		}

		private void HyperVDiskAttachModeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			UpdateHyperVUiState();
		}

		private void BrowseHyperVRestoreDirectoryButton_Click(object? sender, EventArgs e)
		{
			BrowseHyperVRestoreDirectory();
		}

		private void BrowseHyperVVirtualDiskPathButton_Click(object? sender, EventArgs e)
		{
			BrowseHyperVVirtualDiskPath();
		}

		private void BrowseNewHyperVVmPathButton_Click(object? sender, EventArgs e)
		{
			BrowseNewHyperVVmLocation();
		}

		private void RestoreModeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			UpdateRestoreModeFromSelection();
		}

		private async void ShowHiddenCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			showHiddenPartitions = showHiddenCheckBox.Checked;

			try
			{
				await LoadRestoreTargetsAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void TargetListView_SelectedIndexChanged(object? sender, EventArgs e)
		{
			HandleTargetSelectionChanged();
		}

		private async void TargetListView_DoubleClick(object? sender, EventArgs e)
		{
			try
			{
				await StartRestoreAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void RefreshTargetsButton_Click(object? sender, EventArgs e)
		{
			try
			{
				await LoadRestoreTargetsAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void StartRestoreButton_Click(object? sender, EventArgs e)
		{
			try
			{
				await StartRestoreAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private static RestoreSelectionContext CreateDesignTimeRestoreSelection()
		{
			return new RestoreSelectionContext
			{
				Backup = new AvailableBackupInfo
				{
					BackupName = "Sample Image Backup",
					BackupType = "Clone to Disk",
					BackupPath = @"D:\Backups\SystemSample.ssb"
				},
				RestorePoint = new RestorePoint
				{
					DisplayName = "2026-08-15 10:30 (Full)",
					BackupType = "Full",
					FilePath = @"D:\Backups\SystemSample.ssb"
				},
				SelectedVolumes =
				[
					new VolumeInfo
					{
						ImageIndex = 1,
						Label = "Windows",
						FileSystem = "NTFS",
						Size = 240L * 1024 * 1024 * 1024,
						UsedSpace = 120L * 1024 * 1024 * 1024,
						SourceVolumeMountPath = "C:\\",
						PartitionNumber = 3,
						IsBootVolume = true
					}
				]
			};
		}

		private void PopulateDesignTimeState()
		{
			PopulateRestoreModes();
			selectedTargetLabel.Text = "Selected target: Disk 1";
			targetListView.Items.Clear();
			var item = new ListViewItem("Disk");
			item.SubItems.Add("Disk 1");
			item.SubItems.Add("\\.\\PhysicalDrive1");
			item.SubItems.Add("512 GB");
			item.SubItems.Add("Ready for restore");
			targetListView.Items.Add(item);
			if (restoreModeComboBox.Items.Count > 0)
			{
				restoreModeComboBox.SelectedIndex = 0;
			}
			UpdatePanels();
			UpdateActionState();
		}

		private async Task InitializeAsync()
		{
			await LoadBackupStateAsync();
			PopulateRestoreModes();
			LoadAvailableHyperVVirtualMachines();
			LoadNonRunningHyperVVms();
			ApplyHyperVDefaults();
			await LoadRestoreTargetsAsync();
			UpdatePanels();
			UpdateActionState();
		}

		private string BuildSummaryText()
		{
			string scopeText = restoreSelection.ScopeKind switch
			{
				RestoreScopeKind.All => "Scope: all items from the selected restore point.",
				RestoreScopeKind.SelectedVolumes => $"Scope: {restoreSelection.SelectedVolumes.Count} selected volume(s).",
				RestoreScopeKind.SelectedItems => $"Scope: {restoreSelection.SelectedItems.Count} selected item(s).",
				_ => "Scope: restore selection loaded."
			};

			return $"Backup: {restoreSelection.Backup.BackupName} ({restoreSelection.Backup.BackupType}){Environment.NewLine}" +
				   $"Restore point: {restoreSelection.RestorePoint.DisplayName}{Environment.NewLine}" +
				   scopeText;
		}

		private async Task LoadBackupStateAsync()
		{
			isHyperVBackupPoint = RestoreWindowNew.HyperVRestorePointHelper.IsHyperVBackupPoint(restoreSelection.RestorePoint.FilePath);

			await Task.Run(() =>
			{
				try
				{
					using var preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms((IWin32Window)this, restoreSelection.RestorePoint.FilePath, Path.GetFileNameWithoutExtension(restoreSelection.RestorePoint.FilePath), restoreSelection.Backup.ProtectedEncryptionPassword);
					var planResult = NativeBackupMountManager.BuildDiskRestorePlan(preparedBackup.WorkingPath);
					diskRestorePlan = planResult.Success ? planResult.Plan : null;
				}
				catch
				{
					diskRestorePlan = null;
				}
			});
		}

		private void PopulateRestoreModes()
		{
			restoreModeComboBox.Items.Clear();

			RestoreTargetKind detectedTargetKind = DetermineInitialRestoreTargetKind();
			if (isHyperVBackupPoint)
			{
				restoreModeComboBox.Items.Add(new RestoreModeOption("Disk", RestoreTargetKind.Disk));
				restoreModeComboBox.Items.Add(new RestoreModeOption("Volume", RestoreTargetKind.Volume));
				restoreModeComboBox.Items.Add(new RestoreModeOption("Hyper-V VM", RestoreTargetKind.HyperVVm));
			}
			else
			{
				restoreModeComboBox.Items.Add(new RestoreModeOption("Disk", RestoreTargetKind.Disk));
				restoreModeComboBox.Items.Add(new RestoreModeOption("Volume", RestoreTargetKind.Volume));
				restoreModeComboBox.Items.Add(new RestoreModeOption("Hyper-V Virtual Disk", RestoreTargetKind.HyperVVirtualDisk));
			}

			int selectedIndex = 0;
			for (int i = 0; i < restoreModeComboBox.Items.Count; i++)
			{
				if (restoreModeComboBox.Items[i] is RestoreModeOption option && option.TargetKind == detectedTargetKind)
				{
					selectedIndex = i;
					break;
				}
			}

			restoreModeComboBox.SelectedIndex = selectedIndex;
		}

		private RestoreTargetKind DetermineInitialRestoreTargetKind()
		{
			if (diskRestorePlan?.HasMetadata == true)
			{
				return diskRestorePlan.Volumes.Count > 1 ? RestoreTargetKind.Disk : RestoreTargetKind.Volume;
			}

			IReadOnlyList<string> backupItems = RestoreWorkflowHelper.GetBackupItemsForRestorePoint(restoreSelection.RestorePoint);
			return RestoreWorkflowHelper.DetermineRestoreTargetKind(
				restoreSelection.RestorePoint.BackupType,
				restoreSelection.RestorePoint.FilePath,
				backupItems);
		}

		private async Task LoadRestoreTargetsAsync()
		{
			restoreTargets.Clear();
			targetListView.BeginUpdate();
			targetListView.Items.Clear();
			try
			{
				await Task.Run(() =>
				{
					HashSet<int> protectedDisks = new HashSet<int>(GetProtectedDiskIndexes());
					using var diskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive ORDER BY Index");
					foreach (ManagementObject disk in diskSearcher.Get())
					{
						if (!int.TryParse(disk["Index"]?.ToString(), out int diskIndex))
						{
							continue;
						}

						bool isProtected = protectedDisks.Contains(diskIndex);
						string model = disk["Model"]?.ToString()?.Trim() ?? $"Disk {diskIndex}";
						long diskSize = 0;
						try
						{
							diskSize = Convert.ToInt64(disk["Size"] ?? 0);
						}
						catch
						{
						}

						if (!isProtected)
						{
							restoreTargets.Add(new TargetChoice
							{
								ItemType = DriveTreeItemType.Disk,
								DiskNumber = diskIndex,
								Name = $"Disk {diskIndex} — {model}",
								Path = $@"\\.\PHYSICALDRIVE{diskIndex}",
								Size = diskSize,
								Notes = string.Empty
							});
						}

						LoadDiskVolumes(diskIndex, isProtected);
					}
				});

				foreach (TargetChoice target in restoreTargets)
				{
					var item = new ListViewItem(target.ItemType == DriveTreeItemType.Disk ? "Disk" : "Volume");
					item.SubItems.Add(target.Name);
					item.SubItems.Add(target.Path);
					item.SubItems.Add(FormatSize(target.Size));
					item.SubItems.Add(target.Notes);
					item.Tag = target;
					targetListView.Items.Add(item);
				}
			}
			finally
			{
				targetListView.EndUpdate();
			}
			HandleTargetSelectionChanged();
		}

		private void LoadDiskVolumes(int diskIndex, bool isProtected)
		{
			try
			{
				using var partSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_DiskPartition WHERE DiskIndex = {diskIndex}");
				foreach (ManagementObject partition in partSearcher.Get())
				{
					string? partId = partition["DeviceID"]?.ToString();
					if (string.IsNullOrWhiteSpace(partId))
					{
						continue;
					}

					using var logSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partId}'}} WHERE AssocClass=Win32_LogicalDiskToPartition");
					bool hasLogical = false;
					foreach (ManagementObject logical in logSearcher.Get())
					{
						hasLogical = true;
						string? driveLetter = logical["DeviceID"]?.ToString();
						if (string.IsNullOrWhiteSpace(driveLetter))
						{
							continue;
						}

						try
						{
							var driveInfo = new DriveInfo(driveLetter);
							if (!driveInfo.IsReady)
							{
								continue;
							}

							bool isBootVolume = driveInfo.RootDirectory.FullName.TrimEnd('\\').Equals(
								(Path.GetPathRoot(Environment.SystemDirectory) ?? string.Empty).TrimEnd('\\'),
								StringComparison.OrdinalIgnoreCase);
							if (isProtected || isBootVolume)
							{
								continue;
							}

							restoreTargets.Add(new TargetChoice
							{
								ItemType = DriveTreeItemType.Volume,
								DiskNumber = diskIndex,
								Name = $"{driveInfo.Name.TrimEnd('\\')} ({(string.IsNullOrWhiteSpace(driveInfo.VolumeLabel) ? "Local Disk" : driveInfo.VolumeLabel)})",
								Path = driveInfo.Name,
								Size = driveInfo.TotalSize,
								Notes = string.Empty
							});
						}
						catch
						{
						}
					}

					if (!hasLogical && showHiddenPartitions && !isProtected)
					{
						long partitionSize = 0;
						try
						{
							partitionSize = Convert.ToInt64(partition["Size"] ?? 0);
						}
						catch
						{
						}

						restoreTargets.Add(new TargetChoice
						{
							ItemType = DriveTreeItemType.Volume,
							DiskNumber = diskIndex,
							Name = partition["Type"]?.ToString() ?? "Hidden Partition",
							Path = string.Empty,
							Size = partitionSize,
							Notes = "Hidden"
						});
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"LoadDiskVolumes warning: {ex.Message}");
			}
		}

		private void UpdateRestoreModeFromSelection()
		{
			if (restoreModeComboBox.SelectedItem is RestoreModeOption option)
			{
				restoreTargetKind = option.TargetKind;
			}
			UpdatePanels();
			UpdateActionState();
		}

		private void UpdatePanels()
		{
			modeHelpLabel.Text = restoreTargetKind switch
			{
				RestoreTargetKind.Disk => "Disk restore: choose a non-boot target disk. The selected disk will be repartitioned and all data on it will be lost.",
				RestoreTargetKind.Volume => "Volume restore: choose a non-boot target disk or volume. The selected destination will be formatted before restore.",
				RestoreTargetKind.HyperVVm => "Hyper-V VM restore: restore the selected Hyper-V backup point into a Hyper-V virtual machine destination.",
				RestoreTargetKind.HyperVVirtualDisk => "Hyper-V virtual disk restore: create or update a .vhdx target, then optionally attach it to an existing VM or create a new VM.",
				_ => string.Empty
			};

			targetListPanel.Visible = restoreTargetKind == RestoreTargetKind.Disk || restoreTargetKind == RestoreTargetKind.Volume;
			hyperVVmPanel.Visible = restoreTargetKind == RestoreTargetKind.HyperVVm;
			hyperVVirtualDiskPanel.Visible = restoreTargetKind == RestoreTargetKind.HyperVVirtualDisk;
			showHiddenCheckBox.Visible = targetListPanel.Visible;
			UpdateHyperVUiState();
		}

		private Panel BuildHyperVVmPanel()
		{
			var panel = new Panel
			{
				Dock = DockStyle.Fill,
				Visible = false
			};
			var layout = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				ColumnCount = 3,
				Padding = new Padding(0, 8, 0, 0)
			};
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));

			hyperVVmActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hyperVVmActionComboBox.Width = 260;
			hyperVVmActionComboBox.Items.AddRange(new object[]
			{
				"Restore to empty directory",
				"Replace existing non-running VM"
			});
			hyperVVmActionComboBox.SelectedIndex = 0;
			hyperVVmActionComboBox.SelectedIndexChanged += (_, _) => UpdateHyperVUiState();
			AddRow(layout, 0, "Hyper-V VM Action:", hyperVVmActionComboBox);

			hyperVVmReplaceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hyperVVmReplaceComboBox.Width = 320;
			AddRow(layout, 1, "VM to Replace:", hyperVVmReplaceComboBox);

			hyperVVmNameTextBox.Width = 320;
			AddRow(layout, 2, "Restored VM Name:", hyperVVmNameTextBox);

			hyperVRestoreDirectoryTextBox.Width = 520;
			var browseDirectoryButton = new Button
			{
				Text = "Browse...",
				AutoSize = true
			};
			browseDirectoryButton.Click += (_, _) => BrowseHyperVRestoreDirectory();
			AddRow(layout, 3, "Restore Directory:", hyperVRestoreDirectoryTextBox, browseDirectoryButton);

			startHyperVVmCheckBox.Text = "Start VM after restore";
			startHyperVVmCheckBox.AutoSize = true;
			layout.Controls.Add(startHyperVVmCheckBox, 1, 4);
			layout.SetColumnSpan(startHyperVVmCheckBox, 2);

			panel.Controls.Add(layout);
			return panel;
		}

		private Panel BuildHyperVVirtualDiskPanel()
		{
			var panel = new Panel
			{
				Dock = DockStyle.Fill,
				Visible = false
			};
			var layout = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				ColumnCount = 3,
				Padding = new Padding(0, 8, 0, 0)
			};
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));

			hyperVVirtualDiskPathTextBox.Width = 520;
			var browseVhdxButton = new Button
			{
				Text = "Browse...",
				AutoSize = true
			};
			browseVhdxButton.Click += (_, _) => BrowseHyperVVirtualDiskPath();
			AddRow(layout, 0, "Virtual Disk Path:", hyperVVirtualDiskPathTextBox, browseVhdxButton);

			hyperVDiskAttachModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hyperVDiskAttachModeComboBox.Width = 280;
			hyperVDiskAttachModeComboBox.Items.AddRange(new object[]
			{
				"Do not attach",
				"Attach to existing VM",
				"Create new VM"
			});
			hyperVDiskAttachModeComboBox.SelectedIndex = 0;
			hyperVDiskAttachModeComboBox.SelectedIndexChanged += (_, _) => UpdateHyperVUiState();
			AddRow(layout, 1, "After Restore:", hyperVDiskAttachModeComboBox);

			existingHyperVVmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			existingHyperVVmComboBox.Width = 320;
			AddRow(layout, 2, "Existing VM:", existingHyperVVmComboBox);

			newHyperVVmNameTextBox.Width = 320;
			AddRow(layout, 3, "New VM Name:", newHyperVVmNameTextBox);

			newHyperVVmPathTextBox.Width = 520;
			var browseVmPathButton = new Button
			{
				Text = "Browse...",
				AutoSize = true
			};
			browseVmPathButton.Click += (_, _) => BrowseNewHyperVVmLocation();
			AddRow(layout, 4, "New VM Folder:", newHyperVVmPathTextBox, browseVmPathButton);

			newHyperVGenerationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			newHyperVGenerationComboBox.Width = 120;
			newHyperVGenerationComboBox.Items.Add(new GenerationOption("Generation 1", 1));
			newHyperVGenerationComboBox.Items.Add(new GenerationOption("Generation 2", 2));
			newHyperVGenerationComboBox.SelectedIndex = 1;
			AddRow(layout, 5, "VM Generation:", newHyperVGenerationComboBox);

			startCreatedHyperVVmCheckBox.Text = "Start created VM after restore";
			startCreatedHyperVVmCheckBox.AutoSize = true;
			layout.Controls.Add(startCreatedHyperVVmCheckBox, 1, 6);
			layout.SetColumnSpan(startCreatedHyperVVmCheckBox, 2);

			panel.Controls.Add(layout);
			return panel;
		}

		private static void AddRow(TableLayoutPanel layout, int rowIndex, string labelText, Control valueControl, Control? actionControl = null)
		{
			while (layout.RowCount <= rowIndex)
			{
				layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				layout.RowCount++;
			}

			var label = new Label
			{
				Text = labelText,
				AutoSize = true,
				Margin = new Padding(0, 8, 8, 8)
			};
			layout.Controls.Add(label, 0, rowIndex);
			layout.Controls.Add(valueControl, 1, rowIndex);
			if (actionControl != null)
			{
				layout.Controls.Add(actionControl, 2, rowIndex);
			}
		}

		private void UpdateHyperVUiState()
		{
			bool replaceVm = hyperVVmActionComboBox.SelectedIndex == 1;
			hyperVVmReplaceComboBox.Enabled = replaceVm;
			hyperVRestoreDirectoryTextBox.Enabled = !replaceVm;

			bool attachExisting = hyperVDiskAttachModeComboBox.SelectedIndex == 1;
			bool createNew = hyperVDiskAttachModeComboBox.SelectedIndex == 2;
			existingHyperVVmComboBox.Enabled = attachExisting;
			newHyperVVmNameTextBox.Enabled = createNew;
			newHyperVVmPathTextBox.Enabled = createNew;
			newHyperVGenerationComboBox.Enabled = createNew;
			startCreatedHyperVVmCheckBox.Enabled = createNew;
		}

		private void LoadAvailableHyperVVirtualMachines()
		{
			existingHyperVVmComboBox.Items.Clear();
			try
			{
				var buffer = new StringBuilder(32768);
				int result = BackupEngineInterop.EnumerateHyperVMachines(buffer, buffer.Capacity);
				if (result != 0)
				{
					return;
				}

				foreach (string vm in buffer.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries))
				{
					existingHyperVVmComboBox.Items.Add(vm.Trim());
				}

				if (existingHyperVVmComboBox.Items.Count > 0)
				{
					existingHyperVVmComboBox.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"LoadAvailableHyperVVirtualMachines warning: {ex.Message}");
			}
		}

		private void LoadNonRunningHyperVVms()
		{
			hyperVVmReplaceComboBox.Items.Clear();
			try
			{
				string script = "Get-VM | Where-Object { $_.State -notin @('Running','Paused') } | Select-Object -ExpandProperty Name";
				var process = Process.Start(new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				});

				string output = process?.StandardOutput.ReadToEnd() ?? string.Empty;
				process?.WaitForExit();
				foreach (string vm in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
				{
					string trimmed = vm.Trim();
					if (!string.IsNullOrWhiteSpace(trimmed))
					{
						hyperVVmReplaceComboBox.Items.Add(trimmed);
					}
				}

				if (hyperVVmReplaceComboBox.Items.Count > 0)
				{
					hyperVVmReplaceComboBox.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"LoadNonRunningHyperVVms warning: {ex.Message}");
			}
		}

		private void ApplyHyperVDefaults()
		{
			if (isHyperVBackupPoint)
			{
				hyperVVmNameTextBox.Text = RestoreWindowNew.HyperVRestorePointHelper.ResolveVmName(restoreSelection.RestorePoint.FilePath);
			}

			if (string.IsNullOrWhiteSpace(hyperVVirtualDiskPathTextBox.Text))
			{
				string defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				hyperVVirtualDiskPathTextBox.Text = RestoreWindowNew.RegularHyperVRestoreHelper.BuildDefaultHyperVVirtualDiskPath(defaultDirectory, restoreSelection.Backup.BackupName);
			}

			string virtualDiskPath = hyperVVirtualDiskPathTextBox.Text.Trim();
			if (string.IsNullOrWhiteSpace(newHyperVVmNameTextBox.Text) && !string.IsNullOrWhiteSpace(virtualDiskPath))
			{
				newHyperVVmNameTextBox.Text = RestoreWindowNew.RegularHyperVRestoreHelper.GetDefaultHyperVVmName(virtualDiskPath);
			}
			if (string.IsNullOrWhiteSpace(newHyperVVmPathTextBox.Text) && !string.IsNullOrWhiteSpace(virtualDiskPath))
			{
				newHyperVVmPathTextBox.Text = RestoreWindowNew.RegularHyperVRestoreHelper.GetDefaultHyperVVmStoragePath(virtualDiskPath);
			}
		}

		private void HandleTargetSelectionChanged()
		{
			if (targetListView.SelectedItems.Count == 0 || targetListView.SelectedItems[0].Tag is not TargetChoice target)
			{
				selectedTargetDiskNumber = null;
				selectedTargetPath = null;
				selectedTargetLabel.Text = "No target selected";
				UpdateActionState();
				return;
			}

			selectedTargetDiskNumber = target.DiskNumber;
			selectedTargetPath = target.ItemType == DriveTreeItemType.Disk ? target.Path : EnsureTrailingSlash(target.Path);
			selectedTargetLabel.Text = $"Selected: {target.Name}";
			UpdateActionState();
		}

		private void UpdateActionState()
		{
			bool hasTarget = restoreTargetKind switch
			{
				RestoreTargetKind.Disk or RestoreTargetKind.Volume => !string.IsNullOrWhiteSpace(selectedTargetPath) || selectedTargetDiskNumber.HasValue,
				RestoreTargetKind.HyperVVm => ValidateHyperVVmInputs(showMessage: false),
				RestoreTargetKind.HyperVVirtualDisk => ValidateHyperVVirtualDiskInputs(showMessage: false),
				_ => false
			};

			startRestoreButton.Text = restoreTargetKind is RestoreTargetKind.Disk or RestoreTargetKind.Volume ? "Next" : "Start Restore";
			startRestoreButton.Enabled = hasTarget;
		}

		private async Task StartRestoreAsync()
		{
			if (!ValidateRestore())
			{
				return;
			}

			if ((restoreTargetKind == RestoreTargetKind.Disk || restoreTargetKind == RestoreTargetKind.Volume) &&
				!await PromptVolumeSelectionAndSizingAsync())
			{
				return;
			}

			if (restoreTargetKind == RestoreTargetKind.Disk)
			{
				if (MessageBox.Show(this, "WARNING: The selected target disk will be formatted and repartitioned. ALL DATA ON THE TARGET DISK WILL BE LOST.\n\nDo you want to continue?", "Confirm Disk Format", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
				{
					return;
				}
			}
			else if (restoreTargetKind == RestoreTargetKind.Volume)
			{
				if (MessageBox.Show(this, "WARNING: The selected target volume will be formatted. ALL DATA ON THE TARGET VOLUME WILL BE LOST.\n\nDo you want to continue?", "Confirm Volume Format", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
				{
					return;
				}
			}

			bool keepWindowOpen = RestoreWorkflowHelper.ShouldKeepRestoreCompletionWindowOpen(
				restoreTargetKind,
				restoreSelection.RequireAlternateDestination,
				selectedRestoreVolume,
				selectedRestoreDiskGroup);

			using var progressForm = new RestoreProgressForm(
				restoreSelection.RestorePoint.DisplayName,
				keepWindowOpen,
				callback => PerformRestoreAsync(callback));

			Hide();
			try
			{
				progressForm.ShowDialog(this);
				if (progressForm.RestoreSucceeded)
				{
					DialogResult = DialogResult.OK;
					Close();
				}
			}
			finally
			{
				if (!IsDisposed && Visible == false)
				{
					Show();
				}
			}
		}

		private bool ValidateRestore()
		{
			if (restoreTargetKind == RestoreTargetKind.Disk && !selectedTargetDiskNumber.HasValue)
			{
				MessageBox.Show(this, "Please select the target disk to restore to.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			if (restoreTargetKind == RestoreTargetKind.Volume && string.IsNullOrWhiteSpace(selectedTargetPath) && !selectedTargetDiskNumber.HasValue)
			{
				MessageBox.Show(this, "Please select the target disk or volume to restore to.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			if (restoreSelection.RequireAlternateDestination && selectedTargetDiskNumber.HasValue)
			{
				HashSet<int> protectedDisks = new HashSet<int>(GetProtectedDiskIndexes());
				if (protectedDisks.Contains(selectedTargetDiskNumber.Value))
				{
					MessageBox.Show(this, "Please select a restore destination that is not on the currently booted drive.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}
			}

			if (restoreTargetKind == RestoreTargetKind.HyperVVm)
			{
				return ValidateHyperVVmInputs(showMessage: true);
			}

			if (restoreTargetKind == RestoreTargetKind.HyperVVirtualDisk)
			{
				return ValidateHyperVVirtualDiskInputs(showMessage: true);
			}

			if (isHyperVBackupPoint && (restoreTargetKind == RestoreTargetKind.Disk || restoreTargetKind == RestoreTargetKind.Volume) &&
				RestoreWindowNew.HyperVRestorePointHelper.FindPrimaryVirtualDisk(restoreSelection.RestorePoint.FilePath) == null)
			{
				MessageBox.Show(this, "The selected Hyper-V backup point does not contain a guest VHD or VHDX file that can be restored to a disk or volume target.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			return true;
		}

		private bool ValidateHyperVVmInputs(bool showMessage)
		{
			bool replaceMode = hyperVVmActionComboBox.SelectedIndex == 1;
			if (replaceMode)
			{
				if (hyperVVmReplaceComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(hyperVVmReplaceComboBox.SelectedItem.ToString()))
				{
					if (showMessage)
					{
						MessageBox.Show(this, "Please select a non-running Hyper-V virtual machine to replace.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return false;
				}
			}
			else
			{
				string restoreDirectory = hyperVRestoreDirectoryTextBox.Text.Trim();
				if (string.IsNullOrWhiteSpace(restoreDirectory))
				{
					if (showMessage)
					{
						MessageBox.Show(this, "Please select an empty directory to restore the Hyper-V virtual machine into.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return false;
				}
				if (Directory.Exists(restoreDirectory) && Directory.EnumerateFileSystemEntries(restoreDirectory).Any())
				{
					if (showMessage)
					{
						MessageBox.Show(this, "The selected restore directory is not empty. Please choose an empty directory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return false;
				}
			}

			if (string.IsNullOrWhiteSpace(hyperVVmNameTextBox.Text))
			{
				if (showMessage)
				{
					MessageBox.Show(this, "Please enter a virtual machine name for the restored VM.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				return false;
			}

			return true;
		}

		private bool ValidateHyperVVirtualDiskInputs(bool showMessage)
		{
			if (string.IsNullOrWhiteSpace(hyperVVirtualDiskPathTextBox.Text))
			{
				if (showMessage)
				{
					MessageBox.Show(this, "Please select the Hyper-V virtual disk file to create or update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				return false;
			}

			if (hyperVDiskAttachModeComboBox.SelectedIndex == 1)
			{
				string selectedVm = existingHyperVVmComboBox.SelectedItem?.ToString() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(selectedVm))
				{
					if (showMessage)
					{
						MessageBox.Show(this, "Please select the existing Hyper-V virtual machine that should receive the restored virtual disk.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return false;
				}
			}
			else if (hyperVDiskAttachModeComboBox.SelectedIndex == 2)
			{
				if (string.IsNullOrWhiteSpace(newHyperVVmNameTextBox.Text))
				{
					if (showMessage)
					{
						MessageBox.Show(this, "Please enter the name for the new Hyper-V virtual machine.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return false;
				}

				if (string.IsNullOrWhiteSpace(newHyperVVmPathTextBox.Text))
				{
					if (showMessage)
					{
						MessageBox.Show(this, "Please select the storage folder for the new Hyper-V virtual machine.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return false;
				}
			}

			return true;
		}

		private async Task<bool> PromptVolumeSelectionAndSizingAsync()
		{
			if (restoreTargetKind != RestoreTargetKind.Disk && restoreTargetKind != RestoreTargetKind.Volume)
			{
				return true;
			}

			if (restoreTargetKind == RestoreTargetKind.Disk && !EnsureDiskRestorePlanLoaded())
			{
				if (await TryPromptSingleVolumeFallbackSizingAsync())
				{
					return true;
				}

				MessageBox.Show(this, "This disk backup does not contain the reconstruction metadata required to size partitions and rebuild the target disk. Disk restore cannot continue.", "Restore Metadata Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			if (diskRestorePlan == null || !diskRestorePlan.HasMetadata)
			{
				if (restoreTargetKind == RestoreTargetKind.Volume)
				{
					await TryPromptSingleVolumeFallbackSizingAsync();
				}
				return true;
			}

			List<VolumeInfo> volumes = diskRestorePlan.Volumes
				.OrderBy(v => v.PartitionOffsetBytes)
				.ThenBy(v => v.PartitionNumber)
				.Select((v, idx) => new VolumeInfo
				{
					ImageIndex = v.ImageIndex > 0 ? v.ImageIndex : idx + 1,
					Label = !string.IsNullOrWhiteSpace(v.SourceVolumeLabel) ? v.SourceVolumeLabel : $"Volume {v.PartitionNumber}",
					Size = (long)v.PartitionLengthBytes,
					UsedSpace = 0,
					PartitionNumber = v.PartitionNumber,
					PartitionOffsetBytes = v.PartitionOffsetBytes,
					PartitionLengthBytes = v.PartitionLengthBytes,
					PartitionStyle = v.PartitionStyle,
					PartitionType = v.PartitionType,
					SourceVolumeGuidPath = v.SourceVolumeGuidPath,
					SourceVolumeMountPath = v.SourceVolumeMountPath,
					IsBootVolume = v.IsBootVolume,
					IsSystemVolume = v.IsSystemVolume,
					FileSystem = v.SourceFileSystem,
					IsResizable = true,
					TargetSize = (long)v.PartitionLengthBytes
				})
				.ToList();

			IReadOnlyList<VolumeInfo> volumesToResize;
			bool isGroupRestore;
			if (restoreTargetKind == RestoreTargetKind.Volume && volumes.Count == 1)
			{
				volumesToResize = volumes;
				isGroupRestore = false;
				selectedRestoreVolume = volumes[0];
				selectedRestoreDiskGroup = null;
			}
			else
			{
				using var selectionDialog = new RestoreVolumeSelectionForm(
					volumes,
					isDiskOrHyperVBackup: true,
					restoreTargetKind == RestoreTargetKind.Disk
						? "This backup contains multiple volumes. Select a single volume or the entire disk group to restore."
						: "Select the volume to restore from this backup.");
				if (selectionDialog.ShowDialog(this) != DialogResult.OK || !selectionDialog.Confirmed)
				{
					return false;
				}

				isGroupRestore = selectionDialog.SelectedDiskGroup != null;
				volumesToResize = isGroupRestore
					? selectionDialog.SelectedDiskGroup!
					: new[] { selectionDialog.SelectedVolume! };
			}

			long targetDiskSizeBytes = await GetTargetDiskSizeBytesAsync();
			if (targetDiskSizeBytes <= 0)
			{
				if (isGroupRestore)
				{
					selectedRestoreDiskGroup = volumesToResize.ToList();
					selectedRestoreVolume = null;
				}
				else
				{
					selectedRestoreVolume = volumesToResize.FirstOrDefault();
					selectedRestoreDiskGroup = null;
				}
				return true;
			}

			using var sizingForm = new VolumeConfigurationForm(volumesToResize.ToList(), targetDiskSizeBytes, 4096, 4096);
			if (sizingForm.ShowDialog(this) != DialogResult.OK || sizingForm.FinalConfiguration == null)
			{
				return false;
			}

			List<VolumeInfo> resized = sizingForm.FinalConfiguration
				.OrderBy(v => v.PartitionOffsetBytes)
				.ThenBy(v => v.PartitionNumber)
				.ToList();

			if (isGroupRestore)
			{
				selectedRestoreDiskGroup = resized;
				selectedRestoreVolume = null;
			}
			else
			{
				selectedRestoreVolume = resized.FirstOrDefault();
				selectedRestoreDiskGroup = null;
			}

			return true;
		}

		private async Task<bool> TryPromptSingleVolumeFallbackSizingAsync()
		{
			if (restoreTargetKind != RestoreTargetKind.Disk && restoreTargetKind != RestoreTargetKind.Volume)
			{
				return false;
			}

			try
			{
				using var preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms((IWin32Window)this, restoreSelection.RestorePoint.FilePath, Path.GetFileNameWithoutExtension(restoreSelection.RestorePoint.FilePath), restoreSelection.Backup.ProtectedEncryptionPassword);
				var imageInfoResult = NativeBackupMountManager.GetImageInfoWithRestoreMetadata(preparedBackup.WorkingPath);
				if (!imageInfoResult.Success || imageInfoResult.Images.Count != 1)
				{
					return false;
				}

				long targetCapacityBytes = await GetTargetDiskSizeBytesAsync();
				if (targetCapacityBytes <= 0)
				{
					return false;
				}

				var fallbackVolume = CreateSingleVolumeFallbackVolumeInfo(imageInfoResult.Images[0], targetCapacityBytes);
				using var sizingForm = new VolumeConfigurationForm(new List<VolumeInfo> { fallbackVolume }, targetCapacityBytes, 4096, 4096);
				if (sizingForm.ShowDialog(this) != DialogResult.OK || sizingForm.FinalConfiguration == null)
				{
					return false;
				}

				selectedRestoreVolume = sizingForm.FinalConfiguration.FirstOrDefault() ?? fallbackVolume;
				selectedRestoreDiskGroup = null;
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"TryPromptSingleVolumeFallbackSizingAsync exception: {ex.Message}");
				return false;
			}
		}

		private static VolumeInfo CreateSingleVolumeFallbackVolumeInfo(NativeBackupMountManager.SsbImageInfoResult imageInfo, long targetCapacityBytes)
		{
			long capacityBytes = Math.Max(targetCapacityBytes, 1);
			string label = !string.IsNullOrWhiteSpace(imageInfo.Name) ? imageInfo.Name : "Volume 1";
			return new VolumeInfo
			{
				ImageIndex = imageInfo.ImageIndex > 0 ? imageInfo.ImageIndex : 1,
				Label = label,
				Size = capacityBytes,
				UsedSpace = 0,
				IsResizable = true,
				FileSystem = "NTFS",
				PartitionNumber = 1,
				TargetSize = capacityBytes
			};
		}

		private bool EnsureDiskRestorePlanLoaded()
		{
			if (diskRestorePlan?.HasMetadata == true)
			{
				return true;
			}

			try
			{
				using var preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms((IWin32Window)this, restoreSelection.RestorePoint.FilePath, Path.GetFileNameWithoutExtension(restoreSelection.RestorePoint.FilePath), restoreSelection.Backup.ProtectedEncryptionPassword);
				var planResult = NativeBackupMountManager.BuildDiskRestorePlan(preparedBackup.WorkingPath);
				diskRestorePlan = planResult.Success ? planResult.Plan : null;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"EnsureDiskRestorePlanLoaded exception: {ex.Message}");
			}

			return diskRestorePlan?.HasMetadata == true;
		}

		private async Task<long> GetTargetDiskSizeBytesAsync()
		{
			if (selectedTargetDiskNumber.HasValue)
			{
				return await Task.Run(() => GetTargetDiskCapacityBytes(selectedTargetDiskNumber.Value));
			}

			if (!string.IsNullOrWhiteSpace(selectedTargetPath))
			{
				return await Task.Run(() =>
				{
					try
					{
						string drive = Path.GetPathRoot(selectedTargetPath)?.TrimEnd('\\') ?? string.Empty;
						if (string.IsNullOrWhiteSpace(drive))
						{
							return -1L;
						}

						using var searcher = new ManagementObjectSearcher($"SELECT Size FROM Win32_LogicalDisk WHERE DeviceID = '{drive}'");
						foreach (ManagementObject volume in searcher.Get())
						{
							if (long.TryParse(volume["Size"]?.ToString(), out long size))
							{
								return size;
							}
						}
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"GetTargetDiskSizeBytesAsync (volume): {ex.Message}");
					}
					return -1L;
				});
			}

			return -1L;
		}

		private async Task PerformRestoreAsync(BackupEngineInterop.ProgressCallback callback)
		{
			await Task.Run(() =>
			{
				using var preparedBackup = EncryptedBackupFileService.PrepareForReadForWinForms((IWin32Window)this, restoreSelection.RestorePoint.FilePath, Path.GetFileNameWithoutExtension(restoreSelection.RestorePoint.FilePath), restoreSelection.Backup.ProtectedEncryptionPassword);
				int result;
				string destination = restoreTargetKind switch
				{
					RestoreTargetKind.Disk or RestoreTargetKind.Volume => selectedTargetPath ?? string.Empty,
					RestoreTargetKind.HyperVVm => hyperVRestoreDirectoryTextBox.Text.Trim(),
					RestoreTargetKind.HyperVVirtualDisk => hyperVVirtualDiskPathTextBox.Text.Trim(),
					_ => string.Empty
				};

				BackupLogger.LogInfo(RestoreLogJobName, "Restore started", $"Restore point: {restoreSelection.RestorePoint.DisplayName}; Type: {restoreTargetKind}; Source: {restoreSelection.RestorePoint.FilePath}; Destination: {destination}");

				switch (restoreTargetKind)
				{
					case RestoreTargetKind.Disk:
						result = RestoreDiskTarget(preparedBackup.WorkingPath, callback);
						break;
					case RestoreTargetKind.Volume:
						result = RestoreVolumeTarget(preparedBackup.WorkingPath, callback);
						break;
					case RestoreTargetKind.HyperVVm:
						result = ReplaceOrRestoreHyperVVm(preparedBackup.WorkingPath, callback);
						break;
					case RestoreTargetKind.HyperVVirtualDisk:
						RestoreToHyperVVirtualDisk(preparedBackup.WorkingPath, callback);
						result = 0;
						break;
					default:
						throw new InvalidOperationException("Unsupported restore target type.");
				}

				if (result != 0)
				{
					var error = new StringBuilder(1024);
					BackupEngineInterop.GetLastErrorMessage(error, error.Capacity);
					BackupLogger.LogError(RestoreLogJobName, "Restore failed", error.ToString());
					throw new InvalidOperationException($"Restore failed: {error}");
				}

				BackupLogger.LogSuccess(RestoreLogJobName, "Restore completed successfully", restoreSelection.RestorePoint.FilePath, $"Type: {restoreTargetKind}; Destination: {destination}");
			});
		}

		private int RestoreDiskTarget(string preparedBackupPath, BackupEngineInterop.ProgressCallback callback)
		{
			PrepareDiskTarget();
			int targetDisk = selectedTargetDiskNumber ?? -1;

			if (diskRestorePlan?.HasMetadata == true)
			{
				List<VolumeInfo> ordered = (selectedRestoreDiskGroup?.Count > 0
					? selectedRestoreDiskGroup
					: selectedRestoreVolume is not null
						? new[] { selectedRestoreVolume }
						: diskRestorePlan.Volumes.Select((v, idx) => new VolumeInfo
						{
							ImageIndex = v.ImageIndex > 0 ? v.ImageIndex : idx + 1,
							Label = !string.IsNullOrWhiteSpace(v.SourceVolumeLabel) ? v.SourceVolumeLabel : $"Volume {v.PartitionNumber}",
							Size = (long)v.PartitionLengthBytes,
							UsedSpace = 0,
							PartitionNumber = v.PartitionNumber,
							PartitionOffsetBytes = v.PartitionOffsetBytes,
							PartitionLengthBytes = v.PartitionLengthBytes,
							PartitionStyle = v.PartitionStyle,
							PartitionType = v.PartitionType,
							SourceVolumeGuidPath = v.SourceVolumeGuidPath,
							SourceVolumeMountPath = v.SourceVolumeMountPath,
							IsBootVolume = v.IsBootVolume,
							IsSystemVolume = v.IsSystemVolume,
							FileSystem = v.SourceFileSystem,
							IsResizable = true,
							TargetSize = (long)v.PartitionLengthBytes
						}))
					.OrderBy(v => v.PartitionOffsetBytes)
					.ThenBy(v => v.PartitionNumber)
					.ToList();

				if (ordered.Count == 0)
				{
					throw new InvalidOperationException("No restore volumes are available for the selected disk backup.");
				}

				List<string> targetVolumePaths = TryReuseExistingTargetVolumes(targetDisk, ordered) ?? PrepareDiskTargetVolumes(targetDisk, ordered);
				for (int i = 0; i < ordered.Count; i++)
				{
					VolumeInfo volume = ordered[i];
					callback((int)((i / (double)ordered.Count) * 90), $"Restoring partition {i + 1} of {ordered.Count}: {volume.Label}...");
					int lastResult = BackupEngineInterop.RestoreVolumeFromImage(preparedBackupPath, volume.ImageIndex, targetVolumePaths[i], false, callback);
					if (lastResult != 0)
					{
						return lastResult;
					}
				}

				callback(100, "All partitions restored.");
				return 0;
			}

			if (selectedRestoreVolume is not null)
			{
				List<string> targetVolumePaths = TryReuseExistingTargetVolumes(targetDisk, new[] { selectedRestoreVolume }) ?? PrepareDiskTargetVolumes(targetDisk, new[] { selectedRestoreVolume });
				int imageIndex = selectedRestoreVolume.ImageIndex > 0 ? selectedRestoreVolume.ImageIndex : 1;
				return BackupEngineInterop.RestoreVolumeFromImage(preparedBackupPath, imageIndex, targetVolumePaths[0], false, callback);
			}

			if (isHyperVBackupPoint)
			{
				using var mountedDisk = MountPrimaryHyperVVirtualDisk(restoreSelection.RestorePoint.FilePath);
				int mountedDiskNumber = GetDiskNumberForDriveLetter(mountedDisk.DriveRoot);
				int imageIndex = selectedRestoreVolume?.ImageIndex ?? diskRestorePlan?.ImageIndex ?? 1;
				return BackupEngineInterop.RestoreDiskFromImage(preparedBackupPath, imageIndex, mountedDiskNumber, false, callback);
			}

			int fallbackImageIndex = selectedRestoreVolume?.ImageIndex ?? diskRestorePlan?.ImageIndex ?? 1;
			bool isArchiveFile = string.Equals(Path.GetExtension(preparedBackupPath), ".ssb", StringComparison.OrdinalIgnoreCase);
			return isArchiveFile
				? BackupEngineInterop.RestoreDiskFromImage(preparedBackupPath, fallbackImageIndex, selectedTargetDiskNumber ?? -1, false, callback)
				: diskRestorePlan?.HasMetadata == true
					? BackupEngineInterop.RestoreDiskFromImage(preparedBackupPath, fallbackImageIndex, selectedTargetDiskNumber ?? -1, false, callback)
					: BackupEngineInterop.RestoreDisk(preparedBackupPath, selectedTargetDiskNumber ?? -1, false, callback);
		}

		private int RestoreVolumeTarget(string preparedBackupPath, BackupEngineInterop.ProgressCallback callback)
		{
			PrepareVolumeTarget();
			string targetPath = selectedTargetPath ?? string.Empty;

			if (isHyperVBackupPoint)
			{
				using var mountedDisk = MountPrimaryHyperVVirtualDisk(restoreSelection.RestorePoint.FilePath);
				targetPath = mountedDisk.DriveRoot;
				int imageIndex = selectedRestoreVolume?.ImageIndex ?? diskRestorePlan?.ImageIndex ?? 1;
				return BackupEngineInterop.RestoreVolumeFromImage(preparedBackupPath, imageIndex, targetPath, false, callback);
			}

			int configuredImageIndex = selectedRestoreVolume?.ImageIndex ?? diskRestorePlan?.ImageIndex ?? -1;
			return configuredImageIndex > 0
				? BackupEngineInterop.RestoreVolumeFromImage(preparedBackupPath, configuredImageIndex, targetPath, false, callback)
				: BackupEngineInterop.RestoreVolume(preparedBackupPath, targetPath, false, callback);
		}

		private int ReplaceOrRestoreHyperVVm(string preparedBackupPath, BackupEngineInterop.ProgressCallback callback)
		{
			string vmName = hyperVVmNameTextBox.Text.Trim();
			bool replaceMode = hyperVVmActionComboBox.SelectedIndex == 1;
			if (replaceMode)
			{
				string targetVmName = hyperVVmReplaceComboBox.SelectedItem?.ToString()?.Trim() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(targetVmName))
				{
					throw new InvalidOperationException("No target Hyper-V virtual machine was selected for replacement.");
				}

				callback(5, $"Removing existing Hyper-V virtual machine '{targetVmName}'...");
				RemoveHyperVVm(targetVmName);
				string vmStoragePath = GetHyperVVmStoragePath(targetVmName) ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Hyper-V", "Virtual Machines");
				callback(15, $"Importing Hyper-V backup as '{vmName}'...");
				return BackupEngineInterop.RestoreHyperVVM(preparedBackupPath, string.IsNullOrWhiteSpace(vmName) ? targetVmName : vmName, vmStoragePath, startHyperVVmCheckBox.Checked, callback);
			}

			string restoreDirectory = hyperVRestoreDirectoryTextBox.Text.Trim();
			Directory.CreateDirectory(restoreDirectory);
			callback(5, $"Restoring Hyper-V virtual machine to '{restoreDirectory}'...");
			return BackupEngineInterop.RestoreHyperVVM(preparedBackupPath, vmName, restoreDirectory, startHyperVVmCheckBox.Checked, callback);
		}

		private void RestoreToHyperVVirtualDisk(string preparedBackupPath, BackupEngineInterop.ProgressCallback callback)
		{
			string virtualDiskPath = hyperVVirtualDiskPathTextBox.Text.Trim();
			if (string.IsNullOrWhiteSpace(virtualDiskPath))
			{
				throw new InvalidOperationException("No Hyper-V virtual disk path was selected.");
			}

			Directory.CreateDirectory(Path.GetDirectoryName(virtualDiskPath) ?? throw new InvalidOperationException("Invalid Hyper-V virtual disk path."));
			bool restoreAsDisk = restoreTargetKind == RestoreTargetKind.Disk;

			callback(5, "Creating or preparing Hyper-V virtual disk...");
			PrepareHyperVVirtualDiskFile(virtualDiskPath, restoreAsDisk);
			callback(10, "Mounting Hyper-V virtual disk...");
			var mountResult = BackupMountManager.MountVirtualDisk(virtualDiskPath, readOnly: false);
			if (!mountResult.Success || string.IsNullOrWhiteSpace(mountResult.DriveLetter))
			{
				throw new InvalidOperationException($"Failed to mount the Hyper-V virtual disk: {mountResult.Error}");
			}

			string mountedDriveRoot = mountResult.DriveLetter.EndsWith(":", StringComparison.Ordinal)
				? mountResult.DriveLetter + "\\"
				: mountResult.DriveLetter;

			try
			{
				string targetVolumePath = restoreAsDisk
					? CreateVolumeOnDiskForHyperVRestore(GetDiskNumberForDriveLetter(mountedDriveRoot))
					: mountedDriveRoot;
				int imageIndex = diskRestorePlan?.ImageIndex ?? 1;
				int result = restoreAsDisk
					? (diskRestorePlan?.HasMetadata == true
						? BackupEngineInterop.RestoreDiskFromImage(preparedBackupPath, imageIndex, GetDiskNumberForDriveLetter(mountedDriveRoot), false, callback)
						: BackupEngineInterop.RestoreDisk(preparedBackupPath, GetDiskNumberForDriveLetter(mountedDriveRoot), false, callback))
					: BackupEngineInterop.RestoreVolumeFromImage(preparedBackupPath, imageIndex, targetVolumePath, false, callback);
				if (result != 0)
				{
					var error = new StringBuilder(1024);
					BackupEngineInterop.GetLastErrorMessage(error, error.Capacity);
					throw new InvalidOperationException($"Restore to Hyper-V virtual disk failed: {error}");
				}
			}
			finally
			{
				BackupMountManager.UnmountVirtualDisk(virtualDiskPath);
			}

			if (hyperVDiskAttachModeComboBox.SelectedIndex == 1)
			{
				string vmName = RestoreWindowNew.RegularHyperVRestoreHelper.NormalizeHyperVVmName(existingHyperVVmComboBox.SelectedItem?.ToString() ?? string.Empty);
				if (string.IsNullOrWhiteSpace(vmName))
				{
					throw new InvalidOperationException("No Hyper-V virtual machine was selected for disk attachment.");
				}

				callback(95, $"Attaching restored virtual disk to Hyper-V VM '{vmName}'...");
				AttachVirtualDiskToExistingHyperVVm(vmName, virtualDiskPath);
			}
			else if (hyperVDiskAttachModeComboBox.SelectedIndex == 2)
			{
				string vmName = newHyperVVmNameTextBox.Text.Trim();
				string vmStoragePath = newHyperVVmPathTextBox.Text.Trim();
				int generation = GetSelectedNewHyperVVmGeneration();
				callback(95, $"Creating Hyper-V VM '{vmName}'...");
				CreateNewHyperVVm(vmName, vmStoragePath, virtualDiskPath, generation, startCreatedHyperVVmCheckBox.Checked);
			}
		}

		private void PrepareDiskTarget()
		{
			if (!selectedTargetDiskNumber.HasValue)
			{
				throw new InvalidOperationException("No target disk selected.");
			}
		}

		private void PrepareVolumeTarget()
		{
			string volumePath = selectedTargetPath ?? string.Empty;
			if (string.IsNullOrWhiteSpace(volumePath))
			{
				throw new InvalidOperationException("No target volume selected.");
			}

			if (selectedRestoreVolume?.TargetSize > 0)
			{
				ResizeTargetVolumeIfNeeded(volumePath, selectedRestoreVolume.TargetSize);
			}

			FormatTargetVolume(volumePath, GetSelectedRestoreFileSystem(), selectedRestoreVolume?.Label);
		}

		private List<string>? TryReuseExistingTargetVolumes(int targetDiskNumber, IReadOnlyList<VolumeInfo> orderedVolumes)
		{
			if (orderedVolumes.Count == 0)
			{
				return null;
			}

			List<TargetChoice> targetVolumes = restoreTargets
				.Where(target => target.ItemType == DriveTreeItemType.Volume && target.DiskNumber == targetDiskNumber && !string.IsNullOrWhiteSpace(target.Path))
				.OrderBy(target => target.Name)
				.ToList();

			if (targetVolumes.Count == 0)
			{
				return null;
			}

			TargetChoice? diskTarget = restoreTargets.FirstOrDefault(target => target.ItemType == DriveTreeItemType.Disk && target.DiskNumber == targetDiskNumber);
			long targetDiskSize = diskTarget?.Size ?? GetTargetDiskCapacityBytes(targetDiskNumber);

			if (orderedVolumes.Count == 1 && targetVolumes.Count == 1)
			{
				long requestedSize = GetRequestedRestoreSize(orderedVolumes[0]);
				if (RestoreWindowNew.ShouldReuseExistingTargetVolumeLayout(requestedSize, targetVolumes[0].Size, targetDiskSize))
				{
					string targetPath = EnsureTrailingSlash(targetVolumes[0].Path);
					FormatTargetVolume(targetPath, GetSupportedRestoreFileSystem(orderedVolumes[0]), orderedVolumes[0].Label);
					return new List<string> { targetPath };
				}

				return null;
			}

			if (targetVolumes.Count != orderedVolumes.Count)
			{
				return null;
			}

			for (int i = 0; i < orderedVolumes.Count; i++)
			{
				if (!AreSizesEquivalent(GetRequestedRestoreSize(orderedVolumes[i]), targetVolumes[i].Size))
				{
					return null;
				}
			}

			var matchedPaths = new List<string>(orderedVolumes.Count);
			for (int i = 0; i < orderedVolumes.Count; i++)
			{
				string targetPath = EnsureTrailingSlash(targetVolumes[i].Path);
				FormatTargetVolume(targetPath, GetSupportedRestoreFileSystem(orderedVolumes[i]), orderedVolumes[i].Label);
				matchedPaths.Add(targetPath);
			}

			return matchedPaths;
		}

		private List<string> PrepareDiskTargetVolumes(int targetDiskNumber, IReadOnlyList<VolumeInfo> orderedVolumes)
		{
			string partitionStyle = orderedVolumes.Select(v => v.PartitionStyle).FirstOrDefault(style => !string.IsNullOrWhiteSpace(style)) ?? "GPT";
			long[] targetSizes = orderedVolumes.Select(volume => volume.TargetSize > 0 ? volume.TargetSize : volume.Size).ToArray();
			string[] fileSystems = orderedVolumes.Select(GetSupportedRestoreFileSystem).ToArray();
			string[] labels = orderedVolumes.Select(volume => string.IsNullOrWhiteSpace(volume.Label) ? $"Restore{volume.PartitionNumber}" : volume.Label).ToArray();
			string[] partitionTypes = orderedVolumes.Select(volume => volume.PartitionType ?? string.Empty).ToArray();
			long targetDiskCapacityBytes = GetTargetDiskCapacityBytes(targetDiskNumber);
			long requestedTotalBytes = targetSizes.Sum();
			bool expandLastPartition = RestoreWindowNew.ShouldExpandLastPartition(requestedTotalBytes, targetDiskCapacityBytes);

			string sizeArray = string.Join(",", targetSizes.Select(size => $"{size}L"));
			string fileSystemArray = string.Join(",", fileSystems.Select(value => $"'{EscapePowerShellSingleQuotedString(value)}'"));
			string labelArray = string.Join(",", labels.Select(value => $"'{EscapePowerShellSingleQuotedString(value)}'"));
			string partitionTypeArray = string.Join(",", partitionTypes.Select(value => $"'{EscapePowerShellSingleQuotedString(value)}'"));

			string script =
				$"$diskNumber={targetDiskNumber}; " +
				$"$partitionStyle='{EscapePowerShellSingleQuotedString(partitionStyle)}'; " +
				$"$sizes=@({sizeArray}); " +
				$"$fileSystems=@({fileSystemArray}); " +
				$"$labels=@({labelArray}); " +
				$"$partitionTypes=@({partitionTypeArray}); " +
				$"$expandLast={(expandLastPartition ? "$true" : "$false")}; " +
				"$created=@(); " +
				"Clear-Disk -Number $diskNumber -RemoveData -RemoveOEM -Confirm:$false -ErrorAction Stop; " +
				"Initialize-Disk -Number $diskNumber -PartitionStyle $partitionStyle -ErrorAction Stop; " +
				"for ($i = 0; $i -lt $sizes.Count; $i++) { " +
				"$partitionType = $partitionTypes[$i]; " +
				"$newPartitionParams = @{ DiskNumber = $diskNumber; AssignDriveLetter = $true; ErrorAction = 'Stop' }; " +
				"if ($i -eq $sizes.Count - 1 -and $expandLast) { $newPartitionParams['UseMaximumSize'] = $true; } else { $newPartitionParams['Size'] = [Int64]$sizes[$i]; } " +
				"if ($partitionStyle -eq 'GPT' -and -not [string]::IsNullOrWhiteSpace($partitionType)) { $newPartitionParams['GptType'] = $partitionType; } " +
				"$partition = New-Partition @newPartitionParams; " +
				"$fileSystem = $fileSystems[$i]; " +
				"if ([string]::IsNullOrWhiteSpace($fileSystem)) { $fileSystem = 'NTFS'; } " +
				"$label = $labels[$i]; " +
				"if ([string]::IsNullOrWhiteSpace($label)) { $label = 'SSBRestore'; } " +
				"Format-Volume -Partition $partition -FileSystem $fileSystem -NewFileSystemLabel $label -Confirm:$false -Force -ErrorAction Stop | Out-Null; " +
				"$driveLetter = ($partition | Get-Volume).DriveLetter; " +
				"if ([string]::IsNullOrWhiteSpace($driveLetter)) { throw 'Failed to assign a drive letter to a restored partition.'; } " +
				"$created += ($driveLetter + ':\\'); " +
				"}; " +
				"$created -join '|'";

			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});

			string output = process?.StandardOutput.ReadToEnd() ?? string.Empty;
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to partition and format the target disk. {errors}".Trim());
			}

			List<string> createdVolumes = output
				.Split('|', StringSplitOptions.RemoveEmptyEntries)
				.Select(path => path.Trim())
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.ToList();
			if (createdVolumes.Count != orderedVolumes.Count)
			{
				throw new InvalidOperationException("The target disk layout was created, but the number of formatted restore volumes did not match the selected layout.");
			}

			return createdVolumes;
		}

		private long GetTargetDiskCapacityBytes(int targetDiskNumber)
		{
			TargetChoice? diskTarget = restoreTargets.FirstOrDefault(target => target.ItemType == DriveTreeItemType.Disk && target.DiskNumber == targetDiskNumber);
			if (diskTarget != null && diskTarget.Size > 0)
			{
				return diskTarget.Size;
			}

			try
			{
				using var searcher = new ManagementObjectSearcher($"SELECT Size FROM Win32_DiskDrive WHERE Index = {targetDiskNumber}");
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

		private List<int> GetProtectedDiskIndexes()
		{
			var indexes = new List<int>();
			try
			{
				string systemRoot = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) ?? string.Empty;
				string osDriveLetter = systemRoot.TrimEnd('\\').TrimEnd(':');
				if (string.IsNullOrWhiteSpace(osDriveLetter))
				{
					return indexes;
				}

				using var ldSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{osDriveLetter}:'}} WHERE AssocClass=Win32_LogicalDiskToPartition");
				foreach (ManagementObject partition in ldSearcher.Get())
				{
					string? partId = partition["DeviceID"]?.ToString();
					if (string.IsNullOrWhiteSpace(partId))
					{
						continue;
					}

					using var ddSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partId}'}} WHERE AssocClass=Win32_DiskDriveToDiskPartition");
					foreach (ManagementObject drive in ddSearcher.Get())
					{
						if (int.TryParse(drive["Index"]?.ToString(), out int diskIndex) && !indexes.Contains(diskIndex))
						{
							indexes.Add(diskIndex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"GetProtectedDiskIndexes warning: {ex.Message}");
			}

			return indexes;
		}

		private sealed class MountedVirtualDiskScope : IDisposable
		{
			private readonly string virtualDiskPath;
			private bool disposed;

			public MountedVirtualDiskScope(string virtualDiskPath, string driveRoot)
			{
				this.virtualDiskPath = virtualDiskPath;
				DriveRoot = driveRoot;
			}

			public string DriveRoot { get; }

			public void Dispose()
			{
				if (disposed)
				{
					return;
				}

				BackupMountManager.UnmountVirtualDisk(virtualDiskPath);
				disposed = true;
			}
		}

		private MountedVirtualDiskScope MountPrimaryHyperVVirtualDisk(string backupPointPath)
		{
			string? virtualDiskPath = RestoreWindowNew.HyperVRestorePointHelper.FindPrimaryVirtualDisk(backupPointPath);
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

		private static string GetSupportedRestoreFileSystem(VolumeInfo volume)
		{
			string fileSystem = volume.FileSystem?.Trim() ?? string.Empty;
			return fileSystem.ToUpperInvariant() switch
			{
				"NTFS" => "NTFS",
				"FAT32" => "FAT32",
				"EXFAT" => "exFAT",
				"REFS" => "ReFS",
				_ => "NTFS"
			};
		}

		private string GetSelectedRestoreFileSystem()
		{
			if (selectedRestoreVolume != null)
			{
				return GetSupportedRestoreFileSystem(selectedRestoreVolume);
			}
			if (selectedRestoreDiskGroup?.Count > 0)
			{
				return GetSupportedRestoreFileSystem(selectedRestoreDiskGroup[0]);
			}
			return "NTFS";
		}

		private static void ResizeTargetVolumeIfNeeded(string volumePath, long desiredSizeBytes)
		{
			string driveLetter = volumePath.TrimEnd('\\');
			if (string.IsNullOrWhiteSpace(driveLetter) || driveLetter.Length < 2 || driveLetter[1] != ':')
			{
				return;
			}

			var driveInfo = new DriveInfo(driveLetter);
			if (!driveInfo.IsReady || desiredSizeBytes <= 0 || desiredSizeBytes >= driveInfo.TotalSize || AreSizesEquivalent(desiredSizeBytes, driveInfo.TotalSize))
			{
				return;
			}

			string script = $"$partition = Get-Partition -DriveLetter '{driveLetter[0]}' -ErrorAction Stop; Resize-Partition -InputObject $partition -Size {desiredSizeBytes} -ErrorAction Stop | Out-Null";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});

			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to resize the selected target volume. {errors}".Trim());
			}
		}

		private static void FormatTargetVolume(string volumePath, string fileSystem, string? label)
		{
			string driveLetter = volumePath.TrimEnd('\\');
			if (string.IsNullOrWhiteSpace(driveLetter) || driveLetter.Length < 2 || driveLetter[1] != ':')
			{
				throw new InvalidOperationException("The selected target volume does not have a valid drive letter.");
			}

			string supportedFileSystem = string.IsNullOrWhiteSpace(fileSystem) ? "NTFS" : fileSystem;
			string volumeLabel = string.IsNullOrWhiteSpace(label) ? "SSBRestore" : label.Trim();
			string script = $"Get-Volume -DriveLetter '{driveLetter[0]}' -ErrorAction Stop | Format-Volume -FileSystem '{EscapePowerShellSingleQuotedString(supportedFileSystem)}' -NewFileSystemLabel '{EscapePowerShellSingleQuotedString(volumeLabel)}' -Confirm:$false -Force -ErrorAction Stop | Out-Null";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});

			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to format the selected target volume. {errors}".Trim());
			}
		}

		private static int GetDiskNumberForDriveLetter(string driveRoot)
		{
			string normalizedDrive = driveRoot.TrimEnd('\\');
			string script = $"$partition = Get-Partition -DriveLetter '{normalizedDrive[0]}' -ErrorAction Stop; $partition.DiskNumber";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});

			string output = process?.StandardOutput.ReadToEnd() ?? string.Empty;
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0 || !int.TryParse(output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), out int diskNumber))
			{
				throw new InvalidOperationException($"Failed to resolve the mounted Hyper-V virtual disk number. {errors}".Trim());
			}
			return diskNumber;
		}

		private static void RemoveHyperVVm(string vmName)
		{
			string escaped = vmName.Replace("'", "''", StringComparison.Ordinal);
			string script = $"Remove-VM -Name '{escaped}' -Force -ErrorAction Stop";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to remove the existing Hyper-V virtual machine. {errors}".Trim());
			}
		}

		private static string? GetHyperVVmStoragePath(string vmName)
		{
			try
			{
				string escaped = vmName.Replace("'", "''", StringComparison.Ordinal);
				string script = $"(Get-VM -Name '{escaped}' -ErrorAction SilentlyContinue).Path";
				var process = Process.Start(new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				});
				string output = process?.StandardOutput.ReadToEnd() ?? string.Empty;
				process?.WaitForExit();
				string path = output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;
				return string.IsNullOrWhiteSpace(path) ? null : Path.GetDirectoryName(path);
			}
			catch
			{
				return null;
			}
		}

		private static void PrepareHyperVVirtualDiskFile(string virtualDiskPath, bool createFixedDisk)
		{
			string diskType = createFixedDisk ? "Fixed" : "Dynamic";
			long sizeBytes = createFixedDisk ? 137438953472L : 68719476736L;
			string script = $"$path='{virtualDiskPath.Replace("'", "''", StringComparison.Ordinal)}'; if (Test-Path $path) {{ Dismount-DiskImage -ImagePath $path -ErrorAction SilentlyContinue; }} else {{ New-VHD -Path $path -SizeBytes {sizeBytes} -{diskType} | Out-Null; }}";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			});
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to prepare the Hyper-V virtual disk file. {errors}".Trim());
			}
		}

		private string CreateVolumeOnDiskForHyperVRestore(int diskNumber)
		{
			string script = $"$diskNumber={diskNumber}; Clear-Disk -Number $diskNumber -RemoveData -RemoveOEM -Confirm:$false -ErrorAction Stop; Initialize-Disk -Number $diskNumber -PartitionStyle GPT -ErrorAction Stop; $partition = New-Partition -DiskNumber $diskNumber -UseMaximumSize -AssignDriveLetter -ErrorAction Stop; Format-Volume -Partition $partition -FileSystem NTFS -NewFileSystemLabel 'SSBRestore' -Confirm:$false -Force -ErrorAction Stop | Out-Null; ($partition | Get-Volume).DriveLetter";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});
			string output = process?.StandardOutput.ReadToEnd() ?? string.Empty;
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to prepare the target disk for Hyper-V guest restore. {errors}".Trim());
			}
			string driveLetter = output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(driveLetter))
			{
				throw new InvalidOperationException("The target disk was prepared, but no drive letter was assigned to the new restore volume.");
			}
			return driveLetter.EndsWith(":", StringComparison.Ordinal) ? driveLetter + "\\" : driveLetter + ":\\";
		}

		private static void AttachVirtualDiskToExistingHyperVVm(string vmName, string virtualDiskPath)
		{
			string escapedVmName = vmName.Replace("'", "''", StringComparison.Ordinal);
			string escapedDiskPath = virtualDiskPath.Replace("'", "''", StringComparison.Ordinal);
			string script = $"Add-VMHardDiskDrive -VMName '{escapedVmName}' -Path '{escapedDiskPath}' -ErrorAction Stop";
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to attach the restored virtual disk to the selected Hyper-V virtual machine. {errors}".Trim());
			}
		}

		private static void CreateNewHyperVVm(string vmName, string vmStoragePath, string virtualDiskPath, int generation, bool startAfterCreate)
		{
			string script = RestoreWindowNew.RegularHyperVRestoreHelper.BuildCreateVirtualMachineScript(vmName, vmStoragePath, virtualDiskPath, generation, startAfterCreate);
			var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});
			string errors = process?.StandardError.ReadToEnd() ?? string.Empty;
			process?.WaitForExit();
			if (process == null || process.ExitCode != 0)
			{
				throw new InvalidOperationException($"Failed to create the new Hyper-V virtual machine. {errors}".Trim());
			}
		}

		private int GetSelectedNewHyperVVmGeneration()
		{
			return newHyperVGenerationComboBox.SelectedItem is GenerationOption option ? option.Generation : 2;
		}

		private static long GetRequestedRestoreSize(VolumeInfo volume)
		{
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

		private static string EnsureTrailingSlash(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return string.Empty;
			}
			return path.EndsWith("\\", StringComparison.Ordinal) ? path : path + "\\";
		}

		private static string EscapePowerShellSingleQuotedString(string value)
		{
			return (value ?? string.Empty).Replace("'", "''", StringComparison.Ordinal);
		}

		private static string FormatSize(long bytes)
		{
			if (bytes <= 0)
			{
				return "—";
			}
			double gb = bytes / (1024.0 * 1024.0 * 1024.0);
			return gb >= 1.0 ? $"{gb:F2} GB" : $"{bytes / (1024.0 * 1024.0):F0} MB";
		}

		private void BrowseHyperVRestoreDirectory()
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select an empty directory to restore the Hyper-V virtual machine into",
				ShowNewFolderButton = true
			};
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				hyperVRestoreDirectoryTextBox.Text = dialog.SelectedPath;
				UpdateActionState();
			}
		}

		private void BrowseHyperVVirtualDiskPath()
		{
			string currentPath = hyperVVirtualDiskPathTextBox.Text.Trim();
			string initialDirectory = !string.IsNullOrWhiteSpace(currentPath)
				? Path.GetDirectoryName(currentPath) ?? string.Empty
				: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string initialFileName = !string.IsNullOrWhiteSpace(currentPath)
				? Path.GetFileName(currentPath)
				: Path.GetFileName(RestoreWindowNew.RegularHyperVRestoreHelper.BuildDefaultHyperVVirtualDiskPath(initialDirectory, restoreSelection.Backup.BackupName));

			using var dialog = new SaveFileDialog
			{
				Title = "Select Hyper-V virtual disk destination",
				Filter = "Hyper-V Virtual Disk (*.vhdx)|*.vhdx",
				DefaultExt = "vhdx",
				AddExtension = true,
				OverwritePrompt = false,
				CheckPathExists = true,
				InitialDirectory = initialDirectory,
				FileName = initialFileName
			};
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				hyperVVirtualDiskPathTextBox.Text = dialog.FileName;
				ApplyHyperVDefaults();
				UpdateActionState();
			}
		}

		private void BrowseNewHyperVVmLocation()
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select Hyper-V virtual machine storage folder",
				ShowNewFolderButton = true
			};
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				newHyperVVmPathTextBox.Text = dialog.SelectedPath;
				UpdateActionState();
			}
		}

		private sealed class TargetChoice
		{
			public DriveTreeItemType ItemType { get; init; }
			public int DiskNumber { get; init; }
			public string Name { get; init; } = string.Empty;
			public string Path { get; init; } = string.Empty;
			public long Size { get; init; }
			public string Notes { get; init; } = string.Empty;
		}

		private sealed class RestoreModeOption
		{
			public RestoreModeOption(string displayName, RestoreTargetKind targetKind)
			{
				DisplayName = displayName;
				TargetKind = targetKind;
			}

			public string DisplayName { get; }
			public RestoreTargetKind TargetKind { get; }
			public override string ToString() => DisplayName;
		}

		private sealed class GenerationOption
		{
			public GenerationOption(string displayName, int generation)
			{
				DisplayName = displayName;
				Generation = generation;
			}

			public string DisplayName { get; }
			public int Generation { get; }
			public override string ToString() => DisplayName;
		}

	}
}
