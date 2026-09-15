using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class RecoveryEnvironmentForm
	{
		private IContainer components;
		private Label titleLabel;
		private Label isoPathHeaderLabel;
		private TextBox isoPathTextBox;
		private Label isoStatusLabel;
		private Label isoNoteLabel;
		private Button openIsoLocationButton;
		private Button openRufusButton;
		private Button printInstructionsButton;
		private TextBox instructionsTextBox;
		private Button closeButton;

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
			isoPathHeaderLabel = new Label();
			isoPathTextBox = new TextBox();
			isoStatusLabel = new Label();
			isoNoteLabel = new Label();
			openIsoLocationButton = new Button();
			openRufusButton = new Button();
			printInstructionsButton = new Button();
			instructionsTextBox = new TextBox();
			closeButton = new Button();
			SuspendLayout();
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(156, 20);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Recovery USB Creation";
			isoPathHeaderLabel.AutoSize = true;
			isoPathHeaderLabel.Location = new Point(16, 54);
			isoPathHeaderLabel.Name = "isoPathHeaderLabel";
			isoPathHeaderLabel.Size = new Size(125, 20);
			isoPathHeaderLabel.TabIndex = 1;
			isoPathHeaderLabel.Text = "Recovery ISO Path:";
			isoPathTextBox.Location = new Point(16, 76);
			isoPathTextBox.Name = "isoPathTextBox";
			isoPathTextBox.ReadOnly = true;
			isoPathTextBox.Size = new Size(710, 27);
			isoPathTextBox.TabIndex = 2;
			isoStatusLabel.AutoSize = true;
			isoStatusLabel.Location = new Point(16, 110);
			isoStatusLabel.Name = "isoStatusLabel";
			isoStatusLabel.Size = new Size(85, 20);
			isoStatusLabel.TabIndex = 3;
			isoStatusLabel.Text = "ISO status...";
			isoNoteLabel.Location = new Point(16, 136);
			isoNoteLabel.Name = "isoNoteLabel";
			isoNoteLabel.Size = new Size(710, 54);
			isoNoteLabel.TabIndex = 4;
			openIsoLocationButton.Location = new Point(16, 198);
			openIsoLocationButton.Name = "openIsoLocationButton";
			openIsoLocationButton.Size = new Size(140, 32);
			openIsoLocationButton.TabIndex = 5;
			openIsoLocationButton.Text = "Open ISO Location";
			openIsoLocationButton.UseVisualStyleBackColor = true;
			openIsoLocationButton.Click += OpenIsoLocationButton_Click;
			openRufusButton.Location = new Point(166, 198);
			openRufusButton.Name = "openRufusButton";
			openRufusButton.Size = new Size(150, 32);
			openRufusButton.TabIndex = 6;
			openRufusButton.Text = "Open Rufus Website";
			openRufusButton.UseVisualStyleBackColor = true;
			openRufusButton.Click += OpenRufusButton_Click;
			printInstructionsButton.Location = new Point(326, 198);
			printInstructionsButton.Name = "printInstructionsButton";
			printInstructionsButton.Size = new Size(140, 32);
			printInstructionsButton.TabIndex = 7;
			printInstructionsButton.Text = "Print Instructions";
			printInstructionsButton.UseVisualStyleBackColor = true;
			printInstructionsButton.Click += PrintInstructionsButton_Click;
			instructionsTextBox.Location = new Point(16, 246);
			instructionsTextBox.Multiline = true;
			instructionsTextBox.Name = "instructionsTextBox";
			instructionsTextBox.ReadOnly = true;
			instructionsTextBox.ScrollBars = ScrollBars.Vertical;
			instructionsTextBox.Size = new Size(710, 260);
			instructionsTextBox.TabIndex = 8;
			instructionsTextBox.Text = "1. Download Rufus from https://rufus.ie\r\n2. Select the recovery ISO shown above.\r\n3. Create a bootable USB drive in Rufus.\r\n4. Boot from the USB drive on the target machine.\r\n\r\nRestore options on the recovery media:\r\n- restore_gui: graphical interface\r\n- restore_tui: terminal UI\r\n- restore_cli: direct command-line restore";
			closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			closeButton.DialogResult = DialogResult.OK;
			closeButton.Location = new Point(626, 516);
			closeButton.Name = "closeButton";
			closeButton.Size = new Size(100, 32);
			closeButton.TabIndex = 9;
			closeButton.Text = "Close";
			closeButton.UseVisualStyleBackColor = true;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(760, 560);
			Controls.Add(closeButton);
			Controls.Add(instructionsTextBox);
			Controls.Add(printInstructionsButton);
			Controls.Add(openRufusButton);
			Controls.Add(openIsoLocationButton);
			Controls.Add(isoNoteLabel);
			Controls.Add(isoStatusLabel);
			Controls.Add(isoPathTextBox);
			Controls.Add(isoPathHeaderLabel);
			Controls.Add(titleLabel);
			MinimumSize = new Size(760, 560);
			Name = "RecoveryEnvironmentForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Recovery Environment Creator";
			Load += RecoveryEnvironmentForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
