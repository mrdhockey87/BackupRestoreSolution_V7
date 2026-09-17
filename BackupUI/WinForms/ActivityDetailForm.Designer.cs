using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class ActivityDetailForm
	{
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
			topActionsPanel = new FlowLayoutPanel();
			refreshButton = new Button();
			exportCsvButton = new Button();
			exportTextButton = new Button();
			deleteButton = new Button();
			copyButton = new Button();
			selectAllButton = new Button();
			clearSelectionButton = new Button();
			filterLabel = new Label();
			filterComboBox = new ComboBox();
			selectionCountLabel = new Label();
			activitiesGrid = new DataGridView();
			timestampColumn = new DataGridViewTextBoxColumn();
			jobColumn = new DataGridViewTextBoxColumn();
			levelColumn = new DataGridViewTextBoxColumn();
			messageColumn = new DataGridViewTextBoxColumn();
			detailsColumn = new DataGridViewTextBoxColumn();
			backupPathColumn = new DataGridViewTextBoxColumn();
			activitiesContextMenuStrip = new ContextMenuStrip(components);
			exportCsvToolStripMenuItem = new ToolStripMenuItem();
			exportTextToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			copySelectedToolStripMenuItem = new ToolStripMenuItem();
			deleteSelectedToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			selectAllToolStripMenuItem = new ToolStripMenuItem();
			clearSelectionToolStripMenuItem = new ToolStripMenuItem();
			statusLabel = new Label();
			topActionsPanel.SuspendLayout();
			((ISupportInitialize)activitiesGrid).BeginInit();
			activitiesContextMenuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// titleLabel
			// 
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(78, 15);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "All Activities";
			// 
			// topActionsPanel
			// 
			topActionsPanel.Controls.Add(refreshButton);
			topActionsPanel.Controls.Add(exportCsvButton);
			topActionsPanel.Controls.Add(exportTextButton);
			topActionsPanel.Controls.Add(deleteButton);
			topActionsPanel.Controls.Add(copyButton);
			topActionsPanel.Controls.Add(selectAllButton);
			topActionsPanel.Controls.Add(clearSelectionButton);
			topActionsPanel.Location = new Point(16, 48);
			topActionsPanel.Name = "topActionsPanel";
			topActionsPanel.Size = new Size(952, 36);
			topActionsPanel.TabIndex = 1;
			topActionsPanel.WrapContents = true;
			// 
			// refreshButton
			// 
			refreshButton.AutoSize = true;
			refreshButton.Margin = new Padding(0, 0, 8, 0);
			refreshButton.MinimumSize = new Size(100, 30);
			refreshButton.Name = "refreshButton";
			refreshButton.Size = new Size(100, 30);
			refreshButton.TabIndex = 0;
			refreshButton.Text = "Refresh";
			refreshButton.UseVisualStyleBackColor = true;
			refreshButton.Click += RefreshButton_Click;
			// 
			// exportCsvButton
			// 
			exportCsvButton.AutoSize = true;
			exportCsvButton.Margin = new Padding(0, 0, 8, 0);
			exportCsvButton.MinimumSize = new Size(100, 30);
			exportCsvButton.Name = "exportCsvButton";
			exportCsvButton.Size = new Size(100, 30);
			exportCsvButton.TabIndex = 1;
			exportCsvButton.Text = "Export CSV";
			exportCsvButton.UseVisualStyleBackColor = true;
			exportCsvButton.Click += ExportCsvButton_Click;
			// 
			// exportTextButton
			// 
			exportTextButton.AutoSize = true;
			exportTextButton.Margin = new Padding(0, 0, 8, 0);
			exportTextButton.MinimumSize = new Size(100, 30);
			exportTextButton.Name = "exportTextButton";
			exportTextButton.Size = new Size(100, 30);
			exportTextButton.TabIndex = 2;
			exportTextButton.Text = "Export Text";
			exportTextButton.UseVisualStyleBackColor = true;
			exportTextButton.Click += ExportTextButton_Click;
			// 
			// deleteButton
			// 
			deleteButton.AutoSize = true;
			deleteButton.Margin = new Padding(0, 0, 8, 0);
			deleteButton.MinimumSize = new Size(100, 30);
			deleteButton.Name = "deleteButton";
			deleteButton.Size = new Size(111, 30);
			deleteButton.TabIndex = 3;
			deleteButton.Text = "Delete Selected";
			deleteButton.UseVisualStyleBackColor = true;
			deleteButton.Click += DeleteButton_Click;
			// 
			// copyButton
			// 
			copyButton.AutoSize = true;
			copyButton.Margin = new Padding(0, 0, 8, 0);
			copyButton.MinimumSize = new Size(100, 30);
			copyButton.Name = "copyButton";
			copyButton.Size = new Size(108, 30);
			copyButton.TabIndex = 4;
			copyButton.Text = "Copy Selected";
			copyButton.UseVisualStyleBackColor = true;
			copyButton.Click += CopyButton_Click;
			// 
			// selectAllButton
			// 
			selectAllButton.AutoSize = true;
			selectAllButton.Margin = new Padding(0, 0, 8, 0);
			selectAllButton.MinimumSize = new Size(100, 30);
			selectAllButton.Name = "selectAllButton";
			selectAllButton.Size = new Size(100, 30);
			selectAllButton.TabIndex = 5;
			selectAllButton.Text = "Select All";
			selectAllButton.UseVisualStyleBackColor = true;
			selectAllButton.Click += SelectAllButton_Click;
			// 
			// clearSelectionButton
			// 
			clearSelectionButton.AutoSize = true;
			clearSelectionButton.Margin = new Padding(0, 0, 8, 0);
			clearSelectionButton.MinimumSize = new Size(100, 30);
			clearSelectionButton.Name = "clearSelectionButton";
			clearSelectionButton.Size = new Size(111, 30);
			clearSelectionButton.TabIndex = 6;
			clearSelectionButton.Text = "Clear Selection";
			clearSelectionButton.UseVisualStyleBackColor = true;
			clearSelectionButton.Click += ClearSelectionButton_Click;
			// 
			// filterLabel
			// 
			filterLabel.AutoSize = true;
			filterLabel.Location = new Point(16, 96);
			filterLabel.Name = "filterLabel";
			filterLabel.Size = new Size(36, 15);
			filterLabel.TabIndex = 2;
			filterLabel.Text = "Level:";
			// 
			// filterComboBox
			// 
			filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			filterComboBox.FormattingEnabled = true;
			filterComboBox.Items.AddRange(new object[] { "All", "Info", "Success", "Warning", "Error" });
			filterComboBox.Location = new Point(66, 92);
			filterComboBox.Name = "filterComboBox";
			filterComboBox.Size = new Size(140, 23);
			filterComboBox.TabIndex = 3;
			filterComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
			// 
			// selectionCountLabel
			// 
			selectionCountLabel.AutoSize = true;
			selectionCountLabel.Location = new Point(230, 96);
			selectionCountLabel.Name = "selectionCountLabel";
			selectionCountLabel.Size = new Size(113, 15);
			selectionCountLabel.TabIndex = 4;
			selectionCountLabel.Text = "0 activities selected";
			// 
			// activitiesGrid
			// 
			activitiesGrid.AllowUserToAddRows = false;
			activitiesGrid.AllowUserToDeleteRows = false;
			activitiesGrid.AutoGenerateColumns = false;
			activitiesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			activitiesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			activitiesGrid.Columns.AddRange(new DataGridViewColumn[] {
				timestampColumn,
				jobColumn,
				levelColumn,
				messageColumn,
				detailsColumn,
				backupPathColumn});
			activitiesGrid.ContextMenuStrip = activitiesContextMenuStrip;
			activitiesGrid.Location = new Point(16, 128);
			activitiesGrid.MultiSelect = true;
			activitiesGrid.Name = "activitiesGrid";
			activitiesGrid.ReadOnly = true;
			activitiesGrid.RowTemplate.Height = 25;
			activitiesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			activitiesGrid.Size = new Size(952, 430);
			activitiesGrid.TabIndex = 5;
			activitiesGrid.SelectionChanged += ActivitiesGrid_SelectionChanged;
			// 
			// timestampColumn
			// 
			timestampColumn.DataPropertyName = "Timestamp";
			timestampColumn.FillWeight = 140F;
			timestampColumn.HeaderText = "Timestamp";
			timestampColumn.Name = "timestampColumn";
			timestampColumn.ReadOnly = true;
			// 
			// jobColumn
			// 
			jobColumn.DataPropertyName = "JobName";
			jobColumn.FillWeight = 120F;
			jobColumn.HeaderText = "Job";
			jobColumn.Name = "jobColumn";
			jobColumn.ReadOnly = true;
			// 
			// levelColumn
			// 
			levelColumn.DataPropertyName = "Level";
			levelColumn.FillWeight = 70F;
			levelColumn.HeaderText = "Level";
			levelColumn.Name = "levelColumn";
			levelColumn.ReadOnly = true;
			// 
			// messageColumn
			// 
			messageColumn.DataPropertyName = "Message";
			messageColumn.FillWeight = 220F;
			messageColumn.HeaderText = "Message";
			messageColumn.Name = "messageColumn";
			messageColumn.ReadOnly = true;
			// 
			// detailsColumn
			// 
			detailsColumn.DataPropertyName = "Details";
			detailsColumn.FillWeight = 220F;
			detailsColumn.HeaderText = "Details";
			detailsColumn.Name = "detailsColumn";
			detailsColumn.ReadOnly = true;
			// 
			// backupPathColumn
			// 
			backupPathColumn.DataPropertyName = "BackupPath";
			backupPathColumn.FillWeight = 180F;
			backupPathColumn.HeaderText = "Backup Path";
			backupPathColumn.Name = "backupPathColumn";
			backupPathColumn.ReadOnly = true;
			// 
			// activitiesContextMenuStrip
			// 
			activitiesContextMenuStrip.Items.AddRange(new ToolStripItem[] {
				exportCsvToolStripMenuItem,
				exportTextToolStripMenuItem,
				toolStripSeparator1,
				copySelectedToolStripMenuItem,
				deleteSelectedToolStripMenuItem,
				toolStripSeparator2,
				selectAllToolStripMenuItem,
				clearSelectionToolStripMenuItem});
			activitiesContextMenuStrip.Name = "activitiesContextMenuStrip";
			activitiesContextMenuStrip.Size = new Size(163, 148);
			// 
			// exportCsvToolStripMenuItem
			// 
			exportCsvToolStripMenuItem.Name = "exportCsvToolStripMenuItem";
			exportCsvToolStripMenuItem.Size = new Size(162, 22);
			exportCsvToolStripMenuItem.Text = "Export CSV";
			exportCsvToolStripMenuItem.Click += ExportCsvToolStripMenuItem_Click;
			// 
			// exportTextToolStripMenuItem
			// 
			exportTextToolStripMenuItem.Name = "exportTextToolStripMenuItem";
			exportTextToolStripMenuItem.Size = new Size(162, 22);
			exportTextToolStripMenuItem.Text = "Export Text";
			exportTextToolStripMenuItem.Click += ExportTextToolStripMenuItem_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(159, 6);
			// 
			// copySelectedToolStripMenuItem
			// 
			copySelectedToolStripMenuItem.Name = "copySelectedToolStripMenuItem";
			copySelectedToolStripMenuItem.Size = new Size(162, 22);
			copySelectedToolStripMenuItem.Text = "Copy Selected";
			copySelectedToolStripMenuItem.Click += CopySelectedToolStripMenuItem_Click;
			// 
			// deleteSelectedToolStripMenuItem
			// 
			deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
			deleteSelectedToolStripMenuItem.Size = new Size(162, 22);
			deleteSelectedToolStripMenuItem.Text = "Delete Selected";
			deleteSelectedToolStripMenuItem.Click += DeleteSelectedToolStripMenuItem_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(159, 6);
			// 
			// selectAllToolStripMenuItem
			// 
			selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			selectAllToolStripMenuItem.Size = new Size(162, 22);
			selectAllToolStripMenuItem.Text = "Select All";
			selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItem_Click;
			// 
			// clearSelectionToolStripMenuItem
			// 
			clearSelectionToolStripMenuItem.Name = "clearSelectionToolStripMenuItem";
			clearSelectionToolStripMenuItem.Size = new Size(162, 22);
			clearSelectionToolStripMenuItem.Text = "Clear Selection";
			clearSelectionToolStripMenuItem.Click += ClearSelectionToolStripMenuItem_Click;
			// 
			// statusLabel
			// 
			statusLabel.ForeColor = Color.DimGray;
			statusLabel.Location = new Point(16, 570);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(952, 40);
			statusLabel.TabIndex = 6;
			// 
			// ActivityDetailForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(1000, 620);
			Controls.Add(statusLabel);
			Controls.Add(activitiesGrid);
			Controls.Add(selectionCountLabel);
			Controls.Add(filterComboBox);
			Controls.Add(filterLabel);
			Controls.Add(topActionsPanel);
			Controls.Add(titleLabel);
			MinimumSize = new Size(1000, 620);
			Name = "ActivityDetailForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "All Activity Details";
			topActionsPanel.ResumeLayout(false);
			topActionsPanel.PerformLayout();
			((ISupportInitialize)activitiesGrid).EndInit();
			activitiesContextMenuStrip.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;
		private Label titleLabel;
		private FlowLayoutPanel topActionsPanel;
		private Button refreshButton;
		private Button exportCsvButton;
		private Button exportTextButton;
		private Button deleteButton;
		private Button copyButton;
		private Button selectAllButton;
		private Button clearSelectionButton;
		private Label filterLabel;
		private ComboBox filterComboBox;
		private Label selectionCountLabel;
		private DataGridView activitiesGrid;
		private ContextMenuStrip activitiesContextMenuStrip;
		private ToolStripMenuItem exportCsvToolStripMenuItem;
		private ToolStripMenuItem exportTextToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem copySelectedToolStripMenuItem;
		private ToolStripMenuItem deleteSelectedToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem selectAllToolStripMenuItem;
		private ToolStripMenuItem clearSelectionToolStripMenuItem;
		private Label statusLabel;
		private DataGridViewTextBoxColumn timestampColumn;
		private DataGridViewTextBoxColumn jobColumn;
		private DataGridViewTextBoxColumn levelColumn;
		private DataGridViewTextBoxColumn messageColumn;
		private DataGridViewTextBoxColumn detailsColumn;
		private DataGridViewTextBoxColumn backupPathColumn;
	}
}
