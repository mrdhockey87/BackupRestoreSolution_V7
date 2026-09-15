using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureServerBackup.Models;

namespace SecureServerBackup.WinForms.Controls
{
	internal sealed class VolumeResizeControl : UserControl
	{
		private const int HorizontalPadding = 16;
		private const int SourceBarTop = 24;
		private const int TargetBarTop = 114;
		private const int BarHeight = 42;
		private const int HandleWidth = 10;

		private readonly List<Rectangle> resizeHandles = [];
		private static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private List<VolumeResizeInfo> volumes = [];
		private VolumeResizeManager? resizeManager;
		private long targetDiskSize;
		private int activeHandleIndex = -1;
		private int dragStartX;
		private long dragStartSize;

		public VolumeResizeControl()
		{
			DoubleBuffered = true;
			ResizeRedraw = true;
			BackColor = Color.White;
			MinimumSize = new Size(500, 220);
			Cursor = Cursors.Default;

			if (IsInDesignMode)
			{
				InitializeDesignTimePreview();
			}
		}

		public void Initialize(List<VolumeResizeInfo> sourceVolumes, long targetDiskSizeBytes)
		{
			ArgumentNullException.ThrowIfNull(sourceVolumes);

			volumes = sourceVolumes;
			targetDiskSize = targetDiskSizeBytes;
			resizeManager = new VolumeResizeManager(volumes, targetDiskSize);

			foreach (var volume in volumes)
			{
				volume.PropertyChanged -= Volume_PropertyChanged;
				volume.PropertyChanged += Volume_PropertyChanged;
			}

			Invalidate();
		}

		public (bool IsValid, string ErrorMessage) ValidateConfiguration()
		{
			return resizeManager?.Validate() ?? (false, "The resize control has not been initialized.");
		}

		public void AutoFit()
		{
			resizeManager?.AutoFit();
			Invalidate();
		}

