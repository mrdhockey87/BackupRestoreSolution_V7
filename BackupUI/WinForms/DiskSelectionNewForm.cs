//DiskSelectionForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Forms;

using SecureServerBackup.Helpers;
using SecureServerBackup.Services;

namespace SecureServerBackup.WinForms
{
    public partial class DiskSelectionNewForm : Form
    {
        public sealed record DiskInfo(int DiskIndex, string Model, long SizeBytes);

        private readonly HashSet<int> _excludedIndexes;

        public DiskSelectionNewForm()
            : this(Array.Empty<int>())
        {
        }

        public DiskSelectionNewForm(IEnumerable<int> excludedDiskIndexes)
        {
            InitializeComponent();
            _excludedIndexes = excludedDiskIndexes.ToHashSet();
        }

        public DiskInfo? SelectedDisk { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
            {
                return;
            }

            try
            {
                LoadDisks();
            }
            catch (Exception ex)
            {
                CustomDialogService.ShowError(this, $"Error enumerating disks: {ex.Message}", "Error");
            }
        }

        private void LoadDisks()
        {
            using var searcher = new ManagementObjectSearcher("SELECT Index, Model, Size FROM Win32_DiskDrive");
            foreach (ManagementObject disk in searcher.Get().OfType<ManagementObject>())
            {
                int index = Convert.ToInt32(disk["Index"] ?? 0);
                if (_excludedIndexes.Contains(index))
                {
                    continue;
                }

                string model = disk["Model"]?.ToString() ?? "Unknown Model";
                long size = Convert.ToInt64(disk["Size"] ?? 0L);

                var info = new DiskInfo(index, model, size);
                var item = new ListViewItem(index.ToString()) { Tag = info };
                item.SubItems.Add(model);
                item.SubItems.Add(UiFormatHelper.FormatSize(size));
                lvDisks.Items.Add(item);
            }
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            if (lvDisks.SelectedItems.Count == 0)
            {
                CustomDialogService.ShowError(this, "Select a disk first.", "No Disk Selected");
                return;
            }

            SelectedDisk = lvDisks.SelectedItems[0].Tag as DiskInfo;
            DialogResult = DialogResult.OK;
        }

        private void lvDisks_DoubleClick(object? sender, EventArgs e)
        {
            if (lvDisks.SelectedItems.Count > 0)
            {
                btnOk.PerformClick();
            }
        }
    }
}
