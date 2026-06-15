using System.Text;

namespace Forged.Core.Core;

internal static class TemplateCompiler
{
	public static List<Token> Compile(string template)
	{
		var tokens = new List<Token>();
		var span = template.AsSpan();

		var i = 0;

		while (i < span.Length)
		{
			var start = template.IndexOf('{', i);
			if (start == -1)
			{
				tokens.Add(new Literal(template[i..]));
				break;
			}

			if (start > i)
			{
				tokens.Add(new Literal(template[i..start]));
			}
			
			var end = template.IndexOf('}', start);
			var key = template[(start + 1)..end];
			
			tokens.Add(new Placeholder(key));
			
			i = end + 1;
		}

		return tokens;
	}

	public static string Render(List<Token> tokens, Dictionary<string, string> values)
	{
		var sb = new StringBuilder();

		foreach (var token in tokens)
		{
			var part = token switch
			{
				Literal l => l.Value,
				Placeholder p => values[p.Key],
				_ => throw new InvalidOperationException("Token was somehow neither literal nor placeholder"),
			};
			sb.Append(part);
		}
		
		return sb.ToString();
	}

	public static string Render(List<Token> tokens, Dictionary<string, Func<string>> values)
	{
		var sb = new StringBuilder();

		foreach (var token in tokens)
		{
			var part = token switch
			{
				Literal l => l.Value,
				Placeholder p => values[p.Key](),
				_ => throw new InvalidOperationException("Token was somehow neither literal nor placeholder"),
			};
			sb.Append(part);
		}
		
		return sb.ToString();
	}

	internal abstract record Token;
	internal sealed record Literal(string Value) : Token;
	internal sealed record Placeholder(string Key) : Token;
}