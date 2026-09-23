using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Forged.Core.Generators.Text;

/// <summary>
/// Represents a generator for producing random emojis.
/// </summary>
public sealed class EmojiGenerator(Forge forge) : Generator<string>(forge)
{
	private static FrozenDictionary<string, string>? _emojiCache;

	public override string Generate()
	{
		if (_emojiCache is null)
		{
			var file = FileLoader.LoadData("en", "text/emoji", JsonContext.Default.DictionaryStringString);
			_emojiCache = file.ToFrozenDictionary();
		}

		var index = Rng.Next(_emojiCache.Count);
		return _emojiCache.Values[index];
	}
}


[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSourceGenerationOptions(ReadCommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true)]
internal sealed partial class JsonContext : JsonSerializerContext;