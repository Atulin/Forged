using System.Globalization;
using Forged.Core.Generators.Utility.Temporal;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators;

public static class GeneratorExtensions
{
	public static Generator<string> ToUpper(this Generator<string> generator)
		=> new UppercaseGenerator(generator);

	public static Generator<string> ToLower(this Generator<string> generator)
		=> new LowercaseGenerator(generator);

	public static Generator<string> ToTitleCase(this Generator<string> generator)
		=> new TitleCaseGenerator(generator);

	public static Generator<DateTime> ToUtc(this Generator<DateTime> generator)
		=> new ToUtcGenerator(generator);

	public static Generator<DateTime> ToLocal(this Generator<DateTime> generator)
		=> new ToLocalGenerator(generator);

	public static Generator<DateOnly> ToDateOnly(this Generator<DateTime> generator)
		=> new DateOnlyFromDateTimeGenerator(generator);

	public static Generator<TimeOnly> ToTimeOnly(this Generator<DateTime> generator)
		=> new TimeOnlyFromDateTimeGenerator(generator);

	public static Generator<DateTime> TruncateToDate(this Generator<DateTime> generator)
		=> new TruncateToDateGenerator(generator);

	public static Generator<string?> ToString<T>(this Generator<T> generator)
		=> new ToStringGenerator<T>(generator);

	public static Generator<string> ToString<T>(this Generator<T> generator, string format, CultureInfo? cultureInfo = null) where T : ISpanFormattable
		=> new ToFormattedStringGenerator<T>(generator, format, cultureInfo);
}