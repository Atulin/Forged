namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateOnly"/> values in the future.
/// </summary>
/// <param name="latest">The latest possible date. If null, defaults to one year from today.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class DateOnlyInFutureGenerator : Generator<DateOnly>
{
	private readonly int _minDay;
	private readonly int _maxDay;

	/// <summary>
	/// Initializes a new instance with the specified latest date.
	/// </summary>
	public DateOnlyInFutureGenerator(DateOnly? latest, System.Random rng) : base(rng)
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		_minDay = today.DayNumber;
		_maxDay = (latest ?? today.AddYears(1)).DayNumber;
	}

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
