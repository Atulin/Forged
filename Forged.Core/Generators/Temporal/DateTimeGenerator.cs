namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateTime"/> values within a specified range.
/// </summary>
/// <param name="min">The minimum date/time (inclusive). If null, uses <see cref="DateTime.MinValue"/>.</param>
/// <param name="max">The maximum date/time (inclusive). If null, uses <see cref="DateTime.MaxValue"/>.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class DateTimeGenerator(DateTime? min, DateTime? max, Forge forge) : Generator<DateTime>(forge)
{
	private readonly long MinTicks = (min ?? DateTime.MinValue).Ticks;
	private readonly long MaxTicks = (max ?? DateTime.MaxValue).Ticks;

	/// <summary>
	/// Generates a random <see cref="DateTime"/> value.
	/// </summary>
	/// <returns>A random <see cref="DateTime"/> between the specified minimum and maximum.</returns>
	public override DateTime Generate()
	{
		var ticks = Rng.NextInt64(MinTicks, MaxTicks);
		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
