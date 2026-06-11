using System.Globalization;
using System.Text;

namespace Forged.Core.Generators.Utility.Text;

public sealed class SentencifyGenerator(
	Generator<string> innerGenerator,
	int minSentenceLength,
	int maxSentenceLength,
	CultureInfo? cultureInfo,
	System.Random rng
) : Generator<string>(rng)
{
	public override string Generate()
	{
		var str = innerGenerator.Generate().AsSpan();
		var words = str.Split(' ');

		var builder = new StringBuilder();
		
		var length = 0;
		foreach (var wordRange in words)
		{
			length++;
			var word = str[wordRange];

			if (length == 1)
			{
				Span<char> cap = [char.ToUpper(word[0], cultureInfo ?? CultureInfo.InvariantCulture), ..word[1..]];
				builder.Append(cap);
				builder.Append(' ');
				continue;
			}
			
			if (length >= minSentenceLength && (length >= maxSentenceLength || Rng.NextDouble() < 0.15))
			{
				builder.Append(word);
				builder.Append('.');
				builder.Append(' ');
				length = 0;
				continue;
			}
			
			builder.Append(word);
			builder.Append(' ');
		}

		builder.Remove(builder.Length - 1, 1);

		if (builder[^1] != '.')
		{
			builder.Append('.');
		}
		
		return builder.ToString();
	}
}