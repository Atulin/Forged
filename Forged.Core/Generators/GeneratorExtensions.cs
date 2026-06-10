using System.Globalization;
using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;
using Forged.Core.Generators.Utility.Temporal;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators;

public static class GeneratorExtensions
{
	public static Generator<T?> OrNull<T>(this Generator<T> generator, float probability) where T : struct
		=> new NullableOrValueGenerator<T>(generator, probability, generator.Rng);

	public static Generator<string> ToUpper(this Generator<string> generator)
		=> new UppercaseGenerator(generator, generator.Rng);

	public static Generator<string> ToLower(this Generator<string> generator)
		=> new LowercaseGenerator(generator, generator.Rng);

	public static Generator<string> ToTitleCase(this Generator<string> generator)
		=> new TitleCaseGenerator(generator, generator.Rng);

	public static Generator<DateTime> ToUtc(this Generator<DateTime> generator)
		=> new ToUtcGenerator(generator, generator.Rng);

	public static Generator<DateTime> ToLocal(this Generator<DateTime> generator)
		=> new ToLocalGenerator(generator, generator.Rng);

	public static Generator<DateOnly> ToDateOnly(this Generator<DateTime> generator)
		=> new DateOnlyFromDateTimeGenerator(generator, generator.Rng);

	public static Generator<TimeOnly> ToTimeOnly(this Generator<DateTime> generator)
		=> new TimeOnlyFromDateTimeGenerator(generator, generator.Rng);

	public static Generator<DateTime> TruncateToDate(this Generator<DateTime> generator)
		=> new TruncateToDateGenerator(generator, generator.Rng);

	public static Generator<string?> ToString<T>(this Generator<T> generator)
		=> new ToStringGenerator<T>(generator, generator.Rng);

	public static Generator<string> ToString<T>(this Generator<T> generator, string format, CultureInfo? cultureInfo = null) where T : ISpanFormattable
		=> new ToFormattedStringGenerator<T>(generator, format, cultureInfo, generator.Rng);

	/// <summary>Narrows a Generator&lt;IEnumerable&lt;T&gt;&gt; to a Generator&lt;List&lt;T&gt;&gt;.</summary>
	public static Generator<List<T>> AsList<T>(this Generator<IEnumerable<T>> generator)
		=> new RefineGenerator<IEnumerable<T>, List<T>>(generator, e => e.ToList(), generator.Rng);

	/// <summary>Narrows a Generator&lt;ICollection&lt;T&gt;&gt; to a Generator&lt;List&lt;T&gt;&gt;.</summary>
	public static Generator<List<T>> AsList<T>(this Generator<ICollection<T>> generator)
		=> new RefineGenerator<ICollection<T>, List<T>>(generator, c => c.ToList(), generator.Rng);

	/// <summary>Narrows a Generator&lt;IEnumerable&lt;T&gt;&gt; to a Generator&lt;HashSet&lt;T&gt;&gt;.</summary>
	public static Generator<HashSet<T>> AsHashSet<T>(this Generator<IEnumerable<T>> generator)
		=> new RefineGenerator<IEnumerable<T>, HashSet<T>>(generator, e => e.ToHashSet(), generator.Rng);

	/// <summary>Narrows a Generator&lt;ICollection&lt;T&gt;&gt; to a Generator&lt;HashSet&lt;T&gt;&gt;.</summary>
	public static Generator<HashSet<T>> AsHashSet<T>(this Generator<ICollection<T>> generator)
		=> new RefineGenerator<ICollection<T>, HashSet<T>>(generator, c => c.ToHashSet(), generator.Rng);
}
