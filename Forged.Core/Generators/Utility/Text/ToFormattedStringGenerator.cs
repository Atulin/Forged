using System.Globalization;

namespace Forged.Core.Generators.Utility.Text;

public sealed class ToFormattedStringGenerator<T>(
	Generator<T> innerGenerator,
	string format,
	CultureInfo? cultureInfo = null
) : Generator<string> where T : ISpanFormattable
{
	public override string Generate()
		=> innerGenerator.Generate().ToString(format, cultureInfo ?? CultureInfo.CurrentCulture);
}