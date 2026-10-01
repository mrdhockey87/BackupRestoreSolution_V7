using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public sealed record MountedHyperVGuestExecutionPartition(int PartitionNumber, string MountPath, bool CreatedMountDirectory);

	public sealed record MountedHyperVGuestTreeDisk(string MountedDiskPath, List<string> MountDirectories);

	public sealed class MountedHyperVGuestExecutionDisk : IDisposable
	{
		private readonly List<MountedHyperVGuestExecutionPartition> _partitions;
		private readonly string _mountRoot;

		public MountedHyperVGuestExecutionDisk(string virtualDiskPath, string mountRoot, List<MountedHyperVGuestExecutionPartition> partitions)
		{
			VirtualDiskPath = virtualDiskPath;
			_mountRoot = mountRoot;
			_partitions = partitions;
		}

		public string VirtualDiskPath { get; }

		public IReadOnlyList<MountedHyperVGuestExecutionPartition> Partitions => _partitions;

		public void Dispose()
		{
			try
			{
				RunPowerShell($"Dismount-VHD -Path '{EscapePowerShellSingleQuotedString(VirtualDiskPath)}' -ErrorAction SilentlyContinue | Out-Null");
			}
			catch
			{
			}

			foreach (MountedHyperVGuestExecutionPartition partition in _partitions.Where(partition => partition.CreatedMountDirectory))
			{
				try
				{
					if (Directory.Exists(partition.MountPath))
					{
						Directory.Delete(partition.MountPath, recursive: true);
					}
				}
				catch
				{
				}
			}

			try
			{
				if (Directory.Exists(_mountRoot))
				{
					Directory.Delete(_mountRoot, recursive: true);
				}
			}
			catch
			{
			}
		}
		private static string RunPowerShell(string script)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(script);

			string encodedCommand = Convert.ToBase64String(Encoding.Unicode.GetBytes(script));

			using var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encodedCommand}",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			string errors = process.StandardError.ReadToEnd();
			process.WaitForExit();

			if (process.ExitCode != 0)
			{
				string message = string.IsNullOrWhiteSpace(errors)
					? "The PowerShell command failed."
					: StripCliXml(errors).Trim();
				throw new InvalidOperationException(message);
			}

			return output;
		}
		/// <summary>
		/// Extracts readable text from a PowerShell CLIXML error stream.
		/// If the string does not contain CLIXML markup, it is returned unchanged.
		/// </summary>
		private static string StripCliXml(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
				return raw;

			// PowerShell stderr starts with "#< CLIXML" when it wraps errors in XML
			const string clixmlMarker = "#< CLIXML";
			if (!raw.Contains(clixmlMarker, StringComparison.OrdinalIgnoreCase))
				return raw;

			try
			{
				// Extract all <S S="Error">...</S> text nodes — these carry the human-readable message
				var matches = System.Text.RegularExpressions.Regex.Matches(
					raw,
					@"<S S=""Error"">(?<msg>.*?)</S>",
					System.Text.RegularExpressions.RegexOptions.Singleline);

				var lines = matches
					.Select(m => System.Net.WebUtility.HtmlDecode(m.Groups["msg"].Value)
						.Replace("_x000D__x000A_", "\n", StringComparison.Ordinal)
						.Trim())
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.ToList();

				return lines.Count > 0 ? string.Join("\n", lines) : raw;
			}
			catch
			{
				return raw;
			}
		}
	}
}
