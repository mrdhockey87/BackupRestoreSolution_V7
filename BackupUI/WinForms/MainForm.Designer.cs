using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class MainForm
	{
		private IContainer components;
		private MenuStrip mainMenuStrip;
		private StatusStrip mainStatusStrip;
		private ToolStripStatusLabel versionStatusLabel;
		private TabControl mainTabControl;
		private TabPage backupTabPage;
		private TabPage activityTabPage;
		private TabPage mountBackupsTabPage;
		private TabPage verifyTabPage;
		private TabPage restoreTabPage;
		private TabPage schedulesTabPage;
		private FlowLayoutPanel backupJobsPanel;
		private Panel activityTabPanel;
		private TableLayoutPanel mountRootLayout;
		private TableLayoutPanel mountHeaderLayout;
		private Label mountHeaderLabel;
		private FlowLayoutPanel mountActionsPanel;
		private GroupBox availableBackupsGroupBox;
		private GroupBox mountedBackupsGroupBox;
		private ListView mountedBackupsListView;
		private Label mountStatusLabel;
		private TableLayoutPanel verifyRootLayout;
		private FlowLayoutPanel verifyActionsPanel;
		private ListView verifyBackupsListView;
		private Label verifyStatusLabel;
		private TableLayoutPanel restoreRootLayout;
		private FlowLayoutPanel restoreActionsPanel;
		private ListView restoreBackupsListView;
		private Label emptyRestoreBackupsLabel;
		private Label restoreStatusLabel;
		private Panel schedulesTabPanel;
		private Label emptyBackupJobsLabel;

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
			mainMenuStrip = new MenuStrip();
			mainStatusStrip = new StatusStrip();
			versionStatusLabel = new ToolStripStatusLabel();
			mainTabControl = new TabControl();
			backupTabPage = new TabPage();
			backupJobsPanel = new FlowLayoutPanel();
			emptyBackupJobsLabel = new Label();
			activityTabPage = new TabPage();
			activityTabPanel = new Panel();
			tableLayoutPanel1 = new TableLayoutPanel();
			dataGridView1 = new DataGridView();
			flowLayoutPanel1 = new FlowLayoutPanel();
			label1 = new Label();
			RefreshMounts = new Button();
			BrowseBackup = new Button();
			UnmountAll = new Button();
			label2 = new Label();
			dgMountedBackups = new DataGridView();
			label3 = new Label();
			mountBackupsTabPage = new TabPage();
			mountRootLayout = new TableLayoutPanel();
			mountHeaderLayout = new TableLayoutPanel();
			mountHeaderLabel = new Label();
			mountActionsPanel = new FlowLayoutPanel();
			availableBackupsGroupBox = new GroupBox();
			dgAvailableBackups = new DataGridView();
			mountedBackupsGroupBox = new GroupBox();
			mountedBackupsListView = new ListView();
			mountStatusLabel = new Label();
			verifyTabPage = new TabPage();
			verifyBackupsListView = new ListView();
			restoreTabPage = new TabPage();
			restoreRootLayout = new TableLayoutPanel();
			restoreActionsPanel = new FlowLayoutPanel();
			restoreBackupsListView = new ListView();
			emptyRestoreBackupsLabel = new Label();
			restoreStatusLabel = new Label();
			schedulesTabPage = new TabPage();
			schedulesTabPanel = new Panel();
			verifyRootLayout = new TableLayoutPanel();
			verifyActionsPanel = new FlowLayoutPanel();
			verifyStatusLabel = new Label();
			mainStatusStrip.SuspendLayout();
			mainTabControl.SuspendLayout();
			backupTabPage.SuspendLayout();
			backupJobsPanel.SuspendLayout();
			activityTabPage.SuspendLayout();
			activityTabPanel.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)dataGridView1).BeginInit();
			flowLayoutPanel1.SuspendLayout();
			((ISupportInitialize)dgMountedBackups).BeginInit();
			mountBackupsTabPage.SuspendLayout();
			mountRootLayout.SuspendLayout();
			mountHeaderLayout.SuspendLayout();
			availableBackupsGroupBox.SuspendLayout();
			((ISupportInitialize)dgAvailableBackups).BeginInit();
			mountedBackupsGroupBox.SuspendLayout();
			verifyTabPage.SuspendLayout();
			restoreTabPage.SuspendLayout();
			restoreRootLayout.SuspendLayout();
			schedulesTabPage.SuspendLayout();
			SuspendLayout();
			// 
			// mainMenuStrip
			// 
			mainMenuStrip.ImageScalingSize = new Size(20, 20);
			mainMenuStrip.Location = new Point(0, 0);
			mainMenuStrip.Name = "mainMenuStrip";
			mainMenuStrip.Padding = new Padding(5, 2, 0, 2);
			mainMenuStrip.Size = new Size(805, 24);
			mainMenuStrip.TabIndex = 0;
			mainMenuStrip.Text = "menuStrip1";
			// 
			// mainStatusStrip
			// 
			mainStatusStrip.ImageScalingSize = new Size(20, 20);
			mainStatusStrip.Items.AddRange(new ToolStripItem[] { versionStatusLabel });
			mainStatusStrip.Location = new Point(0, 589);
			mainStatusStrip.Name = "mainStatusStrip";
			mainStatusStrip.Padding = new Padding(1, 0, 12, 0);
			mainStatusStrip.Size = new Size(805, 22);
			mainStatusStrip.SizingGrip = false;
			mainStatusStrip.TabIndex = 1;
			// 
			// versionStatusLabel
			// 
			versionStatusLabel.Name = "versionStatusLabel";
			versionStatusLabel.Size = new Size(114, 17);
			versionStatusLabel.Text = "Version: Loading...";
			// 
			// mainTabControl
			// 
			mainTabControl.Controls.Add(backupTabPage);
			mainTabControl.Controls.Add(activityTabPage);
			mainTabControl.Controls.Add(mountBackupsTabPage);
			mainTabControl.Controls.Add(verifyTabPage);
			mainTabControl.Controls.Add(restoreTabPage);
			mainTabControl.Controls.Add(schedulesTabPage);
			mainTabControl.Dock = DockStyle.Fill;
			mainTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
			mainTabControl.ItemSize = new Size(110, 28);
			mainTabControl.Location = new Point(0, 24);
			mainTabControl.Name = "mainTabControl";
			mainTabControl.Padding = new Point(12, 4);
			mainTabControl.SelectedIndex = 0;
			mainTabControl.Size = new Size(805, 565);
			mainTabControl.SizeMode = TabSizeMode.Fixed;
			mainTabControl.TabIndex = 2;
			mainTabControl.DrawItem += MainTabControl_DrawItem;
			mainTabControl.SelectedIndexChanged += MainTabControl_SelectedIndexChanged;
			// 
			// backupTabPage
			// 
			backupTabPage.Controls.Add(backupJobsPanel);
			backupTabPage.Location = new Point(4, 32);
			backupTabPage.Name = "backupTabPage";
			backupTabPage.Padding = new Padding(5);
			backupTabPage.Size = new Size(797, 529);
			backupTabPage.TabIndex = 0;
			backupTabPage.Text = "Backup";
			backupTabPage.UseVisualStyleBackColor = true;
			// 
			// backupJobsPanel
			// 
			backupJobsPanel.AutoScroll = true;
			backupJobsPanel.BorderStyle = BorderStyle.FixedSingle;
			backupJobsPanel.Controls.Add(emptyBackupJobsLabel);
			backupJobsPanel.Dock = DockStyle.Fill;
			backupJobsPanel.FlowDirection = FlowDirection.TopDown;
			backupJobsPanel.Location = new Point(5, 5);
			backupJobsPanel.Name = "backupJobsPanel";
			backupJobsPanel.Padding = new Padding(10, 8, 10, 8);
			backupJobsPanel.Size = new Size(787, 519);
			backupJobsPanel.TabIndex = 0;
			backupJobsPanel.WrapContents = false;
			// 
			// emptyBackupJobsLabel
			// 
			emptyBackupJobsLabel.AutoSize = true;
			emptyBackupJobsLabel.Location = new Point(19, 15);
			emptyBackupJobsLabel.Margin = new Padding(9, 7, 9, 7);
			emptyBackupJobsLabel.Name = "emptyBackupJobsLabel";
			emptyBackupJobsLabel.Size = new Size(216, 17);
			emptyBackupJobsLabel.TabIndex = 0;
			emptyBackupJobsLabel.Text = "No backup jobs have been created.";
			// 
			// activityTabPage
			// 
			activityTabPage.Controls.Add(activityTabPanel);
			activityTabPage.Location = new Point(4, 32);
			activityTabPage.Name = "activityTabPage";
			activityTabPage.Padding = new Padding(5);
			activityTabPage.Size = new Size(797, 529);
			activityTabPage.TabIndex = 1;
			activityTabPage.Text = "Activity";
			activityTabPage.UseVisualStyleBackColor = true;
			activityTabPage.Enter += ActivityTabPage_Enter;
			// 
			// activityTabPanel
			// 
			activityTabPanel.Controls.Add(tableLayoutPanel1);
			activityTabPanel.Dock = DockStyle.Fill;
			activityTabPanel.Location = new Point(5, 5);
			activityTabPanel.Name = "activityTabPanel";
			activityTabPanel.Padding = new Padding(10);
			activityTabPanel.Size = new Size(787, 519);
			activityTabPanel.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(dataGridView1, 0, 2);
			tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
			tableLayoutPanel1.Controls.Add(label2, 0, 3);
			tableLayoutPanel1.Controls.Add(dgMountedBackups, 0, 4);
			tableLayoutPanel1.Controls.Add(label3, 0, 1);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(10, 10);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 5;
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel1.Size = new Size(767, 499);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// dataGridView1
			// 
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Dock = DockStyle.Fill;
			dataGridView1.Location = new Point(3, 68);
			dataGridView1.Margin = new Padding(3, 10, 3, 10);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowHeadersWidth = 45;
			dataGridView1.Size = new Size(761, 190);
			dataGridView1.TabIndex = 1;
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.Controls.Add(label1);
			flowLayoutPanel1.Controls.Add(RefreshMounts);
			flowLayoutPanel1.Controls.Add(BrowseBackup);
			flowLayoutPanel1.Controls.Add(UnmountAll);
			flowLayoutPanel1.Dock = DockStyle.Fill;
			flowLayoutPanel1.Location = new Point(3, 3);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new Size(761, 35);
			flowLayoutPanel1.TabIndex = 6;
			// 
			// label1
			// 
			label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			label1.Location = new Point(5, 5);
			label1.Margin = new Padding(5);
			label1.Name = "label1";
			label1.Size = new Size(289, 25);
			label1.TabIndex = 3;
			label1.Text = "Mount Backups as Virtual Drives";
			label1.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// RefreshMounts
			// 
			RefreshMounts.Location = new Point(304, 5);
			RefreshMounts.Margin = new Padding(5);
			RefreshMounts.Name = "RefreshMounts";
			RefreshMounts.Size = new Size(83, 25);
			RefreshMounts.TabIndex = 4;
			RefreshMounts.Text = "Refresh";
			RefreshMounts.UseVisualStyleBackColor = true;
			RefreshMounts.Click += RefreshMounts_Click;
			// 
			// BrowseBackup
			// 
			BrowseBackup.Location = new Point(397, 5);
			BrowseBackup.Margin = new Padding(5);
			BrowseBackup.Name = "BrowseBackup";
			BrowseBackup.Size = new Size(83, 25);
			BrowseBackup.TabIndex = 5;
			BrowseBackup.Text = "Browse...";
			BrowseBackup.UseVisualStyleBackColor = true;
			BrowseBackup.Click += BrowseBackup_Click;
			// 
			// UnmountAll
			// 
			UnmountAll.Location = new Point(490, 5);
			UnmountAll.Margin = new Padding(5);
			UnmountAll.Name = "UnmountAll";
			UnmountAll.Size = new Size(83, 25);
			UnmountAll.TabIndex = 6;
			UnmountAll.Text = "Unmount All";
			UnmountAll.UseVisualStyleBackColor = true;
			UnmountAll.Click += UnmountAll_Click;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(3, 268);
			label2.Name = "label2";
			label2.Size = new Size(112, 17);
			label2.TabIndex = 7;
			label2.Text = "Mounted Backups";
			// 
			// dgMountedBackups
			// 
			dgMountedBackups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgMountedBackups.Dock = DockStyle.Fill;
			dgMountedBackups.Location = new Point(3, 291);
			dgMountedBackups.Name = "dgMountedBackups";
			dgMountedBackups.RowHeadersWidth = 45;
			dgMountedBackups.Size = new Size(761, 205);
			dgMountedBackups.TabIndex = 8;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(3, 41);
			label3.Name = "label3";
			label3.Size = new Size(111, 17);
			label3.TabIndex = 9;
			label3.Text = "Available Backups";
			// 
			// mountBackupsTabPage
			// 
			mountBackupsTabPage.Controls.Add(mountRootLayout);
			mountBackupsTabPage.Location = new Point(4, 32);
			mountBackupsTabPage.Name = "mountBackupsTabPage";
			mountBackupsTabPage.Padding = new Padding(5);
			mountBackupsTabPage.Size = new Size(797, 525);
			mountBackupsTabPage.TabIndex = 2;
			mountBackupsTabPage.Text = "Mount Backups";
			mountBackupsTabPage.UseVisualStyleBackColor = true;
			// 
			// mountRootLayout
			// 
			mountRootLayout.ColumnCount = 1;
			mountRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			mountRootLayout.Controls.Add(mountHeaderLayout, 0, 0);
			mountRootLayout.Controls.Add(availableBackupsGroupBox, 0, 1);
			mountRootLayout.Controls.Add(mountedBackupsGroupBox, 0, 2);
			mountRootLayout.Controls.Add(mountStatusLabel, 0, 3);
			mountRootLayout.Dock = DockStyle.Fill;
			mountRootLayout.Location = new Point(5, 5);
			mountRootLayout.Name = "mountRootLayout";
			mountRootLayout.Padding = new Padding(9, 8, 9, 8);
			mountRootLayout.RowCount = 4;
			mountRootLayout.RowStyles.Add(new RowStyle());
			mountRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
			mountRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
			mountRootLayout.RowStyles.Add(new RowStyle());
			mountRootLayout.Size = new Size(787, 515);
			mountRootLayout.TabIndex = 0;
			// 
			// mountHeaderLayout
			// 
			mountHeaderLayout.AutoSize = true;
			mountHeaderLayout.ColumnCount = 2;
			mountHeaderLayout.ColumnStyles.Add(new ColumnStyle());
			mountHeaderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			mountHeaderLayout.Controls.Add(mountHeaderLabel, 0, 0);
			mountHeaderLayout.Controls.Add(mountActionsPanel, 1, 0);
			mountHeaderLayout.Dock = DockStyle.Fill;
			mountHeaderLayout.Location = new Point(9, 8);
			mountHeaderLayout.Margin = new Padding(0, 0, 0, 10);
			mountHeaderLayout.Name = "mountHeaderLayout";
			mountHeaderLayout.RowCount = 1;
			mountHeaderLayout.RowStyles.Add(new RowStyle());
			mountHeaderLayout.Size = new Size(769, 25);
			mountHeaderLayout.TabIndex = 0;
			// 
			// mountHeaderLabel
			// 
			mountHeaderLabel.Anchor = AnchorStyles.Left;
			mountHeaderLabel.AutoSize = true;
			mountHeaderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			mountHeaderLabel.Location = new Point(0, 0);
			mountHeaderLabel.Margin = new Padding(0, 0, 18, 0);
			mountHeaderLabel.Name = "mountHeaderLabel";
			mountHeaderLabel.Size = new Size(289, 25);
			mountHeaderLabel.TabIndex = 0;
			mountHeaderLabel.Text = "Mount Backups as Virtual Drives";
			// 
			// mountActionsPanel
			// 
			mountActionsPanel.Anchor = AnchorStyles.Left;
			mountActionsPanel.AutoSize = true;
			mountActionsPanel.Location = new Point(307, 12);
			mountActionsPanel.Margin = new Padding(0);
			mountActionsPanel.Name = "mountActionsPanel";
			mountActionsPanel.Size = new Size(0, 0);
			mountActionsPanel.TabIndex = 1;
			mountActionsPanel.WrapContents = false;
			// 
			// availableBackupsGroupBox
			// 
			availableBackupsGroupBox.Controls.Add(dgAvailableBackups);
			availableBackupsGroupBox.Dock = DockStyle.Fill;
			availableBackupsGroupBox.Location = new Point(12, 46);
			availableBackupsGroupBox.Name = "availableBackupsGroupBox";
			availableBackupsGroupBox.Padding = new Padding(6, 4, 6, 6);
			availableBackupsGroupBox.Size = new Size(763, 239);
			availableBackupsGroupBox.TabIndex = 1;
			availableBackupsGroupBox.TabStop = false;
			availableBackupsGroupBox.Text = "Available Backups";
			// 
			// dgAvailableBackups
			// 
			dgAvailableBackups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgAvailableBackups.Location = new Point(25, 25);
			dgAvailableBackups.Name = "dgAvailableBackups";
			dgAvailableBackups.RowHeadersWidth = 45;
			dgAvailableBackups.Size = new Size(265, 166);
			dgAvailableBackups.TabIndex = 0;
			// 
			// mountedBackupsGroupBox
			// 
			mountedBackupsGroupBox.Controls.Add(mountedBackupsListView);
			mountedBackupsGroupBox.Dock = DockStyle.Fill;
			mountedBackupsGroupBox.Location = new Point(12, 291);
			mountedBackupsGroupBox.Name = "mountedBackupsGroupBox";
			mountedBackupsGroupBox.Padding = new Padding(6, 4, 6, 6);
			mountedBackupsGroupBox.Size = new Size(763, 195);
			mountedBackupsGroupBox.TabIndex = 2;
			mountedBackupsGroupBox.TabStop = false;
			mountedBackupsGroupBox.Text = "Mounted Backups";
			// 
			// mountedBackupsListView
			// 
			mountedBackupsListView.Dock = DockStyle.Fill;
			mountedBackupsListView.Location = new Point(6, 22);
			mountedBackupsListView.Margin = new Padding(0);
			mountedBackupsListView.Name = "mountedBackupsListView";
			mountedBackupsListView.Size = new Size(751, 167);
			mountedBackupsListView.TabIndex = 0;
			mountedBackupsListView.UseCompatibleStateImageBehavior = false;
			mountedBackupsListView.DoubleClick += MountedBackupsListView_DoubleClick;
			// 
			// mountStatusLabel
			// 
			mountStatusLabel.AutoSize = true;
			mountStatusLabel.Location = new Point(18, 489);
			mountStatusLabel.Margin = new Padding(9, 0, 9, 0);
			mountStatusLabel.Name = "mountStatusLabel";
			mountStatusLabel.Size = new Size(0, 17);
			mountStatusLabel.TabIndex = 3;
			// 
			// verifyTabPage
			// 
			verifyTabPage.Controls.Add(verifyBackupsListView);
			verifyTabPage.Location = new Point(4, 32);
			verifyTabPage.Name = "verifyTabPage";
			verifyTabPage.Padding = new Padding(5);
			verifyTabPage.Size = new Size(797, 525);
			verifyTabPage.TabIndex = 3;
			verifyTabPage.Text = "Verify";
			verifyTabPage.UseVisualStyleBackColor = true;
			// 
			// verifyBackupsListView
			// 
			verifyBackupsListView.Dock = DockStyle.Fill;
			verifyBackupsListView.Location = new Point(5, 5);
			verifyBackupsListView.Name = "verifyBackupsListView";
			verifyBackupsListView.Size = new Size(787, 515);
			verifyBackupsListView.TabIndex = 0;
			verifyBackupsListView.UseCompatibleStateImageBehavior = false;
			// 
			// restoreTabPage
			// 
			restoreTabPage.Controls.Add(restoreRootLayout);
			restoreTabPage.Location = new Point(4, 32);
			restoreTabPage.Name = "restoreTabPage";
			restoreTabPage.Padding = new Padding(5);
			restoreTabPage.Size = new Size(797, 525);
			restoreTabPage.TabIndex = 4;
			restoreTabPage.Text = "Restore";
			restoreTabPage.UseVisualStyleBackColor = true;
			// 
			// restoreRootLayout
			// 
			restoreRootLayout.ColumnCount = 1;
			restoreRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			restoreRootLayout.Controls.Add(restoreActionsPanel, 0, 0);
			restoreRootLayout.Controls.Add(restoreBackupsListView, 0, 1);
			restoreRootLayout.Controls.Add(emptyRestoreBackupsLabel, 0, 2);
			restoreRootLayout.Controls.Add(restoreStatusLabel, 0, 3);
			restoreRootLayout.Dock = DockStyle.Fill;
			restoreRootLayout.Location = new Point(5, 5);
			restoreRootLayout.Name = "restoreRootLayout";
			restoreRootLayout.Padding = new Padding(9, 8, 9, 8);
			restoreRootLayout.RowCount = 4;
			restoreRootLayout.RowStyles.Add(new RowStyle());
			restoreRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			restoreRootLayout.RowStyles.Add(new RowStyle());
			restoreRootLayout.RowStyles.Add(new RowStyle());
			restoreRootLayout.Size = new Size(787, 515);
			restoreRootLayout.TabIndex = 0;
			// 
			// restoreActionsPanel
			// 
			restoreActionsPanel.AutoSize = true;
			restoreActionsPanel.Location = new Point(9, 8);
			restoreActionsPanel.Margin = new Padding(0, 0, 0, 7);
			restoreActionsPanel.Name = "restoreActionsPanel";
			restoreActionsPanel.Size = new Size(0, 0);
			restoreActionsPanel.TabIndex = 0;
			// 
			// restoreBackupsListView
			// 
			restoreBackupsListView.Dock = DockStyle.Fill;
			restoreBackupsListView.Location = new Point(12, 18);
			restoreBackupsListView.Name = "restoreBackupsListView";
			restoreBackupsListView.Size = new Size(763, 431);
			restoreBackupsListView.TabIndex = 1;
			restoreBackupsListView.UseCompatibleStateImageBehavior = false;
			restoreBackupsListView.DoubleClick += RestoreBackupsListView_DoubleClick;
			// 
			// emptyRestoreBackupsLabel
			// 
			emptyRestoreBackupsLabel.AutoSize = true;
			emptyRestoreBackupsLabel.Location = new Point(18, 459);
			emptyRestoreBackupsLabel.Margin = new Padding(9, 7, 9, 7);
			emptyRestoreBackupsLabel.Name = "emptyRestoreBackupsLabel";
			emptyRestoreBackupsLabel.Size = new Size(205, 17);
			emptyRestoreBackupsLabel.TabIndex = 2;
			emptyRestoreBackupsLabel.Text = "No restore backups are available.";
			// 
			// restoreStatusLabel
			// 
			restoreStatusLabel.AutoSize = true;
			restoreStatusLabel.Location = new Point(18, 483);
			restoreStatusLabel.Margin = new Padding(9, 0, 9, 7);
			restoreStatusLabel.Name = "restoreStatusLabel";
			restoreStatusLabel.Size = new Size(675, 17);
			restoreStatusLabel.TabIndex = 3;
			restoreStatusLabel.Text = "Select a backup to open the restore workflow. Restoring the boot/system drive requires the recovery environment.";
			// 
			// schedulesTabPage
			// 
			schedulesTabPage.Controls.Add(schedulesTabPanel);
			schedulesTabPage.Location = new Point(4, 32);
			schedulesTabPage.Name = "schedulesTabPage";
			schedulesTabPage.Padding = new Padding(5);
			schedulesTabPage.Size = new Size(797, 525);
			schedulesTabPage.TabIndex = 5;
			schedulesTabPage.Text = "Schedules";
			schedulesTabPage.UseVisualStyleBackColor = true;
			// 
			// schedulesTabPanel
			// 
			schedulesTabPanel.Dock = DockStyle.Fill;
			schedulesTabPanel.Location = new Point(5, 5);
			schedulesTabPanel.Name = "schedulesTabPanel";
			schedulesTabPanel.Padding = new Padding(10);
			schedulesTabPanel.Size = new Size(787, 515);
			schedulesTabPanel.TabIndex = 0;
			// 
			// verifyRootLayout
			// 
			verifyRootLayout.Location = new Point(0, 0);
			verifyRootLayout.Name = "verifyRootLayout";
			verifyRootLayout.Size = new Size(200, 100);
			verifyRootLayout.TabIndex = 0;
			// 
			// verifyActionsPanel
			// 
			verifyActionsPanel.Location = new Point(0, 0);
			verifyActionsPanel.Name = "verifyActionsPanel";
			verifyActionsPanel.Size = new Size(200, 100);
			verifyActionsPanel.TabIndex = 0;
			// 
			// verifyStatusLabel
			// 
			verifyStatusLabel.Location = new Point(0, 0);
			verifyStatusLabel.Name = "verifyStatusLabel";
			verifyStatusLabel.Size = new Size(100, 23);
			verifyStatusLabel.TabIndex = 0;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(805, 611);
			Controls.Add(mainTabControl);
			Controls.Add(mainStatusStrip);
			Controls.Add(mainMenuStrip);
			MainMenuStrip = mainMenuStrip;
			MinimumSize = new Size(821, 577);
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Secure Server Backup";
			Load += MainForm_Load;
			Resize += MainForm_Resize;
			mainStatusStrip.ResumeLayout(false);
			mainStatusStrip.PerformLayout();
			mainTabControl.ResumeLayout(false);
			backupTabPage.ResumeLayout(false);
			backupJobsPanel.ResumeLayout(false);
			backupJobsPanel.PerformLayout();
			activityTabPage.ResumeLayout(false);
			activityTabPanel.ResumeLayout(false);
			activityTabPanel.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)dataGridView1).EndInit();
			flowLayoutPanel1.ResumeLayout(false);
			((ISupportInitialize)dgMountedBackups).EndInit();
			mountBackupsTabPage.ResumeLayout(false);
			mountRootLayout.ResumeLayout(false);
			mountRootLayout.PerformLayout();
			mountHeaderLayout.ResumeLayout(false);
			mountHeaderLayout.PerformLayout();
			availableBackupsGroupBox.ResumeLayout(false);
			((ISupportInitialize)dgAvailableBackups).EndInit();
			mountedBackupsGroupBox.ResumeLayout(false);
			verifyTabPage.ResumeLayout(false);
			restoreTabPage.ResumeLayout(false);
			restoreRootLayout.ResumeLayout(false);
			restoreRootLayout.PerformLayout();
			schedulesTabPage.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		private DataGridView dgAvailableBackups;
		private TableLayoutPanel tableLayoutPanel1;
		private DataGridView dataGridView1;
		private Label label2;
		private DataGridView dgMountedBackups;
		private FlowLayoutPanel flowLayoutPanel1;
		private Label label1;
		private Button RefreshMounts;
		private Button BrowseBackup;
		private Button UnmountAll;
		private Label label3;
	}
}
