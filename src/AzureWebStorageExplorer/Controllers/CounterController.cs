using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace AzureWebStorageExplorer.Controllers
{
	public class CounterController : Controller
	{
		public void Increment(Counter counter)
		{
			counter.Inc();
		}
	}
}
