using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Models
{
	public class RestorePoint
	{
		public string DisplayName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string BackupType { get; set; } = string.Empty;
		public string FilePath { get; set; } = string.Empty;
		public DateTime Timestamp { get; set; }
		public int ImageIndex { get; set; }
	}
}
