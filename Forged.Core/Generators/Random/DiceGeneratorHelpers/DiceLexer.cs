namespace Forged.Core.Generators.Random.DiceGeneratorHelpers;

internal static class DiceLexer
{
	public static IEnumerable<Token> Lex(string expression)
	{
		var position = 0;
		var text = expression.AsMemory().Trim();

		while (true)
		{
			var span = text.Span;
			var start = position;

			if (position >= span.Length)
			{
				yield return new Token(TokenType.End, ReadOnlyMemory<char>.Empty, position);
				break;
			}

			var c = span[position];
			if (char.IsDigit(c))
			{
				while (position < text.Length && char.IsDigit(span[position]))
				{
					position++;
				}

				yield return new Token(TokenType.Number, text[start..position], start);
				continue;
			}

			var type = c switch
			{
				'd' or 'D' => TokenType.D,
				'+' => TokenType.Plus,
				'-' => TokenType.Minus,
				'*' => TokenType.Times,
				'/' => TokenType.Divide,
				'(' => TokenType.LeftParen,
				')' => TokenType.RightParen,
				_ => throw new InvalidOperationException($"Unexpected character {c} at {position}"),
			};

			position++;
			yield return new Token(type, ReadOnlyMemory<char>.Empty, start);
		}
	}
}

internal enum TokenType
{
	Number,
	D,
	Plus,
	Minus,
	Times,
	Divide,
	LeftParen,
	RightParen,
	End,
}

internal readonly record struct Token(TokenType Type, ReadOnlyMemory<char> Text, int Position);
