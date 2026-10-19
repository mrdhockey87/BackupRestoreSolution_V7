using SecureServerBackup.Controls;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class MainForm
	{
		//private IContainer components;
		private MenuStrip mainMenuStrip;
		private StatusStrip mainStatusStrip;
		private ToolStripStatusLabel versionStatusLabel;
		private CustomTabControl mainTabControl;
		private TabPage backupTabPage;
		private TabPage activityTabPage;
		private TabPage mountBackupsTabPage;
		private TabPage verifyTabPage;
		private TabPage restoreTabPage;
		private TabPage schedulesTabPage;
		private FlowLayoutPanel backupJobsPanel;
		private TableLayoutPanel verifyRootLayout;
		private TableLayoutPanel verifyHeaderLayout;
		private Label verifyHeaderLabel;
		private FlowLayoutPanel verifyActionsPanel;
		private DataGridView verifyBackupsDataGridiew;
		private Label verifyStatusLabel;
		private TableLayoutPanel restoreRootLayout;
		private FlowLayoutPanel restoreActionsPanel;
		private DataGridView restoreBackupsDataGridView;
		private Label emptyRestoreBackupsLabel;
		private Label restoreStatusLabel;
		private Panel schedulesTabPanel;
		private Label emptyBackupJobsLabel;


		private void InitializeComponent()
		{
			mainMenuStrip = new MenuStrip();
			mainStatusStrip = new StatusStrip();
			versionStatusLabel = new ToolStripStatusLabel();
			mainTabControl = new CustomTabControl();
			backupTabPage = new TabPage();
			backupJobsPanel = new FlowLayoutPanel();
			emptyBackupJobsLabel = new Label();
			activityTabPage = new TabPage();
			activityTabPanel = new Panel();
			mountBackupsTabPage = new TabPage();
			verifyTabPage = new TabPage();
			verifyRootLayout = new TableLayoutPanel();
			verifyHeaderLayout = new TableLayoutPanel();
			verifyHeaderLabel = new Label();
			verifyActionsPanel = new FlowLayoutPanel();
			verifyBackupsDataGridiew = new DataGridView();
			verifyStatusLabel = new Label();
			restoreTabPage = new TabPage();
			restoreRootLayout = new TableLayoutPanel();
			restoreActionsPanel = new FlowLayoutPanel();
			restoreBackupsDataGridView = new DataGridView();
			emptyRestoreBackupsLabel = new Label();
			restoreStatusLabel = new Label();
			schedulesTabPage = new TabPage();
			schedulesTabPanel = new Panel();
			mainStatusStrip.SuspendLayout();
			mainTabControl.SuspendLayout();
			backupTabPage.SuspendLayout();
			backupJobsPanel.SuspendLayout();
			activityTabPage.SuspendLayout();
			verifyTabPage.SuspendLayout();
			verifyRootLayout.SuspendLayout();
			verifyHeaderLayout.SuspendLayout();
			((ISupportInitialize)verifyBackupsDataGridiew).BeginInit();
			restoreTabPage.SuspendLayout();
			restoreRootLayout.SuspendLayout();
			((ISupportInitialize)restoreBackupsDataGridView).BeginInit();
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
			mainStatusStrip.Location = new Point(0, 649);
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
			mainTabControl.Size = new Size(805, 625);
			mainTabControl.SizeMode = TabSizeMode.Fixed;
			mainTabControl.TabIndex = 2;
			mainTabControl.TabTextColor = Color.Empty;
			mainTabControl.DrawItem += MainTabControl_DrawItem;
			mainTabControl.SelectedIndexChanged += MainTabControl_SelectedIndexChanged;
			// 
			// backupTabPage
			// 
			backupTabPage.Controls.Add(backupJobsPanel);
			backupTabPage.Location = new Point(4, 32);
			backupTabPage.Name = "backupTabPage";
			backupTabPage.Padding = new Padding(5);
			backupTabPage.Size = new Size(797, 589);
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
			backupJobsPanel.Size = new Size(787, 579);
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
			activityTabPage.Size = new Size(797, 589);
			activityTabPage.TabIndex = 1;
			activityTabPage.Text = "Activity";
			activityTabPage.UseVisualStyleBackColor = true;
			// 
			// activityTabPanel
			// 
			activityTabPanel.Dock = DockStyle.Fill;
			activityTabPanel.Location = new Point(5, 5);
			activityTabPanel.Name = "activityTabPanel";
			activityTabPanel.Size = new Size(787, 579);
			activityTabPanel.TabIndex = 0;
			// 
			// mountBackupsTabPage
			// 
			mountBackupsTabPage.Location = new Point(4, 32);
			mountBackupsTabPage.Name = "mountBackupsTabPage";
			mountBackupsTabPage.Padding = new Padding(5);
			mountBackupsTabPage.Size = new Size(797, 589);
			mountBackupsTabPage.TabIndex = 2;
			mountBackupsTabPage.Text = "Mount Backups";
			mountBackupsTabPage.UseVisualStyleBackColor = true;
			// 
			// verifyTabPage
			// 
			verifyTabPage.Controls.Add(verifyRootLayout);
			verifyTabPage.Location = new Point(4, 32);
			verifyTabPage.Name = "verifyTabPage";
			verifyTabPage.Padding = new Padding(5);
			verifyTabPage.Size = new Size(797, 589);
			verifyTabPage.TabIndex = 3;
			verifyTabPage.Text = "Verify";
			verifyTabPage.UseVisualStyleBackColor = true;
			// 
			// verifyRootLayout
			// 
			verifyRootLayout.ColumnCount = 1;
			verifyRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			verifyRootLayout.Controls.Add(verifyHeaderLayout, 0, 0);
			verifyRootLayout.Controls.Add(verifyBackupsDataGridiew, 0, 1);
			verifyRootLayout.Controls.Add(verifyStatusLabel, 0, 2);
			verifyRootLayout.Dock = DockStyle.Fill;
			verifyRootLayout.Location = new Point(5, 5);
			verifyRootLayout.Name = "verifyRootLayout";
			verifyRootLayout.Padding = new Padding(9, 8, 9, 8);
			verifyRootLayout.RowCount = 3;
			verifyRootLayout.RowStyles.Add(new RowStyle());
			verifyRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			verifyRootLayout.RowStyles.Add(new RowStyle());
			verifyRootLayout.Size = new Size(787, 579);
			verifyRootLayout.TabIndex = 0;
			// 
			// verifyHeaderLayout
			// 
			verifyHeaderLayout.AutoSize = true;
			verifyHeaderLayout.ColumnCount = 2;
			verifyHeaderLayout.ColumnStyles.Add(new ColumnStyle());
			verifyHeaderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			verifyHeaderLayout.Controls.Add(verifyHeaderLabel, 0, 0);
			verifyHeaderLayout.Controls.Add(verifyActionsPanel, 1, 0);
			verifyHeaderLayout.Dock = DockStyle.Fill;
			verifyHeaderLayout.Location = new Point(9, 8);
			verifyHeaderLayout.Margin = new Padding(0, 0, 0, 10);
			verifyHeaderLayout.Name = "verifyHeaderLayout";
			verifyHeaderLayout.RowCount = 1;
			verifyHeaderLayout.RowStyles.Add(new RowStyle());
			verifyHeaderLayout.Size = new Size(769, 25);
			verifyHeaderLayout.TabIndex = 0;
			// 
			// verifyHeaderLabel
			// 
			verifyHeaderLabel.Anchor = AnchorStyles.Left;
			verifyHeaderLabel.AutoSize = true;
			verifyHeaderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			verifyHeaderLabel.Location = new Point(0, 0);
			verifyHeaderLabel.Margin = new Padding(0, 0, 18, 0);
			verifyHeaderLabel.Name = "verifyHeaderLabel";
			verifyHeaderLabel.Size = new Size(209, 25);
			verifyHeaderLabel.TabIndex = 0;
			verifyHeaderLabel.Text = "Verify Backup Integrity";
			// 
			// verifyActionsPanel
			// 
			verifyActionsPanel.AutoSize = true;
			verifyActionsPanel.Location = new Point(227, 0);
			verifyActionsPanel.Margin = new Padding(0);
			verifyActionsPanel.Name = "verifyActionsPanel";
			verifyActionsPanel.Size = new Size(0, 0);
			verifyActionsPanel.TabIndex = 1;
			verifyActionsPanel.WrapContents = false;
			// 
			// verifyBackupsDataGridiew
			// 
			verifyBackupsDataGridiew.ColumnHeadersHeight = 25;
			verifyBackupsDataGridiew.Dock = DockStyle.Fill;
			verifyBackupsDataGridiew.Location = new Point(12, 46);
			verifyBackupsDataGridiew.Name = "verifyBackupsDataGridiew";
			verifyBackupsDataGridiew.RowHeadersWidth = 45;
			verifyBackupsDataGridiew.Size = new Size(763, 505);
			verifyBackupsDataGridiew.TabIndex = 0;
			// 
			// verifyStatusLabel
			// 
			verifyStatusLabel.AutoSize = true;
			verifyStatusLabel.Location = new Point(18, 554);
			verifyStatusLabel.Margin = new Padding(9, 0, 9, 0);
			verifyStatusLabel.MaximumSize = new Size(900, 0);
			verifyStatusLabel.Name = "verifyStatusLabel";
			verifyStatusLabel.Size = new Size(488, 17);
			verifyStatusLabel.TabIndex = 3;
			verifyStatusLabel.Text = "Select a backup and click Verify Selected to validate the archive before restoring it.";
			// 
			// restoreTabPage
			// 
			restoreTabPage.Controls.Add(restoreRootLayout);
			restoreTabPage.Location = new Point(4, 32);
			restoreTabPage.Name = "restoreTabPage";
			restoreTabPage.Padding = new Padding(5);
			restoreTabPage.Size = new Size(797, 589);
			restoreTabPage.TabIndex = 4;
			restoreTabPage.Text = "Restore";
			restoreTabPage.UseVisualStyleBackColor = true;
			// 
			// restoreRootLayout
			// 
			restoreRootLayout.ColumnCount = 1;
			restoreRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			restoreRootLayout.Controls.Add(restoreActionsPanel, 0, 0);
			restoreRootLayout.Controls.Add(restoreBackupsDataGridView, 0, 1);
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
			restoreRootLayout.Size = new Size(787, 579);
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
			// restoreBackupsDataGridView
			// 
			restoreBackupsDataGridView.ColumnHeadersHeight = 25;
			restoreBackupsDataGridView.Dock = DockStyle.Fill;
			restoreBackupsDataGridView.Location = new Point(12, 18);
			restoreBackupsDataGridView.Name = "restoreBackupsDataGridView";
			restoreBackupsDataGridView.RowHeadersWidth = 45;
			restoreBackupsDataGridView.Size = new Size(763, 516);
			restoreBackupsDataGridView.TabIndex = 0;
			// 
			// emptyRestoreBackupsLabel
			// 
			emptyRestoreBackupsLabel.Anchor = AnchorStyles.Left;
			emptyRestoreBackupsLabel.AutoSize = true;
			emptyRestoreBackupsLabel.Location = new Point(18, 537);
			emptyRestoreBackupsLabel.Margin = new Padding(9, 0, 9, 0);
			emptyRestoreBackupsLabel.Name = "emptyRestoreBackupsLabel";
			emptyRestoreBackupsLabel.Size = new Size(234, 17);
			emptyRestoreBackupsLabel.TabIndex = 2;
			emptyRestoreBackupsLabel.Text = "No backup files are currently available.";
			emptyRestoreBackupsLabel.Visible = false;
			// 
			// restoreStatusLabel
			// 
			restoreStatusLabel.AutoSize = true;
			restoreStatusLabel.Location = new Point(18, 554);
			restoreStatusLabel.Margin = new Padding(9, 0, 9, 0);
			restoreStatusLabel.MaximumSize = new Size(900, 0);
			restoreStatusLabel.Name = "restoreStatusLabel";
			restoreStatusLabel.Size = new Size(276, 17);
			restoreStatusLabel.TabIndex = 3;
			restoreStatusLabel.Text = "Select a backup to open the restore workflow.";
			// 
			// schedulesTabPage
			// 
			schedulesTabPage.Controls.Add(schedulesTabPanel);
			schedulesTabPage.Location = new Point(4, 32);
			schedulesTabPage.Name = "schedulesTabPage";
			schedulesTabPage.Padding = new Padding(5);
			schedulesTabPage.Size = new Size(797, 589);
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
			schedulesTabPanel.Size = new Size(787, 579);
			schedulesTabPanel.TabIndex = 0;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(805, 671);
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
			verifyTabPage.ResumeLayout(false);
			verifyRootLayout.ResumeLayout(false);
			verifyRootLayout.PerformLayout();
			verifyHeaderLayout.ResumeLayout(false);
			verifyHeaderLayout.PerformLayout();
			((ISupportInitialize)verifyBackupsDataGridiew).EndInit();
			restoreTabPage.ResumeLayout(false);
			restoreRootLayout.ResumeLayout(false);
			restoreRootLayout.PerformLayout();
			((ISupportInitialize)restoreBackupsDataGridView).EndInit();
			schedulesTabPage.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		private Panel activityTabPanel;
	}
}
