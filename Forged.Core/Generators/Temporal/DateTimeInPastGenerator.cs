namespace Forged.Core.Generators.Temporal;

public sealed class DateTimeInPastGenerator : Generator<DateTime>
{
	private readonly long _minTicks;
	private readonly long _maxTicks;

	public DateTimeInPastGenerator(DateTime? earliest, System.Random rng) : base(rng)
	{
		var now = DateTime.UtcNow;
		_maxTicks = now.Ticks;
		_minTicks = (earliest ?? now.AddYears(-1)).Ticks;
	}

	public override DateTime Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
