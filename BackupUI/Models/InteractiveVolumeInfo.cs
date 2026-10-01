using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SecureServerBackup.Models
{
	/// <summary>
	/// Enhanced volume info with interactive properties
	/// </summary>
	public class InteractiveVolumeInfo
	{
		public string Label { get; set; } = string.Empty;
		public long OriginalSize { get; set; }      // Size from source
		public long CurrentSize { get; set; }       // User-modified size
		public long UsedSpace { get; set; }
		public long FreeSpace => CurrentSize - UsedSpace;
		public long MinSize { get; set; }           // Minimum = Used + 10%
		public long MaxSize { get; set; }           // Maximum based on available space
		public bool IsResizable { get; set; }
		public bool IsSystemVolume { get; set; }
		public string FileSystem { get; set; } = string.Empty;
		public int AllocationUnitSize { get; set; }

		// Original VolumeInfo so restore metadata is preserved on Accept
		public VolumeInfo? Source { get; set; }

		// UI State
		public int Index { get; set; }
		public bool IsSelected { get; set; }
		public Rectangle? UIElement { get; set; }    // Visual rectangle on canvas
		public TextBlock? LabelElement { get; set; } // Label text
	}
}
