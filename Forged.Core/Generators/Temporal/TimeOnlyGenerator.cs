namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="TimeOnly"/> values within a specified range.
/// </summary>
/// <param name="min">The minimum time (inclusive). If null, uses <see cref="TimeOnly.MinValue"/>.</param>
/// <param name="max">The maximum time (inclusive). If null, uses <see cref="TimeOnly.MaxValue"/>.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class TimeOnlyGenerator(TimeOnly? min, TimeOnly? max, Forge forge) : Generator<TimeOnly>(forge)
{
	private readonly long MinTicks = (min ?? TimeOnly.MinValue).Ticks;
	private readonly long MaxTicks = (max ?? TimeOnly.MaxValue).Ticks;

	/// <summary>
	/// Generates a random <see cref="TimeOnly"/> value.
	/// </summary>
	/// <returns>A random <see cref="TimeOnly"/> between the specified minimum and maximum.</returns>
	public override TimeOnly Generate()
	{
		var ticks = Rng.NextInt64(MinTicks, MaxTicks);
		return new TimeOnly(ticks);
	}
}
