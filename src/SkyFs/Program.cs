using System.CommandLine;

using SkyFs;

using ProviderRegistry providers = ProviderRegistry.FromEnvironment(Environment.GetEnvironmentVariable);
Commands commands = new(providers, Console.Out, Console.Error, ConfirmOnConsole);

const string locationHelp = "az://<container>/<path>, s3://<bucket>/<path>, gs://<bucket>/<path>, or a local path";

Option<bool> Recursive(string description) => new("--recursive", "-r") { Description = description };
Option<bool> Force() => new("--force", "-f") { Description = "Overwrite files that already exist at the destination" };

Argument<string?> lsTarget = new("location")
{
	Description = $"What to list: {locationHelp}. Leave empty to list the containers of every configured provider.",
	Arity = ArgumentArity.ZeroOrOne,
};
Option<bool> lsRecursive = Recursive("List all files below the folder, not only one level");
Command ls = new("ls", "List containers, folders, and files") { lsTarget, lsRecursive };
ls.SetAction((result, _) => commands.ListAsync(result.GetValue(lsTarget), result.GetValue(lsRecursive)));

Command TransferCommand(string name, string description, bool move)
{
	Argument<string> source = new("source") { Description = locationHelp };
	Argument<string> destination = new("destination")
	{
		Description = $"{locationHelp}. End it with '/' to copy a file into that folder.",
	};
	Option<bool> recursive = Recursive("Copy the contents of a folder, with all its subfolders");
	Option<bool> force = Force();
	Command command = new(name, description) { source, destination, recursive, force };
	command.SetAction((result, _) => commands.CopyAsync(
		result.GetRequiredValue(source), result.GetRequiredValue(destination),
		result.GetValue(recursive), result.GetValue(force), move));
	return command;
}

Argument<string> rmTarget = new("location") { Description = $"What to delete: {locationHelp}" };
Option<bool> rmRecursive = Recursive("Delete a folder and all its contents");
Option<bool> rmYes = new("--yes", "-y") { Description = "Do not ask before a recursive delete" };
Command rm = new("rm", "Delete files, or a folder with --recursive") { rmTarget, rmRecursive, rmYes };
rm.SetAction((result, _) => commands.RemoveAsync(result.GetRequiredValue(rmTarget), result.GetValue(rmRecursive), result.GetValue(rmYes)));

Command providersCommand = new("providers", "Show which cloud providers are configured");
providersCommand.SetAction(_ => commands.Providers());

RootCommand root = new("skyfs: list, copy, move, and delete files on Azure Blob Storage, AWS S3, Google Cloud Storage, and the local disk")
{
	ls,
	TransferCommand("cp", "Copy files in any direction: local, Azure, AWS, and GCP", move: false),
	TransferCommand("mv", "Move files: copy them, then delete each source file after its copy succeeds", move: true),
	rm,
	providersCommand,
};

return await root.Parse(args).InvokeAsync();

static bool ConfirmOnConsole(string question)
{
	if (Console.IsInputRedirected)
	{
		Console.Error.WriteLine($"{question} Input is not a terminal, so use --yes to confirm.");
		return false;
	}

	Console.Error.Write($"{question} [y/N] ");
	string? answer = Console.ReadLine();
	return string.Equals(answer?.Trim(), "y", StringComparison.OrdinalIgnoreCase)
		|| string.Equals(answer?.Trim(), "yes", StringComparison.OrdinalIgnoreCase);
}
