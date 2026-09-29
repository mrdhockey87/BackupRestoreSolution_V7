namespace SecureServerBackup.WinForms
{
	partial class TempPathSelectionDialog
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
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblTempPathCaption = new System.Windows.Forms.Label();
			this.txtTempPath = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtSpaceInfo = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			// lblTitle
			//
			this.lblTitle.AutoSize = false;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lblTitle.Location = new System.Drawing.Point(20, 20);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(510, 26);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "Temporary Path for Mount Operation";
			//
			// lblDescription
			//
			this.lblDescription.ForeColor = System.Drawing.Color.Gray;
			this.lblDescription.Location = new System.Drawing.Point(20, 54);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(510, 62);
			this.lblDescription.TabIndex = 1;
			this.lblDescription.Text = "The Secure Server Backup requires a temporary directory to decompress and proce" +
	"ss backup image data during mount operations.\r\n\r\nChoose a location with adequ" +
	"ate free space (several GB may be needed for large backups).";
			//
			// lblTempPathCaption
			//
			this.lblTempPathCaption.AutoSize = true;
			this.lblTempPathCaption.Location = new System.Drawing.Point(20, 124);
			this.lblTempPathCaption.Name = "lblTempPathCaption";
			this.lblTempPathCaption.Size = new System.Drawing.Size(94, 15);
			this.lblTempPathCaption.TabIndex = 2;
			this.lblTempPathCaption.Text = "Temporary Path:";
			//
			// txtTempPath
			//
			this.txtTempPath.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtTempPath.Location = new System.Drawing.Point(20, 148);
			this.txtTempPath.Name = "txtTempPath";
			this.txtTempPath.ReadOnly = true;
			this.txtTempPath.Size = new System.Drawing.Size(410, 23);
			this.txtTempPath.TabIndex = 3;
			//
			// btnBrowse
			//
			this.btnBrowse.Location = new System.Drawing.Point(440, 147);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(90, 28);
			this.btnBrowse.TabIndex = 4;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
			//
			// txtSpaceInfo
			//
			this.txtSpaceInfo.ForeColor = System.Drawing.Color.Gray;
			this.txtSpaceInfo.Location = new System.Drawing.Point(20, 184);
			this.txtSpaceInfo.Name = "txtSpaceInfo";
			this.txtSpaceInfo.Size = new System.Drawing.Size(510, 40);
			this.txtSpaceInfo.TabIndex = 5;
			this.txtSpaceInfo.Text = "Checking available space...";
			//
			// btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(360, 260);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 28);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.OK_Click);
			//
			// btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(450, 260);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 28);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
			//
			// TempPathSelectionDialog
			//
			this.AcceptButton = this.btnOK;
			this.CancelButton = this.btnCancel;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(550, 310);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtSpaceInfo);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtTempPath);
			this.Controls.Add(this.lblTempPathCaption);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TempPathSelectionDialog";
			this.Padding = new System.Windows.Forms.Padding(20);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Temporary Path for Mount";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblTempPathCaption;
		private System.Windows.Forms.TextBox txtTempPath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label txtSpaceInfo;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}