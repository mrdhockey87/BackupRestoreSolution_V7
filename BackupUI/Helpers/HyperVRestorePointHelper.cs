using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SecureServerBackup.Helpers
{
	public static class HyperVRestorePointHelper
	{
		private static string? ReadMetadataValue(string backupPointPath, string key)
		{
			string metadataPath = Path.Combine(backupPointPath, "hyperv_backup_info.txt");
			if (!File.Exists(metadataPath))
			{
				return null;
			}

			foreach (string line in File.ReadLines(metadataPath))
			{
				string[] parts = line.Split('=', 2);
				if (parts.Length == 2 && string.Equals(parts[0].Trim(), key, StringComparison.OrdinalIgnoreCase))
				{
					string value = parts[1].Trim();
					return string.IsNullOrWhiteSpace(value) ? null : value;
				}
			}

			return null;
		}

		public static bool IsHyperVBackupPoint(string path)
		{
			if (!Directory.Exists(path))
			{
				return false;
			}

			return File.Exists(Path.Combine(path, "hyperv_backup_info.txt")) ||
				   Directory.Exists(Path.Combine(path, "Export"));
		}

		public static string? ResolveExportPath(string backupPointPath)
		{
			if (!Directory.Exists(backupPointPath))
			{
				return null;
			}

			string exportFolder = Path.Combine(backupPointPath, "Export");
			if (Directory.Exists(exportFolder))
			{
				return exportFolder;
			}

			string? exportPath = ReadMetadataValue(backupPointPath, "ExportPath");
			return !string.IsNullOrWhiteSpace(exportPath) && Directory.Exists(exportPath)
				? exportPath
				: null;
		}

		public static string? FindPrimaryVirtualDisk(string backupPointPath)
		{
			string? exportPath = ResolveExportPath(backupPointPath);
			if (string.IsNullOrWhiteSpace(exportPath) || !Directory.Exists(exportPath))
			{
				return null;
			}

			string[] virtualDisks = Directory.GetFiles(exportPath, "*.vhd*", SearchOption.AllDirectories);
			if (virtualDisks.Length == 0)
			{
				return null;
			}

			return virtualDisks
				.Select(path => new FileInfo(path))
				.OrderByDescending(file => file.Length)
				.ThenBy(file => file.FullName)
				.Select(file => file.FullName)
				.FirstOrDefault();
		}

		public static string ResolveVmName(string backupPointPath)
		{
			try
			{
				string? vmNameFromMetadata = ReadMetadataValue(backupPointPath, "VmName");
				if (!string.IsNullOrWhiteSpace(vmNameFromMetadata))
				{
					return vmNameFromMetadata;
				}

				string? exportPath = ResolveExportPath(backupPointPath);
				if (string.IsNullOrWhiteSpace(exportPath) || !Directory.Exists(exportPath))
				{
					return Path.GetFileNameWithoutExtension(backupPointPath);
				}

				string? configFile = Directory.GetFiles(exportPath, "*.xml", SearchOption.AllDirectories)
					.FirstOrDefault(file =>
						string.Equals(Path.GetFileName(Path.GetDirectoryName(file)), "Virtual Machines", StringComparison.OrdinalIgnoreCase) ||
						string.Equals(Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(file) ?? string.Empty)), "Virtual Machines", StringComparison.OrdinalIgnoreCase));

				if (string.IsNullOrWhiteSpace(configFile) || !File.Exists(configFile))
				{
					return Path.GetFileNameWithoutExtension(backupPointPath);
				}

				XDocument document = XDocument.Load(configFile);
				string? vmName = document.Descendants()
					.FirstOrDefault(element => string.Equals(element.Name.LocalName, "Name", StringComparison.OrdinalIgnoreCase))
					?.Value;

				return string.IsNullOrWhiteSpace(vmName)
					? Path.GetFileNameWithoutExtension(backupPointPath)
					: vmName.Trim();
			}
			catch
			{
				return Path.GetFileNameWithoutExtension(backupPointPath);
			}
		}
	}
}
