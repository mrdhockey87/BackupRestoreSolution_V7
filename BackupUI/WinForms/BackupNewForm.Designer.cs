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
			cloneRetentionPanel = new Panel();
			selectedFilesRetentionPanel = new Panel();
			retentionPanel = new Panel();
			verifyCheckBox = new CheckBox();
			compressCheckBox = new CheckBox();
			destinationTextBox = new TextBox();
			backupTypeComboBox = new ComboBox();
			backupNameTextBox = new TextBox();
			scheduleGroup = new GroupBox();
			enableScheduleCheckBox = new CheckBox();
			manageExclusionsButton = new Button();
			showPasswordCheckBox = new CheckBox();
			verifyEncryptionPasswordTextBox = new TextBox();
			encryptionPasswordTextBox = new TextBox();
			encryptCheckBox = new CheckBox();
			exclusionsGroup = new GroupBox();
			encryptionGroup = new GroupBox();
			encryptionPanel = new Panel();
			schedulePanel = new Panel();
			frequencyComboBox = new ComboBox();
			hourComboBox = new ComboBox();
			minuteComboBox = new ComboBox();
			amPmComboBox = new ComboBox();
			weeklyPanel = new Panel();
			weeklyDaysCheckedListBox = new CheckedListBox();
			monthlyPanel = new Panel();
			dayOfMonthComboBox = new ComboBox();
			retainCountTextBox = new TextBox();
			selectedFilesRetentionComboBox = new ComboBox();
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
			rootLayout = new TableLayoutPanel();
			sourceSelectionGroup = new GroupBox();
			sourceDescriptionLabel = new Label();
			sourceButtonPanel = new FlowLayoutPanel();
			leftColumnLayout = new TableLayoutPanel();
			schedulePreviewPanel = new Panel();
			scheduleFrequencyLabel = new Label();
			scheduleTimeLabel = new Label();
			cancelButton = new Button();
			actionButtonPanel = new FlowLayoutPanel();
			settingsScroll.SuspendLayout();
			settingsStack.SuspendLayout();
			basicGroup.SuspendLayout();
			scheduleGroup.SuspendLayout();
			exclusionsGroup.SuspendLayout();
			encryptionGroup.SuspendLayout();
			sourceSelectionGroup.SuspendLayout();
			sourceButtonPanel.SuspendLayout();
			leftColumnLayout.SuspendLayout();
			schedulePreviewPanel.SuspendLayout();
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
			settingsScroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			settingsScroll.AutoScroll = true;
			settingsScroll.Controls.Add(settingsStack);
			settingsScroll.Location = new Point(590, 59);
			settingsScroll.Name = "settingsScroll";
			settingsScroll.Size = new Size(518, 801);
			settingsScroll.TabIndex = 0;
			settingsScroll.Resize += SettingsScroll_Resize;
			// 
			// settingsStack
			// 
			settingsStack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			settingsStack.ColumnCount = 1;
			settingsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			settingsStack.Controls.Add(basicGroup, 0, 0);
			settingsStack.Controls.Add(scheduleGroup, 0, 1);
			settingsStack.Dock = DockStyle.Top;
			settingsStack.Location = new Point(0, 0);
			settingsStack.Name = "settingsStack";
			settingsStack.RowCount = 2;
			settingsStack.RowStyles.Add(new RowStyle());
			settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			settingsStack.Size = new Size(518, 740);
			settingsStack.TabIndex = 0;
			// 
			// basicGroup
			// 
			basicGroup.Controls.Add(cloneRetentionPanel);
			basicGroup.Controls.Add(selectedFilesRetentionPanel);
			basicGroup.Controls.Add(retentionPanel);
			basicGroup.Controls.Add(verifyCheckBox);
			basicGroup.Controls.Add(compressCheckBox);
			basicGroup.Controls.Add(destinationTextBox);
			basicGroup.Controls.Add(backupTypeComboBox);
			basicGroup.Controls.Add(backupNameTextBox);
			basicGroup.Location = new Point(3, 3);
			basicGroup.Name = "basicGroup";
			basicGroup.Size = new Size(512, 500);
			basicGroup.TabIndex = 0;
			basicGroup.TabStop = false;
			basicGroup.Text = "Settings";
			// 
			// cloneRetentionPanel
			// 
			cloneRetentionPanel.Location = new Point(3, 245);
			cloneRetentionPanel.Name = "cloneRetentionPanel";
			cloneRetentionPanel.Size = new Size(484, 62);
			cloneRetentionPanel.TabIndex = 0;
			// 
			// selectedFilesRetentionPanel
			// 
			selectedFilesRetentionPanel.Location = new Point(3, 177);
			selectedFilesRetentionPanel.Name = "selectedFilesRetentionPanel";
			selectedFilesRetentionPanel.Size = new Size(484, 62);
			selectedFilesRetentionPanel.TabIndex = 0;
			// 
			// retentionPanel
			// 
			retentionPanel.Location = new Point(3, 109);
			retentionPanel.Name = "retentionPanel";
			retentionPanel.Size = new Size(484, 62);
			retentionPanel.TabIndex = 0;
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
			// destinationTextBox
			// 
			destinationTextBox.Location = new Point(0, 0);
			destinationTextBox.Name = "destinationTextBox";
			destinationTextBox.Size = new Size(100, 25);
			destinationTextBox.TabIndex = 0;
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
			// backupNameTextBox
			// 
			backupNameTextBox.Location = new Point(0, 0);
			backupNameTextBox.Name = "backupNameTextBox";
			backupNameTextBox.Size = new Size(100, 25);
			backupNameTextBox.TabIndex = 0;
			// 
			// scheduleGroup
			// 
			scheduleGroup.Controls.Add(enableScheduleCheckBox);
			scheduleGroup.Location = new Point(3, 509);
			scheduleGroup.Name = "scheduleGroup";
			scheduleGroup.Size = new Size(512, 228);
			scheduleGroup.TabIndex = 3;
			scheduleGroup.TabStop = false;
			scheduleGroup.Text = "Schedule";
			// 
			// enableScheduleCheckBox
			// 
			enableScheduleCheckBox.Location = new Point(16, 24);
			enableScheduleCheckBox.Name = "enableScheduleCheckBox";
			enableScheduleCheckBox.Size = new Size(121, 25);
			enableScheduleCheckBox.TabIndex = 0;
			enableScheduleCheckBox.Text = "Enable schedule";
			enableScheduleCheckBox.CheckedChanged += EnableScheduleCheckBox_CheckedChanged;
			// 
			// manageExclusionsButton
			// 
			manageExclusionsButton.Location = new Point(16, 24);
			manageExclusionsButton.Name = "manageExclusionsButton";
			manageExclusionsButton.Size = new Size(154, 27);
			manageExclusionsButton.TabIndex = 0;
			manageExclusionsButton.Click += ManageExclusions_Click;
			// 
			// showPasswordCheckBox
			// 
			showPasswordCheckBox.AutoSize = true;
			showPasswordCheckBox.Location = new Point(232, 60);
			showPasswordCheckBox.Name = "showPasswordCheckBox";
			showPasswordCheckBox.Size = new Size(119, 21);
			showPasswordCheckBox.TabIndex = 0;
			showPasswordCheckBox.Text = "Show password";
			showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;
			// 
			// verifyEncryptionPasswordTextBox
			// 
			verifyEncryptionPasswordTextBox.Location = new Point(16, 89);
			verifyEncryptionPasswordTextBox.Name = "verifyEncryptionPasswordTextBox";
			verifyEncryptionPasswordTextBox.Size = new Size(200, 25);
			verifyEncryptionPasswordTextBox.TabIndex = 0;
			verifyEncryptionPasswordTextBox.UseSystemPasswordChar = true;
			// 
			// encryptionPasswordTextBox
			// 
			encryptionPasswordTextBox.Location = new Point(16, 58);
			encryptionPasswordTextBox.Name = "encryptionPasswordTextBox";
			encryptionPasswordTextBox.Size = new Size(200, 25);
			encryptionPasswordTextBox.TabIndex = 0;
			encryptionPasswordTextBox.UseSystemPasswordChar = true;
			encryptionPasswordTextBox.TextChanged += EncryptionPasswordTextBox_TextChanged;
			// 
			// encryptCheckBox
			// 
			encryptCheckBox.AutoSize = true;
			encryptCheckBox.Location = new Point(16, 28);
			encryptCheckBox.Name = "encryptCheckBox";
			encryptCheckBox.Size = new Size(116, 21);
			encryptCheckBox.TabIndex = 0;
			encryptCheckBox.Text = "Encrypt backup";
			encryptCheckBox.CheckedChanged += EncryptCheckBox_CheckedChanged;
			// 
			// exclusionsGroup
			// 
			exclusionsGroup.Controls.Add(manageExclusionsButton);
			exclusionsGroup.Location = new Point(3, 315);
			exclusionsGroup.Name = "exclusionsGroup";
			exclusionsGroup.Size = new Size(484, 64);
			exclusionsGroup.TabIndex = 1;
			exclusionsGroup.TabStop = false;
			exclusionsGroup.Text = "Exclusions";
			// 
			// encryptionGroup
			// 
			encryptionGroup.Controls.Add(showPasswordCheckBox);
			encryptionGroup.Controls.Add(verifyEncryptionPasswordTextBox);
			encryptionGroup.Controls.Add(encryptionPasswordTextBox);
			encryptionGroup.Controls.Add(encryptCheckBox);
			encryptionGroup.Location = new Point(3, 385);
			encryptionGroup.Name = "encryptionGroup";
			encryptionGroup.Size = new Size(484, 132);
			encryptionGroup.TabIndex = 2;
			encryptionGroup.TabStop = false;
			encryptionGroup.Text = "Encryption";
			// 
			// encryptionPanel
			// 
			encryptionPanel.Location = new Point(0, 0);
			encryptionPanel.Name = "encryptionPanel";
			encryptionPanel.Size = new Size(200, 100);
			encryptionPanel.TabIndex = 0;
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
			// retainCountTextBox
			// 
			retainCountTextBox.Location = new Point(0, 0);
			retainCountTextBox.Name = "retainCountTextBox";
			retainCountTextBox.Size = new Size(100, 25);
			retainCountTextBox.TabIndex = 0;
			// 
			// selectedFilesRetentionComboBox
			// 
			selectedFilesRetentionComboBox.Location = new Point(0, 0);
			selectedFilesRetentionComboBox.Name = "selectedFilesRetentionComboBox";
			selectedFilesRetentionComboBox.Size = new Size(121, 25);
			selectedFilesRetentionComboBox.TabIndex = 0;
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
			saveJobButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			saveJobButton.Location = new Point(842, 875);
			saveJobButton.Name = "saveJobButton";
			saveJobButton.Size = new Size(122, 32);
			saveJobButton.TabIndex = 0;
			saveJobButton.Text = "Save Backup";
			saveJobButton.UseVisualStyleBackColor = true;
			saveJobButton.Click += SaveJob_Click;
			// 
			// startBackupButton
			// 
			startBackupButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			startBackupButton.Location = new Point(970, 875);
			startBackupButton.Name = "startBackupButton";
			startBackupButton.Size = new Size(138, 32);
			startBackupButton.TabIndex = 0;
			startBackupButton.Text = "Start Backup";
			startBackupButton.UseVisualStyleBackColor = true;
			startBackupButton.Click += StartBackup_Click;
			// 
			// driveTree
			// 
			driveTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			driveTree.CheckBoxes = true;
			driveTree.HideSelection = false;
			driveTree.ImageIndex = 0;
			driveTree.ImageList = driveImageList;
			driveTree.Location = new Point(12, 82);
			driveTree.Name = "driveTree";
			driveTree.SelectedImageIndex = 0;
			driveTree.Size = new Size(530, 609);
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
			refreshDriveTreeButton.Location = new Point(3, 3);
			refreshDriveTreeButton.Name = "refreshDriveTreeButton";
			refreshDriveTreeButton.Size = new Size(86, 27);
			refreshDriveTreeButton.TabIndex = 0;
			refreshDriveTreeButton.Text = "Refresh";
			refreshDriveTreeButton.UseVisualStyleBackColor = true;
			refreshDriveTreeButton.Click += RefreshDriveTreeButton_Click;
			// 
			// expandTreeButton
			// 
			expandTreeButton.Location = new Point(95, 3);
			expandTreeButton.Name = "expandTreeButton";
			expandTreeButton.Size = new Size(91, 27);
			expandTreeButton.TabIndex = 0;
			expandTreeButton.Text = "Expand All";
			expandTreeButton.UseVisualStyleBackColor = true;
			expandTreeButton.Click += ExpandTreeButton_Click;
			// 
			// collapseTreeButton
			// 
			collapseTreeButton.Location = new Point(192, 3);
			collapseTreeButton.Name = "collapseTreeButton";
			collapseTreeButton.Size = new Size(95, 27);
			collapseTreeButton.TabIndex = 0;
			collapseTreeButton.Text = "Collapse All";
			collapseTreeButton.UseVisualStyleBackColor = true;
			collapseTreeButton.Click += CollapseTreeButton_Click;
			// 
			// showHiddenPartitionsCheckBox
			// 
			showHiddenPartitionsCheckBox.AutoSize = true;
			showHiddenPartitionsCheckBox.Location = new Point(293, 3);
			showHiddenPartitionsCheckBox.Name = "showHiddenPartitionsCheckBox";
			showHiddenPartitionsCheckBox.Size = new Size(161, 21);
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
			// rootLayout
			// 
			rootLayout.Location = new Point(0, 0);
			rootLayout.Name = "rootLayout";
			rootLayout.Size = new Size(200, 100);
			rootLayout.TabIndex = 0;
			// 
			// sourceSelectionGroup
			// 
			sourceSelectionGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			sourceSelectionGroup.Controls.Add(sourceDescriptionLabel);
			sourceSelectionGroup.Controls.Add(sourceButtonPanel);
			sourceSelectionGroup.Controls.Add(driveTree);
			sourceSelectionGroup.Location = new Point(3, 3);
			sourceSelectionGroup.Name = "sourceSelectionGroup";
			sourceSelectionGroup.Padding = new Padding(8);
			sourceSelectionGroup.Size = new Size(554, 795);
			sourceSelectionGroup.TabIndex = 102;
			sourceSelectionGroup.TabStop = false;
			sourceSelectionGroup.Text = "What to Backup";
			// 
			// sourceDescriptionLabel
			// 
			sourceDescriptionLabel.AutoSize = true;
			sourceDescriptionLabel.Location = new Point(12, 28);
			sourceDescriptionLabel.MaximumSize = new Size(520, 0);
			sourceDescriptionLabel.Name = "sourceDescriptionLabel";
			sourceDescriptionLabel.Size = new Size(492, 34);
			sourceDescriptionLabel.TabIndex = 100;
			sourceDescriptionLabel.Text = "Check drives or volumes to backup. Files and folders are shown when volumes are unchecked. Boot volumes will automatically include system state backup.";
			// 
			// sourceButtonPanel
			// 
			sourceButtonPanel.AutoSize = true;
			sourceButtonPanel.Controls.Add(refreshDriveTreeButton);
			sourceButtonPanel.Controls.Add(expandTreeButton);
			sourceButtonPanel.Controls.Add(collapseTreeButton);
			sourceButtonPanel.Controls.Add(showHiddenPartitionsCheckBox);
			sourceButtonPanel.Location = new Point(12, 700);
			sourceButtonPanel.Name = "sourceButtonPanel";
			sourceButtonPanel.Size = new Size(457, 33);
			sourceButtonPanel.TabIndex = 101;
			// 
			// leftColumnLayout
			// 
			leftColumnLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			leftColumnLayout.ColumnCount = 1;
			leftColumnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			leftColumnLayout.Controls.Add(sourceSelectionGroup, 0, 0);
			leftColumnLayout.Location = new Point(12, 59);
			leftColumnLayout.Name = "leftColumnLayout";
			leftColumnLayout.RowCount = 1;
			leftColumnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			leftColumnLayout.Size = new Size(560, 801);
			leftColumnLayout.TabIndex = 103;
			// 
			// schedulePreviewPanel
			// 
			schedulePreviewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			schedulePreviewPanel.BackColor = Color.FromArgb(77, 201, 200);
			schedulePreviewPanel.Controls.Add(scheduleFrequencyLabel);
			schedulePreviewPanel.Controls.Add(scheduleTimeLabel);
			schedulePreviewPanel.Location = new Point(12, 58);
			schedulePreviewPanel.Name = "schedulePreviewPanel";
			schedulePreviewPanel.Size = new Size(456, 78);
			schedulePreviewPanel.TabIndex = 20;
			// 
			// scheduleFrequencyLabel
			// 
			scheduleFrequencyLabel.Location = new Point(12, 12);
			scheduleFrequencyLabel.Name = "scheduleFrequencyLabel";
			scheduleFrequencyLabel.Size = new Size(102, 17);
			scheduleFrequencyLabel.TabIndex = 0;
			scheduleFrequencyLabel.Text = "Frequency: Daily";
			// 
			// scheduleTimeLabel
			// 
			scheduleTimeLabel.Location = new Point(12, 42);
			scheduleTimeLabel.Name = "scheduleTimeLabel";
			scheduleTimeLabel.Size = new Size(91, 17);
			scheduleTimeLabel.TabIndex = 1;
			scheduleTimeLabel.Text = "Time: 2:00 AM";
			// 
			// cancelButton
			// 
			cancelButton.Location = new Point(0, 0);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(75, 23);
			cancelButton.TabIndex = 0;
			// 
			// actionButtonPanel
			// 
			actionButtonPanel.Location = new Point(0, 0);
			actionButtonPanel.Name = "actionButtonPanel";
			actionButtonPanel.Size = new Size(200, 100);
			actionButtonPanel.TabIndex = 0;
			// 
			// BackupNewForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(1120, 959);
			Controls.Add(settingsScroll);
			Controls.Add(leftColumnLayout);
			Controls.Add(startBackupButton);
			Controls.Add(saveJobButton);
			Controls.Add(headerLabel);
			MinimumSize = new Size(1040, 901);
			Name = "BackupNewForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Backup";
			FormClosed += BackupNewForm_FormClosed;
			settingsScroll.ResumeLayout(false);
			settingsStack.ResumeLayout(false);
			basicGroup.ResumeLayout(false);
			basicGroup.PerformLayout();
			scheduleGroup.ResumeLayout(false);
			exclusionsGroup.ResumeLayout(false);
			encryptionGroup.ResumeLayout(false);
			encryptionGroup.PerformLayout();
			sourceSelectionGroup.ResumeLayout(false);
			sourceSelectionGroup.PerformLayout();
			sourceButtonPanel.ResumeLayout(false);
			sourceButtonPanel.PerformLayout();
			leftColumnLayout.ResumeLayout(false);
			schedulePreviewPanel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		private TableLayoutPanel rootLayout;
		private GroupBox sourceSelectionGroup;
		private Label sourceDescriptionLabel;
		private FlowLayoutPanel sourceButtonPanel;
		private TableLayoutPanel leftColumnLayout;
		private Panel schedulePreviewPanel;
		private Label scheduleFrequencyLabel;
		private Label scheduleTimeLabel;
		private Button cancelButton;
		private FlowLayoutPanel actionButtonPanel;
	}
}
