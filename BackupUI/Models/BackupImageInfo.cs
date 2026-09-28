using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Models
{
	/// <summary>
	/// Information about a single backup image/restore point
	/// </summary>
	public class BackupImageInfo
	{
		public int ImageIndex { get; set; }
		public DateTime ImageDate { get; set; }
		public string Name { get; set; } = string.Empty;
		public string ImageType { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
