using System.Collections.Frozen;
using Forged.Core.Core;

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
			var file = FileLoader.LoadData("en", "text/emoji", CommonContext.Default.DictionaryStringString);
			_emojiCache = file.ToFrozenDictionary();
		}

		var index = Rng.Next(_emojiCache.Count);
		return _emojiCache.Values[index];
	}
}