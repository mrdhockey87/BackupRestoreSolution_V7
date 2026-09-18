using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	#nullable enable
	partial class RestoreItemSelectionForm
	{
		private IContainer? components = null;
		private Label summaryLabel;
		private ListBox itemsListBox;
		private Label helpLabel;
		private Button nextButton;
		private Button? cancelButton;

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
			summaryLabel = new Label();
			itemsListBox = new ListBox();
			helpLabel = new Label();
			nextButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			// 
			// summaryLabel
			// 
			summaryLabel.Location = new Point(14, 14);
			summaryLabel.Name = "summaryLabel";
			summaryLabel.Size = new Size(630, 41);
			summaryLabel.TabIndex = 0;
			// 
			// itemsListBox
			// 
			itemsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			itemsListBox.FormattingEnabled = true;
			itemsListBox.ItemHeight = 17;
			itemsListBox.Location = new Point(14, 63);
			itemsListBox.Name = "itemsListBox";
			itemsListBox.SelectionMode = SelectionMode.MultiExtended;
			itemsListBox.Size = new Size(630, 242);
			itemsListBox.TabIndex = 1;
			// 
			// helpLabel
			// 
			helpLabel.Location = new Point(14, 308);
			helpLabel.Name = "helpLabel";
			helpLabel.Size = new Size(630, 20);
			helpLabel.TabIndex = 2;
			helpLabel.Text = "Select one or more files or folders to restore, then click Next.";
			// 
			// nextButton
			// 
			nextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			nextButton.Location = new Point(472, 343);
			nextButton.Name = "nextButton";
			nextButton.Size = new Size(84, 27);
			nextButton.TabIndex = 3;
			nextButton.Text = "Next";
			nextButton.UseVisualStyleBackColor = true;
			nextButton.Click += NextButton_Click;
			// 
			// cancelButton
			// 
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(560, 343);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(84, 27);
			cancelButton.TabIndex = 4;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// 
			// RestoreItemSelectionForm
			// 
			AcceptButton = nextButton;
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(665, 378);
			Controls.Add(cancelButton);
			Controls.Add(nextButton);
			Controls.Add(helpLabel);
			Controls.Add(itemsListBox);
			Controls.Add(summaryLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new Size(667, 372);
			Name = "RestoreItemSelectionForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Select Restore Items";
			ResumeLayout(false);
		}
	}
}
