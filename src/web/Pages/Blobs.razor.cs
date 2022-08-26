using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;
using StorageLibrary.Common;
using web.Utils;

namespace web.Pages
{
	public partial class Blobs : BaseComponent
	{
		[Parameter]
		public string? CurrentContainer { get; set; }
	}
}