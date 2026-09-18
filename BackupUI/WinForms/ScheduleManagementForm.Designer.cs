using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class ScheduleManagementForm
	{
		private IContainer components;
		private Label titleLabel;
		private FlowLayoutPanel actionsPanel;
		private Button refreshButton;
		private Button editNextRunButton;
		private Button editJobButton;
		private Button deleteJobButton;
		private Button runNowButton;
		private DataGridView jobsGrid;
		private DataGridViewTextBoxColumn jobNameColumn;
		private DataGridViewTextBoxColumn typeColumn;
		private DataGridViewTextBoxColumn destinationColumn;
		private DataGridViewCheckBoxColumn runningColumn;
		private DataGridViewTextBoxColumn lastRunColumn;
		private DataGridViewTextBoxColumn nextRunColumn;
		private Button closeButton;

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
			editNextRunButton = new Button();
			editJobButton = new Button();
			deleteJobButton = new Button();
			runNowButton = new Button();
			jobsGrid = new DataGridView();
			jobNameColumn = new DataGridViewTextBoxColumn();
			typeColumn = new DataGridViewTextBoxColumn();
			destinationColumn = new DataGridViewTextBoxColumn();
			runningColumn = new DataGridViewCheckBoxColumn();
			lastRunColumn = new DataGridViewTextBoxColumn();
			nextRunColumn = new DataGridViewTextBoxColumn();
			closeButton = new Button();
			actionsPanel.SuspendLayout();
			((ISupportInitialize)jobsGrid).BeginInit();
			SuspendLayout();
			// titleLabel
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(155, 15);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Manage Scheduled Backup Jobs";
			// actionsPanel
			actionsPanel.Controls.Add(refreshButton);
			actionsPanel.Controls.Add(editNextRunButton);
			actionsPanel.Controls.Add(editJobButton);
			actionsPanel.Controls.Add(deleteJobButton);
			actionsPanel.Controls.Add(runNowButton);
			actionsPanel.Location = new Point(16, 48);
			actionsPanel.Name = "actionsPanel";
			actionsPanel.Size = new Size(940, 36);
			actionsPanel.TabIndex = 1;
			actionsPanel.WrapContents = true;
			// refreshButton
			refreshButton.AutoSize = true;
			refreshButton.Margin = new Padding(0, 0, 8, 0);
			refreshButton.MinimumSize = new Size(100, 30);
			refreshButton.Name = "refreshButton";
			refreshButton.Size = new Size(100, 30);
			refreshButton.TabIndex = 0;
			refreshButton.Text = "Refresh";
			refreshButton.UseVisualStyleBackColor = true;
			refreshButton.Click += RefreshButton_Click;
			// editNextRunButton
			editNextRunButton.AutoSize = true;
			editNextRunButton.Margin = new Padding(0, 0, 8, 0);
			editNextRunButton.MinimumSize = new Size(100, 30);
			editNextRunButton.Name = "editNextRunButton";
			editNextRunButton.Size = new Size(100, 30);
			editNextRunButton.TabIndex = 1;
			editNextRunButton.Text = "Edit Next Run";
			editNextRunButton.UseVisualStyleBackColor = true;
			editNextRunButton.Click += EditNextRunButton_Click;
			// editJobButton
			editJobButton.AutoSize = true;
			editJobButton.Margin = new Padding(0, 0, 8, 0);
			editJobButton.MinimumSize = new Size(100, 30);
			editJobButton.Name = "editJobButton";
			editJobButton.Size = new Size(100, 30);
			editJobButton.TabIndex = 2;
			editJobButton.Text = "Edit Job";
			editJobButton.UseVisualStyleBackColor = true;
			editJobButton.Click += EditJobButton_Click;
			// deleteJobButton
			deleteJobButton.AutoSize = true;
			deleteJobButton.Margin = new Padding(0, 0, 8, 0);
			deleteJobButton.MinimumSize = new Size(100, 30);
			deleteJobButton.Name = "deleteJobButton";
			deleteJobButton.Size = new Size(100, 30);
			deleteJobButton.TabIndex = 3;
			deleteJobButton.Text = "Delete Job";
			deleteJobButton.UseVisualStyleBackColor = true;
			deleteJobButton.Click += DeleteJobButton_Click;
			// runNowButton
			runNowButton.AutoSize = true;
			runNowButton.Margin = new Padding(0, 0, 8, 0);
			runNowButton.MinimumSize = new Size(100, 30);
			runNowButton.Name = "runNowButton";
			runNowButton.Size = new Size(100, 30);
			runNowButton.TabIndex = 4;
			runNowButton.Text = "Run Now";
			runNowButton.UseVisualStyleBackColor = true;
			runNowButton.Click += RunNowButton_Click;
			// jobsGrid
			jobsGrid.AllowUserToAddRows = false;
			jobsGrid.AllowUserToDeleteRows = false;
			jobsGrid.AutoGenerateColumns = false;
			jobsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			jobsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			jobsGrid.Columns.AddRange(new DataGridViewColumn[] { jobNameColumn, typeColumn, destinationColumn, runningColumn, lastRunColumn, nextRunColumn });
			jobsGrid.Location = new Point(16, 96);
			jobsGrid.MultiSelect = false;
			jobsGrid.Name = "jobsGrid";
			jobsGrid.ReadOnly = true;
			jobsGrid.RowTemplate.Height = 25;
			jobsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			jobsGrid.Size = new Size(940, 420);
			jobsGrid.TabIndex = 2;
			jobsGrid.DoubleClick += JobsGrid_DoubleClick;
			// columns
			jobNameColumn.DataPropertyName = "Name";
			jobNameColumn.FillWeight = 180F;
			jobNameColumn.HeaderText = "Job Name";
			jobNameColumn.Name = "jobNameColumn";
			jobNameColumn.ReadOnly = true;
			typeColumn.DataPropertyName = "Type";
			typeColumn.FillWeight = 80F;
			typeColumn.HeaderText = "Type";
			typeColumn.Name = "typeColumn";
			typeColumn.ReadOnly = true;
			destinationColumn.DataPropertyName = "DestinationPath";
			destinationColumn.FillWeight = 220F;
			destinationColumn.HeaderText = "Destination";
			destinationColumn.Name = "destinationColumn";
			destinationColumn.ReadOnly = true;
			runningColumn.DataPropertyName = "IsCurrentlyRunning";
			runningColumn.FillWeight = 60F;
			runningColumn.HeaderText = "Running";
			runningColumn.Name = "runningColumn";
			runningColumn.ReadOnly = true;
			lastRunColumn.DataPropertyName = "LastRunTime";
			lastRunColumn.FillWeight = 110F;
			lastRunColumn.HeaderText = "Last Run";
			lastRunColumn.Name = "lastRunColumn";
			lastRunColumn.ReadOnly = true;
			nextRunColumn.DataPropertyName = "NextScheduledRun";
			nextRunColumn.FillWeight = 110F;
			nextRunColumn.HeaderText = "Next Run";
			nextRunColumn.Name = "nextRunColumn";
			nextRunColumn.ReadOnly = true;
			// closeButton
			closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			closeButton.DialogResult = DialogResult.OK;
			closeButton.Location = new Point(856, 524);
			closeButton.Name = "closeButton";
			closeButton.Size = new Size(100, 32);
			closeButton.TabIndex = 3;
			closeButton.Text = "Close";
			closeButton.UseVisualStyleBackColor = true;
			// ScheduleManagementForm
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(980, 560);
			Controls.Add(closeButton);
			Controls.Add(jobsGrid);
			Controls.Add(actionsPanel);
			Controls.Add(titleLabel);
			MinimumSize = new Size(980, 560);
			Name = "ScheduleManagementForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Schedule Management";
			actionsPanel.ResumeLayout(false);
			actionsPanel.PerformLayout();
			((ISupportInitialize)jobsGrid).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
