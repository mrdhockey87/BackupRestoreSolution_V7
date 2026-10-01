using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SecureServerBackup.Models;

namespace SecureServerBackup.Controls
{
	public partial class VolumeResizeControl : UserControl
	{
		private const double BytesPerGb = 1024.0 * 1024.0 * 1024.0;
		private const int BarPadding = 2;
		private const int HandleWidth = 16;

		private static readonly Color[] VolumeColors =
		{
			Color.FromArgb(66, 165, 245),   // Blue
			Color.FromArgb(102, 187, 106),  // Green
			Color.FromArgb(255, 167, 38),   // Orange
			Color.FromArgb(171, 71, 188),   // Purple
			Color.FromArgb(239, 83, 80),    // Red
			Color.FromArgb(38, 198, 218),   // Cyan
		};

		private static readonly Color SourceBackColor = Color.FromArgb(236, 239, 241);
		private static readonly Color SourceBorderColor = Color.FromArgb(96, 125, 139);
		private static readonly Color TargetBackColor = Color.FromArgb(232, 245, 233);
		private static readonly Color TargetBorderColor = Color.FromArgb(76, 175, 80);
		private static readonly Color VolumeBorderBlue = Color.FromArgb(33, 150, 243);
		private static readonly Color OkGreen = Color.FromArgb(76, 175, 80);

		private List<VolumeResizeInfo> _volumes = new();
		private VolumeResizeManager? _resizeManager;
		private long _targetDiskSize;

		// Drag state
		private int _dragIndex = -1;
		private int _dragStartX;
		private long _dragStartSize;

		public VolumeResizeControl()
		{
			InitializeComponent();

			EnableDoubleBuffering(pnlSourceBar);
			EnableDoubleBuffering(pnlTargetBar);
			EnableDoubleBuffering(pnlArrows);

			pnlSourceBar.Resize += (s, e) => InvalidateAll();
			pnlTargetBar.Resize += (s, e) => InvalidateAll();
			pnlArrows.Resize += (s, e) => InvalidateAll();

			Disposed += (s, e) => UnsubscribeVolumes();
		}

		#region Public API

		/// <summary>
		/// Initializes the control with volume and disk information
		/// </summary>
		public void Initialize(List<VolumeResizeInfo> volumes, long targetDiskSizeBytes)
		{
			UnsubscribeVolumes();

			_volumes = volumes ?? throw new ArgumentNullException(nameof(volumes));
			_targetDiskSize = targetDiskSizeBytes;
			_resizeManager = new VolumeResizeManager(_volumes, _targetDiskSize);

			foreach (var volume in _volumes)
				volume.PropertyChanged += Volume_PropertyChanged;

			RenderBars();
		}

		/// <summary>
		/// Gets the configured volume sizes for restore
		/// </summary>
		public List<VolumeResizeInfo> GetConfiguredVolumes() => _volumes;

		/// <summary>
		/// Validates the current configuration
		/// </summary>
		//public (bool IsValid, string ErrorMessage) Validate() => _resizeManager!.Validate();

		#endregion

		#region State / refresh

		private void UnsubscribeVolumes()
		{
			foreach (var volume in _volumes)
				volume.PropertyChanged -= Volume_PropertyChanged;
		}

