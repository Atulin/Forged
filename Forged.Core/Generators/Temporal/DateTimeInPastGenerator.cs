namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateTime"/> values in the past.
/// </summary>
/// <param name="earliest">The earliest possible date/time. If null, defaults to one year ago from now.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class DateTimeInPastGenerator : Generator<DateTime>
{
	private readonly long _minTicks;
	private readonly long _maxTicks;

	/// <summary>
	/// Initializes a new instance with the specified earliest date/time.
	/// </summary>
	public DateTimeInPastGenerator(DateTime? earliest, System.Random rng) : base(rng)
	{
		var now = DateTime.UtcNow;
		_maxTicks = now.Ticks;
		_minTicks = (earliest ?? now.AddYears(-1)).Ticks;
	}

	/// <summary>
	/// Generates a random <see cref="DateTime"/> value in the past.
	/// </summary>
	/// <returns>A random <see cref="DateTime"/> between the specified earliest date and now.</returns>
	public override DateTime Generate()
	{
		var ticks = Rng.NextInt64(_minTicks, _maxTicks);
		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
