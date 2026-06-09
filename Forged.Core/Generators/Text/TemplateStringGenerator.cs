using System.Text;

namespace Forged.Core.Generators.Text;

public sealed class TemplateStringGenerator(string template, char digitToken = '#', char letterToken = '?', char alphanumericToken = '*') : Generator<string>
{
	public override string Generate()
	{
		var sb = new StringBuilder(template.Length);
		foreach (var c in template.AsSpan())
		{
			if (c == digitToken)
			{
				sb.Append(Rng.Next(0, 10));
			}
			else if (c == letterToken)
			{
				sb.Append((char)('A' + Rng.Next(0, 26)));
			}
			else if (c == alphanumericToken)
			{
				var isDigit = Rng.Next(0, 2) == 0;
				if (isDigit)
				{
					sb.Append(Rng.Next(0, 10));
				}
				else
				{
					sb.Append((char)('A' + Rng.Next(0, 26)));
				}
			}
			else
			{
				sb.Append(c);
			}
		}
		return sb.ToString();
	}
}
