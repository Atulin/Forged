using Forged.Core.Generators;
using Forged.Core.Generators.Temporal;

namespace Forged.Core.Modules;

/// <summary>
/// Provides methods for generating temporal (date and time) data.
/// </summary>
public sealed class ForgeTemporal(Forge forge)
{
	/// <summary>
	/// Creates a generator that produces random <see cref="DateTime"/> values within a specified range.
	/// </summary>
	/// <param name="min">The minimum date/time (inclusive). If null, uses the minimum value of <see cref="DateTime"/>.</param>
	/// <param name="max">The maximum date/time (inclusive). If null, uses the maximum value of <see cref="DateTime"/>.</param>
	/// <returns>A generator that produces random <see cref="DateTime"/> values.</returns>
	public Generator<DateTime> Between(DateTime? min = null, DateTime? max = null)
		=> new DateTimeGenerator(min, max, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateTime"/> values in the past.
	/// </summary>
	/// <param name="earliest">The earliest possible date/time. If null, defaults to one year ago from now.</param>
	/// <returns>A generator that produces random past <see cref="DateTime"/> values.</returns>
	public Generator<DateTime> Past(DateTime? earliest = null)
		=> new DateTimeGenerator(earliest, DateTime.UtcNow, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateTime"/> values in the future.
	/// </summary>
	/// <param name="latest">The latest possible date/time. If null, defaults to one year from now.</param>
	/// <returns>A generator that produces random future <see cref="DateTime"/> values.</returns>
	public Generator<DateTime> Future(DateTime? latest = null)
		=> new DateTimeGenerator(DateTime.UtcNow, latest, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateOnly"/> values within a specified range.
	/// </summary>
	/// <param name="min">The minimum date (inclusive). If null, uses the minimum value of <see cref="DateOnly"/>.</param>
	/// <param name="max">The maximum date (inclusive). If null, uses the maximum value of <see cref="DateOnly"/>.</param>
	/// <returns>A generator that produces random <see cref="DateOnly"/> values.</returns>
	public Generator<DateOnly> DateBetween(DateOnly? min = null, DateOnly? max = null)
		=> new DateOnlyGenerator(min, max, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateOnly"/> values in the past.
	/// </summary>
	/// <param name="earliest">The earliest possible date. If null, defaults to one year ago from today.</param>
	/// <returns>A generator that produces random past <see cref="DateOnly"/> values.</returns>
	public Generator<DateOnly> DateInPast(DateOnly? earliest = null)
		=> new DateOnlyGenerator(DateOnly.FromDateTime(DateTime.UtcNow), earliest, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateOnly"/> values in the future.
	/// </summary>
	/// <param name="latest">The latest possible date. If null, defaults to one year from today.</param>
	/// <returns>A generator that produces random future <see cref="DateOnly"/> values.</returns>
	public Generator<DateOnly> DateInFuture(DateOnly? latest = null)
		=> new DateOnlyGenerator(latest, DateOnly.FromDateTime(DateTime.UtcNow), forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="TimeOnly"/> values within a specified range.
	/// </summary>
	/// <param name="min">The minimum time (inclusive). If null, uses the minimum value of <see cref="TimeOnly"/>.</param>
	/// <param name="max">The maximum time (inclusive). If null, uses the maximum value of <see cref="TimeOnly"/>.</param>
	/// <returns>A generator that produces random <see cref="TimeOnly"/> values.</returns>
	public Generator<TimeOnly> TimeBetween(TimeOnly? min = null, TimeOnly? max = null)
		=> new TimeOnlyGenerator(min, max, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateTimeOffset"/> values within a specified range.
	/// </summary>
	/// <param name="min">The minimum date/time offset (inclusive). If null, uses the minimum value of <see cref="DateTimeOffset"/>.</param>
	/// <param name="max">The maximum date/time offset (inclusive). If null, uses the maximum value of <see cref="DateTimeOffset"/>.</param>
	/// <returns>A generator that produces random <see cref="DateTimeOffset"/> values.</returns>
	public Generator<DateTimeOffset> DateTimeOffsetBetween(DateTimeOffset? min = null, DateTimeOffset? max = null)
		=> new DateTimeOffsetGenerator(min, max, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateTimeOffset"/> values in the past.
	/// </summary>
	/// <param name="earliest">The earliest possible date/time offset. If null, defaults to one year ago from now.</param>
	/// <returns>A generator that produces random past <see cref="DateTimeOffset"/> values.</returns>
	public Generator<DateTimeOffset> DateTimeOffsetInPast(DateTimeOffset? earliest = null)
		=> new DateTimeOffsetGenerator(earliest, DateTimeOffset.UtcNow, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="DateTimeOffset"/> values in the future.
	/// </summary>
	/// <param name="latest">The latest possible date/time offset. If null, defaults to one year from now.</param>
	/// <returns>A generator that produces random future <see cref="DateTimeOffset"/> values.</returns>
	public Generator<DateTimeOffset> DateTimeOffsetInFuture(DateTimeOffset? latest = null)
		=> new DateTimeOffsetGenerator(DateTimeOffset.UtcNow, latest, forge);

	/// <summary>
	/// Creates a generator that produces random <see cref="TimeSpan"/> values within a specified range.
	/// </summary>
	/// <param name="min">The minimum time span (inclusive). If null, uses the minimum value of <see cref="TimeSpan"/>.</param>
	/// <param name="max">The maximum time span (inclusive). If null, uses the maximum value of <see cref="TimeSpan"/>.</param>
	/// <returns>A generator that produces random <see cref="TimeSpan"/> values.</returns>
	public Generator<TimeSpan> TimeSpanBetween(TimeSpan? min = null, TimeSpan? max = null)
		=> new TimeSpanGenerator(min, max, forge);
}
