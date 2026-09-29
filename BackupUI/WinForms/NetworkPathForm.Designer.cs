namespace SecureServerBackup.WinForms
{
	partial class NetworkPathForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.lblHeader = new System.Windows.Forms.Label();
			this.txtNetworkPath = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lblExamples = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			// lblHeader
			//
			this.lblHeader.AutoSize = true;
			this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lblHeader.Location = new System.Drawing.Point(20, 20);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(216, 15);
			this.lblHeader.TabIndex = 0;
			this.lblHeader.Text = "Enter Network Path (UNC Format):";
			//
			// txtNetworkPath
			//
			this.txtNetworkPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right))));
			this.txtNetworkPath.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txtNetworkPath.Location = new System.Drawing.Point(20, 48);
			this.txtNetworkPath.Name = "txtNetworkPath";
			this.txtNetworkPath.Size = new System.Drawing.Size(362, 26);
			this.txtNetworkPath.TabIndex = 1;
			this.txtNetworkPath.Text = "\\\\";
			//
			// btnBrowse
			//
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(390, 47);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(90, 30);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
			//
			// lblExamples
			//
			this.lblExamples.ForeColor = System.Drawing.Color.Gray;
			this.lblExamples.Location = new System.Drawing.Point(20, 88);
			this.lblExamples.Name = "lblExamples";
			this.lblExamples.Size = new System.Drawing.Size(460, 80);
			this.lblExamples.TabIndex = 3;
			this.lblExamples.Text = "Examples:\r\n  \\\\server\\share\r\n  \\\\192.168.1.100\\backups\r\n  \\\\NAS\\Documents";
			//
			// btnOK
			//
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(300, 176);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 30);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.OK_Click);
			//
			// btnCancel
			//
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(390, 176);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 30);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
			//
			// NetworkPathForm
			//
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(500, 226);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.txtNetworkPath);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.lblExamples);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 245);
			this.Name = "NetworkPathForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Network Path";
			this.Load += new System.EventHandler(this.NetworkPathForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtNetworkPath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label lblExamples;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}
