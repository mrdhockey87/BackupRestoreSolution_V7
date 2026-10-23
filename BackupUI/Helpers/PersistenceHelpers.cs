using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureServerBackup.Helpers
{
	public static class PersistenceHelpers
	{
		private static bool IsSelectedFilesBackup(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);
			return job.Type == BackupType.SelectedFilesAndFolders;
		}

		public static void PersistSelectedFileList(string? previousJobName, BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			if (!IsSelectedFilesBackup(job))
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

			if (IsSelectedFilesBackup(job))
			{
				return GetSelectedFilesReplayPaths(job);
			}

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

			if (job.Target == BackupTarget.Disk || job.Target == BackupTarget.Volume || job.Target == BackupTarget.FilesAndFolders)
			{
				return job.SourcePaths
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

			if (!IsSelectedFilesBackup(job))
			{
				return job.SourcePaths;
			}

			List<string> persistedPaths = SelectedFileListStore.Load(job.Name);
			return persistedPaths.Count > 0 ? persistedPaths : job.SourcePaths;
		}

		public static IReadOnlyList<string> GetSelectedFilesReplayPaths(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			if (!IsSelectedFilesBackup(job))
			{
				return job.SourcePaths;
			}

			List<string> replayPaths = new();
			replayPaths.AddRange(job.SelectedFilesSourceRoots.Where(path => !string.IsNullOrWhiteSpace(path)));
			replayPaths.AddRange(LoadSelectedFileList(job));

			return replayPaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		}
	}
}
