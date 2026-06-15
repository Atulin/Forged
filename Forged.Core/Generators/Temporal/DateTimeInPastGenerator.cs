namespace Forged.Core.Generators.Temporal;

/// <summary>
/// Generates random <see cref="DateTime"/> values in the past.
/// </summary>
/// <param name="forge">The Forge instance to use.</param>
/// <param name="earliest">The earliest possible date/time. If null, defaults to one year ago from now.</param>
public sealed class DateTimeInPastGenerator(Forge forge, DateTime? earliest = null) : Generator<DateTime>(forge)
{
	private readonly long _minTicks = (earliest ?? DateTime.UtcNow.AddYears(-1)).Ticks;
	private readonly long _maxTicks = DateTime.UtcNow.Ticks;

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
