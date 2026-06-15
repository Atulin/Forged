namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateTimeOffset"/> values within a specified range.
/// </summary>
/// <param name="min">The minimum date/time offset (inclusive). If null, uses <see cref="DateTimeOffset.MinValue"/>. </param>
/// <param name="max">The maximum date/time offset (inclusive). If null, uses <see cref="DateTimeOffset.MaxValue"/>. </param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class DateTimeOffsetGenerator(DateTimeOffset? min, DateTimeOffset? max, Forge forge) : Generator<DateTimeOffset>(forge)
{
	private readonly long _minTicks = (min ?? DateTimeOffset.MinValue).Ticks;
	private readonly long _maxTicks = (max ?? DateTimeOffset.MaxValue).Ticks;

	/// <summary>
	/// Generates a random <see cref="DateTimeOffset"/> value.
	/// </summary>
	/// <returns>A random <see cref="DateTimeOffset"/> between the specified minimum and maximum.</returns>
	public override DateTimeOffset Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new DateTimeOffset(ticks, TimeSpan.Zero);
	}
}