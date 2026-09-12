using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;

namespace SecureServerBackup.WinForms
{
	internal sealed class RestoreVolumeSelectionForm : Form
	{
		private readonly ListView volumesListView;
		private readonly bool isDiskOrHyperVBackup;
		private readonly List<VolumeInfo> allVolumes;

		public RestoreVolumeSelectionForm(IReadOnlyList<VolumeInfo> volumes, bool isDiskOrHyperVBackup, string? subtitleOverride = null)
		{
			ArgumentNullException.ThrowIfNull(volumes);

			this.isDiskOrHyperVBackup = isDiskOrHyperVBackup;
			allVolumes = volumes.ToList();

			Text = "Select Restore Volume";
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(920, 500);
			MinimumSize = new Size(920, 500);
			BackColor = Color.White;

			var subtitleLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 16),
				Size = new Size(880, 44),
				Text = string.IsNullOrWhiteSpace(subtitleOverride)
					? "Select the volume or full set of volumes to restore from this restore point."
					: subtitleOverride
			};

			var diskGroupLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 62),
				Size = new Size(880, 36),
				Visible = isDiskOrHyperVBackup,
				Text = isDiskOrHyperVBackup
					? "This is a full-disk backup. You may restore a single volume or click Select Full Disk to reconstruct the entire disk in partition order."
					: string.Empty
			};

			volumesListView = new ListView
			{
				Location = new Point(16, 108),
				Size = new Size(880, 310),
				View = View.Details,
				FullRowSelect = true,
				MultiSelect = false,
				GridLines = true,
				Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
			};
			volumesListView.Columns.Add("Image", 70);
			volumesListView.Columns.Add("Label", 220);
			volumesListView.Columns.Add("Partition Type", 140);
			volumesListView.Columns.Add("File System", 110);
			volumesListView.Columns.Add("Source", 150);
			volumesListView.Columns.Add("Original Size", 90);
			volumesListView.Columns.Add("Used Size", 90);
			volumesListView.DoubleClick += (_, _) => CommitSingleVolume();

			foreach (VolumeInfo volume in allVolumes)
			{
				var item = new ListViewItem(volume.ImageIndex.ToString());
				item.SubItems.Add(BuildLabel(volume));
				item.SubItems.Add(DescribePartitionType(volume));
				item.SubItems.Add(string.IsNullOrWhiteSpace(volume.FileSystem) ? "—" : volume.FileSystem);
				item.SubItems.Add(string.IsNullOrWhiteSpace(volume.SourceVolumeMountPath) ? "—" : volume.SourceVolumeMountPath);
				item.SubItems.Add(FormatSize(volume.Size));
				item.SubItems.Add(volume.UsedSpace > 0 ? FormatSize(volume.UsedSpace) : "—");
				item.Tag = volume;
				volumesListView.Items.Add(item);
			}

			if (volumesListView.Items.Count > 0)
			{
				volumesListView.Items[0].Selected = true;
			}

			var hintLabel = new Label
			{
				AutoSize = false,
				Location = new Point(16, 426),
				Size = new Size(520, 24),
				Text = "Select a volume, then click Restore."
			};

			var restoreButton = new Button
			{
				Text = "Restore",
				Size = new Size(96, 32),
				Location = new Point(602, 452),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				Enabled = volumesListView.Items.Count > 0
			};
			restoreButton.Click += (_, _) => CommitSingleVolume();

			var selectFullDiskButton = new Button
			{
				Text = "Select Full Disk",
				Size = new Size(130, 32),
				Location = new Point(704, 452),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				Visible = isDiskOrHyperVBackup
			};
			selectFullDiskButton.Click += (_, _) => CommitFullDisk();

			var cancelButton = new Button
			{
				Text = "Cancel",
				Size = new Size(96, 32),
				Location = new Point(840, 452),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.Cancel
			};

			CancelButton = cancelButton;
			Controls.Add(subtitleLabel);
			Controls.Add(diskGroupLabel);
			Controls.Add(volumesListView);
			Controls.Add(hintLabel);
			Controls.Add(restoreButton);
			Controls.Add(selectFullDiskButton);
			Controls.Add(cancelButton);
		}

		public VolumeInfo? SelectedVolume { get; private set; }

		public IReadOnlyList<VolumeInfo>? SelectedDiskGroup { get; private set; }

		public bool Confirmed { get; private set; }

		private void CommitSingleVolume()
		{
			if (volumesListView.SelectedItems.Count == 0 || volumesListView.SelectedItems[0].Tag is not VolumeInfo selectedVolume)
			{
				return;
			}

			SelectedVolume = selectedVolume;
			SelectedDiskGroup = null;
			Confirmed = true;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CommitFullDisk()
		{
			if (!isDiskOrHyperVBackup)
			{
				return;
			}

			SelectedDiskGroup = allVolumes
				.OrderBy(v => v.PartitionOffsetBytes)
				.ThenBy(v => v.PartitionNumber)
				.ToList();
			SelectedVolume = null;
			Confirmed = true;
			DialogResult = DialogResult.OK;
			Close();
		}

		private static string BuildLabel(VolumeInfo volume)
		{
			if (!string.IsNullOrWhiteSpace(volume.Label))
			{
				return volume.Label;
			}

			if (!string.IsNullOrWhiteSpace(volume.SourceVolumeMountPath))
			{
				return volume.SourceVolumeMountPath.TrimEnd('\\');
			}

			return $"Volume {volume.PartitionNumber}";
		}

		private static string DescribePartitionType(VolumeInfo volume)
		{
			if (volume.IsBootVolume)
			{
				return "Boot";
			}

			if (volume.IsSystemVolume)
			{
				return "System";
			}

			return string.IsNullOrWhiteSpace(volume.PartitionType) ? "Data" : volume.PartitionType;
		}

		private static string FormatSize(long bytes)
		{
			if (bytes <= 0)
			{
				return "—";
			}

			double gb = bytes / (1024.0 * 1024.0 * 1024.0);
			if (gb >= 1.0)
			{
				return $"{gb:F2} GB";
			}

			double mb = bytes / (1024.0 * 1024.0);
			return $"{mb:F0} MB";
		}
	}
}
