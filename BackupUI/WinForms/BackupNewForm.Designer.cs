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
			basicGroup.Visible = false;
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
			// 
			// actionInfoLabel
			// 
			actionInfoLabel.Location = new Point(0, 0);
			actionInfoLabel.Name = "actionInfoLabel";
			actionInfoLabel.Size = new Size(100, 23);
			actionInfoLabel.TabIndex = 0;
			// 
			// actionHelpLabel
			// 
			actionHelpLabel.Location = new Point(0, 0);
			actionHelpLabel.Name = "actionHelpLabel";
			actionHelpLabel.Size = new Size(100, 23);
			actionHelpLabel.TabIndex = 0;
			// 
			// advancedStateLabel
			// 
			advancedStateLabel.Location = new Point(0, 0);
			advancedStateLabel.Name = "advancedStateLabel";
			advancedStateLabel.Size = new Size(100, 23);
			advancedStateLabel.TabIndex = 0;
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
			refreshDriveTreeButton.Click += RefreshDriveTreeButton_Click;
			// 
			// expandTreeButton
			// 
			expandTreeButton.Location = new Point(0, 0);
			expandTreeButton.Name = "expandTreeButton";
			expandTreeButton.Size = new Size(75, 23);
			expandTreeButton.TabIndex = 0;
			expandTreeButton.Click += ExpandTreeButton_Click;
			// 
			// collapseTreeButton
			// 
			collapseTreeButton.Location = new Point(0, 0);
			collapseTreeButton.Name = "collapseTreeButton";
			collapseTreeButton.Size = new Size(75, 23);
			collapseTreeButton.TabIndex = 0;
			collapseTreeButton.Click += CollapseTreeButton_Click;
			// 
			// showHiddenPartitionsCheckBox
			// 
			showHiddenPartitionsCheckBox.Location = new Point(0, 0);
			showHiddenPartitionsCheckBox.Name = "showHiddenPartitionsCheckBox";
			showHiddenPartitionsCheckBox.Size = new Size(104, 24);
			showHiddenPartitionsCheckBox.TabIndex = 0;
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
			// BackupNewForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(1120, 956);
			Controls.Add(headerLabel);
			MinimumSize = new Size(1040, 901);
			Name = "BackupNewForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Backup";
			FormClosed += BackupNewForm_FormClosed;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
