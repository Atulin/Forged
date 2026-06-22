using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates random Lorem Ipsum text.
/// </summary>
/// <param name="minWords">The minimum number of words to generate.</param>
/// <param name="maxWords">The maximum number of words to generate.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class LoremIpsumGenerator(int minWords, int maxWords, LoremIpsumGenerator.Options? options, Forge forge) : Generator<string>(forge)
{
	private static readonly string[] Lorem = ["lorem", "ipsum", "dolor", "sit", "amet"];
	
	private static ConcurrentBag<string>? _cache;
	
	/// <summary>
	/// Generates a random Lorem Ipsum string.
	/// </summary>
	/// <returns>A random Lorem Ipsum string with the specified number of words.</returns>
	public override string Generate()
	{
		_cache ??= new(FileLoader.LoadData("en", "text/lorem", LoremContext.Default.ListString));
		
		var length = minWords == maxWords
			? minWords
			: Rng.Next(minWords, maxWords + 1);

		if (options is not { Starter: > 0})
		{
			return string.Join(' ', Rng.GetItems(_cache.ToArray(), length));
		}
		
		string[] words = [
			..Lorem[..options.Starter], 
			..Rng.GetItems(_cache.ToArray(), length)
		];

		return string.Join(' ', words);
	}

	[UsedImplicitly]
	public sealed record Options(int Starter = 0);
}

[UsedImplicitly]
[JsonSerializable(typeof(List<string>))]
[JsonSourceGenerationOptions(AllowTrailingCommas = true, ReadCommentHandling = JsonCommentHandling.Skip)]
internal sealed partial class LoremContext : JsonSerializerContext;