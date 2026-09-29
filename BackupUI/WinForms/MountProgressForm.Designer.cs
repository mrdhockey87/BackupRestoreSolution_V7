namespace SecureServerBackup.WinForms
{
	partial class MountProgressForm
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
			outerPanel = new System.Windows.Forms.Panel();
			layout = new System.Windows.Forms.TableLayoutPanel();
			lblTitle = new System.Windows.Forms.Label();
			txtBackupName = new System.Windows.Forms.Label();
			progressBar = new System.Windows.Forms.ProgressBar();
			txtStatus = new System.Windows.Forms.Label();
			txtCurrentFile = new System.Windows.Forms.Label();
			outerPanel.SuspendLayout();
			layout.SuspendLayout();
			SuspendLayout();
			// 
			// outerPanel
			// 
			outerPanel.AutoSize = true;
			outerPanel.BackColor = System.Drawing.Color.White;
			outerPanel.Controls.Add(layout);
			outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			outerPanel.Location = new System.Drawing.Point(1, 1);
			outerPanel.Name = "outerPanel";
			outerPanel.Padding = new System.Windows.Forms.Padding(20, 23, 20, 23);
			outerPanel.Size = new System.Drawing.Size(432, 272);
			outerPanel.TabIndex = 0;
			// 
			// layout
			// 
			layout.AutoSize = true;
			layout.ColumnCount = 1;
			layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			layout.Controls.Add(lblTitle, 0, 0);
			layout.Controls.Add(txtBackupName, 0, 1);
			layout.Controls.Add(progressBar, 0, 2);
			layout.Controls.Add(txtStatus, 0, 3);
			layout.Controls.Add(txtCurrentFile, 0, 4);
			layout.Dock = System.Windows.Forms.DockStyle.Top;
			layout.Location = new System.Drawing.Point(20, 23);
			layout.Name = "layout";
			layout.RowCount = 5;
			layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			layout.Size = new System.Drawing.Size(392, 226);
			layout.TabIndex = 0;
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			lblTitle.Location = new System.Drawing.Point(0, 0);
			lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 17);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(239, 32);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "Mounting Backup...";
			// 
			// txtBackupName
			// 
			txtBackupName.AutoSize = true;
			txtBackupName.Font = new System.Drawing.Font("Segoe UI", 14F);
			txtBackupName.Location = new System.Drawing.Point(0, 49);
			txtBackupName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 11);
			txtBackupName.Name = "txtBackupName";
			txtBackupName.Size = new System.Drawing.Size(180, 30);
			txtBackupName.TabIndex = 1;
			txtBackupName.Text = "Backup: Loading...";
			// 
			// progressBar
			// 
			progressBar.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			progressBar.Location = new System.Drawing.Point(0, 90);
			progressBar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 17);
			progressBar.MarqueeAnimationSpeed = 30;
			progressBar.Name = "progressBar";
			progressBar.Size = new System.Drawing.Size(392, 28);
			progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			progressBar.TabIndex = 2;
			// 
			// txtStatus
			// 
			txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			txtStatus.Font = new System.Drawing.Font("Segoe UI", 12F);
			txtStatus.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
			txtStatus.Location = new System.Drawing.Point(0, 135);
			txtStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
			txtStatus.Name = "txtStatus";
			txtStatus.Size = new System.Drawing.Size(392, 45);
			txtStatus.TabIndex = 3;
			txtStatus.Text = "Opening SSB archive...";
			// 
			// txtCurrentFile
			// 
			txtCurrentFile.AutoEllipsis = true;
			txtCurrentFile.Font = new System.Drawing.Font("Segoe UI", 10F);
			txtCurrentFile.ForeColor = System.Drawing.Color.Gray;
			txtCurrentFile.Location = new System.Drawing.Point(0, 186);
			txtCurrentFile.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
			txtCurrentFile.Name = "txtCurrentFile";
			txtCurrentFile.Size = new System.Drawing.Size(392, 34);
			txtCurrentFile.TabIndex = 4;
			// 
			// MountProgressForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			BackColor = System.Drawing.Color.White;
			ClientSize = new System.Drawing.Size(434, 274);
			Controls.Add(outerPanel);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximumSize = new System.Drawing.Size(450, 313);
			MinimumSize = new System.Drawing.Size(450, 313);
			Name = "MountProgressForm";
			Padding = new System.Windows.Forms.Padding(1);
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Mounting Backup";
			outerPanel.ResumeLayout(false);
			outerPanel.PerformLayout();
			layout.ResumeLayout(false);
			layout.PerformLayout();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel outerPanel;
		private System.Windows.Forms.TableLayoutPanel layout;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label txtBackupName;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label txtStatus;
		private System.Windows.Forms.Label txtCurrentFile;
	}
}