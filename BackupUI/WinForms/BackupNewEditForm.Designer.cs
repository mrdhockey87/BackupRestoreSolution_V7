//BackupNewEditForm.Designer.cs

using System;
using System.Drawing;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
    partial class BackupNewEditForm
    {
        private System.ComponentModel.IContainer? components = null;

        // Root layout
        private Panel pnlScrollHost = null!;
        private TableLayoutPanel tlpRoot = null!;

        // What to backup
        private GroupBox gbWhat = null!;
        private FlowLayoutPanel flowWhat = null!;
        private Label lblWhatHelp = null!;
        private Label lblLoading = null!;
        private TreeView treeDrives = null!;
        private ContextMenuStrip treeContextMenu = null!;
        private ToolStripMenuItem miRemoveNetworkPath = null!;
        private FlowLayoutPanel flowTreeButtons = null!;
        private Button btnRefresh = null!;
        private Button btnExpandAll = null!;
        private Button btnCollapseAll = null!;
        private CheckBox chkShowHiddenPartitions = null!;

        // Settings
        private GroupBox gbSettings = null!;
        private FlowLayoutPanel flowSettings = null!;
        private Label lblBackupName = null!;
        private TextBox txtBackupName = null!;
        private Label lblBackupType = null!;
        private FlowLayoutPanel flowBackupTypes = null!;
        private RadioButton rbFullBackup = null!;
        private RadioButton rbIncremental = null!;
        private RadioButton rbDifferential = null!;
        private RadioButton rbSelectedFiles = null!;
        private RadioButton rbCloneDisk = null!;
        private RadioButton rbCloneVirtual = null!;
        private RadioButton rbCloneHyperV = null!;
        private RadioButton rbExportHyperV = null!;
        private TableLayoutPanel pnlCloneDestination = null!;
        private Label lblCloneDestination = null!;
        private TextBox txtCloneDestination = null!;
        private Button btnBrowseClone = null!;
        private TableLayoutPanel pnlDestination = null!;
        private Label lblDestination = null!;
        private TextBox txtDestination = null!;
        private Button btnBrowseDestination = null!;
        private CheckBox chkAutoSystemState = null!;
        private CheckBox chkCompress = null!;
        private CheckBox chkSplitFiles = null!;
        private CheckBox chkVerify = null!;
        private FlowLayoutPanel pnlRenameHyperV = null!;
        private CheckBox chkRenameHyperVSystem = null!;
        private FlowLayoutPanel pnlRenameHyperVName = null!;
        private Label lblRenameHyperVName = null!;
        private TextBox txtRenameHyperVSystemName = null!;
        private Button btnManageExclusions = null!;

        // Retention & encryption
        private GroupBox gbRetention = null!;
        private FlowLayoutPanel flowRetention = null!;
        private FlowLayoutPanel pnlSelectedRetention = null!;
        private Label lblSelRetHeader = null!;
        private Label lblSelRetDesc = null!;
        private FlowLayoutPanel flowSelRetRow = null!;
        private Label lblSelKeepLast = null!;
        private ComboBox cmbSelectedRetention = null!;
        private Label lblSelRetSuffix = null!;
        private FlowLayoutPanel pnlCloneRetention = null!;
        private Label lblCloneRetHeader = null!;
        private Label lblCloneRetDesc = null!;
        private FlowLayoutPanel flowCloneRetRow = null!;
        private Label lblCloneKeepLast = null!;
        private ComboBox cmbCloneRetention = null!;
        private Label lblCloneRetSuffix = null!;
        private FlowLayoutPanel pnlFullRetention = null!;
        private Label lblFullRetHeader = null!;
        private Label lblFullRetDesc = null!;
        private FlowLayoutPanel flowFullRetRow = null!;
        private Label lblFullKeepLast = null!;
        private TextBox txtRetainCount = null!;
        private Label lblFullRetSuffix = null!;
        private Label lblFullRetNote = null!;
        private CheckBox chkEncryptBackup = null!;
        private FlowLayoutPanel pnlEncryption = null!;
        private Label lblPassword = null!;
        private FlowLayoutPanel flowPasswordRow = null!;
        private TextBox txtEncryptionPassword = null!;
        private CheckBox chkShowEncryptionPassword = null!;
        private FlowLayoutPanel pnlVerifyPassword = null!;
        private Label lblVerifyPassword = null!;
        private TextBox txtVerifyEncryptionPassword = null!;
        private Label lblEncryptionInfo = null!;

        // Schedule
        private GroupBox gbSchedule = null!;
        private FlowLayoutPanel flowSchedule = null!;
        private CheckBox chkEnableSchedule = null!;
        private FlowLayoutPanel pnlSchedule = null!;
        private Label lblFrequency = null!;
        private ComboBox cmbFrequency = null!;
        private Label lblTime = null!;
        private FlowLayoutPanel flowTime = null!;
        private ComboBox cmbHour = null!;
        private Label lblTimeColon = null!;
        private ComboBox cmbMinute = null!;
        private ComboBox cmbAmPm = null!;
        private FlowLayoutPanel pnlWeekly = null!;
        private Label lblDays = null!;
        private FlowLayoutPanel flowDays = null!;
        private CheckBox chkMonday = null!;
        private CheckBox chkTuesday = null!;
        private CheckBox chkWednesday = null!;
        private CheckBox chkThursday = null!;
        private CheckBox chkFriday = null!;
        private CheckBox chkSaturday = null!;
        private CheckBox chkSunday = null!;
        private FlowLayoutPanel pnlMonthly = null!;
        private Label lblDayOfMonth = null!;
        private ComboBox cmbDayOfMonth = null!;

        // Progress and actions
        private FlowLayoutPanel pnlProgress = null!;
        private ProgressBar progressBar = null!;
        private Label lblProgress = null!;
        private FlowLayoutPanel pnlButtons = null!;
        private Button btnStartBackup = null!;
        private Button btnSaveJob = null!;
        private Button btnCancel = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			treeContextMenu = new ContextMenuStrip(components);
			miRemoveNetworkPath = new ToolStripMenuItem();
			pnlScrollHost = new Panel();
			tlpRoot = new TableLayoutPanel();
			gbWhat = new GroupBox();
			flowWhat = new FlowLayoutPanel();
			lblWhatHelp = new Label();
			lblLoading = new Label();
			treeDrives = new TreeView();
			flowTreeButtons = new FlowLayoutPanel();
			btnRefresh = new Button();
			btnExpandAll = new Button();
			btnCollapseAll = new Button();
			chkShowHiddenPartitions = new CheckBox();
			gbSettings = new GroupBox();
			flowSettings = new FlowLayoutPanel();
			lblBackupName = new Label();
			txtBackupName = new TextBox();
			lblBackupType = new Label();
			flowBackupTypes = new FlowLayoutPanel();
			rbFullBackup = new RadioButton();
			rbIncremental = new RadioButton();
			rbDifferential = new RadioButton();
			rbSelectedFiles = new RadioButton();
			rbCloneDisk = new RadioButton();
			rbCloneVirtual = new RadioButton();
			rbCloneHyperV = new RadioButton();
			rbExportHyperV = new RadioButton();
			pnlCloneDestination = new TableLayoutPanel();
			lblCloneDestination = new Label();
			txtCloneDestination = new TextBox();
			btnBrowseClone = new Button();
			pnlDestination = new TableLayoutPanel();
			lblDestination = new Label();
			txtDestination = new TextBox();
			btnBrowseDestination = new Button();
			chkAutoSystemState = new CheckBox();
			chkCompress = new CheckBox();
			chkSplitFiles = new CheckBox();
			chkVerify = new CheckBox();
			pnlRenameHyperV = new FlowLayoutPanel();
			chkRenameHyperVSystem = new CheckBox();
			pnlRenameHyperVName = new FlowLayoutPanel();
			lblRenameHyperVName = new Label();
			txtRenameHyperVSystemName = new TextBox();
			btnManageExclusions = new Button();
			gbRetention = new GroupBox();
			flowRetention = new FlowLayoutPanel();
			pnlSelectedRetention = new FlowLayoutPanel();
			lblSelRetHeader = new Label();
			lblSelRetDesc = new Label();
			flowSelRetRow = new FlowLayoutPanel();
			lblSelKeepLast = new Label();
			cmbSelectedRetention = new ComboBox();
			lblSelRetSuffix = new Label();
			pnlCloneRetention = new FlowLayoutPanel();
			lblCloneRetHeader = new Label();
			lblCloneRetDesc = new Label();
			flowCloneRetRow = new FlowLayoutPanel();
			lblCloneKeepLast = new Label();
			cmbCloneRetention = new ComboBox();
			lblCloneRetSuffix = new Label();
			pnlFullRetention = new FlowLayoutPanel();
			lblFullRetHeader = new Label();
			lblFullRetDesc = new Label();
			flowFullRetRow = new FlowLayoutPanel();
			lblFullKeepLast = new Label();
			txtRetainCount = new TextBox();
			lblFullRetSuffix = new Label();
			lblFullRetNote = new Label();
			chkEncryptBackup = new CheckBox();
			pnlEncryption = new FlowLayoutPanel();
			lblPassword = new Label();
			flowPasswordRow = new FlowLayoutPanel();
			txtEncryptionPassword = new TextBox();
			chkShowEncryptionPassword = new CheckBox();
			pnlVerifyPassword = new FlowLayoutPanel();
			lblVerifyPassword = new Label();
			txtVerifyEncryptionPassword = new TextBox();
			lblEncryptionInfo = new Label();
			gbSchedule = new GroupBox();
			flowSchedule = new FlowLayoutPanel();
			chkEnableSchedule = new CheckBox();
			pnlSchedule = new FlowLayoutPanel();
			lblFrequency = new Label();
			cmbFrequency = new ComboBox();
			lblTime = new Label();
			flowTime = new FlowLayoutPanel();
			cmbHour = new ComboBox();
			lblTimeColon = new Label();
			cmbMinute = new ComboBox();
			cmbAmPm = new ComboBox();
			pnlWeekly = new FlowLayoutPanel();
			lblDays = new Label();
			flowDays = new FlowLayoutPanel();
			chkMonday = new CheckBox();
			chkTuesday = new CheckBox();
			chkWednesday = new CheckBox();
			chkThursday = new CheckBox();
			chkFriday = new CheckBox();
			chkSaturday = new CheckBox();
			chkSunday = new CheckBox();
			pnlMonthly = new FlowLayoutPanel();
			lblDayOfMonth = new Label();
			cmbDayOfMonth = new ComboBox();
			pnlProgress = new FlowLayoutPanel();
			progressBar = new ProgressBar();
			lblProgress = new Label();
			pnlButtons = new FlowLayoutPanel();
			btnStartBackup = new Button();
			btnSaveJob = new Button();
			btnCancel = new Button();
			treeContextMenu.SuspendLayout();
			pnlScrollHost.SuspendLayout();
			tlpRoot.SuspendLayout();
			gbWhat.SuspendLayout();
			flowWhat.SuspendLayout();
			flowTreeButtons.SuspendLayout();
			gbSettings.SuspendLayout();
			flowSettings.SuspendLayout();
			flowBackupTypes.SuspendLayout();
			pnlCloneDestination.SuspendLayout();
			pnlDestination.SuspendLayout();
			pnlRenameHyperV.SuspendLayout();
			pnlRenameHyperVName.SuspendLayout();
			gbRetention.SuspendLayout();
			flowRetention.SuspendLayout();
			pnlSelectedRetention.SuspendLayout();
			flowSelRetRow.SuspendLayout();
			pnlCloneRetention.SuspendLayout();
			flowCloneRetRow.SuspendLayout();
			pnlFullRetention.SuspendLayout();
			flowFullRetRow.SuspendLayout();
			pnlEncryption.SuspendLayout();
			flowPasswordRow.SuspendLayout();
			pnlVerifyPassword.SuspendLayout();
			gbSchedule.SuspendLayout();
			flowSchedule.SuspendLayout();
			pnlSchedule.SuspendLayout();
			flowTime.SuspendLayout();
			pnlWeekly.SuspendLayout();
			flowDays.SuspendLayout();
			pnlMonthly.SuspendLayout();
			pnlProgress.SuspendLayout();
			pnlButtons.SuspendLayout();
			SuspendLayout();
			// 
			// treeContextMenu
			// 
			treeContextMenu.ImageScalingSize = new Size(18, 18);
			treeContextMenu.Items.AddRange(new ToolStripItem[] { miRemoveNetworkPath });
			treeContextMenu.Name = "treeContextMenu";
			treeContextMenu.Size = new Size(242, 26);
			treeContextMenu.Opening += treeContextMenu_Opening;
			// 
			// miRemoveNetworkPath
			// 
			miRemoveNetworkPath.Name = "miRemoveNetworkPath";
			miRemoveNetworkPath.Size = new Size(241, 22);
			miRemoveNetworkPath.Text = "Remove saved network path";
			miRemoveNetworkPath.Click += miRemoveNetworkPath_Click;
			// 
			// pnlScrollHost
			// 
			pnlScrollHost.AutoScroll = true;
			pnlScrollHost.Controls.Add(tlpRoot);
			pnlScrollHost.Dock = DockStyle.Fill;
			pnlScrollHost.Location = new Point(0, 0);
			pnlScrollHost.Name = "pnlScrollHost";
			pnlScrollHost.Size = new Size(1180, 720);
			pnlScrollHost.TabIndex = 1;
			// 
			// tlpRoot
			// 
			tlpRoot.AutoSize = true;
			tlpRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			tlpRoot.ColumnCount = 4;
			tlpRoot.ColumnStyles.Add(new ColumnStyle());
			tlpRoot.ColumnStyles.Add(new ColumnStyle());
			tlpRoot.ColumnStyles.Add(new ColumnStyle());
			tlpRoot.ColumnStyles.Add(new ColumnStyle());
			tlpRoot.Controls.Add(gbWhat, 0, 0);
			tlpRoot.Controls.Add(gbSettings, 1, 0);
			tlpRoot.Controls.Add(gbRetention, 2, 0);
			tlpRoot.Controls.Add(gbSchedule, 3, 0);
			tlpRoot.Controls.Add(pnlProgress, 0, 1);
			tlpRoot.Controls.Add(pnlButtons, 0, 2);
			tlpRoot.Location = new Point(12, 12);
			tlpRoot.Margin = new Padding(0);
			tlpRoot.Name = "tlpRoot";
			tlpRoot.RowCount = 3;
			tlpRoot.RowStyles.Add(new RowStyle());
			tlpRoot.RowStyles.Add(new RowStyle());
			tlpRoot.RowStyles.Add(new RowStyle());
			tlpRoot.Size = new Size(1953, 719);
			tlpRoot.TabIndex = 0;
			// 
			// gbWhat
			// 
			gbWhat.AutoSize = true;
			gbWhat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			gbWhat.Controls.Add(flowWhat);
			gbWhat.Dock = DockStyle.Fill;
			gbWhat.Location = new Point(0, 0);
			gbWhat.Margin = new Padding(0, 0, 10, 0);
			gbWhat.Name = "gbWhat";
			gbWhat.Size = new Size(449, 614);
			gbWhat.TabIndex = 0;
			gbWhat.TabStop = false;
			gbWhat.Text = "What to Backup";
			// 
			// flowWhat
			// 
			flowWhat.AutoSize = true;
			flowWhat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowWhat.Controls.Add(lblWhatHelp);
			flowWhat.Controls.Add(lblLoading);
			flowWhat.Controls.Add(treeDrives);
			flowWhat.Controls.Add(flowTreeButtons);
			flowWhat.Dock = DockStyle.Fill;
			flowWhat.FlowDirection = FlowDirection.TopDown;
			flowWhat.Location = new Point(3, 21);
			flowWhat.Name = "flowWhat";
			flowWhat.Padding = new Padding(6);
			flowWhat.Size = new Size(443, 590);
			flowWhat.TabIndex = 0;
			flowWhat.WrapContents = false;
			// 
			// lblWhatHelp
			// 
			lblWhatHelp.Font = new Font("Segoe UI", 8F);
			lblWhatHelp.ForeColor = SystemColors.GrayText;
			lblWhatHelp.Location = new Point(6, 6);
			lblWhatHelp.Margin = new Padding(0, 0, 0, 6);
			lblWhatHelp.Name = "lblWhatHelp";
			lblWhatHelp.Size = new Size(380, 48);
			lblWhatHelp.TabIndex = 0;
			lblWhatHelp.Text = "Check drives or volumes to backup. Files and folders are shown when volumes are unchecked. Boot volumes will automatically include system state backup.";
			// 
			// lblLoading
			// 
			lblLoading.AutoSize = true;
			lblLoading.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblLoading.Location = new Point(6, 60);
			lblLoading.Margin = new Padding(0, 0, 0, 4);
			lblLoading.Name = "lblLoading";
			lblLoading.Size = new Size(209, 19);
			lblLoading.TabIndex = 1;
			lblLoading.Text = "Loading drives and volumes...";
			lblLoading.Visible = false;
			// 
			// treeDrives
			// 
			treeDrives.CheckBoxes = true;
			treeDrives.ContextMenuStrip = treeContextMenu;
			treeDrives.FullRowSelect = true;
			treeDrives.HideSelection = false;
			treeDrives.Location = new Point(9, 86);
			treeDrives.Name = "treeDrives";
			treeDrives.ShowNodeToolTips = true;
			treeDrives.Size = new Size(380, 460);
			treeDrives.TabIndex = 2;
			treeDrives.BeforeCheck += treeDrives_BeforeCheck;
			treeDrives.AfterCheck += treeDrives_AfterCheck;
			treeDrives.AfterExpand += treeDrives_AfterExpand;
			treeDrives.NodeMouseClick += treeDrives_NodeMouseClick;
			// 
			// flowTreeButtons
			// 
			flowTreeButtons.AutoSize = true;
			flowTreeButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowTreeButtons.Controls.Add(btnRefresh);
			flowTreeButtons.Controls.Add(btnExpandAll);
			flowTreeButtons.Controls.Add(btnCollapseAll);
			flowTreeButtons.Controls.Add(chkShowHiddenPartitions);
			flowTreeButtons.Location = new Point(6, 555);
			flowTreeButtons.Margin = new Padding(0, 6, 0, 0);
			flowTreeButtons.Name = "flowTreeButtons";
			flowTreeButtons.Size = new Size(431, 29);
			flowTreeButtons.TabIndex = 3;
			flowTreeButtons.WrapContents = false;
			// 
			// btnRefresh
			// 
			btnRefresh.Location = new Point(0, 0);
			btnRefresh.Margin = new Padding(0, 0, 5, 0);
			btnRefresh.Name = "btnRefresh";
			btnRefresh.Size = new Size(80, 28);
			btnRefresh.TabIndex = 0;
			btnRefresh.Text = "Refresh";
			btnRefresh.Click += btnRefresh_Click;
			// 
			// btnExpandAll
			// 
			btnExpandAll.Location = new Point(85, 0);
			btnExpandAll.Margin = new Padding(0, 0, 5, 0);
			btnExpandAll.Name = "btnExpandAll";
			btnExpandAll.Size = new Size(80, 28);
			btnExpandAll.TabIndex = 1;
			btnExpandAll.Text = "Expand All";
			btnExpandAll.Click += btnExpandAll_Click;
			// 
			// btnCollapseAll
			// 
			btnCollapseAll.Location = new Point(170, 0);
			btnCollapseAll.Margin = new Padding(0, 0, 5, 0);
			btnCollapseAll.Name = "btnCollapseAll";
			btnCollapseAll.Size = new Size(80, 28);
			btnCollapseAll.TabIndex = 2;
			btnCollapseAll.Text = "Collapse All";
			btnCollapseAll.Click += btnCollapseAll_Click;
			// 
			// chkShowHiddenPartitions
			// 
			chkShowHiddenPartitions.AutoSize = true;
			chkShowHiddenPartitions.Location = new Point(260, 6);
			chkShowHiddenPartitions.Margin = new Padding(5, 6, 0, 0);
			chkShowHiddenPartitions.Name = "chkShowHiddenPartitions";
			chkShowHiddenPartitions.Size = new Size(171, 23);
			chkShowHiddenPartitions.TabIndex = 3;
			chkShowHiddenPartitions.Text = "Show Hidden Partitions";
			chkShowHiddenPartitions.Click += chkShowHiddenPartitions_Click;
			// 
			// gbSettings
			// 
			gbSettings.AutoSize = true;
			gbSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			gbSettings.Controls.Add(flowSettings);
			gbSettings.Dock = DockStyle.Fill;
			gbSettings.Location = new Point(459, 0);
			gbSettings.Margin = new Padding(0, 0, 10, 0);
			gbSettings.Name = "gbSettings";
			gbSettings.Size = new Size(344, 614);
			gbSettings.TabIndex = 1;
			gbSettings.TabStop = false;
			gbSettings.Text = "Settings";
			// 
			// flowSettings
			// 
			flowSettings.AutoSize = true;
			flowSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowSettings.Controls.Add(lblBackupName);
			flowSettings.Controls.Add(txtBackupName);
			flowSettings.Controls.Add(lblBackupType);
			flowSettings.Controls.Add(flowBackupTypes);
			flowSettings.Controls.Add(pnlCloneDestination);
			flowSettings.Controls.Add(pnlDestination);
			flowSettings.Controls.Add(chkAutoSystemState);
			flowSettings.Controls.Add(chkCompress);
			flowSettings.Controls.Add(chkSplitFiles);
			flowSettings.Controls.Add(chkVerify);
			flowSettings.Controls.Add(pnlRenameHyperV);
			flowSettings.Controls.Add(btnManageExclusions);
			flowSettings.Dock = DockStyle.Fill;
			flowSettings.FlowDirection = FlowDirection.TopDown;
			flowSettings.Location = new Point(3, 21);
			flowSettings.Name = "flowSettings";
			flowSettings.Padding = new Padding(6);
			flowSettings.Size = new Size(338, 590);
			flowSettings.TabIndex = 0;
			flowSettings.WrapContents = false;
			// 
			// lblBackupName
			// 
			lblBackupName.AutoSize = true;
			lblBackupName.Location = new Point(6, 6);
			lblBackupName.Margin = new Padding(0, 0, 0, 3);
			lblBackupName.Name = "lblBackupName";
			lblBackupName.Size = new Size(96, 19);
			lblBackupName.TabIndex = 0;
			lblBackupName.Text = "Backup Name:";
			// 
			// txtBackupName
			// 
			txtBackupName.Location = new Point(6, 28);
			txtBackupName.Margin = new Padding(0, 0, 0, 10);
			txtBackupName.Name = "txtBackupName";
			txtBackupName.Size = new Size(300, 25);
			txtBackupName.TabIndex = 1;
			// 
			// lblBackupType
			// 
			lblBackupType.AutoSize = true;
			lblBackupType.Location = new Point(6, 63);
			lblBackupType.Margin = new Padding(0, 0, 0, 3);
			lblBackupType.Name = "lblBackupType";
			lblBackupType.Size = new Size(88, 19);
			lblBackupType.TabIndex = 2;
			lblBackupType.Text = "Backup Type:";
			// 
			// flowBackupTypes
			// 
			flowBackupTypes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowBackupTypes.Controls.Add(rbFullBackup);
			flowBackupTypes.Controls.Add(rbIncremental);
			flowBackupTypes.Controls.Add(rbDifferential);
			flowBackupTypes.Controls.Add(rbSelectedFiles);
			flowBackupTypes.Controls.Add(rbCloneDisk);
			flowBackupTypes.Controls.Add(rbCloneVirtual);
			flowBackupTypes.Controls.Add(rbCloneHyperV);
			flowBackupTypes.Controls.Add(rbExportHyperV);
			flowBackupTypes.Location = new Point(6, 85);
			flowBackupTypes.Margin = new Padding(0, 0, 0, 10);
			flowBackupTypes.Name = "flowBackupTypes";
			flowBackupTypes.Size = new Size(320, 100);
			flowBackupTypes.TabIndex = 3;
			// 
			// rbFullBackup
			// 
			rbFullBackup.AutoSize = true;
			rbFullBackup.Checked = true;
			rbFullBackup.Location = new Point(0, 0);
			rbFullBackup.Margin = new Padding(0, 0, 12, 4);
			rbFullBackup.Name = "rbFullBackup";
			rbFullBackup.Size = new Size(96, 23);
			rbFullBackup.TabIndex = 0;
			rbFullBackup.TabStop = true;
			rbFullBackup.Text = "Full Backup";
			rbFullBackup.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbIncremental
			// 
			rbIncremental.AutoSize = true;
			rbIncremental.Location = new Point(108, 0);
			rbIncremental.Margin = new Padding(0, 0, 12, 4);
			rbIncremental.Name = "rbIncremental";
			rbIncremental.Size = new Size(156, 23);
			rbIncremental.TabIndex = 1;
			rbIncremental.Text = "Full then Incremental";
			rbIncremental.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbDifferential
			// 
			rbDifferential.AutoSize = true;
			rbDifferential.Location = new Point(0, 27);
			rbDifferential.Margin = new Padding(0, 0, 12, 4);
			rbDifferential.Name = "rbDifferential";
			rbDifferential.Size = new Size(150, 23);
			rbDifferential.TabIndex = 2;
			rbDifferential.Text = "Full then Differential";
			rbDifferential.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbSelectedFiles
			// 
			rbSelectedFiles.AutoSize = true;
			rbSelectedFiles.Location = new Point(0, 54);
			rbSelectedFiles.Margin = new Padding(0, 0, 12, 4);
			rbSelectedFiles.Name = "rbSelectedFiles";
			rbSelectedFiles.Size = new Size(159, 23);
			rbSelectedFiles.TabIndex = 3;
			rbSelectedFiles.Text = "Selected Files & Folders";
			rbSelectedFiles.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbCloneDisk
			// 
			rbCloneDisk.AutoSize = true;
			rbCloneDisk.Location = new Point(171, 54);
			rbCloneDisk.Margin = new Padding(0, 0, 12, 4);
			rbCloneDisk.Name = "rbCloneDisk";
			rbCloneDisk.Size = new Size(109, 23);
			rbCloneDisk.TabIndex = 4;
			rbCloneDisk.Text = "Clone to Disk";
			rbCloneDisk.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbCloneVirtual
			// 
			rbCloneVirtual.AutoSize = true;
			rbCloneVirtual.Location = new Point(0, 81);
			rbCloneVirtual.Margin = new Padding(0, 0, 12, 4);
			rbCloneVirtual.Name = "rbCloneVirtual";
			rbCloneVirtual.Size = new Size(217, 23);
			rbCloneVirtual.TabIndex = 5;
			rbCloneVirtual.Text = "Clone to Virtual Disk (Hyper-V)";
			rbCloneVirtual.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbCloneHyperV
			// 
			rbCloneHyperV.AutoSize = true;
			rbCloneHyperV.Location = new Point(0, 108);
			rbCloneHyperV.Margin = new Padding(0, 0, 12, 4);
			rbCloneHyperV.Name = "rbCloneHyperV";
			rbCloneHyperV.Size = new Size(166, 23);
			rbCloneHyperV.TabIndex = 6;
			rbCloneHyperV.Text = "Clone Hyper-V System";
			rbCloneHyperV.CheckedChanged += BackupType_CheckedChanged;
			// 
			// rbExportHyperV
			// 
			rbExportHyperV.AutoSize = true;
			rbExportHyperV.Location = new Point(0, 135);
			rbExportHyperV.Margin = new Padding(0, 0, 12, 4);
			rbExportHyperV.Name = "rbExportHyperV";
			rbExportHyperV.Size = new Size(170, 23);
			rbExportHyperV.TabIndex = 7;
			rbExportHyperV.Text = "Export Hyper-V System";
			rbExportHyperV.CheckedChanged += BackupType_CheckedChanged;
			// 
			// pnlCloneDestination
			// 
			pnlCloneDestination.AutoSize = true;
			pnlCloneDestination.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlCloneDestination.ColumnCount = 2;
			pnlCloneDestination.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
			pnlCloneDestination.ColumnStyles.Add(new ColumnStyle());
			pnlCloneDestination.Controls.Add(lblCloneDestination, 0, 0);
			pnlCloneDestination.Controls.Add(txtCloneDestination, 0, 1);
			pnlCloneDestination.Controls.Add(btnBrowseClone, 1, 1);
			pnlCloneDestination.Location = new Point(6, 195);
			pnlCloneDestination.Margin = new Padding(0, 0, 0, 10);
			pnlCloneDestination.Name = "pnlCloneDestination";
			pnlCloneDestination.RowCount = 2;
			pnlCloneDestination.RowStyles.Add(new RowStyle());
			pnlCloneDestination.RowStyles.Add(new RowStyle());
			pnlCloneDestination.Size = new Size(326, 54);
			pnlCloneDestination.TabIndex = 4;
			// 
			// lblCloneDestination
			// 
			lblCloneDestination.AutoSize = true;
			pnlCloneDestination.SetColumnSpan(lblCloneDestination, 2);
			lblCloneDestination.Location = new Point(0, 0);
			lblCloneDestination.Margin = new Padding(0, 0, 0, 3);
			lblCloneDestination.Name = "lblCloneDestination";
			lblCloneDestination.Size = new Size(146, 19);
			lblCloneDestination.TabIndex = 0;
			lblCloneDestination.Text = "Clone to Physical Disk:";
			// 
			// txtCloneDestination
			// 
			txtCloneDestination.Location = new Point(0, 22);
			txtCloneDestination.Margin = new Padding(0, 0, 5, 0);
			txtCloneDestination.Name = "txtCloneDestination";
			txtCloneDestination.ReadOnly = true;
			txtCloneDestination.Size = new Size(235, 25);
			txtCloneDestination.TabIndex = 1;
			// 
			// btnBrowseClone
			// 
			btnBrowseClone.Location = new Point(243, 25);
			btnBrowseClone.Name = "btnBrowseClone";
			btnBrowseClone.Size = new Size(80, 26);
			btnBrowseClone.TabIndex = 2;
			btnBrowseClone.Text = "Browse...";
			btnBrowseClone.Click += btnBrowseClone_Click;
			// 
			// pnlDestination
			// 
			pnlDestination.AutoSize = true;
			pnlDestination.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlDestination.ColumnCount = 2;
			pnlDestination.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
			pnlDestination.ColumnStyles.Add(new ColumnStyle());
			pnlDestination.Controls.Add(lblDestination, 0, 0);
			pnlDestination.Controls.Add(txtDestination, 0, 1);
			pnlDestination.Controls.Add(btnBrowseDestination, 1, 1);
			pnlDestination.Location = new Point(6, 259);
			pnlDestination.Margin = new Padding(0, 0, 0, 10);
			pnlDestination.Name = "pnlDestination";
			pnlDestination.RowCount = 2;
			pnlDestination.RowStyles.Add(new RowStyle());
			pnlDestination.RowStyles.Add(new RowStyle());
			pnlDestination.Size = new Size(326, 54);
			pnlDestination.TabIndex = 5;
			// 
			// lblDestination
			// 
			lblDestination.AutoSize = true;
			pnlDestination.SetColumnSpan(lblDestination, 2);
			lblDestination.Location = new Point(0, 0);
			lblDestination.Margin = new Padding(0, 0, 0, 3);
			lblDestination.Name = "lblDestination";
			lblDestination.Size = new Size(130, 19);
			lblDestination.TabIndex = 0;
			lblDestination.Text = "Backup Destination:";
			// 
			// txtDestination
			// 
			txtDestination.Location = new Point(0, 22);
			txtDestination.Margin = new Padding(0, 0, 5, 0);
			txtDestination.Name = "txtDestination";
			txtDestination.Size = new Size(235, 25);
			txtDestination.TabIndex = 1;
			// 
			// btnBrowseDestination
			// 
			btnBrowseDestination.Location = new Point(243, 25);
			btnBrowseDestination.Name = "btnBrowseDestination";
			btnBrowseDestination.Size = new Size(80, 26);
			btnBrowseDestination.TabIndex = 2;
			btnBrowseDestination.Text = "Browse...";
			btnBrowseDestination.Click += btnBrowseDestination_Click;
			// 
			// chkAutoSystemState
			// 
			chkAutoSystemState.AutoSize = true;
			chkAutoSystemState.Checked = true;
			chkAutoSystemState.CheckState = CheckState.Checked;
			chkAutoSystemState.Location = new Point(6, 323);
			chkAutoSystemState.Margin = new Padding(0, 0, 0, 6);
			chkAutoSystemState.Name = "chkAutoSystemState";
			chkAutoSystemState.Size = new Size(297, 23);
			chkAutoSystemState.TabIndex = 6;
			chkAutoSystemState.Text = "Auto-include system state for boot volumes";
			// 
			// chkCompress
			// 
			chkCompress.AutoSize = true;
			chkCompress.Checked = true;
			chkCompress.CheckState = CheckState.Checked;
			chkCompress.Location = new Point(6, 352);
			chkCompress.Margin = new Padding(0, 0, 0, 6);
			chkCompress.Name = "chkCompress";
			chkCompress.Size = new Size(168, 23);
			chkCompress.TabIndex = 7;
			chkCompress.Text = "Compress backup data";
			// 
			// chkSplitFiles
			// 
			chkSplitFiles.AutoSize = true;
			chkSplitFiles.Checked = true;
			chkSplitFiles.CheckState = CheckState.Checked;
			chkSplitFiles.Location = new Point(6, 381);
			chkSplitFiles.Margin = new Padding(0, 0, 0, 6);
			chkSplitFiles.Name = "chkSplitFiles";
			chkSplitFiles.Size = new Size(217, 23);
			chkSplitFiles.TabIndex = 8;
			chkSplitFiles.Text = "Split into 4.7GB files (DVD size)";
			// 
			// chkVerify
			// 
			chkVerify.AutoSize = true;
			chkVerify.Checked = true;
			chkVerify.CheckState = CheckState.Checked;
			chkVerify.Location = new Point(6, 410);
			chkVerify.Margin = new Padding(0, 0, 0, 6);
			chkVerify.Name = "chkVerify";
			chkVerify.Size = new Size(214, 23);
			chkVerify.TabIndex = 9;
			chkVerify.Text = "Verify backup after completion";
			// 
			// pnlRenameHyperV
			// 
			pnlRenameHyperV.AutoSize = true;
			pnlRenameHyperV.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlRenameHyperV.Controls.Add(chkRenameHyperVSystem);
			pnlRenameHyperV.Controls.Add(pnlRenameHyperVName);
			pnlRenameHyperV.FlowDirection = FlowDirection.TopDown;
			pnlRenameHyperV.Location = new Point(6, 443);
			pnlRenameHyperV.Margin = new Padding(0, 4, 0, 6);
			pnlRenameHyperV.Name = "pnlRenameHyperV";
			pnlRenameHyperV.Size = new Size(312, 88);
			pnlRenameHyperV.TabIndex = 10;
			pnlRenameHyperV.WrapContents = false;
			// 
			// chkRenameHyperVSystem
			// 
			chkRenameHyperVSystem.AutoSize = true;
			chkRenameHyperVSystem.Location = new Point(0, 0);
			chkRenameHyperVSystem.Margin = new Padding(0, 0, 0, 6);
			chkRenameHyperVSystem.Name = "chkRenameHyperVSystem";
			chkRenameHyperVSystem.Size = new Size(181, 23);
			chkRenameHyperVSystem.TabIndex = 0;
			chkRenameHyperVSystem.Text = "Rename Hyper-V System";
			chkRenameHyperVSystem.CheckedChanged += RenameHyperVSystem_CheckedChanged;
			// 
			// pnlRenameHyperVName
			// 
			pnlRenameHyperVName.AutoSize = true;
			pnlRenameHyperVName.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlRenameHyperVName.Controls.Add(lblRenameHyperVName);
			pnlRenameHyperVName.Controls.Add(txtRenameHyperVSystemName);
			pnlRenameHyperVName.FlowDirection = FlowDirection.TopDown;
			pnlRenameHyperVName.Location = new Point(3, 32);
			pnlRenameHyperVName.Name = "pnlRenameHyperVName";
			pnlRenameHyperVName.Size = new Size(306, 53);
			pnlRenameHyperVName.TabIndex = 1;
			pnlRenameHyperVName.WrapContents = false;
			// 
			// lblRenameHyperVName
			// 
			lblRenameHyperVName.AutoSize = true;
			lblRenameHyperVName.Location = new Point(0, 0);
			lblRenameHyperVName.Margin = new Padding(0, 0, 0, 3);
			lblRenameHyperVName.Name = "lblRenameHyperVName";
			lblRenameHyperVName.Size = new Size(79, 19);
			lblRenameHyperVName.TabIndex = 0;
			lblRenameHyperVName.Text = "New Name:";
			// 
			// txtRenameHyperVSystemName
			// 
			txtRenameHyperVSystemName.Location = new Point(3, 25);
			txtRenameHyperVSystemName.Name = "txtRenameHyperVSystemName";
			txtRenameHyperVSystemName.Size = new Size(300, 25);
			txtRenameHyperVSystemName.TabIndex = 1;
			// 
			// btnManageExclusions
			// 
			btnManageExclusions.Location = new Point(6, 541);
			btnManageExclusions.Margin = new Padding(0, 4, 0, 0);
			btnManageExclusions.Name = "btnManageExclusions";
			btnManageExclusions.Size = new Size(150, 28);
			btnManageExclusions.TabIndex = 11;
			btnManageExclusions.Text = "Manage Exclusions...";
			btnManageExclusions.Click += btnManageExclusions_Click;
			// 
			// gbRetention
			// 
			gbRetention.AutoSize = true;
			gbRetention.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			gbRetention.Controls.Add(flowRetention);
			gbRetention.Dock = DockStyle.Fill;
			gbRetention.Location = new Point(813, 0);
			gbRetention.Margin = new Padding(0, 0, 10, 0);
			gbRetention.Name = "gbRetention";
			gbRetention.Size = new Size(324, 614);
			gbRetention.TabIndex = 2;
			gbRetention.TabStop = false;
			gbRetention.Text = "Retention & Encryption";
			// 
			// flowRetention
			// 
			flowRetention.AutoSize = true;
			flowRetention.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowRetention.Controls.Add(pnlSelectedRetention);
			flowRetention.Controls.Add(pnlCloneRetention);
			flowRetention.Controls.Add(pnlFullRetention);
			flowRetention.Controls.Add(chkEncryptBackup);
			flowRetention.Controls.Add(pnlEncryption);
			flowRetention.Dock = DockStyle.Fill;
			flowRetention.FlowDirection = FlowDirection.TopDown;
			flowRetention.Location = new Point(3, 21);
			flowRetention.Name = "flowRetention";
			flowRetention.Padding = new Padding(6);
			flowRetention.Size = new Size(318, 590);
			flowRetention.TabIndex = 0;
			flowRetention.WrapContents = false;
			// 
			// pnlSelectedRetention
			// 
			pnlSelectedRetention.AutoSize = true;
			pnlSelectedRetention.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlSelectedRetention.Controls.Add(lblSelRetHeader);
			pnlSelectedRetention.Controls.Add(lblSelRetDesc);
			pnlSelectedRetention.Controls.Add(flowSelRetRow);
			pnlSelectedRetention.FlowDirection = FlowDirection.TopDown;
			pnlSelectedRetention.Location = new Point(6, 6);
			pnlSelectedRetention.Margin = new Padding(0, 0, 0, 12);
			pnlSelectedRetention.Name = "pnlSelectedRetention";
			pnlSelectedRetention.Size = new Size(306, 97);
			pnlSelectedRetention.TabIndex = 0;
			pnlSelectedRetention.WrapContents = false;
			// 
			// lblSelRetHeader
			// 
			lblSelRetHeader.AutoSize = true;
			lblSelRetHeader.Font = new Font("Segoe UI", 8.830189F, FontStyle.Bold);
			lblSelRetHeader.Location = new Point(0, 0);
			lblSelRetHeader.Margin = new Padding(0, 0, 0, 3);
			lblSelRetHeader.Name = "lblSelRetHeader";
			lblSelRetHeader.Size = new Size(209, 17);
			lblSelRetHeader.TabIndex = 0;
			lblSelRetHeader.Text = "Selected Files Version Retention:";
			// 
			// lblSelRetDesc
			// 
			lblSelRetDesc.Font = new Font("Segoe UI", 8F);
			lblSelRetDesc.ForeColor = SystemColors.GrayText;
			lblSelRetDesc.Location = new Point(3, 20);
			lblSelRetDesc.Name = "lblSelRetDesc";
			lblSelRetDesc.Size = new Size(300, 44);
			lblSelRetDesc.TabIndex = 1;
			lblSelRetDesc.Text = "Keep the last X versions (modified-file versions and deleted-file history) when Selected Files & Folder backups run. Older versions will be automatically deleted.";
			// 
			// flowSelRetRow
			// 
			flowSelRetRow.AutoSize = true;
			flowSelRetRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowSelRetRow.Controls.Add(lblSelKeepLast);
			flowSelRetRow.Controls.Add(cmbSelectedRetention);
			flowSelRetRow.Controls.Add(lblSelRetSuffix);
			flowSelRetRow.Location = new Point(3, 67);
			flowSelRetRow.Name = "flowSelRetRow";
			flowSelRetRow.Size = new Size(211, 27);
			flowSelRetRow.TabIndex = 2;
			flowSelRetRow.WrapContents = false;
			// 
			// lblSelKeepLast
			// 
			lblSelKeepLast.AutoSize = true;
			lblSelKeepLast.Location = new Point(0, 6);
			lblSelKeepLast.Margin = new Padding(0, 6, 5, 0);
			lblSelKeepLast.Name = "lblSelKeepLast";
			lblSelKeepLast.Size = new Size(64, 19);
			lblSelKeepLast.TabIndex = 0;
			lblSelKeepLast.Text = "Keep last";
			// 
			// cmbSelectedRetention
			// 
			cmbSelectedRetention.Location = new Point(69, 2);
			cmbSelectedRetention.Margin = new Padding(0, 2, 5, 0);
			cmbSelectedRetention.Name = "cmbSelectedRetention";
			cmbSelectedRetention.Size = new Size(70, 25);
			cmbSelectedRetention.TabIndex = 1;
			// 
			// lblSelRetSuffix
			// 
			lblSelRetSuffix.AutoSize = true;
			lblSelRetSuffix.Location = new Point(144, 6);
			lblSelRetSuffix.Margin = new Padding(0, 6, 0, 0);
			lblSelRetSuffix.Name = "lblSelRetSuffix";
			lblSelRetSuffix.Size = new Size(67, 19);
			lblSelRetSuffix.TabIndex = 2;
			lblSelRetSuffix.Text = "version(s)";
			// 
			// pnlCloneRetention
			// 
			pnlCloneRetention.AutoSize = true;
			pnlCloneRetention.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlCloneRetention.Controls.Add(lblCloneRetHeader);
			pnlCloneRetention.Controls.Add(lblCloneRetDesc);
			pnlCloneRetention.Controls.Add(flowCloneRetRow);
			pnlCloneRetention.FlowDirection = FlowDirection.TopDown;
			pnlCloneRetention.Location = new Point(6, 115);
			pnlCloneRetention.Margin = new Padding(0, 0, 0, 12);
			pnlCloneRetention.Name = "pnlCloneRetention";
			pnlCloneRetention.Size = new Size(306, 97);
			pnlCloneRetention.TabIndex = 1;
			pnlCloneRetention.WrapContents = false;
			// 
			// lblCloneRetHeader
			// 
			lblCloneRetHeader.AutoSize = true;
			lblCloneRetHeader.Font = new Font("Segoe UI", 8.830189F, FontStyle.Bold);
			lblCloneRetHeader.Location = new Point(0, 0);
			lblCloneRetHeader.Margin = new Padding(0, 0, 0, 3);
			lblCloneRetHeader.Name = "lblCloneRetHeader";
			lblCloneRetHeader.Size = new Size(111, 17);
			lblCloneRetHeader.TabIndex = 0;
			lblCloneRetHeader.Text = "Clone Retention:";
			// 
			// lblCloneRetDesc
			// 
			lblCloneRetDesc.Font = new Font("Segoe UI", 8F);
			lblCloneRetDesc.ForeColor = SystemColors.GrayText;
			lblCloneRetDesc.Location = new Point(3, 20);
			lblCloneRetDesc.Name = "lblCloneRetDesc";
			lblCloneRetDesc.Size = new Size(300, 44);
			lblCloneRetDesc.TabIndex = 1;
			lblCloneRetDesc.Text = "Keep the last X clones when scheduled or manual clones run. Older clones will be automatically deleted.";
			// 
			// flowCloneRetRow
			// 
			flowCloneRetRow.AutoSize = true;
			flowCloneRetRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowCloneRetRow.Controls.Add(lblCloneKeepLast);
			flowCloneRetRow.Controls.Add(cmbCloneRetention);
			flowCloneRetRow.Controls.Add(lblCloneRetSuffix);
			flowCloneRetRow.Location = new Point(3, 67);
			flowCloneRetRow.Name = "flowCloneRetRow";
			flowCloneRetRow.Size = new Size(199, 27);
			flowCloneRetRow.TabIndex = 2;
			flowCloneRetRow.WrapContents = false;
			// 
			// lblCloneKeepLast
			// 
			lblCloneKeepLast.AutoSize = true;
			lblCloneKeepLast.Location = new Point(0, 6);
			lblCloneKeepLast.Margin = new Padding(0, 6, 5, 0);
			lblCloneKeepLast.Name = "lblCloneKeepLast";
			lblCloneKeepLast.Size = new Size(64, 19);
			lblCloneKeepLast.TabIndex = 0;
			lblCloneKeepLast.Text = "Keep last";
			// 
			// cmbCloneRetention
			// 
			cmbCloneRetention.Location = new Point(69, 2);
			cmbCloneRetention.Margin = new Padding(0, 2, 5, 0);
			cmbCloneRetention.Name = "cmbCloneRetention";
			cmbCloneRetention.Size = new Size(70, 25);
			cmbCloneRetention.TabIndex = 1;
			// 
			// lblCloneRetSuffix
			// 
			lblCloneRetSuffix.AutoSize = true;
			lblCloneRetSuffix.Location = new Point(144, 6);
			lblCloneRetSuffix.Margin = new Padding(0, 6, 0, 0);
			lblCloneRetSuffix.Name = "lblCloneRetSuffix";
			lblCloneRetSuffix.Size = new Size(55, 19);
			lblCloneRetSuffix.TabIndex = 2;
			lblCloneRetSuffix.Text = "clone(s)";
			// 
			// pnlFullRetention
			// 
			pnlFullRetention.AutoSize = true;
			pnlFullRetention.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlFullRetention.Controls.Add(lblFullRetHeader);
			pnlFullRetention.Controls.Add(lblFullRetDesc);
			pnlFullRetention.Controls.Add(flowFullRetRow);
			pnlFullRetention.Controls.Add(lblFullRetNote);
			pnlFullRetention.FlowDirection = FlowDirection.TopDown;
			pnlFullRetention.Location = new Point(6, 224);
			pnlFullRetention.Margin = new Padding(0, 0, 0, 12);
			pnlFullRetention.Name = "pnlFullRetention";
			pnlFullRetention.Size = new Size(306, 131);
			pnlFullRetention.TabIndex = 2;
			pnlFullRetention.WrapContents = false;
			// 
			// lblFullRetHeader
			// 
			lblFullRetHeader.AutoSize = true;
			lblFullRetHeader.Font = new Font("Segoe UI", 8.830189F, FontStyle.Bold);
			lblFullRetHeader.Location = new Point(0, 0);
			lblFullRetHeader.Margin = new Padding(0, 0, 0, 3);
			lblFullRetHeader.Name = "lblFullRetHeader";
			lblFullRetHeader.Size = new Size(147, 17);
			lblFullRetHeader.TabIndex = 0;
			lblFullRetHeader.Text = "Full Backup Retention:";
			// 
			// lblFullRetDesc
			// 
			lblFullRetDesc.Font = new Font("Segoe UI", 8F);
			lblFullRetDesc.ForeColor = SystemColors.GrayText;
			lblFullRetDesc.Location = new Point(3, 20);
			lblFullRetDesc.Name = "lblFullRetDesc";
			lblFullRetDesc.Size = new Size(300, 30);
			lblFullRetDesc.TabIndex = 1;
			lblFullRetDesc.Text = "Specify how many full backups to keep (applies to Full backup type only)";
			// 
			// flowFullRetRow
			// 
			flowFullRetRow.AutoSize = true;
			flowFullRetRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowFullRetRow.Controls.Add(lblFullKeepLast);
			flowFullRetRow.Controls.Add(txtRetainCount);
			flowFullRetRow.Controls.Add(lblFullRetSuffix);
			flowFullRetRow.Location = new Point(3, 53);
			flowFullRetRow.Name = "flowFullRetRow";
			flowFullRetRow.Size = new Size(223, 27);
			flowFullRetRow.TabIndex = 2;
			flowFullRetRow.WrapContents = false;
			// 
			// lblFullKeepLast
			// 
			lblFullKeepLast.AutoSize = true;
			lblFullKeepLast.Location = new Point(0, 6);
			lblFullKeepLast.Margin = new Padding(0, 6, 5, 0);
			lblFullKeepLast.Name = "lblFullKeepLast";
			lblFullKeepLast.Size = new Size(64, 19);
			lblFullKeepLast.TabIndex = 0;
			lblFullKeepLast.Text = "Keep last";
			// 
			// txtRetainCount
			// 
			txtRetainCount.Location = new Point(69, 2);
			txtRetainCount.Margin = new Padding(0, 2, 5, 0);
			txtRetainCount.Name = "txtRetainCount";
			txtRetainCount.Size = new Size(60, 25);
			txtRetainCount.TabIndex = 1;
			txtRetainCount.Text = "1";
			txtRetainCount.TextAlign = HorizontalAlignment.Center;
			// 
			// lblFullRetSuffix
			// 
			lblFullRetSuffix.AutoSize = true;
			lblFullRetSuffix.Location = new Point(134, 6);
			lblFullRetSuffix.Margin = new Padding(0, 6, 0, 0);
			lblFullRetSuffix.Name = "lblFullRetSuffix";
			lblFullRetSuffix.Size = new Size(89, 19);
			lblFullRetSuffix.TabIndex = 2;
			lblFullRetSuffix.Text = "full backup(s)";
			// 
			// lblFullRetNote
			// 
			lblFullRetNote.Font = new Font("Segoe UI", 8F);
			lblFullRetNote.ForeColor = SystemColors.GrayText;
			lblFullRetNote.Location = new Point(0, 87);
			lblFullRetNote.Margin = new Padding(0, 4, 0, 0);
			lblFullRetNote.Name = "lblFullRetNote";
			lblFullRetNote.Size = new Size(300, 44);
			lblFullRetNote.TabIndex = 3;
			lblFullRetNote.Text = "Note: When keeping more than 1 backup, date/time will be added to backup names. Old backups are only deleted after successful verification.";
			// 
			// chkEncryptBackup
			// 
			chkEncryptBackup.AutoSize = true;
			chkEncryptBackup.Location = new Point(6, 367);
			chkEncryptBackup.Margin = new Padding(0, 0, 0, 6);
			chkEncryptBackup.Name = "chkEncryptBackup";
			chkEncryptBackup.Size = new Size(122, 23);
			chkEncryptBackup.TabIndex = 3;
			chkEncryptBackup.Text = "Encrypt Backup";
			chkEncryptBackup.CheckedChanged += EncryptBackup_CheckedChanged;
			// 
			// pnlEncryption
			// 
			pnlEncryption.AutoSize = true;
			pnlEncryption.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlEncryption.Controls.Add(lblPassword);
			pnlEncryption.Controls.Add(flowPasswordRow);
			pnlEncryption.Controls.Add(pnlVerifyPassword);
			pnlEncryption.Controls.Add(lblEncryptionInfo);
			pnlEncryption.FlowDirection = FlowDirection.TopDown;
			pnlEncryption.Location = new Point(6, 396);
			pnlEncryption.Margin = new Padding(0);
			pnlEncryption.Name = "pnlEncryption";
			pnlEncryption.Size = new Size(306, 162);
			pnlEncryption.TabIndex = 4;
			pnlEncryption.WrapContents = false;
			// 
			// lblPassword
			// 
			lblPassword.AutoSize = true;
			lblPassword.Location = new Point(0, 0);
			lblPassword.Margin = new Padding(0, 0, 0, 3);
			lblPassword.Name = "lblPassword";
			lblPassword.Size = new Size(160, 19);
			lblPassword.TabIndex = 0;
			lblPassword.Text = "Password for Encryption:";
			// 
			// flowPasswordRow
			// 
			flowPasswordRow.AutoSize = true;
			flowPasswordRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowPasswordRow.Controls.Add(txtEncryptionPassword);
			flowPasswordRow.Controls.Add(chkShowEncryptionPassword);
			flowPasswordRow.Location = new Point(0, 22);
			flowPasswordRow.Margin = new Padding(0, 0, 0, 8);
			flowPasswordRow.Name = "flowPasswordRow";
			flowPasswordRow.Size = new Size(296, 27);
			flowPasswordRow.TabIndex = 1;
			flowPasswordRow.WrapContents = false;
			// 
			// txtEncryptionPassword
			// 
			txtEncryptionPassword.Location = new Point(0, 0);
			txtEncryptionPassword.Margin = new Padding(0, 0, 5, 0);
			txtEncryptionPassword.Name = "txtEncryptionPassword";
			txtEncryptionPassword.Size = new Size(230, 25);
			txtEncryptionPassword.TabIndex = 0;
			txtEncryptionPassword.UseSystemPasswordChar = true;
			// 
			// chkShowEncryptionPassword
			// 
			chkShowEncryptionPassword.AutoSize = true;
			chkShowEncryptionPassword.Location = new Point(235, 4);
			chkShowEncryptionPassword.Margin = new Padding(0, 4, 0, 0);
			chkShowEncryptionPassword.Name = "chkShowEncryptionPassword";
			chkShowEncryptionPassword.Size = new Size(61, 23);
			chkShowEncryptionPassword.TabIndex = 1;
			chkShowEncryptionPassword.Text = "Show";
			chkShowEncryptionPassword.CheckedChanged += chkShowEncryptionPassword_CheckedChanged;
			// 
			// pnlVerifyPassword
			// 
			pnlVerifyPassword.AutoSize = true;
			pnlVerifyPassword.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlVerifyPassword.Controls.Add(lblVerifyPassword);
			pnlVerifyPassword.Controls.Add(txtVerifyEncryptionPassword);
			pnlVerifyPassword.FlowDirection = FlowDirection.TopDown;
			pnlVerifyPassword.Location = new Point(0, 57);
			pnlVerifyPassword.Margin = new Padding(0, 0, 0, 8);
			pnlVerifyPassword.Name = "pnlVerifyPassword";
			pnlVerifyPassword.Size = new Size(306, 53);
			pnlVerifyPassword.TabIndex = 2;
			pnlVerifyPassword.WrapContents = false;
			// 
			// lblVerifyPassword
			// 
			lblVerifyPassword.AutoSize = true;
			lblVerifyPassword.Location = new Point(0, 0);
			lblVerifyPassword.Margin = new Padding(0, 0, 0, 3);
			lblVerifyPassword.Name = "lblVerifyPassword";
			lblVerifyPassword.Size = new Size(108, 19);
			lblVerifyPassword.TabIndex = 0;
			lblVerifyPassword.Text = "Verify Password:";
			// 
			// txtVerifyEncryptionPassword
			// 
			txtVerifyEncryptionPassword.Location = new Point(3, 25);
			txtVerifyEncryptionPassword.Name = "txtVerifyEncryptionPassword";
			txtVerifyEncryptionPassword.Size = new Size(300, 25);
			txtVerifyEncryptionPassword.TabIndex = 1;
			txtVerifyEncryptionPassword.UseSystemPasswordChar = true;
			// 
			// lblEncryptionInfo
			// 
			lblEncryptionInfo.Font = new Font("Segoe UI", 8F);
			lblEncryptionInfo.ForeColor = Color.Gray;
			lblEncryptionInfo.Location = new Point(3, 118);
			lblEncryptionInfo.Name = "lblEncryptionInfo";
			lblEncryptionInfo.Size = new Size(300, 44);
			lblEncryptionInfo.TabIndex = 3;
			lblEncryptionInfo.Text = "Encrypted backups use AES-128. Stored scheduled-backup passwords are protected with Windows DPAPI LocalMachine.";
			// 
			// gbSchedule
			// 
			gbSchedule.AutoSize = true;
			gbSchedule.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			gbSchedule.Controls.Add(flowSchedule);
			gbSchedule.Dock = DockStyle.Fill;
			gbSchedule.Location = new Point(1150, 3);
			gbSchedule.Name = "gbSchedule";
			gbSchedule.Size = new Size(800, 608);
			gbSchedule.TabIndex = 3;
			gbSchedule.TabStop = false;
			gbSchedule.Text = "Schedule";
			// 
			// flowSchedule
			// 
			flowSchedule.AutoSize = true;
			flowSchedule.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowSchedule.Controls.Add(chkEnableSchedule);
			flowSchedule.Controls.Add(pnlSchedule);
			flowSchedule.Dock = DockStyle.Fill;
			flowSchedule.FlowDirection = FlowDirection.TopDown;
			flowSchedule.Location = new Point(3, 21);
			flowSchedule.Name = "flowSchedule";
			flowSchedule.Padding = new Padding(6);
			flowSchedule.Size = new Size(794, 584);
			flowSchedule.TabIndex = 0;
			flowSchedule.WrapContents = false;
			// 
			// chkEnableSchedule
			// 
			chkEnableSchedule.AutoSize = true;
			chkEnableSchedule.Location = new Point(6, 6);
			chkEnableSchedule.Margin = new Padding(0, 0, 0, 6);
			chkEnableSchedule.Name = "chkEnableSchedule";
			chkEnableSchedule.Size = new Size(182, 23);
			chkEnableSchedule.TabIndex = 0;
			chkEnableSchedule.Text = "Enable Scheduled Backup";
			chkEnableSchedule.CheckedChanged += Schedule_CheckedChanged;
			// 
			// pnlSchedule
			// 
			pnlSchedule.AutoSize = true;
			pnlSchedule.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlSchedule.Controls.Add(lblFrequency);
			pnlSchedule.Controls.Add(cmbFrequency);
			pnlSchedule.Controls.Add(lblTime);
			pnlSchedule.Controls.Add(flowTime);
			pnlSchedule.Controls.Add(pnlWeekly);
			pnlSchedule.Controls.Add(pnlMonthly);
			pnlSchedule.FlowDirection = FlowDirection.TopDown;
			pnlSchedule.Location = new Point(9, 38);
			pnlSchedule.Name = "pnlSchedule";
			pnlSchedule.Size = new Size(776, 243);
			pnlSchedule.TabIndex = 1;
			pnlSchedule.WrapContents = false;
			// 
			// lblFrequency
			// 
			lblFrequency.AutoSize = true;
			lblFrequency.Location = new Point(0, 0);
			lblFrequency.Margin = new Padding(0, 0, 0, 3);
			lblFrequency.Name = "lblFrequency";
			lblFrequency.Size = new Size(75, 19);
			lblFrequency.TabIndex = 0;
			lblFrequency.Text = "Frequency:";
			// 
			// cmbFrequency
			// 
			cmbFrequency.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbFrequency.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly", "Once" });
			cmbFrequency.Location = new Point(0, 22);
			cmbFrequency.Margin = new Padding(0, 0, 0, 8);
			cmbFrequency.Name = "cmbFrequency";
			cmbFrequency.Size = new Size(200, 25);
			cmbFrequency.TabIndex = 1;
			cmbFrequency.SelectedIndexChanged += Frequency_SelectedIndexChanged;
			// 
			// lblTime
			// 
			lblTime.AutoSize = true;
			lblTime.Location = new Point(0, 55);
			lblTime.Margin = new Padding(0, 0, 0, 3);
			lblTime.Name = "lblTime";
			lblTime.Size = new Size(41, 19);
			lblTime.TabIndex = 2;
			lblTime.Text = "Time:";
			// 
			// flowTime
			// 
			flowTime.AutoSize = true;
			flowTime.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowTime.Controls.Add(cmbHour);
			flowTime.Controls.Add(lblTimeColon);
			flowTime.Controls.Add(cmbMinute);
			flowTime.Controls.Add(cmbAmPm);
			flowTime.Location = new Point(0, 77);
			flowTime.Margin = new Padding(0, 0, 0, 8);
			flowTime.Name = "flowTime";
			flowTime.Size = new Size(218, 31);
			flowTime.TabIndex = 3;
			flowTime.WrapContents = false;
			// 
			// cmbHour
			// 
			cmbHour.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbHour.Location = new Point(0, 0);
			cmbHour.Margin = new Padding(0, 0, 5, 0);
			cmbHour.Name = "cmbHour";
			cmbHour.Size = new Size(60, 25);
			cmbHour.TabIndex = 0;
			// 
			// lblTimeColon
			// 
			lblTimeColon.AutoSize = true;
			lblTimeColon.Location = new Point(65, 4);
			lblTimeColon.Margin = new Padding(0, 4, 5, 0);
			lblTimeColon.Name = "lblTimeColon";
			lblTimeColon.Size = new Size(12, 19);
			lblTimeColon.TabIndex = 1;
			lblTimeColon.Text = ":";
			// 
			// cmbMinute
			// 
			cmbMinute.Location = new Point(82, 0);
			cmbMinute.Margin = new Padding(0, 0, 10, 0);
			cmbMinute.Name = "cmbMinute";
			cmbMinute.Size = new Size(60, 25);
			cmbMinute.TabIndex = 2;
			// 
			// cmbAmPm
			// 
			cmbAmPm.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbAmPm.Items.AddRange(new object[] { "AM", "PM" });
			cmbAmPm.Location = new Point(155, 3);
			cmbAmPm.Name = "cmbAmPm";
			cmbAmPm.Size = new Size(60, 25);
			cmbAmPm.TabIndex = 3;
			// 
			// pnlWeekly
			// 
			pnlWeekly.AutoSize = true;
			pnlWeekly.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlWeekly.Controls.Add(lblDays);
			pnlWeekly.Controls.Add(flowDays);
			pnlWeekly.FlowDirection = FlowDirection.TopDown;
			pnlWeekly.Location = new Point(0, 116);
			pnlWeekly.Margin = new Padding(0, 0, 0, 8);
			pnlWeekly.Name = "pnlWeekly";
			pnlWeekly.Size = new Size(776, 58);
			pnlWeekly.TabIndex = 4;
			pnlWeekly.WrapContents = false;
			// 
			// lblDays
			// 
			lblDays.AutoSize = true;
			lblDays.Location = new Point(0, 0);
			lblDays.Margin = new Padding(0, 0, 0, 3);
			lblDays.Name = "lblDays";
			lblDays.Size = new Size(95, 19);
			lblDays.TabIndex = 0;
			lblDays.Text = "Days of Week:";
			// 
			// flowDays
			// 
			flowDays.AutoSize = true;
			flowDays.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowDays.Controls.Add(chkMonday);
			flowDays.Controls.Add(chkTuesday);
			flowDays.Controls.Add(chkWednesday);
			flowDays.Controls.Add(chkThursday);
			flowDays.Controls.Add(chkFriday);
			flowDays.Controls.Add(chkSaturday);
			flowDays.Controls.Add(chkSunday);
			flowDays.Location = new Point(3, 25);
			flowDays.Name = "flowDays";
			flowDays.Size = new Size(770, 30);
			flowDays.TabIndex = 1;
			// 
			// chkMonday
			// 
			chkMonday.Location = new Point(3, 3);
			chkMonday.Name = "chkMonday";
			chkMonday.Size = new Size(104, 24);
			chkMonday.TabIndex = 0;
			chkMonday.Text = "Mon";
			// 
			// chkTuesday
			// 
			chkTuesday.Location = new Point(113, 3);
			chkTuesday.Name = "chkTuesday";
			chkTuesday.Size = new Size(104, 24);
			chkTuesday.TabIndex = 1;
			chkTuesday.Text = "Tue";
			// 
			// chkWednesday
			// 
			chkWednesday.Location = new Point(223, 3);
			chkWednesday.Name = "chkWednesday";
			chkWednesday.Size = new Size(104, 24);
			chkWednesday.TabIndex = 2;
			chkWednesday.Text = "Wed";
			// 
			// chkThursday
			// 
			chkThursday.Location = new Point(333, 3);
			chkThursday.Name = "chkThursday";
			chkThursday.Size = new Size(104, 24);
			chkThursday.TabIndex = 3;
			chkThursday.Text = "Thu";
			// 
			// chkFriday
			// 
			chkFriday.Location = new Point(443, 3);
			chkFriday.Name = "chkFriday";
			chkFriday.Size = new Size(104, 24);
			chkFriday.TabIndex = 4;
			chkFriday.Text = "Fri";
			// 
			// chkSaturday
			// 
			chkSaturday.Location = new Point(553, 3);
			chkSaturday.Name = "chkSaturday";
			chkSaturday.Size = new Size(104, 24);
			chkSaturday.TabIndex = 5;
			chkSaturday.Text = "Sat";
			// 
			// chkSunday
			// 
			chkSunday.Location = new Point(663, 3);
			chkSunday.Name = "chkSunday";
			chkSunday.Size = new Size(104, 24);
			chkSunday.TabIndex = 6;
			chkSunday.Text = "Sun";
			// 
			// pnlMonthly
			// 
			pnlMonthly.AutoSize = true;
			pnlMonthly.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlMonthly.Controls.Add(lblDayOfMonth);
			pnlMonthly.Controls.Add(cmbDayOfMonth);
			pnlMonthly.FlowDirection = FlowDirection.TopDown;
			pnlMonthly.Location = new Point(0, 182);
			pnlMonthly.Margin = new Padding(0, 0, 0, 8);
			pnlMonthly.Name = "pnlMonthly";
			pnlMonthly.Size = new Size(98, 53);
			pnlMonthly.TabIndex = 5;
			pnlMonthly.WrapContents = false;
			// 
			// lblDayOfMonth
			// 
			lblDayOfMonth.AutoSize = true;
			lblDayOfMonth.Location = new Point(0, 0);
			lblDayOfMonth.Margin = new Padding(0, 0, 0, 3);
			lblDayOfMonth.Name = "lblDayOfMonth";
			lblDayOfMonth.Size = new Size(98, 19);
			lblDayOfMonth.TabIndex = 0;
			lblDayOfMonth.Text = "Day of Month:";
			// 
			// cmbDayOfMonth
			// 
			cmbDayOfMonth.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbDayOfMonth.Location = new Point(3, 25);
			cmbDayOfMonth.Name = "cmbDayOfMonth";
			cmbDayOfMonth.Size = new Size(60, 25);
			cmbDayOfMonth.TabIndex = 1;
			// 
			// pnlProgress
			// 
			pnlProgress.AutoSize = true;
			pnlProgress.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			tlpRoot.SetColumnSpan(pnlProgress, 4);
			pnlProgress.Controls.Add(progressBar);
			pnlProgress.Controls.Add(lblProgress);
			pnlProgress.FlowDirection = FlowDirection.TopDown;
			pnlProgress.Location = new Point(0, 624);
			pnlProgress.Margin = new Padding(0, 10, 0, 0);
			pnlProgress.Name = "pnlProgress";
			pnlProgress.Size = new Size(1006, 49);
			pnlProgress.TabIndex = 4;
			pnlProgress.Visible = false;
			pnlProgress.WrapContents = false;
			// 
			// progressBar
			// 
			progressBar.Location = new Point(3, 3);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(1000, 20);
			progressBar.TabIndex = 0;
			// 
			// lblProgress
			// 
			lblProgress.AutoSize = true;
			lblProgress.Location = new Point(0, 30);
			lblProgress.Margin = new Padding(0, 4, 0, 0);
			lblProgress.Name = "lblProgress";
			lblProgress.Size = new Size(0, 19);
			lblProgress.TabIndex = 1;
			// 
			// pnlButtons
			// 
			pnlButtons.Anchor = AnchorStyles.Right;
			pnlButtons.AutoSize = true;
			pnlButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			tlpRoot.SetColumnSpan(pnlButtons, 4);
			pnlButtons.Controls.Add(btnStartBackup);
			pnlButtons.Controls.Add(btnSaveJob);
			pnlButtons.Controls.Add(btnCancel);
			pnlButtons.Location = new Point(1637, 683);
			pnlButtons.Margin = new Padding(0, 10, 0, 0);
			pnlButtons.Name = "pnlButtons";
			pnlButtons.Size = new Size(316, 36);
			pnlButtons.TabIndex = 5;
			pnlButtons.WrapContents = false;
			// 
			// btnStartBackup
			// 
			btnStartBackup.Location = new Point(0, 0);
			btnStartBackup.Margin = new Padding(0, 0, 5, 0);
			btnStartBackup.Name = "btnStartBackup";
			btnStartBackup.Size = new Size(100, 30);
			btnStartBackup.TabIndex = 0;
			btnStartBackup.Text = "Start Backup";
			btnStartBackup.Click += btnStartBackup_Click;
			// 
			// btnSaveJob
			// 
			btnSaveJob.Location = new Point(105, 0);
			btnSaveJob.Margin = new Padding(0, 0, 5, 0);
			btnSaveJob.Name = "btnSaveJob";
			btnSaveJob.Size = new Size(100, 30);
			btnSaveJob.TabIndex = 1;
			btnSaveJob.Text = "Save Job";
			btnSaveJob.Click += btnSaveJob_Click;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Location = new Point(213, 3);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(100, 30);
			btnCancel.TabIndex = 2;
			btnCancel.Text = "Cancel";
			btnCancel.Click += btnCancel_Click;
			// 
			// BackupNewEditForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1180, 720);
			Controls.Add(pnlScrollHost);
			Font = new Font("Segoe UI", 9F);
			MinimumSize = new Size(520, 360);
			Name = "BackupNewEditForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Backup";
			treeContextMenu.ResumeLayout(false);
			pnlScrollHost.ResumeLayout(false);
			pnlScrollHost.PerformLayout();
			tlpRoot.ResumeLayout(false);
			tlpRoot.PerformLayout();
			gbWhat.ResumeLayout(false);
			gbWhat.PerformLayout();
			flowWhat.ResumeLayout(false);
			flowWhat.PerformLayout();
			flowTreeButtons.ResumeLayout(false);
			flowTreeButtons.PerformLayout();
			gbSettings.ResumeLayout(false);
			gbSettings.PerformLayout();
			flowSettings.ResumeLayout(false);
			flowSettings.PerformLayout();
			flowBackupTypes.ResumeLayout(false);
			flowBackupTypes.PerformLayout();
			pnlCloneDestination.ResumeLayout(false);
			pnlCloneDestination.PerformLayout();
			pnlDestination.ResumeLayout(false);
			pnlDestination.PerformLayout();
			pnlRenameHyperV.ResumeLayout(false);
			pnlRenameHyperV.PerformLayout();
			pnlRenameHyperVName.ResumeLayout(false);
			pnlRenameHyperVName.PerformLayout();
			gbRetention.ResumeLayout(false);
			gbRetention.PerformLayout();
			flowRetention.ResumeLayout(false);
			flowRetention.PerformLayout();
			pnlSelectedRetention.ResumeLayout(false);
			pnlSelectedRetention.PerformLayout();
			flowSelRetRow.ResumeLayout(false);
			flowSelRetRow.PerformLayout();
			pnlCloneRetention.ResumeLayout(false);
			pnlCloneRetention.PerformLayout();
			flowCloneRetRow.ResumeLayout(false);
			flowCloneRetRow.PerformLayout();
			pnlFullRetention.ResumeLayout(false);
			pnlFullRetention.PerformLayout();
			flowFullRetRow.ResumeLayout(false);
			flowFullRetRow.PerformLayout();
			pnlEncryption.ResumeLayout(false);
			pnlEncryption.PerformLayout();
			flowPasswordRow.ResumeLayout(false);
			flowPasswordRow.PerformLayout();
			pnlVerifyPassword.ResumeLayout(false);
			pnlVerifyPassword.PerformLayout();
			gbSchedule.ResumeLayout(false);
			gbSchedule.PerformLayout();
			flowSchedule.ResumeLayout(false);
			flowSchedule.PerformLayout();
			pnlSchedule.ResumeLayout(false);
			pnlSchedule.PerformLayout();
			flowTime.ResumeLayout(false);
			flowTime.PerformLayout();
			pnlWeekly.ResumeLayout(false);
			pnlWeekly.PerformLayout();
			flowDays.ResumeLayout(false);
			pnlMonthly.ResumeLayout(false);
			pnlMonthly.PerformLayout();
			pnlProgress.ResumeLayout(false);
			pnlProgress.PerformLayout();
			pnlButtons.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
