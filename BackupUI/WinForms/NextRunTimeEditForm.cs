using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class NextRunTimeEditForm : Form
	{
		private readonly DateTime earliestAllowedRun;
		private readonly DateTime latestAllowedRun;
		private readonly List<int> availableYears;
		private bool isUpdatingSelections;

		public NextRunTimeEditForm()
			: this(new BackupJob { Name = "Sample Backup" }, DateTime.Now.AddHours(2), DateTime.Now.AddDays(7))
		{
		}

		public NextRunTimeEditForm(BackupJob job, DateTime currentNextRun, DateTime latestAllowedRun)
		{
			ArgumentNullException.ThrowIfNull(job);

			earliestAllowedRun = DateTime.Now.AddMinutes(1);
			this.latestAllowedRun = latestAllowedRun;
			SelectedNextRun = currentNextRun < earliestAllowedRun ? earliestAllowedRun : currentNextRun;
			availableYears = Enumerable.Range(earliestAllowedRun.Year, latestAllowedRun.Year - earliestAllowedRun.Year + 1).ToList();
			InitializeComponent();
			ApplyRuntimeLabels(currentNextRun, latestAllowedRun);

			LoadDateTimeOptions();
			ApplyDateTimeSelection(SelectedNextRun);
			UpdateSelectedValueText();
		}

		public DateTime SelectedNextRun { get; private set; }

		private void ApplyRuntimeLabels(DateTime currentNextRun, DateTime latestAllowed)
		{
			currentRunLabel.Text = $"Current next run: {currentNextRun:yyyy-MM-dd hh:mm tt}";
			allowedRangeLabel.Text = $"Allowed range: {earliestAllowedRun:yyyy-MM-dd hh:mm tt} to {latestAllowed:yyyy-MM-dd hh:mm tt}";
		}

		private void YearComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			DateTimePartSelectionChanged(yearComboBox);
		}

		private void MonthComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			DateTimePartSelectionChanged(monthComboBox);
		}

		private void DayComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			DateTimePartSelectionChanged(dayComboBox);
		}

		private void PeriodComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			DateTimePartSelectionChanged(periodComboBox);
		}

		private void HourComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			DateTimePartSelectionChanged(hourComboBox);
		}

		private void MinuteComboBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			DateTimePartSelectionChanged(minuteComboBox);
		}

		private void SaveButton_Click(object? sender, EventArgs e)
		{
			SaveSelection();
		}

		private void LoadDateTimeOptions()
		{
			isUpdatingSelections = true;
			yearComboBox.DataSource = availableYears.ToList();
			isUpdatingSelections = false;
		}

		private void ApplyDateTimeSelection(DateTime value)
		{
			isUpdatingSelections = true;
			yearComboBox.SelectedItem = value.Year;
			RefreshMonthOptions(value.Month);
			RefreshDayOptions(value.Day);
			RefreshTimeOptions(value.Hour >= 12 ? "PM" : "AM", value.Hour % 12 == 0 ? 12 : value.Hour % 12, value.Minute.ToString("00"));
			isUpdatingSelections = false;
		}

		private void RefreshMonthOptions(int? preferredMonth = null)
		{
			if (yearComboBox.SelectedItem is not int year)
			{
				return;
			}

			int minMonth = year == earliestAllowedRun.Year ? earliestAllowedRun.Month : 1;
			int maxMonth = year == latestAllowedRun.Year ? latestAllowedRun.Month : 12;
			List<int> months = Enumerable.Range(minMonth, maxMonth - minMonth + 1).ToList();
			monthComboBox.DataSource = months;
			monthComboBox.SelectedItem = preferredMonth.HasValue && months.Contains(preferredMonth.Value) ? preferredMonth.Value : months.FirstOrDefault();
		}

		private void RefreshDayOptions(int? preferredDay = null)
		{
			if (yearComboBox.SelectedItem is not int year || monthComboBox.SelectedItem is not int month)
			{
				return;
			}

			int minDay = year == earliestAllowedRun.Year && month == earliestAllowedRun.Month ? earliestAllowedRun.Day : 1;
			int maxDay = year == latestAllowedRun.Year && month == latestAllowedRun.Month ? latestAllowedRun.Day : DateTime.DaysInMonth(year, month);
			List<int> days = Enumerable.Range(minDay, maxDay - minDay + 1).ToList();
			dayComboBox.DataSource = days;
			dayComboBox.SelectedItem = preferredDay.HasValue && days.Contains(preferredDay.Value) ? preferredDay.Value : days.FirstOrDefault();
		}

		private void RefreshTimeOptions(string? preferredPeriod = null, int? preferredHour = null, string? preferredMinute = null)
		{
			if (yearComboBox.SelectedItem is not int year || monthComboBox.SelectedItem is not int month || dayComboBox.SelectedItem is not int day)
			{
				return;
			}

			List<string> periods = GetAvailablePeriods(year, month, day);
			periodComboBox.DataSource = periods;
			periodComboBox.SelectedItem = !string.IsNullOrWhiteSpace(preferredPeriod) && periods.Contains(preferredPeriod) ? preferredPeriod : periods.FirstOrDefault();
			if (periodComboBox.SelectedItem is not string selectedPeriod)
			{
				hourComboBox.DataSource = null;
				minuteComboBox.DataSource = null;
				return;
			}

			List<int> hours = GetAvailableHours(year, month, day, selectedPeriod);
			hourComboBox.DataSource = hours;
			hourComboBox.SelectedItem = preferredHour.HasValue && hours.Contains(preferredHour.Value) ? preferredHour.Value : hours.FirstOrDefault();
			if (hourComboBox.SelectedItem is not int selectedHour)
			{
				minuteComboBox.DataSource = null;
				return;
			}

			List<string> minutes = GetAvailableMinutes(year, month, day, selectedPeriod, selectedHour);
			minuteComboBox.DataSource = minutes;
			minuteComboBox.SelectedItem = !string.IsNullOrWhiteSpace(preferredMinute) && minutes.Contains(preferredMinute) ? preferredMinute : minutes.FirstOrDefault();
		}

		private List<string> GetAvailablePeriods(int year, int month, int day)
		{
			return GetCandidateTimesForDate(year, month, day)
				.Select(candidate => candidate.Hour >= 12 ? "PM" : "AM")
				.Distinct(StringComparer.Ordinal)
				.OrderBy(static period => period, StringComparer.Ordinal)
				.ToList();
		}

		private List<int> GetAvailableHours(int year, int month, int day, string period)
		{
			return GetCandidateTimesForDate(year, month, day)
				.Where(candidate => string.Equals(candidate.Hour >= 12 ? "PM" : "AM", period, StringComparison.Ordinal))
				.Select(candidate =>
				{
					int hour = candidate.Hour % 12;
					return hour == 0 ? 12 : hour;
				})
				.Distinct()
				.OrderBy(static hour => hour)
				.ToList();
		}

		private List<string> GetAvailableMinutes(int year, int month, int day, string period, int hour12)
		{
			return GetCandidateTimesForDate(year, month, day)
				.Where(candidate => string.Equals(candidate.Hour >= 12 ? "PM" : "AM", period, StringComparison.Ordinal))
				.Where(candidate =>
				{
					int candidateHour = candidate.Hour % 12;
					candidateHour = candidateHour == 0 ? 12 : candidateHour;
					return candidateHour == hour12;
				})
				.Select(candidate => candidate.Minute.ToString("00"))
				.Distinct(StringComparer.Ordinal)
				.OrderBy(static minute => minute, StringComparer.Ordinal)
				.ToList();
		}

		private List<DateTime> GetCandidateTimesForDate(int year, int month, int day)
		{
			DateTime dayStart = new(year, month, day, 0, 0, 0);
			DateTime rangeStart = dayStart == earliestAllowedRun.Date ? earliestAllowedRun : dayStart;
			DateTime rangeEnd = dayStart == latestAllowedRun.Date ? latestAllowedRun : dayStart.AddDays(1).AddMinutes(-1);

			var candidates = new List<DateTime>();
			for (DateTime candidate = rangeStart; candidate <= rangeEnd; candidate = candidate.AddMinutes(1))
			{
				candidates.Add(candidate);
			}

			return candidates;
		}

		private void DateTimePartSelectionChanged(Control sender)
		{
			if (isUpdatingSelections)
			{
				return;
			}

			if (ReferenceEquals(sender, yearComboBox))
			{
				isUpdatingSelections = true;
				RefreshMonthOptions();
				RefreshDayOptions();
				RefreshTimeOptions();
				isUpdatingSelections = false;
			}
			else if (ReferenceEquals(sender, monthComboBox))
			{
				isUpdatingSelections = true;
				RefreshDayOptions();
				RefreshTimeOptions();
				isUpdatingSelections = false;
			}
			else if (ReferenceEquals(sender, dayComboBox))
			{
				isUpdatingSelections = true;
				RefreshTimeOptions();
				isUpdatingSelections = false;
			}
			else if (ReferenceEquals(sender, periodComboBox))
			{
				isUpdatingSelections = true;
				RefreshTimeOptions(periodComboBox.SelectedItem as string);
				isUpdatingSelections = false;
			}
			else if (ReferenceEquals(sender, hourComboBox))
			{
				isUpdatingSelections = true;
				RefreshTimeOptions(periodComboBox.SelectedItem as string, hourComboBox.SelectedItem as int?);
				isUpdatingSelections = false;
			}

			if (TryGetSelectedDateTime(out DateTime selectedValue))
			{
				SelectedNextRun = selectedValue;
				UpdateSelectedValueText();
			}
		}

		private bool TryGetSelectedDateTime(out DateTime selectedValue)
		{
			selectedValue = SelectedNextRun;
			if (yearComboBox.SelectedItem is not int year ||
				monthComboBox.SelectedItem is not int month ||
				dayComboBox.SelectedItem is not int day ||
				hourComboBox.SelectedItem is not int hour ||
				minuteComboBox.SelectedItem is not string minuteText ||
				periodComboBox.SelectedItem is not string period ||
				!int.TryParse(minuteText, out int minute))
			{
				return false;
			}

			int hour24 = hour % 12;
			if (string.Equals(period, "PM", StringComparison.OrdinalIgnoreCase))
			{
				hour24 += 12;
			}

			selectedValue = new DateTime(year, month, day, hour24, minute, 0);
			return true;
		}

		private void UpdateSelectedValueText()
		{
			selectedValueLabel.Text = $"Selected next run: {SelectedNextRun:yyyy-MM-dd hh:mm tt}";
		}

		private void SaveSelection()
		{
			if (!TryGetSelectedDateTime(out DateTime selectedValue))
			{
				CustomDialogService.ShowWarning(this, "Please select a valid date and time.", "Invalid Selection");
				return;
			}

			if (selectedValue < earliestAllowedRun || selectedValue > latestAllowedRun)
			{
				CustomDialogService.ShowWarning(this,
					$"The selected next run must be between {earliestAllowedRun:yyyy-MM-dd hh:mm tt} and {latestAllowedRun:yyyy-MM-dd hh:mm tt}.",
					"Invalid Selection");
				return;
			}

			SelectedNextRun = selectedValue;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
