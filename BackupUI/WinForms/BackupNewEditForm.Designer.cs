namespace SecureServerBackup.WinForms
{
	partial class BackupNewEditForm
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
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label1 = new System.Windows.Forms.Label();
			treeView1 = new System.Windows.Forms.TreeView();
			flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			button3 = new System.Windows.Forms.Button();
			checkBox1 = new System.Windows.Forms.CheckBox();
			tableLayoutPanel1.SuspendLayout();
			flowLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			groupBox1.SuspendLayout();
			flowLayoutPanel2.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
			tableLayoutPanel1.Location = new System.Drawing.Point(5, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.11111F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.88889F));
			tableLayoutPanel1.Size = new System.Drawing.Size(940, 484);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.Controls.Add(tableLayoutPanel2);
			flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			tableLayoutPanel1.SetRowSpan(flowLayoutPanel1, 2);
			flowLayoutPanel1.Size = new System.Drawing.Size(459, 478);
			flowLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			tableLayoutPanel2.ColumnCount = 1;
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			tableLayoutPanel2.Controls.Add(groupBox1, 0, 0);
			tableLayoutPanel2.Controls.Add(treeView1, 0, 1);
			tableLayoutPanel2.Controls.Add(flowLayoutPanel2, 0, 2);
			tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 3;
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.62175F));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.37825F));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
			tableLayoutPanel2.Size = new System.Drawing.Size(451, 475);
			tableLayoutPanel2.TabIndex = 0;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(label1);
			groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			groupBox1.Location = new System.Drawing.Point(3, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(445, 78);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "What to Backup";
			// 
			// label1
			// 
			label1.Dock = System.Windows.Forms.DockStyle.Fill;
			label1.Location = new System.Drawing.Point(3, 21);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(439, 54);
			label1.TabIndex = 0;
			label1.Text = "Select the DIsk, Volumes or File & Folders by checking the box. File & Folder are shown when Volumes are unselected. Boot Volumes will automatically include system state.";
			// 
			// treeView1
			// 
			treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			treeView1.Location = new System.Drawing.Point(3, 87);
			treeView1.Name = "treeView1";
			treeView1.Size = new System.Drawing.Size(445, 341);
			treeView1.TabIndex = 1;
			// 
			// flowLayoutPanel2
			// 
			flowLayoutPanel2.Controls.Add(button1);
			flowLayoutPanel2.Controls.Add(button2);
			flowLayoutPanel2.Controls.Add(button3);
			flowLayoutPanel2.Controls.Add(checkBox1);
			flowLayoutPanel2.Location = new System.Drawing.Point(3, 434);
			flowLayoutPanel2.Name = "flowLayoutPanel2";
			flowLayoutPanel2.Size = new System.Drawing.Size(443, 38);
			flowLayoutPanel2.TabIndex = 2;
			// 
			// button1
			// 
			button1.Location = new System.Drawing.Point(5, 5);
			button1.Margin = new System.Windows.Forms.Padding(5);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(76, 24);
			button1.TabIndex = 3;
			button1.Text = "Refresh";
			button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			button2.Location = new System.Drawing.Point(91, 5);
			button2.Margin = new System.Windows.Forms.Padding(5);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(83, 24);
			button2.TabIndex = 4;
			button2.Text = "Expand All";
			button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			button3.Location = new System.Drawing.Point(184, 5);
			button3.Margin = new System.Windows.Forms.Padding(5);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(85, 26);
			button3.TabIndex = 5;
			button3.Text = "Collapse All";
			button3.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new System.Drawing.Point(277, 8);
			checkBox1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new System.Drawing.Size(162, 21);
			checkBox1.TabIndex = 6;
			checkBox1.Text = "Show Hidden Partitions";
			checkBox1.UseVisualStyleBackColor = true;
			// 
			// BackupNewEditForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(944, 484);
			Controls.Add(tableLayoutPanel1);
			Name = "BackupNewEditForm";
			Text = "BackupNewEditForm";
			tableLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel2.ResumeLayout(false);
			groupBox1.ResumeLayout(false);
			flowLayoutPanel2.ResumeLayout(false);
			flowLayoutPanel2.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}