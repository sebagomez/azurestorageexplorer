using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;

namespace StorageLibTests
{
	//[TestClass]
	public class BaseTests
	{
		public readonly string ACCOUNT = Settings.Instance.Account;
		public readonly string KEY = Settings.Instance.Key;

		internal string GetStringGuid()
		{
			return Guid.NewGuid().ToString().ToLower().Replace("-", "");
		}
	}
}
