namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateOnly"/> values in the past.
/// </summary>
/// <param name="forge">The Forge instance to use.</param>
/// <param name="earliest">The earliest possible date. If null, defaults to one year ago from today.</param>
public sealed class DateOnlyInPastGenerator(Forge forge, DateOnly? earliest = null) : Generator<DateOnly>(forge)
{
	private readonly int _minDay = (earliest ?? DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-1)).DayNumber;
	private readonly int _maxDay = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber;

	/// <summary>
	/// Generates a random <see cref="DateOnly"/> value in the past.
	/// </summary>
	/// <returns>A random <see cref="DateOnly"/> between the specified earliest date and today.</returns>
	public override DateOnly Generate()
	{
		var dayNumber = Rng.Next(_minDay, _maxDay);
		return DateOnly.FromDayNumber(dayNumber);
	}
}
