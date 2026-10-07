using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureServerBackup.Enums;

namespace SecureServerBackup.Models
{
	public class SourceTreeNodeData
	{
		public required SourceTreeNodeKind Kind { get; init; }
		public int DiskNumber { get; init; }
		public string SelectionPath { get; init; } = string.Empty;
		public string FileSystemPath { get; init; } = string.Empty;
		public string VirtualMachineName { get; init; } = string.Empty;
		public bool IsRemovableNetworkPath { get; init; }
	}
}
