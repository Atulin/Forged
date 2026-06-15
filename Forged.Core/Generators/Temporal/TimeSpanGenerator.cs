namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="TimeSpan"/> values within a specified range.
/// </summary>
/// <param name="min">The minimum time span (inclusive). If null, uses <see cref="TimeSpan.MinValue"/>. </param>
/// <param name="max">The maximum time span (inclusive). If null, uses <see cref="TimeSpan.MaxValue"/>. </param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class TimeSpanGenerator(TimeSpan? min, TimeSpan? max, Forge forge) : Generator<TimeSpan>(forge)
{
	private readonly long _minTicks = (min ?? TimeSpan.MinValue).Ticks;
	private readonly long _maxTicks = (max ?? TimeSpan.MaxValue).Ticks;

	/// <summary>
	/// Generates a random <see cref="TimeSpan"/> value.
	/// </summary>
	/// <returns>A random <see cref="TimeSpan"/> between the specified minimum and maximum.</returns>
	public override TimeSpan Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new TimeSpan(ticks);
	}
}