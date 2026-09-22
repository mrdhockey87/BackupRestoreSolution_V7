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
		private FlowLayoutPanel mountActionsPanel;
		private ListView mountBackupsListView;
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
			components = new Container();	
			mainMenuStrip = new MenuStrip();
			mainStatusStrip = new StatusStrip();
			versionStatusLabel = new ToolStripStatusLabel();
			mainTabControl = new TabControl();
			backupTabPage = new TabPage();
			backupJobsPanel = new FlowLayoutPanel();
			emptyBackupJobsLabel = new Label();
			activityTabPage = new TabPage();
			activityTabPanel = new Panel();
			mountBackupsTabPage = new TabPage();
			mountRootLayout = new TableLayoutPanel();
			mountActionsPanel = new FlowLayoutPanel();
			mountBackupsListView = new ListView();
			mountStatusLabel = new Label();
			verifyTabPage = new TabPage();
			verifyRootLayout = new TableLayoutPanel();
			verifyActionsPanel = new FlowLayoutPanel();
			verifyBackupsListView = new ListView();
			verifyStatusLabel = new Label();
			restoreTabPage = new TabPage();
			restoreRootLayout = new TableLayoutPanel();
			restoreActionsPanel = new FlowLayoutPanel();
			restoreBackupsListView = new ListView();
			emptyRestoreBackupsLabel = new Label();
			restoreStatusLabel = new Label();
			schedulesTabPage = new TabPage();
			schedulesTabPanel = new Panel();
			mainStatusStrip.SuspendLayout();
			mainTabControl.SuspendLayout();
			backupTabPage.SuspendLayout();
			backupJobsPanel.SuspendLayout();
			activityTabPage.SuspendLayout();
			mountBackupsTabPage.SuspendLayout();
			mountRootLayout.SuspendLayout();
			verifyTabPage.SuspendLayout();
			verifyRootLayout.SuspendLayout();
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
			mainStatusStrip.Location = new Point(0, 561);
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
			mainTabControl.Size = new Size(805, 537);
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
			backupTabPage.Size = new Size(797, 501);
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
			backupJobsPanel.Size = new Size(787, 491);
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
			activityTabPage.Size = new Size(797, 501);
			activityTabPage.TabIndex = 1;
			activityTabPage.Text = "Activity";
			activityTabPage.UseVisualStyleBackColor = true;
			activityTabPage.Enter += ActivityTabPage_Enter;
			// 
			// activityTabPanel
			// 
			activityTabPanel.Dock = DockStyle.Fill;
			activityTabPanel.Location = new Point(5, 5);
			activityTabPanel.Name = "activityTabPanel";
			activityTabPanel.Padding = new Padding(10);
			activityTabPanel.Size = new Size(787, 491);
			activityTabPanel.TabIndex = 0;
			// 
			// mountBackupsTabPage
			// 
			mountBackupsTabPage.Controls.Add(mountBackupsListView);
			mountBackupsTabPage.Location = new Point(4, 32);
			mountBackupsTabPage.Name = "mountBackupsTabPage";
			mountBackupsTabPage.Padding = new Padding(5);
			mountBackupsTabPage.Size = new Size(797, 497);
			mountBackupsTabPage.TabIndex = 2;
			mountBackupsTabPage.Text = "Mount Backups";
			mountBackupsTabPage.UseVisualStyleBackColor = true;
			// 
			// mountBackupsListView
			// 
			mountBackupsListView.Dock = DockStyle.Fill;
			mountBackupsListView.Location = new Point(5, 5);
			mountBackupsListView.Name = "mountBackupsListView";
			mountBackupsListView.Size = new Size(787, 487);
			mountBackupsListView.TabIndex = 0;
			mountBackupsListView.UseCompatibleStateImageBehavior = false;
			// 
			// verifyTabPage
			// 
			verifyTabPage.Controls.Add(verifyBackupsListView);
			verifyTabPage.Location = new Point(4, 32);
			verifyTabPage.Name = "verifyTabPage";
			verifyTabPage.Padding = new Padding(5);
			verifyTabPage.Size = new Size(797, 497);
			verifyTabPage.TabIndex = 3;
			verifyTabPage.Text = "Verify";
			verifyTabPage.UseVisualStyleBackColor = true;
			// 
			// verifyBackupsListView
			// 
			verifyBackupsListView.Dock = DockStyle.Fill;
			verifyBackupsListView.Location = new Point(5, 5);
			verifyBackupsListView.Name = "verifyBackupsListView";
			verifyBackupsListView.Size = new Size(787, 487);
			verifyBackupsListView.TabIndex = 0;
			verifyBackupsListView.UseCompatibleStateImageBehavior = false;
			// 
			// restoreTabPage
			// 
			restoreTabPage.Controls.Add(restoreRootLayout);
			restoreTabPage.Location = new Point(4, 32);
			restoreTabPage.Name = "restoreTabPage";
			restoreTabPage.Padding = new Padding(5);
			restoreTabPage.Size = new Size(797, 497);
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
			restoreRootLayout.Size = new Size(787, 487);
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
			restoreBackupsListView.Size = new Size(763, 403);
			restoreBackupsListView.TabIndex = 1;
			restoreBackupsListView.UseCompatibleStateImageBehavior = false;
			restoreBackupsListView.DoubleClick += RestoreBackupsListView_DoubleClick;
			// 
			// emptyRestoreBackupsLabel
			// 
			emptyRestoreBackupsLabel.AutoSize = true;
			emptyRestoreBackupsLabel.Location = new Point(18, 431);
			emptyRestoreBackupsLabel.Margin = new Padding(9, 7, 9, 7);
			emptyRestoreBackupsLabel.Name = "emptyRestoreBackupsLabel";
			emptyRestoreBackupsLabel.Size = new Size(205, 17);
			emptyRestoreBackupsLabel.TabIndex = 2;
			emptyRestoreBackupsLabel.Text = "No restore backups are available.";
			// 
			// restoreStatusLabel
			// 
			restoreStatusLabel.AutoSize = true;
			restoreStatusLabel.Location = new Point(18, 455);
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
			schedulesTabPage.Size = new Size(797, 497);
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
			schedulesTabPanel.Size = new Size(787, 487);
			schedulesTabPanel.TabIndex = 0;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(805, 583);
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
			mountBackupsTabPage.ResumeLayout(false);
			mountRootLayout.ResumeLayout(false);
			mountRootLayout.PerformLayout();
			verifyTabPage.ResumeLayout(false);
			verifyRootLayout.ResumeLayout(false);
			verifyRootLayout.PerformLayout();
			restoreTabPage.ResumeLayout(false);
			restoreRootLayout.ResumeLayout(false);
			restoreRootLayout.PerformLayout();
			schedulesTabPage.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
