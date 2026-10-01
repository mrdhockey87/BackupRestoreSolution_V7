namespace SecureServerBackup.WinForms
{
	partial class VolumeSelectionForm
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
			components = new System.ComponentModel.Container();
			tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			lblInstructions = new System.Windows.Forms.Label();
			lstVolumes = new System.Windows.Forms.ListBox();
			chkShowHiddenPartitions = new System.Windows.Forms.CheckBox();
			flowButtons = new System.Windows.Forms.FlowLayoutPanel();
			btnSelect = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			toolTip = new System.Windows.Forms.ToolTip(components);
			tableLayoutPanel.SuspendLayout();
			flowButtons.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.ColumnCount = 1;
			tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel.Controls.Add(lblInstructions, 0, 0);
			tableLayoutPanel.Controls.Add(lstVolumes, 0, 1);
			tableLayoutPanel.Controls.Add(chkShowHiddenPartitions, 0, 2);
			tableLayoutPanel.Controls.Add(flowButtons, 0, 3);
			tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.Padding = new System.Windows.Forms.Padding(15);
			tableLayoutPanel.RowCount = 4;
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel.Size = new System.Drawing.Size(700, 420);
			tableLayoutPanel.TabIndex = 0;
			// 
			// lblInstructions
			// 
			lblInstructions.Anchor = System.Windows.Forms.AnchorStyles.Left;
			lblInstructions.AutoSize = true;
			lblInstructions.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
			lblInstructions.Location = new System.Drawing.Point(15, 15);
			lblInstructions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
			lblInstructions.Name = "lblInstructions";
			lblInstructions.Size = new System.Drawing.Size(318, 15);
			lblInstructions.TabIndex = 0;
			lblInstructions.Text = "Select the target volume to be formatted and restored.";
			// 
			// lstVolumes
			// 
			lstVolumes.Dock = System.Windows.Forms.DockStyle.Fill;
			lstVolumes.DisplayMember = "DisplayName";
			lstVolumes.FormattingEnabled = true;
			lstVolumes.IntegralHeight = false;
			lstVolumes.ItemHeight = 15;
			lstVolumes.Location = new System.Drawing.Point(15, 40);
			lstVolumes.Margin = new System.Windows.Forms.Padding(0);
			lstVolumes.Name = "lstVolumes";
			lstVolumes.Size = new System.Drawing.Size(670, 280);
			lstVolumes.TabIndex = 1;
			lstVolumes.SelectedIndexChanged += VolumeList_SelectedIndexChanged;
			lstVolumes.DoubleClick += VolumeList_DoubleClick;
			// 
			// chkShowHiddenPartitions
			// 
			chkShowHiddenPartitions.Anchor = System.Windows.Forms.AnchorStyles.Left;
			chkShowHiddenPartitions.AutoSize = true;
			chkShowHiddenPartitions.Location = new System.Drawing.Point(15, 328);
			chkShowHiddenPartitions.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			chkShowHiddenPartitions.Name = "chkShowHiddenPartitions";
			chkShowHiddenPartitions.Size = new System.Drawing.Size(150, 19);
			chkShowHiddenPartitions.TabIndex = 2;
			chkShowHiddenPartitions.Text = "Show Hidden Partitions";
			toolTip.SetToolTip(chkShowHiddenPartitions, "Include EFI, Recovery, and System Reserved partitions in the volume list");
			chkShowHiddenPartitions.UseVisualStyleBackColor = true;
			chkShowHiddenPartitions.Click += ShowHiddenPartitions_Click;
			// 
			// flowButtons
			// 
			flowButtons.Anchor = System.Windows.Forms.AnchorStyles.Right;
			flowButtons.AutoSize = true;
			flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowButtons.Controls.Add(btnSelect);
			flowButtons.Controls.Add(btnCancel);
			flowButtons.Location = new System.Drawing.Point(575, 357);
			flowButtons.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			flowButtons.Name = "flowButtons";
			flowButtons.Size = new System.Drawing.Size(210, 30);
			flowButtons.TabIndex = 3;
			flowButtons.WrapContents = false;
			// 
			// btnSelect
			// 
			btnSelect.Enabled = false;
			btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
			btnSelect.Location = new System.Drawing.Point(0, 0);
			btnSelect.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
			btnSelect.Name = "btnSelect";
			btnSelect.Size = new System.Drawing.Size(100, 30);
			btnSelect.TabIndex = 0;
			btnSelect.Text = "Select";
			btnSelect.UseVisualStyleBackColor = true;
			btnSelect.Click += Select_Click;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.Location = new System.Drawing.Point(110, 0);
			btnCancel.Margin = new System.Windows.Forms.Padding(0);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(100, 30);
			btnCancel.TabIndex = 1;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// VolumeSelectionWindow
			// 
			AcceptButton = btnSelect;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new System.Drawing.Size(700, 420);
			Controls.Add(tableLayoutPanel);
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(500, 300);
			Name = "VolumeSelectionWindow";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Select Target Volume";
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			flowButtons.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label lblInstructions;
		private System.Windows.Forms.ListBox lstVolumes;
		private System.Windows.Forms.CheckBox chkShowHiddenPartitions;
		private System.Windows.Forms.FlowLayoutPanel flowButtons;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ToolTip toolTip;
	}
}