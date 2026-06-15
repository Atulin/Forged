using System.Globalization;
using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Temporal;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators;

/// <summary>
/// Provides extension methods for the <see cref="Generator{T}"/> class.
/// </summary>
public static class GeneratorExtensions
{
	/// <summary>
	/// Extension methods for struct generators.
	/// </summary>
	extension<T>(Generator<T> generator) where T : struct
	{
		/// <summary>
		/// Creates a generator that produces null values with a specified probability.
		/// </summary>
		/// <param name="probability">The probability (0.0 to 1.0) of producing null.</param>
		/// <returns>A generator that may produce null values.</returns>
		public Generator<T?> OrNull(float probability)
			=> new NullableOrValueGenerator<T>(generator, probability, generator.Forge);
		
		/// <summary>
		/// Creates a generator that produces nullable values.
		/// </summary>
		/// <returns>A generator that produces nullable values.</returns>
		public Generator<T?> Nullable()
			=> generator.Refine(x => (T?)x);
	}

	/// <summary>
	/// Extension methods for string generators.
	/// </summary>
	extension(Generator<string> generator)
	{
		/// <summary>
		/// Creates a generator that converts strings to uppercase.
		/// </summary>
		/// <returns>A generator that produces uppercase strings.</returns>
		public Generator<string> ToUpper()
			=> new UppercaseGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that converts strings to lowercase.
		/// </summary>
		/// <returns>A generator that produces lowercase strings.</returns>
		public Generator<string> ToLower()
			=> new LowercaseGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that converts strings to title case.
		/// </summary>
		/// <param name="cultureInfo">The culture to use for title case conversion. If null, uses invariant culture.</param>
		/// <returns>A generator that produces title case strings.</returns>
		public Generator<string> ToTitleCase(CultureInfo? cultureInfo = null)
			=> new TitleCaseGenerator(generator, generator.Forge, cultureInfo);
		
		/// <summary>
		/// Creates a generator that capitalizes the first character of strings.
		/// </summary>
		/// <param name="cultureInfo">The culture to use for capitalization. If null, uses invariant culture.</param>
		/// <returns>A generator that produces strings with the first character capitalized.</returns>
		public Generator<string> Capitalize(CultureInfo? cultureInfo = null)
			=> new CapitalizeGenerator(generator, generator.Forge, cultureInfo);
		
		/// <summary>
		/// Creates a generator that converts strings into properly formatted sentences.
		/// </summary>
		/// <param name="sentenceLength">The exact number of words per sentence.</param>
		/// <param name="cultureInfo">The culture to use for capitalization. If null, uses invariant culture.</param>
		/// <returns>A generator that produces properly formatted sentences.</returns>
		public Generator<string> Sentencify(int sentenceLength, CultureInfo? cultureInfo = null)
			=> new SentencifyGenerator(generator, sentenceLength, sentenceLength, generator.Forge, cultureInfo);
		
		/// <summary>
		/// Creates a generator that converts strings into properly formatted sentences with variable length.
		/// </summary>
		/// <param name="minSentenceLength">The minimum number of words per sentence.</param>
		/// <param name="maxSentenceLength">The maximum number of words per sentence.</param>
		/// <param name="cultureInfo">The culture to use for capitalization. If null, uses invariant culture.</param>
		/// <returns>A generator that produces properly formatted sentences.</returns>
		public Generator<string> Sentencify(int minSentenceLength, int maxSentenceLength, CultureInfo? cultureInfo = null)
			=> new SentencifyGenerator(generator, minSentenceLength, maxSentenceLength, generator.Forge, cultureInfo);
	}

