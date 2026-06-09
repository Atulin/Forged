namespace Forged.Core.Generators.Temporal;

public sealed class TimeOnlyGenerator(TimeOnly? min = null, TimeOnly? max = null) : Generator<TimeOnly>
{
	private readonly long _minTicks = (min ?? TimeOnly.MinValue).Ticks;
	private readonly long _maxTicks = (max ?? TimeOnly.MaxValue).Ticks;

	public override TimeOnly Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new TimeOnly(ticks);
	}
}
