using SecureServerBackup.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public class MountedVirtualDiskScope : IDisposable
	{
		private readonly string _virtualDiskPath;
		private bool _disposed;

		public MountedVirtualDiskScope(string virtualDiskPath, string driveRoot)
		{
			_virtualDiskPath = virtualDiskPath;
			DriveRoot = driveRoot;
		}

		public string DriveRoot { get; }

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}

			BackupMountManager.UnmountVirtualDisk(_virtualDiskPath);
			_disposed = true;
		}
	}
}
