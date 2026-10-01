using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Forms;

using SecureServerBackup.Models;

namespace SecureServerBackup.WinForms
{
	public partial class DiskSelectionForm : Form
	{
		

		public DiskInfo? SelectedDisk { get; private set; }
		private List<int> excludedDiskIndexes = new();

		// Parameterless constructor required by the VS Form Designer.
		public DiskSelectionForm() : this(null)
		{
		}

		public DiskSelectionForm(List<int>? excludeDisks)
		{
			InitializeComponent();
			excludedDiskIndexes = excludeDisks ?? new List<int>();
			Load += DiskSelectionForm_Load;
		}

		private void DiskSelectionForm_Load(object? sender, EventArgs e)
		{
			if (DesignMode) return; // skip hardware queries at design time

			LoadAvailableDisks();
		}

		private void LoadAvailableDisks()
		{
			try
			{
				var disks = new List<DiskInfo>();

				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
				{
					foreach (ManagementObject disk in searcher.Get())
					{
						try
						{
							int diskIndex = Convert.ToInt32(disk["Index"]);

							// Skip excluded disks (source disks)
							if (excludedDiskIndexes.Contains(diskIndex))
								continue;

							long sizeBytes = Convert.ToInt64(disk["Size"]);
							string model = disk["Model"]?.ToString() ?? "Unknown";
							string deviceId = disk["DeviceID"]?.ToString() ?? "";
							string interfaceType = disk["InterfaceType"]?.ToString() ?? "Unknown";

							var volumeLetters = GetVolumeLettersForDisk(diskIndex);

							string sizeStr = FormatSize(sizeBytes);

							string displayName = $"Disk {diskIndex}: {model}";
							if (volumeLetters.Count > 0)
								displayName += $" ({string.Join(", ", volumeLetters)})";
							else
								displayName += " (Unallocated/No Volumes)";

							string details = $"Size: {sizeStr} | Interface: {interfaceType}";
							if (volumeLetters.Count > 0)
								details += $" | Volumes: {string.Join(", ", volumeLetters)}";
							else
								details += " | Status: Unallocated or unformatted";

							disks.Add(new DiskInfo
							{
								DiskIndex = diskIndex,
								DisplayName = displayName,
								Details = details,
								SizeBytes = sizeBytes,
								Model = model,
								DeviceId = deviceId,
								VolumeLetters = volumeLetters
							});
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine($"Error processing disk: {ex.Message}");
						}
					}
				}

				disks = disks.OrderBy(d => d.DiskIndex).ToList();

				lstDisks.DataSource = null;
				lstDisks.DisplayMember = nameof(DiskInfo.DisplayName);
				lstDisks.DataSource = disks;

				if (disks.Count == 0)
				{
					MessageBox.Show(this, "No available target disks found.\n\nAll disks may be in use as source disks.",
						"No Disks Available",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, $"Error loading disks: {ex.Message}",
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private List<string> GetVolumeLettersForDisk(int diskIndex)
		{
			var volumeLetters = new List<string>();

			try
			{
				string? deviceId = null;
				using (var diskSearcher = new ManagementObjectSearcher($"SELECT DeviceID FROM Win32_DiskDrive WHERE Index = {diskIndex}"))
				{
					foreach (ManagementObject disk in diskSearcher.Get())
					{
						deviceId = disk["DeviceID"]?.ToString();
						break;
					}
				}

				if (string.IsNullOrEmpty(deviceId))
				{
					System.Diagnostics.Debug.WriteLine($"[GetVolumeLetters] Could not find DeviceID for disk {diskIndex}");
					return volumeLetters;
				}

				string diskQuery = $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceId}'}} WHERE AssocClass=Win32_DiskDriveToDiskPartition";

				using (var partitionSearcher = new ManagementObjectSearcher(diskQuery))
				{
					foreach (ManagementObject partition in partitionSearcher.Get())
					{
						try
						{
							string? partitionDeviceId = partition["DeviceID"]?.ToString();
							if (string.IsNullOrEmpty(partitionDeviceId))
								continue;

							string logicalQuery = $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionDeviceId}'}} WHERE AssocClass=Win32_LogicalDiskToPartition";

							using (var logicalSearcher = new ManagementObjectSearcher(logicalQuery))
							{
								foreach (ManagementObject logical in logicalSearcher.Get())
								{
									try
									{
										string? driveLetter = logical["DeviceID"]?.ToString();
										if (!string.IsNullOrEmpty(driveLetter))
											volumeLetters.Add(driveLetter);
									}
									catch (Exception ex)
									{
										System.Diagnostics.Debug.WriteLine($"[GetVolumeLetters] Error reading logical disk: {ex.Message}");
									}
								}
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine($"[GetVolumeLetters] Error querying logical disks for partition: {ex.Message}");
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[GetVolumeLetters] Error getting volume letters for disk {diskIndex}: {ex.Message}");
			}

			return volumeLetters;
		}

		private string FormatSize(long bytes)
		{
			string[] sizes = { "B", "KB", "MB", "GB", "TB" };
			double len = bytes;
			int order = 0;
			while (len >= 1024 && order < sizes.Length - 1)
			{
				order++;
				len = len / 1024;
			}
			return $"{len:0.##} {sizes[order]}";
		}

		// Renders each item as a two-line "card": bold DisplayName + gray Details
		private void lstDisks_DrawItem(object? sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;

			var disk = (DiskInfo)lstDisks.Items[e.Index];
			bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

			System.Drawing.Color back = selected
				? System.Drawing.Color.FromArgb(227, 242, 253)
				: System.Drawing.Color.White;
			System.Drawing.Color border = selected
				? System.Drawing.Color.FromArgb(33, 150, 243)
				: System.Drawing.Color.FromArgb(224, 224, 224);

			using var backBrush = new System.Drawing.SolidBrush(back);
			e.Graphics.FillRectangle(backBrush, e.Bounds);

			using var borderPen = new System.Drawing.Pen(border);
			var rect = new System.Drawing.Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
			e.Graphics.DrawRectangle(borderPen, rect);

			using var titleFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			using var detailFont = new System.Drawing.Font("Segoe UI", 8.5F);
			using var titleBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
			using var detailBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);

			var titlePoint = new System.Drawing.PointF(e.Bounds.X + 10, e.Bounds.Y + 6);
			var detailPoint = new System.Drawing.PointF(e.Bounds.X + 10, e.Bounds.Y + 26);

			e.Graphics.DrawString(disk.DisplayName, titleFont, titleBrush, titlePoint);
			e.Graphics.DrawString(disk.Details, detailFont, detailBrush, detailPoint);
		}

		private void lstDisks_SelectedIndexChanged(object? sender, EventArgs e)
		{
			btnSelect.Enabled = lstDisks.SelectedItem != null;
		}

		private void btnSelect_Click(object? sender, EventArgs e)
		{
			if (lstDisks.SelectedItem is DiskInfo disk)
			{
				var result = MessageBox.Show(this,
					$"You have selected:\n\n{disk.DisplayName}\n{disk.Details}\n\n" +
					$"WARNING: All data on this disk will be REPLACED!\n\n" +
					$"Are you sure you want to use this disk as the clone target?",
					"Confirm Target Disk",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning,
					MessageBoxDefaultButton.Button2);

				if (result == DialogResult.Yes)
				{
					SelectedDisk = disk;
					DialogResult = DialogResult.OK;
					Close();
				}
			}
		}

		private void btnCancel_Click(object? sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}