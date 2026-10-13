using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public static class PersistenceHelpers
	{
		public static void PersistSelectedFileList(string? previousJobName, BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			bool isFileSelectionJob = job.Target == BackupTarget.FilesAndFolders || job.Type == BackupType.SelectedFilesAndFolders;
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

		public static IReadOnlyList<string> LoadSelectedFileList(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			if (job.Type != BackupType.SelectedFilesAndFolders || job.Target != BackupTarget.FilesAndFolders)
			{
				return job.SourcePaths;
			}

			List<string> persistedPaths = SelectedFileListStore.Load(job.Name);
			return persistedPaths.Count > 0 ? persistedPaths : job.SourcePaths;
		}

		public static IReadOnlyList<string> GetSelectedFilesReplayPaths(BackupJob job)
		{
			ArgumentNullException.ThrowIfNull(job);

			if (job.Type != BackupType.SelectedFilesAndFolders || job.Target != BackupTarget.FilesAndFolders)
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