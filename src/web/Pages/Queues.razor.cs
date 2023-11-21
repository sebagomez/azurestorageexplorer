using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Prometheus;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Queues : BaseComponent
	{
		public string? NewQueueName { get; set; }

		[Parameter]
		public string? SelectedQueue { get; set; }
		private Dictionary<string, Dictionary<string, object>> QueuesAtts = new Dictionary<string, Dictionary<string, object>>();
		List<QueueWrapper> AzureQueues = new List<QueueWrapper>();

		private static readonly Counter QueueCounter = Metrics.CreateCounter("queuescontroller_counter_total", "Keep QueuesController access count");

		protected override async Task OnInitializedAsync()
		{	
			Increment(QueueCounter);
			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			await LoadQueues();
		}

		private async Task LoadQueues()
		{
			AzureQueues.Clear();
			QueuesAtts.Clear();
			foreach(QueueWrapper queue in (await AzureStorage!.Queues.ListQueuesAsync()).OrderBy(q => q.Name))
			{
				QueuesAtts[queue.Name] = new Dictionary<string, object>();
				AzureQueues.Add(queue);
			}
		}

		public async Task NewQueue()
		{
			if (string.IsNullOrWhiteSpace(NewQueueName))
				return;

			try
			{
				await AzureStorage!.Queues.CreateAsync(NewQueueName);
				NewQueueName = string.Empty;
				await LoadQueues();

			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void SelectedChanged(MouseEventArgs e, string queue)
		{
			if (SelectedQueue == queue)
				return;

			if ((!string.IsNullOrEmpty(SelectedQueue)) && QueuesAtts[SelectedQueue].ContainsKey("style"))
			{
				QueuesAtts[SelectedQueue].Remove("style");
				QueuesAtts[SelectedQueue]["class"] = "item";
			}
			SelectedQueue = queue;

			QueuesAtts[SelectedQueue]["class"] = "item active";
			QueuesAtts[SelectedQueue]["style"] = "font-weight: bold;";

			StateHasChanged();
		}

		public override async Task SelectionDeletedAsync()
		{
			SelectedQueue = null;
			await LoadQueues();

			await base.SelectionDeletedAsync();
		}
	}
}