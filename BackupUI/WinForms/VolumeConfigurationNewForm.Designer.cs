//VolumeConfigurationForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
    partial class VolumeConfigurationNewForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Label lblHeader = null!;
        private ListView lvVolumes = null!;
        private Label lblTargetSize = null!;
        private Label lblTotalUsed = null!;
        private Label lblWarning = null!;
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
            lblHeader = new Label();
            lvVolumes = new ListView();
            lblTargetSize = new Label();
            lblTotalUsed = new Label();
            lblWarning = new Label();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblHeader.AutoSize = true;
            lblHeader.Location = new Point(12, 12);
            lblHeader.Font = new Font(Font, FontStyle.Bold);
            lblHeader.Text = "Review clone configuration";

            lvVolumes.Location = new Point(12, 36);
            lvVolumes.Size = new Size(640, 180);
            lvVolumes.View = View.Details;
            lvVolumes.FullRowSelect = true;
            lvVolumes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lvVolumes.Columns.Add("Volume", 240);
            lvVolumes.Columns.Add("Size", 90);
            lvVolumes.Columns.Add("Used", 90);
            lvVolumes.Columns.Add("File System", 90);
            lvVolumes.Columns.Add("Alloc. Unit", 90);

            lblTargetSize.AutoSize = true;
            lblTargetSize.Location = new Point(12, 232);

            lblTotalUsed.AutoSize = true;
            lblTotalUsed.Location = new Point(12, 256);

            lblWarning.Location = new Point(12, 282);
            lblWarning.Size = new Size(640, 40);
            lblWarning.ForeColor = Color.DarkRed;
            lblWarning.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnOk.Location = new Point(496, 332);
            btnOk.Size = new Size(75, 28);
            btnOk.Text = "OK";
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Click += btnOk_Click;

            btnCancel.Location = new Point(577, 332);
            btnCancel.Size = new Size(75, 28);
            btnCancel.Text = "Cancel";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(664, 372);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Volume Configuration";
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.AddRange(new Control[]
            {
                lblHeader, lvVolumes, lblTargetSize, lblTotalUsed, lblWarning, btnOk, btnCancel
            });

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
