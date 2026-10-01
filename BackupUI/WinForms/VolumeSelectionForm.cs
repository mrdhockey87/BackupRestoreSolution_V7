using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
	public partial class VolumeSelectionForm : Form
	{
		public class VolumeInfo
		{
			public string VolumePath { get; set; } = string.Empty;
			public string DisplayName { get; set; } = string.Empty;
			public string FileSystem { get; set; } = string.Empty;
			public bool IsBootVolume { get; set; }
			/// <summary>True for no-drive-letter system partitions (EFI, Recovery, System Reserved).</summary>
			public bool IsHiddenPartition { get; set; }
		}

		public VolumeInfo? SelectedVolume { get; private set; }

		private readonly HashSet<string> _excludedVolumes = new(StringComparer.OrdinalIgnoreCase);

		// Parameterless constructor is handy for the Designer; the Designer
		// itself only needs InitializeComponent() to be callable.
		public VolumeSelectionForm() : this(null)
		{
		}

		public VolumeSelectionForm(IEnumerable<string>? excludedVolumes)
		{
			InitializeComponent();

			if (excludedVolumes != null)
			{
				foreach (var volume in excludedVolumes.Where(v => !string.IsNullOrWhiteSpace(v)))
				{
					_excludedVolumes.Add(volume.TrimEnd('\\'));
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Don't run WMI queries while being displayed in the Visual Studio Designer.
			if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				return;
			}

			LoadVolumes(showHidden: false);
		}

		private void ShowHiddenPartitions_Click(object? sender, EventArgs e)
		{
			LoadVolumes(showHidden: chkShowHiddenPartitions.Checked);
		}

		private void LoadVolumes(bool showHidden)
		{
			try
			{
				var volumes = new List<VolumeInfo>();
				using var searcher = new ManagementObjectSearcher(
					"SELECT DeviceID, FileSystem, VolumeName, DriveLetter, BootVolume FROM Win32_Volume WHERE DriveType = 3");

				foreach (ManagementObject volume in searcher.Get())
				{
					string driveLetter = volume["DriveLetter"]?.ToString() ?? string.Empty;
					string deviceId = volume["DeviceID"]?.ToString() ?? string.Empty;
					string normalized = !string.IsNullOrWhiteSpace(driveLetter)
						? driveLetter.TrimEnd('\\')
						: deviceId.TrimEnd('\\');

					if (string.IsNullOrWhiteSpace(normalized) || _excludedVolumes.Contains(normalized))
					{
						continue;
					}

					bool isBootVolume = false;
					if (bool.TryParse(volume["BootVolume"]?.ToString(), out var boot))
					{
						isBootVolume = boot;
					}

					if (isBootVolume)
					{
						continue;
					}

					bool isHiddenPartition = string.IsNullOrWhiteSpace(driveLetter);

					if (isHiddenPartition && !showHidden)
					{
						continue;
					}

					string label = volume["VolumeName"]?.ToString() ?? "Unnamed Volume";
					string fileSystem = volume["FileSystem"]?.ToString() ?? "Unknown";
					string display = !string.IsNullOrWhiteSpace(driveLetter)
						? $"{driveLetter} - {label} ({fileSystem})"
						: $"(No Letter) {label} ({fileSystem})";

					volumes.Add(new VolumeInfo
					{
						VolumePath = !string.IsNullOrWhiteSpace(driveLetter) ? driveLetter : deviceId,
						DisplayName = display,
						FileSystem = fileSystem,
						IsBootVolume = isBootVolume,
						IsHiddenPartition = isHiddenPartition
					});
				}

				lstVolumes.DataSource = volumes.OrderBy(v => v.DisplayName).ToList();
				lstVolumes.DisplayMember = nameof(VolumeInfo.DisplayName);

				// WinForms auto-selects the first item when a DataSource is bound; WPF did not.
				lstVolumes.ClearSelected();
				lstVolumes.SelectedIndex = -1;
				btnSelect.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Error loading volumes: {ex.Message}", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void VolumeList_SelectedIndexChanged(object? sender, EventArgs e)
		{
			btnSelect.Enabled = lstVolumes.SelectedItem is VolumeInfo;
		}

		private void VolumeList_DoubleClick(object? sender, EventArgs e)
		{
			// Optional convenience: double-click acts like pressing Select.
			if (lstVolumes.SelectedItem is VolumeInfo && btnSelect.Enabled)
			{
				btnSelect.PerformClick();
			}
		}

		private void Select_Click(object? sender, EventArgs e)
		{
			// btnSelect.DialogResult = OK closes the form automatically after this handler runs.
			SelectedVolume = lstVolumes.SelectedItem as VolumeInfo;
		}
	}
}
