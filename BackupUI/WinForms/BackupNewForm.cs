using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;
using Svg;
using SecureServerBackup.Services;
using SecureServerBackup.Models;
using SecureServerBackup.Enums;

using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	public sealed partial class BackupNewForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly BackupJob? existingJob;
		private readonly JobManager jobManager = new();
		private readonly List<string> nativeSourcePaths = new();
		private readonly List<string> nativeUserExclusions = new();
		private readonly List<string> savedNetworkPaths = new();
		private readonly HashSet<TreeNode> loadingVolumeNodes = [];

		private BackupJob? currentJob;
		private bool hasSavedEncryptionPassword;
		private bool isPopulatingDriveTree;
		private string savedProtectedPassword = string.Empty;
		private bool suppressPasswordSync;
		private bool suppressTreeCheckSync;
		private int volumeAnimationFrame;
		private Control[] settingsWidthControls = [];
		private readonly ProgressBar driveTreeLoadProgressBar = new();
		private readonly Label driveTreeStatusLabel = new();
		private readonly CheckBox includeSystemStateCheckBox = new();
		private readonly CheckBox renameHyperVSystemCheckBox = new();
		private readonly TextBox renameHyperVSystemNameTextBox = new();
		private const int VolumeAnimationFrameCount = 6;




		public BackupNewForm()
			: this(null)
		{
		}

		public BackupNewForm(BackupJob? job)
		{
			existingJob = job;
			currentJob = job;
			InitializeComponent();
			BuildFormLayout();

			Text = job == null ? "Create Backup" : $"Edit Backup - {job.Name}";
			headerLabel.Text = Text;

			settingsWidthControls = [basicGroup, scheduleGroup];
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
			Shown += BackupNewForm_Shown;

			nativeCoverageLabel.Visible = false;
			actionInfoLabel.Visible = false;
			actionHelpLabel.Visible = false;
			advancedStateLabel.Visible = false;
			advancedSelectionListBox.Visible = false;
			openAdvancedEditorButton.Visible = false;

			if (IsInDesignMode)
			{
				ApplyDesignTimeState();
				return;
			}

			savedNetworkPaths.AddRange(SavedNetworkPathStore.Load());
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

		private void RenameHyperVSystemCheckBox_CheckedChanged(object? sender, EventArgs e)
		{
			UpdateCloneOptionsUi();
		}

		private void RefreshDriveTreeButton_Click(object? sender, EventArgs e)
		{
			_ = PopulateDriveTreeAsync();
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
			_ = PopulateDriveTreeAsync();
		}

		private async void BackupNewForm_Shown(object? sender, EventArgs e)
		{
			try
			{
				await PopulateDriveTreeAsync().ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[BackupNewForm] Failed to load source tree: {ex.Message}");
			}
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

		private void BuildFormLayout()
		{
			SuspendLayout();

			ConfigureRetentionPanel(retentionPanel, "Full Backup Retention", "Keep the most recent", retainCountTextBox, "full backup set(s).");
			ConfigureRetentionPanel(selectedFilesRetentionPanel, "Selected Files Retention", "Keep the most recent", selectedFilesRetentionComboBox, "selected file backup(s).");
			ConfigureRetentionPanel(cloneRetentionPanel, "Clone Export Retention", "Keep the most recent", cloneRetentionComboBox, "clone or export backup(s).");
			ConfigureScheduleGroup();
			ConfigureSourceControls();
			ConfigureBasicGroup();
			scheduleGroup.AutoSize = false;
			scheduleGroup.MinimumSize = new Size(0, 220);

			Controls.Clear();

			var rootLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(12, 10, 12, 12),
				ColumnCount = 2,
				RowCount = 3
			};
			rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			headerLabel.AutoSize = true;
			headerLabel.Margin = new Padding(0, 0, 0, 12);

			rootLayout.Controls.Add(headerLabel, 0, 0);
			rootLayout.SetColumnSpan(headerLabel, 2);
			rootLayout.Controls.Add(CreateSourcesColumn(), 0, 1);

			settingsScroll.Controls.Clear();
			settingsScroll.AutoScroll = true;
			settingsScroll.Dock = DockStyle.Fill;
			settingsScroll.Margin = new Padding(12, 0, 0, 0);
			settingsScroll.BackColor = Color.Transparent;

			settingsStack.Controls.Clear();
			settingsStack.AutoSize = true;
			settingsStack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			settingsStack.ColumnCount = 1;
			settingsStack.RowCount = 0;
			settingsStack.Dock = DockStyle.Top;
			settingsStack.Margin = Padding.Empty;
			settingsStack.Padding = Padding.Empty;
			settingsStack.ColumnStyles.Clear();
			settingsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

			AddSettingsSection(basicGroup);
			AddSettingsSection(scheduleGroup);

			settingsScroll.Controls.Add(settingsStack);
			rootLayout.Controls.Add(settingsScroll, 1, 1);
			Control actionButtonPanel = CreateActionButtonPanel();
			rootLayout.Controls.Add(actionButtonPanel, 0, 2);
			rootLayout.SetColumnSpan(actionButtonPanel, 2);

			Controls.Add(rootLayout);

			ResumeLayout(false);
			PerformLayout();
		}

		private void AddSettingsSection(Control control)
		{
			ArgumentNullException.ThrowIfNull(control);

			int row = settingsStack.RowCount;
			if (ReferenceEquals(control, scheduleGroup))
			{
				int scheduleHeight = Math.Max(scheduleGroup.Height, scheduleGroup.MinimumSize.Height);
				settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, Math.Max(scheduleHeight, 220)));
			}
			else
			{
				settingsStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}
			settingsStack.RowCount = row + 1;
			control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			settingsStack.Controls.Add(control, 0, row);
		}

		private void ConfigureBasicGroup()
		{
			basicGroup.Controls.Clear();
			basicGroup.Text = "Settings";
			basicGroup.Visible = true;

			var layout = CreateDetailsLayout();
			AddLabeledControl(layout, 0, "Backup Name", backupNameTextBox);

			EnsureRow(layout, 1);
			layout.Controls.Add(new Label
			{
				Text = "Backup Type",
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 4, 8, 4)
			}, 0, 1);

			var backupTypesPanel = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				ColumnCount = 3,
				Margin = new Padding(0, 2, 4, 2)
			};
			backupTypesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
			backupTypesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
			backupTypesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
			AddBackupTypeRadioButton(backupTypesPanel, 0, 0, "Full Backup", 0);
			AddBackupTypeRadioButton(backupTypesPanel, 1, 0, "Full then Incremental", 1);
			AddBackupTypeRadioButton(backupTypesPanel, 2, 0, "Full then Differential", 2);
			AddBackupTypeRadioButton(backupTypesPanel, 0, 1, "Selected Files & Folder", 3);
			AddBackupTypeRadioButton(backupTypesPanel, 1, 1, "Clone to Disk", 4);
			AddBackupTypeRadioButton(backupTypesPanel, 2, 1, "Clone to Virtual Disk (Hyper-V)", 5);
			AddBackupTypeRadioButton(backupTypesPanel, 0, 2, "Clone Hyper-V System", 6);
			AddBackupTypeRadioButton(backupTypesPanel, 1, 2, "Export Hyper-V System", 7);

			layout.Controls.Add(backupTypesPanel, 1, 1);
			layout.SetColumnSpan(backupTypesPanel, 2);

			backupTypeComboBox.Visible = false;
			backupTypeComboBox.TabStop = false;
			layout.Controls.Add(backupTypeComboBox, 2, 1);

			var browseDestinationButton = new Button
			{
				Text = "Browse...",
				AutoSize = true,
				UseVisualStyleBackColor = true,
				Margin = new Padding(0, 2, 0, 2)
			};
			browseDestinationButton.Click += BrowseDestination_Click;

			ConfigureFillControl(destinationTextBox);
			AddLabeledControl(layout, 2, "Backup Destination", destinationTextBox, browseDestinationButton);

			includeSystemStateCheckBox.Text = "Auto-include system state for boot volumes";
			includeSystemStateCheckBox.AutoSize = true;
			includeSystemStateCheckBox.Margin = new Padding(0, 4, 4, 4);
			AddCheckBoxRow(layout, 3, includeSystemStateCheckBox, includeSystemStateCheckBox.Text);
			AddCheckBoxRow(layout, 4, compressCheckBox, "Compress backup data");
			AddCheckBoxRow(layout, 5, verifyCheckBox, "Verify backup after completion");

			manageExclusionsButton.AutoSize = true;
			manageExclusionsButton.UseVisualStyleBackColor = true;
			manageExclusionsButton.Anchor = AnchorStyles.Left;
			UpdateExclusionsButtonText();
			EnsureRow(layout, 6);
			layout.Controls.Add(new Label { AutoSize = true }, 0, 6);
			layout.Controls.Add(manageExclusionsButton, 1, 6);
			layout.SetColumnSpan(manageExclusionsButton, 2);

			EnsureRow(layout, 7);
			retentionPanel.Margin = new Padding(0, 4, 0, 4);
			layout.Controls.Add(retentionPanel, 0, 7);
			layout.SetColumnSpan(retentionPanel, 3);

			EnsureRow(layout, 8);
			selectedFilesRetentionPanel.Margin = new Padding(0, 4, 0, 4);
			layout.Controls.Add(selectedFilesRetentionPanel, 0, 8);
			layout.SetColumnSpan(selectedFilesRetentionPanel, 3);

			EnsureRow(layout, 9);
			cloneRetentionPanel.Margin = new Padding(0, 4, 0, 4);
			layout.Controls.Add(cloneRetentionPanel, 0, 9);
			layout.SetColumnSpan(cloneRetentionPanel, 3);

			encryptCheckBox.AutoSize = true;
			encryptCheckBox.Text = "Encrypt Backup";
			encryptCheckBox.Margin = new Padding(0, 6, 4, 4);
			AddCheckBoxRow(layout, 10, encryptCheckBox, encryptCheckBox.Text);

			encryptionPanel.Controls.Clear();
			encryptionPanel.Dock = DockStyle.Top;
			encryptionPanel.AutoSize = true;
			encryptionPanel.Margin = new Padding(0, 0, 0, 4);

			var encryptionDetailsLayout = CreateDetailsLayout();
			AddLabeledControl(encryptionDetailsLayout, 0, "Password", encryptionPasswordTextBox);

			EnsureRow(encryptionDetailsLayout, 1);
			var verifyPasswordLabel = new Label
			{
				Name = "VerifyPasswordLabel",
				Text = "Verify Password",
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 4, 8, 4)
			};
			encryptionDetailsLayout.Controls.Add(verifyPasswordLabel, 0, 1);
			ConfigureFillControl(verifyEncryptionPasswordTextBox);
			encryptionDetailsLayout.Controls.Add(verifyEncryptionPasswordTextBox, 1, 1);
			encryptionPanel.Controls.Add(encryptionDetailsLayout);

			showPasswordCheckBox.AutoSize = true;
			showPasswordCheckBox.Text = "Show password";
			showPasswordCheckBox.Margin = new Padding(0, 2, 4, 4);

			EnsureRow(layout, 11);
			layout.Controls.Add(encryptionPanel, 0, 11);
			layout.SetColumnSpan(encryptionPanel, 3);

			AddCheckBoxRow(layout, 12, showPasswordCheckBox, showPasswordCheckBox.Text);

			renameHyperVSystemCheckBox.AutoSize = true;
			renameHyperVSystemCheckBox.Text = "Rename imported Hyper-V system";
			renameHyperVSystemCheckBox.Margin = new Padding(0, 4, 4, 4);
			renameHyperVSystemCheckBox.CheckedChanged -= RenameHyperVSystemCheckBox_CheckedChanged;
			renameHyperVSystemCheckBox.CheckedChanged += RenameHyperVSystemCheckBox_CheckedChanged;
			AddCheckBoxRow(layout, 13, renameHyperVSystemCheckBox, renameHyperVSystemCheckBox.Text);

			ConfigureFillControl(renameHyperVSystemNameTextBox);
			AddLabeledControl(layout, 14, "New Hyper-V Name", renameHyperVSystemNameTextBox);

			basicGroup.Controls.Add(layout);
		}

		private void ConfigureExclusionsGroup()
		{
			exclusionsGroup.Controls.Clear();
			exclusionsGroup.Visible = true;

			var layout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				ColumnCount = 1,
				Padding = new Padding(8)
			};
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			var descriptionLabel = new Label
			{
				Text = "Choose folders or files that should be skipped when this backup runs.",
				AutoSize = true,
				MaximumSize = new Size(340, 0),
				Margin = new Padding(0, 0, 0, 8)
			};

			manageExclusionsButton.AutoSize = true;
			manageExclusionsButton.UseVisualStyleBackColor = true;
			manageExclusionsButton.Anchor = AnchorStyles.Left;
			UpdateExclusionsButtonText();

			layout.Controls.Add(descriptionLabel, 0, 0);
			layout.Controls.Add(manageExclusionsButton, 0, 1);
			exclusionsGroup.Controls.Add(layout);
		}

		private void ConfigureEncryptionGroup()
		{
			encryptionGroup.Controls.Clear();
			encryptionGroup.Visible = true;

			var outerLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				ColumnCount = 1,
				Padding = new Padding(8)
			};

			encryptCheckBox.AutoSize = true;
			encryptCheckBox.Text = "Encrypt backup data";
			encryptCheckBox.Margin = new Padding(0, 0, 0, 8);

			encryptionPanel.Controls.Clear();
			encryptionPanel.Dock = DockStyle.Top;
			encryptionPanel.AutoSize = true;

			var detailsLayout = CreateDetailsLayout();
			AddLabeledControl(detailsLayout, 0, "Password", encryptionPasswordTextBox);

			EnsureRow(detailsLayout, 1);
			var verifyPasswordLabel = new Label
			{
				Name = "VerifyPasswordLabel",
				Text = "Verify Password",
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 4, 8, 4)
			};
			detailsLayout.Controls.Add(verifyPasswordLabel, 0, 1);
			ConfigureFillControl(verifyEncryptionPasswordTextBox);
			detailsLayout.Controls.Add(verifyEncryptionPasswordTextBox, 1, 1);

			showPasswordCheckBox.AutoSize = true;
			showPasswordCheckBox.Text = "Show password";
			showPasswordCheckBox.Margin = new Padding(0, 6, 0, 0);

			encryptionPanel.Controls.Add(detailsLayout);
			outerLayout.Controls.Add(encryptCheckBox, 0, 0);
			outerLayout.Controls.Add(encryptionPanel, 0, 1);
			outerLayout.Controls.Add(showPasswordCheckBox, 0, 2);
			encryptionGroup.Controls.Add(outerLayout);
		}

		private void ConfigureScheduleGroup()
		{
			scheduleGroup.Controls.Clear();
			scheduleGroup.Visible = true;
			scheduleGroup.AutoSize = false;
			scheduleGroup.AutoSizeMode = AutoSizeMode.GrowOnly;

			var outerLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = false,
				ColumnCount = 1,
				Padding = new Padding(8)
			};
			outerLayout.RowCount = 2;
			outerLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

			enableScheduleCheckBox.AutoSize = true;
			enableScheduleCheckBox.Text = "Enable Scheduled Backup";
			enableScheduleCheckBox.Margin = new Padding(0, 0, 0, 8);

			schedulePanel.Controls.Clear();
			schedulePanel.Dock = DockStyle.Fill;
			schedulePanel.AutoSize = false;
			schedulePanel.MinimumSize = new Size(0, 150);

			var detailsLayout = CreateDetailsLayout();
			detailsLayout.Dock = DockStyle.Fill;
			detailsLayout.AutoSize = false;
			frequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			AddLabeledControl(detailsLayout, 0, "Frequency", frequencyComboBox);

			var timePanel = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				Margin = new Padding(0, 2, 4, 2)
			};
			hourComboBox.Width = 60;
			minuteComboBox.Width = 60;
			amPmComboBox.Width = 70;
			timePanel.Controls.Add(hourComboBox);
			timePanel.Controls.Add(new Label { Text = ":", AutoSize = true, Margin = new Padding(6, 8, 6, 0) });
			timePanel.Controls.Add(minuteComboBox);
			timePanel.Controls.Add(amPmComboBox);
			AddLabeledControl(detailsLayout, 1, "Time", timePanel);

			weeklyPanel.Controls.Clear();
			weeklyPanel.Dock = DockStyle.Top;
			weeklyPanel.AutoSize = true;
			var weeklyLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				ColumnCount = 2,
				Margin = new Padding(0)
			};
			weeklyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
			weeklyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			weeklyLayout.Controls.Add(new Label
			{
				Text = "Days",
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 4, 8, 4)
			}, 0, 0);
			weeklyDaysCheckedListBox.CheckOnClick = true;
			weeklyDaysCheckedListBox.Height = 116;
			if (weeklyDaysCheckedListBox.Items.Count == 0)
			{
				weeklyDaysCheckedListBox.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
			}
			weeklyLayout.Controls.Add(weeklyDaysCheckedListBox, 1, 0);
			weeklyPanel.Controls.Add(weeklyLayout);

			monthlyPanel.Controls.Clear();
			monthlyPanel.Dock = DockStyle.Top;
			monthlyPanel.AutoSize = true;
			var monthlyLayout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				AutoSize = true,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				Margin = new Padding(0)
			};
			monthlyLayout.Controls.Add(new Label { Text = "Day of month", AutoSize = true, Margin = new Padding(0, 8, 6, 0) });
			monthlyLayout.Controls.Add(dayOfMonthComboBox);
			monthlyPanel.Controls.Add(monthlyLayout);

			EnsureRow(detailsLayout, 2);
			detailsLayout.Controls.Add(new Label { AutoSize = true }, 0, 2);
			detailsLayout.Controls.Add(weeklyPanel, 1, 2);
			detailsLayout.SetColumnSpan(weeklyPanel, 2);

			EnsureRow(detailsLayout, 3);
			detailsLayout.Controls.Add(new Label { AutoSize = true }, 0, 3);
			detailsLayout.Controls.Add(monthlyPanel, 1, 3);
			detailsLayout.SetColumnSpan(monthlyPanel, 2);
			detailsLayout.RowStyles[0] = new RowStyle(SizeType.AutoSize);
			detailsLayout.RowStyles[1] = new RowStyle(SizeType.AutoSize);
			detailsLayout.RowStyles[2] = new RowStyle(SizeType.AutoSize);
			detailsLayout.RowStyles[3] = new RowStyle(SizeType.AutoSize);

			schedulePanel.Controls.Add(detailsLayout);
			outerLayout.Controls.Add(enableScheduleCheckBox, 0, 0);
			outerLayout.Controls.Add(schedulePanel, 0, 1);
			scheduleGroup.Controls.Add(outerLayout);
		}

		private void ConfigureSourceControls()
		{
			refreshDriveTreeButton.Text = "Refresh";
			refreshDriveTreeButton.AutoSize = true;
			refreshDriveTreeButton.UseVisualStyleBackColor = true;

			expandTreeButton.Text = "Expand All";
			expandTreeButton.AutoSize = true;
			expandTreeButton.UseVisualStyleBackColor = true;

			collapseTreeButton.Text = "Collapse All";
			collapseTreeButton.AutoSize = true;
			collapseTreeButton.UseVisualStyleBackColor = true;

			showHiddenPartitionsCheckBox.AutoSize = true;
			showHiddenPartitionsCheckBox.Text = "Show hidden partitions";

			driveTreeLoadProgressBar.Style = ProgressBarStyle.Marquee;
			driveTreeLoadProgressBar.MarqueeAnimationSpeed = 25;
			driveTreeLoadProgressBar.Dock = DockStyle.Fill;
			driveTreeLoadProgressBar.Visible = false;

			driveTreeStatusLabel.AutoSize = true;
			driveTreeStatusLabel.Margin = new Padding(0, 0, 0, 6);
			driveTreeStatusLabel.Text = "Loading backup sources...";
			driveTreeStatusLabel.Visible = false;
		}

		private Control CreateSourcesColumn()
		{
			var sourceColumn = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				ColumnCount = 1,
				RowCount = 1,
				Margin = Padding.Empty
			};
			sourceColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

			sourceColumn.Controls.Add(CreateSourceSelectionGroup(), 0, 0);

			return sourceColumn;
		}

		private Control CreateSourceSelectionGroup()
		{
			GroupBox group = CreateGroupBox("What to Backup");
			group.Dock = DockStyle.Fill;
			group.AutoSize = false;

			var layout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(8),
				ColumnCount = 1,
				RowCount = 5
			};
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			var descriptionLabel = new Label
			{
				Text = "Check drives or volumes to backup. Files and folders are shown when volumes are unchecked. Boot volumes will automatically include system state backup.",
				AutoSize = true,
				MaximumSize = new Size(520, 0),
				Margin = new Padding(0, 0, 0, 8)
			};

			var loadingLayout = new TableLayoutPanel
			{
				AutoSize = true,
				Dock = DockStyle.Fill,
				ColumnCount = 1,
				Margin = new Padding(0, 0, 0, 6)
			};
			loadingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			loadingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			loadingLayout.Controls.Add(driveTreeStatusLabel, 0, 0);
			loadingLayout.Controls.Add(driveTreeLoadProgressBar, 0, 1);

			var treeButtonPanel = new FlowLayoutPanel
			{
				AutoSize = true,
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = true,
				Margin = new Padding(0, 8, 0, 0)
			};
			treeButtonPanel.Controls.Add(refreshDriveTreeButton);
			treeButtonPanel.Controls.Add(expandTreeButton);
			treeButtonPanel.Controls.Add(collapseTreeButton);
			treeButtonPanel.Controls.Add(showHiddenPartitionsCheckBox);

			driveTree.Dock = DockStyle.Fill;
			driveTree.Margin = new Padding(0, 0, 0, 8);

			layout.Controls.Add(descriptionLabel, 0, 0);
			layout.Controls.Add(loadingLayout, 0, 1);
			layout.Controls.Add(driveTree, 0, 2);
			layout.Controls.Add(treeButtonPanel, 0, 3);

			group.Controls.Add(layout);
			return group;
		}

		private Control CreateAdvancedStateGroup()
		{
			GroupBox group = CreateGroupBox("Advanced State");
			group.Dock = DockStyle.Fill;
			group.AutoSize = false;

			var layout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(8),
				ColumnCount = 1,
				RowCount = 5
			};
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

			layout.Controls.Add(nativeCoverageLabel, 0, 0);
			layout.Controls.Add(actionInfoLabel, 0, 1);
			layout.Controls.Add(actionHelpLabel, 0, 2);
			layout.Controls.Add(advancedStateLabel, 0, 3);

			var selectionLayout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				ColumnCount = 1,
				RowCount = 2,
				Margin = Padding.Empty
			};
			selectionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			selectionLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			selectionLayout.Controls.Add(advancedSelectionListBox, 0, 0);
			selectionLayout.Controls.Add(openAdvancedEditorButton, 0, 1);

			layout.Controls.Add(selectionLayout, 0, 4);
			group.Controls.Add(layout);
			return group;
		}

		private Control CreateActionButtonPanel()
		{
			var actionPanel = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.RightToLeft,
				AutoSize = true,
				WrapContents = false,
				Margin = new Padding(0, 12, 0, 0)
			};

			saveJobButton.Text = "Save Backup";
			saveJobButton.Text = "Save Job";
			saveJobButton.AutoSize = true;
			saveJobButton.UseVisualStyleBackColor = true;

			startBackupButton.Text = "Start Backup";
			startBackupButton.AutoSize = true;
			startBackupButton.UseVisualStyleBackColor = true;

			Button cancelButton = new()
			{
				Text = "Cancel",
				AutoSize = true,
				UseVisualStyleBackColor = true,
				DialogResult = DialogResult.Cancel
			};
			cancelButton.Click += (_, _) => Close();
			CancelButton = cancelButton;

			actionPanel.Controls.Add(cancelButton);
			actionPanel.Controls.Add(saveJobButton);
			actionPanel.Controls.Add(startBackupButton);

			return actionPanel;
		}

		private static void ConfigureRetentionPanel(Panel panel, string title, string prefix, TextBox textBox, string suffix)
		{
			ArgumentNullException.ThrowIfNull(panel);
			ArgumentNullException.ThrowIfNull(textBox);

			panel.Controls.Clear();
			panel.Dock = DockStyle.Top;
			panel.AutoSize = true;

			GroupBox group = CreateGroupBox(title);
			var layout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				AutoSize = true,
				Padding = new Padding(10)
			};

			textBox.Width = 60;
			textBox.TextAlign = HorizontalAlignment.Center;

			layout.Controls.Add(new Label { Text = prefix, AutoSize = true, Margin = new Padding(0, 8, 6, 0) });
			layout.Controls.Add(textBox);
			layout.Controls.Add(new Label { Text = suffix, AutoSize = true, Margin = new Padding(6, 8, 0, 0) });

			group.Controls.Add(layout);
			panel.Controls.Add(group);
		}

		private static void ConfigureRetentionPanel(Panel panel, string title, string prefix, ComboBox comboBox, string suffix)
		{
			ArgumentNullException.ThrowIfNull(panel);
			ArgumentNullException.ThrowIfNull(comboBox);

			panel.Controls.Clear();
			panel.Dock = DockStyle.Top;
			panel.AutoSize = true;

			GroupBox group = CreateGroupBox(title);
			var layout = new FlowLayoutPanel
			{
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				AutoSize = true,
				Padding = new Padding(10)
			};

			comboBox.Width = 80;
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

			layout.Controls.Add(new Label { Text = prefix, AutoSize = true, Margin = new Padding(0, 8, 6, 0) });
			layout.Controls.Add(comboBox);
			layout.Controls.Add(new Label { Text = suffix, AutoSize = true, Margin = new Padding(6, 8, 0, 0) });

			group.Controls.Add(layout);
			panel.Controls.Add(group);
		}

		private static void AddLabeledControl(TableLayoutPanel panel, int row, string label, Control control, Control? trailingControl = null)
		{
			ArgumentNullException.ThrowIfNull(panel);
			ArgumentNullException.ThrowIfNull(control);

			EnsureRow(panel, row);
			panel.Controls.Add(new Label
			{
				Text = label,
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(0, 4, 8, 4)
			}, 0, row);

			ConfigureFillControl(control);
			panel.Controls.Add(control, 1, row);

			if (trailingControl != null)
			{
				trailingControl.Anchor = AnchorStyles.Left;
				panel.Controls.Add(trailingControl, 2, row);
			}
		}

		private static void AddCheckBoxRow(TableLayoutPanel panel, int row, CheckBox checkBox, string text)
		{
			ArgumentNullException.ThrowIfNull(panel);
			ArgumentNullException.ThrowIfNull(checkBox);

			EnsureRow(panel, row);
			checkBox.Text = text;
			checkBox.AutoSize = true;
			checkBox.Margin = new Padding(0, 4, 4, 4);
			panel.Controls.Add(new Label { AutoSize = true }, 0, row);
			panel.Controls.Add(checkBox, 1, row);
			panel.SetColumnSpan(checkBox, 2);
		}

		private static void ConfigureFillControl(Control control)
		{
			ArgumentNullException.ThrowIfNull(control);

			control.Dock = DockStyle.Fill;
			control.Margin = new Padding(0, 2, 4, 2);
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
				Text = "WinForms backup status\n\n- Native: common settings, encryption, schedule, retention, source tree, Hyper-V, clone, export, save, and immediate execution\n- This form is the primary backup authoring surface",
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
			else if (job.Target == BackupTarget.HyperV || job.IsHyperVBackup || job.HyperVMachines.Count > 0)
			{
				nativeSourcePaths.AddRange(GetReplayPathsForJob(job)
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
				nativeSourceListBox.Items.Add("No backup sources selected.");
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

		private async Task PopulateDriveTreeAsync()
		{
			if (IsInDesignMode || driveTree.IsDisposed || isPopulatingDriveTree)
			{
				return;
			}

			isPopulatingDriveTree = true;
			SetDriveTreeLoadingState(true, "Loading backup sources...");

			try
			{
				bool showHidden = showHiddenPartitionsCheckBox.Checked;
				List<SourceRootDescriptor> rootDescriptors = await Task.Run(() => LoadDriveTreeRootDescriptors(showHidden)).ConfigureAwait(true);

				if (IsDisposed || driveTree.IsDisposed)
				{
					return;
				}

				driveTree.BeginUpdate();
				try
				{
					driveTree.Nodes.Clear();
					foreach (SourceRootDescriptor descriptor in rootDescriptors)
					{
						driveTree.Nodes.Add(CreateRootNode(descriptor));
					}

					if (driveTree.Nodes.Count == 0)
					{
						driveTree.Nodes.Add(new TreeNode("No disks, Hyper-V systems, or network locations were detected."));
					}
				}
				finally
				{
					driveTree.EndUpdate();
				}

				UpdateLoadedTreeSelectionStates();
				await ReplayDeferredHyperVSelectionsAsync().ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[BackupNewForm] Failed to populate drive tree: {ex.Message}");

				if (!IsDisposed && !driveTree.IsDisposed)
				{
					driveTree.Nodes.Clear();
					driveTree.Nodes.Add(new TreeNode("No disks, Hyper-V systems, or network locations were detected."));
				}
			}
			finally
			{
				SetDriveTreeLoadingState(false);
				isPopulatingDriveTree = false;
			}
		}

		private async Task ReplayDeferredHyperVSelectionsAsync()
		{
			bool hasSavedHyperVVirtualDiskSelections = nativeSourcePaths.Any(path =>
				!string.IsNullOrWhiteSpace(path) &&
				(path.EndsWith(".vhd", StringComparison.OrdinalIgnoreCase) ||
				 path.EndsWith(".vhdx", StringComparison.OrdinalIgnoreCase)));

			if (!hasSavedHyperVVirtualDiskSelections)
			{
				return;
			}

			foreach (TreeNode node in driveTree.Nodes)
			{
				if (node.Tag is not SourceTreeNodeData nodeData || nodeData.Kind != SourceTreeNodeKind.HyperVSystem)
				{
					continue;
				}

				await LoadHyperVVirtualDisksAsync(node, nodeData).ConfigureAwait(true);
			}

			UpdateLoadedTreeSelectionStates();
		}

		private void SetDriveTreeLoadingState(bool isLoading, string? statusText = null)
		{
			driveTreeStatusLabel.Text = statusText ?? "Loading backup sources...";
			driveTreeStatusLabel.Visible = isLoading;
			driveTreeLoadProgressBar.Visible = isLoading;
			refreshDriveTreeButton.Enabled = !isLoading;
			expandTreeButton.Enabled = !isLoading;
			collapseTreeButton.Enabled = !isLoading;
			showHiddenPartitionsCheckBox.Enabled = !isLoading;
			driveTree.Enabled = !isLoading;
		}

		private List<SourceRootDescriptor> LoadDriveTreeRootDescriptors(bool showHidden)
		{
			List<SourceRootDescriptor> descriptors = LoadPhysicalDiskRootDescriptors();
			if (descriptors.Count == 0)
			{
				descriptors.AddRange(LoadLogicalVolumeRootDescriptors(showHidden));
			}

			descriptors.AddRange(LoadHyperVSystemRootDescriptors());
			descriptors.Add(new SourceRootDescriptor(SourceTreeNodeKind.NetworkRoot, "Network Locations", AddPlaceholder: true));
			return descriptors;
		}

		private List<SourceRootDescriptor> LoadHyperVSystemRootDescriptors()
		{
			return EnumerateHyperVVirtualMachines()
				.Select(virtualMachine => new SourceRootDescriptor(
					SourceTreeNodeKind.HyperVSystem,
					virtualMachine.DisplayName,
					SelectionPath: virtualMachine.VirtualMachineName,
					VirtualMachineName: virtualMachine.VirtualMachineName,
					AddPlaceholder: true))
				.ToList();
		}

		private List<SourceRootDescriptor> LoadPhysicalDiskRootDescriptors()
		{
			List<SourceRootDescriptor> descriptors = new();

			try
			{
				using var diskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive ORDER BY Index");
				foreach (ManagementObject disk in diskSearcher.Get())
				{
					if (!int.TryParse(disk["Index"]?.ToString(), out int diskIndex))
					{
						continue;
					}

					string diskModel = string.IsNullOrWhiteSpace(disk["Model"]?.ToString())
						? $"PhysicalDrive{diskIndex}"
						: disk["Model"]!.ToString()!.Trim();

					descriptors.Add(new SourceRootDescriptor(
						SourceTreeNodeKind.Disk,
						$"Disk {diskIndex} - {diskModel} ({FormatStorageSize(TryReadInt64(disk["Size"]))})",
						DiskNumber: diskIndex,
						SelectionPath: $@"\\.\PHYSICALDRIVE{diskIndex}",
						AddPlaceholder: true));
				}
			}
			catch
			{
			}

			return descriptors;
		}

		private List<SourceRootDescriptor> LoadLogicalVolumeRootDescriptors(bool showHidden)
		{
			List<SourceRootDescriptor> descriptors = new();

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

					string rootPath = driveInfo.RootDirectory.FullName;
					if (!showHidden && string.IsNullOrWhiteSpace(rootPath))
					{
						continue;
					}

					descriptors.Add(new SourceRootDescriptor(
						SourceTreeNodeKind.Volume,
						FormatVolumeNodeText(driveInfo),
						DiskNumber: TryGetDiskNumberForDrive(driveInfo.Name) ?? -1,
						SelectionPath: driveInfo.Name,
						FileSystemPath: rootPath,
						AddPlaceholder: true));
				}
				catch
				{
				}
			}

			return descriptors;
		}

		private TreeNode CreateRootNode(SourceRootDescriptor descriptor)
		{
			TreeNode node = new(descriptor.Text)
			{
				Tag = new SourceTreeNodeData
				{
					Kind = descriptor.Kind,
					DiskNumber = descriptor.DiskNumber,
					SelectionPath = descriptor.SelectionPath,
					FileSystemPath = descriptor.FileSystemPath,
					VirtualMachineName = descriptor.VirtualMachineName,
					IsRemovableNetworkPath = descriptor.IsRemovableNetworkPath
				},
				ImageKey = GetTreeImageKey(descriptor.Kind),
				SelectedImageKey = GetTreeImageKey(descriptor.Kind)
			};

			if (descriptor.AddPlaceholder)
			{
				node.Nodes.Add(CreatePlaceholderNode());
			}

			ApplySelectionStateToNode(node);
			return node;
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
			SourceTreeNodeKind.HyperVSystem => driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
			SourceTreeNodeKind.HyperVVirtualDisk => driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
			SourceTreeNodeKind.NetworkDrive => driveImageList.Images.ContainsKey("drive") ? "drive" : string.Empty,
			SourceTreeNodeKind.NetworkShare => driveImageList.Images.ContainsKey("folder") ? "folder" : string.Empty,
			SourceTreeNodeKind.NetworkBrowser => driveImageList.Images.ContainsKey("folder") ? "folder" : string.Empty,
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
				case SourceTreeNodeKind.HyperVRoot:
					e.Cancel = true;
					_ = LoadHyperVSystemsAsync(e.Node);
					break;
				case SourceTreeNodeKind.HyperVSystem:
					e.Cancel = true;
					_ = LoadHyperVVirtualDisksAsync(e.Node, nodeData);
					break;
				case SourceTreeNodeKind.NetworkRoot:
					e.Cancel = true;
					_ = LoadNetworkLocationsAsync(e.Node);
					break;
				case SourceTreeNodeKind.Volume:
				case SourceTreeNodeKind.NetworkDrive:
				case SourceTreeNodeKind.NetworkShare:
					e.Cancel = true;
					_ = LoadNodeFileSystemChildrenAsync(e.Node, nodeData, animateNode: true);
					break;
				case SourceTreeNodeKind.Directory:
					e.Cancel = true;
					_ = LoadNodeFileSystemChildrenAsync(e.Node, nodeData, animateNode: false);
					break;
			}
		}

		private async Task LoadHyperVSystemsAsync(TreeNode parentNode)
		{
			ArgumentNullException.ThrowIfNull(parentNode);

			if (loadingVolumeNodes.Contains(parentNode))
			{
				return;
			}

			BeginNodeLoading(parentNode, animateNode: true);
			try
			{
				List<HyperVVirtualMachineInfo> virtualMachines = await Task.Run(EnumerateHyperVVirtualMachines).ConfigureAwait(true);

				if (IsDisposed || driveTree.IsDisposed)
				{
					return;
				}

				parentNode.Nodes.Clear();
				foreach (HyperVVirtualMachineInfo virtualMachine in virtualMachines)
				{
					TreeNode childNode = new(virtualMachine.DisplayName)
					{
						Tag = new SourceTreeNodeData
						{
							Kind = SourceTreeNodeKind.HyperVSystem,
							SelectionPath = virtualMachine.VirtualMachineName,
							VirtualMachineName = virtualMachine.VirtualMachineName
						},
						ImageKey = GetTreeImageKey(SourceTreeNodeKind.HyperVSystem),
						SelectedImageKey = GetTreeImageKey(SourceTreeNodeKind.HyperVSystem)
					};

					childNode.Nodes.Add(CreatePlaceholderNode());
					ApplySelectionStateToNode(childNode);
					parentNode.Nodes.Add(childNode);
				}

				if (parentNode.Nodes.Count == 0)
				{
					parentNode.Nodes.Add(new TreeNode("No Hyper-V systems detected."));
				}

				parentNode.Expand();
			}
			catch
			{
				if (!IsDisposed && !driveTree.IsDisposed)
				{
					parentNode.Nodes.Clear();
					parentNode.Nodes.Add(new TreeNode("Unable to load Hyper-V systems."));
					parentNode.Expand();
				}
			}
			finally
			{
				EndNodeLoading(parentNode, animateNode: true, SourceTreeNodeKind.HyperVRoot);
			}
		}

		private async Task LoadHyperVVirtualDisksAsync(TreeNode parentNode, SourceTreeNodeData nodeData)
		{
			ArgumentNullException.ThrowIfNull(parentNode);
			ArgumentNullException.ThrowIfNull(nodeData);

			if (loadingVolumeNodes.Contains(parentNode) || string.IsNullOrWhiteSpace(nodeData.VirtualMachineName))
			{
				return;
			}

			BeginNodeLoading(parentNode, animateNode: true);
			try
			{
				List<string> virtualDiskPaths = await Task.Run(() => EnumerateHyperVVirtualDisks(nodeData.VirtualMachineName)).ConfigureAwait(true);

				if (IsDisposed || driveTree.IsDisposed)
				{
					return;
				}

				parentNode.Nodes.Clear();
				foreach (string virtualDiskPath in virtualDiskPaths)
				{
					TreeNode childNode = new(Path.GetFileName(virtualDiskPath))
					{
						Tag = new SourceTreeNodeData
						{
							Kind = SourceTreeNodeKind.HyperVVirtualDisk,
							SelectionPath = virtualDiskPath,
							VirtualMachineName = nodeData.VirtualMachineName
						},
						ImageKey = GetTreeImageKey(SourceTreeNodeKind.HyperVVirtualDisk),
						SelectedImageKey = GetTreeImageKey(SourceTreeNodeKind.HyperVVirtualDisk)
					};

					ApplySelectionStateToNode(childNode);
					parentNode.Nodes.Add(childNode);
				}

				if (parentNode.Nodes.Count == 0)
				{
					parentNode.Nodes.Add(new TreeNode("No Hyper-V virtual disks detected."));
				}

				parentNode.Expand();
			}
			catch
			{
				if (!IsDisposed && !driveTree.IsDisposed)
				{
					parentNode.Nodes.Clear();
					parentNode.Nodes.Add(new TreeNode("Unable to load Hyper-V virtual disks."));
					parentNode.Expand();
				}
			}
			finally
			{
				EndNodeLoading(parentNode, animateNode: true, SourceTreeNodeKind.HyperVSystem);
			}
		}

		private async Task LoadNetworkLocationsAsync(TreeNode parentNode)
		{
			ArgumentNullException.ThrowIfNull(parentNode);

			if (loadingVolumeNodes.Contains(parentNode))
			{
				return;
			}

			BeginNodeLoading(parentNode, animateNode: true);
			try
			{
				List<SourceRootDescriptor> networkDescriptors = await Task.Run(LoadNetworkLocationDescriptors).ConfigureAwait(true);

				if (IsDisposed || driveTree.IsDisposed)
				{
					return;
				}

				parentNode.Nodes.Clear();
				foreach (SourceRootDescriptor descriptor in networkDescriptors)
				{
					parentNode.Nodes.Add(CreateRootNode(descriptor));
				}

				if (parentNode.Nodes.Count == 0)
				{
					parentNode.Nodes.Add(new TreeNode("No network locations detected."));
				}

				parentNode.Expand();
			}
			catch
			{
				if (!IsDisposed && !driveTree.IsDisposed)
				{
					parentNode.Nodes.Clear();
					parentNode.Nodes.Add(new TreeNode("Unable to load network locations."));
					parentNode.Expand();
				}
			}
			finally
			{
				EndNodeLoading(parentNode, animateNode: true, SourceTreeNodeKind.NetworkRoot);
			}
		}

		private List<SourceRootDescriptor> LoadNetworkLocationDescriptors()
		{
			List<SourceRootDescriptor> descriptors = new();
			List<string> savedPaths = savedNetworkPaths.ToList();

			foreach (DriveInfo driveInfo in DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Network))
			{
				try
				{
					descriptors.Add(new SourceRootDescriptor(
						SourceTreeNodeKind.NetworkDrive,
						$"{driveInfo.Name.TrimEnd('\\')} - Mapped",
						SelectionPath: driveInfo.Name,
						FileSystemPath: driveInfo.RootDirectory.FullName,
						AddPlaceholder: true));
				}
				catch
				{
				}
			}

			foreach (string savedNetworkPath in savedPaths)
			{
				descriptors.Add(new SourceRootDescriptor(
					SourceTreeNodeKind.NetworkShare,
					savedNetworkPath,
					SelectionPath: savedNetworkPath,
					FileSystemPath: savedNetworkPath,
					AddPlaceholder: true,
					IsRemovableNetworkPath: true));
			}

			descriptors.Add(new SourceRootDescriptor(SourceTreeNodeKind.NetworkBrowser, "Add Network Path..."));
			return descriptors;
		}

		private static List<HyperVVirtualMachineInfo> EnumerateHyperVVirtualMachines()
		{
			List<HyperVVirtualMachineInfo> virtualMachines = new();
			foreach (string csvLine in RunPowerShellLines("Get-VM | Select-Object -Property Name,State | ConvertTo-Csv -NoTypeInformation").Skip(1))
			{
				string[] parts = ParseCsvLine(csvLine);
				if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]))
				{
					continue;
				}

				string vmName = parts[0].Trim();
				string vmState = parts[1].Trim();
				string displayName = string.Equals(vmState, "Running", StringComparison.OrdinalIgnoreCase)
					? $"{vmName} (Running)"
					: vmName;

				virtualMachines.Add(new HyperVVirtualMachineInfo(vmName, displayName));
			}

			return virtualMachines;
		}

		private static List<string> EnumerateHyperVVirtualDisks(string virtualMachineName)
		{
			string escapedVmName = virtualMachineName.Replace("'", "''", StringComparison.Ordinal);
			return RunPowerShellLines($"Get-VMHardDiskDrive -VMName '{escapedVmName}' | Select-Object -ExpandProperty Path")
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Select(path => path.Trim().Trim('"'))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		private static List<string> RunPowerShellLines(string command)
		{
			using var process = Process.Start(new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});

			if (process == null)
			{
				return new List<string>();
			}

			string output = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			return output
				.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Trim())
				.Where(line => !string.IsNullOrWhiteSpace(line))
				.ToList();
		}

		private static string[] ParseCsvLine(string csvLine)
		{
			if (string.IsNullOrWhiteSpace(csvLine))
			{
				return Array.Empty<string>();
			}

			return csvLine.Trim().Trim('"').Split(new[] { "\",\"" }, StringSplitOptions.None);
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

		internal static List<DriveTreeItem> BuildDirectoryChildItems(DriveTreeItem parentItem, string rootPath)
		{
			ArgumentNullException.ThrowIfNull(parentItem);
			ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);

			List<DriveTreeItem> children = [];

			foreach (string directoryPath in Directory.EnumerateDirectories(rootPath))
			{
				DirectoryInfo directoryInfo = new(directoryPath);
				children.Add(CreateDirectoryChildItem(parentItem, directoryInfo.FullName, directoryInfo.Name, DriveTreeItemType.Folder));
			}

			foreach (string filePath in Directory.EnumerateFiles(rootPath))
			{
				FileInfo fileInfo = new(filePath);
				children.Add(CreateDirectoryChildItem(parentItem, fileInfo.FullName, fileInfo.Name, DriveTreeItemType.File));
			}

			return children;
		}

		internal static bool IsSelectedFilesAndFoldersSelectionAllowed(IEnumerable<DriveTreeItem> selectedItems)
		{
			ArgumentNullException.ThrowIfNull(selectedItems);

			foreach (DriveTreeItem item in selectedItems)
			{
				if (item.ItemType is not DriveTreeItemType.File and not DriveTreeItemType.Folder)
				{
					return false;
				}

				if (HyperVGuestSelectionPath.IsEncodedPath(item.FullPath) &&
					!HyperVGuestSelectionPath.TryParse(item.FullPath, out HyperVGuestSelectionInfo? selection))
				{
					return false;
				}
			}

			return true;
		}

		internal static bool IsValidWindowsComputerName(string? name)
		{
			if (string.IsNullOrWhiteSpace(name) || name.Length > 15)
			{
				return false;
			}

			if (name.StartsWith("-", StringComparison.Ordinal) || name.EndsWith("-", StringComparison.Ordinal))
			{
				return false;
			}

			return name.All(ch => char.IsLetterOrDigit(ch) || ch == '-');
		}

		internal static IReadOnlyList<string> GetReplayPathsForJob(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			if ((job.Type == BackupType.CloneHyperVSystem ||
				 job.Type == BackupType.ExportHyperVSystem ||
				 job.Target == BackupTarget.HyperV ||
				 job.IsHyperVBackup) &&
				job.HyperVMachines.Count > 0)
			{
				return job.HyperVMachines
					.Where(name => !string.IsNullOrWhiteSpace(name))
					.Select(NormalizeHyperVDisplayName)
					.Where(name => !string.IsNullOrWhiteSpace(name))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToArray();
			}

			return job.SourcePaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();
		}

		internal static SecureServerBackup.Helpers.CloneHyperVPaths CreateCloneHyperVPaths(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			if (string.IsNullOrWhiteSpace(job.DestinationPath))
			{
				throw new InvalidOperationException("Clone Hyper-V System requires a destination folder.");
			}

			bool renameRequested = job.RenameHyperVSystem && !string.IsNullOrWhiteSpace(job.RenameHyperVSystemName);
			bool diskClone = job.Target == BackupTarget.Disk && job.SourcePaths.Count > 0;
			string vmName = renameRequested
				? job.RenameHyperVSystemName!.Trim()
				: diskClone
					? Environment.MachineName
					: job.Name;
			string rootDirectoryName = renameRequested ? vmName : job.Name;
			string rootDirectory = Path.Combine(job.DestinationPath, rootDirectoryName);

			Directory.CreateDirectory(rootDirectory);

			string virtualDiskPath = Path.Combine(rootDirectory, $"{vmName}.vhdx");
			return new SecureServerBackup.Helpers.CloneHyperVPaths(rootDirectory, rootDirectory, rootDirectory, virtualDiskPath, vmName);
		}

		internal static string BuildMissingSavedSelectionsWarningMessage(BackupType backupType, IReadOnlyCollection<string> missingSelections)
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

		private static DriveTreeItem CreateDirectoryChildItem(DriveTreeItem parentItem, string resolvedPath, string name, DriveTreeItemType itemType)
		{
			ArgumentNullException.ThrowIfNull(parentItem);
			ArgumentException.ThrowIfNullOrWhiteSpace(resolvedPath);
			ArgumentException.ThrowIfNullOrWhiteSpace(name);

			string fullPath = resolvedPath;
			if (parentItem.ItemType == DriveTreeItemType.HyperVVolume &&
				HyperVGuestSelectionPath.TryParse(parentItem.FullPath, out HyperVGuestSelectionInfo? parentSelection))
			{
				string relativePath = Path.GetRelativePath(parentItem.ResolvedPath, resolvedPath)
					.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

				fullPath = HyperVGuestSelectionPath.Encode(
					itemType == DriveTreeItemType.File ? HyperVGuestSelectionKind.File : HyperVGuestSelectionKind.Folder,
					parentSelection!.VirtualMachineName,
					parentSelection.VirtualDiskPath,
					parentSelection.PartitionNumber,
					relativePath);
			}

			return new DriveTreeItem
			{
				Name = name,
				FullPath = fullPath,
				ResolvedPath = resolvedPath,
				VirtualMachineName = parentItem.VirtualMachineName,
				VirtualDiskPath = parentItem.VirtualDiskPath,
				PartitionNumber = parentItem.PartitionNumber,
				ItemType = itemType,
				Parent = parentItem
			};
		}

		private static string NormalizeHyperVDisplayName(string name)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name);

			string normalized = name.Trim();
			const string hyperVPrefix = "Hyper-V:";
			if (normalized.StartsWith(hyperVPrefix, StringComparison.OrdinalIgnoreCase))
			{
				normalized = normalized[hyperVPrefix.Length..].Trim();
			}

			int statusIndex = normalized.LastIndexOf(" (", StringComparison.Ordinal);
			if (statusIndex > 0 && normalized.EndsWith(")", StringComparison.Ordinal))
			{
				normalized = normalized[..statusIndex].TrimEnd();
			}

			return normalized;
		}

		private static bool HasBackupArchive(string directoryPath, string archiveName)
		{
			if (string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(archiveName))
			{
				return false;
			}

			string filePath = Path.Combine(directoryPath, archiveName + ".ssb");
			string folderPath = Path.Combine(directoryPath, archiveName + ".ssb");
			return File.Exists(filePath) || Directory.Exists(folderPath);
		}

		private static bool RequiresLazyLoad(DriveTreeItem item)
		{
			ArgumentNullException.ThrowIfNull(item);

			if (item.ChildrenLoaded)
			{
				return false;
			}

			return item.ItemType switch
			{
				DriveTreeItemType.Volume or DriveTreeItemType.Folder or DriveTreeItemType.HyperVVolume => !string.IsNullOrWhiteSpace(item.ResolvedPath),
				DriveTreeItemType.HyperVVirtualDisk => !string.IsNullOrWhiteSpace(item.VirtualDiskPath) || !string.IsNullOrWhiteSpace(item.ResolvedPath),
				_ => false
			};
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

			if (nodeData.Kind == SourceTreeNodeKind.NetworkBrowser)
			{
				bool openDialog = e.Node.Checked;
				suppressTreeCheckSync = true;
				try
				{
					e.Node.Checked = false;
				}
				finally
				{
					suppressTreeCheckSync = false;
				}

				if (openDialog)
				{
					AddNetworkPathFromDialog();
				}

				return;
			}

			if (string.IsNullOrWhiteSpace(nodeData.SelectionPath))
			{
				suppressTreeCheckSync = true;
				try
				{
					e.Node.Checked = false;
				}
				finally
				{
					suppressTreeCheckSync = false;
				}

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

		private void AddNetworkPathFromDialog()
		{
			using var dialog = new NetworkPathForm();
			if (dialog.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.NetworkPath))
			{
				return;
			}

			string networkPath = dialog.NetworkPath.Trim();
			if (!savedNetworkPaths.Contains(networkPath, StringComparer.OrdinalIgnoreCase))
			{
				savedNetworkPaths.Add(networkPath);
				SavedNetworkPathStore.Add(networkPath);
			}

			AddNativeSourcePath(networkPath);
			RefreshNetworkRootNode();
		}

		private void RefreshNetworkRootNode()
		{
			foreach (TreeNode node in driveTree.Nodes)
			{
				if (node.Tag is SourceTreeNodeData nodeData && nodeData.Kind == SourceTreeNodeKind.NetworkRoot)
				{
					node.Nodes.Clear();
					node.Nodes.Add(CreatePlaceholderNode());
					if (node.IsExpanded)
					{
						_ = LoadNetworkLocationsAsync(node);
					}

					break;
				}
			}
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

			if (nodeData.Kind == SourceTreeNodeKind.HyperVSystem)
			{
				node.Checked = nativeSourcePaths.Any(existingPath =>
					string.Equals(NormalizeHyperVDisplayName(existingPath), nodeData.VirtualMachineName, StringComparison.OrdinalIgnoreCase) ||
					string.Equals(existingPath, nodeData.VirtualMachineName, StringComparison.OrdinalIgnoreCase));
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
			return true;
		}

		private SelectedSourceState CollectSelectedSourceState()
		{
			SelectedSourceState state = new();

			foreach (TreeNode rootNode in driveTree.Nodes)
			{
				CollectSelectedSourceState(rootNode, state);
			}

			if (state.SourcePaths.Count == 0 && state.HyperVMachines.Count == 0)
			{
				state.SourcePaths.AddRange(nativeSourcePaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase));
			}

			state.SourcePaths = state.SourcePaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();

			state.HyperVMachines = state.HyperVMachines
				.Where(name => !string.IsNullOrWhiteSpace(name))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();

			return state;
		}

		private void CollectSelectedSourceState(TreeNode node, SelectedSourceState state)
		{
			ArgumentNullException.ThrowIfNull(node);
			ArgumentNullException.ThrowIfNull(state);

			if (node.Checked && node.Tag is SourceTreeNodeData nodeData)
			{
				switch (nodeData.Kind)
				{
					case SourceTreeNodeKind.Disk:
						state.HasDiskSelection = true;
						state.SourcePaths.Add(nodeData.SelectionPath);
						break;
					case SourceTreeNodeKind.Volume:
					case SourceTreeNodeKind.Partition:
						state.HasVolumeSelection = true;
						state.SourcePaths.Add(nodeData.SelectionPath);
						break;
					case SourceTreeNodeKind.Directory:
					case SourceTreeNodeKind.File:
					case SourceTreeNodeKind.NetworkDrive:
					case SourceTreeNodeKind.NetworkShare:
						state.HasFileSystemSelection = true;
						state.SourcePaths.Add(nodeData.SelectionPath);
						break;
					case SourceTreeNodeKind.HyperVSystem:
						state.HasHyperVSelection = true;
						state.HyperVMachines.Add(nodeData.VirtualMachineName);
						break;
					case SourceTreeNodeKind.HyperVVirtualDisk:
						state.HasHyperVSelection = true;
						state.SourcePaths.Add(nodeData.SelectionPath);
						state.HyperVMachines.Add(nodeData.VirtualMachineName);
						break;
				}
			}

			foreach (TreeNode childNode in node.Nodes)
			{
				if (!IsPlaceholderNode(childNode))
				{
					CollectSelectedSourceState(childNode, state);
				}
			}
		}

		private void UpdateAdvancedStateSummary()
		{
			if (!advancedSelectionListBox.Visible)
			{
				return;
			}

			advancedSelectionListBox.Items.Clear();

			BackupJob? job = currentJob ?? existingJob;
			if (job == null)
			{
				if (nativeSourcePaths.Count > 0)
				{
					advancedStateLabel.Text = $"Native backup sources staged for save: {nativeSourcePaths.Count} item(s).";
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
					advancedStateLabel.Text = "No source selection has been configured yet. Choose disks, volumes, files, folders, Hyper-V systems, or network locations from the tree.";
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
				return $"Selected file and folder backup currently staged natively: {selectionText}. You can save this backup directly from WinForms.";
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
			return $"{targetText} currently attached to this job: {countText}. You can edit these source selections directly from this WinForms screen.";
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
			bool isSelectedFiles = backupType == BackupType.SelectedFilesAndFolders;
			bool isCloneOrExport = backupType == BackupType.CloneToVirtualDisk || backupType == BackupType.CloneHyperVSystem || backupType == BackupType.ExportHyperVSystem;
			bool nativeSourceSelectionSupported = IsNativeSourceSelectionSupportedForCurrentState();
			bool isNativeSaveSupported = nativeSourceSelectionSupported;

			retentionPanel.Visible = backupType == BackupType.Full;
			selectedFilesRetentionPanel.Visible = isSelectedFiles;
			cloneRetentionPanel.Visible = isCloneOrExport;
			saveJobButton.Enabled = isNativeSaveSupported;
			startBackupButton.Enabled = true;
			openAdvancedEditorButton.Enabled = false;
			openAdvancedEditorButton.Visible = false;
			startBackupButton.Text = "Start Backup";
			actionInfoLabel.Text = isSelectedFiles
				? "WinForms backup status\n\n- Native: common settings, encryption, schedule, selected-file retention, source tree, Hyper-V list, network locations, save, and start-now support\n- This form is the primary authoring surface for current backup jobs"
				: "WinForms backup status\n\n- Native: common settings, encryption, schedule, retention, source tree, Hyper-V list, network locations, save, and start-now support\n- This form is the primary authoring surface for all backup types";
			actionHelpLabel.Text = isSelectedFiles
				? "Selected Files & Folder jobs support files, folders, and network shares directly from this screen. Use the tree or the Add Folder and Add File buttons to build the selection list."
				: "Use the source tree to choose disks, volumes, Hyper-V systems, and network locations. Use this screen to save and start all backup types, including clone and export jobs.";
			UpdateNativeSourceUi();
			UpdateCloneOptionsUi();
			UpdateAdvancedStateSummary();
		}

		private void UpdateCloneOptionsUi()
		{
			bool isCloneHyperVSystem = GetSelectedBackupType() == BackupType.CloneHyperVSystem;
			renameHyperVSystemCheckBox.Visible = isCloneHyperVSystem;
			renameHyperVSystemNameTextBox.Visible = isCloneHyperVSystem;
			renameHyperVSystemNameTextBox.Enabled = isCloneHyperVSystem && renameHyperVSystemCheckBox.Checked;
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
				"All backup types are now expected to be configured directly from this WinForms screen. If something is still missing here, it should be implemented on this form instead of using a separate editor.",
				"WinForms Backup Editor",
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
						$"Backup job '{job.Name}' updated successfully!\n\nThe current WinForms selections and settings were saved with the job.",
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
						$"Backup job '{job.Name}' created successfully!\n\nThe current WinForms selections and settings were saved with the job.",
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

			SelectedSourceState selectedSourceState = CollectSelectedSourceState();

			if (IsNativeSourceSelectionSupportedForCurrentState() && selectedSourceState.SourcePaths.Count == 0 && selectedSourceState.HyperVMachines.Count == 0)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please select at least one backup source.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			bool allowsDiskPlusHyperV = backupType == BackupType.CloneHyperVSystem;
			if (!allowsDiskPlusHyperV && selectedSourceState.HasHyperVSelection && (selectedSourceState.HasDiskSelection || selectedSourceState.HasVolumeSelection || selectedSourceState.HasFileSystemSelection))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Hyper-V system selections cannot be combined with disk, volume, file, or network selections in the same backup job.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.SelectedFilesAndFolders && (selectedSourceState.HasDiskSelection || selectedSourceState.HasVolumeSelection || selectedSourceState.HasHyperVSelection))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Selected Files & Folder backups only support file, folder, and network-share sources.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.CloneToVirtualDisk && (selectedSourceState.HasFileSystemSelection || selectedSourceState.HasHyperVSelection || (!selectedSourceState.HasDiskSelection && !selectedSourceState.HasVolumeSelection)))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Clone to Virtual Disk requires at least one disk or volume source and does not support file, network, or Hyper-V system selections.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.CloneHyperVSystem && selectedSourceState.HasVolumeSelection)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Clone Hyper-V System supports either a Hyper-V system selection or a disk selection. Volume selections are not supported for this backup type.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.CloneHyperVSystem && selectedSourceState.HasFileSystemSelection)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Clone Hyper-V System does not support file or network selections. Choose a Hyper-V system or a disk source.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.CloneHyperVSystem && !selectedSourceState.HasHyperVSelection && !selectedSourceState.HasDiskSelection)
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Clone Hyper-V System requires either a selected Hyper-V system or a selected disk.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.ExportHyperVSystem && (selectedSourceState.HasDiskSelection || selectedSourceState.HasVolumeSelection || selectedSourceState.HasFileSystemSelection || !selectedSourceState.HasHyperVSelection))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Export Hyper-V System requires a selected Hyper-V system and does not support disk, volume, file, or network selections.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}

			if (backupType == BackupType.CloneHyperVSystem && renameHyperVSystemCheckBox.Checked && !IsValidWindowsComputerName(renameHyperVSystemNameTextBox.Text.Trim()))
			{
				global::System.Windows.Forms.MessageBox.Show(this, "Please enter a valid new Hyper-V system name. Use letters, numbers, or hyphens, and do not start or end the name with a hyphen.", "Validation Error", global::System.Windows.Forms.MessageBoxButtons.OK, global::System.Windows.Forms.MessageBoxIcon.Warning);
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
			SelectedSourceState selectedSourceState = CollectSelectedSourceState();
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
			job.RenameHyperVSystem = backupType == BackupType.CloneHyperVSystem && renameHyperVSystemCheckBox.Checked;
			job.RenameHyperVSystemName = job.RenameHyperVSystem ? renameHyperVSystemNameTextBox.Text.Trim() : string.Empty;

			if (backupType == BackupType.SelectedFilesAndFolders)
			{
				job.Target = BackupTarget.FilesAndFolders;
				job.IsHyperVBackup = false;
				job.HyperVMachines.Clear();
				job.SourcePaths = selectedSourceState.SourcePaths.ToList();
				job.SelectedFilesSourceRoots = GetSelectedFilesSourceRoots(job.SourcePaths);
			}
			else if (CanUseNativeSourceSelection(backupType, sourceJob?.Target ?? BackupTarget.FilesAndFolders))
			{
				job.Target = backupType switch
				{
					BackupType.CloneHyperVSystem when selectedSourceState.HasHyperVSelection => BackupTarget.HyperV,
					BackupType.CloneHyperVSystem when selectedSourceState.HasDiskSelection => BackupTarget.Disk,
					BackupType.ExportHyperVSystem => BackupTarget.HyperV,
					_ => selectedSourceState.HasHyperVSelection
						? BackupTarget.HyperV
						: selectedSourceState.HasDiskSelection
							? BackupTarget.Disk
							: selectedSourceState.HasVolumeSelection
								? BackupTarget.Volume
								: BackupTarget.FilesAndFolders
				};
				job.IsHyperVBackup = selectedSourceState.HasHyperVSelection;
				job.HyperVMachines = selectedSourceState.HyperVMachines.ToList();
				job.SelectedFilesSourceRoots.Clear();
				job.SourcePaths = selectedSourceState.SourcePaths.ToList();
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

		private void BackupNewForm_FormClosed(object sender, FormClosedEventArgs e)
		{

		}

		private sealed record SourceRootDescriptor(
			SourceTreeNodeKind Kind,
			string Text,
			int DiskNumber = -1,
			string SelectionPath = "",
			string FileSystemPath = "",
			string VirtualMachineName = "",
			bool AddPlaceholder = false,
			bool IsRemovableNetworkPath = false);

		private sealed record HyperVVirtualMachineInfo(string VirtualMachineName, string DisplayName);

		private sealed class SelectedSourceState
		{
			public List<string> SourcePaths { get; set; } = new();
			public List<string> HyperVMachines { get; set; } = new();
			public bool HasDiskSelection { get; set; }
			public bool HasVolumeSelection { get; set; }
			public bool HasFileSystemSelection { get; set; }
			public bool HasHyperVSelection { get; set; }
		}
	}
}
