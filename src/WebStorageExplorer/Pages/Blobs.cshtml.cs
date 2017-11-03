using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageLibrary;

namespace WebStorageExplorer.Pages
{
    public class BlobsModel : PageModel
    {
		public List<CloudBlobContainer> Containers { get; set; }

		public List<IListBlobItem> Blobs { get; set; }

		public async Task OnGetAsync()
        {
			Containers = await Container.ListContainersAsync(Settings.Instance.Account, Settings.Instance.Key);
        }

		public async Task<IActionResult> OnPostAsync(string container)
		{

			Blobs = await Container.ListBlobsAsync(Settings.Instance.Account, Settings.Instance.Key, container);

			return RedirectToPage();
		}
		
    }
}