		private void InitializeDesignTimePreview()
		{
			volumes =
			[
				new VolumeResizeInfo
				{
					Index = 0,
					Label = "Windows",
					OriginalSize = 240L * 1024 * 1024 * 1024,
					DataSize = 120L * 1024 * 1024 * 1024,
					TargetSize = 260L * 1024 * 1024 * 1024
				},
				new VolumeResizeInfo
				{
					Index = 1,
					Label = "Data",
					OriginalSize = 180L * 1024 * 1024 * 1024,
					DataSize = 80L * 1024 * 1024 * 1024,
					TargetSize = 200L * 1024 * 1024 * 1024
				}
			];

			targetDiskSize = 512L * 1024 * 1024 * 1024;
			resizeManager = new VolumeResizeManager(volumes, targetDiskSize);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var baseFont = Font ?? SystemFonts.DefaultFont;

			e.Graphics.Clear(BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			if (volumes.Count == 0 || resizeManager == null || targetDiskSize <= 0)
			{
				DrawCenteredText(e.Graphics, "Volume resize preview is unavailable.", ClientRectangle, baseFont, Brushes.DimGray);
				return;
			}

			resizeHandles.Clear();

			using var headerBrush = new SolidBrush(Color.FromArgb(48, 48, 48));
			using var subtleBrush = new SolidBrush(Color.DimGray);
			e.Graphics.DrawString("Source Layout", new Font(baseFont, FontStyle.Bold), headerBrush, HorizontalPadding, 2);
			e.Graphics.DrawString("Target Layout", new Font(baseFont, FontStyle.Bold), headerBrush, HorizontalPadding, 92);

			var sourceArea = new Rectangle(HorizontalPadding, SourceBarTop, Math.Max(120, Width - (HorizontalPadding * 2)), BarHeight);
			var targetArea = new Rectangle(HorizontalPadding, TargetBarTop, Math.Max(120, Width - (HorizontalPadding * 2)), BarHeight);

			DrawVolumeBar(e.Graphics, sourceArea, volumes.Sum(v => v.OriginalSize), useTargetSizes: false);
			DrawVolumeBar(e.Graphics, targetArea, targetDiskSize, useTargetSizes: true);

			var sourceTotal = volumes.Sum(v => v.OriginalSize);
			var targetTotal = volumes.Sum(v => v.TargetSize);
			var freeSpace = resizeManager.RemainingSpace;

			e.Graphics.DrawString($"Total: {FormatGb(sourceTotal)}", baseFont, subtleBrush, HorizontalPadding, 72);
			e.Graphics.DrawString($"Used: {FormatGb(targetTotal)}", baseFont, subtleBrush, HorizontalPadding, 162);
			e.Graphics.DrawString($"Free: {FormatGb(freeSpace)}", baseFont, subtleBrush, HorizontalPadding + 150, 162);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			for (var i = 0; i < resizeHandles.Count; i++)
			{
				if (resizeHandles[i].Contains(e.Location))
				{
					activeHandleIndex = i;
					dragStartX = e.X;
					dragStartSize = volumes[i].TargetSize;
					Cursor = Cursors.SizeWE;
					return;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (activeHandleIndex >= 0 && resizeManager != null)
			{
				var availableWidth = Math.Max(1, Width - (HorizontalPadding * 2));
				var deltaPixels = e.X - dragStartX;
				var sizeDelta = (long)(deltaPixels / (double)availableWidth * targetDiskSize);
				var requestedSize = dragStartSize + sizeDelta;
				var direction = sizeDelta >= 0 ? 1 : -1;

				if (resizeManager.ResizeVolume(activeHandleIndex, requestedSize, direction))
				{
					dragStartX = e.X;
					dragStartSize = volumes[activeHandleIndex].TargetSize;
					Invalidate();
				}

				return;
			}

			Cursor = resizeHandles.Any(handle => handle.Contains(e.Location)) ? Cursors.SizeWE : Cursors.Default;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			activeHandleIndex = -1;
			Cursor = Cursors.Default;
		}

		private void DrawVolumeBar(Graphics graphics, Rectangle area, long totalSize, bool useTargetSizes)
		{
			if (totalSize <= 0)
			{
				return;
			}

			var currentX = area.Left;
			for (var i = 0; i < volumes.Count; i++)
			{
				var volume = volumes[i];
				var size = useTargetSizes ? volume.TargetSize : volume.OriginalSize;
				var width = (int)Math.Round(size / (double)totalSize * area.Width);
				if (i == volumes.Count - 1)
				{
					width = area.Right - currentX;
				}

				var segment = new Rectangle(currentX, area.Top, Math.Max(8, width), area.Height);
				using var fillBrush = new SolidBrush(GetVolumeColor(i));
				using var borderPen = new Pen(useTargetSizes ? Color.FromArgb(76, 175, 80) : Color.FromArgb(33, 150, 243), 2);
				graphics.FillRectangle(fillBrush, segment);
				graphics.DrawRectangle(borderPen, segment);

				var text = $"{volume.Label}{Environment.NewLine}{FormatGb(size)}";
				DrawCenteredText(graphics, text, segment, Font ?? SystemFonts.DefaultFont, Brushes.White);

				if (useTargetSizes && i < volumes.Count - 1)
				{
					var handleX = Math.Max(segment.Right - (HandleWidth / 2), area.Left);
					var handle = new Rectangle(handleX, segment.Top, HandleWidth, segment.Height);
					resizeHandles.Add(handle);
					using var handleBrush = new SolidBrush(Color.FromArgb(255, 107, 107));
					graphics.FillRectangle(handleBrush, handle);
				}

				currentX = segment.Right;
			}

			if (!useTargetSizes || resizeManager == null || resizeManager.RemainingSpace <= 0)
			{
				return;
			}

			var freeWidth = area.Right - currentX;
			if (freeWidth <= 0)
			{
				return;
			}

			var freeRect = new Rectangle(currentX, area.Top, freeWidth, area.Height);
			using var dashPen = new Pen(Color.Gray, 2) { DashPattern = [4, 2] };
			using var freeBrush = new SolidBrush(Color.FromArgb(40, 158, 158, 158));
			graphics.FillRectangle(freeBrush, freeRect);
			graphics.DrawRectangle(dashPen, freeRect);
			DrawCenteredText(graphics, $"Free{Environment.NewLine}{FormatGb(resizeManager.RemainingSpace)}", freeRect, Font ?? SystemFonts.DefaultFont, Brushes.DimGray);
		}

		private void Volume_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(VolumeResizeInfo.TargetSize))
			{
				Invalidate();
			}
		}

		private static void DrawCenteredText(Graphics graphics, string text, Rectangle bounds, Font font, Brush brush)
		{
			var format = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};
			graphics.DrawString(text, font, brush, bounds, format);
		}

		private static string FormatGb(long bytes)
		{
			return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
		}

		private static Color GetVolumeColor(int index)
		{
			Color[] colors =
			[
				Color.FromArgb(52, 152, 219),
				Color.FromArgb(46, 204, 113),
				Color.FromArgb(155, 89, 182),
				Color.FromArgb(230, 126, 34),
				Color.FromArgb(231, 76, 60),
				Color.FromArgb(26, 188, 156)
			];

			return colors[index % colors.Length];
		}
	}
}