		private void Volume_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(VolumeResizeInfo.TargetSize))
			{
				if (InvokeRequired)
					BeginInvoke(new Action(RenderBars));
				else
					RenderBars();
			}
		}

		private void RenderBars()
		{
			if (_volumes == null || _volumes.Count == 0 || _resizeManager == null)
				return;

			InvalidateAll();
			UpdateSizeLabels();
		}

		private void InvalidateAll()
		{
			pnlSourceBar.Invalidate();
			pnlTargetBar.Invalidate();
			pnlArrows.Invalidate();
		}

		private void UpdateSizeLabels()
		{
			if (_resizeManager == null) return;

			long totalOriginal = _volumes.Sum(v => v.OriginalSize);
			long totalTarget = _volumes.Sum(v => v.TargetSize);
			long freeSpace = _resizeManager.RemainingSpace;

			lblSourceSize.Text = $"Total: {totalOriginal / BytesPerGb:F2} GB";
			lblTargetUsed.Text = $"Used: {totalTarget / BytesPerGb:F2} GB";
			lblTargetFree.Text = $"Free: {freeSpace / BytesPerGb:F2} GB";

			var (isValid, _) = _resizeManager.Validate();
			if (!isValid)
			{
				lblTargetFree.ForeColor = Color.Red;
				lblTargetFree.Text += " \u26A0";
			}
			else
			{
				lblTargetFree.ForeColor = OkGreen;
			}
		}

		#endregion

		#region Layout helpers

		private static void EnableDoubleBuffering(Control control)
		{
			typeof(Control)
				.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
				?.SetValue(control, true);
		}

		private static Color GetVolumeColor(int index) => VolumeColors[index % VolumeColors.Length];

		private static GraphicsPath RoundedRect(RectangleF r, float radius)
		{
			float d = radius * 2;
			var path = new GraphicsPath();
			if (r.Width < d || r.Height < d)
			{
				path.AddRectangle(r);
				return path;
			}
			path.AddArc(r.X, r.Y, d, d, 180, 90);
			path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
			path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
			path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
			path.CloseFigure();
			return path;
		}

		private static float AvailableWidth(Control c) => Math.Max(1, c.ClientSize.Width - BarPadding * 2);

		private List<RectangleF> GetSourceRects(Control bar)
		{
			var rects = new List<RectangleF>();
			long total = _volumes.Sum(v => v.OriginalSize);
			if (total <= 0) return rects;

			float x = BarPadding;
			float h = bar.ClientSize.Height - BarPadding * 2;
			float avail = AvailableWidth(bar);

			foreach (var v in _volumes)
			{
				float w = (float)(v.OriginalSize / (double)total * avail);
				rects.Add(new RectangleF(x, BarPadding, w, h));
				x += w;
			}
			return rects;
		}

		private List<RectangleF> GetTargetRects(Control bar)
		{
			var rects = new List<RectangleF>();
			if (_targetDiskSize <= 0) return rects;

			float x = BarPadding;
			float h = bar.ClientSize.Height - BarPadding * 2;
			float avail = AvailableWidth(bar);

			foreach (var v in _volumes)
			{
				float w = (float)(v.TargetSize / (double)_targetDiskSize * avail);
				rects.Add(new RectangleF(x, BarPadding, w, h));
				x += w;
			}
			return rects;
		}

		private static RectangleF GetHandleRect(RectangleF volumeRect) =>
			new RectangleF(volumeRect.Right - HandleWidth / 2f, volumeRect.Y, HandleWidth, volumeRect.Height);

		private static void DrawContainer(Graphics g, Control bar, Color back, Color border)
		{
			var rect = new RectangleF(1, 1, bar.ClientSize.Width - 2, bar.ClientSize.Height - 2);
			using var path = RoundedRect(rect, 4);
			using var fill = new SolidBrush(back);
			using var pen = new Pen(border, 2);
			g.FillPath(fill, path);
			g.DrawPath(pen, path);
		}

		private static void DrawVolume(Graphics g, RectangleF rect, Color fillColor, Color borderColor, string text)
		{
			if (rect.Width < 1) return;

			using var path = RoundedRect(rect, 4);
			using var fill = new SolidBrush(fillColor);
			using var pen = new Pen(borderColor, 2);
			g.FillPath(fill, path);
			g.DrawPath(pen, path);

			using var font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold);
			using var sf = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisCharacter
			};
			g.DrawString(text, font, Brushes.White, rect, sf);
		}

		#endregion

		#region Painting

		private void pnlSourceBar_Paint(object? sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			DrawContainer(g, pnlSourceBar, SourceBackColor, SourceBorderColor);

			if (_volumes.Count == 0) return;
			var rects = GetSourceRects(pnlSourceBar);

			for (int i = 0; i < rects.Count; i++)
			{
				var v = _volumes[i];
				DrawVolume(g, rects[i], GetVolumeColor(v.Index), VolumeBorderBlue,
					$"{v.Label}\n{v.OriginalSizeGB:F2} GB");
			}
		}

		private void pnlTargetBar_Paint(object? sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			DrawContainer(g, pnlTargetBar, TargetBackColor, TargetBorderColor);

			if (_volumes.Count == 0 || _resizeManager == null) return;
			var rects = GetTargetRects(pnlTargetBar);

			for (int i = 0; i < rects.Count; i++)
			{
				var v = _volumes[i];
				DrawVolume(g, rects[i], GetVolumeColor(v.Index), TargetBorderColor,
					$"{v.Label}\n{v.TargetSizeGB:F2} GB");
			}

			// Free space
			long freeSpace = _resizeManager.RemainingSpace;
			if (freeSpace > 0 && rects.Count > 0)
			{
				float x = rects[^1].Right;
				float w = (float)(freeSpace / (double)_targetDiskSize * AvailableWidth(pnlTargetBar));
				var freeRect = new RectangleF(x, BarPadding, w, pnlTargetBar.ClientSize.Height - BarPadding * 2);

				using var fill = new SolidBrush(Color.FromArgb(50, 158, 158, 158));
				using var pen = new Pen(Color.Gray, 2) { DashPattern = new[] { 2f, 1f } };
				using var path = RoundedRect(freeRect, 4);
				g.FillPath(fill, path);
				g.DrawPath(pen, path);

				using var font = new Font("Segoe UI", 7.5F);
				using var sf = new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				};
				g.DrawString($"Free\n{freeSpace / BytesPerGb:F2} GB", font, Brushes.Gray, freeRect, sf);
			}

			// Resize handles (all but last volume)
			using var handleFill = new SolidBrush(Color.FromArgb(255, 107, 107));
			using var handlePen = new Pen(Color.DarkRed, 1);
			for (int i = 0; i < rects.Count - 1; i++)
			{
				var h = GetHandleRect(rects[i]);
				float cx = h.X + h.Width / 2f;
				float cy = h.Y + h.Height / 2f;
				var tri = new[]
				{
					new PointF(cx - 4, cy - 10),
					new PointF(cx + 4, cy),
					new PointF(cx - 4, cy + 10)
				};
				g.FillPolygon(handleFill, tri);
				g.DrawPolygon(handlePen, tri);
			}
		}

		private void pnlArrows_Paint(object? sender, PaintEventArgs e)
		{
			if (_volumes.Count == 0 || _targetDiskSize <= 0) return;

			var g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			var src = GetSourceRects(pnlSourceBar);
			var tgt = GetTargetRects(pnlTargetBar);
			if (src.Count != tgt.Count) return;

			using var pen = new Pen(Color.Silver, 1)
			{
				DashPattern = new[] { 2f, 2f },
				CustomEndCap = new AdjustableArrowCap(3, 4)
			};

			float top = 3;
			float bottom = pnlArrows.ClientSize.Height - 3;

			for (int i = 0; i < src.Count; i++)
			{
				if (tgt[i].Width < 1) continue;
				float x1 = src[i].X + src[i].Width / 2f;
				float x2 = tgt[i].X + tgt[i].Width / 2f;
				g.DrawLine(pen, x1, top, x2, bottom);
			}
		}

		#endregion

		#region Drag handling

		private int HitTestHandle(Point p)
		{
			if (_volumes.Count < 2 || _targetDiskSize <= 0) return -1;

			var rects = GetTargetRects(pnlTargetBar);
			for (int i = 0; i < rects.Count - 1; i++)
			{
				if (GetHandleRect(rects[i]).Contains(p))
					return i;
			}
			return -1;
		}

		private void pnlTargetBar_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			int idx = HitTestHandle(e.Location);
			if (idx < 0) return;

			_dragIndex = idx;
			_dragStartX = e.X;
			_dragStartSize = _volumes[idx].TargetSize;
		}

		private void pnlTargetBar_MouseMove(object? sender, MouseEventArgs e)
		{
			if (_dragIndex < 0)
			{
				pnlTargetBar.Cursor = HitTestHandle(e.Location) >= 0 ? Cursors.SizeWE : Cursors.Default;
				return;
			}

			if (_resizeManager == null) return;

			double deltaX = e.X - _dragStartX;
			long sizeChange = (long)(deltaX / AvailableWidth(pnlTargetBar) * _targetDiskSize);
			long newSize = _dragStartSize + sizeChange;

			int direction = newSize >= _volumes[_dragIndex].TargetSize ? 1 : -1;
			_resizeManager.ResizeVolume(_dragIndex, newSize, direction);

			RenderBars();
		}

		private void pnlTargetBar_MouseUp(object? sender, MouseEventArgs e)
		{
			_dragIndex = -1;
		}

		#endregion

		#region Buttons

		private void btnAutoFit_Click(object? sender, EventArgs e)
		{
			if (_resizeManager == null) return;
			_resizeManager.AutoFit();
			RenderBars();
		}

		private void btnReset_Click(object? sender, EventArgs e)
		{
			foreach (var volume in _volumes)
				volume.TargetSize = volume.OriginalSize;

			RenderBars();
		}

		#endregion
	}
}