using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class RestoreVolumeSelectionForm
	{
		private IContainer components;
		private Label subtitleLabel;
		private Label diskGroupLabel;
		private ListView volumesListView;
		private ColumnHeader imageColumnHeader;
		private ColumnHeader labelColumnHeader;
		private ColumnHeader partitionTypeColumnHeader;
		private ColumnHeader fileSystemColumnHeader;
		private ColumnHeader sourceColumnHeader;
		private ColumnHeader originalSizeColumnHeader;
		private ColumnHeader usedSizeColumnHeader;
		private Label hintLabel;
		private Button restoreButton;
		private Button selectFullDiskButton;
		private Button cancelButton;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			subtitleLabel = new Label();
			diskGroupLabel = new Label();
			volumesListView = new ListView();
			imageColumnHeader = new ColumnHeader();
			labelColumnHeader = new ColumnHeader();
			partitionTypeColumnHeader = new ColumnHeader();
			fileSystemColumnHeader = new ColumnHeader();
			sourceColumnHeader = new ColumnHeader();
			originalSizeColumnHeader = new ColumnHeader();
			usedSizeColumnHeader = new ColumnHeader();
			hintLabel = new Label();
			restoreButton = new Button();
			selectFullDiskButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			subtitleLabel.Location = new Point(16, 16);
			subtitleLabel.Name = "subtitleLabel";
			subtitleLabel.Size = new Size(880, 44);
			subtitleLabel.TabIndex = 0;
			diskGroupLabel.Location = new Point(16, 62);
			diskGroupLabel.Name = "diskGroupLabel";
			diskGroupLabel.Size = new Size(880, 36);
			diskGroupLabel.TabIndex = 1;
			volumesListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			volumesListView.Columns.AddRange(new ColumnHeader[] { imageColumnHeader, labelColumnHeader, partitionTypeColumnHeader, fileSystemColumnHeader, sourceColumnHeader, originalSizeColumnHeader, usedSizeColumnHeader });
			volumesListView.FullRowSelect = true;
			volumesListView.GridLines = true;
			volumesListView.Location = new Point(16, 108);
			volumesListView.MultiSelect = false;
			volumesListView.Name = "volumesListView";
			volumesListView.Size = new Size(880, 310);
			volumesListView.TabIndex = 2;
			volumesListView.UseCompatibleStateImageBehavior = false;
			volumesListView.View = View.Details;
			volumesListView.DoubleClick += VolumesListView_DoubleClick;
			imageColumnHeader.Text = "Image";
			imageColumnHeader.Width = 70;
			labelColumnHeader.Text = "Label";
			labelColumnHeader.Width = 220;
			partitionTypeColumnHeader.Text = "Partition Type";
			partitionTypeColumnHeader.Width = 140;
			fileSystemColumnHeader.Text = "File System";
			fileSystemColumnHeader.Width = 110;
			sourceColumnHeader.Text = "Source";
			sourceColumnHeader.Width = 150;
			originalSizeColumnHeader.Text = "Original Size";
			originalSizeColumnHeader.Width = 90;
			usedSizeColumnHeader.Text = "Used Size";
			usedSizeColumnHeader.Width = 90;
			hintLabel.Location = new Point(16, 426);
			hintLabel.Name = "hintLabel";
			hintLabel.Size = new Size(520, 24);
			hintLabel.TabIndex = 3;
			hintLabel.Text = "Select a volume, then click Restore.";
			restoreButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			restoreButton.Location = new Point(602, 452);
			restoreButton.Name = "restoreButton";
			restoreButton.Size = new Size(96, 32);
			restoreButton.TabIndex = 4;
			restoreButton.Text = "Restore";
			restoreButton.UseVisualStyleBackColor = true;
			restoreButton.Click += RestoreButton_Click;
			selectFullDiskButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			selectFullDiskButton.Location = new Point(704, 452);
			selectFullDiskButton.Name = "selectFullDiskButton";
			selectFullDiskButton.Size = new Size(130, 32);
			selectFullDiskButton.TabIndex = 5;
			selectFullDiskButton.Text = "Select Full Disk";
			selectFullDiskButton.UseVisualStyleBackColor = true;
			selectFullDiskButton.Click += SelectFullDiskButton_Click;
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(840, 452);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(96, 32);
			cancelButton.TabIndex = 6;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(920, 500);
			Controls.Add(cancelButton);
			Controls.Add(selectFullDiskButton);
			Controls.Add(restoreButton);
			Controls.Add(hintLabel);
			Controls.Add(volumesListView);
			Controls.Add(diskGroupLabel);
			Controls.Add(subtitleLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new Size(920, 500);
			Name = "RestoreVolumeSelectionForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Select Restore Volume";
			ResumeLayout(false);
		}
	}
}
