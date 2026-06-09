using System.Globalization;
using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Temporal;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators;

public static class GeneratorExtensions
{
	public static Generator<T?> OrNull<T>(this Generator<T> generator, float probability) where T : struct
		=> new NullableOrValueGenerator<T>(generator, probability) { Rng = generator.Rng };
	
	public static Generator<string> ToUpper(this Generator<string> generator)
		=> new UppercaseGenerator(generator) { Rng = generator.Rng };

	public static Generator<string> ToLower(this Generator<string> generator)
		=> new LowercaseGenerator(generator) { Rng = generator.Rng };

	public static Generator<string> ToTitleCase(this Generator<string> generator)
		=> new TitleCaseGenerator(generator) { Rng = generator.Rng };

	public static Generator<DateTime> ToUtc(this Generator<DateTime> generator)
		=> new ToUtcGenerator(generator) { Rng = generator.Rng };

	public static Generator<DateTime> ToLocal(this Generator<DateTime> generator)
		=> new ToLocalGenerator(generator) { Rng = generator.Rng };

	public static Generator<DateOnly> ToDateOnly(this Generator<DateTime> generator)
		=> new DateOnlyFromDateTimeGenerator(generator) { Rng = generator.Rng };

	public static Generator<TimeOnly> ToTimeOnly(this Generator<DateTime> generator)
		=> new TimeOnlyFromDateTimeGenerator(generator) { Rng = generator.Rng };

	public static Generator<DateTime> TruncateToDate(this Generator<DateTime> generator)
		=> new TruncateToDateGenerator(generator) { Rng = generator.Rng };

	public static Generator<string?> ToString<T>(this Generator<T> generator)
		=> new ToStringGenerator<T>(generator) { Rng = generator.Rng };

	public static Generator<string> ToString<T>(this Generator<T> generator, string format, CultureInfo? cultureInfo = null) where T : ISpanFormattable
		=> new ToFormattedStringGenerator<T>(generator, format, cultureInfo) { Rng = generator.Rng };
}