using System.Globalization;

namespace Forged.Core.Generators.Utility.Text;

public sealed class TitleCaseGenerator(Generator<string> innerGenerator, CultureInfo? cultureInfo, System.Random rng) : Generator<string>(rng)
{
	private readonly TextInfo _textInfo = cultureInfo?.TextInfo ?? CultureInfo.InvariantCulture.TextInfo;

	public override string Generate() => _textInfo.ToTitleCase(innerGenerator.Generate());
}
