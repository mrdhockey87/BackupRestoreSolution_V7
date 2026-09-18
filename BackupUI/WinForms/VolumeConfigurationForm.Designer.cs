using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SecureServerBackup.WinForms.Controls;

namespace SecureServerBackup.WinForms
{
	partial class VolumeConfigurationForm
	{
		private IContainer components;
		private Label titleLabel;
		private Label sourceDiskInfoLabel;
		private Label targetDiskInfoLabel;
		private Label spaceInfoLabel;
		private Label warningLabel;
		private VolumeResizeControl resizeControl;
		private Label instructionsLabel;
		private Label statusLabel;
		private Button autoFitButton;
		private Button resetButton;
		private Button acceptButton;
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
			titleLabel = new Label();
			sourceDiskInfoLabel = new Label();
			targetDiskInfoLabel = new Label();
			spaceInfoLabel = new Label();
			warningLabel = new Label();
			resizeControl = new VolumeResizeControl();
			instructionsLabel = new Label();
			statusLabel = new Label();
			autoFitButton = new Button();
			resetButton = new Button();
			acceptButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			// titleLabel
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(154, 15);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Configure Restore Volume Sizes";
			// sourceDiskInfoLabel
			sourceDiskInfoLabel.AutoSize = true;
			sourceDiskInfoLabel.Location = new Point(16, 52);
			sourceDiskInfoLabel.Name = "sourceDiskInfoLabel";
			sourceDiskInfoLabel.Size = new Size(0, 15);
			sourceDiskInfoLabel.TabIndex = 1;
			// targetDiskInfoLabel
			targetDiskInfoLabel.AutoSize = true;
			targetDiskInfoLabel.Location = new Point(16, 76);
			targetDiskInfoLabel.Name = "targetDiskInfoLabel";
			targetDiskInfoLabel.Size = new Size(0, 15);
			targetDiskInfoLabel.TabIndex = 2;
			// spaceInfoLabel
			spaceInfoLabel.AutoSize = true;
			spaceInfoLabel.ForeColor = Color.DimGray;
			spaceInfoLabel.Location = new Point(16, 100);
			spaceInfoLabel.Name = "spaceInfoLabel";
			spaceInfoLabel.Size = new Size(0, 15);
			spaceInfoLabel.TabIndex = 3;
			// warningLabel
			warningLabel.ForeColor = Color.DarkOrange;
			warningLabel.Location = new Point(16, 128);
			warningLabel.Name = "warningLabel";
			warningLabel.Size = new Size(940, 50);
			warningLabel.TabIndex = 4;
			warningLabel.Visible = false;
			// resizeControl
			resizeControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			resizeControl.Location = new Point(16, 190);
			resizeControl.Name = "resizeControl";
			resizeControl.Size = new Size(940, 330);
			resizeControl.TabIndex = 5;
			// instructionsLabel
			instructionsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			instructionsLabel.Location = new Point(16, 530);
			instructionsLabel.Name = "instructionsLabel";
			instructionsLabel.Size = new Size(940, 36);
			instructionsLabel.TabIndex = 6;
			instructionsLabel.Text = "Drag the red handles in the target layout to resize adjacent volumes. Use Auto Fit to proportionally fill the target disk or Reset to restore the default layout.";
			// statusLabel
			statusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			statusLabel.ForeColor = Color.DimGray;
			statusLabel.Location = new Point(16, 572);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(620, 28);
			statusLabel.TabIndex = 7;
			statusLabel.Text = "Ready.";
			// autoFitButton
			autoFitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			autoFitButton.Location = new Point(646, 568);
			autoFitButton.Name = "autoFitButton";
			autoFitButton.Size = new Size(100, 32);
			autoFitButton.TabIndex = 8;
			autoFitButton.Text = "Auto Fit";
			autoFitButton.UseVisualStyleBackColor = true;
			autoFitButton.Click += AutoFitButton_Click;
			// resetButton
			resetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			resetButton.Location = new Point(756, 568);
			resetButton.Name = "resetButton";
			resetButton.Size = new Size(100, 32);
			resetButton.TabIndex = 9;
			resetButton.Text = "Reset";
			resetButton.UseVisualStyleBackColor = true;
			resetButton.Click += ResetButton_Click;
			// acceptButton
			acceptButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			acceptButton.Location = new Point(646, 608);
			acceptButton.Name = "acceptButton";
			acceptButton.Size = new Size(100, 32);
			acceptButton.TabIndex = 10;
			acceptButton.Text = "Accept";
			acceptButton.UseVisualStyleBackColor = true;
			acceptButton.Click += AcceptButton_Click;
			// cancelButton
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(756, 608);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(100, 32);
			cancelButton.TabIndex = 11;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// VolumeConfigurationForm
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(980, 680);
			Controls.Add(cancelButton);
			Controls.Add(acceptButton);
			Controls.Add(resetButton);
			Controls.Add(autoFitButton);
			Controls.Add(statusLabel);
			Controls.Add(instructionsLabel);
			Controls.Add(resizeControl);
			Controls.Add(warningLabel);
			Controls.Add(spaceInfoLabel);
			Controls.Add(targetDiskInfoLabel);
			Controls.Add(sourceDiskInfoLabel);
			Controls.Add(titleLabel);
			MinimumSize = new Size(900, 620);
			Name = "VolumeConfigurationForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Configure Volume Sizes";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
