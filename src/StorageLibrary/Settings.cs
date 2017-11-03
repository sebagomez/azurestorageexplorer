using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace StorageLibrary
{
	public class Settings
	{
		static string s_settingsFile = "azure.data";

		static Settings s_instance;

		public string Account { get; set; }
		public string Key { get; set; }
		public string RefFile { get; set; }

		public static Settings Instance
		{
			get { return s_instance; }
		}

		static Settings()
		{
			try
			{
				s_instance = GetData(GetFullPath(s_settingsFile));
				while (s_instance != null && string.IsNullOrEmpty(s_instance.Account) && string.IsNullOrEmpty(s_instance.Key) && !string.IsNullOrEmpty(s_instance.RefFile) && File.Exists(GetFullPath(s_instance.RefFile)))
					s_instance = GetData(GetFullPath(s_instance.RefFile));
			}
			catch { }
		}

		static string GetFullPath(string fileName)
		{
			return Path.Combine(AppContext.BaseDirectory, fileName);
		}

		static Settings GetData(string fileName)
		{
			using (FileStream file = File.Open(fileName, FileMode.Open))
			{
				DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Settings));
				return (Settings)jsonSerializer.ReadObject(file);
			}
		}
	}
}
