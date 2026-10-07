using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SecureServerBackupCommon
{
	public static class SavedNetworkPathStore
	{
		private static readonly string JobDataDirectory = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
			"SecureServerBackupService");

		private static string StoreFilePath => Path.Combine(JobDataDirectory, "SavedNetworkPaths.json");

		public static List<string> Load()
		{
			if (!File.Exists(StoreFilePath))
			{
				return new List<string>();
			}

			string json = File.ReadAllText(StoreFilePath);
			return JsonSerializer.Deserialize<List<string>>(json)
				?.Where(path => !string.IsNullOrWhiteSpace(path))
				.Select(path => path.Trim())
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList() ?? new List<string>();
		}

		public static void Save(IEnumerable<string> networkPaths)
		{
			ArgumentNullException.ThrowIfNull(networkPaths);

			Directory.CreateDirectory(JobDataDirectory);

			List<string> normalizedPaths = networkPaths
				.Where(path => !string.IsNullOrWhiteSpace(path))
				.Select(path => path.Trim())
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();

			string json = JsonSerializer.Serialize(normalizedPaths, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(StoreFilePath, json);
		}

		public static void Add(string networkPath)
		{
			if (string.IsNullOrWhiteSpace(networkPath))
			{
				return;
			}

			List<string> networkPaths = Load();
			if (!networkPaths.Contains(networkPath.Trim(), StringComparer.OrdinalIgnoreCase))
			{
				networkPaths.Add(networkPath.Trim());
				Save(networkPaths);
			}
		}

		public static void Remove(string networkPath)
		{
			if (string.IsNullOrWhiteSpace(networkPath))
			{
				return;
			}

			List<string> networkPaths = Load();
			networkPaths.RemoveAll(path => string.Equals(path, networkPath.Trim(), StringComparison.OrdinalIgnoreCase));
			Save(networkPaths);
		}
	}
}