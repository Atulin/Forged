using System.Globalization;

namespace Forged.Core.Generators.Utility.Text;

public sealed class TitleCaseGenerator(Generator<string> innerGenerator, System.Random rng) : Generator<string>(rng)
{
	private static readonly TextInfo TextInfo = CultureInfo.InvariantCulture.TextInfo;

	public override string Generate() => TextInfo.ToTitleCase(innerGenerator.Generate());
}
