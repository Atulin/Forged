using System.Text;

namespace Forged.Core.Generators.Text;

public sealed class TemplateStringGenerator(
	string template,
	System.Random rng,
	char digitToken = '#',
	char letterToken = '?',
	char alphanumericToken = '*') : Generator<string>(rng)
{
	public override string Generate()
	{
		var sb = new StringBuilder(template.Length);
		var escaped = false;
		foreach (var c in template.AsSpan())
		{
			if (escaped)
			{
				sb.Append(c);
				escaped = false;
				continue;
			}
			
			if (c == '\\')
			{
				escaped = true;
				continue;
			}
			
			if (c == digitToken)
			{
				sb.Append(Rng.Next(0, 10));
			}
			else if (c == letterToken)
			{
				sb.Append(Rng.GetItems(Constants.AlphanumericString.AsSpan()[..^10], 1));
			}
			else if (c == alphanumericToken)
			{
				sb.Append(Rng.GetItems(Constants.AlphanumericString.AsSpan(), 1));
			}
			else
			{
				sb.Append(c);
			}
		}
		return sb.ToString();
	}
}
