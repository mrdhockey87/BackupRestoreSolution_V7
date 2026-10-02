using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForm
{
	partial class RestoreFormNew
	{
		private System.ComponentModel.IContainer components = null;

		private TableLayoutPanel mainLayout;
		private TableLayoutPanel bodyLayout;
		private TableLayoutPanel leftLayout;
		private TableLayoutPanel targetLayout;
		private Label lblTitle;

		private GroupBox grpSelectBackup;
		private GroupBox grpRestoreOptions;
		private GroupBox grpRestoreTarget;

		private TextBox txtBackupSource;
		private Button btnBrowseBackup;
		private Button btnScanBackup;
		private Panel pnlBackupInfo;
		private Label txtBackupFileCount;
		private Label txtBackupTotalSize;
		private Label txtBackupRestorePointCount;
		private Label txtRestorePointPrompt;
		private Label txtPreselectedRestorePointSummary;
		private ListBox lstRestorePoints;

		private Panel optionsScroll;
		private Panel optionsContent;
		private Label txtWhatToRestoreLabel;
		private Label txtPreselectedScopeSummary;
		private RadioButton rbRestoreAll;
		private RadioButton rbRestoreSelected;
		private Panel pnlItemSelection;
		private ListBox lstBackupItems;
		private Label txtDestinationHelp;

		private Panel pnlHyperVRestoreMode;
		private ComboBox cmbHyperVRestoreTarget;

		private Panel pnlHyperVVmOptions;
		private RadioButton rbHyperVReplaceExisting;
		private RadioButton rbHyperVRestoreToDirectory;
		private Panel pnlHyperVReplaceExistingOptions;
		private ComboBox cmbHyperVVmToReplace;
		private Panel pnlHyperVDirectoryOptions;
		private TextBox txtHyperVRestoreDirectory;
		private Button btnBrowseHyperVRestoreDirectory;
		private TextBox txtHyperVVmName;
		private CheckBox chkStartHyperVVm;

		private CheckBox chkRestoreToHyperVDisk;
		private Panel pnlRegularHyperVRestore;
		private TextBox txtHyperVVirtualDiskPath;
		private Button btnBrowseHyperVVirtualDisk;
		private RadioButton rbLeaveHyperVDiskDetached;
		private RadioButton chkAttachToExistingHyperVVm;
		private RadioButton rbCreateNewHyperVVm;
		private Panel pnlExistingHyperVVmOptions;
		private ComboBox cmbExistingHyperVVm;
		private Panel pnlNewHyperVVmOptions;
		private TextBox txtNewHyperVVmName;
		private TextBox txtNewHyperVVmPath;
		private Button btnBrowseNewHyperVVmLocation;
		private ComboBox cmbNewHyperVGeneration;
		private CheckBox chkStartCreatedHyperVVm;

		private Panel pnlLocationChoice;
		private Button btnBrowseRestoreDestination;
		private TextBox txtFolderRestoreDestination;

		private Panel pnlHyperVCloneDestination;
		private RadioButton rbHyperVCloneDefault;
		private RadioButton rbHyperVCloneAlternate;
		private Panel pnlHyperVCloneAlternate;
		private TextBox txtHyperVCloneVmFolder;
		private TextBox txtHyperVCloneDiskFolder;
		private Button btnBrowseHyperVCloneVmFolder;
		private Button btnBrowseHyperVCloneDiskFolder;

		private CheckBox chkOverwrite;
		private CheckBox chkPreservePermissions;
		private CheckBox chkVerifyAfterRestore;

		private Label txtDriveTreeHelp;
		private Panel targetTreeHost;
		private TreeView treeViewRestoreTarget;
		private Panel loadingTargetOverlay;
		private Label lblLoadingTargets;
		private ProgressBar progressLoadingTargets;
		private FlowLayoutPanel targetButtons;
		private Button btnRefreshTarget;
		private Button btnExpandTarget;
		private Button btnCollapseTarget;
		private CheckBox chkShowHiddenPartitionsTarget;
		private Label txtSelectedTargetLabel;

		private Panel pnlDriveTargetTree;
		private FlowLayoutPanel actionButtons;
		private Button btnRestore;
		private Button btnCancelRestore;

		// Labels that remain designer-editable.
		private Label lblBackupSource;
		private Label lblDestinationHeading;
		private Label lblHyperVTarget;
		private Label lblHyperVMode;
		private Label lblVmReplace;
		private Label lblVmDirectory;
		private Label lblVmName;
		private Label lblVirtualDisk;
		private Label lblAfterRestore;
		private Label lblExistingVm;
		private Label lblNewVmName;
		private Label lblNewVmPath;
		private Label lblGeneration;
		private Label lblFolderTarget;
		private Label lblCloneHeading;
		private Label lblCloneVmFolder;
		private Label lblCloneDiskFolder;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				components?.Dispose();

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();

			mainLayout = new TableLayoutPanel();
			bodyLayout = new TableLayoutPanel();
			leftLayout = new TableLayoutPanel();
			targetLayout = new TableLayoutPanel();
			lblTitle = new Label();

			grpSelectBackup = new GroupBox();
			grpRestoreOptions = new GroupBox();
			grpRestoreTarget = new GroupBox();

			txtBackupSource = new TextBox();
			btnBrowseBackup = new Button();
			btnScanBackup = new Button();
			pnlBackupInfo = new Panel();
			txtBackupFileCount = new Label();
			txtBackupTotalSize = new Label();
			txtBackupRestorePointCount = new Label();
			txtRestorePointPrompt = new Label();
			txtPreselectedRestorePointSummary = new Label();
			lstRestorePoints = new ListBox();

			optionsScroll = new Panel();
			optionsContent = new Panel();
			txtWhatToRestoreLabel = new Label();
			txtPreselectedScopeSummary = new Label();
			rbRestoreAll = new RadioButton();
			rbRestoreSelected = new RadioButton();
			pnlItemSelection = new Panel();
			lstBackupItems = new ListBox();
			txtDestinationHelp = new Label();

			pnlHyperVRestoreMode = new Panel();
			cmbHyperVRestoreTarget = new ComboBox();

			pnlHyperVVmOptions = new Panel();
			rbHyperVReplaceExisting = new RadioButton();
			rbHyperVRestoreToDirectory = new RadioButton();
			pnlHyperVReplaceExistingOptions = new Panel();
			cmbHyperVVmToReplace = new ComboBox();
			pnlHyperVDirectoryOptions = new Panel();
			txtHyperVRestoreDirectory = new TextBox();
			btnBrowseHyperVRestoreDirectory = new Button();
			txtHyperVVmName = new TextBox();
			chkStartHyperVVm = new CheckBox();

			chkRestoreToHyperVDisk = new CheckBox();
			pnlRegularHyperVRestore = new Panel();
			txtHyperVVirtualDiskPath = new TextBox();
			btnBrowseHyperVVirtualDisk = new Button();
			rbLeaveHyperVDiskDetached = new RadioButton();
			chkAttachToExistingHyperVVm = new RadioButton();
			rbCreateNewHyperVVm = new RadioButton();
			pnlExistingHyperVVmOptions = new Panel();
			cmbExistingHyperVVm = new ComboBox();
			pnlNewHyperVVmOptions = new Panel();
			txtNewHyperVVmName = new TextBox();
			txtNewHyperVVmPath = new TextBox();
			btnBrowseNewHyperVVmLocation = new Button();
			cmbNewHyperVGeneration = new ComboBox();
			chkStartCreatedHyperVVm = new CheckBox();

			pnlLocationChoice = new Panel();
			btnBrowseRestoreDestination = new Button();
			txtFolderRestoreDestination = new TextBox();

			pnlHyperVCloneDestination = new Panel();
			rbHyperVCloneDefault = new RadioButton();
			rbHyperVCloneAlternate = new RadioButton();
			pnlHyperVCloneAlternate = new Panel();
			txtHyperVCloneVmFolder = new TextBox();
			txtHyperVCloneDiskFolder = new TextBox();
			btnBrowseHyperVCloneVmFolder = new Button();
			btnBrowseHyperVCloneDiskFolder = new Button();

			chkOverwrite = new CheckBox();
			chkPreservePermissions = new CheckBox();
			chkVerifyAfterRestore = new CheckBox();

			txtDriveTreeHelp = new Label();
			targetTreeHost = new Panel();
			treeViewRestoreTarget = new TreeView();
			loadingTargetOverlay = new Panel();
			lblLoadingTargets = new Label();
			progressLoadingTargets = new ProgressBar();
			targetButtons = new FlowLayoutPanel();
			btnRefreshTarget = new Button();
			btnExpandTarget = new Button();
			btnCollapseTarget = new Button();
			chkShowHiddenPartitionsTarget = new CheckBox();
			txtSelectedTargetLabel = new Label();

			pnlDriveTargetTree = new Panel();
			actionButtons = new FlowLayoutPanel();
			btnRestore = new Button();
			btnCancelRestore = new Button();

			lblBackupSource = new Label();
			lblDestinationHeading = new Label();
			lblHyperVTarget = new Label();
			lblHyperVMode = new Label();
			lblVmReplace = new Label();
			lblVmDirectory = new Label();
			lblVmName = new Label();
			lblVirtualDisk = new Label();
			lblAfterRestore = new Label();
			lblExistingVm = new Label();
			lblNewVmName = new Label();
			lblNewVmPath = new Label();
			lblGeneration = new Label();
			lblFolderTarget = new Label();
			lblCloneHeading = new Label();
			lblCloneVmFolder = new Label();
			lblCloneDiskFolder = new Label();

			SuspendLayout();

			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1100, 720);
			MinimumSize = new Size(850, 600);
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore Backup";

			mainLayout.Dock = DockStyle.Fill;
			mainLayout.Padding = new Padding(10);
			mainLayout.ColumnCount = 1;
			mainLayout.RowCount = 3;
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));

			lblTitle.Dock = DockStyle.Fill;
			lblTitle.Text = "Restore from Backup";
			lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
			lblTitle.TextAlign = ContentAlignment.MiddleLeft;
			mainLayout.Controls.Add(lblTitle, 0, 0);

			bodyLayout.Dock = DockStyle.Fill;
			bodyLayout.ColumnCount = 3;
			bodyLayout.RowCount = 1;
			bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
			bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10));
			bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
			bodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			mainLayout.Controls.Add(bodyLayout, 0, 1);

			leftLayout.Dock = DockStyle.Fill;
			leftLayout.ColumnCount = 1;
			leftLayout.RowCount = 2;
			leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 255));
			leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			bodyLayout.Controls.Add(leftLayout, 0, 0);

			// Select Backup
			grpSelectBackup.Dock = DockStyle.Fill;
			grpSelectBackup.Text = "Select Backup";
			grpSelectBackup.Padding = new Padding(10);
			grpSelectBackup.Margin = new Padding(0, 0, 0, 8);
			leftLayout.Controls.Add(grpSelectBackup, 0, 0);

			lblBackupSource.Text = "Backup Source:";
			lblBackupSource.Location = new Point(15, 24);
			lblBackupSource.Size = new Size(200, 20);

			txtBackupSource.Name = "txtBackupSource";
			txtBackupSource.Location = new Point(15, 47);
			txtBackupSource.Size = new Size(390, 23);
			txtBackupSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtBackupSource.ReadOnly = true;

			btnBrowseBackup.Name = "btnBrowseBackup";
			btnBrowseBackup.Text = "Browse...";
			btnBrowseBackup.Location = new Point(410, 46);
			btnBrowseBackup.Size = new Size(80, 25);
			btnBrowseBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnBrowseBackup.Click += BrowseBackup_Click;

			btnScanBackup.Name = "btnScanBackup";
			btnScanBackup.Text = "Scan Backup";
			btnScanBackup.Location = new Point(15, 80);
			btnScanBackup.Size = new Size(120, 28);
			btnScanBackup.Click += ScanBackup_Click;

			pnlBackupInfo.Name = "pnlBackupInfo";
			pnlBackupInfo.Location = new Point(15, 115);
			pnlBackupInfo.Size = new Size(475, 125);
			pnlBackupInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			pnlBackupInfo.Visible = false;

			txtBackupFileCount.Name = "txtBackupFileCount";
			txtBackupFileCount.Location = new Point(0, 0);
			txtBackupFileCount.Size = new Size(160, 20);
			txtBackupFileCount.ForeColor = SystemColors.GrayText;

			txtBackupTotalSize.Name = "txtBackupTotalSize";
			txtBackupTotalSize.Location = new Point(160, 0);
			txtBackupTotalSize.Size = new Size(145, 20);
			txtBackupTotalSize.ForeColor = SystemColors.GrayText;

			txtBackupRestorePointCount.Name = "txtBackupRestorePointCount";
			txtBackupRestorePointCount.Location = new Point(305, 0);
			txtBackupRestorePointCount.Size = new Size(170, 20);
			txtBackupRestorePointCount.ForeColor = SystemColors.GrayText;

			txtRestorePointPrompt.Name = "txtRestorePointPrompt";
			txtRestorePointPrompt.Text = "Select Restore Point:";
			txtRestorePointPrompt.Font = new Font(Font, FontStyle.Bold);
			txtRestorePointPrompt.Location = new Point(0, 23);
			txtRestorePointPrompt.Size = new Size(220, 20);

			txtPreselectedRestorePointSummary.Name = "txtPreselectedRestorePointSummary";
			txtPreselectedRestorePointSummary.Font = new Font(Font, FontStyle.Bold);
			txtPreselectedRestorePointSummary.Location = new Point(0, 23);
			txtPreselectedRestorePointSummary.Size = new Size(460, 36);
			txtPreselectedRestorePointSummary.Visible = false;

			lstRestorePoints.Name = "lstRestorePoints";
			lstRestorePoints.Location = new Point(0, 46);
			lstRestorePoints.Size = new Size(475, 70);
			lstRestorePoints.Anchor =
				AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			lstRestorePoints.DisplayMember = "DisplayName";
			lstRestorePoints.SelectedIndexChanged += RestorePoints_SelectionChanged;

			pnlBackupInfo.Controls.Add(txtBackupFileCount);
			pnlBackupInfo.Controls.Add(txtBackupTotalSize);
			pnlBackupInfo.Controls.Add(txtBackupRestorePointCount);
			pnlBackupInfo.Controls.Add(txtRestorePointPrompt);
			pnlBackupInfo.Controls.Add(txtPreselectedRestorePointSummary);
			pnlBackupInfo.Controls.Add(lstRestorePoints);

			grpSelectBackup.Controls.Add(lblBackupSource);
			grpSelectBackup.Controls.Add(txtBackupSource);
			grpSelectBackup.Controls.Add(btnBrowseBackup);
			grpSelectBackup.Controls.Add(btnScanBackup);
			grpSelectBackup.Controls.Add(pnlBackupInfo);

			// Restore Options: scrollable content, with individual named panels
			grpRestoreOptions.Name = "grpRestoreOptions";
			grpRestoreOptions.Text = "Restore Options";
			grpRestoreOptions.Dock = DockStyle.Fill;
			grpRestoreOptions.Enabled = false;
			leftLayout.Controls.Add(grpRestoreOptions, 0, 1);

			optionsScroll.Dock = DockStyle.Fill;
			optionsScroll.AutoScroll = true;
			grpRestoreOptions.Controls.Add(optionsScroll);

			optionsContent.Location = new Point(0, 0);
			optionsContent.Size = new Size(475, 1390);
			optionsContent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			optionsScroll.Controls.Add(optionsContent);

			txtWhatToRestoreLabel.Name = "txtWhatToRestoreLabel";
			txtWhatToRestoreLabel.Text = "What to Restore:";
			txtWhatToRestoreLabel.Font = new Font(Font, FontStyle.Bold);
			txtWhatToRestoreLabel.Location = new Point(12, 12);
			txtWhatToRestoreLabel.Size = new Size(220, 20);

			txtPreselectedScopeSummary.Name = "txtPreselectedScopeSummary";
			txtPreselectedScopeSummary.Location = new Point(12, 12);
			txtPreselectedScopeSummary.Size = new Size(445, 38);
			txtPreselectedScopeSummary.ForeColor = SystemColors.GrayText;
			txtPreselectedScopeSummary.Visible = false;

			rbRestoreAll.Name = "rbRestoreAll";
			rbRestoreAll.Text = "Restore everything from backup";
			rbRestoreAll.Location = new Point(12, 38);
			rbRestoreAll.Size = new Size(360, 22);
			rbRestoreAll.Checked = true;
			rbRestoreAll.CheckedChanged += RestoreScope_Changed;

			rbRestoreSelected.Name = "rbRestoreSelected";
			rbRestoreSelected.Text = "Select specific items to restore";
			rbRestoreSelected.Location = new Point(12, 65);
			rbRestoreSelected.Size = new Size(360, 22);
			rbRestoreSelected.CheckedChanged += RestoreScope_Changed;

			pnlItemSelection.Name = "pnlItemSelection";
			pnlItemSelection.Location = new Point(12, 90);
			pnlItemSelection.Size = new Size(445, 125);
			pnlItemSelection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			pnlItemSelection.Visible = false;

			lstBackupItems.Name = "lstBackupItems";
			lstBackupItems.Dock = DockStyle.Fill;
			lstBackupItems.SelectionMode = SelectionMode.MultiExtended;
			lstBackupItems.SelectedIndexChanged += RestoreItems_SelectionChanged;
			pnlItemSelection.Controls.Add(lstBackupItems);

			lblDestinationHeading.Text = "Restore Destination:";
			lblDestinationHeading.Font = new Font(Font, FontStyle.Bold);
			lblDestinationHeading.Location = new Point(12, 220);
			lblDestinationHeading.Size = new Size(260, 20);

			txtDestinationHelp.Name = "txtDestinationHelp";
			txtDestinationHelp.Text =
				"Select a target on the right to specify where the restore should be written.";
			txtDestinationHelp.ForeColor = SystemColors.GrayText;
			txtDestinationHelp.Location = new Point(12, 244);
			txtDestinationHelp.Size = new Size(445, 45);
			txtDestinationHelp.Anchor =
				AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

			pnlHyperVRestoreMode.Name = "pnlHyperVRestoreMode";
			pnlHyperVRestoreMode.Location = new Point(12, 295);
			pnlHyperVRestoreMode.Size = new Size(445, 66);
			pnlHyperVRestoreMode.Visible = false;

			lblHyperVTarget.Text = "Hyper-V Restore Target:";
			lblHyperVTarget.Font = new Font(Font, FontStyle.Bold);
			lblHyperVTarget.Location = new Point(0, 0);
			lblHyperVTarget.Size = new Size(260, 20);
			cmbHyperVRestoreTarget.Name = "cmbHyperVRestoreTarget";
			cmbHyperVRestoreTarget.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbHyperVRestoreTarget.Location = new Point(0, 28);
			cmbHyperVRestoreTarget.Size = new Size(320, 23);
			cmbHyperVRestoreTarget.Items.AddRange(new object[]
			{
				"Restore exported files/folders",
				"Restore guest disk to volume",
				"Restore guest disk to disk",
				"Restore as Hyper-V virtual machine"
			});
			cmbHyperVRestoreTarget.SelectedIndexChanged +=
				HyperVRestoreTarget_Changed;
			pnlHyperVRestoreMode.Controls.Add(lblHyperVTarget);
			pnlHyperVRestoreMode.Controls.Add(cmbHyperVRestoreTarget);

			pnlHyperVVmOptions.Name = "pnlHyperVVmOptions";
			pnlHyperVVmOptions.Location = new Point(12, 370);
			pnlHyperVVmOptions.Size = new Size(445, 260);
			pnlHyperVVmOptions.Visible = false;

			lblHyperVMode.Text = "Restore Mode:";
			lblHyperVMode.Font = new Font(Font, FontStyle.Bold);
			lblHyperVMode.Location = new Point(0, 0);
			lblHyperVMode.Size = new Size(200, 20);

			rbHyperVReplaceExisting.Name = "rbHyperVReplaceExisting";
			rbHyperVReplaceExisting.Text = "Replace a non-running Hyper-V virtual machine";
			rbHyperVReplaceExisting.Location = new Point(0, 26);
			rbHyperVReplaceExisting.Size = new Size(430, 22);
			rbHyperVReplaceExisting.Checked = true;
			rbHyperVReplaceExisting.CheckedChanged += HyperVVmRestoreMode_Changed;

			rbHyperVRestoreToDirectory.Name = "rbHyperVRestoreToDirectory";
			rbHyperVRestoreToDirectory.Text = "Restore to an empty directory";
			rbHyperVRestoreToDirectory.Location = new Point(0, 51);
			rbHyperVRestoreToDirectory.Size = new Size(400, 22);
			rbHyperVRestoreToDirectory.CheckedChanged += HyperVVmRestoreMode_Changed;

			pnlHyperVReplaceExistingOptions.Name =
				"pnlHyperVReplaceExistingOptions";
			pnlHyperVReplaceExistingOptions.Location = new Point(20, 80);
			pnlHyperVReplaceExistingOptions.Size = new Size(420, 48);
			lblVmReplace.Text = "Non-running virtual machine to replace:";
			lblVmReplace.Location = new Point(0, 0);
			lblVmReplace.Size = new Size(350, 20);
			cmbHyperVVmToReplace.Name = "cmbHyperVVmToReplace";
			cmbHyperVVmToReplace.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbHyperVVmToReplace.Location = new Point(0, 22);
			cmbHyperVVmToReplace.Size = new Size(320, 23);
			pnlHyperVReplaceExistingOptions.Controls.Add(lblVmReplace);
			pnlHyperVReplaceExistingOptions.Controls.Add(cmbHyperVVmToReplace);

			pnlHyperVDirectoryOptions.Name = "pnlHyperVDirectoryOptions";
			pnlHyperVDirectoryOptions.Location = new Point(20, 80);
			pnlHyperVDirectoryOptions.Size = new Size(420, 48);
			pnlHyperVDirectoryOptions.Visible = false;
			lblVmDirectory.Text = "Restore destination directory (must be empty):";
			lblVmDirectory.Location = new Point(0, 0);
			lblVmDirectory.Size = new Size(400, 20);
			txtHyperVRestoreDirectory.Name = "txtHyperVRestoreDirectory";
			txtHyperVRestoreDirectory.Location = new Point(0, 22);
			txtHyperVRestoreDirectory.Size = new Size(325, 23);
			btnBrowseHyperVRestoreDirectory.Text = "Browse...";
			btnBrowseHyperVRestoreDirectory.Location = new Point(330, 21);
			btnBrowseHyperVRestoreDirectory.Size = new Size(80, 25);
			btnBrowseHyperVRestoreDirectory.Click +=
				BrowseHyperVRestoreDirectory_Click;
			pnlHyperVDirectoryOptions.Controls.Add(lblVmDirectory);
			pnlHyperVDirectoryOptions.Controls.Add(txtHyperVRestoreDirectory);
			pnlHyperVDirectoryOptions.Controls.Add(btnBrowseHyperVRestoreDirectory);

			lblVmName.Text = "Virtual machine name after restore:";
			lblVmName.Location = new Point(0, 141);
			lblVmName.Size = new Size(350, 20);
			txtHyperVVmName.Name = "txtHyperVVmName";
			txtHyperVVmName.Location = new Point(0, 164);
			txtHyperVVmName.Size = new Size(405, 23);
			chkStartHyperVVm.Name = "chkStartHyperVVm";
			chkStartHyperVVm.Text = "Start the virtual machine after restore";
			chkStartHyperVVm.Location = new Point(0, 196);
			chkStartHyperVVm.Size = new Size(400, 22);

			pnlHyperVVmOptions.Controls.Add(lblHyperVMode);
			pnlHyperVVmOptions.Controls.Add(rbHyperVReplaceExisting);
			pnlHyperVVmOptions.Controls.Add(rbHyperVRestoreToDirectory);
			pnlHyperVVmOptions.Controls.Add(pnlHyperVReplaceExistingOptions);
			pnlHyperVVmOptions.Controls.Add(pnlHyperVDirectoryOptions);
			pnlHyperVVmOptions.Controls.Add(lblVmName);
			pnlHyperVVmOptions.Controls.Add(txtHyperVVmName);
			pnlHyperVVmOptions.Controls.Add(chkStartHyperVVm);

			chkRestoreToHyperVDisk.Name = "chkRestoreToHyperVDisk";
			chkRestoreToHyperVDisk.Text =
				"Restore into a Hyper-V virtual disk (.vhdx)";
			chkRestoreToHyperVDisk.Location = new Point(12, 640);
			chkRestoreToHyperVDisk.Size = new Size(430, 25);
			chkRestoreToHyperVDisk.Visible = false;
			chkRestoreToHyperVDisk.CheckedChanged +=
				RegularHyperVRestoreOption_Changed;

			pnlRegularHyperVRestore.Name = "pnlRegularHyperVRestore";
			pnlRegularHyperVRestore.Location = new Point(12, 670);
			pnlRegularHyperVRestore.Size = new Size(445, 310);
			pnlRegularHyperVRestore.Visible = false;

			lblVirtualDisk.Text = "Hyper-V virtual disk file:";
			lblVirtualDisk.Location = new Point(0, 0);
			lblVirtualDisk.Size = new Size(300, 20);
			txtHyperVVirtualDiskPath.Name = "txtHyperVVirtualDiskPath";
			txtHyperVVirtualDiskPath.Location = new Point(0, 23);
			txtHyperVVirtualDiskPath.Size = new Size(350, 23);
			btnBrowseHyperVVirtualDisk.Text = "Browse...";
			btnBrowseHyperVVirtualDisk.Location = new Point(355, 22);
			btnBrowseHyperVVirtualDisk.Size = new Size(80, 25);
			btnBrowseHyperVVirtualDisk.Click += BrowseHyperVVirtualDisk_Click;

			lblAfterRestore.Text = "After restore:";
			lblAfterRestore.Font = new Font(Font, FontStyle.Bold);
			lblAfterRestore.Location = new Point(0, 55);
			lblAfterRestore.Size = new Size(200, 20);

			rbLeaveHyperVDiskDetached.Name = "rbLeaveHyperVDiskDetached";
			rbLeaveHyperVDiskDetached.Text = "Leave restored virtual disk detached";
			rbLeaveHyperVDiskDetached.Location = new Point(0, 79);
			rbLeaveHyperVDiskDetached.Size = new Size(430, 22);
			rbLeaveHyperVDiskDetached.Checked = true;
			rbLeaveHyperVDiskDetached.CheckedChanged +=
				ExistingHyperVVmAttach_Changed;

			// Kept under its original name for code migration; it is a RadioButton.
			chkAttachToExistingHyperVVm.Name = "chkAttachToExistingHyperVVm";
			chkAttachToExistingHyperVVm.Text =
				"Attach to an existing Hyper-V virtual machine";
			chkAttachToExistingHyperVVm.Location = new Point(0, 104);
			chkAttachToExistingHyperVVm.Size = new Size(430, 22);
			chkAttachToExistingHyperVVm.CheckedChanged +=
				ExistingHyperVVmAttach_Changed;

			rbCreateNewHyperVVm.Name = "rbCreateNewHyperVVm";
			rbCreateNewHyperVVm.Text =
				"Create a new Hyper-V virtual machine";
			rbCreateNewHyperVVm.Location = new Point(0, 129);
			rbCreateNewHyperVVm.Size = new Size(430, 22);
			rbCreateNewHyperVVm.CheckedChanged +=
				ExistingHyperVVmAttach_Changed;

			pnlExistingHyperVVmOptions.Name = "pnlExistingHyperVVmOptions";
			pnlExistingHyperVVmOptions.Location = new Point(20, 160);
			pnlExistingHyperVVmOptions.Size = new Size(410, 52);
			pnlExistingHyperVVmOptions.Visible = false;
			lblExistingVm.Text = "Existing Hyper-V virtual machine:";
			lblExistingVm.Location = new Point(0, 0);
			lblExistingVm.Size = new Size(350, 20);
			cmbExistingHyperVVm.Name = "cmbExistingHyperVVm";
			cmbExistingHyperVVm.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbExistingHyperVVm.Location = new Point(0, 22);
			cmbExistingHyperVVm.Size = new Size(320, 23);
			pnlExistingHyperVVmOptions.Controls.Add(lblExistingVm);
			pnlExistingHyperVVmOptions.Controls.Add(cmbExistingHyperVVm);

			pnlNewHyperVVmOptions.Name = "pnlNewHyperVVmOptions";
			pnlNewHyperVVmOptions.Location = new Point(20, 160);
			pnlNewHyperVVmOptions.Size = new Size(415, 150);
			pnlNewHyperVVmOptions.Visible = false;

			lblNewVmName.Text = "New virtual machine name:";
			lblNewVmName.Location = new Point(0, 0);
			lblNewVmName.Size = new Size(300, 20);
			txtNewHyperVVmName.Name = "txtNewHyperVVmName";
			txtNewHyperVVmName.Location = new Point(0, 20);
			txtNewHyperVVmName.Size = new Size(400, 23);

			lblNewVmPath.Text = "Virtual machine storage folder:";
			lblNewVmPath.Location = new Point(0, 48);
			lblNewVmPath.Size = new Size(300, 20);
			txtNewHyperVVmPath.Name = "txtNewHyperVVmPath";
			txtNewHyperVVmPath.Location = new Point(0, 68);
			txtNewHyperVVmPath.Size = new Size(315, 23);
			btnBrowseNewHyperVVmLocation.Text = "Browse...";
			btnBrowseNewHyperVVmLocation.Location = new Point(320, 67);
			btnBrowseNewHyperVVmLocation.Size = new Size(80, 25);
			btnBrowseNewHyperVVmLocation.Click +=
				BrowseNewHyperVVmLocation_Click;

			lblGeneration.Text = "Generation:";
			lblGeneration.Location = new Point(0, 98);
			lblGeneration.Size = new Size(85, 22);
			cmbNewHyperVGeneration.Name = "cmbNewHyperVGeneration";
			cmbNewHyperVGeneration.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbNewHyperVGeneration.Location = new Point(90, 96);
			cmbNewHyperVGeneration.Size = new Size(135, 23);
			cmbNewHyperVGeneration.Items.AddRange(
				new object[] { "Generation 1", "Generation 2" });
			chkStartCreatedHyperVVm.Name = "chkStartCreatedHyperVVm";
			chkStartCreatedHyperVVm.Text = "Start new VM after restore";
			chkStartCreatedHyperVVm.Location = new Point(0, 123);
			chkStartCreatedHyperVVm.Size = new Size(300, 22);
			chkStartCreatedHyperVVm.Checked = true;

			pnlNewHyperVVmOptions.Controls.Add(lblNewVmName);
			pnlNewHyperVVmOptions.Controls.Add(txtNewHyperVVmName);
			pnlNewHyperVVmOptions.Controls.Add(lblNewVmPath);
			pnlNewHyperVVmOptions.Controls.Add(txtNewHyperVVmPath);
			pnlNewHyperVVmOptions.Controls.Add(btnBrowseNewHyperVVmLocation);
			pnlNewHyperVVmOptions.Controls.Add(lblGeneration);
			pnlNewHyperVVmOptions.Controls.Add(cmbNewHyperVGeneration);
			pnlNewHyperVVmOptions.Controls.Add(chkStartCreatedHyperVVm);

			pnlRegularHyperVRestore.Controls.Add(lblVirtualDisk);
			pnlRegularHyperVRestore.Controls.Add(txtHyperVVirtualDiskPath);
			pnlRegularHyperVRestore.Controls.Add(btnBrowseHyperVVirtualDisk);
			pnlRegularHyperVRestore.Controls.Add(lblAfterRestore);
			pnlRegularHyperVRestore.Controls.Add(rbLeaveHyperVDiskDetached);
			pnlRegularHyperVRestore.Controls.Add(chkAttachToExistingHyperVVm);
			pnlRegularHyperVRestore.Controls.Add(rbCreateNewHyperVVm);
			pnlRegularHyperVRestore.Controls.Add(pnlExistingHyperVVmOptions);
			pnlRegularHyperVRestore.Controls.Add(pnlNewHyperVVmOptions);

			pnlLocationChoice.Name = "pnlLocationChoice";
			pnlLocationChoice.Location = new Point(12, 990);
			pnlLocationChoice.Size = new Size(445, 90);
			pnlLocationChoice.Visible = false;
			lblFolderTarget.Text = "File/Folder Restore Target";
			lblFolderTarget.Font = new Font(Font, FontStyle.Bold);
			lblFolderTarget.Location = new Point(0, 0);
			lblFolderTarget.Size = new Size(300, 20);
			btnBrowseRestoreDestination.Name = "btnBrowseRestoreDestination";
			btnBrowseRestoreDestination.Text = "Browse...";
			btnBrowseRestoreDestination.Location = new Point(0, 25);
			btnBrowseRestoreDestination.Size = new Size(120, 26);
			btnBrowseRestoreDestination.Click +=
				BrowseRestoreDestination_Click;
			txtFolderRestoreDestination.Name = "txtFolderRestoreDestination";
			txtFolderRestoreDestination.Location = new Point(0, 57);
			txtFolderRestoreDestination.Size = new Size(435, 23);
			txtFolderRestoreDestination.TextChanged +=
				TargetLocation_TextChanged;
			pnlLocationChoice.Controls.Add(lblFolderTarget);
			pnlLocationChoice.Controls.Add(btnBrowseRestoreDestination);
			pnlLocationChoice.Controls.Add(txtFolderRestoreDestination);

			pnlHyperVCloneDestination.Name = "pnlHyperVCloneDestination";
			pnlHyperVCloneDestination.Location = new Point(12, 1090);
			pnlHyperVCloneDestination.Size = new Size(445, 170);
			pnlHyperVCloneDestination.Visible = false;
			lblCloneHeading.Text = "Restore Destination:";
			lblCloneHeading.Font = new Font(Font, FontStyle.Bold);
			lblCloneHeading.Location = new Point(0, 0);
			lblCloneHeading.Size = new Size(300, 20);
			rbHyperVCloneDefault.Name = "rbHyperVCloneDefault";
			rbHyperVCloneDefault.Text = "Use Hyper-V default storage location";
			rbHyperVCloneDefault.Location = new Point(0, 24);
			rbHyperVCloneDefault.Size = new Size(400, 22);
			rbHyperVCloneDefault.Checked = true;
			rbHyperVCloneDefault.CheckedChanged +=
				HyperVCloneDestination_Changed;
			rbHyperVCloneAlternate.Name = "rbHyperVCloneAlternate";
			rbHyperVCloneAlternate.Text = "Alternate location";
			rbHyperVCloneAlternate.Location = new Point(0, 49);
			rbHyperVCloneAlternate.Size = new Size(400, 22);
			rbHyperVCloneAlternate.CheckedChanged +=
				HyperVCloneDestination_Changed;

			pnlHyperVCloneAlternate.Name = "pnlHyperVCloneAlternate";
			pnlHyperVCloneAlternate.Location = new Point(12, 75);
			pnlHyperVCloneAlternate.Size = new Size(425, 90);
			pnlHyperVCloneAlternate.Visible = false;
			lblCloneVmFolder.Text = "Virtual machine config folder:";
			lblCloneVmFolder.Location = new Point(0, 0);
			lblCloneVmFolder.Size = new Size(300, 20);
			txtHyperVCloneVmFolder.Name = "txtHyperVCloneVmFolder";
			txtHyperVCloneVmFolder.Location = new Point(0, 20);
			txtHyperVCloneVmFolder.Size = new Size(335, 23);
			btnBrowseHyperVCloneVmFolder.Text = "Browse...";
			btnBrowseHyperVCloneVmFolder.Location = new Point(340, 19);
			btnBrowseHyperVCloneVmFolder.Size = new Size(80, 25);
			btnBrowseHyperVCloneVmFolder.Click +=
				BrowseHyperVCloneVmFolder_Click;
			lblCloneDiskFolder.Text = "Virtual disk data folder:";
			lblCloneDiskFolder.Location = new Point(0, 47);
			lblCloneDiskFolder.Size = new Size(300, 20);
			txtHyperVCloneDiskFolder.Name = "txtHyperVCloneDiskFolder";
			txtHyperVCloneDiskFolder.Location = new Point(0, 66);
			txtHyperVCloneDiskFolder.Size = new Size(335, 23);
			btnBrowseHyperVCloneDiskFolder.Text = "Browse...";
			btnBrowseHyperVCloneDiskFolder.Location = new Point(340, 65);
			btnBrowseHyperVCloneDiskFolder.Size = new Size(80, 25);
			btnBrowseHyperVCloneDiskFolder.Click +=
				BrowseHyperVCloneDiskFolder_Click;
			pnlHyperVCloneAlternate.Controls.Add(lblCloneVmFolder);
			pnlHyperVCloneAlternate.Controls.Add(txtHyperVCloneVmFolder);
			pnlHyperVCloneAlternate.Controls.Add(btnBrowseHyperVCloneVmFolder);
			pnlHyperVCloneAlternate.Controls.Add(lblCloneDiskFolder);
			pnlHyperVCloneAlternate.Controls.Add(txtHyperVCloneDiskFolder);
			pnlHyperVCloneAlternate.Controls.Add(btnBrowseHyperVCloneDiskFolder);
			pnlHyperVCloneDestination.Controls.Add(lblCloneHeading);
			pnlHyperVCloneDestination.Controls.Add(rbHyperVCloneDefault);
			pnlHyperVCloneDestination.Controls.Add(rbHyperVCloneAlternate);
			pnlHyperVCloneDestination.Controls.Add(pnlHyperVCloneAlternate);

			chkOverwrite.Name = "chkOverwrite";
			chkOverwrite.Text = "Overwrite existing files";
			chkOverwrite.Location = new Point(12, 1270);
			chkOverwrite.Size = new Size(400, 22);
			chkPreservePermissions.Name = "chkPreservePermissions";
			chkPreservePermissions.Text =
				"Preserve file permissions and attributes";
			chkPreservePermissions.Location = new Point(12, 1297);
			chkPreservePermissions.Size = new Size(420, 22);
			chkPreservePermissions.Checked = true;
			chkVerifyAfterRestore.Name = "chkVerifyAfterRestore";
			chkVerifyAfterRestore.Text = "Verify files after restore";
			chkVerifyAfterRestore.Location = new Point(12, 1324);
			chkVerifyAfterRestore.Size = new Size(400, 22);
			chkVerifyAfterRestore.Checked = true;

			optionsContent.Controls.Add(txtWhatToRestoreLabel);
			optionsContent.Controls.Add(txtPreselectedScopeSummary);
			optionsContent.Controls.Add(rbRestoreAll);
			optionsContent.Controls.Add(rbRestoreSelected);
			optionsContent.Controls.Add(pnlItemSelection);
			optionsContent.Controls.Add(lblDestinationHeading);
			optionsContent.Controls.Add(txtDestinationHelp);
			optionsContent.Controls.Add(pnlHyperVRestoreMode);
			optionsContent.Controls.Add(pnlHyperVVmOptions);
			optionsContent.Controls.Add(chkRestoreToHyperVDisk);
			optionsContent.Controls.Add(pnlRegularHyperVRestore);
			optionsContent.Controls.Add(pnlLocationChoice);
			optionsContent.Controls.Add(pnlHyperVCloneDestination);
			optionsContent.Controls.Add(chkOverwrite);
			optionsContent.Controls.Add(chkPreservePermissions);
			optionsContent.Controls.Add(chkVerifyAfterRestore);

			// Right-hand target tree
			grpRestoreTarget.Name = "grpRestoreTarget";
			grpRestoreTarget.Text = "Select Restore Target";
			grpRestoreTarget.Dock = DockStyle.Fill;
			bodyLayout.Controls.Add(grpRestoreTarget, 2, 0);

			targetLayout.Dock = DockStyle.Fill;
			targetLayout.Padding = new Padding(8);
			targetLayout.RowCount = 4;
			targetLayout.ColumnCount = 1;
			targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
			targetLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 39));
			targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
			grpRestoreTarget.Controls.Add(targetLayout);

			txtDriveTreeHelp.Name = "txtDriveTreeHelp";
			txtDriveTreeHelp.Dock = DockStyle.Fill;
			txtDriveTreeHelp.ForeColor = SystemColors.GrayText;
			txtDriveTreeHelp.Text =
				"Select a backup on the left to load available restore targets. " +
				"Choose a target disk or volume below. The boot/system disk " +
				"must not be selectable.";
			targetLayout.Controls.Add(txtDriveTreeHelp, 0, 0);

			targetTreeHost.Dock = DockStyle.Fill;
			treeViewRestoreTarget.Name = "treeViewRestoreTarget";
			treeViewRestoreTarget.Dock = DockStyle.Fill;
			treeViewRestoreTarget.HideSelection = false;
			treeViewRestoreTarget.AfterSelect +=
				TreeViewRestoreTarget_AfterSelect;

			loadingTargetOverlay.Name = "loadingTargetOverlay";
			loadingTargetOverlay.Dock = DockStyle.Fill;
			loadingTargetOverlay.BackColor = SystemColors.Control;
			loadingTargetOverlay.Visible = false;
			lblLoadingTargets.Text = "Loading drives...";
			lblLoadingTargets.TextAlign = ContentAlignment.MiddleCenter;
			lblLoadingTargets.Anchor = AnchorStyles.None;
			lblLoadingTargets.Location = new Point(130, 110);
			lblLoadingTargets.Size = new Size(180, 24);
			progressLoadingTargets.Style = ProgressBarStyle.Marquee;
			progressLoadingTargets.Location = new Point(140, 142);
			progressLoadingTargets.Size = new Size(160, 18);
			loadingTargetOverlay.Controls.Add(lblLoadingTargets);
			loadingTargetOverlay.Controls.Add(progressLoadingTargets);

			targetTreeHost.Controls.Add(treeViewRestoreTarget);
			targetTreeHost.Controls.Add(loadingTargetOverlay);
			targetLayout.Controls.Add(targetTreeHost, 0, 1);

			targetButtons.Dock = DockStyle.Fill;
			targetButtons.WrapContents = false;
			btnRefreshTarget.Text = "Refresh";
			btnRefreshTarget.Size = new Size(80, 27);
			btnRefreshTarget.Click += RefreshRestoreTarget_Click;
			btnExpandTarget.Text = "Expand All";
			btnExpandTarget.Size = new Size(85, 27);
			btnExpandTarget.Click += ExpandAllTarget_Click;
			btnCollapseTarget.Text = "Collapse All";
			btnCollapseTarget.Size = new Size(95, 27);
			btnCollapseTarget.Click += CollapseAllTarget_Click;
			chkShowHiddenPartitionsTarget.Name = "chkShowHiddenPartitionsTarget";
			chkShowHiddenPartitionsTarget.Text = "Show Hidden Partitions";
			chkShowHiddenPartitionsTarget.AutoSize = true;
			chkShowHiddenPartitionsTarget.Margin = new Padding(10, 6, 0, 0);
			chkShowHiddenPartitionsTarget.CheckedChanged +=
				ShowHiddenPartitionsTarget_Click;
			targetButtons.Controls.Add(btnRefreshTarget);
			targetButtons.Controls.Add(btnExpandTarget);
			targetButtons.Controls.Add(btnCollapseTarget);
			targetButtons.Controls.Add(chkShowHiddenPartitionsTarget);
			targetLayout.Controls.Add(targetButtons, 0, 2);

			txtSelectedTargetLabel.Name = "txtSelectedTargetLabel";
			txtSelectedTargetLabel.Text = "No target selected";
			txtSelectedTargetLabel.Font = new Font(Font, FontStyle.Italic);
			txtSelectedTargetLabel.ForeColor = SystemColors.GrayText;
			txtSelectedTargetLabel.Dock = DockStyle.Fill;
			targetLayout.Controls.Add(txtSelectedTargetLabel, 0, 3);

			// Compatibility placeholder, not part of the visible layout.
			pnlDriveTargetTree.Name = "pnlDriveTargetTree";
			pnlDriveTargetTree.Visible = false;
			grpRestoreTarget.Controls.Add(pnlDriveTargetTree);

			actionButtons.Dock = DockStyle.Fill;
			actionButtons.FlowDirection = FlowDirection.RightToLeft;
			actionButtons.WrapContents = false;
			btnCancelRestore.Text = "Cancel";
			btnCancelRestore.Size = new Size(100, 28);
			btnCancelRestore.Click += Cancel_Click;
			btnRestore.Name = "btnRestore";
			btnRestore.Text = "Next";
			btnRestore.Size = new Size(110, 28);
			btnRestore.Enabled = false;
			btnRestore.Click += StartRestore_Click;
			actionButtons.Controls.Add(btnCancelRestore);
			actionButtons.Controls.Add(btnRestore);
			mainLayout.Controls.Add(actionButtons, 0, 2);

			Controls.Add(mainLayout);
			ResumeLayout(false);
		}
	}
}