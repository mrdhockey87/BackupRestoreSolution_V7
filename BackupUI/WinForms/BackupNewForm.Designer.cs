using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class BackupNewForm
	{
		private IContainer components;
		private Label headerLabel;
		private Panel settingsScroll;
		private TableLayoutPanel settingsStack;
		private GroupBox basicGroup;
		private TextBox backupNameTextBox;
		private ComboBox backupTypeComboBox;
		private TextBox destinationTextBox;
		private CheckBox compressCheckBox;
		private CheckBox verifyCheckBox;
		private GroupBox exclusionsGroup;
		private Button manageExclusionsButton;
		private GroupBox encryptionGroup;
		private CheckBox encryptCheckBox;
		private Panel encryptionPanel;
		private TextBox encryptionPasswordTextBox;
		private TextBox verifyEncryptionPasswordTextBox;
		private CheckBox showPasswordCheckBox;
		private GroupBox scheduleGroup;
		private CheckBox enableScheduleCheckBox;
		private Panel schedulePanel;
		private ComboBox frequencyComboBox;
		private ComboBox hourComboBox;
		private ComboBox minuteComboBox;
		private ComboBox amPmComboBox;
		private Panel weeklyPanel;
		private CheckedListBox weeklyDaysCheckedListBox;
		private Panel monthlyPanel;
		private ComboBox dayOfMonthComboBox;
		private Panel retentionPanel;
		private TextBox retainCountTextBox;
		private Panel selectedFilesRetentionPanel;
		private ComboBox selectedFilesRetentionComboBox;
		private Panel cloneRetentionPanel;
		private ComboBox cloneRetentionComboBox;
		private Label nativeCoverageLabel;
		private Label actionInfoLabel;
		private Label actionHelpLabel;
		private Label advancedStateLabel;
		private ListBox advancedSelectionListBox;
		private Button openAdvancedEditorButton;
		private Button saveJobButton;
		private Button startBackupButton;
		private TreeView driveTree;
		private Button addFolderSourceButton;
		private Button addFileSourceButton;
		private Button removeSourceButton;
		private Button refreshDriveTreeButton;
		private Button expandTreeButton;
		private Button collapseTreeButton;
		private CheckBox showHiddenPartitionsCheckBox;
		private ListBox nativeSourceListBox;
		private ImageList driveImageList;
		private Timer volumeAnimationTimer;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			driveImageList = new ImageList(components);
			volumeAnimationTimer = new Timer(components);
			headerLabel = new Label();
			settingsScroll = new Panel();
			settingsStack = new TableLayoutPanel();
			basicGroup = new GroupBox();
			backupNameTextBox = new TextBox();
			backupTypeComboBox = new ComboBox();
			destinationTextBox = new TextBox();
			compressCheckBox = new CheckBox();
			verifyCheckBox = new CheckBox();
			exclusionsGroup = new GroupBox();
			manageExclusionsButton = new Button();
			encryptionGroup = new GroupBox();
			encryptCheckBox = new CheckBox();
			encryptionPanel = new Panel();
			encryptionPasswordTextBox = new TextBox();
			verifyEncryptionPasswordTextBox = new TextBox();
			showPasswordCheckBox = new CheckBox();
			scheduleGroup = new GroupBox();
			enableScheduleCheckBox = new CheckBox();
			schedulePanel = new Panel();
			frequencyComboBox = new ComboBox();
			hourComboBox = new ComboBox();
			minuteComboBox = new ComboBox();
			amPmComboBox = new ComboBox();
			weeklyPanel = new Panel();
			weeklyDaysCheckedListBox = new CheckedListBox();
			monthlyPanel = new Panel();
			dayOfMonthComboBox = new ComboBox();
			retentionPanel = new Panel();
			retainCountTextBox = new TextBox();
			selectedFilesRetentionPanel = new Panel();
			selectedFilesRetentionComboBox = new ComboBox();
			cloneRetentionPanel = new Panel();
			cloneRetentionComboBox = new ComboBox();
			nativeCoverageLabel = new Label();
			actionInfoLabel = new Label();
			actionHelpLabel = new Label();
			advancedStateLabel = new Label();
			advancedSelectionListBox = new ListBox();
			openAdvancedEditorButton = new Button();
			saveJobButton = new Button();
			startBackupButton = new Button();
			driveTree = new TreeView();
			addFolderSourceButton = new Button();
			addFileSourceButton = new Button();
			removeSourceButton = new Button();
			refreshDriveTreeButton = new Button();
			expandTreeButton = new Button();
			collapseTreeButton = new Button();
			showHiddenPartitionsCheckBox = new CheckBox();
			nativeSourceListBox = new ListBox();
			var sourceSelectionGroup = new GroupBox();
			var advancedStateGroup = new GroupBox();
			var leftColumnLayout = new TableLayoutPanel();
			SuspendLayout();
			// 
			// driveImageList
			// 
			driveImageList.ColorDepth = ColorDepth.Depth32Bit;
			driveImageList.ImageSize = new Size(16, 16);
			driveImageList.TransparentColor = Color.Transparent;
			// 
			// volumeAnimationTimer
			// 
			volumeAnimationTimer.Interval = 220;
			volumeAnimationTimer.Tick += VolumeAnimationTimer_Tick;
			// 
			// headerLabel
			// 
			headerLabel.AutoSize = true;
			headerLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
			headerLabel.ForeColor = Color.FromArgb(18, 97, 93);
			headerLabel.Location = new Point(12, 10);
			headerLabel.Name = "headerLabel";
			headerLabel.Size = new Size(201, 37);
			headerLabel.TabIndex = 0;
			headerLabel.Text = "Create Backup";
			// 
			// settingsScroll
			// 
			settingsScroll.Location = new Point(0, 0);
			settingsScroll.Name = "settingsScroll";
			settingsScroll.Size = new Size(200, 100);
			settingsScroll.TabIndex = 0;
			settingsScroll.Resize += SettingsScroll_Resize;
			// 
			// settingsStack
			// 
			settingsStack.Location = new Point(0, 0);
			settingsStack.Name = "settingsStack";
			settingsStack.Size = new Size(200, 100);
			settingsStack.TabIndex = 0;
			// 
			// basicGroup
			// 
			basicGroup.Location = new Point(0, 0);
			basicGroup.Name = "basicGroup";
			basicGroup.Size = new Size(200, 100);
			basicGroup.TabIndex = 0;
			basicGroup.TabStop = false;
			basicGroup.Text = "Settings";
			basicGroup.Visible = true;
			// 
			// backupNameTextBox
			// 
			backupNameTextBox.Location = new Point(0, 0);
			backupNameTextBox.Name = "backupNameTextBox";
			backupNameTextBox.Size = new Size(100, 25);
			backupNameTextBox.TabIndex = 0;
			// 
			// backupTypeComboBox
			// 
			backupTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			backupTypeComboBox.Items.AddRange(new object[] { "Full Backup", "Full then Incremental", "Full then Differential", "Selected Files & Folder", "Clone to Disk", "Clone to Virtual Disk (Hyper-V)", "Clone Hyper-V System", "Export Hyper-V System" });
			backupTypeComboBox.Location = new Point(0, 0);
			backupTypeComboBox.Name = "backupTypeComboBox";
			backupTypeComboBox.Size = new Size(121, 25);
			backupTypeComboBox.TabIndex = 0;
			backupTypeComboBox.SelectedIndexChanged += BackupTypeComboBox_SelectedIndexChanged;
			// 
			// destinationTextBox
			// 
			destinationTextBox.Location = new Point(0, 0);
			destinationTextBox.Name = "destinationTextBox";
			destinationTextBox.Size = new Size(100, 25);
			destinationTextBox.TabIndex = 0;
			// 
			// compressCheckBox
			// 
			compressCheckBox.Checked = true;
			compressCheckBox.CheckState = CheckState.Checked;
			compressCheckBox.Location = new Point(0, 0);
			compressCheckBox.Name = "compressCheckBox";
			compressCheckBox.Size = new Size(104, 24);
			compressCheckBox.TabIndex = 0;
			compressCheckBox.Text = "Compress backup data";
			// 
			// verifyCheckBox
			// 
			verifyCheckBox.Checked = true;
			verifyCheckBox.CheckState = CheckState.Checked;
			verifyCheckBox.Location = new Point(0, 0);
			verifyCheckBox.Name = "verifyCheckBox";
			verifyCheckBox.Size = new Size(104, 24);
			verifyCheckBox.TabIndex = 0;
			verifyCheckBox.Text = "Verify backup after completion";
			// 
			// exclusionsGroup
			// 
			exclusionsGroup.Location = new Point(0, 0);
			exclusionsGroup.Name = "exclusionsGroup";
			exclusionsGroup.Size = new Size(200, 100);
			exclusionsGroup.TabIndex = 0;
			exclusionsGroup.TabStop = false;
			exclusionsGroup.Text = "Exclusions";
			exclusionsGroup.Visible = false;
			// 
			// manageExclusionsButton
			// 
			manageExclusionsButton.Location = new Point(0, 0);
			manageExclusionsButton.Name = "manageExclusionsButton";
			manageExclusionsButton.Size = new Size(75, 23);
			manageExclusionsButton.TabIndex = 0;
			manageExclusionsButton.Click += ManageExclusions_Click;
			// 
			// encryptionGroup
			// 
			encryptionGroup.Location = new Point(0, 0);
			encryptionGroup.Name = "encryptionGroup";
			encryptionGroup.Size = new Size(200, 100);
			encryptionGroup.TabIndex = 0;
			encryptionGroup.TabStop = false;
			encryptionGroup.Text = "Encryption";
			encryptionGroup.Visible = false;
			// 
			// encryptCheckBox
			// 
			encryptCheckBox.Location = new Point(0, 0);
			encryptCheckBox.Name = "encryptCheckBox";
			encryptCheckBox.Size = new Size(104, 24);
			encryptCheckBox.TabIndex = 0;
			encryptCheckBox.CheckedChanged += EncryptCheckBox_CheckedChanged;
			// 
			// encryptionPanel
			// 
			encryptionPanel.Location = new Point(0, 0);
			encryptionPanel.Name = "encryptionPanel";
			encryptionPanel.Size = new Size(200, 100);
			encryptionPanel.TabIndex = 0;
			// 
			// encryptionPasswordTextBox
			// 
			encryptionPasswordTextBox.Location = new Point(0, 0);
			encryptionPasswordTextBox.Name = "encryptionPasswordTextBox";
			encryptionPasswordTextBox.Size = new Size(100, 25);
			encryptionPasswordTextBox.TabIndex = 0;
			encryptionPasswordTextBox.UseSystemPasswordChar = true;
			encryptionPasswordTextBox.TextChanged += EncryptionPasswordTextBox_TextChanged;
			// 
			// verifyEncryptionPasswordTextBox
			// 
			verifyEncryptionPasswordTextBox.Location = new Point(0, 0);
			verifyEncryptionPasswordTextBox.Name = "verifyEncryptionPasswordTextBox";
			verifyEncryptionPasswordTextBox.Size = new Size(100, 25);
			verifyEncryptionPasswordTextBox.TabIndex = 0;
			verifyEncryptionPasswordTextBox.UseSystemPasswordChar = true;
			// 
			// showPasswordCheckBox
			// 
			showPasswordCheckBox.Location = new Point(0, 0);
			showPasswordCheckBox.Name = "showPasswordCheckBox";
			showPasswordCheckBox.Size = new Size(104, 24);
			showPasswordCheckBox.TabIndex = 0;
			showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;
			// 
			// scheduleGroup
			// 
			scheduleGroup.Location = new Point(0, 0);
			scheduleGroup.Name = "scheduleGroup";
			scheduleGroup.Size = new Size(200, 100);
			scheduleGroup.TabIndex = 0;
			scheduleGroup.TabStop = false;
			scheduleGroup.Text = "Schedule";
			scheduleGroup.Visible = false;
			// 
			// enableScheduleCheckBox
			// 
			enableScheduleCheckBox.Location = new Point(0, 0);
			enableScheduleCheckBox.Name = "enableScheduleCheckBox";
			enableScheduleCheckBox.Size = new Size(104, 24);
			enableScheduleCheckBox.TabIndex = 0;
			enableScheduleCheckBox.Text = "Enable schedule";
			enableScheduleCheckBox.CheckedChanged += EnableScheduleCheckBox_CheckedChanged;
			// 
			// schedulePanel
			// 
			schedulePanel.Location = new Point(0, 0);
			schedulePanel.Name = "schedulePanel";
			schedulePanel.Size = new Size(200, 100);
			schedulePanel.TabIndex = 0;
			// 
			// frequencyComboBox
			// 
			frequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			frequencyComboBox.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly", "Once" });
			frequencyComboBox.Location = new Point(0, 0);
			frequencyComboBox.Name = "frequencyComboBox";
			frequencyComboBox.Size = new Size(121, 25);
			frequencyComboBox.TabIndex = 0;
			frequencyComboBox.SelectedIndexChanged += FrequencyComboBox_SelectedIndexChanged;
			// 
			// hourComboBox
			// 
			hourComboBox.Location = new Point(0, 0);
			hourComboBox.Name = "hourComboBox";
			hourComboBox.Size = new Size(121, 25);
			hourComboBox.TabIndex = 0;
			// 
			// minuteComboBox
			// 
			minuteComboBox.Location = new Point(0, 0);
			minuteComboBox.Name = "minuteComboBox";
			minuteComboBox.Size = new Size(121, 25);
			minuteComboBox.TabIndex = 0;
			// 
			// amPmComboBox
			// 
			amPmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			amPmComboBox.Location = new Point(0, 0);
			amPmComboBox.Name = "amPmComboBox";
			amPmComboBox.Size = new Size(121, 25);
			amPmComboBox.TabIndex = 0;
			// 
			// weeklyPanel
			// 
			weeklyPanel.Location = new Point(0, 0);
			weeklyPanel.Name = "weeklyPanel";
			weeklyPanel.Size = new Size(200, 100);
			weeklyPanel.TabIndex = 0;
			// 
			// weeklyDaysCheckedListBox
			// 
			weeklyDaysCheckedListBox.CheckOnClick = true;
			weeklyDaysCheckedListBox.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
			weeklyDaysCheckedListBox.Location = new Point(0, 0);
			weeklyDaysCheckedListBox.Name = "weeklyDaysCheckedListBox";
			weeklyDaysCheckedListBox.Size = new Size(120, 96);
			weeklyDaysCheckedListBox.TabIndex = 0;
			// 
			// monthlyPanel
			// 
			monthlyPanel.Location = new Point(0, 0);
			monthlyPanel.Name = "monthlyPanel";
			monthlyPanel.Size = new Size(200, 100);
			monthlyPanel.TabIndex = 0;
			// 
			// dayOfMonthComboBox
			// 
			dayOfMonthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			dayOfMonthComboBox.Location = new Point(0, 0);
			dayOfMonthComboBox.Name = "dayOfMonthComboBox";
			dayOfMonthComboBox.Size = new Size(121, 25);
			dayOfMonthComboBox.TabIndex = 0;
			// 
			// retentionPanel
			// 
			retentionPanel.Location = new Point(0, 0);
			retentionPanel.Name = "retentionPanel";
			retentionPanel.Size = new Size(200, 100);
			retentionPanel.TabIndex = 0;
			retentionPanel.Visible = false;
			// 
			// retainCountTextBox
			// 
			retainCountTextBox.Location = new Point(0, 0);
			retainCountTextBox.Name = "retainCountTextBox";
			retainCountTextBox.Size = new Size(100, 25);
			retainCountTextBox.TabIndex = 0;
			// 
			// selectedFilesRetentionPanel
			// 
			selectedFilesRetentionPanel.Location = new Point(0, 0);
			selectedFilesRetentionPanel.Name = "selectedFilesRetentionPanel";
			selectedFilesRetentionPanel.Size = new Size(200, 100);
			selectedFilesRetentionPanel.TabIndex = 0;
			selectedFilesRetentionPanel.Visible = false;
			// 
			// selectedFilesRetentionComboBox
			// 
			selectedFilesRetentionComboBox.Location = new Point(0, 0);
			selectedFilesRetentionComboBox.Name = "selectedFilesRetentionComboBox";
			selectedFilesRetentionComboBox.Size = new Size(121, 25);
			selectedFilesRetentionComboBox.TabIndex = 0;
			// 
			// cloneRetentionPanel
			// 
			cloneRetentionPanel.Location = new Point(0, 0);
			cloneRetentionPanel.Name = "cloneRetentionPanel";
			cloneRetentionPanel.Size = new Size(200, 100);
			cloneRetentionPanel.TabIndex = 0;
			cloneRetentionPanel.Visible = false;
			// 
			// cloneRetentionComboBox
			// 
			cloneRetentionComboBox.Location = new Point(0, 0);
			cloneRetentionComboBox.Name = "cloneRetentionComboBox";
			cloneRetentionComboBox.Size = new Size(121, 25);
			cloneRetentionComboBox.TabIndex = 0;
			// 
			// nativeCoverageLabel
			// 
			nativeCoverageLabel.Location = new Point(0, 0);
			nativeCoverageLabel.Name = "nativeCoverageLabel";
			nativeCoverageLabel.Size = new Size(100, 23);
			nativeCoverageLabel.TabIndex = 0;
			nativeCoverageLabel.Text = "Native source selection preview";
			// 
			// actionInfoLabel
			// 
			actionInfoLabel.Location = new Point(0, 0);
			actionInfoLabel.Name = "actionInfoLabel";
			actionInfoLabel.Size = new Size(100, 23);
			actionInfoLabel.TabIndex = 0;
			actionInfoLabel.Text = "Advanced migration status";
			// 
			// actionHelpLabel
			// 
			actionHelpLabel.Location = new Point(0, 0);
			actionHelpLabel.Name = "actionHelpLabel";
			actionHelpLabel.Size = new Size(100, 23);
			actionHelpLabel.TabIndex = 0;
			actionHelpLabel.Text = "Designer preview of the backup editor layout";
			// 
			// advancedStateLabel
			// 
			advancedStateLabel.Location = new Point(0, 0);
			advancedStateLabel.Name = "advancedStateLabel";
			advancedStateLabel.Size = new Size(100, 23);
			advancedStateLabel.TabIndex = 0;
			advancedStateLabel.Text = "No sources selected yet.";
			// 
			// advancedSelectionListBox
			// 
			advancedSelectionListBox.HorizontalScrollbar = true;
			advancedSelectionListBox.Location = new Point(0, 0);
			advancedSelectionListBox.Name = "advancedSelectionListBox";
			advancedSelectionListBox.Size = new Size(120, 96);
			advancedSelectionListBox.TabIndex = 0;
			// 
			// openAdvancedEditorButton
			// 
			openAdvancedEditorButton.Location = new Point(0, 0);
			openAdvancedEditorButton.Name = "openAdvancedEditorButton";
			openAdvancedEditorButton.Size = new Size(75, 23);
			openAdvancedEditorButton.TabIndex = 0;
			openAdvancedEditorButton.Click += OpenAdvancedEditor_Click;
			// 
			// saveJobButton
			// 
			saveJobButton.Location = new Point(0, 0);
			saveJobButton.Name = "saveJobButton";
			saveJobButton.Size = new Size(75, 23);
			saveJobButton.TabIndex = 0;
			saveJobButton.Click += SaveJob_Click;
			// 
			// startBackupButton
			// 
			startBackupButton.Location = new Point(0, 0);
			startBackupButton.Name = "startBackupButton";
			startBackupButton.Size = new Size(75, 23);
			startBackupButton.TabIndex = 0;
			startBackupButton.Click += StartBackup_Click;
			// 
			// driveTree
			// 
			driveTree.CheckBoxes = true;
			driveTree.HideSelection = false;
			driveTree.ImageIndex = 0;
			driveTree.ImageList = driveImageList;
			driveTree.LineColor = Color.Empty;
			driveTree.Location = new Point(0, 0);
			driveTree.Name = "driveTree";
			driveTree.SelectedImageIndex = 0;
			driveTree.Size = new Size(121, 97);
			driveTree.TabIndex = 0;
			driveTree.AfterCheck += DriveTree_AfterCheck;
			driveTree.BeforeExpand += DriveTree_BeforeExpand;
			// 
			// addFolderSourceButton
			// 
			addFolderSourceButton.Location = new Point(0, 0);
			addFolderSourceButton.Name = "addFolderSourceButton";
			addFolderSourceButton.Size = new Size(75, 23);
			addFolderSourceButton.TabIndex = 0;
			addFolderSourceButton.Text = "Add Folder...";
			addFolderSourceButton.UseVisualStyleBackColor = true;
			addFolderSourceButton.Click += AddFolderSource_Click;
			// 
			// addFileSourceButton
			// 
			addFileSourceButton.Location = new Point(0, 0);
			addFileSourceButton.Name = "addFileSourceButton";
			addFileSourceButton.Size = new Size(75, 23);
			addFileSourceButton.TabIndex = 0;
			addFileSourceButton.Text = "Add File...";
			addFileSourceButton.UseVisualStyleBackColor = true;
			addFileSourceButton.Click += AddFileSource_Click;
			// 
			// removeSourceButton
			// 
			removeSourceButton.Location = new Point(0, 0);
			removeSourceButton.Name = "removeSourceButton";
			removeSourceButton.Size = new Size(75, 23);
			removeSourceButton.TabIndex = 0;
			removeSourceButton.Text = "Remove Selected";
			removeSourceButton.UseVisualStyleBackColor = true;
			removeSourceButton.Click += RemoveSelectedSource_Click;
			// 
			// refreshDriveTreeButton
			// 
			refreshDriveTreeButton.Location = new Point(0, 0);
			refreshDriveTreeButton.Name = "refreshDriveTreeButton";
			refreshDriveTreeButton.Size = new Size(75, 23);
			refreshDriveTreeButton.TabIndex = 0;
			refreshDriveTreeButton.Text = "Refresh";
			refreshDriveTreeButton.UseVisualStyleBackColor = true;
			refreshDriveTreeButton.Click += RefreshDriveTreeButton_Click;
			// 
			// expandTreeButton
			// 
			expandTreeButton.Location = new Point(0, 0);
			expandTreeButton.Name = "expandTreeButton";
			expandTreeButton.Size = new Size(75, 23);
			expandTreeButton.TabIndex = 0;
			expandTreeButton.Text = "Expand All";
			expandTreeButton.UseVisualStyleBackColor = true;
			expandTreeButton.Click += ExpandTreeButton_Click;
			// 
			// collapseTreeButton
			// 
			collapseTreeButton.Location = new Point(0, 0);
			collapseTreeButton.Name = "collapseTreeButton";
			collapseTreeButton.Size = new Size(75, 23);
			collapseTreeButton.TabIndex = 0;
			collapseTreeButton.Text = "Collapse All";
			collapseTreeButton.UseVisualStyleBackColor = true;
			collapseTreeButton.Click += CollapseTreeButton_Click;
			// 
			// showHiddenPartitionsCheckBox
			// 
			showHiddenPartitionsCheckBox.Location = new Point(0, 0);
			showHiddenPartitionsCheckBox.Name = "showHiddenPartitionsCheckBox";
			showHiddenPartitionsCheckBox.Size = new Size(104, 24);
			showHiddenPartitionsCheckBox.TabIndex = 0;
			showHiddenPartitionsCheckBox.Text = "Show hidden partitions";
			showHiddenPartitionsCheckBox.CheckedChanged += ShowHiddenPartitionsCheckBox_CheckedChanged;
			// 
			// nativeSourceListBox
			// 
			nativeSourceListBox.HorizontalScrollbar = true;
			nativeSourceListBox.Location = new Point(0, 0);
			nativeSourceListBox.Name = "nativeSourceListBox";
			nativeSourceListBox.Size = new Size(120, 96);
			nativeSourceListBox.TabIndex = 0;
			nativeSourceListBox.SelectedIndexChanged += NativeSourceListBox_SelectedIndexChanged;
			// 
			// sourceSelectionGroup
			// 
			sourceSelectionGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			sourceSelectionGroup.Location = new Point(0, 0);
			sourceSelectionGroup.Name = "sourceSelectionGroup";
			sourceSelectionGroup.Padding = new Padding(8);
			sourceSelectionGroup.Size = new Size(554, 512);
			sourceSelectionGroup.TabIndex = 100;
			sourceSelectionGroup.TabStop = false;
			sourceSelectionGroup.Text = "Source Selection";
			sourceSelectionGroup.Controls.Add(nativeSourceListBox);
			sourceSelectionGroup.Controls.Add(showHiddenPartitionsCheckBox);
			sourceSelectionGroup.Controls.Add(collapseTreeButton);
			sourceSelectionGroup.Controls.Add(expandTreeButton);
			sourceSelectionGroup.Controls.Add(refreshDriveTreeButton);
			sourceSelectionGroup.Controls.Add(removeSourceButton);
			sourceSelectionGroup.Controls.Add(addFileSourceButton);
			sourceSelectionGroup.Controls.Add(addFolderSourceButton);
			sourceSelectionGroup.Controls.Add(driveTree);
			// 
			// advancedStateGroup
			// 
			advancedStateGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			advancedStateGroup.Location = new Point(0, 518);
			advancedStateGroup.Name = "advancedStateGroup";
			advancedStateGroup.Padding = new Padding(8);
			advancedStateGroup.Size = new Size(554, 274);
			advancedStateGroup.TabIndex = 101;
			advancedStateGroup.TabStop = false;
			advancedStateGroup.Text = "Advanced State";
			advancedStateGroup.Controls.Add(openAdvancedEditorButton);
			advancedStateGroup.Controls.Add(advancedSelectionListBox);
			advancedStateGroup.Controls.Add(advancedStateLabel);
			advancedStateGroup.Controls.Add(actionHelpLabel);
			advancedStateGroup.Controls.Add(actionInfoLabel);
			advancedStateGroup.Controls.Add(nativeCoverageLabel);
			// 
			// leftColumnLayout
			// 
			leftColumnLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			leftColumnLayout.ColumnCount = 1;
			leftColumnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			leftColumnLayout.Controls.Add(sourceSelectionGroup, 0, 0);
			leftColumnLayout.Controls.Add(advancedStateGroup, 0, 1);
			leftColumnLayout.Location = new Point(12, 59);
			leftColumnLayout.Name = "leftColumnLayout";
			leftColumnLayout.RowCount = 2;
			leftColumnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
			leftColumnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
			leftColumnLayout.Size = new Size(560, 798);
			leftColumnLayout.TabIndex = 102;
			// 
			// settingsStack
			// 
			settingsStack.Controls.Add(basicGroup, 0, 0);
			settingsStack.Controls.Add(retentionPanel, 0, 1);
			settingsStack.Controls.Add(selectedFilesRetentionPanel, 0, 2);
			settingsStack.Controls.Add(cloneRetentionPanel, 0, 3);
			settingsStack.Controls.Add(exclusionsGroup, 0, 4);
			settingsStack.Controls.Add(encryptionGroup, 0, 5);
			settingsStack.Controls.Add(scheduleGroup, 0, 6);
			settingsStack.RowCount = 7;
			// 
			// settingsScroll
			// 
			settingsScroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			settingsScroll.AutoScroll = true;
			settingsScroll.Controls.Add(settingsStack);
			settingsScroll.Location = new Point(590, 59);
			settingsScroll.Size = new Size(518, 798);
			// 
			// driveTree
			// 
			driveTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			driveTree.Location = new Point(12, 68);
			driveTree.Size = new Size(530, 436);
			// 
			// showHiddenPartitionsCheckBox
			// 
			showHiddenPartitionsCheckBox.AutoSize = true;
			showHiddenPartitionsCheckBox.Location = new Point(12, 38);
			showHiddenPartitionsCheckBox.Size = new Size(155, 21);
			// 
			// refreshDriveTreeButton
			// 
			refreshDriveTreeButton.Location = new Point(12, 510);
			refreshDriveTreeButton.Size = new Size(86, 27);
			// 
			// expandTreeButton
			// 
			expandTreeButton.Location = new Point(104, 510);
			expandTreeButton.Size = new Size(91, 27);
			// 
			// collapseTreeButton
			// 
			collapseTreeButton.Location = new Point(201, 510);
			collapseTreeButton.Size = new Size(95, 27);
			// 
			// nativeSourceListBox
			// 
			nativeSourceListBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			nativeSourceListBox.Location = new Point(12, 552);
			nativeSourceListBox.Size = new Size(530, 112);
			// 
			// addFolderSourceButton
			// 
			addFolderSourceButton.Location = new Point(12, 674);
			addFolderSourceButton.Size = new Size(96, 27);
			// 
			// addFileSourceButton
			// 
			addFileSourceButton.Location = new Point(114, 674);
			addFileSourceButton.Size = new Size(84, 27);
			// 
			// removeSourceButton
			// 
			removeSourceButton.Location = new Point(204, 674);
			removeSourceButton.Size = new Size(126, 27);
			// 
			// nativeCoverageLabel
			// 
			nativeCoverageLabel.AutoSize = true;
			nativeCoverageLabel.Location = new Point(12, 28);
			nativeCoverageLabel.Size = new Size(204, 17);
			// 
			// actionInfoLabel
			// 
			actionInfoLabel.AutoSize = true;
			actionInfoLabel.Location = new Point(12, 54);
			actionInfoLabel.Size = new Size(154, 17);
			// 
			// actionHelpLabel
			// 
			actionHelpLabel.AutoSize = true;
			actionHelpLabel.Location = new Point(12, 80);
			actionHelpLabel.Size = new Size(261, 17);
			// 
			// advancedStateLabel
			// 
			advancedStateLabel.AutoSize = true;
			advancedStateLabel.Location = new Point(12, 106);
			advancedStateLabel.Size = new Size(138, 17);
			// 
			// advancedSelectionListBox
			// 
			advancedSelectionListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			advancedSelectionListBox.Location = new Point(12, 134);
			advancedSelectionListBox.Size = new Size(530, 100);
			// 
			// openAdvancedEditorButton
			// 
			openAdvancedEditorButton.Location = new Point(12, 238);
			openAdvancedEditorButton.Size = new Size(150, 27);
			openAdvancedEditorButton.Text = "Open Advanced Editor";
			openAdvancedEditorButton.UseVisualStyleBackColor = true;
			// 
			// settingsStack
			// 
			settingsStack.AutoSize = true;
			settingsStack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			settingsStack.ColumnCount = 1;
			settingsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			settingsStack.Dock = DockStyle.Top;
			settingsStack.Location = new Point(0, 0);
			settingsStack.Padding = Padding.Empty;
			settingsStack.Size = new Size(490, 740);
			// 
			// exclusionsGroup
			// 
			exclusionsGroup.Location = new Point(3, 315);
			exclusionsGroup.Name = "exclusionsGroup";
			exclusionsGroup.Size = new Size(484, 64);
			exclusionsGroup.TabIndex = 1;
			exclusionsGroup.TabStop = false;
			exclusionsGroup.Text = "Exclusions";
			exclusionsGroup.Visible = true;
			exclusionsGroup.Controls.Add(manageExclusionsButton);
			manageExclusionsButton.Location = new Point(16, 24);
			manageExclusionsButton.Size = new Size(154, 27);
			// 
			// encryptionGroup
			// 
			encryptionGroup.Location = new Point(3, 385);
			encryptionGroup.Name = "encryptionGroup";
			encryptionGroup.Size = new Size(484, 132);
			encryptionGroup.TabIndex = 2;
			encryptionGroup.TabStop = false;
			encryptionGroup.Text = "Encryption";
			encryptionGroup.Visible = true;
			encryptionGroup.Controls.Add(showPasswordCheckBox);
			encryptionGroup.Controls.Add(verifyEncryptionPasswordTextBox);
			encryptionGroup.Controls.Add(encryptionPasswordTextBox);
			encryptionGroup.Controls.Add(encryptCheckBox);
			encryptCheckBox.AutoSize = true;
			encryptCheckBox.Location = new Point(16, 28);
			encryptCheckBox.Text = "Encrypt backup";
			encryptionPasswordTextBox.Location = new Point(16, 58);
			encryptionPasswordTextBox.Size = new Size(200, 25);
			verifyEncryptionPasswordTextBox.Location = new Point(16, 89);
			verifyEncryptionPasswordTextBox.Size = new Size(200, 25);
			showPasswordCheckBox.AutoSize = true;
			showPasswordCheckBox.Location = new Point(232, 60);
			showPasswordCheckBox.Text = "Show password";
			// 
			// scheduleGroup
			// 
			scheduleGroup.Location = new Point(3, 523);
			scheduleGroup.Name = "scheduleGroup";
			scheduleGroup.Size = new Size(484, 150);
			scheduleGroup.TabIndex = 3;
			scheduleGroup.TabStop = false;
			scheduleGroup.Text = "Schedule";
			scheduleGroup.Visible = true;
			scheduleGroup.Controls.Add(enableScheduleCheckBox);
			enableScheduleCheckBox.AutoSize = true;
			enableScheduleCheckBox.Location = new Point(16, 28);
			// 
			// retentionPanel
			// 
			retentionPanel.Location = new Point(3, 109);
			retentionPanel.Size = new Size(484, 62);
			retentionPanel.Visible = true;
			// 
			// selectedFilesRetentionPanel
			// 
			selectedFilesRetentionPanel.Location = new Point(3, 177);
			selectedFilesRetentionPanel.Size = new Size(484, 62);
			selectedFilesRetentionPanel.Visible = true;
			// 
			// cloneRetentionPanel
			// 
			cloneRetentionPanel.Location = new Point(3, 245);
			cloneRetentionPanel.Size = new Size(484, 62);
			cloneRetentionPanel.Visible = true;
			// 
			// basicGroup
			// 
			basicGroup.Location = new Point(3, 3);
			basicGroup.Size = new Size(484, 100);
			// 
			// BackupNewForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(1120, 956);
			Controls.Add(settingsScroll);
			Controls.Add(leftColumnLayout);
			Controls.Add(startBackupButton);
			Controls.Add(saveJobButton);
			Controls.Add(headerLabel);
			MinimumSize = new Size(1040, 901);
			Name = "BackupNewForm";
			saveJobButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			saveJobButton.Location = new Point(842, 872);
			saveJobButton.Size = new Size(122, 32);
			saveJobButton.Text = "Save Backup";
			saveJobButton.UseVisualStyleBackColor = true;
			startBackupButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			startBackupButton.Location = new Point(970, 872);
			startBackupButton.Size = new Size(138, 32);
			startBackupButton.Text = "Start Backup";
			startBackupButton.UseVisualStyleBackColor = true;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Backup";
			FormClosed += BackupNewForm_FormClosed;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
