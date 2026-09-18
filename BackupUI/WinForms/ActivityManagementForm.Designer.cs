using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class ActivityManagementForm
	{
		private IContainer components;
		private Label titleLabel;
		private FlowLayoutPanel actionsPanel;
		private Button refreshButton;
		private Button viewAllActivitiesButton;
		private DataGridView jobsGrid;
		private ContextMenuStrip jobsContextMenuStrip;
		private ToolStripMenuItem viewDetailsToolStripMenuItem;
		private ToolStripMenuItem exportActivitiesToolStripMenuItem;
		private Label statusLabel;
		private DataGridViewTextBoxColumn jobNameColumn;
		private DataGridViewTextBoxColumn totalActivitiesColumn;
		private DataGridViewTextBoxColumn lastActivityColumn;
		private DataGridViewTextBoxColumn successCountColumn;
		private DataGridViewTextBoxColumn warningCountColumn;
		private DataGridViewTextBoxColumn errorCountColumn;
		private DataGridViewTextBoxColumn actionsColumn;

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
			titleLabel = new Label();
			actionsPanel = new FlowLayoutPanel();
			refreshButton = new Button();
			viewAllActivitiesButton = new Button();
			jobsGrid = new DataGridView();
			jobNameColumn = new DataGridViewTextBoxColumn();
			totalActivitiesColumn = new DataGridViewTextBoxColumn();
			lastActivityColumn = new DataGridViewTextBoxColumn();
			successCountColumn = new DataGridViewTextBoxColumn();
			warningCountColumn = new DataGridViewTextBoxColumn();
			errorCountColumn = new DataGridViewTextBoxColumn();
			actionsColumn = new DataGridViewTextBoxColumn();
			jobsContextMenuStrip = new ContextMenuStrip(components);
			viewDetailsToolStripMenuItem = new ToolStripMenuItem();
			exportActivitiesToolStripMenuItem = new ToolStripMenuItem();
			statusLabel = new Label();
			actionsPanel.SuspendLayout();
			((ISupportInitialize)jobsGrid).BeginInit();
			jobsContextMenuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// titleLabel
			// 
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(141, 15);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Backup Job Activity Logs";
			// 
			// actionsPanel
			// 
			actionsPanel.Controls.Add(refreshButton);
			actionsPanel.Controls.Add(viewAllActivitiesButton);
			actionsPanel.Location = new Point(16, 48);
			actionsPanel.Name = "actionsPanel";
			actionsPanel.Size = new Size(920, 36);
			actionsPanel.TabIndex = 1;
			actionsPanel.WrapContents = true;
			// 
			// refreshButton
			// 
			refreshButton.AutoSize = true;
			refreshButton.Margin = new Padding(0, 0, 8, 0);
			refreshButton.MinimumSize = new Size(120, 30);
			refreshButton.Name = "refreshButton";
			refreshButton.Size = new Size(120, 30);
			refreshButton.TabIndex = 0;
			refreshButton.Text = "Refresh";
			refreshButton.UseVisualStyleBackColor = true;
			refreshButton.Click += RefreshButton_Click;
			// 
			// viewAllActivitiesButton
			// 
			viewAllActivitiesButton.AutoSize = true;
			viewAllActivitiesButton.Margin = new Padding(0, 0, 8, 0);
			viewAllActivitiesButton.MinimumSize = new Size(120, 30);
			viewAllActivitiesButton.Name = "viewAllActivitiesButton";
			viewAllActivitiesButton.Size = new Size(129, 30);
			viewAllActivitiesButton.TabIndex = 1;
			viewAllActivitiesButton.Text = "View All Activities";
			viewAllActivitiesButton.UseVisualStyleBackColor = true;
			viewAllActivitiesButton.Click += ViewAllActivitiesButton_Click;
			// 
			// jobsGrid
			// 
			jobsGrid.AllowUserToAddRows = false;
			jobsGrid.AllowUserToDeleteRows = false;
			jobsGrid.AutoGenerateColumns = false;
			jobsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			jobsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			jobsGrid.Columns.AddRange(new DataGridViewColumn[] { jobNameColumn, totalActivitiesColumn, lastActivityColumn, successCountColumn, warningCountColumn, errorCountColumn, actionsColumn });
			jobsGrid.ContextMenuStrip = jobsContextMenuStrip;
			jobsGrid.Location = new Point(16, 96);
			jobsGrid.MultiSelect = false;
			jobsGrid.Name = "jobsGrid";
			jobsGrid.ReadOnly = true;
			jobsGrid.RowTemplate.Height = 25;
			jobsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			jobsGrid.Size = new Size(920, 456);
			jobsGrid.TabIndex = 2;
			jobsGrid.CellDoubleClick += JobsGrid_CellDoubleClick;
			jobsGrid.CellMouseClick += JobsGrid_CellMouseClick;
			jobsGrid.CellPainting += JobsGrid_CellPainting;
			// 
			// jobNameColumn
			// 
			jobNameColumn.DataPropertyName = "JobName";
			jobNameColumn.FillWeight = 180F;
			jobNameColumn.HeaderText = "Job Name";
			jobNameColumn.Name = "jobNameColumn";
			jobNameColumn.ReadOnly = true;
			// 
			// totalActivitiesColumn
			// 
			totalActivitiesColumn.DataPropertyName = "TotalActivities";
			totalActivitiesColumn.FillWeight = 70F;
			totalActivitiesColumn.HeaderText = "Total";
			totalActivitiesColumn.Name = "totalActivitiesColumn";
			totalActivitiesColumn.ReadOnly = true;
			// 
			// lastActivityColumn
			// 
			lastActivityColumn.DataPropertyName = "LastActivity";
			lastActivityColumn.FillWeight = 140F;
			lastActivityColumn.HeaderText = "Last Activity";
			lastActivityColumn.Name = "lastActivityColumn";
			lastActivityColumn.ReadOnly = true;
			// 
			// successCountColumn
			// 
			successCountColumn.DataPropertyName = "SuccessCount";
			successCountColumn.FillWeight = 70F;
			successCountColumn.HeaderText = "Success";
			successCountColumn.Name = "successCountColumn";
			successCountColumn.ReadOnly = true;
			// 
			// warningCountColumn
			// 
			warningCountColumn.DataPropertyName = "WarningCount";
			warningCountColumn.FillWeight = 70F;
			warningCountColumn.HeaderText = "Warning";
			warningCountColumn.Name = "warningCountColumn";
			warningCountColumn.ReadOnly = true;
			// 
			// errorCountColumn
			// 
			errorCountColumn.DataPropertyName = "ErrorCount";
			errorCountColumn.FillWeight = 70F;
			errorCountColumn.HeaderText = "Error";
			errorCountColumn.Name = "errorCountColumn";
			errorCountColumn.ReadOnly = true;
			// 
			// actionsColumn
			// 
			actionsColumn.FillWeight = 220F;
			actionsColumn.HeaderText = "Actions";
			actionsColumn.MinimumWidth = 220;
			actionsColumn.Name = "actionsColumn";
			actionsColumn.ReadOnly = true;
			actionsColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			// 
			// jobsContextMenuStrip
			// 
			jobsContextMenuStrip.ImageScalingSize = new Size(18, 18);
			jobsContextMenuStrip.Items.AddRange(new ToolStripItem[] { viewDetailsToolStripMenuItem, exportActivitiesToolStripMenuItem });
			jobsContextMenuStrip.Name = "jobsContextMenuStrip";
			jobsContextMenuStrip.Size = new Size(168, 48);
			// 
			// viewDetailsToolStripMenuItem
			// 
			viewDetailsToolStripMenuItem.Name = "viewDetailsToolStripMenuItem";
			viewDetailsToolStripMenuItem.Size = new Size(167, 22);
			viewDetailsToolStripMenuItem.Text = "View Details";
			viewDetailsToolStripMenuItem.Click += ViewDetailsToolStripMenuItem_Click;
			// 
			// exportActivitiesToolStripMenuItem
			// 
			exportActivitiesToolStripMenuItem.Name = "exportActivitiesToolStripMenuItem";
			exportActivitiesToolStripMenuItem.Size = new Size(167, 22);
			exportActivitiesToolStripMenuItem.Text = "Export Activities";
			exportActivitiesToolStripMenuItem.Click += ExportActivitiesToolStripMenuItem_Click;
			// 
			// statusLabel
			// 
			statusLabel.ForeColor = Color.DimGray;
			statusLabel.Location = new Point(16, 560);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(920, 24);
			statusLabel.TabIndex = 3;
			statusLabel.Visible = false;
			// 
			// ActivityManagementForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(960, 590);
			Controls.Add(statusLabel);
			Controls.Add(jobsGrid);
			Controls.Add(actionsPanel);
			Controls.Add(titleLabel);
			MinimumSize = new Size(960, 560);
			Name = "ActivityManagementForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Backup Job Activity Logs";
			actionsPanel.ResumeLayout(false);
			actionsPanel.PerformLayout();
			((ISupportInitialize)jobsGrid).EndInit();
			jobsContextMenuStrip.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
