using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Models
{
	public class DiskInfo
	{
		public int DiskIndex { get; set; }
		public string DisplayName { get; set; } = string.Empty;
		public string Details { get; set; } = string.Empty;
		public long SizeBytes { get; set; }
		public string Model { get; set; } = string.Empty;
		public string DeviceId { get; set; } = string.Empty;
		public List<string> VolumeLetters { get; set; } = [];
		// Fallback text (accessibility / keyboard-search); actual rendering is owner-drawn.
		public override string ToString() => DisplayName;
	}
}
