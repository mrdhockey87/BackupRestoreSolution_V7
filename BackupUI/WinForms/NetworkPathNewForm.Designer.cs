//NetworkPathForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;
#nullable enable
namespace SecureServerBackup.WinForms
{
    partial class NetworkPathNewForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Label lblPath = null!;
        private TextBox txtPath = null!;
        private Label lblHint = null!;
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
            lblPath = new Label();
            txtPath = new TextBox();
            lblHint = new Label();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblPath.AutoSize = true;
            lblPath.Location = new Point(12, 15);
            lblPath.Text = "Network path (UNC):";

            txtPath.Location = new Point(12, 36);
            txtPath.Size = new Size(416, 23);
            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblHint.Location = new Point(12, 64);
            lblHint.Size = new Size(416, 20);
            lblHint.ForeColor = SystemColors.GrayText;
            lblHint.Text = @"Example: \\server\share or \\server\share\folder";
            lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnOk.Location = new Point(271, 100);
            btnOk.Size = new Size(75, 28);
            btnOk.Text = "OK";
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Click += btnOk_Click;

            btnCancel.Location = new Point(353, 100);
            btnCancel.Size = new Size(75, 28);
            btnCancel.Text = "Cancel";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(440, 140);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Network Path";
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.AddRange(new Control[] { lblPath, txtPath, lblHint, btnOk, btnCancel });

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
