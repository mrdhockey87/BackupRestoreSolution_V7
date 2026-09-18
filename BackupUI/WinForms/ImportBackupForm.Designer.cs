using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	#nullable enable
	partial class ImportBackupForm
	{
		private IContainer? components = null;
		private TableLayoutPanel? rootLayout;
		private Label? headerLabel;
		private GroupBox? fileGroupBox;
		private TableLayoutPanel? fileLayout;
		private Label? backupFileLabel;
		private TableLayoutPanel? fileBrowsePanel;
		private TextBox? filePathTextBox;
		private Button? browseButton;
		private Panel? validationPanel;
		private FlowLayoutPanel? validationLayout;
		private Label? validationStatusLabel;
		private Label? validationDetailsLabel;
		private GroupBox? backupInfoGroupBox;
		private TableLayoutPanel? infoLayout;
		private Label? formatCaptionLabel;
		private Label? formatValueLabel;
		private Label? backupNameCaptionLabel;
		private Label? backupNameValueLabel;
		private Label? backupTypeCaptionLabel;
		private Label? backupTypeValueLabel;
		private Label? timestampCaptionLabel;
		private Label? timestampValueLabel;
		private Label? sizeCaptionLabel;
		private Label? sizeValueLabel;
		private Label? compressedCaptionLabel;
		private Label? compressedValueLabel;
		private Label? encryptedCaptionLabel;
		private Label? encryptedValueLabel;
		private GroupBox? optionsGroupBox;
		private FlowLayoutPanel? optionsLayout;
		private CheckBox? renameJobCheckBox;
		private Panel? renamePanel;
		private Label? renameLabel;
		private TextBox? jobNameTextBox;
		private FlowLayoutPanel? buttonsPanel;
		private Button? cancelButton;
		private Button? importButton;

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
			rootLayout = new TableLayoutPanel();
			headerLabel = new Label();
			fileGroupBox = new GroupBox();
			fileLayout = new TableLayoutPanel();
			backupFileLabel = new Label();
			fileBrowsePanel = new TableLayoutPanel();
			filePathTextBox = new TextBox();
			browseButton = new Button();
			validationPanel = new Panel();
			validationLayout = new FlowLayoutPanel();
			validationStatusLabel = new Label();
			validationDetailsLabel = new Label();
			backupInfoGroupBox = new GroupBox();
			infoLayout = new TableLayoutPanel();
			formatCaptionLabel = new Label();
			formatValueLabel = new Label();
			backupNameCaptionLabel = new Label();
			backupNameValueLabel = new Label();
			backupTypeCaptionLabel = new Label();
			backupTypeValueLabel = new Label();
			timestampCaptionLabel = new Label();
			timestampValueLabel = new Label();
			sizeCaptionLabel = new Label();
			sizeValueLabel = new Label();
			compressedCaptionLabel = new Label();
			compressedValueLabel = new Label();
			encryptedCaptionLabel = new Label();
			encryptedValueLabel = new Label();
			optionsGroupBox = new GroupBox();
			optionsLayout = new FlowLayoutPanel();
			renameJobCheckBox = new CheckBox();
			renamePanel = new Panel();
			renameLabel = new Label();
			jobNameTextBox = new TextBox();
			buttonsPanel = new FlowLayoutPanel();
			cancelButton = new Button();
			importButton = new Button();
			rootLayout.SuspendLayout();
			fileGroupBox.SuspendLayout();
			fileLayout.SuspendLayout();
			fileBrowsePanel.SuspendLayout();
			validationPanel.SuspendLayout();
			validationLayout.SuspendLayout();
			backupInfoGroupBox.SuspendLayout();
			infoLayout.SuspendLayout();
			optionsGroupBox.SuspendLayout();
			optionsLayout.SuspendLayout();
			renamePanel.SuspendLayout();
			buttonsPanel.SuspendLayout();
			SuspendLayout();
			// 
			// rootLayout
			// 
			rootLayout.ColumnCount = 1;
			rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			rootLayout.Controls.Add(headerLabel, 0, 0);
			rootLayout.Controls.Add(fileGroupBox, 0, 1);
			rootLayout.Controls.Add(optionsGroupBox, 0, 2);
			rootLayout.Controls.Add(buttonsPanel, 0, 3);
			rootLayout.Dock = DockStyle.Fill;
			rootLayout.Location = new Point(0, 0);
			rootLayout.Name = "rootLayout";
			rootLayout.Padding = new Padding(15, 17, 15, 17);
			rootLayout.RowCount = 4;
			rootLayout.RowStyles.Add(new RowStyle());
			rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			rootLayout.RowStyles.Add(new RowStyle());
			rootLayout.RowStyles.Add(new RowStyle());
			rootLayout.Size = new Size(700, 575);
			rootLayout.TabIndex = 0;
			// 
			// headerLabel
			// 
			headerLabel.AutoSize = true;
			headerLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			headerLabel.Location = new Point(18, 17);
			headerLabel.Margin = new Padding(3, 0, 3, 17);
			headerLabel.Name = "headerLabel";
			headerLabel.Size = new Size(166, 19);
			headerLabel.TabIndex = 0;
			headerLabel.Text = "Import External Backup";
			// 
			// fileGroupBox
			// 
			fileGroupBox.Controls.Add(fileLayout);
			fileGroupBox.Dock = DockStyle.Fill;
			fileGroupBox.Location = new Point(18, 56);
			fileGroupBox.Margin = new Padding(3, 3, 3, 17);
			fileGroupBox.Name = "fileGroupBox";
			fileGroupBox.Size = new Size(664, 275);
			fileGroupBox.TabIndex = 1;
			fileGroupBox.TabStop = false;
			fileGroupBox.Text = "Select Backup File";
			// 
			// fileLayout
			// 
			fileLayout.ColumnCount = 1;
			fileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			fileLayout.Controls.Add(backupFileLabel, 0, 0);
			fileLayout.Controls.Add(fileBrowsePanel, 0, 1);
			fileLayout.Controls.Add(validationPanel, 0, 2);
			fileLayout.Controls.Add(backupInfoGroupBox, 0, 3);
			fileLayout.Dock = DockStyle.Fill;
			fileLayout.Location = new Point(3, 21);
			fileLayout.Name = "fileLayout";
			fileLayout.Padding = new Padding(10, 11, 10, 11);
			fileLayout.RowCount = 4;
			fileLayout.RowStyles.Add(new RowStyle());
			fileLayout.RowStyles.Add(new RowStyle());
			fileLayout.RowStyles.Add(new RowStyle());
			fileLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			fileLayout.Size = new Size(658, 251);
			fileLayout.TabIndex = 0;
			// 
			// backupFileLabel
			// 
			backupFileLabel.AutoSize = true;
			backupFileLabel.Location = new Point(13, 11);
			backupFileLabel.Margin = new Padding(3, 0, 3, 6);
			backupFileLabel.Name = "backupFileLabel";
			backupFileLabel.Size = new Size(75, 17);
			backupFileLabel.TabIndex = 0;
			backupFileLabel.Text = "Backup File:";
			// 
			// fileBrowsePanel
			// 
			fileBrowsePanel.AutoSize = true;
			fileBrowsePanel.ColumnCount = 2;
			fileBrowsePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			fileBrowsePanel.ColumnStyles.Add(new ColumnStyle());
			fileBrowsePanel.Controls.Add(filePathTextBox, 0, 0);
			fileBrowsePanel.Controls.Add(browseButton, 1, 0);
			fileBrowsePanel.Dock = DockStyle.Top;
			fileBrowsePanel.Location = new Point(13, 34);
			fileBrowsePanel.Margin = new Padding(3, 0, 3, 17);
			fileBrowsePanel.Name = "fileBrowsePanel";
			fileBrowsePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			fileBrowsePanel.Size = new Size(632, 20);
			fileBrowsePanel.TabIndex = 1;
			// 
			// filePathTextBox
			// 
			filePathTextBox.Dock = DockStyle.Fill;
			filePathTextBox.Location = new Point(3, 3);
			filePathTextBox.Margin = new Padding(3, 3, 10, 3);
			filePathTextBox.Name = "filePathTextBox";
			filePathTextBox.Size = new Size(513, 25);
			filePathTextBox.TabIndex = 0;
			// 
			// browseButton
			// 
			browseButton.AutoSize = true;
			browseButton.Location = new Point(529, 3);
			browseButton.MinimumSize = new Size(100, 34);
			browseButton.Name = "browseButton";
			browseButton.Size = new Size(100, 34);
			browseButton.TabIndex = 1;
			browseButton.Text = "Browse...";
			browseButton.UseVisualStyleBackColor = true;
			browseButton.Click += Browse_Click;
			// 
			// validationPanel
			// 
			validationPanel.AutoSize = true;
			validationPanel.BackColor = Color.FromArgb(232, 245, 233);
			validationPanel.Controls.Add(validationLayout);
			validationPanel.Dock = DockStyle.Top;
			validationPanel.Location = new Point(13, 71);
			validationPanel.Margin = new Padding(3, 0, 3, 17);
			validationPanel.Name = "validationPanel";
			validationPanel.Padding = new Padding(10, 11, 10, 11);
			validationPanel.Size = new Size(632, 69);
			validationPanel.TabIndex = 2;
			validationPanel.Visible = false;
			// 
			// validationLayout
			// 
			validationLayout.AutoSize = true;
			validationLayout.Controls.Add(validationStatusLabel);
			validationLayout.Controls.Add(validationDetailsLabel);
			validationLayout.Dock = DockStyle.Fill;
			validationLayout.FlowDirection = FlowDirection.TopDown;
			validationLayout.Location = new Point(10, 11);
			validationLayout.Name = "validationLayout";
			validationLayout.Size = new Size(612, 47);
			validationLayout.TabIndex = 0;
			validationLayout.WrapContents = false;
			// 
			// validationStatusLabel
			// 
			validationStatusLabel.AutoSize = true;
			validationStatusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			validationStatusLabel.ForeColor = Color.Green;
			validationStatusLabel.Location = new Point(3, 0);
			validationStatusLabel.Margin = new Padding(3, 0, 3, 11);
			validationStatusLabel.Name = "validationStatusLabel";
			validationStatusLabel.Size = new Size(0, 19);
			validationStatusLabel.TabIndex = 0;
			// 
			// validationDetailsLabel
			// 
			validationDetailsLabel.AutoSize = true;
			validationDetailsLabel.Location = new Point(3, 30);
			validationDetailsLabel.MaximumSize = new Size(620, 0);
			validationDetailsLabel.Name = "validationDetailsLabel";
			validationDetailsLabel.Size = new Size(0, 17);
			validationDetailsLabel.TabIndex = 1;
			// 
			// backupInfoGroupBox
			// 
			backupInfoGroupBox.Controls.Add(infoLayout);
			backupInfoGroupBox.Dock = DockStyle.Fill;
			backupInfoGroupBox.Location = new Point(13, 160);
			backupInfoGroupBox.Name = "backupInfoGroupBox";
			backupInfoGroupBox.Size = new Size(632, 77);
			backupInfoGroupBox.TabIndex = 3;
			backupInfoGroupBox.TabStop = false;
			backupInfoGroupBox.Text = "Backup Information";
			backupInfoGroupBox.Visible = false;
			// 
			// infoLayout
			// 
			infoLayout.ColumnCount = 2;
			infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
			infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			infoLayout.Controls.Add(formatCaptionLabel, 0, 0);
			infoLayout.Controls.Add(formatValueLabel, 1, 0);
			infoLayout.Controls.Add(backupNameCaptionLabel, 0, 1);
			infoLayout.Controls.Add(backupNameValueLabel, 1, 1);
			infoLayout.Controls.Add(backupTypeCaptionLabel, 0, 2);
			infoLayout.Controls.Add(backupTypeValueLabel, 1, 2);
			infoLayout.Controls.Add(timestampCaptionLabel, 0, 3);
			infoLayout.Controls.Add(timestampValueLabel, 1, 3);
			infoLayout.Controls.Add(sizeCaptionLabel, 0, 4);
			infoLayout.Controls.Add(sizeValueLabel, 1, 4);
			infoLayout.Controls.Add(compressedCaptionLabel, 0, 5);
			infoLayout.Controls.Add(compressedValueLabel, 1, 5);
			infoLayout.Controls.Add(encryptedCaptionLabel, 0, 6);
			infoLayout.Controls.Add(encryptedValueLabel, 1, 6);
			infoLayout.Dock = DockStyle.Fill;
			infoLayout.Location = new Point(3, 21);
			infoLayout.Name = "infoLayout";
			infoLayout.Padding = new Padding(10, 11, 10, 11);
			infoLayout.RowCount = 7;
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.RowStyles.Add(new RowStyle());
			infoLayout.Size = new Size(626, 53);
			infoLayout.TabIndex = 0;
			// 
			// formatCaptionLabel
			// 
			formatCaptionLabel.AutoSize = true;
			formatCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			formatCaptionLabel.Location = new Point(13, 17);
			formatCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			formatCaptionLabel.Name = "formatCaptionLabel";
			formatCaptionLabel.Size = new Size(61, 19);
			formatCaptionLabel.TabIndex = 0;
			formatCaptionLabel.Text = "Format:";
			// 
			// formatValueLabel
			// 
			formatValueLabel.AutoSize = true;
			formatValueLabel.Location = new Point(133, 17);
			formatValueLabel.Margin = new Padding(3, 6, 3, 6);
			formatValueLabel.Name = "formatValueLabel";
			formatValueLabel.Size = new Size(13, 17);
			formatValueLabel.TabIndex = 1;
			formatValueLabel.Text = "-";
			// 
			// backupNameCaptionLabel
			// 
			backupNameCaptionLabel.AutoSize = true;
			backupNameCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			backupNameCaptionLabel.Location = new Point(13, 48);
			backupNameCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			backupNameCaptionLabel.Name = "backupNameCaptionLabel";
			backupNameCaptionLabel.Size = new Size(106, 19);
			backupNameCaptionLabel.TabIndex = 2;
			backupNameCaptionLabel.Text = "Backup Name:";
			// 
			// backupNameValueLabel
			// 
			backupNameValueLabel.AutoSize = true;
			backupNameValueLabel.Location = new Point(133, 48);
			backupNameValueLabel.Margin = new Padding(3, 6, 3, 6);
			backupNameValueLabel.Name = "backupNameValueLabel";
			backupNameValueLabel.Size = new Size(13, 17);
			backupNameValueLabel.TabIndex = 3;
			backupNameValueLabel.Text = "-";
			// 
			// backupTypeCaptionLabel
			// 
			backupTypeCaptionLabel.AutoSize = true;
			backupTypeCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			backupTypeCaptionLabel.Location = new Point(13, 79);
			backupTypeCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			backupTypeCaptionLabel.Name = "backupTypeCaptionLabel";
			backupTypeCaptionLabel.Size = new Size(98, 19);
			backupTypeCaptionLabel.TabIndex = 4;
			backupTypeCaptionLabel.Text = "Backup Type:";
			// 
			// backupTypeValueLabel
			// 
			backupTypeValueLabel.AutoSize = true;
			backupTypeValueLabel.Location = new Point(133, 79);
			backupTypeValueLabel.Margin = new Padding(3, 6, 3, 6);
			backupTypeValueLabel.Name = "backupTypeValueLabel";
			backupTypeValueLabel.Size = new Size(13, 17);
			backupTypeValueLabel.TabIndex = 5;
			backupTypeValueLabel.Text = "-";
			// 
			// timestampCaptionLabel
			// 
			timestampCaptionLabel.AutoSize = true;
			timestampCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			timestampCaptionLabel.Location = new Point(13, 110);
			timestampCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			timestampCaptionLabel.Name = "timestampCaptionLabel";
			timestampCaptionLabel.Size = new Size(101, 19);
			timestampCaptionLabel.TabIndex = 6;
			timestampCaptionLabel.Text = "Date Created:";
			// 
			// timestampValueLabel
			// 
			timestampValueLabel.AutoSize = true;
			timestampValueLabel.Location = new Point(133, 110);
			timestampValueLabel.Margin = new Padding(3, 6, 3, 6);
			timestampValueLabel.Name = "timestampValueLabel";
			timestampValueLabel.Size = new Size(13, 17);
			timestampValueLabel.TabIndex = 7;
			timestampValueLabel.Text = "-";
			// 
			// sizeCaptionLabel
			// 
			sizeCaptionLabel.AutoSize = true;
			sizeCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			sizeCaptionLabel.Location = new Point(13, 141);
			sizeCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			sizeCaptionLabel.Name = "sizeCaptionLabel";
			sizeCaptionLabel.Size = new Size(40, 19);
			sizeCaptionLabel.TabIndex = 8;
			sizeCaptionLabel.Text = "Size:";
			// 
			// sizeValueLabel
			// 
			sizeValueLabel.AutoSize = true;
			sizeValueLabel.Location = new Point(133, 141);
			sizeValueLabel.Margin = new Padding(3, 6, 3, 6);
			sizeValueLabel.Name = "sizeValueLabel";
			sizeValueLabel.Size = new Size(13, 17);
			sizeValueLabel.TabIndex = 9;
			sizeValueLabel.Text = "-";
			// 
			// compressedCaptionLabel
			// 
			compressedCaptionLabel.AutoSize = true;
			compressedCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			compressedCaptionLabel.Location = new Point(13, 172);
			compressedCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			compressedCaptionLabel.Name = "compressedCaptionLabel";
			compressedCaptionLabel.Size = new Size(96, 19);
			compressedCaptionLabel.TabIndex = 10;
			compressedCaptionLabel.Text = "Compressed:";
			// 
			// compressedValueLabel
			// 
			compressedValueLabel.AutoSize = true;
			compressedValueLabel.Location = new Point(133, 172);
			compressedValueLabel.Margin = new Padding(3, 6, 3, 6);
			compressedValueLabel.Name = "compressedValueLabel";
			compressedValueLabel.Size = new Size(13, 17);
			compressedValueLabel.TabIndex = 11;
			compressedValueLabel.Text = "-";
			// 
			// encryptedCaptionLabel
			// 
			encryptedCaptionLabel.AutoSize = true;
			encryptedCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			encryptedCaptionLabel.Location = new Point(13, 203);
			encryptedCaptionLabel.Margin = new Padding(3, 6, 3, 6);
			encryptedCaptionLabel.Name = "encryptedCaptionLabel";
			encryptedCaptionLabel.Size = new Size(81, 19);
			encryptedCaptionLabel.TabIndex = 12;
			encryptedCaptionLabel.Text = "Encrypted:";
			// 
			// encryptedValueLabel
			// 
			encryptedValueLabel.AutoSize = true;
			encryptedValueLabel.Location = new Point(133, 203);
			encryptedValueLabel.Margin = new Padding(3, 6, 3, 6);
			encryptedValueLabel.Name = "encryptedValueLabel";
			encryptedValueLabel.Size = new Size(13, 17);
			encryptedValueLabel.TabIndex = 13;
			encryptedValueLabel.Text = "-";
			// 
			// optionsGroupBox
			// 
			optionsGroupBox.AutoSize = true;
			optionsGroupBox.Controls.Add(optionsLayout);
			optionsGroupBox.Dock = DockStyle.Top;
			optionsGroupBox.Location = new Point(18, 351);
			optionsGroupBox.Margin = new Padding(3, 3, 3, 17);
			optionsGroupBox.Name = "optionsGroupBox";
			optionsGroupBox.Size = new Size(664, 138);
			optionsGroupBox.TabIndex = 2;
			optionsGroupBox.TabStop = false;
			optionsGroupBox.Text = "Import Options";
			// 
			// optionsLayout
			// 
			optionsLayout.AutoSize = true;
			optionsLayout.Controls.Add(renameJobCheckBox);
			optionsLayout.Controls.Add(renamePanel);
			optionsLayout.Dock = DockStyle.Fill;
			optionsLayout.FlowDirection = FlowDirection.TopDown;
			optionsLayout.Location = new Point(3, 21);
			optionsLayout.Name = "optionsLayout";
			optionsLayout.Padding = new Padding(10, 11, 10, 11);
			optionsLayout.Size = new Size(658, 114);
			optionsLayout.TabIndex = 0;
			optionsLayout.WrapContents = false;
			// 
			// renameJobCheckBox
			// 
			renameJobCheckBox.AutoSize = true;
			renameJobCheckBox.Location = new Point(13, 14);
			renameJobCheckBox.Margin = new Padding(3, 3, 3, 11);
			renameJobCheckBox.Name = "renameJobCheckBox";
			renameJobCheckBox.Size = new Size(178, 21);
			renameJobCheckBox.TabIndex = 0;
			renameJobCheckBox.Text = "Rename imported backup";
			renameJobCheckBox.UseVisualStyleBackColor = true;
			renameJobCheckBox.CheckedChanged += RenameJob_Changed;
			// 
			// renamePanel
			// 
			renamePanel.AutoSize = true;
			renamePanel.Controls.Add(renameLabel);
			renamePanel.Controls.Add(jobNameTextBox);
			renamePanel.Location = new Point(13, 49);
			renamePanel.Name = "renamePanel";
			renamePanel.Size = new Size(303, 51);
			renamePanel.TabIndex = 1;
			renamePanel.Visible = false;
			// 
			// renameLabel
			// 
			renameLabel.AutoSize = true;
			renameLabel.Location = new Point(0, 0);
			renameLabel.Margin = new Padding(0, 0, 0, 6);
			renameLabel.Name = "renameLabel";
			renameLabel.Size = new Size(101, 17);
			renameLabel.TabIndex = 0;
			renameLabel.Text = "New Job Name:";
			// 
			// jobNameTextBox
			// 
			jobNameTextBox.Location = new Point(0, 23);
			jobNameTextBox.Name = "jobNameTextBox";
			jobNameTextBox.Size = new Size(300, 25);
			jobNameTextBox.TabIndex = 1;
			// 
			// buttonsPanel
			// 
			buttonsPanel.AutoSize = true;
			buttonsPanel.Controls.Add(cancelButton);
			buttonsPanel.Controls.Add(importButton);
			buttonsPanel.Dock = DockStyle.Fill;
			buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
			buttonsPanel.Location = new Point(18, 509);
			buttonsPanel.Name = "buttonsPanel";
			buttonsPanel.Size = new Size(664, 46);
			buttonsPanel.TabIndex = 3;
			// 
			// cancelButton
			// 
			cancelButton.AutoSize = true;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(561, 3);
			cancelButton.MinimumSize = new Size(100, 40);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(100, 40);
			cancelButton.TabIndex = 0;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// 
			// importButton
			// 
			importButton.AutoSize = true;
			importButton.Enabled = false;
			importButton.Location = new Point(448, 3);
			importButton.Margin = new Padding(0, 3, 10, 3);
			importButton.MinimumSize = new Size(100, 40);
			importButton.Name = "importButton";
			importButton.Size = new Size(100, 40);
			importButton.TabIndex = 1;
			importButton.Text = "Import";
			importButton.UseVisualStyleBackColor = true;
			importButton.Click += Import_Click;
			// 
			// ImportBackupForm
			// 
			AcceptButton = importButton;
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = cancelButton;
			ClientSize = new Size(700, 575);
			Controls.Add(rootLayout);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ImportBackupForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Import Backup";
			rootLayout.ResumeLayout(false);
			rootLayout.PerformLayout();
			fileGroupBox.ResumeLayout(false);
			fileLayout.ResumeLayout(false);
			fileLayout.PerformLayout();
			fileBrowsePanel.ResumeLayout(false);
			fileBrowsePanel.PerformLayout();
			validationPanel.ResumeLayout(false);
			validationPanel.PerformLayout();
			validationLayout.ResumeLayout(false);
			validationLayout.PerformLayout();
			backupInfoGroupBox.ResumeLayout(false);
			infoLayout.ResumeLayout(false);
			infoLayout.PerformLayout();
			optionsGroupBox.ResumeLayout(false);
			optionsGroupBox.PerformLayout();
			optionsLayout.ResumeLayout(false);
			optionsLayout.PerformLayout();
			renamePanel.ResumeLayout(false);
			renamePanel.PerformLayout();
			buttonsPanel.ResumeLayout(false);
			buttonsPanel.PerformLayout();
			ResumeLayout(false);
		}
	}
}
