using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SecureServerBackup.Helpers
{
	public static class PersistenceHelpers
	{
		public static bool IsSelectedFilesBackup(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);
			return job.Type == BackupType.SelectedFilesAndFolders ||
				   job.Target == BackupTarget.FilesAndFolders;
		}

		public static void PersistSelectedFileList(string? previousJobName, BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);
			bool isFileSelectionJob = IsSelectedFilesBackup(job);
			if (!isFileSelectionJob)
			{
				if (!string.IsNullOrWhiteSpace(previousJobName))
				{
					SelectedFileListStore.Delete(previousJobName);
				}

				SelectedFileListStore.Delete(job.Name);
				return;
			}

			if (!string.IsNullOrWhiteSpace(previousJobName) &&
				!string.Equals(previousJobName, job.Name, StringComparison.OrdinalIgnoreCase))
			{
				SelectedFileListStore.Delete(previousJobName);
			}

			SelectedFileListStore.Save(job.Name, job.SourcePaths);
		}

		public static IReadOnlyList<string> GetReplayPathsForJob(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			bool selectedFilesJob = IsSelectedFilesBackup(job);

			if (job.Type == BackupType.CloneHyperVSystem || job.Type == BackupType.ExportHyperVSystem)
			{
				IReadOnlyList<string> cloneHyperVPaths = job.HyperVMachines.Count > 0
					? job.HyperVMachines
						.Select(HyperVBackupTreeHelper.NormalizeSavedHyperVSystemName)
						.Where(path => !string.IsNullOrWhiteSpace(path))
						.ToList()
					: job.SourcePaths;

				return cloneHyperVPaths
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();
			}

			if (job.Target == BackupTarget.Disk || job.Target == BackupTarget.Volume || job.Target == BackupTarget.FilesAndFolders || selectedFilesJob)
			{
				return IsSelectedFilesBackup(job)
					? GetSelectedFilesReplayPaths(job)
					: job.SourcePaths
						.Where(path => !string.IsNullOrWhiteSpace(path))
						.Distinct(StringComparer.OrdinalIgnoreCase)
						.ToList();
			}


			if (job.IsHyperVBackup)
			{
				return job.HyperVMachines
					.Where(path => !string.IsNullOrWhiteSpace(path))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();
			}

			return Array.Empty<string>();
		}

		public static IReadOnlyList<string> LoadSelectedFileList(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			bool isFileSelectionJob = IsSelectedFilesBackup(job);
			if (!isFileSelectionJob)
			{
				return job.SourcePaths;
			}

			List<string> persistedPaths = SelectedFileListStore.Load(job.Name);
			return persistedPaths.Count > 0 ? persistedPaths : job.SourcePaths;
		}

		public static IReadOnlyList<string> GetSelectedFilesReplayPaths(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			bool isFileSelectionJob = IsSelectedFilesBackup(job);
			if (!isFileSelectionJob)
			{
				return job.SourcePaths;
			}

			List<string> replayPaths = SelectedFileListStore.Load(job.Name);
			if (replayPaths.Count == 0)
			{
				replayPaths = LoadSelectedFileList(job).ToList();
			}

			replayPaths = replayPaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.Select(PreSelectHelpers.NormalizeSelectionPath)
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.ToList();

			List<string> exactPaths = replayPaths
				.Where(path => !IsSelectedFilesVolumeRoot(path, replayPaths))
				.ToList();

			if (exactPaths.Count > 0)
			{
				return exactPaths;
			}

			return replayPaths.Count > 0 ? replayPaths : job.SourcePaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		private static bool IsSelectedFilesVolumeRoot(string candidatePath, IReadOnlyCollection<string> allPaths)
		{
			if (string.IsNullOrWhiteSpace(candidatePath))
			{
				return false;
			}

			string normalizedCandidate = PreSelectHelpers.NormalizeSelectionPath(candidatePath);
			if (string.IsNullOrWhiteSpace(normalizedCandidate))
			{
				return false;
			}

			string rootPath = Path.GetPathRoot(normalizedCandidate) ?? normalizedCandidate;
			string candidateRoot = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			string candidateWithoutTrailingSlash = normalizedCandidate.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			bool isVolumeRoot = string.Equals(candidateRoot, candidateWithoutTrailingSlash, StringComparison.OrdinalIgnoreCase);
			if (!isVolumeRoot)
			{
				return false;
			}

			return allPaths.Any(path =>
				!string.Equals(path, candidatePath, StringComparison.OrdinalIgnoreCase) &&
				!string.IsNullOrWhiteSpace(path) &&
				PreSelectHelpers.NormalizeSelectionPath(path).StartsWith(rootPath, StringComparison.OrdinalIgnoreCase));
		}

	}
}
