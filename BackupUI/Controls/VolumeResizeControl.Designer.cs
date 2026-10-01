namespace SecureServerBackup.Controls
{
	partial class VolumeResizeControl
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

		#region Component Designer generated code

		private void InitializeComponent()
		{
			this.tlpRoot = new System.Windows.Forms.TableLayoutPanel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlSourceSection = new System.Windows.Forms.Panel();
			this.pnlSourceBar = new System.Windows.Forms.Panel();
			this.lblSourceSize = new System.Windows.Forms.Label();
			this.lblSourceTitle = new System.Windows.Forms.Label();
			this.pnlArrows = new System.Windows.Forms.Panel();
			this.pnlTargetSection = new System.Windows.Forms.Panel();
			this.pnlTargetBar = new System.Windows.Forms.Panel();
			this.flpTargetLabels = new System.Windows.Forms.FlowLayoutPanel();
			this.lblTargetFree = new System.Windows.Forms.Label();
			this.lblTargetUsed = new System.Windows.Forms.Label();
			this.lblTargetTitle = new System.Windows.Forms.Label();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnAutoFit = new System.Windows.Forms.Button();
			this.tlpRoot.SuspendLayout();
			this.pnlSourceSection.SuspendLayout();
			this.pnlTargetSection.SuspendLayout();
			this.flpTargetLabels.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			//
			// tlpRoot
			//
			this.tlpRoot.ColumnCount = 1;
			this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpRoot.Controls.Add(this.lblTitle, 0, 0);
			this.tlpRoot.Controls.Add(this.pnlSourceSection, 0, 1);
			this.tlpRoot.Controls.Add(this.pnlArrows, 0, 2);
			this.tlpRoot.Controls.Add(this.pnlTargetSection, 0, 3);
			this.tlpRoot.Controls.Add(this.flpButtons, 0, 4);
			this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpRoot.Location = new System.Drawing.Point(10, 10);
			this.tlpRoot.Name = "tlpRoot";
			this.tlpRoot.RowCount = 5;
			this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpRoot.Size = new System.Drawing.Size(780, 280);
			this.tlpRoot.TabIndex = 0;
			//
			// lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblTitle.Location = new System.Drawing.Point(3, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(774, 30);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "Volume Resize Configuration";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// pnlSourceSection
			//
			this.pnlSourceSection.Controls.Add(this.pnlSourceBar);
			this.pnlSourceSection.Controls.Add(this.lblSourceSize);
			this.pnlSourceSection.Controls.Add(this.lblSourceTitle);
			this.pnlSourceSection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSourceSection.Location = new System.Drawing.Point(3, 33);
			this.pnlSourceSection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.pnlSourceSection.Name = "pnlSourceSection";
			this.pnlSourceSection.Size = new System.Drawing.Size(774, 87);
			this.pnlSourceSection.TabIndex = 1;
			//
			// pnlSourceBar
			//
			this.pnlSourceBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSourceBar.Location = new System.Drawing.Point(0, 20);
			this.pnlSourceBar.Name = "pnlSourceBar";
			this.pnlSourceBar.Size = new System.Drawing.Size(774, 47);
			this.pnlSourceBar.TabIndex = 1;
			this.pnlSourceBar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSourceBar_Paint);
			//
			// lblSourceSize
			//
			this.lblSourceSize.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblSourceSize.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.lblSourceSize.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
			this.lblSourceSize.Location = new System.Drawing.Point(0, 67);
			this.lblSourceSize.Name = "lblSourceSize";
			this.lblSourceSize.Size = new System.Drawing.Size(774, 20);
			this.lblSourceSize.TabIndex = 2;
			this.lblSourceSize.Text = "Total: 0 GB";
			this.lblSourceSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// lblSourceTitle
			//
			this.lblSourceTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSourceTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
			this.lblSourceTitle.Location = new System.Drawing.Point(0, 0);
			this.lblSourceTitle.Name = "lblSourceTitle";
			this.lblSourceTitle.Size = new System.Drawing.Size(774, 20);
			this.lblSourceTitle.TabIndex = 0;
			this.lblSourceTitle.Text = "Original Backup Volumes";
			this.lblSourceTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// pnlArrows
			//
			this.pnlArrows.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlArrows.Location = new System.Drawing.Point(3, 123);
			this.pnlArrows.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.pnlArrows.Name = "pnlArrows";
			this.pnlArrows.Size = new System.Drawing.Size(774, 47);
			this.pnlArrows.TabIndex = 2;
			this.pnlArrows.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlArrows_Paint);
			//
			// pnlTargetSection
			//
			this.pnlTargetSection.Controls.Add(this.pnlTargetBar);
			this.pnlTargetSection.Controls.Add(this.flpTargetLabels);
			this.pnlTargetSection.Controls.Add(this.lblTargetTitle);
			this.pnlTargetSection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTargetSection.Location = new System.Drawing.Point(3, 173);
			this.pnlTargetSection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.pnlTargetSection.Name = "pnlTargetSection";
			this.pnlTargetSection.Size = new System.Drawing.Size(774, 87);
			this.pnlTargetSection.TabIndex = 3;
			//
			// pnlTargetBar
			//
			this.pnlTargetBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTargetBar.Location = new System.Drawing.Point(0, 20);
			this.pnlTargetBar.Name = "pnlTargetBar";
			this.pnlTargetBar.Size = new System.Drawing.Size(774, 47);
			this.pnlTargetBar.TabIndex = 1;
			this.pnlTargetBar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTargetBar_Paint);
			this.pnlTargetBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTargetBar_MouseDown);
			this.pnlTargetBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlTargetBar_MouseMove);
			this.pnlTargetBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlTargetBar_MouseUp);
			//
			// flpTargetLabels
			//
			this.flpTargetLabels.Controls.Add(this.lblTargetFree);
			this.flpTargetLabels.Controls.Add(this.lblTargetUsed);
			this.flpTargetLabels.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flpTargetLabels.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpTargetLabels.Location = new System.Drawing.Point(0, 67);
			this.flpTargetLabels.Margin = new System.Windows.Forms.Padding(0);
			this.flpTargetLabels.Name = "flpTargetLabels";
			this.flpTargetLabels.Size = new System.Drawing.Size(774, 20);
			this.flpTargetLabels.TabIndex = 2;
			//
			// lblTargetFree
			//
			this.lblTargetFree.AutoSize = true;
			this.lblTargetFree.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.lblTargetFree.ForeColor = System.Drawing.Color.FromArgb(76, 175, 80);
			this.lblTargetFree.Location = new System.Drawing.Point(715, 3);
			this.lblTargetFree.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.lblTargetFree.Name = "lblTargetFree";
			this.lblTargetFree.Size = new System.Drawing.Size(56, 13);
			this.lblTargetFree.TabIndex = 1;
			this.lblTargetFree.Text = "Free: 0 GB";
			//
			// lblTargetUsed
			//
			this.lblTargetUsed.AutoSize = true;
			this.lblTargetUsed.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.lblTargetUsed.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
			this.lblTargetUsed.Location = new System.Drawing.Point(640, 3);
			this.lblTargetUsed.Margin = new System.Windows.Forms.Padding(3, 3, 12, 0);
			this.lblTargetUsed.Name = "lblTargetUsed";
			this.lblTargetUsed.Size = new System.Drawing.Size(60, 13);
			this.lblTargetUsed.TabIndex = 0;
			this.lblTargetUsed.Text = "Used: 0 GB";
			//
			// lblTargetTitle
			//
			this.lblTargetTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTargetTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
			this.lblTargetTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTargetTitle.Name = "lblTargetTitle";
			this.lblTargetTitle.Size = new System.Drawing.Size(774, 20);
			this.lblTargetTitle.TabIndex = 0;
			this.lblTargetTitle.Text = "Target Disk Configuration";
			this.lblTargetTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// flpButtons
			//
			this.flpButtons.Controls.Add(this.btnReset);
			this.flpButtons.Controls.Add(this.btnAutoFit);
			this.flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 263);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.flpButtons.Size = new System.Drawing.Size(774, 14);
			this.flpButtons.TabIndex = 4;
			//
			// btnReset
			//
			this.btnReset.Location = new System.Drawing.Point(671, 13);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(100, 30);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			//
			// btnAutoFit
			//
			this.btnAutoFit.Location = new System.Drawing.Point(565, 13);
			this.btnAutoFit.Name = "btnAutoFit";
			this.btnAutoFit.Size = new System.Drawing.Size(100, 30);
			this.btnAutoFit.TabIndex = 0;
			this.btnAutoFit.Text = "Auto Fit";
			this.btnAutoFit.UseVisualStyleBackColor = true;
			this.btnAutoFit.Click += new System.EventHandler(this.btnAutoFit_Click);
			//
			// VolumeResizeControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpRoot);
			this.MinimumSize = new System.Drawing.Size(0, 300);
			this.Name = "VolumeResizeControl";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.Size = new System.Drawing.Size(800, 300);
			this.tlpRoot.ResumeLayout(false);
			this.tlpRoot.PerformLayout();
			this.pnlSourceSection.ResumeLayout(false);
			this.pnlTargetSection.ResumeLayout(false);
			this.flpTargetLabels.ResumeLayout(false);
			this.flpTargetLabels.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpRoot;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlSourceSection;
		private System.Windows.Forms.Panel pnlSourceBar;
		private System.Windows.Forms.Label lblSourceSize;
		private System.Windows.Forms.Label lblSourceTitle;
		private System.Windows.Forms.Panel pnlArrows;
		private System.Windows.Forms.Panel pnlTargetSection;
		private System.Windows.Forms.Panel pnlTargetBar;
		private System.Windows.Forms.FlowLayoutPanel flpTargetLabels;
		private System.Windows.Forms.Label lblTargetFree;
		private System.Windows.Forms.Label lblTargetUsed;
		private System.Windows.Forms.Label lblTargetTitle;
		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnAutoFit;
	}
}