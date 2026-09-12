using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SecureServerBackup.Models;
using SecureServerBackup.Services;
using BackupEngineInterop = SecureServerBackup.Services.BackupEngineInterop;

namespace SecureServerBackup.Windows
{
	internal static class RestoreWorkflowHelper
	{
		internal static IReadOnlyList<RestorePoint> GetRestorePointsForBackup(string backupPath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);

			IReadOnlyList<string> backupItems = GetBackupItemsForArchive(backupPath);
			string backupType = DetermineBackupTypeForArchive(backupPath, backupItems);
			IReadOnlyList<RestorePointArchiveImage> archiveImages = GetRestorePointArchiveImages(backupPath);
			bool forceSingleRestorePoint = ShouldTreatBackupAsSingleFileRestorePoint(backupPath, backupItems);
			DateTime timestamp = GetRestorePointTimestamp(backupPath, archiveImages);

			return CreateRestorePointsForBackupFile(
				backupPath,
				backupType,
				timestamp,
				startingPointNumber: 1,
				archiveImages,
				forceSingleRestorePoint);
		}

		internal static string DetermineBackupTypeForArchive(string backupPath, IReadOnlyList<string> backupItems)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);
			ArgumentNullException.ThrowIfNull(backupItems);

			return ShouldTreatBackupAsSingleFileRestorePoint(backupPath, backupItems)
				? "Selected Files"
				: DetermineBackupTypeForPath(backupPath);
		}

		internal static IReadOnlyList<string> GetBackupItemsForRestorePoint(RestorePoint restorePoint)
		{
			ArgumentNullException.ThrowIfNull(restorePoint);

			try
			{
				using var preparedBackup = EncryptedBackupFileService.PrepareForRead(null, restorePoint.FilePath, Path.GetFileNameWithoutExtension(restorePoint.FilePath));
				var buffer = new StringBuilder(32768);
				int result = BackupEngineInterop.ListBackupContents(preparedBackup.WorkingPath, buffer, buffer.Capacity);
				if (result != 0)
				{
					return Array.Empty<string>();
				}

				return ParseListedBackupItems(buffer.ToString());
			}
			catch
			{
				return Array.Empty<string>();
			}
		}

		internal static IReadOnlyList<VolumeInfo> GetBackupVolumesForRestorePoint(RestorePoint restorePoint)
		{
			ArgumentNullException.ThrowIfNull(restorePoint);

			try
			{
				var imagesWithMetadata = NativeBackupMountManager.GetImageInfoWithRestoreMetadata(restorePoint.FilePath);
				if (!imagesWithMetadata.Success || imagesWithMetadata.Images.Count == 0)
				{
					return Array.Empty<VolumeInfo>();
				}

				return imagesWithMetadata.Images
					.Where(image => image.ImageIndex > 0)
					.Select((image, idx) => new VolumeInfo
					{
						ImageIndex = image.ImageIndex,
						Label = !string.IsNullOrWhiteSpace(image.RestoreMetadata?.SourceVolumeLabel)
							? image.RestoreMetadata.SourceVolumeLabel
							: (!string.IsNullOrWhiteSpace(image.Name) ? image.Name : $"Volume {idx + 1}"),
						Size = image.RestoreMetadata?.PartitionLengthBytes > 0
							? (long)image.RestoreMetadata.PartitionLengthBytes
							: 0,
						UsedSpace = image.RestoreMetadata?.SourceUsedSpaceBytes > 0
							? (long)image.RestoreMetadata.SourceUsedSpaceBytes
							: 0,
						PartitionNumber = image.RestoreMetadata?.PartitionNumber ?? 0,
						PartitionOffsetBytes = image.RestoreMetadata?.PartitionOffsetBytes ?? 0,
						PartitionLengthBytes = image.RestoreMetadata?.PartitionLengthBytes ?? 0,
						PartitionStyle = image.RestoreMetadata?.PartitionStyle ?? string.Empty,
						PartitionType = image.RestoreMetadata?.PartitionType ?? string.Empty,
						SourceVolumeGuidPath = image.RestoreMetadata?.SourceVolumeGuidPath ?? string.Empty,
						SourceVolumeMountPath = image.RestoreMetadata?.SourceVolumeMountPath ?? string.Empty,
						IsBootVolume = image.RestoreMetadata?.IsBootVolume == true,
						IsSystemVolume = image.RestoreMetadata?.IsSystemVolume == true,
						FileSystem = image.RestoreMetadata?.SourceFileSystem ?? string.Empty,
						IsResizable = true,
						TargetSize = image.RestoreMetadata?.PartitionLengthBytes > 0
							? (long)image.RestoreMetadata.PartitionLengthBytes
							: 0
					})
					.OrderBy(volume => volume.PartitionOffsetBytes)
					.ThenBy(volume => volume.PartitionNumber)
					.ToArray();
			}
			catch
			{
				return Array.Empty<VolumeInfo>();
			}
		}

		internal static bool IsSelectedFilesBackupArchive(string backupPath)
		{
			IReadOnlyList<string> backupItems = GetBackupItemsForArchive(backupPath);
			return ShouldTreatBackupAsSingleFileRestorePoint(backupPath, backupItems);
		}

		internal static bool ShouldTreatBackupAsSingleFileRestorePoint(string backupPath, IReadOnlyList<string> backupItems)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);
			ArgumentNullException.ThrowIfNull(backupItems);

			if (!string.Equals(Path.GetExtension(backupPath), ".ssb", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}

			string fileName = Path.GetFileName(backupPath);
			if (fileName.Contains("_SelectedFiles_", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}

			return backupItems.Count > 0 &&
				DetermineRestoreTargetKind("Unknown", backupPath, backupItems) == RestoreTargetKind.FileOrFolder;
		}

		internal static IReadOnlyList<RestorePoint> CreateRestorePointsForBackupFile(
			string backupPath,
			string backupType,
			DateTime timestamp,
			int startingPointNumber,
			IReadOnlyList<RestorePointArchiveImage>? archiveImages = null,
			bool forceSingleRestorePoint = false)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(backupPath);
			ArgumentException.ThrowIfNullOrWhiteSpace(backupType);

			var restorePointsForBackup = new List<RestorePoint>();
			IReadOnlyList<RestorePointArchiveImage> orderedArchiveImages = OrderRestorePointArchiveImages(archiveImages);

			if (forceSingleRestorePoint ||
				orderedArchiveImages.Count == 0 ||
				ShouldCollapseArchiveImagesToSingleRestorePoint(orderedArchiveImages))
			{
				restorePointsForBackup.Add(new RestorePoint
				{
					DisplayName = $"Point {startingPointNumber}: {backupType} Backup",
					Description = $"Created: {timestamp:yyyy-MM-dd HH:mm:ss}",
					BackupType = backupType,
					FilePath = backupPath,
					Timestamp = timestamp
				});

				return restorePointsForBackup;
			}

			int pointNumber = startingPointNumber;
			foreach (RestorePointArchiveImage archiveImage in orderedArchiveImages)
			{
				string volumeDisplay = GetRestorePointVolumeDisplayName(archiveImage);
				restorePointsForBackup.Add(new RestorePoint
				{
					DisplayName = $"Point {pointNumber}: {backupType} Backup - {volumeDisplay}",
					Description = $"Created: {timestamp:yyyy-MM-dd HH:mm:ss}",
					BackupType = backupType,
					FilePath = backupPath,
					Timestamp = timestamp,
					ImageIndex = archiveImage.ImageIndex
				});

				pointNumber++;
			}

			return restorePointsForBackup;
		}

		internal static RestoreTargetKind DetermineRestoreTargetKind(string? backupType, string? filePath, IEnumerable<string> backupItems)
		{
			ArgumentNullException.ThrowIfNull(backupItems);

			if (string.Equals(backupType, "Selected Files", StringComparison.OrdinalIgnoreCase))
			{
				return RestoreTargetKind.FileOrFolder;
			}

			bool hasItems = false;
			bool hasDisk = false;
			bool hasVolume = false;
			bool hasNonDiskOrVolumeItem = false;

			foreach (string itemText in backupItems.Where(item => !string.IsNullOrWhiteSpace(item)))
			{
				hasItems = true;

				if (IsDiskBackupSurface(itemText))
				{
					hasDisk = true;
					continue;
				}

				if (IsVolumeBackupSurface(itemText))
				{
					hasVolume = true;
					continue;
				}

				hasNonDiskOrVolumeItem = true;
				break;
			}

			if (hasNonDiskOrVolumeItem)
			{
				return RestoreTargetKind.FileOrFolder;
			}

			if (hasDisk)
			{
				return RestoreTargetKind.Disk;
			}

			if (hasVolume)
			{
				return RestoreTargetKind.Volume;
			}

			if (!hasItems)
			{
				string normalizedFilePath = filePath ?? string.Empty;
				if (normalizedFilePath.Contains("disk", StringComparison.OrdinalIgnoreCase) ||
					normalizedFilePath.Contains("drive", StringComparison.OrdinalIgnoreCase) ||
					normalizedFilePath.Contains("physical", StringComparison.OrdinalIgnoreCase))
				{
					return RestoreTargetKind.Disk;
				}

				if (normalizedFilePath.Contains("volume", StringComparison.OrdinalIgnoreCase) ||
					normalizedFilePath.Contains("partition", StringComparison.OrdinalIgnoreCase))
				{
					return RestoreTargetKind.Volume;
				}
			}

			return RestoreTargetKind.FileOrFolder;
		}

		internal static bool ShouldKeepRestoreCompletionWindowOpen(
			RestoreTargetKind restoreTargetKind,
			bool requireAlternateDestination,
			VolumeInfo? selectedRestoreVolume,
			IReadOnlyList<VolumeInfo>? selectedRestoreDiskGroup)
		{
			if (restoreTargetKind == RestoreTargetKind.FileOrFolder)
			{
				return requireAlternateDestination;
			}

			if (selectedRestoreVolume?.IsBootVolume == true)
			{
				return true;
			}

			return selectedRestoreDiskGroup?.Any(volume => volume.IsBootVolume) == true;
		}

		private static string DetermineBackupTypeForPath(string backupPath)
		{
			string fileName = Path.GetFileName(backupPath);
			if (fileName.Contains("_SelectedFiles_", StringComparison.OrdinalIgnoreCase))
			{
				return "Selected Files";
			}

			if (fileName.Contains("incremental", StringComparison.OrdinalIgnoreCase))
			{
				return "Incremental";
			}

			if (fileName.Contains("differential", StringComparison.OrdinalIgnoreCase))
			{
				return "Differential";
			}

			if (fileName.Contains("full", StringComparison.OrdinalIgnoreCase))
			{
				return "Full";
			}

			return "Unknown";
		}

		private static IReadOnlyList<string> GetBackupItemsForArchive(string backupPath)
		{
			if (string.IsNullOrWhiteSpace(backupPath))
			{
				return Array.Empty<string>();
			}

			try
			{
				using var preparedBackup = EncryptedBackupFileService.PrepareForRead(null, backupPath, Path.GetFileNameWithoutExtension(backupPath));
				var buffer = new StringBuilder(32768);
				int result = BackupEngineInterop.ListBackupContents(preparedBackup.WorkingPath, buffer, buffer.Capacity);
				if (result != 0)
				{
					return Array.Empty<string>();
				}

				return buffer.ToString()
					.Split('\n', StringSplitOptions.RemoveEmptyEntries)
					.Select(item => item.Trim())
					.Where(item => !string.IsNullOrWhiteSpace(item))
					.ToArray();
			}
			catch
			{
				return Array.Empty<string>();
			}
		}

		private static IReadOnlyList<RestorePointArchiveImage> GetRestorePointArchiveImages(string backupPath)
		{
			if (!string.Equals(Path.GetExtension(backupPath), ".ssb", StringComparison.OrdinalIgnoreCase))
			{
				return Array.Empty<RestorePointArchiveImage>();
			}

			try
			{
				var imagesWithMetadata = NativeBackupMountManager.GetImageInfoWithRestoreMetadata(backupPath);
				if (imagesWithMetadata.Success && imagesWithMetadata.Images.Count > 0)
				{
					return OrderRestorePointArchiveImages(imagesWithMetadata.Images.Select(image => new RestorePointArchiveImage
					{
						ImageIndex = image.ImageIndex,
						Name = image.Name,
						VolumeLabel = image.RestoreMetadata?.SourceVolumeLabel ?? string.Empty,
						SourceVolumeMountPath = image.RestoreMetadata?.SourceVolumeMountPath ?? string.Empty,
						PartitionOffsetBytes = image.RestoreMetadata?.PartitionOffsetBytes ?? 0,
						VolumeIndex = image.RestoreMetadata?.VolumeIndex ?? 0,
						BackupStartTime = image.RestoreMetadata?.BackupStartTime,
						CollapseToSingleRestorePoint = image.RestoreMetadata is not null
					}).ToList());
				}

				var images = NativeBackupMountManager.GetImageInfo(backupPath);
				if (images.Success && images.Images.Count > 0)
				{
					return OrderRestorePointArchiveImages(images.Images.Select(image => new RestorePointArchiveImage
					{
						ImageIndex = image.ImageIndex,
						Name = image.Name
					}).ToList());
				}
			}
			catch
			{
			}

			return Array.Empty<RestorePointArchiveImage>();
		}

		private static DateTime GetRestorePointTimestamp(string backupPath, IReadOnlyList<RestorePointArchiveImage> archiveImages)
		{
			if (TryGetBackupStartTime(backupPath, archiveImages, out DateTime backupStartTime))
			{
				return backupStartTime;
			}

			return GetEntryTimestamp(backupPath);
		}

		private static bool TryGetBackupStartTime(string backupPath, IReadOnlyList<RestorePointArchiveImage> archiveImages, out DateTime backupStartTime)
		{
			if (TryGetArchiveBackupStartTime(archiveImages, out backupStartTime))
			{
				return true;
			}

			return TryGetFileBackupStartTime(backupPath, out backupStartTime);
		}

		private static bool TryGetArchiveBackupStartTime(IReadOnlyList<RestorePointArchiveImage> archiveImages, out DateTime backupStartTime)
		{
			backupStartTime = default;

			if (archiveImages == null)
			{
				return false;
			}

			DateTime? earliestStartTime = archiveImages
				.Select(image => image.BackupStartTime)
				.Where(timestamp => timestamp.HasValue)
				.OrderBy(timestamp => timestamp)
				.FirstOrDefault();

			if (!earliestStartTime.HasValue)
			{
				return false;
			}

			backupStartTime = earliestStartTime.Value;
			return true;
		}

		private static bool TryGetFileBackupStartTime(string backupPath, out DateTime backupStartTime)
		{
			backupStartTime = default;

			string metadataPath = Path.Combine(backupPath, "backup_metadata.dat");
			if (!Directory.Exists(backupPath) || !File.Exists(metadataPath))
			{
				return false;
			}

			foreach (string line in File.ReadLines(metadataPath))
			{
				if (!line.StartsWith("#BACKUP_START_TIME|", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				string timestampText = line["#BACKUP_START_TIME|".Length..].Trim();
				if (DateTime.TryParse(timestampText, out backupStartTime))
				{
					return true;
				}

				return false;
			}

			return false;
		}

		private static IReadOnlyList<string> ParseListedBackupItems(string listedContents)
		{
			if (string.IsNullOrWhiteSpace(listedContents))
			{
				return Array.Empty<string>();
			}

			return listedContents
				.Split('\n', StringSplitOptions.RemoveEmptyEntries)
				.Select(item => item.Trim())
				.Where(item => !string.IsNullOrWhiteSpace(item) && !string.Equals(item, "(No files in backup)", StringComparison.OrdinalIgnoreCase))
				.ToArray();
		}

		private static DateTime GetEntryTimestamp(string path)
		{
			return File.Exists(path)
				? File.GetCreationTime(path)
				: Directory.GetCreationTime(path);
		}

		private static bool ShouldCollapseArchiveImagesToSingleRestorePoint(IReadOnlyList<RestorePointArchiveImage> archiveImages)
		{
			if (archiveImages == null || archiveImages.Count == 0)
			{
				return false;
			}

			return archiveImages.Any(image => image.CollapseToSingleRestorePoint);
		}

		private static IReadOnlyList<RestorePointArchiveImage> OrderRestorePointArchiveImages(IReadOnlyList<RestorePointArchiveImage>? archiveImages)
		{
			if (archiveImages == null || archiveImages.Count == 0)
			{
				return Array.Empty<RestorePointArchiveImage>();
			}

			return archiveImages
				.Where(image => image.ImageIndex > 0)
				.OrderBy(image => image.PartitionOffsetBytes)
				.ThenBy(image => image.VolumeIndex <= 0 ? int.MaxValue : image.VolumeIndex)
				.ThenBy(image => image.ImageIndex)
				.ToList();
		}

		private static string GetRestorePointVolumeDisplayName(RestorePointArchiveImage archiveImage)
		{
			ArgumentNullException.ThrowIfNull(archiveImage);

			if (!string.IsNullOrWhiteSpace(archiveImage.VolumeLabel))
			{
				return archiveImage.VolumeLabel.Trim();
			}

			if (!string.IsNullOrWhiteSpace(archiveImage.SourceVolumeMountPath))
			{
				return archiveImage.SourceVolumeMountPath.TrimEnd('\\');
			}

			if (archiveImage.VolumeIndex > 0)
			{
				return $"Volume {archiveImage.VolumeIndex}";
			}

			if (!string.IsNullOrWhiteSpace(archiveImage.Name))
			{
				return archiveImage.Name.Trim();
			}

			return $"Image {archiveImage.ImageIndex}";
		}

		private static bool IsDiskBackupSurface(string itemText)
		{
			return itemText.Contains("PHYSICALDRIVE", StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsVolumeBackupSurface(string itemText)
		{
			if (string.IsNullOrWhiteSpace(itemText))
			{
				return false;
			}

			string trimmed = itemText.Trim();
			return trimmed.StartsWith(@"\\?\Volume{", StringComparison.OrdinalIgnoreCase) ||
				   trimmed.StartsWith("\\?\\Volume{", StringComparison.OrdinalIgnoreCase) ||
				   trimmed.EndsWith(@":\", StringComparison.OrdinalIgnoreCase) ||
				   (trimmed.Length == 2 && char.IsLetter(trimmed[0]) && trimmed[1] == ':');
		}
	}
}
