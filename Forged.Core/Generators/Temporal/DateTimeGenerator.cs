namespace Forged.Core.Generators.Temporal;

public sealed class DateTimeGenerator(DateTime? min = null, DateTime? max = null) : Generator<DateTime>
{
	private readonly long _minTicks = (min ?? DateTime.MinValue).Ticks;
	private readonly long _maxTicks = (max ?? DateTime.MaxValue).Ticks;

	public override DateTime Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
