using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public static class RegularHyperVRestoreHelper
	{
		public static bool SupportsHyperVVirtualDiskRestore(string selectedItemText)
		{
			if (string.IsNullOrWhiteSpace(selectedItemText))
			{
				return false;
			}

			return selectedItemText.Contains("PHYSICALDRIVE", StringComparison.OrdinalIgnoreCase) ||
				   selectedItemText.StartsWith("\\?\\", StringComparison.OrdinalIgnoreCase) ||
				   selectedItemText.StartsWith(@"\\?\", StringComparison.OrdinalIgnoreCase) ||
				   selectedItemText.EndsWith(":\\", StringComparison.OrdinalIgnoreCase) ||
				   selectedItemText.EndsWith(":", StringComparison.OrdinalIgnoreCase) ||
				   selectedItemText.Contains("SystemState", StringComparison.OrdinalIgnoreCase);
		}

		public static string NormalizeHyperVVmName(string? displayText)
		{
			if (string.IsNullOrWhiteSpace(displayText))
			{
				return string.Empty;
			}

			int stateIndex = displayText.LastIndexOf(" (", StringComparison.Ordinal);
			return stateIndex > 0 ? displayText[..stateIndex].Trim() : displayText.Trim();
		}

		public static string GetDefaultHyperVVmName(string virtualDiskPath)
		{
			if (string.IsNullOrWhiteSpace(virtualDiskPath))
			{
				return string.Empty;
			}

			return Path.GetFileNameWithoutExtension(virtualDiskPath)?.Trim() ?? string.Empty;
		}

		public static string GetDefaultHyperVVmStoragePath(string virtualDiskPath)
		{
			if (string.IsNullOrWhiteSpace(virtualDiskPath))
			{
				return string.Empty;
			}

			return Path.GetDirectoryName(virtualDiskPath)?.Trim() ?? string.Empty;
		}

		public static string BuildDefaultHyperVVirtualDiskPath(string? directoryPath, string? backupName)
		{
			string sanitizedBackupName = SanitizeFileName(backupName);
			string fileName = string.IsNullOrWhiteSpace(sanitizedBackupName)
				? "RestoredBackup.vhdx"
				: sanitizedBackupName + ".vhdx";

			return string.IsNullOrWhiteSpace(directoryPath)
				? fileName
				: Path.Combine(directoryPath.Trim(), fileName);
		}

		public static string BuildCreateVirtualMachineScript(string vmName, string vmStoragePath, string virtualDiskPath, int generation, bool startAfterCreate)
		{
			string escapedVmName = EscapePowerShellSingleQuotedString(vmName);
			string escapedVmStoragePath = EscapePowerShellSingleQuotedString(vmStoragePath);
			string escapedVirtualDiskPath = EscapePowerShellSingleQuotedString(virtualDiskPath);
			string controllerType = generation == 1 ? "IDE" : "SCSI";
			string firmwareCommand = generation == 2
				? $"; $bootDisk = Get-VMHardDiskDrive -VMName '{escapedVmName}' | Where-Object {{ $_.Path -eq '{escapedVirtualDiskPath}' }} | Select-Object -First 1; if ($null -eq $bootDisk) {{ throw 'The restored virtual disk could not be located on the new virtual machine.'; }}; Set-VMFirmware -VMName '{escapedVmName}' -FirstBootDevice $bootDisk -EnableSecureBoot Off -ErrorAction Stop"
				: string.Empty;
			string startCommand = startAfterCreate
				? $"; Start-VM -Name '{escapedVmName}' -ErrorAction Stop | Out-Null"
				: string.Empty;

			return $"$vmName='{escapedVmName}'; $vmPath='{escapedVmStoragePath}'; $diskPath='{escapedVirtualDiskPath}'; if ([string]::IsNullOrWhiteSpace($vmName)) {{ throw 'A virtual machine name is required.'; }}; if ([string]::IsNullOrWhiteSpace($vmPath)) {{ throw 'A virtual machine storage path is required.'; }}; if ([string]::IsNullOrWhiteSpace($diskPath)) {{ throw 'A Hyper-V virtual disk path is required.'; }}; New-Item -ItemType Directory -Path $vmPath -Force | Out-Null; if (Get-VM -Name $vmName -ErrorAction SilentlyContinue) {{ throw \"A Hyper-V virtual machine named '$vmName' already exists.\"; }}; New-VM -Name $vmName -Generation {generation} -Path $vmPath -MemoryStartupBytes 2GB -ErrorAction Stop | Out-Null; Add-VMHardDiskDrive -VMName $vmName -ControllerType {controllerType} -ControllerNumber 0 -ControllerLocation 0 -Path $diskPath -ErrorAction Stop | Out-Null{firmwareCommand}{startCommand}";
		}

		public static string BuildRegenerateMacAddressScript(string vmName)
		{
			string escapedVmName = EscapePowerShellSingleQuotedString(vmName);
			return $"$vmName='{escapedVmName}'; $vm = Get-VM -Name $vmName -ErrorAction Stop; if ($vm.State -notin @('Off','Saved')) {{ throw 'The Hyper-V virtual machine must be off before regenerating the MAC address.'; }}; Get-VMNetworkAdapter -VMName $vmName -ErrorAction Stop | ForEach-Object {{ Set-VMNetworkAdapter -VMName $vmName -Name $_.Name -DynamicMacAddress -ErrorAction Stop | Out-Null }}";
		}

		private static string EscapePowerShellSingleQuotedString(string value)
		{
			return (value ?? string.Empty).Replace("'", "''", StringComparison.Ordinal);
		}

		private static string SanitizeFileName(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			char[] invalidChars = Path.GetInvalidFileNameChars();
			var builder = new StringBuilder(value.Length);

			foreach (char character in value.Trim())
			{
				builder.Append(invalidChars.Contains(character) ? '_' : character);
			}

			return builder.ToString().Trim().TrimEnd('.');
		}
	}
}
