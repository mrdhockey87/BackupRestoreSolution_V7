//ExclusionsManagementForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SecureServerBackup.WinForms
{
    public partial class ExclusionsManagementNewForm : Form
    {
        public ExclusionsManagementNewForm()
        {
            InitializeComponent();
        }

        public ExclusionsManagementNewForm(IEnumerable<string> existingExclusions) : this()
        {
            foreach (string entry in existingExclusions)
            {
                AddEntry(entry);
            }
        }

        public List<string> Exclusions { get; private set; } = new();

        private void AddEntry(string? value)
        {
            string entry = value?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(entry))
            {
                return;
            }

            bool exists = lstExclusions.Items.Cast<string>()
                .Any(item => string.Equals(item, entry, StringComparison.OrdinalIgnoreCase));
            if (!exists)
            {
                lstExclusions.Items.Add(entry);
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            AddEntry(txtEntry.Text);
            txtEntry.Clear();
        }

        private void btnBrowseFile_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select files to exclude",
                Multiselect = true
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in dialog.FileNames)
                {
                    AddEntry(file);
                }
            }
        }

        private void btnBrowseFolder_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select a folder to exclude",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                AddEntry(dialog.SelectedPath);
            }
        }

        private void btnRemove_Click(object? sender, EventArgs e)
        {
            foreach (object item in lstExclusions.SelectedItems.Cast<object>().ToList())
            {
                lstExclusions.Items.Remove(item);
            }
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            Exclusions = lstExclusions.Items.Cast<string>().ToList();
            DialogResult = DialogResult.OK;
        }
    }
}
