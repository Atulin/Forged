namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateTime"/> values in the future.
/// </summary>
/// <param name="latest">The latest possible date/time. If null, defaults to one year from now.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class DateTimeInFutureGenerator : Generator<DateTime>
{
	private readonly long _minTicks;
	private readonly long _maxTicks;

	/// <summary>
	/// Initializes a new instance with the specified latest date/time.
	/// </summary>
	public DateTimeInFutureGenerator(DateTime? latest, System.Random rng) : base(rng)
	{
		var now = DateTime.UtcNow;
		_minTicks = now.Ticks;
		_maxTicks = (latest ?? now.AddYears(1)).Ticks;
	}

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
