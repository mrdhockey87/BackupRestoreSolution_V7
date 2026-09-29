namespace SecureServerBackup.WinForms
{
	partial class DiskSelectionForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			this.lblHeader = new System.Windows.Forms.Label();
			this.pnlInfo = new System.Windows.Forms.Panel();
			this.lblWarning = new System.Windows.Forms.Label();
			this.lblInfoText = new System.Windows.Forms.Label();
			this.lstDisks = new System.Windows.Forms.ListBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlInfo.SuspendLayout();
			this.SuspendLayout();
			//
			// lblHeader
			//
			this.lblHeader.AutoSize = true;
			this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lblHeader.Location = new System.Drawing.Point(20, 20);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(367, 21);
			this.lblHeader.TabIndex = 0;
			this.lblHeader.Text = "Select Target Disk for Clone Operation";
			//
			// pnlInfo
			//
			this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
			this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlInfo.Controls.Add(this.lblWarning);
			this.pnlInfo.Controls.Add(this.lblInfoText);
			this.pnlInfo.Location = new System.Drawing.Point(20, 55);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(540, 95);
			this.pnlInfo.TabIndex = 1;
			//
			// lblWarning
			//
			this.lblWarning.AutoSize = true;
			this.lblWarning.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lblWarning.ForeColor = System.Drawing.Color.FromArgb(211, 47, 47);
			this.lblWarning.Location = new System.Drawing.Point(10, 68);
			this.lblWarning.Name = "lblWarning";
			this.lblWarning.Size = new System.Drawing.Size(297, 15);
			this.lblWarning.TabIndex = 1;
			this.lblWarning.Text = "- All data on the target disk will be REPLACED";
			//
			// lblInfoText
			//
			this.lblInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblInfoText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblInfoText.Location = new System.Drawing.Point(0, 0);
			this.lblInfoText.Name = "lblInfoText";
			this.lblInfoText.Padding = new System.Windows.Forms.Padding(10);
			this.lblInfoText.Size = new System.Drawing.Size(538, 93);
			this.lblInfoText.TabIndex = 0;
			this.lblInfoText.Text = "Select the destination disk for the clone operation:\r\n- Only available physical" +
	" disks are shown\r\n- Source disk is excluded from the list";
			//
			// lstDisks
			//
			this.lstDisks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstDisks.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstDisks.FormattingEnabled = true;
			this.lstDisks.IntegralHeight = false;
			this.lstDisks.ItemHeight = 48;
			this.lstDisks.Location = new System.Drawing.Point(20, 160);
			this.lstDisks.Name = "lstDisks";
			this.lstDisks.Size = new System.Drawing.Size(540, 245);
			this.lstDisks.TabIndex = 2;
			this.lstDisks.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstDisks_DrawItem);
			this.lstDisks.SelectedIndexChanged += new System.EventHandler(this.lstDisks_SelectedIndexChanged);
			//
			// btnSelect
			//
			this.btnSelect.Enabled = false;
			this.btnSelect.Location = new System.Drawing.Point(340, 420);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(100, 35);
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select Disk";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			//
			// btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(450, 420);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 35);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			//
			// DiskSelectionForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(580, 475);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.lstDisks);
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.lblHeader);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DiskSelectionForm";
			this.Padding = new System.Windows.Forms.Padding(20);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Target Disk";
			this.pnlInfo.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.Label lblInfoText;
		private System.Windows.Forms.Label lblWarning;
		private System.Windows.Forms.ListBox lstDisks;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCancel;
	}
}