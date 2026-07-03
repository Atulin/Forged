using System.Collections.Immutable;
using Rng = System.Random;

namespace Forged.Core.Generators.Random.DiceGeneratorHelpers;

public sealed class DiceParser(string expression, Rng random)
{
	private readonly ImmutableArray<Token> _tokens = [..new DiceLexer(expression).Lex()];
	private int _index;

	public static double Evaluate(string expression, Rng random)
	{
		var parser = new DiceParser(expression, random);
		var ast = parser.ParseExpression();
		parser.Expect(TokenType.End);
		return ast.Evaluate(random);
	}
	
	private Token Current => _tokens[_index];
	private Token Next() => _tokens[_index++];

	private Token Expect(TokenType type) => Current.Type == type 
		? Next() 
		: throw new InvalidOperationException($"Expected {type} but got {Current.Type}");

	private Node ParseExpression()
	{
		var left = ParseTerm();
		while (Current.Type is TokenType.Plus or TokenType.Minus)
		{
			var op = Next().Type;
			left = new BinaryNode(left, op, ParseTerm());
		}
		return left;
	}

	private Node ParseTerm()
	{
		var left = ParseFactor();
		while (Current.Type is TokenType.Times or TokenType.Divide)
		{
			var op = Next().Type;
			left = new BinaryNode(left, op, ParseFactor());
		}
		return left;
	}
	
	private Node ParseFactor()
	{
		if (Current.Type == TokenType.Minus)
		{
			Next();
			return new UnaryMinusNode(ParseFactor());
		}
		
		return ParseDiceOrPrimary();
	}

	private Node ParseDiceOrPrimary()
	{
		if (Current.Type == TokenType.D)
		{
			Next();
			return new DiceNode(null, ParsePrimary());
		}
		
		var primary = ParsePrimary();

		if (Current.Type == TokenType.D)
		{
			Next();
			return new DiceNode(primary, ParsePrimary());
		}
		
		return primary;
	}
	
	private Node ParsePrimary()
	{
		// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
		switch (Current.Type)
		{
			case TokenType.Number:
				return new NumberNode(double.Parse(Next().Text.Span));
			case TokenType.LeftParen:
				Next();
				var node = ParseExpression();
				Expect(TokenType.RightParen);
				return node;
			default:
				throw new InvalidOperationException($"Unexpected token {Current.Type} at position {Current.Position}");
		}
	}
}

internal abstract record Node
{
	public abstract double Evaluate(Rng random);
}

internal sealed record NumberNode(double Value) : Node
{
	public override double Evaluate(Rng random) => Value;
}

internal sealed record UnaryMinusNode(Node Operand) : Node
{
	public override double Evaluate(Rng random) => -Operand.Evaluate(random);
}

internal sealed record BinaryNode(Node Left, TokenType Operator, Node Right) : Node
{
	public override double Evaluate(Rng random)
	{
		var left = Left.Evaluate(random);
		var right = Right.Evaluate(random);
		
		// ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
		return Operator switch
		{
			TokenType.Plus => left + right,
			TokenType.Minus => left - right,
			TokenType.Times => left * right,
			TokenType.Divide => left / right,
			_ => throw new InvalidOperationException($"Unknown binary operator {Operator}"),
		};
	}
}

internal sealed record DiceNode(Node? Count, Node Sides) : Node
{
	public override double Evaluate(Rng random)
	{
		var count = (int?)Count?.Evaluate(random) ?? 1;
		var sides = (int)Sides.Evaluate(random);
		
		if (count < 0) throw new InvalidOperationException("Dice count cannot be negative");
		if (sides < 1) throw new InvalidOperationException("Dice sides must be greater than 0");

		var total = 0d;
		for (var i = 0; i < count; i++)
		{
			total += random.Next(1, sides + 1);
		}
		
		return total;
	}
}