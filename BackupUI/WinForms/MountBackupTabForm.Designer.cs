namespace SecureServerBackup.WinForms
{
	partial class MountBackupTabForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			mountTabPanel = new System.Windows.Forms.Panel();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			lblNoBackups = new System.Windows.Forms.Label();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			label1 = new System.Windows.Forms.Label();
			RefreshMounts = new System.Windows.Forms.Button();
			BrowseBackup = new System.Windows.Forms.Button();
			UnmountAll = new System.Windows.Forms.Button();
			label2 = new System.Windows.Forms.Label();
			dgMountedBackups = new System.Windows.Forms.DataGridView();
			MountedBackupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			MountedBackupType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			MountedIsEncrypted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			MountedBackupDate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			MountedBackupPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			UnmountBtn = new System.Windows.Forms.DataGridViewButtonColumn();
			label3 = new System.Windows.Forms.Label();
			dgAvailableBackups = new System.Windows.Forms.DataGridView();
			pnlBackupPoints = new System.Windows.Forms.FlowLayoutPanel();
			lblSelectPoints = new System.Windows.Forms.Label();
			cmbBackupPoints = new System.Windows.Forms.ComboBox();
			BackupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			BackupType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			IsEncrypted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			BackupDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			BackupPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Mount = new System.Windows.Forms.DataGridViewButtonColumn();
			mountTabPanel.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgMountedBackups).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgAvailableBackups).BeginInit();
			pnlBackupPoints.SuspendLayout();
			SuspendLayout();
			// 
			// mountTabPanel
			// 
			mountTabPanel.Controls.Add(tableLayoutPanel1);
			mountTabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			mountTabPanel.Location = new System.Drawing.Point(0, 0);
			mountTabPanel.Name = "mountTabPanel";
			mountTabPanel.Padding = new System.Windows.Forms.Padding(10);
			mountTabPanel.Size = new System.Drawing.Size(800, 495);
			mountTabPanel.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(lblNoBackups, 0, 6);
			tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
			tableLayoutPanel1.Controls.Add(label2, 0, 4);
			tableLayoutPanel1.Controls.Add(dgMountedBackups, 0, 5);
			tableLayoutPanel1.Controls.Add(label3, 0, 1);
			tableLayoutPanel1.Controls.Add(dgAvailableBackups, 0, 2);
			tableLayoutPanel1.Controls.Add(pnlBackupPoints, 0, 3);
			tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 7;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			tableLayoutPanel1.Size = new System.Drawing.Size(780, 475);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// lblNoBackups
			// 
			lblNoBackups.AutoSize = true;
			lblNoBackups.Location = new System.Drawing.Point(3, 455);
			lblNoBackups.Name = "lblNoBackups";
			lblNoBackups.Size = new System.Drawing.Size(133, 17);
			lblNoBackups.TabIndex = 1;
			lblNoBackups.Text = "No Backups Available";
			lblNoBackups.Visible = false;
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.Controls.Add(label1);
			flowLayoutPanel1.Controls.Add(RefreshMounts);
			flowLayoutPanel1.Controls.Add(BrowseBackup);
			flowLayoutPanel1.Controls.Add(UnmountAll);
			flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(774, 35);
			flowLayoutPanel1.TabIndex = 6;
			// 
			// label1
			// 
			label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			label1.Location = new System.Drawing.Point(5, 5);
			label1.Margin = new System.Windows.Forms.Padding(5);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(289, 25);
			label1.TabIndex = 3;
			label1.Text = "Mount Backups as Virtual Drives";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RefreshMounts
			// 
			RefreshMounts.Location = new System.Drawing.Point(304, 5);
			RefreshMounts.Margin = new System.Windows.Forms.Padding(5);
			RefreshMounts.Name = "RefreshMounts";
			RefreshMounts.Size = new System.Drawing.Size(83, 25);
			RefreshMounts.TabIndex = 4;
			RefreshMounts.Text = "Refresh";
			RefreshMounts.UseVisualStyleBackColor = true;
			// 
			// BrowseBackup
			// 
			BrowseBackup.Location = new System.Drawing.Point(397, 5);
			BrowseBackup.Margin = new System.Windows.Forms.Padding(5);
			BrowseBackup.Name = "BrowseBackup";
			BrowseBackup.Size = new System.Drawing.Size(83, 25);
			BrowseBackup.TabIndex = 5;
			BrowseBackup.Text = "Browse...";
			BrowseBackup.UseVisualStyleBackColor = true;
			// 
			// UnmountAll
			// 
			UnmountAll.Location = new System.Drawing.Point(490, 5);
			UnmountAll.Margin = new System.Windows.Forms.Padding(5);
			UnmountAll.Name = "UnmountAll";
			UnmountAll.Size = new System.Drawing.Size(83, 25);
			UnmountAll.TabIndex = 6;
			UnmountAll.Text = "Unmount All";
			UnmountAll.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(3, 266);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(112, 17);
			label2.TabIndex = 7;
			label2.Text = "Mounted Backups";
			// 
			// dgMountedBackups
			// 
			dgMountedBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgMountedBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { MountedBackupName, MountedBackupType, MountedIsEncrypted, MountedBackupDate, MountedBackupPath, UnmountBtn });
			dgMountedBackups.Dock = System.Windows.Forms.DockStyle.Fill;
			dgMountedBackups.Location = new System.Drawing.Point(3, 290);
			dgMountedBackups.Name = "dgMountedBackups";
			dgMountedBackups.RowHeadersWidth = 45;
			dgMountedBackups.Size = new System.Drawing.Size(774, 162);
			dgMountedBackups.TabIndex = 8;
			// 
			// MountedBackupName
			// 
			MountedBackupName.HeaderText = "Backup Name";
			MountedBackupName.MinimumWidth = 6;
			MountedBackupName.Name = "MountedBackupName";
			MountedBackupName.Width = 110;
			// 
			// MountedBackupType
			// 
			MountedBackupType.HeaderText = "Mounted Backup Type";
			MountedBackupType.MinimumWidth = 6;
			MountedBackupType.Name = "MountedBackupType";
			MountedBackupType.Width = 110;
			// 
			// MountedIsEncrypted
			// 
			MountedIsEncrypted.HeaderText = "Encrypted";
			MountedIsEncrypted.MinimumWidth = 6;
			MountedIsEncrypted.Name = "MountedIsEncrypted";
			MountedIsEncrypted.Width = 110;
			// 
			// MountedBackupDate
			// 
			MountedBackupDate.HeaderText = "Backup Date";
			MountedBackupDate.MinimumWidth = 6;
			MountedBackupDate.Name = "MountedBackupDate";
			MountedBackupDate.Width = 110;
			// 
			// MountedBackupPath
			// 
			MountedBackupPath.HeaderText = "Backup Path";
			MountedBackupPath.MinimumWidth = 6;
			MountedBackupPath.Name = "MountedBackupPath";
			MountedBackupPath.Width = 110;
			// 
			// UnmountBtn
			// 
			UnmountBtn.HeaderText = "Unmount";
			UnmountBtn.MinimumWidth = 6;
			UnmountBtn.Name = "UnmountBtn";
			UnmountBtn.Width = 110;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(3, 41);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(111, 17);
			label3.TabIndex = 9;
			label3.Text = "Available Backups";
			// 
			// dgAvailableBackups
			// 
			dgAvailableBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgAvailableBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { BackupName, BackupType, IsEncrypted, BackupDate, BackupPath, Mount });
			dgAvailableBackups.Dock = System.Windows.Forms.DockStyle.Fill;
			dgAvailableBackups.Location = new System.Drawing.Point(3, 61);
			dgAvailableBackups.Name = "dgAvailableBackups";
			dgAvailableBackups.RowHeadersWidth = 45;
			dgAvailableBackups.Size = new System.Drawing.Size(774, 162);
			dgAvailableBackups.TabIndex = 10;
			// 
			// pnlBackupPoints
			// 
			pnlBackupPoints.Controls.Add(lblSelectPoints);
			pnlBackupPoints.Controls.Add(cmbBackupPoints);
			pnlBackupPoints.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlBackupPoints.Location = new System.Drawing.Point(3, 229);
			pnlBackupPoints.Name = "pnlBackupPoints";
			pnlBackupPoints.Size = new System.Drawing.Size(774, 34);
			pnlBackupPoints.TabIndex = 11;
			// 
			// lblSelectPoints
			// 
			lblSelectPoints.AutoSize = true;
			lblSelectPoints.Font = new System.Drawing.Font("Segoe UI", 8.830189F, System.Drawing.FontStyle.Bold);
			lblSelectPoints.Location = new System.Drawing.Point(20, 10);
			lblSelectPoints.Margin = new System.Windows.Forms.Padding(20, 10, 0, 10);
			lblSelectPoints.Name = "lblSelectPoints";
			lblSelectPoints.Size = new System.Drawing.Size(133, 17);
			lblSelectPoints.TabIndex = 12;
			lblSelectPoints.Text = "Select Backup Point:";
			// 
			// cmbBackupPoints
			// 
			cmbBackupPoints.FormattingEnabled = true;
			cmbBackupPoints.Location = new System.Drawing.Point(156, 3);
			cmbBackupPoints.Name = "cmbBackupPoints";
			cmbBackupPoints.Size = new System.Drawing.Size(294, 25);
			cmbBackupPoints.TabIndex = 13;
			// 
			// BackupName
			// 
			BackupName.HeaderText = "Backup Name";
			BackupName.MinimumWidth = 6;
			BackupName.Name = "BackupName";
			BackupName.Width = 110;
			// 
			// BackupType
			// 
			BackupType.HeaderText = "Backup Type";
			BackupType.MinimumWidth = 6;
			BackupType.Name = "BackupType";
			BackupType.Width = 110;
			// 
			// IsEncrypted
			// 
			IsEncrypted.HeaderText = "Encrypted";
			IsEncrypted.MinimumWidth = 6;
			IsEncrypted.Name = "IsEncrypted";
			IsEncrypted.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			IsEncrypted.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			IsEncrypted.Width = 110;
			// 
			// BackupDate
			// 
			BackupDate.HeaderText = "Backup Date";
			BackupDate.MinimumWidth = 6;
			BackupDate.Name = "BackupDate";
			BackupDate.Width = 110;
			// 
			// BackupPath
			// 
			BackupPath.HeaderText = "Backup Path";
			BackupPath.MinimumWidth = 6;
			BackupPath.Name = "BackupPath";
			BackupPath.Width = 110;
			// 
			// Mount
			// 
			Mount.HeaderText = "Mount";
			Mount.MinimumWidth = 6;
			Mount.Name = "Mount";
			Mount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			Mount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			Mount.Text = "Mount";
			Mount.Width = 110;
			// 
			// MountBackupTabForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(800, 495);
			Controls.Add(mountTabPanel);
			Name = "MountBackupTabForm";
			Text = "MountBackupTabForm";
			mountTabPanel.ResumeLayout(false);
			mountTabPanel.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgMountedBackups).EndInit();
			((System.ComponentModel.ISupportInitialize)dgAvailableBackups).EndInit();
			pnlBackupPoints.ResumeLayout(false);
			pnlBackupPoints.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Panel mountTabPanel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblNoBackups;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button RefreshMounts;
		private System.Windows.Forms.Button BrowseBackup;
		private System.Windows.Forms.Button UnmountAll;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridView dgMountedBackups;
		private System.Windows.Forms.DataGridViewTextBoxColumn MountedBackupName;
		private System.Windows.Forms.DataGridViewTextBoxColumn MountedBackupType;
		private System.Windows.Forms.DataGridViewCheckBoxColumn MountedIsEncrypted;
		private System.Windows.Forms.DataGridViewCheckBoxColumn MountedBackupDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn MountedBackupPath;
		private System.Windows.Forms.DataGridViewButtonColumn UnmountBtn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGridView dgAvailableBackups;
		private System.Windows.Forms.FlowLayoutPanel pnlBackupPoints;
		private System.Windows.Forms.Label lblSelectPoints;
		private System.Windows.Forms.ComboBox cmbBackupPoints;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupName;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupType;
		private System.Windows.Forms.DataGridViewCheckBoxColumn IsEncrypted;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupPath;
		private System.Windows.Forms.DataGridViewButtonColumn Mount;
	}
}