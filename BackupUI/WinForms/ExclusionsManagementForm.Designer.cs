using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class ExclusionsManagementForm
	{
		private IContainer components;
		private Label titleLabel;
		private FlowLayoutPanel buttonPanel;
		private Button addFileButton;
		private Button addFolderButton;
		private Button removeSelectedButton;
		private Button clearAllButton;
		private Label patternLabel;
		private TextBox extensionPatternTextBox;
		private Button addPatternButton;
		private ListBox exclusionsListBox;
		private Label noExclusionsLabel;
		private Label statusLabel;
		private Button okButton;
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
			buttonPanel = new FlowLayoutPanel();
			addFileButton = new Button();
			addFolderButton = new Button();
			removeSelectedButton = new Button();
			clearAllButton = new Button();
			patternLabel = new Label();
			extensionPatternTextBox = new TextBox();
			addPatternButton = new Button();
			exclusionsListBox = new ListBox();
			noExclusionsLabel = new Label();
			statusLabel = new Label();
			okButton = new Button();
			cancelButton = new Button();
			buttonPanel.SuspendLayout();
			SuspendLayout();
			// titleLabel
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(148, 15);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Custom Backup Exclusions";
			// buttonPanel
			buttonPanel.Controls.Add(addFileButton);
			buttonPanel.Controls.Add(addFolderButton);
			buttonPanel.Controls.Add(removeSelectedButton);
			buttonPanel.Controls.Add(clearAllButton);
			buttonPanel.Location = new Point(16, 48);
			buttonPanel.Name = "buttonPanel";
			buttonPanel.Size = new Size(680, 36);
			buttonPanel.TabIndex = 1;
			buttonPanel.WrapContents = true;
			// addFileButton
			addFileButton.AutoSize = true;
			addFileButton.Margin = new Padding(0, 0, 8, 0);
			addFileButton.MinimumSize = new Size(120, 30);
			addFileButton.Name = "addFileButton";
			addFileButton.Size = new Size(120, 30);
			addFileButton.TabIndex = 0;
			addFileButton.Text = "Add File...";
			addFileButton.UseVisualStyleBackColor = true;
			addFileButton.Click += AddFileButton_Click;
			// addFolderButton
			addFolderButton.AutoSize = true;
			addFolderButton.Margin = new Padding(0, 0, 8, 0);
			addFolderButton.MinimumSize = new Size(120, 30);
			addFolderButton.Name = "addFolderButton";
			addFolderButton.Size = new Size(120, 30);
			addFolderButton.TabIndex = 1;
			addFolderButton.Text = "Add Folder...";
			addFolderButton.UseVisualStyleBackColor = true;
			addFolderButton.Click += AddFolderButton_Click;
			// removeSelectedButton
			removeSelectedButton.AutoSize = true;
			removeSelectedButton.Margin = new Padding(0, 0, 8, 0);
			removeSelectedButton.MinimumSize = new Size(120, 30);
			removeSelectedButton.Name = "removeSelectedButton";
			removeSelectedButton.Size = new Size(124, 30);
			removeSelectedButton.TabIndex = 2;
			removeSelectedButton.Text = "Remove Selected";
			removeSelectedButton.UseVisualStyleBackColor = true;
			removeSelectedButton.Click += RemoveSelectedButton_Click;
			// clearAllButton
			clearAllButton.AutoSize = true;
			clearAllButton.Margin = new Padding(0, 0, 8, 0);
			clearAllButton.MinimumSize = new Size(120, 30);
			clearAllButton.Name = "clearAllButton";
			clearAllButton.Size = new Size(120, 30);
			clearAllButton.TabIndex = 3;
			clearAllButton.Text = "Clear All";
			clearAllButton.UseVisualStyleBackColor = true;
			clearAllButton.Click += ClearAllButton_Click;
			// patternLabel
			patternLabel.AutoSize = true;
			patternLabel.Location = new Point(16, 96);
			patternLabel.Name = "patternLabel";
			patternLabel.Size = new Size(162, 15);
			patternLabel.TabIndex = 2;
			patternLabel.Text = "Extension or wildcard pattern:";
			// extensionPatternTextBox
			extensionPatternTextBox.Location = new Point(16, 122);
			extensionPatternTextBox.Name = "extensionPatternTextBox";
			extensionPatternTextBox.Size = new Size(520, 23);
			extensionPatternTextBox.TabIndex = 3;
			extensionPatternTextBox.KeyDown += ExtensionPatternTextBox_KeyDown;
			// addPatternButton
			addPatternButton.Location = new Point(552, 120);
			addPatternButton.Name = "addPatternButton";
			addPatternButton.Size = new Size(120, 28);
			addPatternButton.TabIndex = 4;
			addPatternButton.Text = "Add Pattern";
			addPatternButton.UseVisualStyleBackColor = true;
			addPatternButton.Click += AddPatternButton_Click;
			// exclusionsListBox
			exclusionsListBox.FormattingEnabled = true;
			exclusionsListBox.HorizontalScrollbar = true;
			exclusionsListBox.ItemHeight = 15;
			exclusionsListBox.Location = new Point(16, 160);
			exclusionsListBox.Name = "exclusionsListBox";
			exclusionsListBox.SelectionMode = SelectionMode.MultiExtended;
			exclusionsListBox.Size = new Size(680, 274);
			exclusionsListBox.TabIndex = 5;
			// noExclusionsLabel
			noExclusionsLabel.AutoSize = true;
			noExclusionsLabel.BackColor = Color.Transparent;
			noExclusionsLabel.ForeColor = Color.DimGray;
			noExclusionsLabel.Location = new Point(24, 168);
			noExclusionsLabel.Name = "noExclusionsLabel";
			noExclusionsLabel.Size = new Size(149, 15);
			noExclusionsLabel.TabIndex = 6;
			noExclusionsLabel.Text = "No custom exclusions defined.";
			// statusLabel
			statusLabel.ForeColor = Color.DimGray;
			statusLabel.Location = new Point(16, 440);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(680, 40);
			statusLabel.TabIndex = 7;
			// okButton
			okButton.DialogResult = DialogResult.OK;
			okButton.Location = new Point(516, 486);
			okButton.Name = "okButton";
			okButton.Size = new Size(90, 30);
			okButton.TabIndex = 8;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			// cancelButton
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(606, 486);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(90, 30);
			cancelButton.TabIndex = 9;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// ExclusionsManagementForm
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(720, 530);
			Controls.Add(cancelButton);
			Controls.Add(okButton);
			Controls.Add(statusLabel);
			Controls.Add(noExclusionsLabel);
			Controls.Add(exclusionsListBox);
			Controls.Add(addPatternButton);
			Controls.Add(extensionPatternTextBox);
			Controls.Add(patternLabel);
			Controls.Add(buttonPanel);
			Controls.Add(titleLabel);
			MinimumSize = new Size(720, 520);
			Name = "ExclusionsManagementForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Manage Exclusions";
			buttonPanel.ResumeLayout(false);
			buttonPanel.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
