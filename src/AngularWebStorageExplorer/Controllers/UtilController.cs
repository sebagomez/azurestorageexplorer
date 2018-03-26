using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace AngularWebStorageExplorer.Controllers
{
	[Produces("text/plain")]
	[Route("api/Util")]
	public class UtilController : Controller
	{
		[HttpGet("[action]")]
		public string GetVersion(string account, string key)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			return assembly.GetName().Version.ToString();
		}
	}
}
