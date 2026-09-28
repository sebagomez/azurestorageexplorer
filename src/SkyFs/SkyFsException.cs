namespace SkyFs;

/// <summary>An error with a message that is safe and useful to show as is.</summary>
internal class SkyFsException(string message) : Exception(message);

/// <summary>The target of a copy exists and the user did not give --force.</summary>
internal sealed class TargetExistsException(string target)
	: SkyFsException($"'{target}' already exists. Use --force to overwrite it.");
