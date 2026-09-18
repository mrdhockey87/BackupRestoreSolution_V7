using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class RestoreImageSelectionForm
	{
		private IContainer components;
		private TableLayoutPanel rootLayout;
		private Label summaryLabel;
		private FlowLayoutPanel modePanel;
		private Label restoreTargetTypeLabel;
		private ComboBox restoreModeComboBox;
		private CheckBox showHiddenCheckBox;
		private Label modeHelpLabel;
		private Panel contentHostPanel;
		private Panel targetListPanel;
		private FlowLayoutPanel targetToolbar;
		private Button refreshTargetsButton;
		private Label selectedTargetLabel;
		private ListView targetListView;
		private ColumnHeader targetTypeColumn;
		private ColumnHeader targetNameColumn;
		private ColumnHeader targetPathColumn;
		private ColumnHeader targetSizeColumn;
		private ColumnHeader targetNotesColumn;
		private Panel hyperVVmPanel;
		private TableLayoutPanel hyperVVmLayout;
		private Label hyperVVmActionLabel;
		private ComboBox hyperVVmActionComboBox;
		private Label hyperVVmReplaceLabel;
		private ComboBox hyperVVmReplaceComboBox;
		private Label hyperVVmNameLabel;
		private TextBox hyperVVmNameTextBox;
		private Label hyperVRestoreDirectoryLabel;
		private TextBox hyperVRestoreDirectoryTextBox;
		private Button browseHyperVRestoreDirectoryButton;
		private CheckBox startHyperVVmCheckBox;
		private Panel hyperVVirtualDiskPanel;
		private TableLayoutPanel hyperVVirtualDiskLayout;
		private Label hyperVVirtualDiskPathLabel;
		private TextBox hyperVVirtualDiskPathTextBox;
		private Button browseHyperVVirtualDiskPathButton;
		private Label hyperVDiskAttachModeLabel;
		private ComboBox hyperVDiskAttachModeComboBox;
		private Label existingHyperVVmLabel;
		private ComboBox existingHyperVVmComboBox;
		private Label newHyperVVmNameLabel;
		private TextBox newHyperVVmNameTextBox;
		private Label newHyperVVmPathLabel;
		private TextBox newHyperVVmPathTextBox;
		private Button browseNewHyperVVmPathButton;
		private Label newHyperVGenerationLabel;
		private ComboBox newHyperVGenerationComboBox;
		private CheckBox startCreatedHyperVVmCheckBox;
		private FlowLayoutPanel buttonsPanel;
		private Button cancelButton;
		private Button startRestoreButton;

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
			rootLayout = new TableLayoutPanel();
			summaryLabel = new Label();
			modePanel = new FlowLayoutPanel();
			restoreTargetTypeLabel = new Label();
			restoreModeComboBox = new ComboBox();
			showHiddenCheckBox = new CheckBox();
			modeHelpLabel = new Label();
			contentHostPanel = new Panel();
			targetListPanel = new Panel();
			targetListView = new ListView();
			targetTypeColumn = new ColumnHeader();
			targetNameColumn = new ColumnHeader();
			targetPathColumn = new ColumnHeader();
			targetSizeColumn = new ColumnHeader();
			targetNotesColumn = new ColumnHeader();
			targetToolbar = new FlowLayoutPanel();
			refreshTargetsButton = new Button();
			selectedTargetLabel = new Label();
			hyperVVmPanel = new Panel();
			hyperVVmLayout = new TableLayoutPanel();
			hyperVVmActionLabel = new Label();
			hyperVVmActionComboBox = new ComboBox();
			hyperVVmReplaceLabel = new Label();
			hyperVVmReplaceComboBox = new ComboBox();
			hyperVVmNameLabel = new Label();
			hyperVVmNameTextBox = new TextBox();
			hyperVRestoreDirectoryLabel = new Label();
			hyperVRestoreDirectoryTextBox = new TextBox();
			browseHyperVRestoreDirectoryButton = new Button();
			startHyperVVmCheckBox = new CheckBox();
			hyperVVirtualDiskPanel = new Panel();
			hyperVVirtualDiskLayout = new TableLayoutPanel();
			hyperVVirtualDiskPathLabel = new Label();
			hyperVVirtualDiskPathTextBox = new TextBox();
			browseHyperVVirtualDiskPathButton = new Button();
			hyperVDiskAttachModeLabel = new Label();
			hyperVDiskAttachModeComboBox = new ComboBox();
			existingHyperVVmLabel = new Label();
			existingHyperVVmComboBox = new ComboBox();
			newHyperVVmNameLabel = new Label();
			newHyperVVmNameTextBox = new TextBox();
			newHyperVVmPathLabel = new Label();
			newHyperVVmPathTextBox = new TextBox();
			browseNewHyperVVmPathButton = new Button();
			newHyperVGenerationLabel = new Label();
			newHyperVGenerationComboBox = new ComboBox();
			startCreatedHyperVVmCheckBox = new CheckBox();
			buttonsPanel = new FlowLayoutPanel();
			cancelButton = new Button();
			startRestoreButton = new Button();
			rootLayout.SuspendLayout();
			modePanel.SuspendLayout();
			contentHostPanel.SuspendLayout();
			targetListPanel.SuspendLayout();
			targetToolbar.SuspendLayout();
			hyperVVmPanel.SuspendLayout();
			hyperVVmLayout.SuspendLayout();
			hyperVVirtualDiskPanel.SuspendLayout();
			hyperVVirtualDiskLayout.SuspendLayout();
			buttonsPanel.SuspendLayout();
			SuspendLayout();
			// rootLayout
			rootLayout.ColumnCount = 1;
			rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			rootLayout.Controls.Add(summaryLabel, 0, 0);
			rootLayout.Controls.Add(modePanel, 0, 1);
			rootLayout.Controls.Add(modeHelpLabel, 0, 2);
			rootLayout.Controls.Add(contentHostPanel, 0, 3);
			rootLayout.Controls.Add(buttonsPanel, 0, 4);
			rootLayout.Dock = DockStyle.Fill;
			rootLayout.Location = new Point(0, 0);
			rootLayout.Name = "rootLayout";
			rootLayout.Padding = new Padding(12);
			rootLayout.RowCount = 5;
			rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			rootLayout.Size = new Size(1040, 760);
			// summaryLabel
			summaryLabel.Dock = DockStyle.Top;
			summaryLabel.Location = new Point(15, 12);
			summaryLabel.Name = "summaryLabel";
			summaryLabel.Size = new Size(1010, 64);
			// modePanel
			modePanel.AutoSize = true;
			modePanel.Controls.Add(restoreTargetTypeLabel);
			modePanel.Controls.Add(restoreModeComboBox);
			modePanel.Controls.Add(showHiddenCheckBox);
			modePanel.Dock = DockStyle.Fill;
			modePanel.FlowDirection = FlowDirection.LeftToRight;
			modePanel.Location = new Point(15, 80);
			modePanel.Margin = new Padding(3, 4, 3, 4);
			modePanel.Name = "modePanel";
			modePanel.Size = new Size(1010, 29);
			modePanel.WrapContents = false;
			// restoreTargetTypeLabel
			restoreTargetTypeLabel.AutoSize = true;
			restoreTargetTypeLabel.Margin = new Padding(0, 8, 8, 0);
			restoreTargetTypeLabel.Text = "Restore Target Type:";
			// restoreModeComboBox
			restoreModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			restoreModeComboBox.FormattingEnabled = true;
			restoreModeComboBox.Name = "restoreModeComboBox";
			restoreModeComboBox.Size = new Size(240, 23);
			restoreModeComboBox.SelectedIndexChanged += RestoreModeComboBox_SelectedIndexChanged;
			// showHiddenCheckBox
			showHiddenCheckBox.AutoSize = true;
			showHiddenCheckBox.Margin = new Padding(16, 8, 0, 0);
			showHiddenCheckBox.Name = "showHiddenCheckBox";
			showHiddenCheckBox.Size = new Size(143, 19);
			showHiddenCheckBox.Text = "Show hidden partitions";
			showHiddenCheckBox.UseVisualStyleBackColor = true;
			showHiddenCheckBox.CheckedChanged += ShowHiddenCheckBox_CheckedChanged;
			// modeHelpLabel
			modeHelpLabel.Dock = DockStyle.Top;
			modeHelpLabel.ForeColor = Color.DimGray;
			modeHelpLabel.Location = new Point(15, 116);
			modeHelpLabel.Name = "modeHelpLabel";
			modeHelpLabel.Size = new Size(1010, 40);
			// contentHostPanel
			contentHostPanel.Controls.Add(targetListPanel);
			contentHostPanel.Controls.Add(hyperVVmPanel);
			contentHostPanel.Controls.Add(hyperVVirtualDiskPanel);
			contentHostPanel.Dock = DockStyle.Fill;
			contentHostPanel.Location = new Point(15, 159);
			contentHostPanel.Name = "contentHostPanel";
			contentHostPanel.Size = new Size(1010, 548);
			// targetListPanel
			targetListPanel.Controls.Add(targetListView);
			targetListPanel.Controls.Add(targetToolbar);
			targetListPanel.Dock = DockStyle.Fill;
			targetListPanel.Location = new Point(0, 0);
			targetListPanel.Name = "targetListPanel";
			targetListPanel.Size = new Size(1010, 548);
			// targetListView
			targetListView.Columns.AddRange(new ColumnHeader[] { targetTypeColumn, targetNameColumn, targetPathColumn, targetSizeColumn, targetNotesColumn });
			targetListView.Dock = DockStyle.Fill;
			targetListView.FullRowSelect = true;
			targetListView.GridLines = true;
			targetListView.HideSelection = false;
			targetListView.Location = new Point(0, 31);
			targetListView.MultiSelect = false;
			targetListView.Name = "targetListView";
			targetListView.Size = new Size(1010, 517);
			targetListView.UseCompatibleStateImageBehavior = false;
			targetListView.View = View.Details;
			targetListView.SelectedIndexChanged += TargetListView_SelectedIndexChanged;
			targetListView.DoubleClick += TargetListView_DoubleClick;
			// targetTypeColumn
			targetTypeColumn.Text = "Type";
			targetTypeColumn.Width = 120;
			// targetNameColumn
			targetNameColumn.Text = "Name";
			targetNameColumn.Width = 320;
			// targetPathColumn
			targetPathColumn.Text = "Path";
			targetPathColumn.Width = 300;
			// targetSizeColumn
			targetSizeColumn.Text = "Size";
			targetSizeColumn.Width = 120;
			// targetNotesColumn
			targetNotesColumn.Text = "Notes";
			targetNotesColumn.Width = 120;
			// targetToolbar
			targetToolbar.AutoSize = true;
			targetToolbar.Controls.Add(refreshTargetsButton);
			targetToolbar.Controls.Add(selectedTargetLabel);
			targetToolbar.Dock = DockStyle.Top;
			targetToolbar.FlowDirection = FlowDirection.LeftToRight;
			targetToolbar.Location = new Point(0, 0);
			targetToolbar.Name = "targetToolbar";
			targetToolbar.Size = new Size(1010, 31);
			targetToolbar.WrapContents = false;
			// refreshTargetsButton
			refreshTargetsButton.AutoSize = true;
			refreshTargetsButton.Name = "refreshTargetsButton";
			refreshTargetsButton.Size = new Size(97, 25);
			refreshTargetsButton.Text = "Refresh Targets";
			refreshTargetsButton.UseVisualStyleBackColor = true;
			refreshTargetsButton.Click += RefreshTargetsButton_Click;
			// selectedTargetLabel
			selectedTargetLabel.AutoSize = true;
			selectedTargetLabel.Margin = new Padding(12, 8, 0, 0);
			selectedTargetLabel.Name = "selectedTargetLabel";
			selectedTargetLabel.Size = new Size(106, 15);
			selectedTargetLabel.Text = "No target selected";
			// hyperVVmPanel
			hyperVVmPanel.Controls.Add(hyperVVmLayout);
			hyperVVmPanel.Dock = DockStyle.Fill;
			hyperVVmPanel.Location = new Point(0, 0);
			hyperVVmPanel.Name = "hyperVVmPanel";
			hyperVVmPanel.Size = new Size(1010, 548);
			hyperVVmPanel.Visible = false;
			// hyperVVmLayout
			hyperVVmLayout.AutoSize = true;
			hyperVVmLayout.ColumnCount = 3;
			hyperVVmLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
			hyperVVmLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			hyperVVmLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
			hyperVVmLayout.Controls.Add(hyperVVmActionLabel, 0, 0);
			hyperVVmLayout.Controls.Add(hyperVVmActionComboBox, 1, 0);
			hyperVVmLayout.Controls.Add(hyperVVmReplaceLabel, 0, 1);
			hyperVVmLayout.Controls.Add(hyperVVmReplaceComboBox, 1, 1);
			hyperVVmLayout.Controls.Add(hyperVVmNameLabel, 0, 2);
			hyperVVmLayout.Controls.Add(hyperVVmNameTextBox, 1, 2);
			hyperVVmLayout.Controls.Add(hyperVRestoreDirectoryLabel, 0, 3);
			hyperVVmLayout.Controls.Add(hyperVRestoreDirectoryTextBox, 1, 3);
			hyperVVmLayout.Controls.Add(browseHyperVRestoreDirectoryButton, 2, 3);
			hyperVVmLayout.Controls.Add(startHyperVVmCheckBox, 1, 4);
			hyperVVmLayout.Dock = DockStyle.Top;
			hyperVVmLayout.Location = new Point(0, 0);
			hyperVVmLayout.Name = "hyperVVmLayout";
			hyperVVmLayout.Padding = new Padding(0, 8, 0, 0);
			hyperVVmLayout.RowCount = 5;
			hyperVVmLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVmLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVmLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVmLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVmLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			// hyperVVmActionLabel
			hyperVVmActionLabel.AutoSize = true;
			hyperVVmActionLabel.Margin = new Padding(0, 8, 8, 8);
			hyperVVmActionLabel.Text = "Hyper-V VM Action:";
			// hyperVVmActionComboBox
			hyperVVmActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hyperVVmActionComboBox.FormattingEnabled = true;
			hyperVVmActionComboBox.Items.AddRange(new object[] { "Restore to empty directory", "Replace existing non-running VM" });
			hyperVVmActionComboBox.Name = "hyperVVmActionComboBox";
			hyperVVmActionComboBox.SelectedIndex = 0;
			hyperVVmActionComboBox.Size = new Size(260, 23);
			hyperVVmActionComboBox.SelectedIndexChanged += HyperVVmActionComboBox_SelectedIndexChanged;
			// hyperVVmReplaceLabel
			hyperVVmReplaceLabel.AutoSize = true;
			hyperVVmReplaceLabel.Margin = new Padding(0, 8, 8, 8);
			hyperVVmReplaceLabel.Text = "VM to Replace:";
			// hyperVVmReplaceComboBox
			hyperVVmReplaceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hyperVVmReplaceComboBox.FormattingEnabled = true;
			hyperVVmReplaceComboBox.Name = "hyperVVmReplaceComboBox";
			hyperVVmReplaceComboBox.Size = new Size(320, 23);
			// hyperVVmNameLabel
			hyperVVmNameLabel.AutoSize = true;
			hyperVVmNameLabel.Margin = new Padding(0, 8, 8, 8);
			hyperVVmNameLabel.Text = "Restored VM Name:";
			// hyperVVmNameTextBox
			hyperVVmNameTextBox.Name = "hyperVVmNameTextBox";
			hyperVVmNameTextBox.Size = new Size(320, 23);
			// hyperVRestoreDirectoryLabel
			hyperVRestoreDirectoryLabel.AutoSize = true;
			hyperVRestoreDirectoryLabel.Margin = new Padding(0, 8, 8, 8);
			hyperVRestoreDirectoryLabel.Text = "Restore Directory:";
			// hyperVRestoreDirectoryTextBox
			hyperVRestoreDirectoryTextBox.Name = "hyperVRestoreDirectoryTextBox";
			hyperVRestoreDirectoryTextBox.Size = new Size(520, 23);
			// browseHyperVRestoreDirectoryButton
			browseHyperVRestoreDirectoryButton.AutoSize = true;
			browseHyperVRestoreDirectoryButton.Name = "browseHyperVRestoreDirectoryButton";
			browseHyperVRestoreDirectoryButton.Size = new Size(69, 25);
			browseHyperVRestoreDirectoryButton.Text = "Browse...";
			browseHyperVRestoreDirectoryButton.UseVisualStyleBackColor = true;
			browseHyperVRestoreDirectoryButton.Click += BrowseHyperVRestoreDirectoryButton_Click;
			// startHyperVVmCheckBox
			startHyperVVmCheckBox.AutoSize = true;
			hyperVVmLayout.SetColumnSpan(startHyperVVmCheckBox, 2);
			startHyperVVmCheckBox.Name = "startHyperVVmCheckBox";
			startHyperVVmCheckBox.Size = new Size(131, 19);
			startHyperVVmCheckBox.Text = "Start VM after restore";
			startHyperVVmCheckBox.UseVisualStyleBackColor = true;
			// hyperVVirtualDiskPanel
			hyperVVirtualDiskPanel.Controls.Add(hyperVVirtualDiskLayout);
			hyperVVirtualDiskPanel.Dock = DockStyle.Fill;
			hyperVVirtualDiskPanel.Location = new Point(0, 0);
			hyperVVirtualDiskPanel.Name = "hyperVVirtualDiskPanel";
			hyperVVirtualDiskPanel.Size = new Size(1010, 548);
			hyperVVirtualDiskPanel.Visible = false;
			// hyperVVirtualDiskLayout
			hyperVVirtualDiskLayout.AutoSize = true;
			hyperVVirtualDiskLayout.ColumnCount = 3;
			hyperVVirtualDiskLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
			hyperVVirtualDiskLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			hyperVVirtualDiskLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
			hyperVVirtualDiskLayout.Controls.Add(hyperVVirtualDiskPathLabel, 0, 0);
			hyperVVirtualDiskLayout.Controls.Add(hyperVVirtualDiskPathTextBox, 1, 0);
			hyperVVirtualDiskLayout.Controls.Add(browseHyperVVirtualDiskPathButton, 2, 0);
			hyperVVirtualDiskLayout.Controls.Add(hyperVDiskAttachModeLabel, 0, 1);
			hyperVVirtualDiskLayout.Controls.Add(hyperVDiskAttachModeComboBox, 1, 1);
			hyperVVirtualDiskLayout.Controls.Add(existingHyperVVmLabel, 0, 2);
			hyperVVirtualDiskLayout.Controls.Add(existingHyperVVmComboBox, 1, 2);
			hyperVVirtualDiskLayout.Controls.Add(newHyperVVmNameLabel, 0, 3);
			hyperVVirtualDiskLayout.Controls.Add(newHyperVVmNameTextBox, 1, 3);
			hyperVVirtualDiskLayout.Controls.Add(newHyperVVmPathLabel, 0, 4);
			hyperVVirtualDiskLayout.Controls.Add(newHyperVVmPathTextBox, 1, 4);
			hyperVVirtualDiskLayout.Controls.Add(browseNewHyperVVmPathButton, 2, 4);
			hyperVVirtualDiskLayout.Controls.Add(newHyperVGenerationLabel, 0, 5);
			hyperVVirtualDiskLayout.Controls.Add(newHyperVGenerationComboBox, 1, 5);
			hyperVVirtualDiskLayout.Controls.Add(startCreatedHyperVVmCheckBox, 1, 6);
			hyperVVirtualDiskLayout.Dock = DockStyle.Top;
			hyperVVirtualDiskLayout.Location = new Point(0, 0);
			hyperVVirtualDiskLayout.Name = "hyperVVirtualDiskLayout";
			hyperVVirtualDiskLayout.Padding = new Padding(0, 8, 0, 0);
			hyperVVirtualDiskLayout.RowCount = 7;
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			hyperVVirtualDiskLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			// hyperVVirtualDiskPathLabel
			hyperVVirtualDiskPathLabel.AutoSize = true;
			hyperVVirtualDiskPathLabel.Margin = new Padding(0, 8, 8, 8);
			hyperVVirtualDiskPathLabel.Text = "Virtual Disk Path:";
			// hyperVVirtualDiskPathTextBox
			hyperVVirtualDiskPathTextBox.Name = "hyperVVirtualDiskPathTextBox";
			hyperVVirtualDiskPathTextBox.Size = new Size(520, 23);
			// browseHyperVVirtualDiskPathButton
			browseHyperVVirtualDiskPathButton.AutoSize = true;
			browseHyperVVirtualDiskPathButton.Name = "browseHyperVVirtualDiskPathButton";
			browseHyperVVirtualDiskPathButton.Size = new Size(69, 25);
			browseHyperVVirtualDiskPathButton.Text = "Browse...";
			browseHyperVVirtualDiskPathButton.UseVisualStyleBackColor = true;
			browseHyperVVirtualDiskPathButton.Click += BrowseHyperVVirtualDiskPathButton_Click;
			// hyperVDiskAttachModeLabel
			hyperVDiskAttachModeLabel.AutoSize = true;
			hyperVDiskAttachModeLabel.Margin = new Padding(0, 8, 8, 8);
			hyperVDiskAttachModeLabel.Text = "After Restore:";
			// hyperVDiskAttachModeComboBox
			hyperVDiskAttachModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hyperVDiskAttachModeComboBox.FormattingEnabled = true;
			hyperVDiskAttachModeComboBox.Items.AddRange(new object[] { "Do not attach", "Attach to existing VM", "Create new VM" });
			hyperVDiskAttachModeComboBox.Name = "hyperVDiskAttachModeComboBox";
			hyperVDiskAttachModeComboBox.SelectedIndex = 0;
			hyperVDiskAttachModeComboBox.Size = new Size(280, 23);
			hyperVDiskAttachModeComboBox.SelectedIndexChanged += HyperVDiskAttachModeComboBox_SelectedIndexChanged;
			// existingHyperVVmLabel
			existingHyperVVmLabel.AutoSize = true;
			existingHyperVVmLabel.Margin = new Padding(0, 8, 8, 8);
			existingHyperVVmLabel.Text = "Existing VM:";
			// existingHyperVVmComboBox
			existingHyperVVmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			existingHyperVVmComboBox.FormattingEnabled = true;
			existingHyperVVmComboBox.Name = "existingHyperVVmComboBox";
			existingHyperVVmComboBox.Size = new Size(320, 23);
			// newHyperVVmNameLabel
			newHyperVVmNameLabel.AutoSize = true;
			newHyperVVmNameLabel.Margin = new Padding(0, 8, 8, 8);
			newHyperVVmNameLabel.Text = "New VM Name:";
			// newHyperVVmNameTextBox
			newHyperVVmNameTextBox.Name = "newHyperVVmNameTextBox";
			newHyperVVmNameTextBox.Size = new Size(320, 23);
			// newHyperVVmPathLabel
			newHyperVVmPathLabel.AutoSize = true;
			newHyperVVmPathLabel.Margin = new Padding(0, 8, 8, 8);
			newHyperVVmPathLabel.Text = "New VM Folder:";
			// newHyperVVmPathTextBox
			newHyperVVmPathTextBox.Name = "newHyperVVmPathTextBox";
			newHyperVVmPathTextBox.Size = new Size(520, 23);
			// browseNewHyperVVmPathButton
			browseNewHyperVVmPathButton.AutoSize = true;
			browseNewHyperVVmPathButton.Name = "browseNewHyperVVmPathButton";
			browseNewHyperVVmPathButton.Size = new Size(69, 25);
			browseNewHyperVVmPathButton.Text = "Browse...";
			browseNewHyperVVmPathButton.UseVisualStyleBackColor = true;
			browseNewHyperVVmPathButton.Click += BrowseNewHyperVVmPathButton_Click;
			// newHyperVGenerationLabel
			newHyperVGenerationLabel.AutoSize = true;
			newHyperVGenerationLabel.Margin = new Padding(0, 8, 8, 8);
			newHyperVGenerationLabel.Text = "VM Generation:";
			// newHyperVGenerationComboBox
			newHyperVGenerationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			newHyperVGenerationComboBox.FormattingEnabled = true;
			newHyperVGenerationComboBox.Name = "newHyperVGenerationComboBox";
			newHyperVGenerationComboBox.Size = new Size(120, 23);
			// startCreatedHyperVVmCheckBox
			startCreatedHyperVVmCheckBox.AutoSize = true;
			hyperVVirtualDiskLayout.SetColumnSpan(startCreatedHyperVVmCheckBox, 2);
			startCreatedHyperVVmCheckBox.Name = "startCreatedHyperVVmCheckBox";
			startCreatedHyperVVmCheckBox.Size = new Size(176, 19);
			startCreatedHyperVVmCheckBox.Text = "Start created VM after restore";
			startCreatedHyperVVmCheckBox.UseVisualStyleBackColor = true;
			// buttonsPanel
			buttonsPanel.AutoSize = true;
			buttonsPanel.Controls.Add(cancelButton);
			buttonsPanel.Controls.Add(startRestoreButton);
			buttonsPanel.Dock = DockStyle.Fill;
			buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
			buttonsPanel.Location = new Point(15, 713);
			buttonsPanel.Name = "buttonsPanel";
			buttonsPanel.Padding = new Padding(0, 8, 0, 0);
			buttonsPanel.Size = new Size(1010, 32);
			buttonsPanel.WrapContents = false;
			// cancelButton
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(96, 32);
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// startRestoreButton
			startRestoreButton.Name = "startRestoreButton";
			startRestoreButton.Size = new Size(120, 32);
			startRestoreButton.Text = "Start Restore";
			startRestoreButton.UseVisualStyleBackColor = true;
			startRestoreButton.Click += StartRestoreButton_Click;
			// RestoreImageSelectionForm
			AcceptButton = startRestoreButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(1040, 760);
			Controls.Add(rootLayout);
			MinimumSize = new Size(980, 680);
			Name = "RestoreImageSelectionForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Restore Backup";
			rootLayout.ResumeLayout(false);
			rootLayout.PerformLayout();
			modePanel.ResumeLayout(false);
			modePanel.PerformLayout();
			contentHostPanel.ResumeLayout(false);
			targetListPanel.ResumeLayout(false);
			targetListPanel.PerformLayout();
			targetToolbar.ResumeLayout(false);
			targetToolbar.PerformLayout();
			hyperVVmPanel.ResumeLayout(false);
			hyperVVmPanel.PerformLayout();
			hyperVVmLayout.ResumeLayout(false);
			hyperVVmLayout.PerformLayout();
			hyperVVirtualDiskPanel.ResumeLayout(false);
			hyperVVirtualDiskPanel.PerformLayout();
			hyperVVirtualDiskLayout.ResumeLayout(false);
			hyperVVirtualDiskLayout.PerformLayout();
			buttonsPanel.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
