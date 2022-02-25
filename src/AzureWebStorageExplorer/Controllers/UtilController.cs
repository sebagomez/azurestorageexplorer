using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace AzureWebStorageExplorer.Controllers
{
	[Produces("text/plain")]
	[Route("api/Util")]
	public class UtilController : Controller
	{
		[HttpGet("[action]")]
		public string GetVersion(string account, string key)
		{
			string? envVer = Environment.GetEnvironmentVariable("APPVERSION");
			if (!(envVer is null))
				return envVer;

			Assembly assembly = Assembly.GetExecutingAssembly();
			Version? assVer = assembly.GetName().Version;
			return $"{assVer?.Major}.{assVer?.Minor}.{assVer?.Build}";
		}
	}
}
