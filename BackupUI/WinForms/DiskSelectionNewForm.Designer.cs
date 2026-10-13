//DiskSelectionForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
    partial class DiskSelectionNewForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Label lblInfo = null!;
        private ListView lvDisks = null!;
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
            lblInfo = new Label();
            lvDisks = new ListView();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(12, 12);
            lblInfo.Text = "Select the physical disk to clone to. Source disks are hidden.";

            lvDisks.Location = new Point(12, 36);
            lvDisks.Size = new Size(560, 240);
            lvDisks.View = View.Details;
            lvDisks.FullRowSelect = true;
            lvDisks.MultiSelect = false;
            lvDisks.HideSelection = false;
            lvDisks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvDisks.Columns.Add("Disk", 70);
            lvDisks.Columns.Add("Model", 320);
            lvDisks.Columns.Add("Size", 120);
            lvDisks.DoubleClick += lvDisks_DoubleClick;

            btnOk.Location = new Point(412, 288);
            btnOk.Size = new Size(75, 28);
            btnOk.Text = "OK";
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Click += btnOk_Click;

            btnCancel.Location = new Point(493, 288);
            btnCancel.Size = new Size(75, 28);
            btnCancel.Text = "Cancel";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 328);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Target Disk";
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.AddRange(new Control[] { lblInfo, lvDisks, btnOk, btnCancel });

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
