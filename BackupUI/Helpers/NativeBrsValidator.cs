using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	// P/Invoke wrapper for BRS validation
	internal static class NativeBrsValidator
	{
		private const string NativeDllName = "SecureServerBackupEngine.dll";

		[DllImport(NativeDllName, CharSet = CharSet.Unicode)]
		private static extern bool Brs_ValidateBackupFile(
			[MarshalAs(UnmanagedType.LPWStr)] string filePath,
			out bool isCompressed,
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder backupName,
			int backupNameSize,
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder backupType,
			int backupTypeSize,
			out long timestamp,
			out ulong originalSize,
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder errorMsg,
			int errorMsgSize
		);

		public static bool ValidateBackupFile(
			string filePath,
			out bool isCompressed,
			out string backupName,
			out string backupType,
			out DateTime timestamp,
			out long size,
			StringBuilder errorMsg,
			int errorMsgSize
		)
		{
			var nameBuilder = new StringBuilder(256);
			var typeBuilder = new StringBuilder(64);
			long timestampTicks;
			ulong sizeULong;

			bool result = Brs_ValidateBackupFile(
				filePath,
				out isCompressed,
				nameBuilder,
				256,
				typeBuilder,
				64,
				out timestampTicks,
				out sizeULong,
				errorMsg,
				errorMsgSize
			);

			backupName = nameBuilder.ToString();
			backupType = typeBuilder.ToString();
			timestamp = DateTime.FromFileTime(timestampTicks);
			size = (long)sizeULong;

			return result;
		}
	}
}
