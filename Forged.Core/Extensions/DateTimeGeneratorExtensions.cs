using Forged.Core.Generators;
using Forged.Core.Generators.Utility.Temporal;

namespace Forged.Core.Extensions;

/// <summary>
/// Extension methods for DateTime generators.
/// </summary>
public static class DateTimeGeneratorExtensions
{
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
}
