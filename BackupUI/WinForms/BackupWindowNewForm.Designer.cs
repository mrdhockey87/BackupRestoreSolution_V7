using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class BackupWindowNewForm
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
			// headerLabel
			headerLabel.AutoSize = true;
			headerLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
			headerLabel.ForeColor = Color.FromArgb(18, 97, 93);
			headerLabel.Location = new Point(12, 9);
			headerLabel.Name = "headerLabel";
			headerLabel.Size = new Size(161, 32);
			headerLabel.TabIndex = 0;
			headerLabel.Text = "Create Backup";
			// basicGroup
			basicGroup.Text = "Settings";
			basicGroup.Visible = false;
			// exclusionsGroup
			exclusionsGroup.Text = "Exclusions";
			exclusionsGroup.Visible = false;
			// encryptionGroup
			encryptionGroup.Text = "Encryption";
			encryptionGroup.Visible = false;
			// scheduleGroup
			scheduleGroup.Text = "Schedule";
			scheduleGroup.Visible = false;
			// retentionPanel
			retentionPanel.Visible = false;
			// selectedFilesRetentionPanel
			selectedFilesRetentionPanel.Visible = false;
			// cloneRetentionPanel
			cloneRetentionPanel.Visible = false;
			// backupNameTextBox
			backupNameTextBox.Name = "backupNameTextBox";
			// backupTypeComboBox
			backupTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			backupTypeComboBox.Items.AddRange(new object[] { "Full Backup", "Full then Incremental", "Full then Differential", "Selected Files & Folder", "Clone to Disk", "Clone to Virtual Disk (Hyper-V)", "Clone Hyper-V System", "Export Hyper-V System" });
			backupTypeComboBox.Name = "backupTypeComboBox";
			backupTypeComboBox.SelectedIndexChanged += BackupTypeComboBox_SelectedIndexChanged;
			// destinationTextBox
			destinationTextBox.Name = "destinationTextBox";
			// compressCheckBox
			compressCheckBox.Checked = true;
			compressCheckBox.Name = "compressCheckBox";
			compressCheckBox.Text = "Compress backup data";
			// verifyCheckBox
			verifyCheckBox.Checked = true;
			verifyCheckBox.Name = "verifyCheckBox";
			verifyCheckBox.Text = "Verify backup after completion";
			// manageExclusionsButton
			manageExclusionsButton.Name = "manageExclusionsButton";
			manageExclusionsButton.Click += ManageExclusions_Click;
			// encryptCheckBox
			encryptCheckBox.Name = "encryptCheckBox";
			encryptCheckBox.CheckedChanged += EncryptCheckBox_CheckedChanged;
			// encryptionPasswordTextBox
			encryptionPasswordTextBox.Name = "encryptionPasswordTextBox";
			encryptionPasswordTextBox.UseSystemPasswordChar = true;
			encryptionPasswordTextBox.TextChanged += EncryptionPasswordTextBox_TextChanged;
			// verifyEncryptionPasswordTextBox
			verifyEncryptionPasswordTextBox.Name = "verifyEncryptionPasswordTextBox";
			verifyEncryptionPasswordTextBox.UseSystemPasswordChar = true;
			// showPasswordCheckBox
			showPasswordCheckBox.Name = "showPasswordCheckBox";
			showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;
			// enableScheduleCheckBox
			enableScheduleCheckBox.Name = "enableScheduleCheckBox";
			enableScheduleCheckBox.CheckedChanged += EnableScheduleCheckBox_CheckedChanged;
			// frequencyComboBox
			frequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			frequencyComboBox.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly", "Once" });
			frequencyComboBox.Name = "frequencyComboBox";
			frequencyComboBox.SelectedIndexChanged += FrequencyComboBox_SelectedIndexChanged;
			// amPmComboBox
			amPmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			amPmComboBox.Name = "amPmComboBox";
			// weeklyDaysCheckedListBox
			weeklyDaysCheckedListBox.CheckOnClick = true;
			weeklyDaysCheckedListBox.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
			weeklyDaysCheckedListBox.Name = "weeklyDaysCheckedListBox";
			// dayOfMonthComboBox
			dayOfMonthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			dayOfMonthComboBox.Name = "dayOfMonthComboBox";
			// nativeCoverageLabel
			nativeCoverageLabel.Name = "nativeCoverageLabel";
			// actionInfoLabel
			actionInfoLabel.Name = "actionInfoLabel";
			// actionHelpLabel
			actionHelpLabel.Name = "actionHelpLabel";
			// advancedStateLabel
			advancedStateLabel.Name = "advancedStateLabel";
			// advancedSelectionListBox
			advancedSelectionListBox.HorizontalScrollbar = true;
			advancedSelectionListBox.Name = "advancedSelectionListBox";
			// openAdvancedEditorButton
			openAdvancedEditorButton.Name = "openAdvancedEditorButton";
			openAdvancedEditorButton.Click += OpenAdvancedEditor_Click;
			// saveJobButton
			saveJobButton.Name = "saveJobButton";
			saveJobButton.Click += SaveJob_Click;
			// startBackupButton
			startBackupButton.Name = "startBackupButton";
			startBackupButton.Click += StartBackup_Click;
			// driveTree
			driveTree.CheckBoxes = true;
			driveTree.HideSelection = false;
			driveTree.ImageList = driveImageList;
			driveTree.Name = "driveTree";
			driveTree.BeforeExpand += DriveTree_BeforeExpand;
			driveTree.AfterCheck += DriveTree_AfterCheck;
			// addFolderSourceButton
			addFolderSourceButton.Name = "addFolderSourceButton";
			addFolderSourceButton.Text = "Add Folder...";
			addFolderSourceButton.UseVisualStyleBackColor = true;
			addFolderSourceButton.Click += AddFolderSource_Click;
			// addFileSourceButton
			addFileSourceButton.Name = "addFileSourceButton";
			addFileSourceButton.Text = "Add File...";
			addFileSourceButton.UseVisualStyleBackColor = true;
			addFileSourceButton.Click += AddFileSource_Click;
			// removeSourceButton
			removeSourceButton.Name = "removeSourceButton";
			removeSourceButton.Text = "Remove Selected";
			removeSourceButton.UseVisualStyleBackColor = true;
			removeSourceButton.Click += RemoveSelectedSource_Click;
			// refreshDriveTreeButton
			refreshDriveTreeButton.Name = "refreshDriveTreeButton";
			refreshDriveTreeButton.Click += RefreshDriveTreeButton_Click;
			// expandTreeButton
			expandTreeButton.Name = "expandTreeButton";
			expandTreeButton.Click += ExpandTreeButton_Click;
			// collapseTreeButton
			collapseTreeButton.Name = "collapseTreeButton";
			collapseTreeButton.Click += CollapseTreeButton_Click;
			// showHiddenPartitionsCheckBox
			showHiddenPartitionsCheckBox.Name = "showHiddenPartitionsCheckBox";
			showHiddenPartitionsCheckBox.CheckedChanged += ShowHiddenPartitionsCheckBox_CheckedChanged;
			// nativeSourceListBox
			nativeSourceListBox.HorizontalScrollbar = true;
			nativeSourceListBox.Name = "nativeSourceListBox";
			nativeSourceListBox.SelectedIndexChanged += NativeSourceListBox_SelectedIndexChanged;
			// settingsScroll
			settingsScroll.Name = "settingsScroll";
			settingsScroll.Resize += SettingsScroll_Resize;
			// driveImageList
			driveImageList.ColorDepth = ColorDepth.Depth32Bit;
			driveImageList.ImageSize = new Size(16, 16);
			driveImageList.TransparentColor = Color.Transparent;
			// volumeAnimationTimer
			volumeAnimationTimer.Interval = 220;
			volumeAnimationTimer.Tick += VolumeAnimationTimer_Tick;
			// BackupWindowNewForm
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(1120, 840);
			Controls.Add(headerLabel);
			MinimumSize = new Size(1040, 800);
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Backup";
			FormClosed += BackupWindowNewForm_FormClosed;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
