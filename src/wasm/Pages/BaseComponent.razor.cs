using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace wasm.Pages
{
	public partial class BaseComponent
	{
		public string? ErrorMessage {get;set;}

		public bool HasError {get;set;} = false;

		public bool Loading {get;set;} = false;

		[Parameter]
		public string? Selected { get; set; }

	}
}