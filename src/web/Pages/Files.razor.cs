using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Files : BaseComponent
	{
		[Parameter]
		public string CurrentFileShare { get; set; }
	}
}