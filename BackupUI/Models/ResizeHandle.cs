using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SecureServerBackup.Models
{
	/// <summary>
	/// Represents a draggable resize handle between two volumes
	/// </summary>
	public class ResizeHandle
	{
		public int LeftVolumeIndex { get; set; }    // Volume on left side
		public int RightVolumeIndex { get; set; }   // Volume on right side
		public double CenterX { get; set; }         // X position of handle center
		public Ellipse? UIElement { get; set; }      // Visual handle circle
		public bool IsEnabled { get; set; }         // Can this handle be dragged?
	}
}
