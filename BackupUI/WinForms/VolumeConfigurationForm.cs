using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.WinForms.Controls;

namespace SecureServerBackup.WinForms
{
	internal sealed class VolumeConfigurationForm : Form
	{
		private readonly List<VolumeInfo> sourceVolumes;
		private readonly List<VolumeResizeInfo> resizeVolumes;
		private readonly long targetTotalSize;
		private readonly int sourceAllocationUnitSize;
		private readonly int targetAllocationUnitSize;
		private readonly VolumeResizeControl resizeControl;
		private readonly Label sourceDiskInfoLabel;
		private readonly Label targetDiskInfoLabel;
		private readonly Label spaceInfoLabel;
		private readonly Label statusLabel;
		private readonly Label warningLabel;
		private readonly Button acceptButton;

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

			Text = "Configure Volume Sizes";
			StartPosition = FormStartPosition.CenterParent;
			MinimumSize = new Size(900, 620);
			ClientSize = new Size(980, 680);
			BackColor = Color.White;

			var titleLabel = new Label
			{
				Text = "Configure Restore Volume Sizes",
				Font = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(16, 16)
			};

			sourceDiskInfoLabel = new Label
			{
				AutoSize = true,
				Location = new Point(16, 52)
			};

			targetDiskInfoLabel = new Label
			{
				AutoSize = true,
				Location = new Point(16, 76)
			};

			spaceInfoLabel = new Label
			{
				AutoSize = true,
				Location = new Point(16, 100),
				ForeColor = Color.DimGray
			};

			warningLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 128),
				Size = new Size(940, 50),
				ForeColor = Color.DarkOrange,
				Visible = false
			};

			resizeControl = new VolumeResizeControl
			{
				Location = new Point(16, 190),
				Size = new Size(940, 330),
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
			};

			var instructionsLabel = new Label
			{
				Text = "Drag the red handles in the target layout to resize adjacent volumes. Use Auto Fit to proportionally fill the target disk or Reset to restore the default layout.",
				AutoSize = false,
				Location = new Point(16, 530),
				Size = new Size(940, 36),
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
			};

			statusLabel = new Label
			{
				Text = "Ready.",
				AutoSize = false,
				Location = new Point(16, 572),
				Size = new Size(620, 28),
				ForeColor = Color.DimGray,
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
			};

			var autoFitButton = new Button
			{
				Text = "Auto Fit",
				Size = new Size(100, 32),
				Location = new Point(646, 568),
				Anchor = AnchorStyles.Right | AnchorStyles.Bottom
			};
			autoFitButton.Click += (_, _) => AutoFitVolumes();

			var resetButton = new Button
			{
				Text = "Reset",
				Size = new Size(100, 32),
				Location = new Point(756, 568),
				Anchor = AnchorStyles.Right | AnchorStyles.Bottom
			};
			resetButton.Click += (_, _) => ResetVolumes();

			acceptButton = new Button
			{
				Text = "Accept",
				Size = new Size(100, 32),
				Location = new Point(646, 608),
				Anchor = AnchorStyles.Right | AnchorStyles.Bottom
			};
			acceptButton.Click += (_, _) => AcceptConfiguration();

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(100, 32),
				Location = new Point(756, 608),
				Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
				DialogResult = DialogResult.Cancel
			};

			Controls.Add(titleLabel);
			Controls.Add(sourceDiskInfoLabel);
			Controls.Add(targetDiskInfoLabel);
			Controls.Add(spaceInfoLabel);
			Controls.Add(warningLabel);
			Controls.Add(resizeControl);
			Controls.Add(instructionsLabel);
			Controls.Add(statusLabel);
			Controls.Add(autoFitButton);
			Controls.Add(resetButton);
			Controls.Add(acceptButton);
			Controls.Add(cancelButton);

			Load += (_, _) => InitializeLayout();
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
