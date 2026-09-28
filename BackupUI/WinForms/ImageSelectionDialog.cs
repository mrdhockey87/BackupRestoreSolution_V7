using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;

namespace SecureServerBackup.WinForms
{
	public partial class ImageSelectionDialog : Form
	{
		public int SelectedImageIndex { get; private set; } = -1;

		private readonly BindingSource _bindingSource = new BindingSource();

		/// <param name="images">List of images in the backup.</param>
		/// <param name="actionLabel">Label for the primary action button, e.g. "Mount Selected" or "Verify Selected".</param>
		/// <param name="subtitleText">Optional subtitle shown below the header.</param>
		public ImageSelectionDialog(List<BackupImageInfo> images, string actionLabel = "Mount Selected", string? subtitleText = null)
		{
			InitializeComponent();

			if (images == null)
				throw new ArgumentNullException(nameof(images));
			if (images.Count == 0)
				throw new ArgumentException("No images provided", nameof(images));

			btnAction.Text = actionLabel;
			if (subtitleText != null)
				lblSubtitle.Text = subtitleText;

			// Sort images by date (most recent first)
			var sortedImages = images.OrderByDescending(i => i.ImageDate).ToList();

			_bindingSource.DataSource = sortedImages;
			dgImages.DataSource = _bindingSource;

			// Pre-select most recent (first row after sorting)
			if (dgImages.Rows.Count > 0)
			{
				dgImages.Rows[0].Selected = true;
				dgImages.CurrentCell = dgImages.Rows[0].Cells[0];
			}
		}

		private void btnMount_Click(object sender, EventArgs e)
		{
			if (dgImages.CurrentRow?.DataBoundItem is BackupImageInfo selectedImage)
			{
				SelectedImageIndex = selectedImage.ImageIndex;
				DialogResult = DialogResult.OK;
				Close();
			}
			else
			{
				MessageBox.Show(
					"Please select a restore point.",
					"No Selection",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void dgImages_DoubleClick(object sender, EventArgs e)
		{
			// Double-click to mount
			if (dgImages.CurrentRow != null)
			{
				btnMount_Click(sender, e);
			}
		}
	}
}
