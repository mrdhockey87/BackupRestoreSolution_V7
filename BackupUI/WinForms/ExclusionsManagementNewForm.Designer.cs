//ExclusionsManagementForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
    partial class ExclusionsManagementNewForm
    {
        private System.ComponentModel.IContainer? components = null;
        private ListBox lstExclusions = null!;
        private TextBox txtEntry = null!;
        private Button btnAdd = null!;
        private Button btnBrowseFile = null!;
        private Button btnBrowseFolder = null!;
        private Button btnRemove = null!;
        private Button btnOk = null!;
        private Button btnCancel = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lstExclusions = new ListBox();
            txtEntry = new TextBox();
            btnAdd = new Button();
            btnBrowseFile = new Button();
            btnBrowseFolder = new Button();
            btnRemove = new Button();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lstExclusions.Location = new Point(12, 12);
            lstExclusions.Size = new Size(400, 180);
            lstExclusions.SelectionMode = SelectionMode.MultiExtended;
            lstExclusions.IntegralHeight = false;
            lstExclusions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            txtEntry.Location = new Point(12, 205);
            txtEntry.Size = new Size(300, 23);
            txtEntry.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnAdd.Location = new Point(318, 203);
            btnAdd.Size = new Size(94, 28);
            btnAdd.Text = "Add Pattern";
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.Click += btnAdd_Click;

            btnBrowseFile.Location = new Point(12, 240);
            btnBrowseFile.Size = new Size(120, 28);
            btnBrowseFile.Text = "Add File...";
            btnBrowseFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBrowseFile.Click += btnBrowseFile_Click;

            btnBrowseFolder.Location = new Point(138, 240);
            btnBrowseFolder.Size = new Size(120, 28);
            btnBrowseFolder.Text = "Add Folder...";
            btnBrowseFolder.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBrowseFolder.Click += btnBrowseFolder_Click;

            btnRemove.Location = new Point(290, 240);
            btnRemove.Size = new Size(122, 28);
            btnRemove.Text = "Remove Selected";
            btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRemove.Click += btnRemove_Click;

            btnOk.Location = new Point(256, 285);
            btnOk.Size = new Size(75, 28);
            btnOk.Text = "OK";
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Click += btnOk_Click;

            btnCancel.Location = new Point(337, 285);
            btnCancel.Size = new Size(75, 28);
            btnCancel.Text = "Cancel";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(424, 330);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(360, 300);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Exclusions";
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.AddRange(new Control[]
            {
                lstExclusions, txtEntry, btnAdd, btnBrowseFile, btnBrowseFolder, btnRemove, btnOk, btnCancel
            });

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
