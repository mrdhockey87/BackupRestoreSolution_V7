using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Models
{
	public class RestorePointArchiveImage
	{
		public int ImageIndex { get; set; }
		public DateTime? BackupStartTime { get; set; }
		public string Name { get; set; } = string.Empty;
		public string VolumeLabel { get; set; } = string.Empty;
		public string SourceVolumeMountPath { get; set; } = string.Empty;
		public ulong PartitionOffsetBytes { get; set; }
		public int VolumeIndex { get; set; }
		public bool CollapseToSingleRestorePoint { get; set; }
	}
}
