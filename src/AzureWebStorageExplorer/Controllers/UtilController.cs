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
			string envVer = System.Environment.GetEnvironmentVariable("APPVERSION");
			if (!string.IsNullOrEmpty(envVer))
				return envVer;

			Assembly assembly = Assembly.GetExecutingAssembly();
			return assembly.GetName().Version.ToString();
		}
	}
}
