using System.Text;

namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates strings based on a template with placeholder tokens.
/// </summary>
/// <param name="template">The template string containing placeholder tokens.</param>
/// <param name="rng">The random number generator to use.</param>
/// <param name="digitToken">The character token that will be replaced with a random digit (default: '#').</param>
/// <param name="letterToken">The character token that will be replaced with a random letter (default: '?').</param>
/// <param name="alphanumericToken">The character token that will be replaced with a random alphanumeric character (default: '*').</param>
public sealed class TemplateStringGenerator(
	string template,
	System.Random rng,
	char digitToken = '#',
	char letterToken = '?',
	char alphanumericToken = '*') : Generator<string>(rng)
{
	/// <summary>
	/// Generates a string from the template.
	/// </summary>
	/// <returns>A string with placeholder tokens replaced by random characters.</returns>
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
