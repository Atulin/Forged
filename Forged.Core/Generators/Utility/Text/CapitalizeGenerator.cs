using System.Globalization;

namespace Forged.Core.Generators.Utility.Text;

public sealed class CapitalizeGenerator(Generator<string> innerGenerator, CultureInfo? cultureInfo, System.Random rng) : Generator<string>(rng)
{
	public override string Generate()
	{
		var str = innerGenerator.Generate().AsSpan();
		Span<char> capitalized = [char.ToUpper(str[0], cultureInfo ?? CultureInfo.InvariantCulture), ..str[1..]];
		return capitalized.ToString();
	}
}
