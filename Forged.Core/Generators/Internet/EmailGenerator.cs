using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Forged.Core.Core;
using JetBrains.Annotations;
using NetEscapades.EnumGenerators;

namespace Forged.Core.Generators.Internet;

/// <summary>
/// Represents a generator for creating email addresses based on the specified kind and optional parameters.
/// </summary>
/// <param name="kind">The type of email address to generate.</param>
/// <param name="nameGenerator">An optional generator to supply email address usernames. Defaults to null, which uses a username generator.</param>
/// <param name="forge">The Forge instance to use.</param>
/// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid <see cref="EmailKind"/> is provided.</exception>
public sealed class EmailGenerator(EmailKind kind, IGenerator<string>? nameGenerator, Forge forge) : Generator<string>(forge)
{
	private static ConcurrentBag<string>? _providers;

	public override string Generate()
	{
		_providers ??= new(FileLoader.LoadData(Locale.Name, "internet/email", EmailDataContext.Default.ListString));

		var name = (nameGenerator ?? Forge.Internet.Username(leetChance: 0)).Generate();

		var domain = kind switch
		{
			EmailKind.Known => Rng.GetItem(_providers.ToArray()),
			EmailKind.Example => Rng.GetItem(["example.com", "example.org", "example.net"]),
			EmailKind.Random => $"{Forge.Text.Pronounceable(1, 3)}.{Forge.Internet.Domain()}",
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
		};
		
		return $"{name}@{domain}";
	}
}

/// <summary>
/// Represents the type of email address to generate, dictating the format and domain selection process.
/// </summary>
/// <remarks>
/// This enum is used to specify the strategy for generating email addresses. There are three possible kinds:
/// - Known: Email addresses with domains from a predefined list of commonly used providers.
/// - Example: Email addresses with domains explicitly designed for testing, such as "example.com".
/// - Random: Email addresses with randomly generated domain names.
/// </remarks>
[EnumExtensions]
public enum EmailKind
{
	/// <summary>
	/// Email addresses with domains from a predefined list of commonly used providers.
	/// </summary>
	Known,
	/// <summary>
	/// Email addresses with domains explicitly designed for testing, such as "example.com".
	/// </summary>
	Example,
	/// <summary>
	/// Email addresses with randomly generated domain names.
	/// </summary>
	Random,
}

[UsedImplicitly]
[JsonSerializable(typeof(List<string>))]
[JsonSourceGenerationOptions(
	AllowTrailingCommas = true,
	ReadCommentHandling = JsonCommentHandling.Skip,
	PropertyNameCaseInsensitive = true
)]
internal sealed partial class EmailDataContext : JsonSerializerContext;