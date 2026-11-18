using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable enable
namespace SecureServerBackup.WinForms
{
	partial class RestorePointSelectionForm
	{
		private IContainer? components = null;
		private Label summaryLabel = null!;
		private ListBox restorePointsListBox = null!;
		private Label helpLabel = null!;
		private Button nextButton = null!;
		private Button? cancelButton;
		private Panel pnlLoading = null!;
		private Label txtLoadingStatus = null!;
		private ProgressBar pbLoading = null!;

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
			restorePointsListBox = new ListBox();
			helpLabel = new Label();
			nextButton = new Button();
			cancelButton = new Button();
			pnlLoading = new Panel();
			txtLoadingStatus = new Label();
			pbLoading = new ProgressBar();
			pnlLoading.SuspendLayout();
			SuspendLayout();
			// 
			// pnlLoading
			// 
			pnlLoading.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pnlLoading.BackColor = Color.White;
			pnlLoading.Controls.Add(pbLoading);
			pnlLoading.Controls.Add(txtLoadingStatus);
			pnlLoading.Location = new Point(14, 70);
			pnlLoading.Name = "pnlLoading";
			pnlLoading.Size = new Size(630, 208);
			pnlLoading.TabIndex = 5;
			pnlLoading.Visible = false;
			// 
			// txtLoadingStatus
			// 
			txtLoadingStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtLoadingStatus.Location = new Point(10, 60);
			txtLoadingStatus.Name = "txtLoadingStatus";
			txtLoadingStatus.Size = new Size(610, 23);
			txtLoadingStatus.TabIndex = 0;
			txtLoadingStatus.Text = "Loading restore points…";
			txtLoadingStatus.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// pbLoading
			// 
			pbLoading.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			pbLoading.Location = new Point(10, 90);
			pbLoading.Name = "pbLoading";
			pbLoading.Size = new Size(610, 23);
			pbLoading.Style = ProgressBarStyle.Marquee;
			pbLoading.TabIndex = 1;
			// 
			// summaryLabel
			// 
			summaryLabel.Location = new Point(14, 14);
			summaryLabel.Name = "summaryLabel";
			summaryLabel.Size = new Size(630, 48);
			summaryLabel.TabIndex = 0;
			// 
			// restorePointsListBox
			// 
			restorePointsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			restorePointsListBox.DisplayMember = "DisplayName";
			restorePointsListBox.FormattingEnabled = true;
			restorePointsListBox.ItemHeight = 17;
			restorePointsListBox.Location = new Point(14, 70);
			restorePointsListBox.Name = "restorePointsListBox";
			restorePointsListBox.Size = new Size(630, 208);
			restorePointsListBox.TabIndex = 1;
			restorePointsListBox.DoubleClick += RestorePointsListBox_DoubleClick;
			// 
			// helpLabel
			// 
			helpLabel.Location = new Point(14, 289);
			helpLabel.Name = "helpLabel";
			helpLabel.Size = new Size(630, 20);
			helpLabel.TabIndex = 2;
			// 
			// nextButton
			// 
			nextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			nextButton.Location = new Point(472, 346);
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
			cancelButton.Location = new Point(560, 346);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(84, 27);
			cancelButton.TabIndex = 4;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// 
			// RestorePointSelectionForm
			// 
			AcceptButton = nextButton;
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(665, 387);
			Controls.Add(pnlLoading);
			Controls.Add(cancelButton);
			Controls.Add(nextButton);
			Controls.Add(helpLabel);
			Controls.Add(restorePointsListBox);
			Controls.Add(summaryLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new Size(667, 363);
			Name = "RestorePointSelectionForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Select Restore Point";
			pnlLoading.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
