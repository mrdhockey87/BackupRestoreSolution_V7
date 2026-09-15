using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class BackupProgressForm
	{
		private IContainer components;
		private Label progressLabel;
		private ProgressBar progressBar;
		private Label percentageLabel;
		private Label currentFileLabel;
		private TextBox currentFileTextBox;
		private Button abortButton;
		private Button hideCloseButton;
		private Timer progressTimer;

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
			progressTimer = new Timer(components);
			progressLabel = new Label();
			progressBar = new ProgressBar();
			percentageLabel = new Label();
			currentFileLabel = new Label();
			currentFileTextBox = new TextBox();
			abortButton = new Button();
			hideCloseButton = new Button();
			SuspendLayout();
			progressTimer.Interval = 1000;
			progressTimer.Tick += ProgressTimer_Tick;
			progressLabel.Location = new Point(20, 20);
			progressLabel.Name = "progressLabel";
			progressLabel.Size = new Size(600, 24);
			progressLabel.TabIndex = 0;
			progressLabel.Text = "Initializing backup...";
			progressBar.Location = new Point(20, 56);
			progressBar.Maximum = 100;
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(600, 24);
			progressBar.TabIndex = 1;
			percentageLabel.AutoSize = true;
			percentageLabel.Location = new Point(20, 90);
			percentageLabel.Name = "percentageLabel";
			percentageLabel.Size = new Size(23, 20);
			percentageLabel.TabIndex = 2;
			percentageLabel.Text = "0%";
			currentFileLabel.AutoSize = true;
			currentFileLabel.Location = new Point(20, 122);
			currentFileLabel.Name = "currentFileLabel";
			currentFileLabel.Size = new Size(81, 20);
			currentFileLabel.TabIndex = 3;
			currentFileLabel.Text = "Current File:";
			currentFileTextBox.Location = new Point(20, 146);
			currentFileTextBox.Multiline = true;
			currentFileTextBox.Name = "currentFileTextBox";
			currentFileTextBox.ReadOnly = true;
			currentFileTextBox.ScrollBars = ScrollBars.Vertical;
			currentFileTextBox.Size = new Size(600, 96);
			currentFileTextBox.TabIndex = 4;
			abortButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			abortButton.Location = new Point(410, 262);
			abortButton.Name = "abortButton";
			abortButton.Size = new Size(100, 32);
			abortButton.TabIndex = 5;
			abortButton.Text = "Abort";
			abortButton.UseVisualStyleBackColor = true;
			abortButton.Click += AbortButton_Click;
			hideCloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			hideCloseButton.Location = new Point(520, 262);
			hideCloseButton.Name = "hideCloseButton";
			hideCloseButton.Size = new Size(100, 32);
			hideCloseButton.TabIndex = 6;
			hideCloseButton.Text = "Hide";
			hideCloseButton.UseVisualStyleBackColor = true;
			hideCloseButton.Click += HideCloseButton_Click;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(660, 320);
			Controls.Add(hideCloseButton);
			Controls.Add(abortButton);
			Controls.Add(currentFileTextBox);
			Controls.Add(currentFileLabel);
			Controls.Add(percentageLabel);
			Controls.Add(progressBar);
			Controls.Add(progressLabel);
			MinimumSize = new Size(660, 320);
			Name = "BackupProgressForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Backup Progress";
			Load += BackupProgressForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
