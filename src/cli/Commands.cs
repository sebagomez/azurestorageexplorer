using System.Globalization;
using System.Text.RegularExpressions;

using SkyFs.Stores;

using StorageLibrary;
using StorageLibrary.Common;

namespace SkyFs;

/// <summary>
/// The behavior of each command, apart from argument parsing. Every method returns the
/// process exit code: 0 for success, 1 when anything failed.
/// </summary>
internal sealed partial class Commands(ProviderRegistry providers, TextWriter output, TextWriter error, Func<string, bool> confirm)
{
	public int Providers()
	{
		foreach ((string scheme, CloudProvider provider) in Location.Schemes)
		{
			string? description = providers.Describe(provider);
			string state = description is null
				? $"not configured (set {ProviderRegistry.Requirements(provider)})"
				: $"configured ({description})";
			output.WriteLine($"{scheme,-6} {provider,-6} {state}");
		}
		return 0;
	}

	public Task<int> ListAsync(string? target, bool recursive) => Run(async () =>
	{
		if (string.IsNullOrEmpty(target))
		{
			if (!providers.Configured.Any())
				throw new SkyFsException("No provider is configured. Run 'skyfs providers' to see what to set.");

			foreach (CloudProvider provider in providers.Configured)
				await ListContainersAsync(provider);
			return 0;
		}

		Location location = Location.Parse(target);
		if (location.IsCloud && location.Container.Length == 0)
		{
			await ListContainersAsync(location.Provider!.Value);
			return 0;
		}

		IStore store = providers.Resolve(location);
		switch (await store.GetKindAsync(location.Path))
		{
			case ItemKind.None:
				throw new SkyFsException($"'{target}' does not exist");
			case ItemKind.File:
				output.WriteLine(Format(new StoreEntry(store.FileName(location.Path), false, await store.GetFileSizeAsync(location.Path))));
				break;
			default:
				foreach (StoreEntry entry in await store.ListAsync(location.Path, recursive))
					output.WriteLine(Format(entry));
				break;
		}
		return 0;
	});

	public Task<int> CopyAsync(string source, string destination, bool recursive, bool force, bool move) => Run(async () =>
	{
		Location from = Location.Parse(source);
		Location to = Location.Parse(destination);
		IStore fromStore = providers.Resolve(from);
		IStore toStore = providers.Resolve(to);

		List<(string From, string To)> plan = [];
		ItemKind kind = await fromStore.GetKindAsync(from.Path);
		switch (kind)
		{
			case ItemKind.None:
				throw new SkyFsException($"'{source}' does not exist");

			case ItemKind.File:
				string target = await toStore.IsFolderTargetAsync(to.Path)
					? toStore.Combine(to.Path, fromStore.FileName(from.Path))
					: to.Path;
				plan.Add((from.Path, target));
				break;

			case ItemKind.Folder:
				if (!recursive)
					throw new SkyFsException($"'{source}' is a folder. Use --recursive to copy its contents.");

				// The contents of the source folder go into the destination folder. The
				// whole list is read first, so a copy into a subfolder of the source
				// does not also copy its own output.
				foreach (StoreEntry entry in await fromStore.ListAsync(from.Path, recursive: true))
					plan.Add((fromStore.Combine(from.Path, entry.Name), toStore.Combine(to.Path, entry.Name)));
				break;
		}

		string verb = move ? "moved" : "copied";
		int failed = 0;
		foreach ((string fromKey, string toKey) in plan)
		{
			try
			{
				await CopyOneAsync(fromStore, fromKey, toStore, toKey, force, move);
				output.WriteLine($"{verb}: {fromStore.Display(fromKey)} -> {toStore.Display(toKey)}");
			}
			catch (Exception ex)
			{
				failed++;
				ReportError(ex);
			}
		}

		if (move && kind == ItemKind.Folder)
			await fromStore.RemoveEmptyFoldersAsync(from.Path);

		if (plan.Count > 1)
			output.WriteLine($"{plan.Count - failed} {verb}, {failed} failed");

		return failed == 0 ? 0 : 1;
	});

