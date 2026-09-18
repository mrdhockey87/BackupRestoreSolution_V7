using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.WinForms.Controls;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class VolumeConfigurationForm : Form
	{
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private readonly List<VolumeInfo> sourceVolumes;
		private readonly List<VolumeResizeInfo> resizeVolumes;
		private readonly long targetTotalSize;
		private readonly int sourceAllocationUnitSize;
		private readonly int targetAllocationUnitSize;

		public VolumeConfigurationForm()
			: this(
			[
				new VolumeInfo { Label = "Windows", Size = 240L * 1024 * 1024 * 1024, UsedSpace = 120L * 1024 * 1024 * 1024, FileSystem = "NTFS", AllocationUnitSize = 4096 },
				new VolumeInfo { Label = "Data", Size = 180L * 1024 * 1024 * 1024, UsedSpace = 80L * 1024 * 1024 * 1024, FileSystem = "NTFS", AllocationUnitSize = 4096 }
			],
			512L * 1024 * 1024 * 1024,
			4096,
			4096)
		{
		}

		public VolumeConfigurationForm(List<VolumeInfo> sourceVols, long targetSize, int sourceAUS, int targetAUS)
		{
			ArgumentNullException.ThrowIfNull(sourceVols);
			if (targetSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(targetSize));
			}

			sourceVolumes = sourceVols.Select(CloneVolumeInfo).ToList();
			targetTotalSize = targetSize;
			sourceAllocationUnitSize = sourceAUS;
			targetAllocationUnitSize = targetAUS;
			resizeVolumes = [];
			InitializeComponent();

			if (IsInDesignMode)
			{
				InitializeLayout();
			}
			else
			{
				Load += VolumeConfigurationForm_Load;
			}
		}

		private void VolumeConfigurationForm_Load(object? sender, EventArgs e)
		{
			InitializeLayout();
		}

		private void AutoFitButton_Click(object? sender, EventArgs e)
		{
			AutoFitVolumes();
		}

		private void ResetButton_Click(object? sender, EventArgs e)
		{
			ResetVolumes();
		}

		private void AcceptButton_Click(object? sender, EventArgs e)
		{
			AcceptConfiguration();
		}

		public VolumeInfo[]? FinalConfiguration { get; private set; }

		private void InitializeLayout()
		{
			VolumeSizingLayout layout = VolumeSizingLayoutCalculator.CalculateDefaultLayout(sourceVolumes, targetTotalSize, sourceAllocationUnitSize, targetAllocationUnitSize);
			resizeVolumes.Clear();

			for (int i = 0; i < sourceVolumes.Count; i++)
			{
				VolumeInfo source = sourceVolumes[i];
				resizeVolumes.Add(new VolumeResizeInfo
				{
					Index = i,
					Label = source.Label,
					OriginalSize = source.Size,
					DataSize = VolumeSizingLayoutCalculator.CalculateActualUsedSpaceForLayout(source.UsedSpace, sourceAllocationUnitSize, targetAllocationUnitSize),
					TargetSize = layout.CurrentSizes[i]
				});
			}

			resizeControl.Initialize(resizeVolumes, targetTotalSize);
			RefreshSummary(layout);
		}

		private void RefreshSummary(VolumeSizingLayout layout)
		{
			long sourceTotal = sourceVolumes.Sum(volume => volume.Size);
			long currentTotal = resizeVolumes.Sum(volume => volume.TargetSize);
			long freeSpace = targetTotalSize - currentTotal;
			long totalUsedSpace = sourceVolumes.Sum(volume => VolumeSizingLayoutCalculator.CalculateActualUsedSpaceForLayout(volume.UsedSpace, sourceAllocationUnitSize, targetAllocationUnitSize));
			long requiredSpace = (long)(totalUsedSpace * 1.05);

			sourceDiskInfoLabel.Text = $"Source Disk ({sourceVolumes.Count} volume{(sourceVolumes.Count == 1 ? string.Empty : "s")}) - Total: {FormatSize(sourceTotal)}";
			targetDiskInfoLabel.Text = $"Target Disk - Total: {FormatSize(targetTotalSize)}";
			spaceInfoLabel.Text = $"Current layout: {FormatSize(currentTotal)} used, {FormatSize(freeSpace)} free";

			if (requiredSpace > targetTotalSize)
			{
				warningLabel.Text = sourceVolumes.Any(VolumeSizingLayoutCalculator.CanVolumeBeResizedForLayout)
					? $"Source ({FormatSize(sourceTotal)}) is larger than target ({FormatSize(targetTotalSize)}). Resizable volumes were reduced to their minimum data-based sizes. Adjust sizes carefully before accepting."
					: $"Source ({FormatSize(sourceTotal)}) is larger than target ({FormatSize(targetTotalSize)}) and no volumes can be resized. Please choose a larger target disk.";
				warningLabel.Visible = true;
			}
			else
			{
				warningLabel.Visible = false;
				warningLabel.Text = string.Empty;
			}

			(bool isValid, string errorMessage) = resizeControl.ValidateConfiguration();
			statusLabel.Text = isValid ? "Ready. Drag a handle, use Auto Fit, or click Accept." : errorMessage;
			statusLabel.ForeColor = isValid ? Color.DimGray : Color.DarkRed;
			acceptButton.Enabled = isValid || requiredSpace <= targetTotalSize;
		}

		private void AutoFitVolumes()
		{
			resizeControl.AutoFit();
			RefreshSummary(VolumeSizingLayoutCalculator.CalculateDefaultLayout(sourceVolumes, targetTotalSize, sourceAllocationUnitSize, targetAllocationUnitSize));
			statusLabel.Text = "Auto Fit applied.";
			statusLabel.ForeColor = Color.DimGray;
		}

		private void ResetVolumes()
		{
			DialogResult result = MessageBox.Show(this,
				"Reset all volumes to the default calculated layout?",
				"Confirm Reset",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			if (result != DialogResult.Yes)
			{
				return;
			}

			VolumeSizingLayout layout = VolumeSizingLayoutCalculator.CalculateDefaultLayout(sourceVolumes, targetTotalSize, sourceAllocationUnitSize, targetAllocationUnitSize);
			for (int i = 0; i < resizeVolumes.Count; i++)
			{
				resizeVolumes[i].TargetSize = layout.CurrentSizes[i];
			}

			resizeControl.Refresh();
			RefreshSummary(layout);
			statusLabel.Text = "Layout reset to the default configuration.";
			statusLabel.ForeColor = Color.DimGray;
		}

		private void AcceptConfiguration()
		{
			(bool isValid, string errorMessage) = resizeControl.ValidateConfiguration();
			long totalSize = resizeVolumes.Sum(volume => volume.TargetSize);
			if (!isValid)
			{
				MessageBox.Show(this, errorMessage, "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (totalSize > targetTotalSize)
			{
				MessageBox.Show(this,
					$"Total size ({FormatSize(totalSize)}) exceeds target disk capacity ({FormatSize(targetTotalSize)}). Please adjust volume sizes before accepting.",
					"Invalid Configuration",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return;
			}

			FinalConfiguration = resizeVolumes.Select(resizeVolume =>
				{
					VolumeInfo source = sourceVolumes[resizeVolume.Index];
					return new VolumeInfo
					{
						Label = source.Label,
						Size = resizeVolume.TargetSize,
						UsedSpace = source.UsedSpace,
						IsResizable = source.IsResizable,
						IsSystemVolume = source.IsSystemVolume,
						FileSystem = source.FileSystem,
						AllocationUnitSize = source.AllocationUnitSize,
						ImageIndex = source.ImageIndex,
						PartitionNumber = source.PartitionNumber,
						PartitionOffsetBytes = source.PartitionOffsetBytes,
						PartitionLengthBytes = source.PartitionLengthBytes,
						PartitionStyle = source.PartitionStyle,
						PartitionType = source.PartitionType,
						SourceVolumeGuidPath = source.SourceVolumeGuidPath,
						SourceVolumeMountPath = source.SourceVolumeMountPath,
						IsBootVolume = source.IsBootVolume,
						TargetSize = resizeVolume.TargetSize
					};
				})
				.ToArray();

			DialogResult = DialogResult.OK;
			Close();
		}

		private static VolumeInfo CloneVolumeInfo(VolumeInfo volume)
		{
			return new VolumeInfo
			{
				Label = volume.Label,
				Size = volume.Size,
				UsedSpace = volume.UsedSpace,
				IsResizable = volume.IsResizable,
				IsSystemVolume = volume.IsSystemVolume,
				FileSystem = volume.FileSystem,
				AllocationUnitSize = volume.AllocationUnitSize,
				ImageIndex = volume.ImageIndex,
				PartitionNumber = volume.PartitionNumber,
				PartitionOffsetBytes = volume.PartitionOffsetBytes,
				PartitionLengthBytes = volume.PartitionLengthBytes,
				PartitionStyle = volume.PartitionStyle,
				PartitionType = volume.PartitionType,
				SourceVolumeGuidPath = volume.SourceVolumeGuidPath,
				SourceVolumeMountPath = volume.SourceVolumeMountPath,
				IsBootVolume = volume.IsBootVolume,
				TargetSize = volume.TargetSize
			};
		}

		private static string FormatSize(long bytes)
		{
			string[] sizes = ["B", "KB", "MB", "GB", "TB"];
			double length = bytes;
			int order = 0;
			while (length >= 1024 && order < sizes.Length - 1)
			{
				order++;
				length /= 1024;
			}

			return $"{length:0.##} {sizes[order]}";
		}
	}
}
