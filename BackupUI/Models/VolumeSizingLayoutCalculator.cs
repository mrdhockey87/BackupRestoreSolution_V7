using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureServerBackup.Models
{
	public sealed record VolumeSizingLayout(long[] CurrentSizes, long[] MinimumSizes, long[] MaximumSizes);

	public static class VolumeSizingLayoutCalculator
	{
		public static bool CanVolumeBeResizedForLayout(VolumeInfo volume)
		{
			ArgumentNullException.ThrowIfNull(volume);

			if (!volume.FileSystem.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}

			double freePercentage = (double)volume.FreeSpace / volume.Size * 100;
			return freePercentage >= 10;
		}

		public static long CalculateMinimumSizeForLayout(VolumeInfo volume, int sourceAllocationUnitSize, int targetAllocationUnitSize)
		{
			ArgumentNullException.ThrowIfNull(volume);

			long actualUsedSpace = CalculateActualUsedSpaceForLayout(volume.UsedSpace, sourceAllocationUnitSize, targetAllocationUnitSize);
			return (long)(actualUsedSpace * 1.10);
		}

		public static long CalculateActualUsedSpaceForLayout(long usedSpace, int sourceAllocationUnitSize, int targetAllocationUnitSize)
		{
			if (sourceAllocationUnitSize == targetAllocationUnitSize)
			{
				return usedSpace;
			}

			long sourceUnits = (usedSpace + sourceAllocationUnitSize - 1) / sourceAllocationUnitSize;
			return sourceUnits * targetAllocationUnitSize;
		}

		public static VolumeSizingLayout CalculateDefaultLayout(IReadOnlyList<VolumeInfo> sourceVolumes, long targetTotalSize, int sourceAllocationUnitSize, int targetAllocationUnitSize)
		{
			ArgumentNullException.ThrowIfNull(sourceVolumes);
			if (targetTotalSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(targetTotalSize));
			}

			long[] currentSizes = new long[sourceVolumes.Count];
			long[] minimumSizes = new long[sourceVolumes.Count];
			long[] maximumSizes = new long[sourceVolumes.Count];
			bool[] resizable = new bool[sourceVolumes.Count];

			for (int i = 0; i < sourceVolumes.Count; i++)
			{
				VolumeInfo volume = sourceVolumes[i];
				resizable[i] = CanVolumeBeResizedForLayout(volume);
				minimumSizes[i] = resizable[i]
					? CalculateMinimumSizeForLayout(volume, sourceAllocationUnitSize, targetAllocationUnitSize)
					: volume.Size;
				currentSizes[i] = minimumSizes[i];
			}

			long remainingSpace = targetTotalSize - currentSizes.Sum();
			int preferredGrowthIndex = GetPreferredGrowthVolumeIndex(sourceVolumes, resizable);
			if (remainingSpace > 0 && preferredGrowthIndex >= 0)
			{
				currentSizes[preferredGrowthIndex] += remainingSpace;
			}

			for (int i = 0; i < sourceVolumes.Count; i++)
			{
				if (!resizable[i])
				{
					maximumSizes[i] = sourceVolumes[i].Size;
					currentSizes[i] = sourceVolumes[i].Size;
					minimumSizes[i] = sourceVolumes[i].Size;
					continue;
				}

				long reservedSpace = 0;
				for (int j = 0; j < sourceVolumes.Count; j++)
				{
					if (i == j)
					{
						continue;
					}

					reservedSpace += resizable[j] ? minimumSizes[j] : sourceVolumes[j].Size;
				}

				maximumSizes[i] = Math.Max(minimumSizes[i], targetTotalSize - reservedSpace);
				currentSizes[i] = Math.Min(currentSizes[i], maximumSizes[i]);
			}

			return new VolumeSizingLayout(currentSizes, minimumSizes, maximumSizes);
		}

		private static int GetPreferredGrowthVolumeIndex(IReadOnlyList<VolumeInfo> sourceVolumes, IReadOnlyList<bool> resizable)
		{
			List<int> resizableIndexes = Enumerable.Range(0, sourceVolumes.Count)
				.Where(index => resizable[index])
				.ToList();

			if (resizableIndexes.Count == 0)
			{
				return -1;
			}

			List<int> bootOrSystemIndexes = resizableIndexes
				.Where(index => sourceVolumes[index].IsBootVolume || sourceVolumes[index].IsSystemVolume)
				.ToList();

			List<int> candidateIndexes = bootOrSystemIndexes.Count > 0 ? bootOrSystemIndexes : resizableIndexes;
			return candidateIndexes
				.OrderByDescending(index => sourceVolumes[index].Size)
				.First();
		}
	}
}
