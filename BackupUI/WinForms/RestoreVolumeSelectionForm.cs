using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;

namespace SecureServerBackup.WinForms
{
	internal sealed partial class RestoreVolumeSelectionForm : Form
	{
		private readonly bool isDiskOrHyperVBackup;
		private readonly List<VolumeInfo> allVolumes;

		public RestoreVolumeSelectionForm()
			: this(Array.Empty<VolumeInfo>(), false)
		{
		}

		public RestoreVolumeSelectionForm(IReadOnlyList<VolumeInfo> volumes, bool isDiskOrHyperVBackup, string? subtitleOverride = null)
		{
			ArgumentNullException.ThrowIfNull(volumes);

			this.isDiskOrHyperVBackup = isDiskOrHyperVBackup;
			allVolumes = volumes.ToList();

			InitializeComponent();
			subtitleLabel.Text = string.IsNullOrWhiteSpace(subtitleOverride)
				? "Select the volume or full set of volumes to restore from this restore point."
				: subtitleOverride;
			diskGroupLabel.Visible = isDiskOrHyperVBackup;
			diskGroupLabel.Text = isDiskOrHyperVBackup
				? "This is a full-disk backup. You may restore a single volume or click Select Full Disk to reconstruct the entire disk in partition order."
				: string.Empty;
			selectFullDiskButton.Visible = isDiskOrHyperVBackup;
			PopulateVolumes();
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

		private void PopulateVolumes()
		{
			volumesListView.Items.Clear();

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

			restoreButton.Enabled = volumesListView.Items.Count > 0;
		}

		private void VolumesListView_DoubleClick(object? sender, EventArgs e)
		{
			CommitSingleVolume();
		}

		private void RestoreButton_Click(object? sender, EventArgs e)
		{
			CommitSingleVolume();
		}

		private void SelectFullDiskButton_Click(object? sender, EventArgs e)
		{
			CommitFullDisk();
		}
	}
}