	public Task<int> RemoveAsync(string target, bool recursive, bool yes) => Run(async () =>
	{
		Location location = Location.Parse(target);
		IStore store = providers.Resolve(location);

		switch (await store.GetKindAsync(location.Path))
		{
			case ItemKind.None:
				throw new SkyFsException($"'{target}' does not exist");

			case ItemKind.File:
				await store.DeleteFileAsync(location.Path);
				output.WriteLine($"deleted: {store.Display(location.Path)}");
				return 0;
		}

		if (!recursive)
			throw new SkyFsException($"'{target}' is a folder. Use --recursive to delete it and all its contents.");

		List<StoreEntry> files = await store.ListAsync(location.Path, recursive: true);
		if (!yes && !confirm($"Delete {files.Count} file(s) under '{target}'?"))
		{
			error.WriteLine("Canceled. Nothing was deleted.");
			return 1;
		}

		int failed = 0;
		foreach (StoreEntry file in files)
		{
			string key = store.Combine(location.Path, file.Name);
			try
			{
				await store.DeleteFileAsync(key);
				output.WriteLine($"deleted: {store.Display(key)}");
			}
			catch (Exception ex)
			{
				failed++;
				ReportError(ex);
			}
		}

		await store.RemoveEmptyFoldersAsync(location.Path);
		output.WriteLine($"{files.Count - failed} deleted, {failed} failed");
		return failed == 0 ? 0 : 1;
	});

	static async Task CopyOneAsync(IStore fromStore, string fromKey, IStore toStore, string toKey, bool force, bool move)
	{
		if (fromStore.Identity == toStore.Identity && fromStore.NormalizeKey(fromKey) == toStore.NormalizeKey(toKey))
			throw new SkyFsException($"'{fromStore.Display(fromKey)}' is both the source and the destination");

		// Fail early, before a possibly large download. The write itself still refuses
		// to overwrite, so this check cannot let a race replace a file.
		if (!force && await toStore.GetKindAsync(toKey) == ItemKind.File)
			throw new TargetExistsException(toStore.Display(toKey));

		await using (Stream content = await fromStore.OpenReadAsync(fromKey))
			await toStore.WriteAsync(toKey, content, force);

		if (move)
			await fromStore.DeleteFileAsync(fromKey);
	}

	async Task ListContainersAsync(CloudProvider provider)
	{
		string scheme = Location.SchemeOf(provider);
		foreach (CloudBlobContainerWrapper container in await providers.Get(provider).ListContainersAsync())
			output.WriteLine($"{scheme}{container.Name}/");
	}

	static string Format(StoreEntry entry) =>
		$"{(entry.IsFolder ? "<DIR>" : FormatSize(entry.Size)),10}  {entry.Name}";

	internal static string FormatSize(long bytes)
	{
		string[] units = ["B", "KB", "MB", "GB", "TB"];
		double size = bytes;
		int unit = 0;
		while (size >= 1024 && unit < units.Length - 1)
		{
			size /= 1024;
			unit++;
		}
		return unit == 0 ? $"{bytes} B" : string.Create(CultureInfo.InvariantCulture, $"{size:0.0} {units[unit]}");
	}

	async Task<int> Run(Func<Task<int>> action)
	{
		try
		{
			return await action();
		}
		catch (Exception ex)
		{
			ReportError(ex);
			return 1;
		}
	}

	void ReportError(Exception ex) => error.WriteLine($"error: {RedactSignatures(ex.Message)}");

	/// <summary>
	/// A SAS in a URL is a credential, and storage exceptions often include the request
	/// URL. Same rule as the web app's Util.RedactSignatures.
	/// </summary>
	internal static string RedactSignatures(string text) =>
		string.IsNullOrEmpty(text) ? text : SignatureRegex().Replace(text, "REDACTED");

	[GeneratedRegex(@"(?<=[?&]sig=)[^&\s""']+", RegexOptions.IgnoreCase)]
	private static partial Regex SignatureRegex();
}
