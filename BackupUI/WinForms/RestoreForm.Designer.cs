using System.Drawing;
using System.Windows.Forms;

using static System.Net.Mime.MediaTypeNames;

namespace SecureServerBackup.WinForm
{
	partial class RestoreForm
	{
		private System.ComponentModel.IContainer components = null;

		private TableLayoutPanel mainLayout;
		private Label lblTitle;
		private Panel wizardHost;
		private TableLayoutPanel pnlStep1;
		private TableLayoutPanel pnlStep2;
		private TableLayoutPanel pnlStep3;

		private TextBox txtBackupSource;
		private Label txtBackupInfo;
		private ListView lstBackupDates;
		private TreeView treeRestoreContents;

		private RadioButton rbOriginalLocation;
		private RadioButton rbNewLocation;
		private Panel pnlNewLocation;
		private TextBox txtRestoreDestination;

		private CheckBox chkOverwrite;
		private CheckBox chkPreservePermissions;
		private CheckBox chkRestoreSystemState;
		private CheckBox chkRestoreAsHyperV;
		private Panel pnlHyperV;
		private TextBox txtVMName;
		private TextBox txtVMStorage;
		private CheckBox chkStartVM;

		private ProgressBar progressBar;
		private Label txtProgress;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				components?.Dispose();

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			mainLayout = new TableLayoutPanel();
			lblTitle = new Label();
			wizardHost = new Panel();
			pnlStep3 = new TableLayoutPanel();
			optionsScroll = new Panel();
			optionsLayout = new TableLayoutPanel();
			locationGroup = new GroupBox();
			locationLayout = new TableLayoutPanel();
			rbOriginalLocation = new RadioButton();
			rbNewLocation = new RadioButton();
			pnlNewLocation = new Panel();
			newLocationLayout = new TableLayoutPanel();
			txtRestoreDestination = new TextBox();
			btnBrowseDestination = new Button();
			restoreOptionsGroup = new GroupBox();
			restoreOptionsLayout = new TableLayoutPanel();
			chkOverwrite = new CheckBox();
			chkPreservePermissions = new CheckBox();
			chkRestoreSystemState = new CheckBox();
			advancedGroup = new GroupBox();
			advancedLayout = new TableLayoutPanel();
			chkRestoreAsHyperV = new CheckBox();
			pnlHyperV = new Panel();
			hyperVLayout = new TableLayoutPanel();
			txtVMName = new TextBox();
			txtVMStorage = new TextBox();
			btnBrowseVMStorage = new Button();
			chkStartVM = new CheckBox();
			step3Buttons = new FlowLayoutPanel();
			btnStartRestore = new Button();
			btnStep3Back = new Button();
			pnlStep2 = new TableLayoutPanel();
			contentsGroup = new GroupBox();
			contentsLayout = new TableLayoutPanel();
			treeRestoreContents = new TreeView();
			treeButtons = new FlowLayoutPanel();
			btnExpand = new Button();
			btnCollapse = new Button();
			btnSelect = new Button();
			btnUnselect = new Button();
			step2Buttons = new FlowLayoutPanel();
			btnStep2Next = new Button();
			btnStep2Back = new Button();
			pnlStep1 = new TableLayoutPanel();
			step1Title = new Label();
			sourceGroup = new GroupBox();
			sourceLayout = new TableLayoutPanel();
			txtBackupSource = new TextBox();
			btnBrowseBackup = new Button();
			btnLoadBackup = new Button();
			datesGroup = new GroupBox();
			datesLayout = new TableLayoutPanel();
			txtBackupInfo = new Label();
			lstBackupDates = new ListView();
			step1Buttons = new FlowLayoutPanel();
			btnStep1Next = new Button();
			progressLayout = new TableLayoutPanel();
			progressBar = new ProgressBar();
			txtProgress = new Label();
			actionButtons = new FlowLayoutPanel();
			btnCancel = new Button();
			mainLayout.SuspendLayout();
			wizardHost.SuspendLayout();
			pnlStep3.SuspendLayout();
			optionsScroll.SuspendLayout();
			optionsLayout.SuspendLayout();
			locationGroup.SuspendLayout();
			locationLayout.SuspendLayout();
			pnlNewLocation.SuspendLayout();
			newLocationLayout.SuspendLayout();
			restoreOptionsGroup.SuspendLayout();
			restoreOptionsLayout.SuspendLayout();
			advancedGroup.SuspendLayout();
			advancedLayout.SuspendLayout();
			pnlHyperV.SuspendLayout();
			hyperVLayout.SuspendLayout();
			step3Buttons.SuspendLayout();
			pnlStep2.SuspendLayout();
			contentsGroup.SuspendLayout();
			contentsLayout.SuspendLayout();
			treeButtons.SuspendLayout();
			step2Buttons.SuspendLayout();
			pnlStep1.SuspendLayout();
			sourceGroup.SuspendLayout();
			sourceLayout.SuspendLayout();
			datesGroup.SuspendLayout();
			datesLayout.SuspendLayout();
			step1Buttons.SuspendLayout();
			progressLayout.SuspendLayout();
			actionButtons.SuspendLayout();
			SuspendLayout();
			// 
			// mainLayout
			// 
			mainLayout.ColumnCount = 1;
			mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			mainLayout.Controls.Add(lblTitle, 0, 0);
			mainLayout.Controls.Add(wizardHost, 0, 1);
			mainLayout.Controls.Add(progressLayout, 0, 2);
			mainLayout.Controls.Add(actionButtons, 0, 3);
			mainLayout.Dock = DockStyle.Fill;
			mainLayout.Location = new Point(0, 0);
			mainLayout.Name = "mainLayout";
			mainLayout.Padding = new Padding(10);
			mainLayout.RowCount = 4;
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
			mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
			mainLayout.Size = new Size(1000, 754);
			mainLayout.TabIndex = 0;
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Dock = DockStyle.Fill;
			lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.830189F, FontStyle.Bold);
			lblTitle.Location = new Point(13, 10);
			lblTitle.Name = "lblTitle";
			lblTitle.Padding = new Padding(0, 0, 0, 5);
			lblTitle.Size = new Size(974, 45);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "Restore from Backup";
			lblTitle.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// wizardHost
			// 
			wizardHost.Controls.Add(pnlStep3);
			wizardHost.Controls.Add(pnlStep2);
			wizardHost.Controls.Add(pnlStep1);
			wizardHost.Dock = DockStyle.Fill;
			wizardHost.Location = new Point(13, 58);
			wizardHost.Name = "wizardHost";
			wizardHost.Size = new Size(974, 593);
			wizardHost.TabIndex = 1;
			// 
			// pnlStep3
			// 
			pnlStep3.ColumnCount = 1;
			pnlStep3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			pnlStep3.Controls.Add(optionsScroll, 0, 1);
			pnlStep3.Controls.Add(step3Buttons, 0, 2);
			pnlStep3.Dock = DockStyle.Fill;
			pnlStep3.Location = new Point(0, 0);
			pnlStep3.Name = "pnlStep3";
			pnlStep3.RowCount = 3;
			pnlStep3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			pnlStep3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			pnlStep3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			pnlStep3.Size = new Size(974, 593);
			pnlStep3.TabIndex = 0;
			// 
			// optionsScroll
			// 
			optionsScroll.Controls.Add(optionsLayout);
			optionsScroll.Location = new Point(3, 48);
			optionsScroll.Name = "optionsScroll";
			optionsScroll.Size = new Size(200, 100);
			optionsScroll.TabIndex = 1;
			// 
			// optionsLayout
			// 
			optionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			optionsLayout.Controls.Add(locationGroup, 0, 0);
			optionsLayout.Controls.Add(restoreOptionsGroup, 0, 1);
			optionsLayout.Controls.Add(advancedGroup, 0, 2);
			optionsLayout.Dock = DockStyle.Fill;
			optionsLayout.Location = new Point(0, 0);
			optionsLayout.Name = "optionsLayout";
			optionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 165F));
			optionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 145F));
			optionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 235F));
			optionsLayout.Size = new Size(200, 100);
			optionsLayout.TabIndex = 0;
			// 
			// locationGroup
			// 
			locationGroup.Controls.Add(locationLayout);
			locationGroup.Dock = DockStyle.Fill;
			locationGroup.Location = new Point(3, 3);
			locationGroup.Name = "locationGroup";
			locationGroup.Size = new Size(194, 159);
			locationGroup.TabIndex = 0;
			locationGroup.TabStop = false;
			// 
			// locationLayout
			// 
			locationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			locationLayout.Controls.Add(rbOriginalLocation, 0, 0);
			locationLayout.Controls.Add(rbNewLocation, 0, 1);
			locationLayout.Controls.Add(pnlNewLocation, 0, 2);
			locationLayout.Dock = DockStyle.Fill;
			locationLayout.Location = new Point(3, 21);
			locationLayout.Name = "locationLayout";
			locationLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			locationLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			locationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			locationLayout.Size = new Size(188, 135);
			locationLayout.TabIndex = 0;
			// 
			// rbOriginalLocation
			// 
			rbOriginalLocation.Checked = true;
			rbOriginalLocation.Dock = DockStyle.Fill;
			rbOriginalLocation.Location = new Point(3, 3);
			rbOriginalLocation.Name = "rbOriginalLocation";
			rbOriginalLocation.Size = new Size(182, 24);
			rbOriginalLocation.TabIndex = 0;
			rbOriginalLocation.TabStop = true;
			rbOriginalLocation.Text = "Restore to original location";
			rbOriginalLocation.CheckedChanged += RestoreLocation_Changed;
			// 
			// rbNewLocation
			// 
			rbNewLocation.Dock = DockStyle.Fill;
			rbNewLocation.Location = new Point(3, 33);
			rbNewLocation.Name = "rbNewLocation";
			rbNewLocation.Size = new Size(182, 24);
			rbNewLocation.TabIndex = 1;
			rbNewLocation.Text = "Restore to new location";
			rbNewLocation.CheckedChanged += RestoreLocation_Changed;
			// 
			// pnlNewLocation
			// 
			pnlNewLocation.Controls.Add(newLocationLayout);
			pnlNewLocation.Dock = DockStyle.Fill;
			pnlNewLocation.Location = new Point(3, 63);
			pnlNewLocation.Name = "pnlNewLocation";
			pnlNewLocation.Padding = new Padding(20, 0, 0, 0);
			pnlNewLocation.Size = new Size(182, 69);
			pnlNewLocation.TabIndex = 2;
			pnlNewLocation.Visible = false;
			// 
			// newLocationLayout
			// 
			newLocationLayout.AutoSize = true;
			newLocationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			newLocationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
			newLocationLayout.Controls.Add(txtRestoreDestination, 0, 1);
			newLocationLayout.Controls.Add(btnBrowseDestination, 1, 1);
			newLocationLayout.Location = new Point(0, 0);
			newLocationLayout.Name = "newLocationLayout";
			newLocationLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
			newLocationLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			newLocationLayout.Size = new Size(200, 55);
			newLocationLayout.TabIndex = 0;
			// 
			// txtRestoreDestination
			// 
			txtRestoreDestination.Dock = DockStyle.Fill;
			txtRestoreDestination.Location = new Point(3, 28);
			txtRestoreDestination.Name = "txtRestoreDestination";
			txtRestoreDestination.Size = new Size(104, 25);
			txtRestoreDestination.TabIndex = 1;
			// 
			// btnBrowseDestination
			// 
			btnBrowseDestination.Location = new Point(113, 28);
			btnBrowseDestination.Name = "btnBrowseDestination";
			btnBrowseDestination.Size = new Size(75, 23);
			btnBrowseDestination.TabIndex = 2;
			btnBrowseDestination.Text = "Browse Destination";
			btnBrowseDestination.Click += BrowseDestination_Click;
			// 
			// restoreOptionsGroup
			// 
			restoreOptionsGroup.Controls.Add(restoreOptionsLayout);
			restoreOptionsGroup.Location = new Point(3, 168);
			restoreOptionsGroup.Name = "restoreOptionsGroup";
			restoreOptionsGroup.Size = new Size(194, 100);
			restoreOptionsGroup.TabIndex = 1;
			restoreOptionsGroup.TabStop = false;
			// 
			// restoreOptionsLayout
			// 
			restoreOptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			restoreOptionsLayout.Controls.Add(chkOverwrite, 0, 0);
			restoreOptionsLayout.Controls.Add(chkPreservePermissions, 0, 1);
			restoreOptionsLayout.Controls.Add(chkRestoreSystemState, 0, 2);
			restoreOptionsLayout.Location = new Point(0, 0);
			restoreOptionsLayout.Name = "restoreOptionsLayout";
			restoreOptionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
			restoreOptionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
			restoreOptionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
			restoreOptionsLayout.Size = new Size(200, 100);
			restoreOptionsLayout.TabIndex = 0;
			// 
			// chkOverwrite
			// 
			chkOverwrite.Checked = true;
			chkOverwrite.CheckState = CheckState.Checked;
			chkOverwrite.Dock = DockStyle.Fill;
			chkOverwrite.Location = new Point(3, 3);
			chkOverwrite.Name = "chkOverwrite";
			chkOverwrite.Size = new Size(194, 27);
			chkOverwrite.TabIndex = 0;
			chkOverwrite.Text = "Overwrite existing files";
			// 
			// chkPreservePermissions
			// 
			chkPreservePermissions.Checked = true;
			chkPreservePermissions.CheckState = CheckState.Checked;
			chkPreservePermissions.Dock = DockStyle.Fill;
			chkPreservePermissions.Location = new Point(3, 36);
			chkPreservePermissions.Name = "chkPreservePermissions";
			chkPreservePermissions.Size = new Size(194, 27);
			chkPreservePermissions.TabIndex = 1;
			chkPreservePermissions.Text = "Preserve file permissions and ownership";
			// 
			// chkRestoreSystemState
			// 
			chkRestoreSystemState.Dock = DockStyle.Fill;
			chkRestoreSystemState.Location = new Point(3, 69);
			chkRestoreSystemState.Name = "chkRestoreSystemState";
			chkRestoreSystemState.Size = new Size(194, 28);
			chkRestoreSystemState.TabIndex = 2;
			chkRestoreSystemState.Text = "Restore System State (if backed up)";
			// 
			// advancedGroup
			// 
			advancedGroup.Controls.Add(advancedLayout);
			advancedGroup.Location = new Point(3, 313);
			advancedGroup.Name = "advancedGroup";
			advancedGroup.Size = new Size(194, 100);
			advancedGroup.TabIndex = 2;
			advancedGroup.TabStop = false;
			// 
			// advancedLayout
			// 
			advancedLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			advancedLayout.Controls.Add(chkRestoreAsHyperV, 0, 0);
			advancedLayout.Controls.Add(pnlHyperV, 0, 1);
			advancedLayout.Location = new Point(0, 0);
			advancedLayout.Name = "advancedLayout";
			advancedLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
			advancedLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			advancedLayout.Size = new Size(200, 100);
			advancedLayout.TabIndex = 0;
			// 
			// chkRestoreAsHyperV
			// 
			chkRestoreAsHyperV.Dock = DockStyle.Fill;
			chkRestoreAsHyperV.Location = new Point(3, 3);
			chkRestoreAsHyperV.Name = "chkRestoreAsHyperV";
			chkRestoreAsHyperV.Size = new Size(194, 26);
			chkRestoreAsHyperV.TabIndex = 0;
			chkRestoreAsHyperV.Text = "Restore as Hyper-V Virtual Machine";
			chkRestoreAsHyperV.CheckedChanged += HyperV_CheckedChanged;
			// 
			// pnlHyperV
			// 
			pnlHyperV.Controls.Add(hyperVLayout);
			pnlHyperV.Dock = DockStyle.Fill;
			pnlHyperV.Location = new Point(3, 35);
			pnlHyperV.Name = "pnlHyperV";
			pnlHyperV.Padding = new Padding(20, 0, 0, 0);
			pnlHyperV.Size = new Size(194, 62);
			pnlHyperV.TabIndex = 1;
			pnlHyperV.Visible = false;
			// 
			// hyperVLayout
			// 
			hyperVLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			hyperVLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
			hyperVLayout.Controls.Add(txtVMName, 0, 1);
			hyperVLayout.Controls.Add(txtVMStorage, 0, 3);
			hyperVLayout.Controls.Add(btnBrowseVMStorage, 1, 3);
			hyperVLayout.Controls.Add(chkStartVM, 0, 4);
			hyperVLayout.Location = new Point(0, 0);
			hyperVLayout.Name = "hyperVLayout";
			hyperVLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
			hyperVLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			hyperVLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
			hyperVLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			hyperVLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			hyperVLayout.Size = new Size(200, 100);
			hyperVLayout.TabIndex = 0;
			// 
			// txtVMName
			// 
			txtVMName.Dock = DockStyle.Fill;
			txtVMName.Location = new Point(3, 28);
			txtVMName.Name = "txtVMName";
			txtVMName.Size = new Size(104, 25);
			txtVMName.TabIndex = 1;
			// 
			// txtVMStorage
			// 
			txtVMStorage.Dock = DockStyle.Fill;
			txtVMStorage.Location = new Point(3, 83);
			txtVMStorage.Name = "txtVMStorage";
			txtVMStorage.Size = new Size(104, 25);
			txtVMStorage.TabIndex = 3;
			// 
			// btnBrowseVMStorage
			// 
			btnBrowseVMStorage.Location = new Point(113, 83);
			btnBrowseVMStorage.Name = "btnBrowseVMStorage";
			btnBrowseVMStorage.Size = new Size(75, 23);
			btnBrowseVMStorage.TabIndex = 4;
			btnBrowseVMStorage.Click += BrowseVMStorage_Click;
			// 
			// chkStartVM
			// 
			chkStartVM.Dock = DockStyle.Fill;
			chkStartVM.Location = new Point(3, 113);
			chkStartVM.Name = "chkStartVM";
			chkStartVM.Size = new Size(104, 24);
			chkStartVM.TabIndex = 5;
			chkStartVM.Text = "Start VM after restore";
			// 
			// step3Buttons
			// 
			step3Buttons.Controls.Add(btnStartRestore);
			step3Buttons.Controls.Add(btnStep3Back);
			step3Buttons.Location = new Point(3, 551);
			step3Buttons.Name = "step3Buttons";
			step3Buttons.Size = new Size(200, 39);
			step3Buttons.TabIndex = 2;
			// 
			// btnStartRestore
			// 
			btnStartRestore.Location = new Point(3, 3);
			btnStartRestore.Name = "btnStartRestore";
			btnStartRestore.Size = new Size(75, 23);
			btnStartRestore.TabIndex = 0;
			btnStartRestore.Text = "Start Restore";
			btnStartRestore.Click += StartRestore_Click;
			// 
			// btnStep3Back
			// 
			btnStep3Back.Location = new Point(84, 3);
			btnStep3Back.Name = "btnStep3Back";
			btnStep3Back.Size = new Size(75, 23);
			btnStep3Back.TabIndex = 1;
			btnStep3Back.Text = "Back";
			btnStep3Back.Click += Step3Back_Click;
			// 
			// pnlStep2
			// 
			pnlStep2.ColumnCount = 1;
			pnlStep2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			pnlStep2.Controls.Add(contentsGroup, 0, 1);
			pnlStep2.Controls.Add(step2Buttons, 0, 2);
			pnlStep2.Dock = DockStyle.Fill;
			pnlStep2.Location = new Point(0, 0);
			pnlStep2.Name = "pnlStep2";
			pnlStep2.RowCount = 3;
			pnlStep2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			pnlStep2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			pnlStep2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			pnlStep2.Size = new Size(974, 593);
			pnlStep2.TabIndex = 1;
			// 
			// contentsGroup
			// 
			contentsGroup.Controls.Add(contentsLayout);
			contentsGroup.Location = new Point(3, 48);
			contentsGroup.Name = "contentsGroup";
			contentsGroup.Size = new Size(200, 100);
			contentsGroup.TabIndex = 1;
			contentsGroup.TabStop = false;
			// 
			// contentsLayout
			// 
			contentsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			contentsLayout.Controls.Add(treeRestoreContents, 0, 1);
			contentsLayout.Controls.Add(treeButtons, 0, 2);
			contentsLayout.Location = new Point(0, 0);
			contentsLayout.Name = "contentsLayout";
			contentsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
			contentsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			contentsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
			contentsLayout.Size = new Size(200, 100);
			contentsLayout.TabIndex = 0;
			// 
			// treeRestoreContents
			// 
			treeRestoreContents.CheckBoxes = true;
			treeRestoreContents.Dock = DockStyle.Fill;
			treeRestoreContents.HideSelection = false;
			treeRestoreContents.Location = new Point(3, 38);
			treeRestoreContents.Name = "treeRestoreContents";
			treeRestoreContents.Size = new Size(194, 17);
			treeRestoreContents.TabIndex = 1;
			// 
			// treeButtons
			// 
			treeButtons.Controls.Add(btnExpand);
			treeButtons.Controls.Add(btnCollapse);
			treeButtons.Controls.Add(btnSelect);
			treeButtons.Controls.Add(btnUnselect);
			treeButtons.Location = new Point(3, 61);
			treeButtons.Name = "treeButtons";
			treeButtons.Size = new Size(194, 36);
			treeButtons.TabIndex = 2;
			// 
			// btnExpand
			// 
			btnExpand.Location = new Point(3, 3);
			btnExpand.Name = "btnExpand";
			btnExpand.Size = new Size(75, 23);
			btnExpand.TabIndex = 0;
			btnExpand.Click += ExpandAll_Click;
			// 
			// btnCollapse
			// 
			btnCollapse.Location = new Point(84, 3);
			btnCollapse.Name = "btnCollapse";
			btnCollapse.Size = new Size(75, 23);
			btnCollapse.TabIndex = 1;
			btnCollapse.Click += CollapseAll_Click;
			// 
			// btnSelect
			// 
			btnSelect.Location = new Point(3, 32);
			btnSelect.Name = "btnSelect";
			btnSelect.Size = new Size(75, 23);
			btnSelect.TabIndex = 2;
			btnSelect.Click += SelectAll_Click;
			// 
			// btnUnselect
			// 
			btnUnselect.Location = new Point(84, 32);
			btnUnselect.Name = "btnUnselect";
			btnUnselect.Size = new Size(75, 23);
			btnUnselect.TabIndex = 3;
			btnUnselect.Click += UnselectAll_Click;
			// 
			// step2Buttons
			// 
			step2Buttons.Controls.Add(btnStep2Next);
			step2Buttons.Controls.Add(btnStep2Back);
			step2Buttons.Location = new Point(3, 551);
			step2Buttons.Name = "step2Buttons";
			step2Buttons.Size = new Size(200, 39);
			step2Buttons.TabIndex = 2;
			// 
			// btnStep2Next
			// 
			btnStep2Next.Location = new Point(3, 3);
			btnStep2Next.Name = "btnStep2Next";
			btnStep2Next.Size = new Size(75, 23);
			btnStep2Next.TabIndex = 0;
			btnStep2Next.Click += Step2Next_Click;
			// 
			// btnStep2Back
			// 
			btnStep2Back.Location = new Point(84, 3);
			btnStep2Back.Name = "btnStep2Back";
			btnStep2Back.Size = new Size(75, 23);
			btnStep2Back.TabIndex = 1;
			btnStep2Back.Click += Step2Back_Click;
			// 
			// pnlStep1
			// 
			pnlStep1.ColumnCount = 1;
			pnlStep1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			pnlStep1.Controls.Add(step1Title, 0, 0);
			pnlStep1.Controls.Add(sourceGroup, 0, 1);
			pnlStep1.Controls.Add(datesGroup, 0, 2);
			pnlStep1.Controls.Add(step1Buttons, 0, 3);
			pnlStep1.Dock = DockStyle.Fill;
			pnlStep1.Location = new Point(0, 0);
			pnlStep1.Name = "pnlStep1";
			pnlStep1.RowCount = 4;
			pnlStep1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			pnlStep1.RowStyles.Add(new RowStyle(SizeType.Absolute, 125F));
			pnlStep1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			pnlStep1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			pnlStep1.Size = new Size(974, 593);
			pnlStep1.TabIndex = 2;
			// 
			// step1Title
			// 
			step1Title.Location = new Point(3, 0);
			step1Title.Name = "step1Title";
			step1Title.Size = new Size(100, 23);
			step1Title.TabIndex = 0;
			// 
			// sourceGroup
			// 
			sourceGroup.Controls.Add(sourceLayout);
			sourceGroup.Location = new Point(3, 48);
			sourceGroup.Name = "sourceGroup";
			sourceGroup.Size = new Size(200, 100);
			sourceGroup.TabIndex = 1;
			sourceGroup.TabStop = false;
			// 
			// sourceLayout
			// 
			sourceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			sourceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
			sourceLayout.Controls.Add(txtBackupSource, 0, 1);
			sourceLayout.Controls.Add(btnBrowseBackup, 1, 1);
			sourceLayout.Controls.Add(btnLoadBackup, 0, 2);
			sourceLayout.Location = new Point(0, 0);
			sourceLayout.Name = "sourceLayout";
			sourceLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
			sourceLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
			sourceLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
			sourceLayout.Size = new Size(200, 100);
			sourceLayout.TabIndex = 0;
			// 
			// txtBackupSource
			// 
			txtBackupSource.Dock = DockStyle.Fill;
			txtBackupSource.Location = new Point(0, 28);
			txtBackupSource.Margin = new Padding(0, 3, 5, 3);
			txtBackupSource.Name = "txtBackupSource";
			txtBackupSource.Size = new Size(105, 25);
			txtBackupSource.TabIndex = 1;
			// 
			// btnBrowseBackup
			// 
			btnBrowseBackup.Location = new Point(113, 28);
			btnBrowseBackup.Name = "btnBrowseBackup";
			btnBrowseBackup.Size = new Size(75, 23);
			btnBrowseBackup.TabIndex = 2;
			btnBrowseBackup.Click += BrowseBackup_Click;
			// 
			// btnLoadBackup
			// 
			btnLoadBackup.Location = new Point(3, 60);
			btnLoadBackup.Name = "btnLoadBackup";
			btnLoadBackup.Size = new Size(75, 23);
			btnLoadBackup.TabIndex = 3;
			btnLoadBackup.Click += LoadBackup_Click;
			// 
			// datesGroup
			// 
			datesGroup.Controls.Add(datesLayout);
			datesGroup.Location = new Point(3, 173);
			datesGroup.Name = "datesGroup";
			datesGroup.Size = new Size(200, 100);
			datesGroup.TabIndex = 2;
			datesGroup.TabStop = false;
			// 
			// datesLayout
			// 
			datesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			datesLayout.Controls.Add(txtBackupInfo, 0, 0);
			datesLayout.Controls.Add(lstBackupDates, 0, 1);
			datesLayout.Location = new Point(0, 0);
			datesLayout.Name = "datesLayout";
			datesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			datesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			datesLayout.Size = new Size(200, 100);
			datesLayout.TabIndex = 0;
			// 
			// txtBackupInfo
			// 
			txtBackupInfo.Dock = DockStyle.Fill;
			txtBackupInfo.ForeColor = SystemColors.GrayText;
			txtBackupInfo.Location = new Point(3, 0);
			txtBackupInfo.Name = "txtBackupInfo";
			txtBackupInfo.Size = new Size(194, 40);
			txtBackupInfo.TabIndex = 0;
			txtBackupInfo.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// lstBackupDates
			// 
			lstBackupDates.Dock = DockStyle.Fill;
			lstBackupDates.FullRowSelect = true;
			lstBackupDates.Location = new Point(3, 43);
			lstBackupDates.MultiSelect = false;
			lstBackupDates.Name = "lstBackupDates";
			lstBackupDates.Size = new Size(194, 54);
			lstBackupDates.TabIndex = 1;
			lstBackupDates.UseCompatibleStateImageBehavior = false;
			lstBackupDates.View = View.Details;
			// 
			// step1Buttons
			// 
			step1Buttons.Controls.Add(btnStep1Next);
			step1Buttons.Location = new Point(3, 551);
			step1Buttons.Name = "step1Buttons";
			step1Buttons.Size = new Size(200, 39);
			step1Buttons.TabIndex = 3;
			// 
			// btnStep1Next
			// 
			btnStep1Next.Location = new Point(3, 3);
			btnStep1Next.Name = "btnStep1Next";
			btnStep1Next.Size = new Size(75, 23);
			btnStep1Next.TabIndex = 0;
			btnStep1Next.Click += Step1Next_Click;
			// 
			// progressLayout
			// 
			progressLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			progressLayout.Controls.Add(progressBar, 0, 0);
			progressLayout.Controls.Add(txtProgress, 0, 1);
			progressLayout.Location = new Point(13, 657);
			progressLayout.Name = "progressLayout";
			progressLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
			progressLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
			progressLayout.Size = new Size(200, 42);
			progressLayout.TabIndex = 2;
			// 
			// progressBar
			// 
			progressBar.Dock = DockStyle.Fill;
			progressBar.Location = new Point(3, 3);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(194, 17);
			progressBar.TabIndex = 0;
			progressBar.Visible = false;
			// 
			// txtProgress
			// 
			txtProgress.Dock = DockStyle.Fill;
			txtProgress.Location = new Point(3, 23);
			txtProgress.Name = "txtProgress";
			txtProgress.Size = new Size(194, 23);
			txtProgress.TabIndex = 1;
			txtProgress.TextAlign = ContentAlignment.MiddleCenter;
			txtProgress.Visible = false;
			// 
			// actionButtons
			// 
			actionButtons.Controls.Add(btnCancel);
			actionButtons.Location = new Point(13, 705);
			actionButtons.Name = "actionButtons";
			actionButtons.Size = new Size(200, 36);
			actionButtons.TabIndex = 3;
			// 
			// btnCancel
			// 
			btnCancel.Location = new Point(3, 3);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(75, 23);
			btnCancel.TabIndex = 0;
			btnCancel.Text = "Cancel";
			btnCancel.Click += Cancel_Click;
			// 
			// RestoreForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1000, 754);
			Controls.Add(mainLayout);
			MinimumSize = new Size(760, 600);
			Name = "RestoreForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore from Backup";
			mainLayout.ResumeLayout(false);
			mainLayout.PerformLayout();
			wizardHost.ResumeLayout(false);
			pnlStep3.ResumeLayout(false);
			optionsScroll.ResumeLayout(false);
			optionsLayout.ResumeLayout(false);
			locationGroup.ResumeLayout(false);
			locationLayout.ResumeLayout(false);
			pnlNewLocation.ResumeLayout(false);
			pnlNewLocation.PerformLayout();
			newLocationLayout.ResumeLayout(false);
			newLocationLayout.PerformLayout();
			restoreOptionsGroup.ResumeLayout(false);
			restoreOptionsLayout.ResumeLayout(false);
			advancedGroup.ResumeLayout(false);
			advancedLayout.ResumeLayout(false);
			pnlHyperV.ResumeLayout(false);
			hyperVLayout.ResumeLayout(false);
			hyperVLayout.PerformLayout();
			step3Buttons.ResumeLayout(false);
			pnlStep2.ResumeLayout(false);
			contentsGroup.ResumeLayout(false);
			contentsLayout.ResumeLayout(false);
			treeButtons.ResumeLayout(false);
			step2Buttons.ResumeLayout(false);
			pnlStep1.ResumeLayout(false);
			sourceGroup.ResumeLayout(false);
			sourceLayout.ResumeLayout(false);
			sourceLayout.PerformLayout();
			datesGroup.ResumeLayout(false);
			datesLayout.ResumeLayout(false);
			step1Buttons.ResumeLayout(false);
			progressLayout.ResumeLayout(false);
			actionButtons.ResumeLayout(false);
			ResumeLayout(false);
		}

		private Panel optionsScroll;
		private TableLayoutPanel optionsLayout;
		private GroupBox locationGroup;
		private TableLayoutPanel locationLayout;
		private TableLayoutPanel newLocationLayout;
		private Button btnBrowseDestination;
		private GroupBox restoreOptionsGroup;
		private TableLayoutPanel restoreOptionsLayout;
		private GroupBox advancedGroup;
		private TableLayoutPanel advancedLayout;
		private TableLayoutPanel hyperVLayout;
		private Button btnBrowseVMStorage;
		private FlowLayoutPanel step3Buttons;
		private Button btnStartRestore;
		private Button btnStep3Back;
		private GroupBox contentsGroup;
		private TableLayoutPanel contentsLayout;
		private FlowLayoutPanel treeButtons;
		private Button btnExpand;
		private Button btnCollapse;
		private Button btnSelect;
		private Button btnUnselect;
		private FlowLayoutPanel step2Buttons;
		private Button btnStep2Next;
		private Button btnStep2Back;
		private Label step1Title;
		private GroupBox sourceGroup;
		private TableLayoutPanel sourceLayout;
		private Button btnBrowseBackup;
		private Button btnLoadBackup;
		private GroupBox datesGroup;
		private TableLayoutPanel datesLayout;
		private FlowLayoutPanel step1Buttons;
		private Button btnStep1Next;
		private TableLayoutPanel progressLayout;
		private FlowLayoutPanel actionButtons;
		private Button btnCancel;
	}
}
