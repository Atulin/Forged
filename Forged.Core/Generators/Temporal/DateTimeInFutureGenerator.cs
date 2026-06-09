namespace Forged.Core.Generators.Temporal;

public sealed class DateTimeInFutureGenerator : Generator<DateTime>
{
	private readonly long _minTicks;
	private readonly long _maxTicks;

	public DateTimeInFutureGenerator(DateTime? latest = null)
	{
		var now = DateTime.UtcNow;
		_minTicks = now.Ticks;
		_maxTicks = (latest ?? now.AddYears(1)).Ticks;
	}

	public override DateTime Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
