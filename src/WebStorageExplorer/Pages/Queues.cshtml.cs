using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StorageLibrary;

namespace WebStorageExplorer.Pages
{
    public class QueuesModel : PageModel
    {
		public List<string> Queues { get; set; }
		public async Task OnGetAsync()
        {
			Queues = await Queue.ListQueuesAsync(Settings.Instance.Account, Settings.Instance.Key);
		}
    }
}