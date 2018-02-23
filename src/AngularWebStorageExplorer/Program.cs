using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AngularWebStorageExplorer
{
	public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
				//.UseKestrel(options => options.Listen(IPAddress.Any,443))
                .UseStartup<Startup>()
                .Build();
    }
}
