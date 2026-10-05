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
			activityTabPanel = new System.Windows.Forms.Panel();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
			BackupNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			BackupTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			IsEncryptedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			BackupDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			BackupPathCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			MountCol = new System.Windows.Forms.DataGridViewButtonColumn();
			activityTabPanel.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgMountedBackups).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgAvailableBackups).BeginInit();
			SuspendLayout();
			// 
			// activityTabPanel
			// 
			activityTabPanel.Controls.Add(tableLayoutPanel1);
			activityTabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			activityTabPanel.Location = new System.Drawing.Point(0, 0);
			activityTabPanel.Name = "activityTabPanel";
			activityTabPanel.Padding = new System.Windows.Forms.Padding(10);
			activityTabPanel.Size = new System.Drawing.Size(800, 452);
			activityTabPanel.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
			tableLayoutPanel1.Controls.Add(label2, 0, 3);
			tableLayoutPanel1.Controls.Add(dgMountedBackups, 0, 4);
			tableLayoutPanel1.Controls.Add(label3, 0, 1);
			tableLayoutPanel1.Controls.Add(dgAvailableBackups, 0, 2);
			tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 5;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel1.Size = new System.Drawing.Size(780, 432);
			tableLayoutPanel1.TabIndex = 0;
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
			label2.Location = new System.Drawing.Point(3, 235);
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
			dgMountedBackups.Location = new System.Drawing.Point(3, 258);
			dgMountedBackups.Name = "dgMountedBackups";
			dgMountedBackups.RowHeadersWidth = 45;
			dgMountedBackups.Size = new System.Drawing.Size(774, 171);
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
			dgAvailableBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { BackupNameCol, BackupTypeCol, IsEncryptedCol, BackupDateCol, BackupPathCol, MountCol });
			dgAvailableBackups.Dock = System.Windows.Forms.DockStyle.Fill;
			dgAvailableBackups.Location = new System.Drawing.Point(3, 61);
			dgAvailableBackups.Name = "dgAvailableBackups";
			dgAvailableBackups.RowHeadersWidth = 45;
			dgAvailableBackups.Size = new System.Drawing.Size(774, 171);
			dgAvailableBackups.TabIndex = 10;
			// 
			// BackupNameCol
			// 
			BackupNameCol.HeaderText = "Backup Name";
			BackupNameCol.MinimumWidth = 6;
			BackupNameCol.Name = "BackupNameCol";
			BackupNameCol.Width = 110;
			// 
			// BackupTypeCol
			// 
			BackupTypeCol.HeaderText = "Backup Type";
			BackupTypeCol.MinimumWidth = 6;
			BackupTypeCol.Name = "BackupTypeCol";
			BackupTypeCol.Width = 110;
			// 
			// IsEncryptedCol
			// 
			IsEncryptedCol.HeaderText = "Encrypted";
			IsEncryptedCol.MinimumWidth = 6;
			IsEncryptedCol.Name = "IsEncryptedCol";
			IsEncryptedCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			IsEncryptedCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			IsEncryptedCol.Width = 110;
			// 
			// BackupDateCol
			// 
			BackupDateCol.HeaderText = "Backup Date";
			BackupDateCol.MinimumWidth = 6;
			BackupDateCol.Name = "BackupDateCol";
			BackupDateCol.Width = 110;
			// 
			// BackupPathCol
			// 
			BackupPathCol.HeaderText = "Backup Path";
			BackupPathCol.MinimumWidth = 6;
			BackupPathCol.Name = "BackupPathCol";
			BackupPathCol.Width = 110;
			// 
			// MountCol
			// 
			MountCol.HeaderText = "Mount";
			MountCol.MinimumWidth = 6;
			MountCol.Name = "MountCol";
			MountCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			MountCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			MountCol.Text = "Mount";
			MountCol.Width = 110;
			// 
			// MountBackupTabForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(800, 452);
			Controls.Add(activityTabPanel);
			Name = "MountBackupTabForm";
			Text = "MountBackupTabForm";
			activityTabPanel.ResumeLayout(false);
			activityTabPanel.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgMountedBackups).EndInit();
			((System.ComponentModel.ISupportInitialize)dgAvailableBackups).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Panel activityTabPanel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupTypeCol;
		private System.Windows.Forms.DataGridViewCheckBoxColumn IsEncryptedCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupDateCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn BackupPathCol;
		private System.Windows.Forms.DataGridViewButtonColumn MountCol;
	}
}