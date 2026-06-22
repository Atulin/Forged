namespace Forged.Core.Generators.Utility.Text;

/// <summary>
/// A generator that transforms the output of another string generator by replacing all occurrences
/// of a specified substring with a given replacement string.
/// </summary>
public sealed class ReplaceGenerator(Generator<string> innerGenerator, string from, string to, Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var str = innerGenerator.Generate();
		return str.Replace(from, to);
	}
}
