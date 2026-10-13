//VolumeConfigurationForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;
using SecureServerBackup.Helpers;

#nullable enable
namespace SecureServerBackup.WinForms
{
    public partial class VolumeConfigurationNewForm : Form
    {
        public VolumeConfigurationNewForm()
        {
            InitializeComponent();
        }

        public VolumeConfigurationNewForm(
            IReadOnlyList<VolumeInfo> sourceVolumes,
            long targetDiskSize,
            int sourceAllocationUnitSize,
            int targetAllocationUnitSize) : this()
        {
            ArgumentNullException.ThrowIfNull(sourceVolumes);
            Populate(sourceVolumes, targetDiskSize, sourceAllocationUnitSize, targetAllocationUnitSize);
        }

        private void Populate(
            IReadOnlyList<VolumeInfo> volumes,
            long targetDiskSize,
            int sourceAus,
            int targetAus)
        {
            foreach (VolumeInfo volume in volumes)
            {
                var item = new ListViewItem(volume.Label);
                item.SubItems.Add(UiFormatHelper.FormatSize(volume.Size));
                item.SubItems.Add(UiFormatHelper.FormatSize(volume.UsedSpace));
                item.SubItems.Add(volume.FileSystem);
                item.SubItems.Add($"{volume.AllocationUnitSize} B");
                lvVolumes.Items.Add(item);
            }

            long totalUsed = volumes.Sum(v => v.UsedSpace);
            bool fits = totalUsed <= targetDiskSize;

            lblTargetSize.Text = $"Target disk size: {UiFormatHelper.FormatSize(targetDiskSize)}";
            lblTotalUsed.Text = $"Data to copy: {UiFormatHelper.FormatSize(totalUsed)}";

            if (!fits)
            {
                lblWarning.Text = "The selected data is larger than the target disk. " +
                                  "Deselect volumes or choose a larger disk.";
            }
            else if (sourceAus != targetAus)
            {
                lblWarning.Text = $"Allocation unit size differs (source {sourceAus} B, target {targetAus} B).";
            }

            btnOk.Enabled = fits;
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
