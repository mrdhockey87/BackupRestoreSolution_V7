using System.Drawing;
using System.Windows.Forms;
using SecureServerBackup.Controls;

namespace SecureServerBackup.WinForm
{
	partial class RestoreFormNew
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			tlpRoot = new TableLayoutPanel();
			lblTitle = new Label();
			tlpBody = new TableLayoutPanel();
			tlpLeft = new TableLayoutPanel();
			grpSelectBackup = new GroupBox();
			tlpSelectBackup = new TableLayoutPanel();
			lblBackupSource = new Label();
			tlpBackupSourceRow = new TableLayoutPanel();
			txtBackupSource = new TextBox();
			btnBrowseBackup = new Button();
			btnScanBackup = new Button();
			pnlBackupInfo = new TableLayoutPanel();
			flpBackupCounts = new FlowLayoutPanel();
			txtBackupFileCount = new Label();
			lblCountSep1 = new Label();
			txtBackupTotalSize = new Label();
			lblCountSep2 = new Label();
			txtBackupRestorePointCount = new Label();
			txtRestorePointPrompt = new Label();
			lstRestorePoints = new ToggleSelectListBox();
			txtPreselectedRestorePointSummary = new Label();
			grpRestoreOptions = new GroupBox();
			pnlOptionsScroll = new Panel();
			tlpOptions = new TableLayoutPanel();
			txtWhatToRestoreLabel = new Label();
			txtPreselectedScopeSummary = new Label();
			rbRestoreAll = new RadioButton();
			rbRestoreSelected = new RadioButton();
			pnlItemSelection = new Panel();
			lstBackupItems = new ListBox();
			lblRestoreDestination = new Label();
			txtDestinationHelp = new Label();
			pnlHyperVRestoreMode = new TableLayoutPanel();
			lblHyperVRestoreTarget = new Label();
			cmbHyperVRestoreTarget = new ComboBox();
			pnlHyperVVmOptions = new TableLayoutPanel();
			lblRestoreMode = new Label();
			rbHyperVReplaceExisting = new RadioButton();
			rbHyperVRestoreToDirectory = new RadioButton();
			pnlHyperVReplaceExistingOptions = new TableLayoutPanel();
			lblReplaceVm = new Label();
			cmbHyperVVmToReplace = new ComboBox();
			pnlHyperVDirectoryOptions = new TableLayoutPanel();
			lblRestoreDir = new Label();
			tlpHyperVDirRow = new TableLayoutPanel();
			txtHyperVRestoreDirectory = new TextBox();
			btnBrowseHyperVDir = new Button();
			lblVmNameAfter = new Label();
			txtHyperVVmName = new TextBox();
			chkStartHyperVVm = new CheckBox();
			chkRestoreToHyperVDisk = new CheckBox();
			pnlRegularHyperVRestore = new TableLayoutPanel();
			lblVhdPath = new Label();
			tlpHyperVDiskRow = new TableLayoutPanel();
			txtHyperVVirtualDiskPath = new TextBox();
			btnBrowseHyperVDisk = new Button();
			lblAfterRestore = new Label();
			rbLeaveHyperVDiskDetached = new RadioButton();
			chkAttachToExistingHyperVVm = new RadioButton();
			rbCreateNewHyperVVm = new RadioButton();
			pnlExistingHyperVVmOptions = new TableLayoutPanel();
			lblExistingVm = new Label();
			cmbExistingHyperVVm = new ComboBox();
			pnlNewHyperVVmOptions = new TableLayoutPanel();
			lblNewVmName = new Label();
			txtNewHyperVVmName = new TextBox();
			lblNewVmFolder = new Label();
			tlpNewVmPathRow = new TableLayoutPanel();
			txtNewHyperVVmPath = new TextBox();
			btnBrowseNewVmPath = new Button();
			lblNewVmGen = new Label();
			cmbNewHyperVGeneration = new ComboBox();
			chkStartCreatedHyperVVm = new CheckBox();
			pnlLocationChoice = new TableLayoutPanel();
			lblFileFolderTarget = new Label();
			btnBrowseRestoreDestination = new Button();
			lblTargetLocation = new Label();
			txtFolderRestoreDestination = new TextBox();
			pnlHyperVCloneDestination = new TableLayoutPanel();
			lblCloneDestination = new Label();
			rbHyperVCloneDefault = new RadioButton();
			rbHyperVCloneAlternate = new RadioButton();
			pnlHyperVCloneAlternate = new TableLayoutPanel();
			lblCloneVmFolder = new Label();
			tlpCloneVmRow = new TableLayoutPanel();
			txtHyperVCloneVmFolder = new TextBox();
			btnBrowseCloneVm = new Button();
			lblCloneDiskFolder = new Label();
			tlpCloneDiskRow = new TableLayoutPanel();
			txtHyperVCloneDiskFolder = new TextBox();
			btnBrowseCloneDisk = new Button();
			chkOverwrite = new CheckBox();
			chkPreservePermissions = new CheckBox();
			chkVerifyAfterRestore = new CheckBox();
			grpRestoreTarget = new GroupBox();
			tlpTarget = new TableLayoutPanel();
			txtDriveTreeHelp = new Label();
			pnlTargetTreeHost = new Panel();
			loadingTargetOverlay = new Panel();
			tlpLoadingCenter = new TableLayoutPanel();
			pnlLoadingContent = new Panel();
			progressLoadingTargets = new ProgressBar();
			lblLoadingTargets = new Label();
			treeViewRestoreTarget = new TreeView();
			imgCheckStates = new ImageList(components);
			tlpTargetFooter = new TableLayoutPanel();
			flpTargetButtons = new FlowLayoutPanel();
			btnRefreshTarget = new Button();
			btnExpandAllTarget = new Button();
			btnCollapseAllTarget = new Button();
			chkShowHiddenPartitionsTarget = new CheckBox();
			txtSelectedTargetLabel = new Label();
			flpButtons = new FlowLayoutPanel();
			btnCancel = new Button();
			btnRestore = new Button();
			toolTip1 = new ToolTip(components);
			tlpRoot.SuspendLayout();
			tlpBody.SuspendLayout();
			tlpLeft.SuspendLayout();
			grpSelectBackup.SuspendLayout();
			tlpSelectBackup.SuspendLayout();
			tlpBackupSourceRow.SuspendLayout();
			pnlBackupInfo.SuspendLayout();
			flpBackupCounts.SuspendLayout();
			grpRestoreOptions.SuspendLayout();
			pnlOptionsScroll.SuspendLayout();
			tlpOptions.SuspendLayout();
			pnlItemSelection.SuspendLayout();
			pnlHyperVRestoreMode.SuspendLayout();
			pnlHyperVVmOptions.SuspendLayout();
			pnlHyperVReplaceExistingOptions.SuspendLayout();
			pnlHyperVDirectoryOptions.SuspendLayout();
			tlpHyperVDirRow.SuspendLayout();
			pnlRegularHyperVRestore.SuspendLayout();
			tlpHyperVDiskRow.SuspendLayout();
			pnlExistingHyperVVmOptions.SuspendLayout();
			pnlNewHyperVVmOptions.SuspendLayout();
			tlpNewVmPathRow.SuspendLayout();
			pnlLocationChoice.SuspendLayout();
			pnlHyperVCloneDestination.SuspendLayout();
			pnlHyperVCloneAlternate.SuspendLayout();
			tlpCloneVmRow.SuspendLayout();
			tlpCloneDiskRow.SuspendLayout();
			grpRestoreTarget.SuspendLayout();
			tlpTarget.SuspendLayout();
			pnlTargetTreeHost.SuspendLayout();
			loadingTargetOverlay.SuspendLayout();
			tlpLoadingCenter.SuspendLayout();
			pnlLoadingContent.SuspendLayout();
			tlpTargetFooter.SuspendLayout();
			flpTargetButtons.SuspendLayout();
			flpButtons.SuspendLayout();
			SuspendLayout();
			// 
			// tlpRoot
			// 
			tlpRoot.ColumnCount = 1;
			tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpRoot.Controls.Add(lblTitle, 0, 0);
			tlpRoot.Controls.Add(tlpBody, 0, 1);
			tlpRoot.Controls.Add(flpButtons, 0, 2);
			tlpRoot.Dock = DockStyle.Fill;
			tlpRoot.Location = new Point(10, 11);
			tlpRoot.Name = "tlpRoot";
			tlpRoot.RowCount = 3;
			tlpRoot.RowStyles.Add(new RowStyle());
			tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpRoot.RowStyles.Add(new RowStyle());
			tlpRoot.Size = new Size(1080, 937);
			tlpRoot.TabIndex = 0;
			// 
			// lblTitle
			// 
			lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
			lblTitle.Location = new Point(3, 0);
			lblTitle.Margin = new Padding(3, 0, 3, 11);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new Size(218, 30);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "Restore from Backup";
			// 
			// tlpBody
			// 
			tlpBody.ColumnCount = 3;
			tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
			tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tlpBody.Controls.Add(tlpLeft, 0, 0);
			tlpBody.Controls.Add(grpRestoreTarget, 2, 0);
			tlpBody.Dock = DockStyle.Fill;
			tlpBody.Location = new Point(3, 44);
			tlpBody.Name = "tlpBody";
			tlpBody.RowCount = 1;
			tlpBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpBody.Size = new Size(1074, 844);
			tlpBody.TabIndex = 1;
			// 
			// tlpLeft
			// 
			tlpLeft.ColumnCount = 1;
			tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpLeft.Controls.Add(grpSelectBackup, 0, 0);
			tlpLeft.Controls.Add(grpRestoreOptions, 0, 1);
			tlpLeft.Dock = DockStyle.Fill;
			tlpLeft.Location = new Point(3, 3);
			tlpLeft.Name = "tlpLeft";
			tlpLeft.RowCount = 2;
			tlpLeft.RowStyles.Add(new RowStyle());
			tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpLeft.Size = new Size(526, 838);
			tlpLeft.TabIndex = 0;
			// 
			// grpSelectBackup
			// 
			grpSelectBackup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			grpSelectBackup.Controls.Add(tlpSelectBackup);
			grpSelectBackup.Dock = DockStyle.Fill;
			grpSelectBackup.Location = new Point(0, 0);
			grpSelectBackup.Margin = new Padding(0, 0, 0, 9);
			grpSelectBackup.Name = "grpSelectBackup";
			grpSelectBackup.Padding = new Padding(10, 11, 10, 11);
			grpSelectBackup.Size = new Size(526, 120);
			grpSelectBackup.TabIndex = 0;
			grpSelectBackup.TabStop = false;
			grpSelectBackup.Text = "Select Backup";
			// 
			// tlpSelectBackup
			// 
			tlpSelectBackup.ColumnCount = 1;
			tlpSelectBackup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpSelectBackup.Controls.Add(lblBackupSource, 0, 0);
			tlpSelectBackup.Controls.Add(tlpBackupSourceRow, 0, 1);
			tlpSelectBackup.Controls.Add(btnScanBackup, 0, 2);
			tlpSelectBackup.Controls.Add(pnlBackupInfo, 0, 3);
			tlpSelectBackup.Dock = DockStyle.Top;
			tlpSelectBackup.Location = new Point(10, 29);
			tlpSelectBackup.Name = "tlpSelectBackup";
			tlpSelectBackup.RowCount = 4;
			tlpSelectBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpSelectBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpSelectBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpSelectBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpSelectBackup.Size = new Size(506, 149);
			tlpSelectBackup.TabIndex = 0;
			// 
			// lblBackupSource
			// 
			lblBackupSource.Location = new Point(3, 0);
			lblBackupSource.Name = "lblBackupSource";
			lblBackupSource.Size = new Size(96, 17);
			lblBackupSource.TabIndex = 0;
			lblBackupSource.Text = "Backup Source:";
			// 
			// tlpBackupSourceRow
			// 
			tlpBackupSourceRow.ColumnCount = 2;
			tlpBackupSourceRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpBackupSourceRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
			tlpBackupSourceRow.Controls.Add(txtBackupSource, 0, 0);
			tlpBackupSourceRow.Controls.Add(btnBrowseBackup, 1, 0);
			tlpBackupSourceRow.Dock = DockStyle.Fill;
			tlpBackupSourceRow.Location = new Point(3, 23);
			tlpBackupSourceRow.Name = "tlpBackupSourceRow";
			tlpBackupSourceRow.RowCount = 1;
			tlpBackupSourceRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpBackupSourceRow.Size = new Size(500, 14);
			tlpBackupSourceRow.TabIndex = 1;
			// 
			// txtBackupSource
			// 
			txtBackupSource.Dock = DockStyle.Fill;
			txtBackupSource.Location = new Point(3, 3);
			txtBackupSource.Name = "txtBackupSource";
			txtBackupSource.ReadOnly = true;
			txtBackupSource.Size = new Size(404, 25);
			txtBackupSource.TabIndex = 0;
			// 
			// btnBrowseBackup
			// 
			btnBrowseBackup.Dock = DockStyle.Fill;
			btnBrowseBackup.Location = new Point(413, 3);
			btnBrowseBackup.Name = "btnBrowseBackup";
			btnBrowseBackup.Size = new Size(84, 14);
			btnBrowseBackup.TabIndex = 1;
			btnBrowseBackup.Text = "Browse...";
			btnBrowseBackup.UseVisualStyleBackColor = true;
			btnBrowseBackup.Click += BrowseBackup_Click;
			// 
			// btnScanBackup
			// 
			btnScanBackup.Location = new Point(3, 43);
			btnScanBackup.Name = "btnScanBackup";
			btnScanBackup.Size = new Size(60, 14);
			btnScanBackup.TabIndex = 2;
			btnScanBackup.Text = "Scan Backup";
			btnScanBackup.UseVisualStyleBackColor = true;
			btnScanBackup.Click += ScanBackup_Click;
			// 
			// pnlBackupInfo
			// 
			pnlBackupInfo.ColumnCount = 1;
			pnlBackupInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlBackupInfo.Controls.Add(flpBackupCounts, 0, 0);
			pnlBackupInfo.Controls.Add(txtRestorePointPrompt, 0, 1);
			pnlBackupInfo.Controls.Add(lstRestorePoints, 0, 2);
			pnlBackupInfo.Controls.Add(txtPreselectedRestorePointSummary, 0, 3);
			pnlBackupInfo.Dock = DockStyle.Fill;
			pnlBackupInfo.Location = new Point(3, 63);
			pnlBackupInfo.Name = "pnlBackupInfo";
			pnlBackupInfo.RowCount = 4;
			pnlBackupInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlBackupInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlBackupInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
			pnlBackupInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlBackupInfo.Size = new Size(500, 83);
			pnlBackupInfo.TabIndex = 3;
			pnlBackupInfo.Visible = false;
			// 
			// flpBackupCounts
			// 
			flpBackupCounts.Controls.Add(txtBackupFileCount);
			flpBackupCounts.Controls.Add(lblCountSep1);
			flpBackupCounts.Controls.Add(txtBackupTotalSize);
			flpBackupCounts.Controls.Add(lblCountSep2);
			flpBackupCounts.Controls.Add(txtBackupRestorePointCount);
			flpBackupCounts.Dock = DockStyle.Fill;
			flpBackupCounts.Location = new Point(3, 3);
			flpBackupCounts.Name = "flpBackupCounts";
			flpBackupCounts.Size = new Size(494, 14);
			flpBackupCounts.TabIndex = 0;
			// 
			// txtBackupFileCount
			// 
			txtBackupFileCount.ForeColor = Color.FromArgb(96, 96, 96);
			txtBackupFileCount.Location = new Point(0, 0);
			txtBackupFileCount.Margin = new Padding(0);
			txtBackupFileCount.Name = "txtBackupFileCount";
			txtBackupFileCount.Size = new Size(0, 17);
			txtBackupFileCount.TabIndex = 0;
			// 
			// lblCountSep1
			// 
			lblCountSep1.ForeColor = Color.FromArgb(96, 96, 96);
			lblCountSep1.Location = new Point(0, 0);
			lblCountSep1.Margin = new Padding(0, 0, 4, 0);
			lblCountSep1.Name = "lblCountSep1";
			lblCountSep1.Size = new Size(11, 17);
			lblCountSep1.TabIndex = 1;
			lblCountSep1.Text = ",";
			// 
			// txtBackupTotalSize
			// 
			txtBackupTotalSize.ForeColor = Color.FromArgb(96, 96, 96);
			txtBackupTotalSize.Location = new Point(15, 0);
			txtBackupTotalSize.Margin = new Padding(0);
			txtBackupTotalSize.Name = "txtBackupTotalSize";
			txtBackupTotalSize.Size = new Size(0, 17);
			txtBackupTotalSize.TabIndex = 2;
			// 
			// lblCountSep2
			// 
			lblCountSep2.ForeColor = Color.FromArgb(96, 96, 96);
			lblCountSep2.Location = new Point(15, 0);
			lblCountSep2.Margin = new Padding(0, 0, 4, 0);
			lblCountSep2.Name = "lblCountSep2";
			lblCountSep2.Size = new Size(11, 17);
			lblCountSep2.TabIndex = 3;
			lblCountSep2.Text = ",";
			// 
			// txtBackupRestorePointCount
			// 
			txtBackupRestorePointCount.ForeColor = Color.FromArgb(96, 96, 96);
			txtBackupRestorePointCount.Location = new Point(30, 0);
			txtBackupRestorePointCount.Margin = new Padding(0);
			txtBackupRestorePointCount.Name = "txtBackupRestorePointCount";
			txtBackupRestorePointCount.Size = new Size(0, 17);
			txtBackupRestorePointCount.TabIndex = 4;
			// 
			// txtRestorePointPrompt
			// 
			txtRestorePointPrompt.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			txtRestorePointPrompt.Location = new Point(3, 28);
			txtRestorePointPrompt.Margin = new Padding(3, 8, 3, 6);
			txtRestorePointPrompt.Name = "txtRestorePointPrompt";
			txtRestorePointPrompt.Size = new Size(147, 6);
			txtRestorePointPrompt.TabIndex = 1;
			txtRestorePointPrompt.Text = "Select Restore Point:";
			// 
			// lstRestorePoints
			// 
			lstRestorePoints.Dock = DockStyle.Fill;
			lstRestorePoints.DrawMode = DrawMode.OwnerDrawFixed;
			lstRestorePoints.FormattingEnabled = true;
			lstRestorePoints.IntegralHeight = false;
			lstRestorePoints.ItemHeight = 36;
			lstRestorePoints.Location = new Point(3, 43);
			lstRestorePoints.Name = "lstRestorePoints";
			lstRestorePoints.Size = new Size(494, 94);
			lstRestorePoints.TabIndex = 2;
			lstRestorePoints.DrawItem += lstRestorePoints_DrawItem;
			lstRestorePoints.SelectedIndexChanged += RestorePoints_SelectionChanged;
			// 
			// txtPreselectedRestorePointSummary
			// 
			txtPreselectedRestorePointSummary.Dock = DockStyle.Fill;
			txtPreselectedRestorePointSummary.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			txtPreselectedRestorePointSummary.Location = new Point(3, 140);
			txtPreselectedRestorePointSummary.Name = "txtPreselectedRestorePointSummary";
			txtPreselectedRestorePointSummary.Size = new Size(494, 20);
			txtPreselectedRestorePointSummary.TabIndex = 3;
			txtPreselectedRestorePointSummary.Visible = false;
			// 
			// grpRestoreOptions
			// 
			grpRestoreOptions.Controls.Add(pnlOptionsScroll);
			grpRestoreOptions.Dock = DockStyle.Fill;
			grpRestoreOptions.Enabled = false;
			grpRestoreOptions.Location = new Point(3, 132);
			grpRestoreOptions.Name = "grpRestoreOptions";
			grpRestoreOptions.Padding = new Padding(10, 11, 10, 11);
			grpRestoreOptions.Size = new Size(520, 703);
			grpRestoreOptions.TabIndex = 1;
			grpRestoreOptions.TabStop = false;
			grpRestoreOptions.Text = "Restore Options";
			// 
			// pnlOptionsScroll
			// 
			pnlOptionsScroll.AutoScroll = true;
			pnlOptionsScroll.Controls.Add(tlpOptions);
			pnlOptionsScroll.Dock = DockStyle.Fill;
			pnlOptionsScroll.Location = new Point(10, 29);
			pnlOptionsScroll.Name = "pnlOptionsScroll";
			pnlOptionsScroll.Size = new Size(500, 663);
			pnlOptionsScroll.TabIndex = 0;
			// 
			// tlpOptions
			// 
			tlpOptions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			tlpOptions.ColumnCount = 1;
			tlpOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpOptions.Controls.Add(txtWhatToRestoreLabel, 0, 0);
			tlpOptions.Controls.Add(txtPreselectedScopeSummary, 0, 1);
			tlpOptions.Controls.Add(rbRestoreAll, 0, 2);
			tlpOptions.Controls.Add(rbRestoreSelected, 0, 3);
			tlpOptions.Controls.Add(pnlItemSelection, 0, 4);
			tlpOptions.Controls.Add(lblRestoreDestination, 0, 5);
			tlpOptions.Controls.Add(txtDestinationHelp, 0, 6);
			tlpOptions.Controls.Add(pnlHyperVRestoreMode, 0, 7);
			tlpOptions.Controls.Add(pnlHyperVVmOptions, 0, 8);
			tlpOptions.Controls.Add(chkRestoreToHyperVDisk, 0, 9);
			tlpOptions.Controls.Add(pnlRegularHyperVRestore, 0, 10);
			tlpOptions.Controls.Add(pnlLocationChoice, 0, 11);
			tlpOptions.Controls.Add(pnlHyperVCloneDestination, 0, 12);
			tlpOptions.Controls.Add(chkPreservePermissions, 0, 14);
			tlpOptions.Controls.Add(chkVerifyAfterRestore, 0, 15);
			tlpOptions.Controls.Add(chkOverwrite, 0, 13);
			tlpOptions.Location = new Point(0, 0);
			tlpOptions.Name = "tlpOptions";
			tlpOptions.RowCount = 16;
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 213F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
			tlpOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
			tlpOptions.Size = new Size(483, 663);
			tlpOptions.TabIndex = 0;
			// 
			// txtWhatToRestoreLabel
			// 
			txtWhatToRestoreLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			txtWhatToRestoreLabel.Location = new Point(3, 0);
			txtWhatToRestoreLabel.Name = "txtWhatToRestoreLabel";
			txtWhatToRestoreLabel.Size = new Size(121, 19);
			txtWhatToRestoreLabel.TabIndex = 0;
			txtWhatToRestoreLabel.Text = "What to Restore:";
			// 
			// txtPreselectedScopeSummary
			// 
			txtPreselectedScopeSummary.Dock = DockStyle.Fill;
			txtPreselectedScopeSummary.ForeColor = Color.FromArgb(96, 96, 96);
			txtPreselectedScopeSummary.Location = new Point(3, 20);
			txtPreselectedScopeSummary.Name = "txtPreselectedScopeSummary";
			txtPreselectedScopeSummary.Size = new Size(477, 8);
			txtPreselectedScopeSummary.TabIndex = 1;
			txtPreselectedScopeSummary.Visible = false;
			// 
			// rbRestoreAll
			// 
			rbRestoreAll.Checked = true;
			rbRestoreAll.Location = new Point(3, 31);
			rbRestoreAll.Name = "rbRestoreAll";
			rbRestoreAll.Size = new Size(213, 26);
			rbRestoreAll.TabIndex = 2;
			rbRestoreAll.TabStop = true;
			rbRestoreAll.Text = "Restore everything from backup";
			rbRestoreAll.UseVisualStyleBackColor = true;
			// 
			// rbRestoreSelected
			// 
			rbRestoreSelected.Location = new Point(3, 63);
			rbRestoreSelected.Name = "rbRestoreSelected";
			rbRestoreSelected.Size = new Size(204, 14);
			rbRestoreSelected.TabIndex = 3;
			rbRestoreSelected.Text = "Select specific items to restore";
			rbRestoreSelected.UseVisualStyleBackColor = true;
			// 
			// pnlItemSelection
			// 
			pnlItemSelection.Controls.Add(lstBackupItems);
			pnlItemSelection.Dock = DockStyle.Fill;
			pnlItemSelection.Location = new Point(3, 83);
			pnlItemSelection.Name = "pnlItemSelection";
			pnlItemSelection.Padding = new Padding(0, 0, 0, 11);
			pnlItemSelection.Size = new Size(477, 14);
			pnlItemSelection.TabIndex = 4;
			pnlItemSelection.Visible = false;
			// 
			// lstBackupItems
			// 
			lstBackupItems.Dock = DockStyle.Fill;
			lstBackupItems.HorizontalScrollbar = true;
			lstBackupItems.IntegralHeight = false;
			lstBackupItems.ItemHeight = 17;
			lstBackupItems.Location = new Point(0, 0);
			lstBackupItems.Name = "lstBackupItems";
			lstBackupItems.SelectionMode = SelectionMode.MultiSimple;
			lstBackupItems.Size = new Size(477, 3);
			lstBackupItems.TabIndex = 0;
			lstBackupItems.SelectedIndexChanged += RestoreItems_SelectionChanged;
			// 
			// lblRestoreDestination
			// 
			lblRestoreDestination.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblRestoreDestination.Location = new Point(3, 109);
			lblRestoreDestination.Margin = new Padding(3, 9, 3, 3);
			lblRestoreDestination.Name = "lblRestoreDestination";
			lblRestoreDestination.Size = new Size(143, 8);
			lblRestoreDestination.TabIndex = 5;
			lblRestoreDestination.Text = "Restore Destination:";
			// 
			// txtDestinationHelp
			// 
			txtDestinationHelp.Dock = DockStyle.Fill;
			txtDestinationHelp.ForeColor = Color.FromArgb(96, 96, 96);
			txtDestinationHelp.Location = new Point(3, 120);
			txtDestinationHelp.Name = "txtDestinationHelp";
			txtDestinationHelp.Size = new Size(477, 20);
			txtDestinationHelp.TabIndex = 6;
			txtDestinationHelp.Text = "Select a target on the right to specify where the restore should be written.";
			// 
			// pnlHyperVRestoreMode
			// 
			pnlHyperVRestoreMode.ColumnCount = 1;
			pnlHyperVRestoreMode.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlHyperVRestoreMode.Controls.Add(lblHyperVRestoreTarget, 0, 0);
			pnlHyperVRestoreMode.Controls.Add(cmbHyperVRestoreTarget, 0, 1);
			pnlHyperVRestoreMode.Dock = DockStyle.Fill;
			pnlHyperVRestoreMode.Location = new Point(3, 143);
			pnlHyperVRestoreMode.Name = "pnlHyperVRestoreMode";
			pnlHyperVRestoreMode.RowCount = 2;
			pnlHyperVRestoreMode.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVRestoreMode.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVRestoreMode.Size = new Size(477, 14);
			pnlHyperVRestoreMode.TabIndex = 7;
			pnlHyperVRestoreMode.Visible = false;
			// 
			// lblHyperVRestoreTarget
			// 
			lblHyperVRestoreTarget.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblHyperVRestoreTarget.Location = new Point(3, 0);
			lblHyperVRestoreTarget.Name = "lblHyperVRestoreTarget";
			lblHyperVRestoreTarget.Size = new Size(171, 19);
			lblHyperVRestoreTarget.TabIndex = 0;
			lblHyperVRestoreTarget.Text = "Hyper-V Restore Target:";
			// 
			// cmbHyperVRestoreTarget
			// 
			cmbHyperVRestoreTarget.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbHyperVRestoreTarget.Items.AddRange(new object[] { "Restore exported files/folders", "Restore guest disk to volume", "Restore guest disk to disk", "Restore as Hyper-V virtual machine" });
			cmbHyperVRestoreTarget.Location = new Point(3, 23);
			cmbHyperVRestoreTarget.Name = "cmbHyperVRestoreTarget";
			cmbHyperVRestoreTarget.Size = new Size(48, 25);
			cmbHyperVRestoreTarget.TabIndex = 1;
			cmbHyperVRestoreTarget.SelectedIndexChanged += HyperVRestoreTarget_Changed;
			// 
			// pnlHyperVVmOptions
			// 
			pnlHyperVVmOptions.ColumnCount = 1;
			pnlHyperVVmOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlHyperVVmOptions.Controls.Add(lblRestoreMode, 0, 0);
			pnlHyperVVmOptions.Controls.Add(rbHyperVReplaceExisting, 0, 1);
			pnlHyperVVmOptions.Controls.Add(rbHyperVRestoreToDirectory, 0, 2);
			pnlHyperVVmOptions.Controls.Add(pnlHyperVReplaceExistingOptions, 0, 3);
			pnlHyperVVmOptions.Controls.Add(pnlHyperVDirectoryOptions, 0, 4);
			pnlHyperVVmOptions.Controls.Add(lblVmNameAfter, 0, 5);
			pnlHyperVVmOptions.Controls.Add(txtHyperVVmName, 0, 6);
			pnlHyperVVmOptions.Controls.Add(chkStartHyperVVm, 0, 7);
			pnlHyperVVmOptions.Dock = DockStyle.Fill;
			pnlHyperVVmOptions.Location = new Point(3, 163);
			pnlHyperVVmOptions.Name = "pnlHyperVVmOptions";
			pnlHyperVVmOptions.RowCount = 8;
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVVmOptions.Size = new Size(477, 14);
			pnlHyperVVmOptions.TabIndex = 8;
			pnlHyperVVmOptions.Visible = false;
			// 
			// lblRestoreMode
			// 
			lblRestoreMode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblRestoreMode.Location = new Point(3, 9);
			lblRestoreMode.Margin = new Padding(3, 9, 3, 3);
			lblRestoreMode.Name = "lblRestoreMode";
			lblRestoreMode.Size = new Size(107, 8);
			lblRestoreMode.TabIndex = 0;
			lblRestoreMode.Text = "Restore Mode:";
			// 
			// rbHyperVReplaceExisting
			// 
			rbHyperVReplaceExisting.Checked = true;
			rbHyperVReplaceExisting.Location = new Point(3, 23);
			rbHyperVReplaceExisting.Name = "rbHyperVReplaceExisting";
			rbHyperVReplaceExisting.Size = new Size(301, 14);
			rbHyperVReplaceExisting.TabIndex = 1;
			rbHyperVReplaceExisting.TabStop = true;
			rbHyperVReplaceExisting.Text = "Replace a non-running Hyper-V virtual machine";
			rbHyperVReplaceExisting.UseVisualStyleBackColor = true;
			rbHyperVReplaceExisting.CheckedChanged += HyperVVmRestoreMode_Changed;
			// 
			// rbHyperVRestoreToDirectory
			// 
			rbHyperVRestoreToDirectory.Location = new Point(3, 43);
			rbHyperVRestoreToDirectory.Name = "rbHyperVRestoreToDirectory";
			rbHyperVRestoreToDirectory.Size = new Size(201, 14);
			rbHyperVRestoreToDirectory.TabIndex = 2;
			rbHyperVRestoreToDirectory.Text = "Restore to an empty directory";
			rbHyperVRestoreToDirectory.UseVisualStyleBackColor = true;
			rbHyperVRestoreToDirectory.CheckedChanged += HyperVVmRestoreMode_Changed;
			// 
			// pnlHyperVReplaceExistingOptions
			// 
			pnlHyperVReplaceExistingOptions.ColumnCount = 1;
			pnlHyperVReplaceExistingOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlHyperVReplaceExistingOptions.Controls.Add(lblReplaceVm, 0, 0);
			pnlHyperVReplaceExistingOptions.Controls.Add(cmbHyperVVmToReplace, 0, 1);
			pnlHyperVReplaceExistingOptions.Dock = DockStyle.Fill;
			pnlHyperVReplaceExistingOptions.Location = new Point(20, 60);
			pnlHyperVReplaceExistingOptions.Margin = new Padding(20, 0, 0, 9);
			pnlHyperVReplaceExistingOptions.Name = "pnlHyperVReplaceExistingOptions";
			pnlHyperVReplaceExistingOptions.RowCount = 2;
			pnlHyperVReplaceExistingOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVReplaceExistingOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVReplaceExistingOptions.Size = new Size(457, 11);
			pnlHyperVReplaceExistingOptions.TabIndex = 3;
			// 
			// lblReplaceVm
			// 
			lblReplaceVm.Location = new Point(3, 0);
			lblReplaceVm.Name = "lblReplaceVm";
			lblReplaceVm.Size = new Size(285, 17);
			lblReplaceVm.TabIndex = 0;
			lblReplaceVm.Text = "Select a non-running virtual machine to replace:";
			// 
			// cmbHyperVVmToReplace
			// 
			cmbHyperVVmToReplace.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbHyperVVmToReplace.Location = new Point(3, 23);
			cmbHyperVVmToReplace.Name = "cmbHyperVVmToReplace";
			cmbHyperVVmToReplace.Size = new Size(28, 25);
			cmbHyperVVmToReplace.TabIndex = 1;
			// 
			// pnlHyperVDirectoryOptions
			// 
			pnlHyperVDirectoryOptions.ColumnCount = 1;
			pnlHyperVDirectoryOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlHyperVDirectoryOptions.Controls.Add(lblRestoreDir, 0, 0);
			pnlHyperVDirectoryOptions.Controls.Add(tlpHyperVDirRow, 0, 1);
			pnlHyperVDirectoryOptions.Dock = DockStyle.Fill;
			pnlHyperVDirectoryOptions.Location = new Point(20, 80);
			pnlHyperVDirectoryOptions.Margin = new Padding(20, 0, 0, 9);
			pnlHyperVDirectoryOptions.Name = "pnlHyperVDirectoryOptions";
			pnlHyperVDirectoryOptions.RowCount = 2;
			pnlHyperVDirectoryOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVDirectoryOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVDirectoryOptions.Size = new Size(457, 11);
			pnlHyperVDirectoryOptions.TabIndex = 4;
			pnlHyperVDirectoryOptions.Visible = false;
			// 
			// lblRestoreDir
			// 
			lblRestoreDir.Location = new Point(3, 0);
			lblRestoreDir.Name = "lblRestoreDir";
			lblRestoreDir.Size = new Size(279, 17);
			lblRestoreDir.TabIndex = 0;
			lblRestoreDir.Text = "Restore destination directory (must be empty):";
			// 
			// tlpHyperVDirRow
			// 
			tlpHyperVDirRow.ColumnCount = 2;
			tlpHyperVDirRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpHyperVDirRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
			tlpHyperVDirRow.Controls.Add(txtHyperVRestoreDirectory, 0, 0);
			tlpHyperVDirRow.Controls.Add(btnBrowseHyperVDir, 1, 0);
			tlpHyperVDirRow.Dock = DockStyle.Fill;
			tlpHyperVDirRow.Location = new Point(3, 23);
			tlpHyperVDirRow.Name = "tlpHyperVDirRow";
			tlpHyperVDirRow.RowCount = 1;
			tlpHyperVDirRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpHyperVDirRow.Size = new Size(451, 14);
			tlpHyperVDirRow.TabIndex = 1;
			// 
			// txtHyperVRestoreDirectory
			// 
			txtHyperVRestoreDirectory.Dock = DockStyle.Fill;
			txtHyperVRestoreDirectory.Location = new Point(3, 3);
			txtHyperVRestoreDirectory.Name = "txtHyperVRestoreDirectory";
			txtHyperVRestoreDirectory.Size = new Size(365, 25);
			txtHyperVRestoreDirectory.TabIndex = 0;
			// 
			// btnBrowseHyperVDir
			// 
			btnBrowseHyperVDir.Dock = DockStyle.Fill;
			btnBrowseHyperVDir.Location = new Point(374, 3);
			btnBrowseHyperVDir.Name = "btnBrowseHyperVDir";
			btnBrowseHyperVDir.Size = new Size(74, 14);
			btnBrowseHyperVDir.TabIndex = 1;
			btnBrowseHyperVDir.Text = "Browse...";
			btnBrowseHyperVDir.UseVisualStyleBackColor = true;
			btnBrowseHyperVDir.Click += BrowseHyperVRestoreDirectory_Click;
			// 
			// lblVmNameAfter
			// 
			lblVmNameAfter.Location = new Point(3, 100);
			lblVmNameAfter.Name = "lblVmNameAfter";
			lblVmNameAfter.Size = new Size(213, 17);
			lblVmNameAfter.TabIndex = 5;
			lblVmNameAfter.Text = "Virtual machine name after restore:";
			// 
			// txtHyperVVmName
			// 
			txtHyperVVmName.Dock = DockStyle.Fill;
			txtHyperVVmName.Location = new Point(3, 123);
			txtHyperVVmName.Name = "txtHyperVVmName";
			txtHyperVVmName.Size = new Size(471, 25);
			txtHyperVVmName.TabIndex = 6;
			// 
			// chkStartHyperVVm
			// 
			chkStartHyperVVm.Location = new Point(3, 143);
			chkStartHyperVVm.Name = "chkStartHyperVVm";
			chkStartHyperVVm.Size = new Size(244, 14);
			chkStartHyperVVm.TabIndex = 7;
			chkStartHyperVVm.Text = "Start the virtual machine after restore";
			chkStartHyperVVm.UseVisualStyleBackColor = true;
			// 
			// chkRestoreToHyperVDisk
			// 
			chkRestoreToHyperVDisk.Location = new Point(3, 183);
			chkRestoreToHyperVDisk.Name = "chkRestoreToHyperVDisk";
			chkRestoreToHyperVDisk.Size = new Size(269, 14);
			chkRestoreToHyperVDisk.TabIndex = 9;
			chkRestoreToHyperVDisk.Text = "Restore into a Hyper-V virtual disk (.vhdx)";
			chkRestoreToHyperVDisk.UseVisualStyleBackColor = true;
			chkRestoreToHyperVDisk.Visible = false;
			chkRestoreToHyperVDisk.CheckedChanged += RegularHyperVRestoreOption_Changed;
			// 
			// pnlRegularHyperVRestore
			// 
			pnlRegularHyperVRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			pnlRegularHyperVRestore.ColumnCount = 1;
			pnlRegularHyperVRestore.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlRegularHyperVRestore.Controls.Add(tlpHyperVDiskRow, 0, 1);
			pnlRegularHyperVRestore.Controls.Add(lblAfterRestore, 0, 2);
			pnlRegularHyperVRestore.Controls.Add(rbLeaveHyperVDiskDetached, 0, 3);
			pnlRegularHyperVRestore.Controls.Add(chkAttachToExistingHyperVVm, 0, 4);
			pnlRegularHyperVRestore.Controls.Add(rbCreateNewHyperVVm, 0, 5);
			pnlRegularHyperVRestore.Controls.Add(pnlExistingHyperVVmOptions, 0, 6);
			pnlRegularHyperVRestore.Controls.Add(pnlNewHyperVVmOptions, 0, 7);
			pnlRegularHyperVRestore.Controls.Add(lblVhdPath, 0, 0);
			pnlRegularHyperVRestore.Location = new Point(3, 211);
			pnlRegularHyperVRestore.Name = "pnlRegularHyperVRestore";
			pnlRegularHyperVRestore.RowCount = 8;
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlRegularHyperVRestore.Size = new Size(477, 31);
			pnlRegularHyperVRestore.TabIndex = 10;
			pnlRegularHyperVRestore.Visible = false;
			// 
			// lblVhdPath
			// 
			lblVhdPath.Location = new Point(3, 0);
			lblVhdPath.Name = "lblVhdPath";
			lblVhdPath.Size = new Size(146, 17);
			lblVhdPath.TabIndex = 0;
			lblVhdPath.Text = "Hyper-V virtual disk file:";
			// 
			// tlpHyperVDiskRow
			// 
			tlpHyperVDiskRow.ColumnCount = 2;
			tlpHyperVDiskRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpHyperVDiskRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
			tlpHyperVDiskRow.Controls.Add(txtHyperVVirtualDiskPath, 0, 0);
			tlpHyperVDiskRow.Controls.Add(btnBrowseHyperVDisk, 1, 0);
			tlpHyperVDiskRow.Dock = DockStyle.Fill;
			tlpHyperVDiskRow.Location = new Point(3, 23);
			tlpHyperVDiskRow.Name = "tlpHyperVDiskRow";
			tlpHyperVDiskRow.RowCount = 1;
			tlpHyperVDiskRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpHyperVDiskRow.Size = new Size(471, 14);
			tlpHyperVDiskRow.TabIndex = 1;
			// 
			// txtHyperVVirtualDiskPath
			// 
			txtHyperVVirtualDiskPath.Dock = DockStyle.Fill;
			txtHyperVVirtualDiskPath.Location = new Point(3, 3);
			txtHyperVVirtualDiskPath.Name = "txtHyperVVirtualDiskPath";
			txtHyperVVirtualDiskPath.Size = new Size(385, 25);
			txtHyperVVirtualDiskPath.TabIndex = 0;
			// 
			// btnBrowseHyperVDisk
			// 
			btnBrowseHyperVDisk.Dock = DockStyle.Fill;
			btnBrowseHyperVDisk.Location = new Point(394, 3);
			btnBrowseHyperVDisk.Name = "btnBrowseHyperVDisk";
			btnBrowseHyperVDisk.Size = new Size(74, 14);
			btnBrowseHyperVDisk.TabIndex = 1;
			btnBrowseHyperVDisk.Text = "Browse...";
			btnBrowseHyperVDisk.UseVisualStyleBackColor = true;
			btnBrowseHyperVDisk.Click += BrowseHyperVVirtualDisk_Click;
			// 
			// lblAfterRestore
			// 
			lblAfterRestore.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblAfterRestore.Location = new Point(3, 49);
			lblAfterRestore.Margin = new Padding(3, 9, 3, 3);
			lblAfterRestore.Name = "lblAfterRestore";
			lblAfterRestore.Size = new Size(99, 8);
			lblAfterRestore.TabIndex = 2;
			lblAfterRestore.Text = "After restore:";
			// 
			// rbLeaveHyperVDiskDetached
			// 
			rbLeaveHyperVDiskDetached.Checked = true;
			rbLeaveHyperVDiskDetached.Location = new Point(3, 63);
			rbLeaveHyperVDiskDetached.Name = "rbLeaveHyperVDiskDetached";
			rbLeaveHyperVDiskDetached.Size = new Size(259, 14);
			rbLeaveHyperVDiskDetached.TabIndex = 3;
			rbLeaveHyperVDiskDetached.TabStop = true;
			rbLeaveHyperVDiskDetached.Text = "Leave the restored virtual disk detached";
			rbLeaveHyperVDiskDetached.UseVisualStyleBackColor = true;
			rbLeaveHyperVDiskDetached.CheckedChanged += ExistingHyperVVmAttach_Changed;
			// 
			// chkAttachToExistingHyperVVm
			// 
			chkAttachToExistingHyperVVm.Location = new Point(3, 83);
			chkAttachToExistingHyperVVm.Name = "chkAttachToExistingHyperVVm";
			chkAttachToExistingHyperVVm.Size = new Size(364, 14);
			chkAttachToExistingHyperVVm.TabIndex = 4;
			chkAttachToExistingHyperVVm.Text = "Attach to an existing Hyper-V virtual machine after restore";
			chkAttachToExistingHyperVVm.UseVisualStyleBackColor = true;
			chkAttachToExistingHyperVVm.CheckedChanged += ExistingHyperVVmAttach_Changed;
			// 
			// rbCreateNewHyperVVm
			// 
			rbCreateNewHyperVVm.Location = new Point(3, 103);
			rbCreateNewHyperVVm.Name = "rbCreateNewHyperVVm";
			rbCreateNewHyperVVm.Size = new Size(422, 14);
			rbCreateNewHyperVVm.TabIndex = 5;
			rbCreateNewHyperVVm.Text = "Create a new Hyper-V virtual machine using the restored virtual disk";
			rbCreateNewHyperVVm.UseVisualStyleBackColor = true;
			rbCreateNewHyperVVm.CheckedChanged += ExistingHyperVVmAttach_Changed;
			// 
			// pnlExistingHyperVVmOptions
			// 
			pnlExistingHyperVVmOptions.ColumnCount = 1;
			pnlExistingHyperVVmOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlExistingHyperVVmOptions.Controls.Add(lblExistingVm, 0, 0);
			pnlExistingHyperVVmOptions.Controls.Add(cmbExistingHyperVVm, 0, 1);
			pnlExistingHyperVVmOptions.Dock = DockStyle.Fill;
			pnlExistingHyperVVmOptions.Location = new Point(3, 123);
			pnlExistingHyperVVmOptions.Name = "pnlExistingHyperVVmOptions";
			pnlExistingHyperVVmOptions.RowCount = 2;
			pnlExistingHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlExistingHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlExistingHyperVVmOptions.Size = new Size(471, 14);
			pnlExistingHyperVVmOptions.TabIndex = 6;
			pnlExistingHyperVVmOptions.Visible = false;
			// 
			// lblExistingVm
			// 
			lblExistingVm.Location = new Point(3, 0);
			lblExistingVm.Name = "lblExistingVm";
			lblExistingVm.Size = new Size(198, 17);
			lblExistingVm.TabIndex = 0;
			lblExistingVm.Text = "Existing Hyper-V virtual machine:";
			// 
			// cmbExistingHyperVVm
			// 
			cmbExistingHyperVVm.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbExistingHyperVVm.Location = new Point(3, 23);
			cmbExistingHyperVVm.Name = "cmbExistingHyperVVm";
			cmbExistingHyperVVm.Size = new Size(42, 25);
			cmbExistingHyperVVm.TabIndex = 1;
			// 
			// pnlNewHyperVVmOptions
			// 
			pnlNewHyperVVmOptions.ColumnCount = 1;
			pnlNewHyperVVmOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlNewHyperVVmOptions.Controls.Add(lblNewVmName, 0, 0);
			pnlNewHyperVVmOptions.Controls.Add(txtNewHyperVVmName, 0, 1);
			pnlNewHyperVVmOptions.Controls.Add(lblNewVmFolder, 0, 2);
			pnlNewHyperVVmOptions.Controls.Add(tlpNewVmPathRow, 0, 3);
			pnlNewHyperVVmOptions.Controls.Add(lblNewVmGen, 0, 4);
			pnlNewHyperVVmOptions.Controls.Add(cmbNewHyperVGeneration, 0, 5);
			pnlNewHyperVVmOptions.Controls.Add(chkStartCreatedHyperVVm, 0, 6);
			pnlNewHyperVVmOptions.Dock = DockStyle.Fill;
			pnlNewHyperVVmOptions.Location = new Point(3, 143);
			pnlNewHyperVVmOptions.Name = "pnlNewHyperVVmOptions";
			pnlNewHyperVVmOptions.RowCount = 7;
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlNewHyperVVmOptions.Size = new Size(471, 14);
			pnlNewHyperVVmOptions.TabIndex = 7;
			pnlNewHyperVVmOptions.Visible = false;
			// 
			// lblNewVmName
			// 
			lblNewVmName.Location = new Point(3, 0);
			lblNewVmName.Name = "lblNewVmName";
			lblNewVmName.Size = new Size(216, 17);
			lblNewVmName.TabIndex = 0;
			lblNewVmName.Text = "New Hyper-V virtual machine name:";
			// 
			// txtNewHyperVVmName
			// 
			txtNewHyperVVmName.Dock = DockStyle.Fill;
			txtNewHyperVVmName.Location = new Point(3, 23);
			txtNewHyperVVmName.Name = "txtNewHyperVVmName";
			txtNewHyperVVmName.Size = new Size(465, 25);
			txtNewHyperVVmName.TabIndex = 1;
			// 
			// lblNewVmFolder
			// 
			lblNewVmFolder.Location = new Point(3, 40);
			lblNewVmFolder.Name = "lblNewVmFolder";
			lblNewVmFolder.Size = new Size(188, 17);
			lblNewVmFolder.TabIndex = 2;
			lblNewVmFolder.Text = "Virtual machine storage folder:";
			// 
			// tlpNewVmPathRow
			// 
			tlpNewVmPathRow.ColumnCount = 2;
			tlpNewVmPathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpNewVmPathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
			tlpNewVmPathRow.Controls.Add(txtNewHyperVVmPath, 0, 0);
			tlpNewVmPathRow.Controls.Add(btnBrowseNewVmPath, 1, 0);
			tlpNewVmPathRow.Dock = DockStyle.Fill;
			tlpNewVmPathRow.Location = new Point(3, 63);
			tlpNewVmPathRow.Name = "tlpNewVmPathRow";
			tlpNewVmPathRow.RowCount = 1;
			tlpNewVmPathRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpNewVmPathRow.Size = new Size(465, 14);
			tlpNewVmPathRow.TabIndex = 3;
			// 
			// txtNewHyperVVmPath
			// 
			txtNewHyperVVmPath.Dock = DockStyle.Fill;
			txtNewHyperVVmPath.Location = new Point(3, 3);
			txtNewHyperVVmPath.Name = "txtNewHyperVVmPath";
			txtNewHyperVVmPath.Size = new Size(379, 25);
			txtNewHyperVVmPath.TabIndex = 0;
			// 
			// btnBrowseNewVmPath
			// 
			btnBrowseNewVmPath.Dock = DockStyle.Fill;
			btnBrowseNewVmPath.Location = new Point(388, 3);
			btnBrowseNewVmPath.Name = "btnBrowseNewVmPath";
			btnBrowseNewVmPath.Size = new Size(74, 14);
			btnBrowseNewVmPath.TabIndex = 1;
			btnBrowseNewVmPath.Text = "Browse...";
			btnBrowseNewVmPath.UseVisualStyleBackColor = true;
			btnBrowseNewVmPath.Click += BrowseNewHyperVVmLocation_Click;
			// 
			// lblNewVmGen
			// 
			lblNewVmGen.Location = new Point(3, 80);
			lblNewVmGen.Name = "lblNewVmGen";
			lblNewVmGen.Size = new Size(167, 17);
			lblNewVmGen.TabIndex = 4;
			lblNewVmGen.Text = "Virtual machine generation:";
			// 
			// cmbNewHyperVGeneration
			// 
			cmbNewHyperVGeneration.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbNewHyperVGeneration.Items.AddRange(new object[] { "Generation 1", "Generation 2" });
			cmbNewHyperVGeneration.Location = new Point(3, 103);
			cmbNewHyperVGeneration.Name = "cmbNewHyperVGeneration";
			cmbNewHyperVGeneration.Size = new Size(42, 25);
			cmbNewHyperVGeneration.TabIndex = 5;
			// 
			// chkStartCreatedHyperVVm
			// 
			chkStartCreatedHyperVVm.Checked = true;
			chkStartCreatedHyperVVm.CheckState = CheckState.Checked;
			chkStartCreatedHyperVVm.Location = new Point(3, 123);
			chkStartCreatedHyperVVm.Name = "chkStartCreatedHyperVVm";
			chkStartCreatedHyperVVm.Size = new Size(271, 14);
			chkStartCreatedHyperVVm.TabIndex = 6;
			chkStartCreatedHyperVVm.Text = "Start the new virtual machine after restore";
			chkStartCreatedHyperVVm.UseVisualStyleBackColor = true;
			// 
			// pnlLocationChoice
			// 
			pnlLocationChoice.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			pnlLocationChoice.ColumnCount = 1;
			pnlLocationChoice.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlLocationChoice.Controls.Add(lblFileFolderTarget, 0, 0);
			pnlLocationChoice.Controls.Add(btnBrowseRestoreDestination, 0, 1);
			pnlLocationChoice.Controls.Add(lblTargetLocation, 0, 2);
			pnlLocationChoice.Controls.Add(txtFolderRestoreDestination, 0, 3);
			pnlLocationChoice.Location = new Point(3, 248);
			pnlLocationChoice.Name = "pnlLocationChoice";
			pnlLocationChoice.RowCount = 4;
			pnlLocationChoice.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			pnlLocationChoice.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
			pnlLocationChoice.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
			pnlLocationChoice.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
			pnlLocationChoice.Size = new Size(477, 109);
			pnlLocationChoice.TabIndex = 11;
			pnlLocationChoice.Visible = false;
			// 
			// lblFileFolderTarget
			// 
			lblFileFolderTarget.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblFileFolderTarget.Location = new Point(3, 0);
			lblFileFolderTarget.Name = "lblFileFolderTarget";
			lblFileFolderTarget.Size = new Size(183, 19);
			lblFileFolderTarget.TabIndex = 0;
			lblFileFolderTarget.Text = "File/Folder Restore Target";
			// 
			// btnBrowseRestoreDestination
			// 
			btnBrowseRestoreDestination.Location = new Point(3, 33);
			btnBrowseRestoreDestination.Name = "btnBrowseRestoreDestination";
			btnBrowseRestoreDestination.Size = new Size(72, 25);
			btnBrowseRestoreDestination.TabIndex = 1;
			btnBrowseRestoreDestination.Text = "Browse...";
			btnBrowseRestoreDestination.UseVisualStyleBackColor = true;
			btnBrowseRestoreDestination.Click += BrowseRestoreDestination_Click;
			// 
			// lblTargetLocation
			// 
			lblTargetLocation.Location = new Point(3, 61);
			lblTargetLocation.Name = "lblTargetLocation";
			lblTargetLocation.Size = new Size(98, 17);
			lblTargetLocation.TabIndex = 2;
			lblTargetLocation.Text = "Target Location";
			// 
			// txtFolderRestoreDestination
			// 
			txtFolderRestoreDestination.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			txtFolderRestoreDestination.Location = new Point(3, 87);
			txtFolderRestoreDestination.Name = "txtFolderRestoreDestination";
			txtFolderRestoreDestination.Size = new Size(471, 25);
			txtFolderRestoreDestination.TabIndex = 3;
			txtFolderRestoreDestination.TextChanged += TargetLocation_TextChanged;
			// 
			// pnlHyperVCloneDestination
			// 
			pnlHyperVCloneDestination.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			pnlHyperVCloneDestination.ColumnCount = 1;
			pnlHyperVCloneDestination.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			pnlHyperVCloneDestination.Controls.Add(rbHyperVCloneDefault, 0, 1);
			pnlHyperVCloneDestination.Controls.Add(rbHyperVCloneAlternate, 0, 2);
			pnlHyperVCloneDestination.Controls.Add(pnlHyperVCloneAlternate, 0, 3);
			pnlHyperVCloneDestination.Controls.Add(lblCloneDestination, 0, 0);
			pnlHyperVCloneDestination.Location = new Point(3, 363);
			pnlHyperVCloneDestination.Name = "pnlHyperVCloneDestination";
			pnlHyperVCloneDestination.RowCount = 4;
			pnlHyperVCloneDestination.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
			pnlHyperVCloneDestination.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
			pnlHyperVCloneDestination.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
			pnlHyperVCloneDestination.RowStyles.Add(new RowStyle(SizeType.Absolute, 111F));
			pnlHyperVCloneDestination.Size = new Size(477, 207);
			pnlHyperVCloneDestination.TabIndex = 12;
			pnlHyperVCloneDestination.Visible = false;
			// 
			// lblCloneDestination
			// 
			lblCloneDestination.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lblCloneDestination.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblCloneDestination.Location = new Point(3, 12);
			lblCloneDestination.Name = "lblCloneDestination";
			lblCloneDestination.Size = new Size(143, 19);
			lblCloneDestination.TabIndex = 0;
			lblCloneDestination.Text = "Restore Destination:";
			// 
			// rbHyperVCloneDefault
			// 
			rbHyperVCloneDefault.Checked = true;
			rbHyperVCloneDefault.Location = new Point(3, 34);
			rbHyperVCloneDefault.Name = "rbHyperVCloneDefault";
			rbHyperVCloneDefault.Size = new Size(243, 21);
			rbHyperVCloneDefault.TabIndex = 1;
			rbHyperVCloneDefault.TabStop = true;
			rbHyperVCloneDefault.Text = "Use Hyper-V default storage location";
			rbHyperVCloneDefault.UseVisualStyleBackColor = true;
			rbHyperVCloneDefault.CheckedChanged += HyperVCloneDestination_Changed;
			// 
			// rbHyperVCloneAlternate
			// 
			rbHyperVCloneAlternate.Location = new Point(3, 66);
			rbHyperVCloneAlternate.Name = "rbHyperVCloneAlternate";
			rbHyperVCloneAlternate.Size = new Size(128, 18);
			rbHyperVCloneAlternate.TabIndex = 2;
			rbHyperVCloneAlternate.Text = "Alternate location";
			rbHyperVCloneAlternate.UseVisualStyleBackColor = true;
			rbHyperVCloneAlternate.CheckedChanged += HyperVCloneDestination_Changed;
			// 
			// pnlHyperVCloneAlternate
			// 
			pnlHyperVCloneAlternate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			pnlHyperVCloneAlternate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			pnlHyperVCloneAlternate.ColumnCount = 1;
			pnlHyperVCloneAlternate.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 467F));
			pnlHyperVCloneAlternate.Controls.Add(lblCloneVmFolder, 0, 0);
			pnlHyperVCloneAlternate.Controls.Add(tlpCloneVmRow, 0, 1);
			pnlHyperVCloneAlternate.Controls.Add(lblCloneDiskFolder, 0, 2);
			pnlHyperVCloneAlternate.Controls.Add(tlpCloneDiskRow, 0, 3);
			pnlHyperVCloneAlternate.Location = new Point(10, 94);
			pnlHyperVCloneAlternate.Margin = new Padding(10, 0, 0, 0);
			pnlHyperVCloneAlternate.Name = "pnlHyperVCloneAlternate";
			pnlHyperVCloneAlternate.RowCount = 4;
			pnlHyperVCloneAlternate.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
			pnlHyperVCloneAlternate.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			pnlHyperVCloneAlternate.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			pnlHyperVCloneAlternate.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
			pnlHyperVCloneAlternate.Size = new Size(467, 113);
			pnlHyperVCloneAlternate.TabIndex = 3;
			pnlHyperVCloneAlternate.Visible = false;
			// 
			// lblCloneVmFolder
			// 
			lblCloneVmFolder.Location = new Point(3, 0);
			lblCloneVmFolder.Name = "lblCloneVmFolder";
			lblCloneVmFolder.Size = new Size(179, 17);
			lblCloneVmFolder.TabIndex = 0;
			lblCloneVmFolder.Text = "Virtual machine config folder:";
			// 
			// tlpCloneVmRow
			// 
			tlpCloneVmRow.ColumnCount = 2;
			tlpCloneVmRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpCloneVmRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
			tlpCloneVmRow.Controls.Add(txtHyperVCloneVmFolder, 0, 0);
			tlpCloneVmRow.Controls.Add(btnBrowseCloneVm, 1, 0);
			tlpCloneVmRow.Dock = DockStyle.Fill;
			tlpCloneVmRow.Location = new Point(3, 26);
			tlpCloneVmRow.Name = "tlpCloneVmRow";
			tlpCloneVmRow.RowCount = 1;
			tlpCloneVmRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpCloneVmRow.Size = new Size(461, 34);
			tlpCloneVmRow.TabIndex = 1;
			// 
			// txtHyperVCloneVmFolder
			// 
			txtHyperVCloneVmFolder.Dock = DockStyle.Fill;
			txtHyperVCloneVmFolder.Location = new Point(3, 3);
			txtHyperVCloneVmFolder.Name = "txtHyperVCloneVmFolder";
			txtHyperVCloneVmFolder.Size = new Size(375, 25);
			txtHyperVCloneVmFolder.TabIndex = 0;
			// 
			// btnBrowseCloneVm
			// 
			btnBrowseCloneVm.Dock = DockStyle.Fill;
			btnBrowseCloneVm.Location = new Point(384, 3);
			btnBrowseCloneVm.Name = "btnBrowseCloneVm";
			btnBrowseCloneVm.Size = new Size(74, 28);
			btnBrowseCloneVm.TabIndex = 1;
			btnBrowseCloneVm.Text = "Browse...";
			btnBrowseCloneVm.UseVisualStyleBackColor = true;
			btnBrowseCloneVm.Click += BrowseHyperVCloneVmFolder_Click;
			// 
			// lblCloneDiskFolder
			// 
			lblCloneDiskFolder.Location = new Point(3, 63);
			lblCloneDiskFolder.Name = "lblCloneDiskFolder";
			lblCloneDiskFolder.Size = new Size(144, 17);
			lblCloneDiskFolder.TabIndex = 2;
			lblCloneDiskFolder.Text = "Virtual disk data folder:";
			// 
			// tlpCloneDiskRow
			// 
			tlpCloneDiskRow.ColumnCount = 2;
			tlpCloneDiskRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpCloneDiskRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
			tlpCloneDiskRow.Controls.Add(txtHyperVCloneDiskFolder, 0, 0);
			tlpCloneDiskRow.Controls.Add(btnBrowseCloneDisk, 1, 0);
			tlpCloneDiskRow.Dock = DockStyle.Fill;
			tlpCloneDiskRow.Location = new Point(3, 86);
			tlpCloneDiskRow.Name = "tlpCloneDiskRow";
			tlpCloneDiskRow.RowCount = 1;
			tlpCloneDiskRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpCloneDiskRow.Size = new Size(461, 29);
			tlpCloneDiskRow.TabIndex = 3;
			// 
			// txtHyperVCloneDiskFolder
			// 
			txtHyperVCloneDiskFolder.Dock = DockStyle.Fill;
			txtHyperVCloneDiskFolder.Location = new Point(3, 3);
			txtHyperVCloneDiskFolder.Name = "txtHyperVCloneDiskFolder";
			txtHyperVCloneDiskFolder.Size = new Size(375, 25);
			txtHyperVCloneDiskFolder.TabIndex = 0;
			// 
			// btnBrowseCloneDisk
			// 
			btnBrowseCloneDisk.Dock = DockStyle.Fill;
			btnBrowseCloneDisk.Location = new Point(384, 3);
			btnBrowseCloneDisk.Name = "btnBrowseCloneDisk";
			btnBrowseCloneDisk.Size = new Size(74, 23);
			btnBrowseCloneDisk.TabIndex = 1;
			btnBrowseCloneDisk.Text = "Browse...";
			btnBrowseCloneDisk.UseVisualStyleBackColor = true;
			btnBrowseCloneDisk.Click += BrowseHyperVCloneDiskFolder_Click;
			// 
			// chkOverwrite
			// 
			chkOverwrite.Location = new Point(3, 584);
			chkOverwrite.Margin = new Padding(3, 11, 3, 3);
			chkOverwrite.Name = "chkOverwrite";
			chkOverwrite.Size = new Size(158, 21);
			chkOverwrite.TabIndex = 13;
			chkOverwrite.Text = "Overwrite existing files";
			chkOverwrite.UseVisualStyleBackColor = true;
			// 
			// chkPreservePermissions
			// 
			chkPreservePermissions.Checked = true;
			chkPreservePermissions.CheckState = CheckState.Checked;
			chkPreservePermissions.Location = new Point(3, 611);
			chkPreservePermissions.Name = "chkPreservePermissions";
			chkPreservePermissions.Size = new Size(257, 19);
			chkPreservePermissions.TabIndex = 14;
			chkPreservePermissions.Text = "Preserve file permissions and attributes";
			chkPreservePermissions.UseVisualStyleBackColor = true;
			// 
			// chkVerifyAfterRestore
			// 
			chkVerifyAfterRestore.Checked = true;
			chkVerifyAfterRestore.CheckState = CheckState.Checked;
			chkVerifyAfterRestore.Location = new Point(3, 639);
			chkVerifyAfterRestore.Name = "chkVerifyAfterRestore";
			chkVerifyAfterRestore.Size = new Size(163, 21);
			chkVerifyAfterRestore.TabIndex = 15;
			chkVerifyAfterRestore.Text = "Verify files after restore";
			chkVerifyAfterRestore.UseVisualStyleBackColor = true;
			// 
			// grpRestoreTarget
			// 
			grpRestoreTarget.Controls.Add(tlpTarget);
			grpRestoreTarget.Dock = DockStyle.Fill;
			grpRestoreTarget.Location = new Point(545, 3);
			grpRestoreTarget.Name = "grpRestoreTarget";
			grpRestoreTarget.Padding = new Padding(10, 11, 10, 11);
			grpRestoreTarget.Size = new Size(526, 838);
			grpRestoreTarget.TabIndex = 1;
			grpRestoreTarget.TabStop = false;
			grpRestoreTarget.Text = "Select Restore Target";
			// 
			// tlpTarget
			// 
			tlpTarget.ColumnCount = 1;
			tlpTarget.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpTarget.Controls.Add(txtDriveTreeHelp, 0, 0);
			tlpTarget.Controls.Add(pnlTargetTreeHost, 0, 1);
			tlpTarget.Controls.Add(tlpTargetFooter, 0, 2);
			tlpTarget.Dock = DockStyle.Fill;
			tlpTarget.Location = new Point(10, 29);
			tlpTarget.Name = "tlpTarget";
			tlpTarget.RowCount = 3;
			tlpTarget.RowStyles.Add(new RowStyle());
			tlpTarget.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpTarget.RowStyles.Add(new RowStyle());
			tlpTarget.Size = new Size(506, 798);
			tlpTarget.TabIndex = 0;
			// 
			// txtDriveTreeHelp
			// 
			txtDriveTreeHelp.Dock = DockStyle.Fill;
			txtDriveTreeHelp.ForeColor = Color.FromArgb(96, 96, 96);
			txtDriveTreeHelp.Location = new Point(3, 0);
			txtDriveTreeHelp.Name = "txtDriveTreeHelp";
			txtDriveTreeHelp.Size = new Size(500, 63);
			txtDriveTreeHelp.TabIndex = 0;
			txtDriveTreeHelp.Text = "Select a backup on the left to load available restore targets. Choose the target disk or volume below. The boot/system disk is shown but cannot be selected.";
			// 
			// pnlTargetTreeHost
			// 
			pnlTargetTreeHost.Controls.Add(loadingTargetOverlay);
			pnlTargetTreeHost.Controls.Add(treeViewRestoreTarget);
			pnlTargetTreeHost.Dock = DockStyle.Fill;
			pnlTargetTreeHost.Location = new Point(3, 66);
			pnlTargetTreeHost.Name = "pnlTargetTreeHost";
			pnlTargetTreeHost.Size = new Size(500, 680);
			pnlTargetTreeHost.TabIndex = 1;
			// 
			// loadingTargetOverlay
			// 
			loadingTargetOverlay.BackColor = SystemColors.Control;
			loadingTargetOverlay.Controls.Add(tlpLoadingCenter);
			loadingTargetOverlay.Dock = DockStyle.Fill;
			loadingTargetOverlay.Location = new Point(0, 0);
			loadingTargetOverlay.Name = "loadingTargetOverlay";
			loadingTargetOverlay.Size = new Size(500, 680);
			loadingTargetOverlay.TabIndex = 0;
			loadingTargetOverlay.Visible = false;
			// 
			// tlpLoadingCenter
			// 
			tlpLoadingCenter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			tlpLoadingCenter.ColumnCount = 1;
			tlpLoadingCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpLoadingCenter.Controls.Add(pnlLoadingContent, 0, 0);
			tlpLoadingCenter.Location = new Point(0, 0);
			tlpLoadingCenter.Name = "tlpLoadingCenter";
			tlpLoadingCenter.RowCount = 1;
			tlpLoadingCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpLoadingCenter.Size = new Size(500, 549);
			tlpLoadingCenter.TabIndex = 0;
			// 
			// pnlLoadingContent
			// 
			pnlLoadingContent.Anchor = AnchorStyles.None;
			pnlLoadingContent.Controls.Add(progressLoadingTargets);
			pnlLoadingContent.Controls.Add(lblLoadingTargets);
			pnlLoadingContent.Location = new Point(160, 249);
			pnlLoadingContent.Name = "pnlLoadingContent";
			pnlLoadingContent.Size = new Size(180, 50);
			pnlLoadingContent.TabIndex = 0;
			// 
			// progressLoadingTargets
			// 
			progressLoadingTargets.Dock = DockStyle.Bottom;
			progressLoadingTargets.Location = new Point(0, 32);
			progressLoadingTargets.Name = "progressLoadingTargets";
			progressLoadingTargets.Size = new Size(180, 18);
			progressLoadingTargets.Style = ProgressBarStyle.Marquee;
			progressLoadingTargets.TabIndex = 0;
			// 
			// lblLoadingTargets
			// 
			lblLoadingTargets.Dock = DockStyle.Top;
			lblLoadingTargets.Location = new Point(0, 0);
			lblLoadingTargets.Name = "lblLoadingTargets";
			lblLoadingTargets.Size = new Size(180, 29);
			lblLoadingTargets.TabIndex = 1;
			lblLoadingTargets.Text = "Loading drives...";
			lblLoadingTargets.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// treeViewRestoreTarget
			// 
			treeViewRestoreTarget.BorderStyle = BorderStyle.FixedSingle;
			treeViewRestoreTarget.Dock = DockStyle.Fill;
			treeViewRestoreTarget.HideSelection = false;
			treeViewRestoreTarget.Location = new Point(0, 0);
			treeViewRestoreTarget.Name = "treeViewRestoreTarget";
			treeViewRestoreTarget.Size = new Size(500, 680);
			treeViewRestoreTarget.StateImageList = imgCheckStates;
			treeViewRestoreTarget.TabIndex = 1;
			treeViewRestoreTarget.AfterCollapse += treeViewRestoreTarget_AfterCollapse;
			treeViewRestoreTarget.AfterExpand += treeViewRestoreTarget_AfterExpand;
			treeViewRestoreTarget.KeyDown += treeViewRestoreTarget_KeyDown;
			treeViewRestoreTarget.MouseDown += treeViewRestoreTarget_MouseDown;
			// 
			// imgCheckStates
			// 
			imgCheckStates.ColorDepth = ColorDepth.Depth32Bit;
			imgCheckStates.ImageSize = new Size(16, 16);
			imgCheckStates.TransparentColor = Color.Transparent;
			// 
			// tlpTargetFooter
			// 
			tlpTargetFooter.ColumnCount = 1;
			tlpTargetFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpTargetFooter.Controls.Add(flpTargetButtons, 0, 0);
			tlpTargetFooter.Controls.Add(txtSelectedTargetLabel, 0, 1);
			tlpTargetFooter.Dock = DockStyle.Fill;
			tlpTargetFooter.Location = new Point(3, 758);
			tlpTargetFooter.Margin = new Padding(3, 9, 3, 0);
			tlpTargetFooter.Name = "tlpTargetFooter";
			tlpTargetFooter.RowCount = 2;
			tlpTargetFooter.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpTargetFooter.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tlpTargetFooter.Size = new Size(500, 40);
			tlpTargetFooter.TabIndex = 2;
			// 
			// flpTargetButtons
			// 
			flpTargetButtons.Controls.Add(btnRefreshTarget);
			flpTargetButtons.Controls.Add(btnExpandAllTarget);
			flpTargetButtons.Controls.Add(btnCollapseAllTarget);
			flpTargetButtons.Controls.Add(chkShowHiddenPartitionsTarget);
			flpTargetButtons.Dock = DockStyle.Fill;
			flpTargetButtons.Location = new Point(3, 3);
			flpTargetButtons.Name = "flpTargetButtons";
			flpTargetButtons.Size = new Size(494, 14);
			flpTargetButtons.TabIndex = 0;
			flpTargetButtons.WrapContents = false;
			// 
			// btnRefreshTarget
			// 
			btnRefreshTarget.Location = new Point(3, 3);
			btnRefreshTarget.Name = "btnRefreshTarget";
			btnRefreshTarget.Size = new Size(80, 32);
			btnRefreshTarget.TabIndex = 0;
			btnRefreshTarget.Text = "Refresh";
			btnRefreshTarget.UseVisualStyleBackColor = true;
			btnRefreshTarget.Click += RefreshRestoreTarget_Click;
			// 
			// btnExpandAllTarget
			// 
			btnExpandAllTarget.Location = new Point(89, 3);
			btnExpandAllTarget.Name = "btnExpandAllTarget";
			btnExpandAllTarget.Size = new Size(80, 32);
			btnExpandAllTarget.TabIndex = 1;
			btnExpandAllTarget.Text = "Expand All";
			btnExpandAllTarget.UseVisualStyleBackColor = true;
			btnExpandAllTarget.Click += ExpandAllTarget_Click;
			// 
			// btnCollapseAllTarget
			// 
			btnCollapseAllTarget.Location = new Point(175, 3);
			btnCollapseAllTarget.Name = "btnCollapseAllTarget";
			btnCollapseAllTarget.Size = new Size(90, 32);
			btnCollapseAllTarget.TabIndex = 2;
			btnCollapseAllTarget.Text = "Collapse All";
			btnCollapseAllTarget.UseVisualStyleBackColor = true;
			btnCollapseAllTarget.Click += CollapseAllTarget_Click;
			// 
			// chkShowHiddenPartitionsTarget
			// 
			chkShowHiddenPartitionsTarget.Location = new Point(278, 8);
			chkShowHiddenPartitionsTarget.Margin = new Padding(10, 8, 3, 3);
			chkShowHiddenPartitionsTarget.Name = "chkShowHiddenPartitionsTarget";
			chkShowHiddenPartitionsTarget.Size = new Size(162, 21);
			chkShowHiddenPartitionsTarget.TabIndex = 3;
			chkShowHiddenPartitionsTarget.Text = "Show Hidden Partitions";
			toolTip1.SetToolTip(chkShowHiddenPartitionsTarget, "Include EFI, Recovery, and System Reserved partitions");
			chkShowHiddenPartitionsTarget.UseVisualStyleBackColor = true;
			chkShowHiddenPartitionsTarget.Click += ShowHiddenPartitionsTarget_Click;
			// 
			// txtSelectedTargetLabel
			// 
			txtSelectedTargetLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
			txtSelectedTargetLabel.ForeColor = Color.FromArgb(96, 96, 96);
			txtSelectedTargetLabel.Location = new Point(3, 20);
			txtSelectedTargetLabel.Name = "txtSelectedTargetLabel";
			txtSelectedTargetLabel.Size = new Size(124, 19);
			txtSelectedTargetLabel.TabIndex = 1;
			txtSelectedTargetLabel.Text = "No target selected";
			// 
			// flpButtons
			// 
			flpButtons.Controls.Add(btnCancel);
			flpButtons.Controls.Add(btnRestore);
			flpButtons.Dock = DockStyle.Fill;
			flpButtons.FlowDirection = FlowDirection.RightToLeft;
			flpButtons.Location = new Point(3, 894);
			flpButtons.Name = "flpButtons";
			flpButtons.Size = new Size(1074, 40);
			flpButtons.TabIndex = 2;
			// 
			// btnCancel
			// 
			btnCancel.Location = new Point(971, 3);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(100, 34);
			btnCancel.TabIndex = 0;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			btnCancel.Click += Cancel_Click;
			// 
			// btnRestore
			// 
			btnRestore.Enabled = false;
			btnRestore.Location = new Point(865, 3);
			btnRestore.Name = "btnRestore";
			btnRestore.Size = new Size(100, 34);
			btnRestore.TabIndex = 1;
			btnRestore.Text = "Next";
			btnRestore.UseVisualStyleBackColor = true;
			btnRestore.Click += StartRestore_Click;
			// 
			// RestoreFormNew
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1100, 959);
			Controls.Add(tlpRoot);
			MinimumSize = new Size(850, 675);
			Name = "RestoreFormNew";
			Padding = new Padding(10, 11, 10, 11);
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore Backup";
			Load += RestoreWindowNew_Load;
			tlpRoot.ResumeLayout(false);
			tlpBody.ResumeLayout(false);
			tlpLeft.ResumeLayout(false);
			grpSelectBackup.ResumeLayout(false);
			tlpSelectBackup.ResumeLayout(false);
			tlpBackupSourceRow.ResumeLayout(false);
			tlpBackupSourceRow.PerformLayout();
			pnlBackupInfo.ResumeLayout(false);
			flpBackupCounts.ResumeLayout(false);
			grpRestoreOptions.ResumeLayout(false);
			pnlOptionsScroll.ResumeLayout(false);
			tlpOptions.ResumeLayout(false);
			pnlItemSelection.ResumeLayout(false);
			pnlHyperVRestoreMode.ResumeLayout(false);
			pnlHyperVVmOptions.ResumeLayout(false);
			pnlHyperVVmOptions.PerformLayout();
			pnlHyperVReplaceExistingOptions.ResumeLayout(false);
			pnlHyperVDirectoryOptions.ResumeLayout(false);
			tlpHyperVDirRow.ResumeLayout(false);
			tlpHyperVDirRow.PerformLayout();
			pnlRegularHyperVRestore.ResumeLayout(false);
			tlpHyperVDiskRow.ResumeLayout(false);
			tlpHyperVDiskRow.PerformLayout();
			pnlExistingHyperVVmOptions.ResumeLayout(false);
			pnlNewHyperVVmOptions.ResumeLayout(false);
			pnlNewHyperVVmOptions.PerformLayout();
			tlpNewVmPathRow.ResumeLayout(false);
			tlpNewVmPathRow.PerformLayout();
			pnlLocationChoice.ResumeLayout(false);
			pnlLocationChoice.PerformLayout();
			pnlHyperVCloneDestination.ResumeLayout(false);
			pnlHyperVCloneAlternate.ResumeLayout(false);
			tlpCloneVmRow.ResumeLayout(false);
			tlpCloneVmRow.PerformLayout();
			tlpCloneDiskRow.ResumeLayout(false);
			tlpCloneDiskRow.PerformLayout();
			grpRestoreTarget.ResumeLayout(false);
			tlpTarget.ResumeLayout(false);
			pnlTargetTreeHost.ResumeLayout(false);
			loadingTargetOverlay.ResumeLayout(false);
			tlpLoadingCenter.ResumeLayout(false);
			pnlLoadingContent.ResumeLayout(false);
			tlpTargetFooter.ResumeLayout(false);
			flpTargetButtons.ResumeLayout(false);
			flpButtons.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private TableLayoutPanel tlpRoot;
		private Label lblTitle;
		private TableLayoutPanel tlpBody;
		private FlowLayoutPanel flpButtons;
		private Button btnRestore;
		private Button btnCancel;
		private ToolTip toolTip1;
		private ImageList imgCheckStates;

		private TableLayoutPanel tlpLeft;
		private GroupBox grpSelectBackup;
		private TableLayoutPanel tlpSelectBackup;
		private Label lblBackupSource;
		private TableLayoutPanel tlpBackupSourceRow;
		private TextBox txtBackupSource;
		private Button btnBrowseBackup;
		private Button btnScanBackup;
		private TableLayoutPanel pnlBackupInfo;
		private FlowLayoutPanel flpBackupCounts;
		private Label txtBackupFileCount;
		private Label lblCountSep1;
		private Label txtBackupTotalSize;
		private Label lblCountSep2;
		private Label txtBackupRestorePointCount;
		private Label txtRestorePointPrompt;
		private Label txtPreselectedRestorePointSummary;
		private ToggleSelectListBox lstRestorePoints;

		private GroupBox grpRestoreOptions;
		private Panel pnlOptionsScroll;
		private TableLayoutPanel tlpOptions;
		private Label txtWhatToRestoreLabel;
		private Label txtPreselectedScopeSummary;
		private RadioButton rbRestoreAll;
		private RadioButton rbRestoreSelected;
		private Panel pnlItemSelection;
		private ListBox lstBackupItems;
		private Label lblRestoreDestination;
		private Label txtDestinationHelp;

		private TableLayoutPanel pnlHyperVRestoreMode;
		private Label lblHyperVRestoreTarget;
		private ComboBox cmbHyperVRestoreTarget;

		private TableLayoutPanel pnlHyperVVmOptions;
		private Label lblRestoreMode;
		private RadioButton rbHyperVReplaceExisting;
		private RadioButton rbHyperVRestoreToDirectory;
		private TableLayoutPanel pnlHyperVReplaceExistingOptions;
		private Label lblReplaceVm;
		private ComboBox cmbHyperVVmToReplace;
		private TableLayoutPanel pnlHyperVDirectoryOptions;
		private Label lblRestoreDir;
		private TableLayoutPanel tlpHyperVDirRow;
		private TextBox txtHyperVRestoreDirectory;
		private Button btnBrowseHyperVDir;
		private Label lblVmNameAfter;
		private TextBox txtHyperVVmName;
		private CheckBox chkStartHyperVVm;

		private CheckBox chkRestoreToHyperVDisk;
		private TableLayoutPanel pnlRegularHyperVRestore;
		private Label lblVhdPath;
		private TableLayoutPanel tlpHyperVDiskRow;
		private TextBox txtHyperVVirtualDiskPath;
		private Button btnBrowseHyperVDisk;
		private Label lblAfterRestore;
		private RadioButton rbLeaveHyperVDiskDetached;
		private RadioButton chkAttachToExistingHyperVVm;
		private RadioButton rbCreateNewHyperVVm;
		private TableLayoutPanel pnlExistingHyperVVmOptions;
		private Label lblExistingVm;
		private ComboBox cmbExistingHyperVVm;
		private TableLayoutPanel pnlNewHyperVVmOptions;
		private Label lblNewVmName;
		private TextBox txtNewHyperVVmName;
		private Label lblNewVmFolder;
		private TableLayoutPanel tlpNewVmPathRow;
		private TextBox txtNewHyperVVmPath;
		private Button btnBrowseNewVmPath;
		private Label lblNewVmGen;
		private ComboBox cmbNewHyperVGeneration;
		private CheckBox chkStartCreatedHyperVVm;

		private TableLayoutPanel pnlLocationChoice;
		private Label lblFileFolderTarget;
		private Button btnBrowseRestoreDestination;
		private Label lblTargetLocation;
		private TextBox txtFolderRestoreDestination;

		private TableLayoutPanel pnlHyperVCloneDestination;
		private Label lblCloneDestination;
		private RadioButton rbHyperVCloneDefault;
		private RadioButton rbHyperVCloneAlternate;
		private TableLayoutPanel pnlHyperVCloneAlternate;
		private Label lblCloneVmFolder;
		private TableLayoutPanel tlpCloneVmRow;
		private TextBox txtHyperVCloneVmFolder;
		private Button btnBrowseCloneVm;
		private Label lblCloneDiskFolder;
		private TableLayoutPanel tlpCloneDiskRow;
		private TextBox txtHyperVCloneDiskFolder;
		private Button btnBrowseCloneDisk;

		private CheckBox chkOverwrite;
		private CheckBox chkPreservePermissions;
		private CheckBox chkVerifyAfterRestore;

		private GroupBox grpRestoreTarget;
		private TableLayoutPanel tlpTarget;
		private Label txtDriveTreeHelp;
		private Panel pnlTargetTreeHost;
		private TreeView treeViewRestoreTarget;
		private Panel loadingTargetOverlay;
		private TableLayoutPanel tlpLoadingCenter;
		private Panel pnlLoadingContent;
		private Label lblLoadingTargets;
		private ProgressBar progressLoadingTargets;
		private TableLayoutPanel tlpTargetFooter;
		private FlowLayoutPanel flpTargetButtons;
		private Button btnRefreshTarget;
		private Button btnExpandAllTarget;
		private Button btnCollapseAllTarget;
		private CheckBox chkShowHiddenPartitionsTarget;
		private Label txtSelectedTargetLabel;
	}
}
