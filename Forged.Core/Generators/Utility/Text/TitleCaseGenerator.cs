namespace Forged.Core.Generators.Utility.Text;

public sealed class TitleCaseGenerator(Generator<string> innerGenerator) : Generator<string>
{
	public override string Generate()
	{
		var text = innerGenerator.Generate().AsSpan();
		Span<char> output = stackalloc char[text.Length];

		var isNewWord = true;

		for (var i = 0; i < text.Length; i++)
		{
			var c = text[i];

			if (char.IsWhiteSpace(c))
			{
				isNewWord = true;
			}
			else if (isNewWord)
			{
				output[i] = char.ToUpperInvariant(c);
				isNewWord = false;
			}
			else
			{
				output[i] = char.ToLowerInvariant(c);
			}
		}
		
		return new string(output);
	}
}