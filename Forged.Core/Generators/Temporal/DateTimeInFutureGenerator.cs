namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateTime"/> values in the future.
/// </summary>
/// <param name="forge">The Forge instance to use.</param>
/// <param name="latest">The latest possible date/time. If null, defaults to one year from now.</param>
public sealed class DateTimeInFutureGenerator(Forge forge, DateTime? latest = null) : Generator<DateTime>(forge)
{
	private readonly long _minTicks = DateTime.UtcNow.Ticks;
	private readonly long _maxTicks = (latest ?? DateTime.UtcNow.AddYears(1)).Ticks;

	/// <summary>
	/// Generates a random <see cref="DateTime"/> value in the future.
	/// </summary>
	/// <returns>A random <see cref="DateTime"/> between now and the specified latest date.</returns>
	public override DateTime Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
