using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class ServiceManagementForm
	{
		private IContainer components;
		private Label titleLabel;
		private GroupBox statusGroup;
		private Label statusCaptionLabel;
		private Label statusValueLabel;
		private Label installedCaptionLabel;
		private Label installedValueLabel;
		private Label uiVersionCaptionLabel;
		private Label uiVersionValueLabel;
		private Label serviceVersionCaptionLabel;
		private Label serviceVersionValueLabel;
		private Label versionWarningLabel;
		private Button refreshStatusButton;
		private Button abortRetriesButton;
		private GroupBox controlGroup;
		private Button startButton;
		private Button stopButton;
		private Button restartButton;
		private Button installButton;
		private Button uninstallButton;
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
			statusGroup = new GroupBox();
			statusCaptionLabel = new Label();
			statusValueLabel = new Label();
			installedCaptionLabel = new Label();
			installedValueLabel = new Label();
			uiVersionCaptionLabel = new Label();
			uiVersionValueLabel = new Label();
			serviceVersionCaptionLabel = new Label();
			serviceVersionValueLabel = new Label();
			versionWarningLabel = new Label();
			refreshStatusButton = new Button();
			abortRetriesButton = new Button();
			controlGroup = new GroupBox();
			startButton = new Button();
			stopButton = new Button();
			restartButton = new Button();
			installButton = new Button();
			uninstallButton = new Button();
			closeButton = new Button();
			statusGroup.SuspendLayout();
			controlGroup.SuspendLayout();
			SuspendLayout();
			// titleLabel
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			titleLabel.Location = new Point(16, 16);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(147, 15);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "Backup Service Management";
			// statusGroup
			statusGroup.Controls.Add(refreshStatusButton);
			statusGroup.Controls.Add(abortRetriesButton);
			statusGroup.Controls.Add(versionWarningLabel);
			statusGroup.Controls.Add(serviceVersionValueLabel);
			statusGroup.Controls.Add(serviceVersionCaptionLabel);
			statusGroup.Controls.Add(uiVersionValueLabel);
			statusGroup.Controls.Add(uiVersionCaptionLabel);
			statusGroup.Controls.Add(installedValueLabel);
			statusGroup.Controls.Add(installedCaptionLabel);
			statusGroup.Controls.Add(statusValueLabel);
			statusGroup.Controls.Add(statusCaptionLabel);
			statusGroup.Location = new Point(16, 52);
			statusGroup.Name = "statusGroup";
			statusGroup.Size = new Size(580, 190);
			statusGroup.TabIndex = 1;
			statusGroup.TabStop = false;
			statusGroup.Text = "Service Status";
			// statusCaptionLabel
			statusCaptionLabel.AutoSize = true;
			statusCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			statusCaptionLabel.Location = new Point(16, 32);
			statusCaptionLabel.Name = "statusCaptionLabel";
			statusCaptionLabel.Size = new Size(42, 15);
			statusCaptionLabel.TabIndex = 0;
			statusCaptionLabel.Text = "Status:";
			// statusValueLabel
			statusValueLabel.AutoSize = true;
			statusValueLabel.Location = new Point(140, 32);
			statusValueLabel.Name = "statusValueLabel";
			statusValueLabel.Size = new Size(56, 15);
			statusValueLabel.TabIndex = 1;
			statusValueLabel.Text = "Unknown";
			// installedCaptionLabel
			installedCaptionLabel.AutoSize = true;
			installedCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			installedCaptionLabel.Location = new Point(16, 60);
			installedCaptionLabel.Name = "installedCaptionLabel";
			installedCaptionLabel.Size = new Size(57, 15);
			installedCaptionLabel.TabIndex = 2;
			installedCaptionLabel.Text = "Installed:";
			// installedValueLabel
			installedValueLabel.AutoSize = true;
			installedValueLabel.Location = new Point(140, 60);
			installedValueLabel.Name = "installedValueLabel";
			installedValueLabel.Size = new Size(56, 15);
			installedValueLabel.TabIndex = 3;
			installedValueLabel.Text = "Unknown";
			// uiVersionCaptionLabel
			uiVersionCaptionLabel.AutoSize = true;
			uiVersionCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			uiVersionCaptionLabel.Location = new Point(16, 98);
			uiVersionCaptionLabel.Name = "uiVersionCaptionLabel";
			uiVersionCaptionLabel.Size = new Size(65, 15);
			uiVersionCaptionLabel.TabIndex = 4;
			uiVersionCaptionLabel.Text = "UI Version:";
			// uiVersionValueLabel
			uiVersionValueLabel.AutoSize = true;
			uiVersionValueLabel.Location = new Point(140, 98);
			uiVersionValueLabel.Name = "uiVersionValueLabel";
			uiVersionValueLabel.Size = new Size(56, 15);
			uiVersionValueLabel.TabIndex = 5;
			uiVersionValueLabel.Text = "Unknown";
			// serviceVersionCaptionLabel
			serviceVersionCaptionLabel.AutoSize = true;
			serviceVersionCaptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			serviceVersionCaptionLabel.Location = new Point(16, 126);
			serviceVersionCaptionLabel.Name = "serviceVersionCaptionLabel";
			serviceVersionCaptionLabel.Size = new Size(90, 15);
			serviceVersionCaptionLabel.TabIndex = 6;
			serviceVersionCaptionLabel.Text = "Service Version:";
			// serviceVersionValueLabel
			serviceVersionValueLabel.AutoSize = true;
			serviceVersionValueLabel.Location = new Point(140, 126);
			serviceVersionValueLabel.Name = "serviceVersionValueLabel";
			serviceVersionValueLabel.Size = new Size(56, 15);
			serviceVersionValueLabel.TabIndex = 7;
			serviceVersionValueLabel.Text = "Unknown";
			// versionWarningLabel
			versionWarningLabel.AutoSize = true;
			versionWarningLabel.ForeColor = Color.DarkRed;
			versionWarningLabel.Location = new Point(280, 126);
			versionWarningLabel.Name = "versionWarningLabel";
			versionWarningLabel.Size = new Size(0, 15);
			versionWarningLabel.TabIndex = 8;
			versionWarningLabel.Visible = false;
			// refreshStatusButton
			refreshStatusButton.Location = new Point(16, 154);
			refreshStatusButton.Name = "refreshStatusButton";
			refreshStatusButton.Size = new Size(130, 28);
			refreshStatusButton.TabIndex = 9;
			refreshStatusButton.Text = "Refresh Status";
			refreshStatusButton.UseVisualStyleBackColor = true;
			refreshStatusButton.Click += RefreshStatusButton_Click;
			// abortRetriesButton
			abortRetriesButton.Location = new Point(156, 154);
			abortRetriesButton.Name = "abortRetriesButton";
			abortRetriesButton.Size = new Size(150, 28);
			abortRetriesButton.TabIndex = 10;
			abortRetriesButton.Text = "Abort Failed Retries";
			abortRetriesButton.UseVisualStyleBackColor = true;
			abortRetriesButton.Click += AbortRetriesButton_Click;
			// controlGroup
			controlGroup.Controls.Add(uninstallButton);
			controlGroup.Controls.Add(installButton);
			controlGroup.Controls.Add(restartButton);
			controlGroup.Controls.Add(stopButton);
			controlGroup.Controls.Add(startButton);
			controlGroup.Location = new Point(16, 252);
			controlGroup.Name = "controlGroup";
			controlGroup.Size = new Size(580, 140);
			controlGroup.TabIndex = 2;
			controlGroup.TabStop = false;
			controlGroup.Text = "Service Control";
			// startButton
			startButton.Location = new Point(16, 30);
			startButton.Name = "startButton";
			startButton.Size = new Size(120, 30);
			startButton.TabIndex = 0;
			startButton.Text = "Start Service";
			startButton.UseVisualStyleBackColor = true;
			startButton.Click += StartButton_Click;
			// stopButton
			stopButton.Location = new Point(146, 30);
			stopButton.Name = "stopButton";
			stopButton.Size = new Size(120, 30);
			stopButton.TabIndex = 1;
			stopButton.Text = "Stop Service";
			stopButton.UseVisualStyleBackColor = true;
			stopButton.Click += StopButton_Click;
			// restartButton
			restartButton.Location = new Point(276, 30);
			restartButton.Name = "restartButton";
			restartButton.Size = new Size(120, 30);
			restartButton.TabIndex = 2;
			restartButton.Text = "Restart Service";
			restartButton.UseVisualStyleBackColor = true;
			restartButton.Click += RestartButton_Click;
			// installButton
			installButton.Location = new Point(16, 78);
			installButton.Name = "installButton";
			installButton.Size = new Size(170, 30);
			installButton.TabIndex = 3;
			installButton.Text = "Install and Start Service";
			installButton.UseVisualStyleBackColor = true;
			installButton.Click += InstallButton_Click;
			// uninstallButton
			uninstallButton.Location = new Point(196, 78);
			uninstallButton.Name = "uninstallButton";
			uninstallButton.Size = new Size(130, 30);
			uninstallButton.TabIndex = 4;
			uninstallButton.Text = "Uninstall Service";
			uninstallButton.UseVisualStyleBackColor = true;
			uninstallButton.Click += UninstallButton_Click;
			// closeButton
			closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			closeButton.DialogResult = DialogResult.OK;
			closeButton.Location = new Point(496, 410);
			closeButton.Name = "closeButton";
			closeButton.Size = new Size(100, 32);
			closeButton.TabIndex = 3;
			closeButton.Text = "Close";
			closeButton.UseVisualStyleBackColor = true;
			// ServiceManagementForm
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(620, 500);
			Controls.Add(closeButton);
			Controls.Add(controlGroup);
			Controls.Add(statusGroup);
			Controls.Add(titleLabel);
			MinimumSize = new Size(620, 500);
			Name = "ServiceManagementForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Service Management";
			statusGroup.ResumeLayout(false);
			statusGroup.PerformLayout();
			controlGroup.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
