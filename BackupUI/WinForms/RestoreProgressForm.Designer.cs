using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class RestoreProgressForm
	{
		private IContainer components;
		private Label progressLabel;
		private ProgressBar progressBar;
		private Label percentageLabel;
		private TextBox currentItemTextBox;
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
			progressLabel = new Label();
			progressBar = new ProgressBar();
			percentageLabel = new Label();
			currentItemTextBox = new TextBox();
			closeButton = new Button();
			SuspendLayout();
			progressLabel.Location = new Point(20, 20);
			progressLabel.Name = "progressLabel";
			progressLabel.Size = new Size(560, 24);
			progressLabel.TabIndex = 0;
			progressLabel.Text = "Preparing restore...";
			progressBar.Location = new Point(20, 56);
			progressBar.Maximum = 100;
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(560, 24);
			progressBar.TabIndex = 1;
			percentageLabel.AutoSize = true;
			percentageLabel.Location = new Point(20, 90);
			percentageLabel.Name = "percentageLabel";
			percentageLabel.Size = new Size(23, 20);
			percentageLabel.TabIndex = 2;
			percentageLabel.Text = "0%";
			currentItemTextBox.Location = new Point(20, 120);
			currentItemTextBox.Multiline = true;
			currentItemTextBox.Name = "currentItemTextBox";
			currentItemTextBox.ReadOnly = true;
			currentItemTextBox.ScrollBars = ScrollBars.Vertical;
			currentItemTextBox.Size = new Size(560, 70);
			currentItemTextBox.TabIndex = 3;
			closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			closeButton.Location = new Point(480, 208);
			closeButton.Name = "closeButton";
			closeButton.Size = new Size(100, 32);
			closeButton.TabIndex = 4;
			closeButton.Text = "Hide";
			closeButton.UseVisualStyleBackColor = true;
			closeButton.Click += CloseButton_Click;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(620, 260);
			Controls.Add(closeButton);
			Controls.Add(currentItemTextBox);
			Controls.Add(percentageLabel);
			Controls.Add(progressBar);
			Controls.Add(progressLabel);
			MinimumSize = new Size(620, 260);
			Name = "RestoreProgressForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore Progress";
			Load += RestoreProgressForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