	/// <summary>
	/// Extension methods for DateTime generators.
	/// </summary>
	extension(Generator<DateTime> generator)
	{
		/// <summary>
		/// Creates a generator that converts DateTime values to UTC.
		/// </summary>
		/// <returns>A generator that produces UTC DateTime values.</returns>
		public Generator<DateTime> ToUtc()
			=> new ToUtcGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that converts DateTime values to local time.
		/// </summary>
		/// <returns>A generator that produces local DateTime values.</returns>
		public Generator<DateTime> ToLocal()
			=> new ToLocalGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that extracts the date component from DateTime values.
		/// </summary>
		/// <returns>A generator that produces DateOnly values.</returns>
		public Generator<DateOnly> ToDateOnly()
			=> new DateOnlyFromDateTimeGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that extracts the time component from DateTime values.
		/// </summary>
		/// <returns>A generator that produces TimeOnly values.</returns>
		public Generator<TimeOnly> ToTimeOnly()
			=> new TimeOnlyFromDateTimeGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that truncates DateTime values to date precision.
		/// </summary>
		/// <returns>A generator that produces DateTime values with time component set to midnight.</returns>
		public Generator<DateTime> TruncateToDate()
			=> new TruncateToDateGenerator(generator, generator.Forge);
	}

	/// <summary>
	/// Converts values from a generator to their string representation.
	/// </summary>
	/// <typeparam name="T">The type to convert to string.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <returns>A generator that produces string representations.</returns>
	public static Generator<string?> ToString<T>(this Generator<T> generator)
		=> new ToStringGenerator<T>(generator, generator.Forge);

	/// <summary>
	/// Converts values from a generator to formatted strings.
	/// </summary>
	/// <typeparam name="T">The type to format (must implement ISpanFormattable).</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <param name="format">The format string to use.</param>
	/// <param name="cultureInfo">The culture to use for formatting. If null, uses invariant culture.</param>
	/// <returns>A generator that produces formatted strings.</returns>
	public static Generator<string> ToString<T>(this Generator<T> generator, string format, CultureInfo? cultureInfo = null) where T : ISpanFormattable
		=> new ToFormattedStringGenerator<T>(generator, format, generator.Forge, cultureInfo);

	/// <summary>
	/// Converts enumerables from a generator to lists.
	/// </summary>
	/// <typeparam name="T">The type of items in the enumerable.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <returns>A generator that produces lists.</returns>
	public static Generator<List<T>> AsList<T>(this Generator<IEnumerable<T>> generator)
		=> new RefineGenerator<IEnumerable<T>, List<T>>(generator, static e => e.ToList(), generator.Forge);

	/// <summary>
	/// Converts collections from a generator to lists.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <returns>A generator that produces lists.</returns>
	public static Generator<List<T>> AsList<T>(this Generator<ICollection<T>> generator)
		=> new RefineGenerator<ICollection<T>, List<T>>(generator, static c => c.ToList(), generator.Forge);

	/// <summary>
	/// Converts enumerables from a generator to hash sets.
	/// </summary>
	/// <typeparam name="T">The type of items in the enumerable.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <returns>A generator that produces hash sets.</returns>
	public static Generator<HashSet<T>> AsHashSet<T>(this Generator<IEnumerable<T>> generator)
		=> new RefineGenerator<IEnumerable<T>, HashSet<T>>(generator, static e => e.ToHashSet(), generator.Forge);

	/// <summary>
	/// Converts collections from a generator to hash sets.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <returns>A generator that produces hash sets.</returns>
	public static Generator<HashSet<T>> AsHashSet<T>(this Generator<ICollection<T>> generator)
		=> new RefineGenerator<ICollection<T>, HashSet<T>>(generator, static c => c.ToHashSet(), generator.Forge);

	/// <summary>
	/// Converts enumerables from a generator to dictionaries.
	/// </summary>
	/// <typeparam name="T">The type of items in the enumerable.</typeparam>
	/// <typeparam name="TKey">The type of dictionary keys.</typeparam>
	/// <typeparam name="TValue">The type of dictionary values.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <param name="keySelector">A function to extract the key from each item.</param>
	/// <param name="valueSelector">A function to extract the value from each item.</param>
	/// <returns>A generator that produces dictionaries.</returns>
	public static Generator<Dictionary<TKey, TValue>> AsDictionary<T, TKey, TValue>(
		this Generator<IEnumerable<T>> generator,
		Func<T, TKey> keySelector,
		Func<T, TValue> valueSelector
	) where TKey : notnull
		=> generator.Refine(e => e.ToDictionary(keySelector, valueSelector));
}
