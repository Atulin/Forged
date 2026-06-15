namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateOnly"/> values within a specified range.
/// </summary>
/// <param name="min">The minimum date (inclusive). If null, uses <see cref="DateOnly.MinValue"/>.</param>
/// <param name="max">The maximum date (inclusive). If null, uses <see cref="DateOnly.MaxValue"/>.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class DateOnlyGenerator(DateOnly? min, DateOnly? max, Forge forge) : Generator<DateOnly>(forge)
{
	private readonly int _minDay = (min ?? DateOnly.MinValue).DayNumber;
	private readonly int _maxDay = (max ?? DateOnly.MaxValue).DayNumber;

	/// <summary>
	/// Generates a random <see cref="DateOnly"/> value.
	/// </summary>
	/// <returns>A random <see cref="DateOnly"/> between the specified minimum and maximum.</returns>
	public override DateOnly Generate()
	{
		var dayNumber = Rng.Next(_minDay, _maxDay);
		return DateOnly.FromDayNumber(dayNumber);
	}
}
