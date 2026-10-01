using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecureServerBackup.Enums;

namespace SecureServerBackup.Models
{
	public class FileSystemNodeEntry
	{
		public required SourceTreeNodeKind Kind { get; init; }
		public required string Text { get; init; }
		public required string SelectionPath { get; init; }
		public required string FileSystemPath { get; init; }
	}
}
