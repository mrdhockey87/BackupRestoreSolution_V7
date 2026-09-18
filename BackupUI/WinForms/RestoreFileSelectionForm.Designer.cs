using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class RestoreFileSelectionForm
	{
		private IContainer components;
		private Label summaryLabel;
		private Label destinationLabel;
		private TextBox destinationTextBox;
		private Button browseButton;
		private CheckBox overwriteCheckBox;
		private Label noteLabel;
		private Button startRestoreButton;
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
			summaryLabel = new Label();
			destinationLabel = new Label();
			destinationTextBox = new TextBox();
			browseButton = new Button();
			overwriteCheckBox = new CheckBox();
			noteLabel = new Label();
			startRestoreButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			// summaryLabel
			summaryLabel.Location = new Point(16, 16);
			summaryLabel.Name = "summaryLabel";
			summaryLabel.Size = new Size(720, 72);
			summaryLabel.TabIndex = 0;
			// destinationLabel
			destinationLabel.AutoSize = true;
			destinationLabel.Location = new Point(16, 108);
			destinationLabel.Name = "destinationLabel";
			destinationLabel.Size = new Size(87, 15);
			destinationLabel.TabIndex = 1;
			destinationLabel.Text = "Target Location:";
			// destinationTextBox
			destinationTextBox.Location = new Point(16, 132);
			destinationTextBox.Name = "destinationTextBox";
			destinationTextBox.Size = new Size(600, 23);
			destinationTextBox.TabIndex = 2;
			destinationTextBox.TextChanged += DestinationTextBox_TextChanged;
			// browseButton
			browseButton.Location = new Point(626, 130);
			browseButton.Name = "browseButton";
			browseButton.Size = new Size(110, 28);
			browseButton.TabIndex = 3;
			browseButton.Text = "Browse...";
			browseButton.UseVisualStyleBackColor = true;
			browseButton.Click += BrowseButton_Click;
			// overwriteCheckBox
			overwriteCheckBox.AutoSize = true;
			overwriteCheckBox.Location = new Point(16, 176);
			overwriteCheckBox.Name = "overwriteCheckBox";
			overwriteCheckBox.Size = new Size(128, 19);
			overwriteCheckBox.TabIndex = 4;
			overwriteCheckBox.Text = "Overwrite existing files";
			overwriteCheckBox.UseVisualStyleBackColor = true;
			// noteLabel
			noteLabel.ForeColor = Color.DimGray;
			noteLabel.Location = new Point(16, 208);
			noteLabel.Name = "noteLabel";
			noteLabel.Size = new Size(720, 56);
			noteLabel.TabIndex = 5;
			// startRestoreButton
			startRestoreButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			startRestoreButton.Location = new Point(526, 290);
			startRestoreButton.Name = "startRestoreButton";
			startRestoreButton.Size = new Size(100, 32);
			startRestoreButton.TabIndex = 6;
			startRestoreButton.Text = "Start Restore";
			startRestoreButton.UseVisualStyleBackColor = true;
			startRestoreButton.Click += StartRestoreButton_Click;
			// cancelButton
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(636, 290);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(100, 32);
			cancelButton.TabIndex = 7;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// RestoreFileSelectionForm
			AcceptButton = startRestoreButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(760, 360);
			Controls.Add(cancelButton);
			Controls.Add(startRestoreButton);
			Controls.Add(noteLabel);
			Controls.Add(overwriteCheckBox);
			Controls.Add(browseButton);
			Controls.Add(destinationTextBox);
			Controls.Add(destinationLabel);
			Controls.Add(summaryLabel);
			MinimumSize = new Size(760, 360);
			Name = "RestoreFileSelectionForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore Files and Folders";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
