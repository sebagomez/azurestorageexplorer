using System;
using System.Collections.Generic;

namespace StorageLibrary.Mocks
{
	public class MockUtils
	{
		public static readonly string FAKE_URL = "https://this.is.fake";

		static Random s_rand = new Random();

		public static long NewRandomSize { get => s_rand.NextInt64(512, 5 * 1024 * 1024); } // between 512 bytes and 5 Megs

		static Dictionary<string, List<string>> s_folderStructure = new Dictionary<string, List<string>>();

		static MockUtils()
		{
			Reintialize();
		}

		public static void Reintialize()
		{
			s_folderStructure = new Dictionary<string, List<string>>()
			{
				{ "one", new List<string> {"fromOne:1", "fromOne:2", "fromOne:3"}},
				{ "two", new List<string> {"fromTwo:1", "fromTwo:2"}},
				{ "three", new List<string> {"fromThree:1"}},
				{ "empty", new List<string> {}},
				{ "with-folder", new List<string> {"root-file", "folder1/", "folder1/file"}},
				{ "with-many-folders", new List<string> {"file-at-root", "folder1/", "folder1/file1", "folder1/folder11/", "folder1/folder11/file-at-11", "folder1/folder11/another-file-at-11"}},
				{ "brothers", new List<string> {"ale/", "seba/", "ale/lauti", "ale/alfo", "ale/ciro", "seba/jose", "seba/juan", "seba/benja" }},
			};
		}

		public static Dictionary<string, List<string>> FolderStructure { get => s_folderStructure; }

		public static IEnumerable<string> GetItems(string key, string folder)
		{
			foreach (string val in MockUtils.FolderStructure[key])
			{
				int slash = val.LastIndexOf("/");

				string[] deep = folder.Split("/", StringSplitOptions.RemoveEmptyEntries);
				string[] dirs = val.Split("/", StringSplitOptions.RemoveEmptyEntries);

				if (!string.IsNullOrEmpty(folder))
				{
					if (val == folder)
						continue;

					if (dirs.Length > (deep.Length + 1) || dirs.Length <= deep.Length)
						continue;

					bool inCurrentDir = true;
					if (dirs.Length >= deep.Length)
					{
						for (int i = 0; i < dirs.Length - 1; i++)
						{
							if (dirs[i] != deep[i])
							{
								inCurrentDir &= false;
								break;
							}
						}
					}

					if (inCurrentDir)
						yield return $"{MockUtils.FAKE_URL}/{key}/{val}";
				}
				else
				{
					if (dirs.Length > 1)
						continue;

					yield return $"{MockUtils.FAKE_URL}/{key}/{val}";
				}
			}
		}
	}
}