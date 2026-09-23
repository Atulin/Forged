using System.Collections.Immutable;
using Forged.Core.Core;

namespace Forged.Core.Generators.Network;

/// <summary>
/// Represents a generator for producing URI schemes. This sealed class provides functionality
/// to generate random URI scheme strings from a predefined set of protocols, with an optional
/// ability to strip additional parts of the scheme that follow a colon (:).
/// </summary>
public sealed class UriScheme(bool strip, Forge forge) : Generator<string>(forge)
{
	private readonly ImmutableArray<string> _protocols =
	[
		"http://",
		"https://",
		"ftp://",
		"ftps://",
		"sftp://",
		"file:///",
		"mailto:",
		"tel:",
		"sms:",
		"ssh://",
		"git://",
		"magnet:",
	];

	public override string Generate()
	{
		var scheme = Rng.GetItem(_protocols).AsSpan();
		
		if (!strip)
		{
			return scheme.ToString();
		}
		
		var semi = scheme.IndexOf(':');
		return semi != -1 
			? scheme[..semi].ToString() 
			: scheme.ToString();
	}
}