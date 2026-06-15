namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateOnly"/> values in the future.
/// </summary>
/// <param name="forge">The Forge instance to use.</param>
/// <param name="latest">The latest possible date. If null, defaults to one year from today.</param>
public sealed class DateOnlyInFutureGenerator(Forge forge, DateOnly? latest = null) : Generator<DateOnly>(forge)
{
	private readonly int _minDay = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber;
	private readonly int _maxDay = (latest ?? DateOnly.FromDateTime(DateTime.UtcNow).AddYears(1)).DayNumber;

	/// <summary>
	/// Generates a random <see cref="DateOnly"/> value in the future.
	/// </summary>
	/// <returns>A random <see cref="DateOnly"/> between today and the specified latest date.</returns>
	public override DateOnly Generate()
	{
		var dayNumber = Rng.Next(_minDay, _maxDay);
		return DateOnly.FromDayNumber(dayNumber);
	}
}
