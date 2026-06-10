namespace Forged.Core.Generators.Temporal;

public sealed class DateOnlyInFutureGenerator : Generator<DateOnly>
{
	private readonly int _minDay;
	private readonly int _maxDay;

	public DateOnlyInFutureGenerator(DateOnly? latest, System.Random rng) : base(rng)
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		_minDay = today.DayNumber;
		_maxDay = (latest ?? today.AddYears(1)).DayNumber;
	}

	public override DateOnly Generate()
	{
		var dayNumber = Rng.Next(_minDay, _maxDay);
		return DateOnly.FromDayNumber(dayNumber);
	}
}
