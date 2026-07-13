using System.Reflection;
using web;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container (Blazor Web App / interactive server components).
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();

		// Razor Pages is kept only to serve the static /Error page. Interactivity is non-prerendered
		// (see App.razor), so a Blazor-routed error page would render blank until a circuit connects;
		// a plain Razor Page renders instantly even when the app is failing.
		builder.Services.AddRazorPages();

		var app = builder.Build();

		// Version pulled from the assembly (fed by AzureStorageWebExplorerVersion in Directory.Build.props).
		string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "unknown";

		// Expose the version on every response (all paths/status codes). OnStarting runs just before
		// the headers flush, so it survives the UseExceptionHandler re-execution on error responses.
		app.Use((ctx, next) =>
		{
			ctx.Response.OnStarting(() =>
			{
				ctx.Response.Headers["X-App-Version"] = version;
				return Task.CompletedTask;
			});
			return next();
		});

		string? basePath = Environment.GetEnvironmentVariable("BASEPATH");
		if (basePath is not null && !string.IsNullOrWhiteSpace(basePath))
		{
			if (!basePath.StartsWith("/"))
				basePath = $"/{basePath}";

			app.UsePathBase(basePath);
		}

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error", createScopeForErrors: true);
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseStaticFiles();

		app.UseAntiforgery();

		app.MapGet("/version", () => Results.Json(new { version }));

		app.MapRazorPages();

		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode();

		app.Run();
	}
}