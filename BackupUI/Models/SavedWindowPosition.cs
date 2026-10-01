using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SecureServerBackup.Models
{
	/// <summary>
	/// Data class for saved window position (matches WindowPositionManager format)
	/// </summary>
	public class SavedWindowPosition
	{
		public double Left { get; set; }
		public double Top { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public WindowState WindowState { get; set; }
	}
}
