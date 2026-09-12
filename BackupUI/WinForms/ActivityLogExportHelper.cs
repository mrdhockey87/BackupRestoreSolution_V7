using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SecureServerBackupCommon;

namespace SecureServerBackup.WinForms
{
	internal static class ActivityLogExportHelper
	{
		public static bool PromptAndExport(IWin32Window owner, IReadOnlyList<BackupLogEntry> logs, string defaultBaseName, string format)
		{
			ArgumentNullException.ThrowIfNull(logs);

			using var dialog = new SaveFileDialog
			{
				FileName = defaultBaseName,
				Filter = string.Equals(format, "CSV", StringComparison.OrdinalIgnoreCase)
					? "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
					: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
				DefaultExt = string.Equals(format, "CSV", StringComparison.OrdinalIgnoreCase) ? ".csv" : ".txt"
			};

			if (dialog.ShowDialog(owner) != DialogResult.OK)
			{
				return false;
			}

			if (string.Equals(format, "CSV", StringComparison.OrdinalIgnoreCase))
			{
				ExportToCsv(logs, dialog.FileName);
			}
			else
			{
				ExportToText(logs, dialog.FileName);
			}

			return true;
		}

		public static string BuildClipboardText(IReadOnlyList<BackupLogEntry> logs)
		{
			var text = new StringBuilder();
			foreach (BackupLogEntry log in logs.OrderBy(l => l.Timestamp))
			{
				text.AppendLine($"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] [{log.Level}] {log.JobName}");
				text.AppendLine($"  Message: {log.Message}");
				if (!string.IsNullOrEmpty(log.Details))
				{
					text.AppendLine($"  Details: {log.Details}");
				}
				if (!string.IsNullOrEmpty(log.BackupPath))
				{
					text.AppendLine($"  Backup Path: {log.BackupPath}");
				}
				text.AppendLine($"  Validation: {(log.ValidationPassed ? "PASSED" : "FAILED")}");
				text.AppendLine();
			}

			return text.ToString();
		}

		private static void ExportToCsv(IReadOnlyList<BackupLogEntry> logs, string filePath)
		{
			var csv = new StringBuilder();
			csv.AppendLine("Timestamp,Job Name,Level,Message,Details,Backup Path,Validation Passed");

			foreach (BackupLogEntry log in logs.OrderBy(l => l.Timestamp))
			{
				csv.AppendLine($"\"{log.Timestamp:yyyy-MM-dd HH:mm:ss}\"," +
							  $"\"{EscapeCsv(log.JobName)}\"," +
							  $"\"{log.Level}\"," +
							  $"\"{EscapeCsv(log.Message)}\"," +
							  $"\"{EscapeCsv(log.Details)}\"," +
							  $"\"{EscapeCsv(log.BackupPath)}\"," +
							  $"\"{log.ValidationPassed}\"");
			}

			File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
		}

		private static void ExportToText(IReadOnlyList<BackupLogEntry> logs, string filePath)
		{
			var text = new StringBuilder();
			text.AppendLine("===== BACKUP ACTIVITY LOG =====");
			text.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
			text.AppendLine($"Total Entries: {logs.Count}");
			text.AppendLine("================================");
			text.AppendLine();
			text.Append(BuildClipboardText(logs));
			File.WriteAllText(filePath, text.ToString(), Encoding.UTF8);
		}

		private static string EscapeCsv(string? value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}

			return value.Replace("\"", "\"\"").Replace("\n", " ").Replace("\r", string.Empty);
		}
	}
}
