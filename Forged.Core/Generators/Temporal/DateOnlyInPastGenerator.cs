namespace Forged.Core.Generators.Temporal;

public sealed class DateOnlyInPastGenerator : Generator<DateOnly>
{
	private readonly int _minDay;
	private readonly int _maxDay;

	public DateOnlyInPastGenerator(DateOnly? earliest, System.Random rng) : base(rng)
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		_maxDay = today.DayNumber;
		_minDay = (earliest ?? today.AddYears(-1)).DayNumber;
	}

	public override DateOnly Generate()
	{
		var dayNumber = Rng.Next(_minDay, _maxDay);
		return DateOnly.FromDayNumber(dayNumber);
	}
}
