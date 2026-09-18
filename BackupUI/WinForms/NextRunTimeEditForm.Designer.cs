using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	partial class NextRunTimeEditForm
	{
		private IContainer components;
		private Label currentRunLabel;
		private Label allowedRangeLabel;
		private Label noticeLabel;
		private Label yearLabel;
		private ComboBox yearComboBox;
		private Label monthLabel;
		private ComboBox monthComboBox;
		private Label dayLabel;
		private ComboBox dayComboBox;
		private Label periodLabel;
		private ComboBox periodComboBox;
		private Label hourLabel;
		private ComboBox hourComboBox;
		private Label minuteLabel;
		private ComboBox minuteComboBox;
		private Label selectedValueLabel;
		private Button saveButton;
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
			currentRunLabel = new Label();
			allowedRangeLabel = new Label();
			noticeLabel = new Label();
			yearLabel = new Label();
			yearComboBox = new ComboBox();
			monthLabel = new Label();
			monthComboBox = new ComboBox();
			dayLabel = new Label();
			dayComboBox = new ComboBox();
			periodLabel = new Label();
			periodComboBox = new ComboBox();
			hourLabel = new Label();
			hourComboBox = new ComboBox();
			minuteLabel = new Label();
			minuteComboBox = new ComboBox();
			selectedValueLabel = new Label();
			saveButton = new Button();
			cancelButton = new Button();
			SuspendLayout();
			// currentRunLabel
			currentRunLabel.AutoSize = true;
			currentRunLabel.Location = new Point(16, 16);
			currentRunLabel.Name = "currentRunLabel";
			currentRunLabel.Size = new Size(97, 15);
			currentRunLabel.TabIndex = 0;
			currentRunLabel.Text = "Current next run:";
			// allowedRangeLabel
			allowedRangeLabel.AutoSize = true;
			allowedRangeLabel.Location = new Point(16, 42);
			allowedRangeLabel.Name = "allowedRangeLabel";
			allowedRangeLabel.Size = new Size(83, 15);
			allowedRangeLabel.TabIndex = 1;
			allowedRangeLabel.Text = "Allowed range:";
			// noticeLabel
			noticeLabel.Location = new Point(16, 70);
			noticeLabel.Name = "noticeLabel";
			noticeLabel.Size = new Size(480, 42);
			noticeLabel.TabIndex = 2;
			noticeLabel.Text = "This changes only the upcoming next run. The original schedule time and date settings stay unchanged for future runs.";
			// yearLabel
			yearLabel.AutoSize = true;
			yearLabel.Location = new Point(16, 112);
			yearLabel.Name = "yearLabel";
			yearLabel.Size = new Size(29, 15);
			yearLabel.TabIndex = 3;
			yearLabel.Text = "Year";
			// yearComboBox
			yearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			yearComboBox.FormattingEnabled = true;
			yearComboBox.Location = new Point(16, 132);
			yearComboBox.Name = "yearComboBox";
			yearComboBox.Size = new Size(72, 23);
			yearComboBox.TabIndex = 4;
			yearComboBox.SelectedIndexChanged += YearComboBox_SelectedIndexChanged;
			// monthLabel
			monthLabel.AutoSize = true;
			monthLabel.Location = new Point(104, 112);
			monthLabel.Name = "monthLabel";
			monthLabel.Size = new Size(43, 15);
			monthLabel.TabIndex = 5;
			monthLabel.Text = "Month";
			// monthComboBox
			monthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			monthComboBox.FormattingEnabled = true;
			monthComboBox.Location = new Point(104, 132);
			monthComboBox.Name = "monthComboBox";
			monthComboBox.Size = new Size(72, 23);
			monthComboBox.TabIndex = 6;
			monthComboBox.SelectedIndexChanged += MonthComboBox_SelectedIndexChanged;
			// dayLabel
			dayLabel.AutoSize = true;
			dayLabel.Location = new Point(192, 112);
			dayLabel.Name = "dayLabel";
			dayLabel.Size = new Size(27, 15);
			dayLabel.TabIndex = 7;
			dayLabel.Text = "Day";
			// dayComboBox
			dayComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			dayComboBox.FormattingEnabled = true;
			dayComboBox.Location = new Point(192, 132);
			dayComboBox.Name = "dayComboBox";
			dayComboBox.Size = new Size(72, 23);
			dayComboBox.TabIndex = 8;
			dayComboBox.SelectedIndexChanged += DayComboBox_SelectedIndexChanged;
			// periodLabel
			periodLabel.AutoSize = true;
			periodLabel.Location = new Point(280, 112);
			periodLabel.Name = "periodLabel";
			periodLabel.Size = new Size(42, 15);
			periodLabel.TabIndex = 9;
			periodLabel.Text = "AM/PM";
			// periodComboBox
			periodComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			periodComboBox.FormattingEnabled = true;
			periodComboBox.Location = new Point(280, 132);
			periodComboBox.Name = "periodComboBox";
			periodComboBox.Size = new Size(72, 23);
			periodComboBox.TabIndex = 10;
			periodComboBox.SelectedIndexChanged += PeriodComboBox_SelectedIndexChanged;
			// hourLabel
			hourLabel.AutoSize = true;
			hourLabel.Location = new Point(368, 112);
			hourLabel.Name = "hourLabel";
			hourLabel.Size = new Size(33, 15);
			hourLabel.TabIndex = 11;
			hourLabel.Text = "Hour";
			// hourComboBox
			hourComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			hourComboBox.FormattingEnabled = true;
			hourComboBox.Location = new Point(368, 132);
			hourComboBox.Name = "hourComboBox";
			hourComboBox.Size = new Size(72, 23);
			hourComboBox.TabIndex = 12;
			hourComboBox.SelectedIndexChanged += HourComboBox_SelectedIndexChanged;
			// minuteLabel
			minuteLabel.AutoSize = true;
			minuteLabel.Location = new Point(456, 112);
			minuteLabel.Name = "minuteLabel";
			minuteLabel.Size = new Size(45, 15);
			minuteLabel.TabIndex = 13;
			minuteLabel.Text = "Minute";
			// minuteComboBox
			minuteComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			minuteComboBox.FormattingEnabled = true;
			minuteComboBox.Location = new Point(456, 132);
			minuteComboBox.Name = "minuteComboBox";
			minuteComboBox.Size = new Size(48, 23);
			minuteComboBox.TabIndex = 14;
			minuteComboBox.SelectedIndexChanged += MinuteComboBox_SelectedIndexChanged;
			// selectedValueLabel
			selectedValueLabel.AutoSize = true;
			selectedValueLabel.Location = new Point(16, 184);
			selectedValueLabel.Name = "selectedValueLabel";
			selectedValueLabel.Size = new Size(105, 15);
			selectedValueLabel.TabIndex = 15;
			selectedValueLabel.Text = "Selected next run:";
			// saveButton
			saveButton.Location = new Point(324, 264);
			saveButton.Name = "saveButton";
			saveButton.Size = new Size(90, 30);
			saveButton.TabIndex = 16;
			saveButton.Text = "Save";
			saveButton.UseVisualStyleBackColor = true;
			saveButton.Click += SaveButton_Click;
			// cancelButton
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(424, 264);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(90, 30);
			cancelButton.TabIndex = 17;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// NextRunTimeEditForm
			AcceptButton = saveButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			CancelButton = cancelButton;
			ClientSize = new Size(520, 320);
			Controls.Add(cancelButton);
			Controls.Add(saveButton);
			Controls.Add(selectedValueLabel);
			Controls.Add(minuteComboBox);
			Controls.Add(minuteLabel);
			Controls.Add(hourComboBox);
			Controls.Add(hourLabel);
			Controls.Add(periodComboBox);
			Controls.Add(periodLabel);
			Controls.Add(dayComboBox);
			Controls.Add(dayLabel);
			Controls.Add(monthComboBox);
			Controls.Add(monthLabel);
			Controls.Add(yearComboBox);
			Controls.Add(yearLabel);
			Controls.Add(noticeLabel);
			Controls.Add(allowedRangeLabel);
			Controls.Add(currentRunLabel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "NextRunTimeEditForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Edit Next Run";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
