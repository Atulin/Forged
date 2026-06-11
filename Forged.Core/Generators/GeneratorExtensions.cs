using System.Globalization;
using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Temporal;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators;

public static class GeneratorExtensions
{
	extension<T>(Generator<T> generator) where T : struct
	{
		public Generator<T?> OrNull(float probability)
			=> new NullableOrValueGenerator<T>(generator, probability, generator.Rng);
		public Generator<T?> Nullable()
			=> generator.Refine(x => (T?)x);
	}

	extension(Generator<string> generator)
	{
		public Generator<string> ToUpper()
			=> new UppercaseGenerator(generator, generator.Rng);
		public Generator<string> ToLower()
			=> new LowercaseGenerator(generator, generator.Rng);
		public Generator<string> ToTitleCase(CultureInfo? cultureInfo = null)
			=> new TitleCaseGenerator(generator, cultureInfo, generator.Rng);
		public Generator<string> Capitalize(CultureInfo? cultureInfo = null)
			=> new CapitalizeGenerator(generator, cultureInfo, generator.Rng);
		public Generator<string> Sentencify(int sentenceLength, CultureInfo? cultureInfo = null)
			=> new SentencifyGenerator(generator, sentenceLength, sentenceLength, cultureInfo, generator.Rng);
		public Generator<string> Sentencify(int minSentenceLength, int maxSentenceLength, CultureInfo? cultureInfo = null)
			=> new SentencifyGenerator(generator, minSentenceLength, maxSentenceLength, cultureInfo, generator.Rng);
	}

	extension(Generator<DateTime> generator)
	{
		public Generator<DateTime> ToUtc()
			=> new ToUtcGenerator(generator, generator.Rng);
		public Generator<DateTime> ToLocal()
			=> new ToLocalGenerator(generator, generator.Rng);
		public Generator<DateOnly> ToDateOnly()
			=> new DateOnlyFromDateTimeGenerator(generator, generator.Rng);
		public Generator<TimeOnly> ToTimeOnly()
			=> new TimeOnlyFromDateTimeGenerator(generator, generator.Rng);
		public Generator<DateTime> TruncateToDate()
			=> new TruncateToDateGenerator(generator, generator.Rng);
	}

	public static Generator<string?> ToString<T>(this Generator<T> generator)
		=> new ToStringGenerator<T>(generator, generator.Rng);

	public static Generator<string> ToString<T>(this Generator<T> generator, string format, CultureInfo? cultureInfo = null) where T : ISpanFormattable
		=> new ToFormattedStringGenerator<T>(generator, format, cultureInfo, generator.Rng);

	public static Generator<List<T>> AsList<T>(this Generator<IEnumerable<T>> generator)
		=> new RefineGenerator<IEnumerable<T>, List<T>>(generator, static e => e.ToList(), generator.Rng);

	public static Generator<List<T>> AsList<T>(this Generator<ICollection<T>> generator)
		=> new RefineGenerator<ICollection<T>, List<T>>(generator, static c => c.ToList(), generator.Rng);

	public static Generator<HashSet<T>> AsHashSet<T>(this Generator<IEnumerable<T>> generator)
		=> new RefineGenerator<IEnumerable<T>, HashSet<T>>(generator, static e => e.ToHashSet(), generator.Rng);

	public static Generator<HashSet<T>> AsHashSet<T>(this Generator<ICollection<T>> generator)
		=> new RefineGenerator<ICollection<T>, HashSet<T>>(generator, static c => c.ToHashSet(), generator.Rng);

	public static Generator<Dictionary<TKey, TValue>> AsDictionary<T, TKey, TValue>(
		this Generator<IEnumerable<T>> generator,
		Func<T, TKey> keySelector,
		Func<T, TValue> valueSelector
	) where TKey : notnull
		=> generator.Refine(e => e.ToDictionary(keySelector, valueSelector));
}