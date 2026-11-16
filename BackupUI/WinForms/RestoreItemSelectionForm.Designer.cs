using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	#nullable enable
	partial class RestoreItemSelectionForm
	{
		private IContainer? components = null;
		private Label summaryLabel = null!;
		private ListBox itemsListBox = null!;
		private Panel pnlLoading = null!;
		private Label txtLoadingStatus = null!;
		private ProgressBar pbLoading = null!;
		private Label helpLabel = null!;
		private Button nextButton = null!;
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
			pnlLoading = new Panel();
			pbLoading = new ProgressBar();
			txtLoadingStatus = new Label();
			helpLabel = new Label();
			nextButton = new Button();
			cancelButton = new Button();
			pnlLoading.SuspendLayout();
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
			// pnlLoading
			// 
			pnlLoading.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pnlLoading.BackColor = Color.FromArgb(245, 245, 245);
			pnlLoading.BorderStyle = BorderStyle.FixedSingle;
			pnlLoading.Controls.Add(pbLoading);
			pnlLoading.Controls.Add(txtLoadingStatus);
			pnlLoading.Location = new Point(14, 63);
			pnlLoading.Name = "pnlLoading";
			pnlLoading.Size = new Size(630, 246);
			pnlLoading.TabIndex = 2;
			pnlLoading.Visible = false;
			// 
			// pbLoading
			// 
			pbLoading.Anchor = AnchorStyles.None;
			pbLoading.Location = new Point(630, 264);
			pbLoading.Name = "pbLoading";
			pbLoading.Size = new Size(220, 18);
			pbLoading.Style = ProgressBarStyle.Marquee;
			pbLoading.TabIndex = 1;
			// 
			// txtLoadingStatus
			// 
			txtLoadingStatus.Anchor = AnchorStyles.None;
			txtLoadingStatus.Location = new Point(630, 244);
			txtLoadingStatus.Name = "txtLoadingStatus";
			txtLoadingStatus.Size = new Size(220, 20);
			txtLoadingStatus.TabIndex = 0;
			txtLoadingStatus.Text = "Loading backup contents…";
			txtLoadingStatus.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// helpLabel
			// 
			helpLabel.Location = new Point(14, 308);
			helpLabel.Name = "helpLabel";
			helpLabel.Size = new Size(630, 20);
			helpLabel.TabIndex = 3;
			helpLabel.Text = "Select one or more files or folders to restore, then click Next.";
			// 
			// nextButton
			// 
			nextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			nextButton.Location = new Point(472, 355);
			nextButton.Name = "nextButton";
			nextButton.Size = new Size(84, 27);
			nextButton.TabIndex = 4;
			nextButton.Text = "Next";
			nextButton.UseVisualStyleBackColor = true;
			nextButton.Click += NextButton_Click;
			// 
			// cancelButton
			// 
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(560, 355);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(84, 27);
			cancelButton.TabIndex = 5;
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
			ClientSize = new Size(665, 390);
			Controls.Add(cancelButton);
			Controls.Add(nextButton);
			Controls.Add(helpLabel);
			Controls.Add(itemsListBox);
			Controls.Add(pnlLoading);
			Controls.Add(summaryLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new Size(667, 372);
			Name = "RestoreItemSelectionForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Select Restore Items";
			pnlLoading.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
