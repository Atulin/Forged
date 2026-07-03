#:package Spectre.Console@0.57.0
#:package NuGet.Versioning@7.6.0
#:property PublishAot=false

using System.Text.RegularExpressions;
using NuGet.Versioning;
using Spectre.Console;

var path = args[0];

var file = File.ReadAllText(path);
var versionStr = Regexes.VersionRegex.Match(file).Groups["version"].Value;

if (!SemanticVersion.TryParse(versionStr, out var version))
{
	AnsiConsole.MarkupLine($"[red]Failed to parse version: {versionStr}[/]");
	return 1;
}

AnsiConsole.MarkupLine($"[green]Current version: [bold]{versionStr}[/][/]");

var part = AnsiConsole.Prompt(
	new SelectionPrompt<string>()
		.Title("Select the part to bump:")
		.AddChoices("Major", "Minor", "Patch", "Build"));
		
var isBuild = part == "Build";

var buildKind = "";
if (isBuild)
{
	buildKind = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
			.Title("Select the build kind:")
			.AddChoices("Alpha", "Beta", "RC", "Preview", "Other"));

	if (buildKind == "Other")
	{
		buildKind = AnsiConsole.Ask<string>("What kind of build is it?");
	}
}

string? tag = null;
if (!string.IsNullOrEmpty(buildKind))
{
	tag = buildKind;
}
else if (!string.IsNullOrEmpty(version.Release))
{
	tag = AnsiConsole.Confirm("Clear the release tag?") 
		? null 
		: version.Release;
}

var major = version.Major;
var minor = version.Minor;
var patch = version.Patch;

switch (part)
{
	case "Major":
		major++;
		minor = 0;
		patch = 0;
		break;
	case "Minor":
		minor++;
		patch = 0;
		break;
	case "Patch":
		patch++;
		break;
	case "Build":
		break;
}

var newVersion = new SemanticVersion(major, minor, patch, tag, null);

AnsiConsole.MarkupLine($"[green]Bumping to: [bold]{newVersion}[/][/]");

if (AnsiConsole.Confirm("Proceed?"))
{
	var newText = Regexes.VersionRegex.Replace(file, $$"""${open}{{newVersion}}${close}""");
	File.WriteAllText(path, newText);
}

return 0;

internal static partial class Regexes
{
	[GeneratedRegex(@"^(?<open>\s+<PackageVersion>)(?<version>.+)(?<close></PackageVersion>\s?)$", RegexOptions.Multiline)]
	public static partial Regex VersionRegex { get; }
}