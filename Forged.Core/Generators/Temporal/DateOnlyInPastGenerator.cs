namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateOnly"/> values in the past.
/// </summary>
/// <param name="earliest">The earliest possible date. If null, defaults to one year ago from today.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class DateOnlyInPastGenerator : Generator<DateOnly>
{
	private readonly int _minDay;
	private readonly int _maxDay;

	/// <summary>
	/// Initializes a new instance with the specified earliest date.
	/// </summary>
	public DateOnlyInPastGenerator(DateOnly? earliest, System.Random rng) : base(rng)
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		_maxDay = today.DayNumber;
		_minDay = (earliest ?? today.AddYears(-1)).DayNumber;
	}

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
