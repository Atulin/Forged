namespace Forged.Core.Generators.Random.DiceGeneratorHelpers;

internal sealed class DiceLexer(string expression)
{
	private int _position;
	private ReadOnlyMemory<char> _text = expression.AsMemory().Trim();

	public IEnumerable<Token> Lex()
	{
		while (true)
		{
			var span = _text.Span;
			var start = _position;
			
			if (_position >= span.Length)
			{
				yield return new Token(TokenType.End, ReadOnlyMemory<char>.Empty, _position);
				break;
			}
			
			var c = span[_position];
			if (char.IsDigit(c))
			{
				while (_position < _text.Length && char.IsDigit(span[_position]))
				{
					_position++;
				}
				
				yield return new Token(TokenType.Number, _text[start.._position], start);
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
				_ => throw new InvalidOperationException($"Unexpected character {c} at {_position}"),
			};

			_position++;
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