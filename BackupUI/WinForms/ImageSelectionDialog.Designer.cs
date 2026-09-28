using System;
using System.Windows.Forms;
using System.Drawing;

using SecureServerBackup.Models;

namespace SecureServerBackup.WinForms
{
	partial class ImageSelectionDialog
	{
		private System.ComponentModel.IContainer components = null;

		private Label lblTitle;
		private Label lblSubtitle;
		private Panel pnlGridContainer;
		private DataGridView dgImages;
		private Button btnAction;
		private Button btnCancel;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblTitle = new Label();
			this.lblSubtitle = new Label();
			this.pnlGridContainer = new Panel();
			this.dgImages = new DataGridView();
			this.btnAction = new Button();
			this.btnCancel = new Button();

			((System.ComponentModel.ISupportInitialize)(this.dgImages)).BeginInit();
			this.pnlGridContainer.SuspendLayout();
			this.SuspendLayout();

			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblTitle.Location = new System.Drawing.Point(20, 20);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(300, 25);
			this.lblTitle.Text = "Multiple Restore Points Available";

			// 
			// lblSubtitle
			// 
			this.lblSubtitle.Location = new System.Drawing.Point(20, 48);
			this.lblSubtitle.Size = new System.Drawing.Size(560, 40);
			this.lblSubtitle.Name = "lblSubtitle";
			this.lblSubtitle.Text = "This backup contains multiple restore points. Select which point to mount:";

			// 
			// pnlGridContainer
			// 
			this.pnlGridContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGridContainer.Location = new System.Drawing.Point(20, 95);
			this.pnlGridContainer.Size = new System.Drawing.Size(560, 220);
			this.pnlGridContainer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.pnlGridContainer.Controls.Add(this.dgImages);

			// 
			// dgImages
			// 
			this.dgImages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgImages.AllowUserToAddRows = false;
			this.dgImages.AllowUserToDeleteRows = false;
			this.dgImages.AllowUserToResizeRows = false;
			this.dgImages.AllowUserToOrderColumns = false;
			this.dgImages.AutoGenerateColumns = false;
			this.dgImages.ReadOnly = true;
			this.dgImages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgImages.MultiSelect = false;
			this.dgImages.RowHeadersVisible = false;
			this.dgImages.BackgroundColor = System.Drawing.Color.White;
			this.dgImages.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgImages.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.dgImages.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
			this.dgImages.DoubleClick += new System.EventHandler(this.dgImages_DoubleClick);

			// Columns
			var colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn
			{
				Name = "ImageIndex",
				HeaderText = "Image #",
				DataPropertyName = "ImageIndex",
				Width = 80,
				DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
			};

			var colDate = new DataGridViewTextBoxColumn
			{
				Name = "ImageDate",
				HeaderText = "Date/Time",
				DataPropertyName = "ImageDate",
				Width = 180,
				DefaultCellStyle = new DataGridViewCellStyle { Format = "MM/dd/yyyy HH:mm:ss" }
			};

			var colType = new DataGridViewTextBoxColumn
			{
				Name = "ImageType",
				HeaderText = "Type",
				DataPropertyName = "ImageType",
				Width = 120
			};

			var colDesc = new DataGridViewTextBoxColumn
			{
				Name = "Description",
				HeaderText = "Description",
				DataPropertyName = "Description",
				AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
			};

			this.dgImages.Columns.AddRange(new DataGridViewColumn[] { colIndex, colDate, colType, colDesc });

			// 
			// btnAction
			// 
			this.btnAction.Text = "Mount Selected";
			this.btnAction.Size = new Size(130, 32);
			this.btnAction.Location = new Point(340, 330);
			this.btnAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnAction.BackColor = Color.FromArgb(40, 167, 69); // success green
			this.btnAction.ForeColor = Color.White;
			this.btnAction.FlatStyle = FlatStyle.Flat;
			this.btnAction.FlatAppearance.BorderSize = 0;
			this.btnAction.Click += new EventHandler(this.btnMount_Click);

			// 
			// btnCancel
			// 
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Size = new Size(100, 32);
			this.btnCancel.Location = new Point(480, 330);
			this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnCancel.FlatStyle = FlatStyle.Flat;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

			// 
			// ImageSelectionDialog
			// 
			this.ClientSize = new Size(600, 400);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.lblSubtitle);
			this.Controls.Add(this.pnlGridContainer);
			this.Controls.Add(this.btnAction);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Select Restore Point";
			this.Padding = new Padding(20);

			((System.ComponentModel.ISupportInitialize)(this.dgImages)).EndInit();
			this.pnlGridContainer.ResumeLayout(false);
			this.ResumeLayout(false);
		}
	}
}