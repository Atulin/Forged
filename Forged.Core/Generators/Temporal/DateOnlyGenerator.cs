namespace Forged.Core.Generators.Temporal;

public sealed class DateOnlyGenerator(DateOnly? min, DateOnly? max, System.Random rng) : Generator<DateOnly>(rng)
{
	private readonly int _minDay = (min ?? DateOnly.MinValue).DayNumber;
	private readonly int _maxDay = (max ?? DateOnly.MaxValue).DayNumber;

	public override DateOnly Generate()
	{
		var dayNumber = Rng.Next(_minDay, _maxDay);
		return DateOnly.FromDayNumber(dayNumber);
	}
}
