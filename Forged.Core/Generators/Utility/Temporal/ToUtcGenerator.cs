namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Converts <see cref="DateTime"/> values to UTC time.
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="rng">The random number generator to use (not used by this generator).</param>
public sealed class ToUtcGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<DateTime>(rng)
{
	/// <summary>
	/// Generates a <see cref="DateTime"/> value with UTC kind.
	/// </summary>
	/// <returns>A <see cref="DateTime"/> value with <see cref="DateTimeKind.Utc"/>.</returns>
	public override DateTime Generate()
		=> DateTime.SpecifyKind(innerGenerator.Generate(), DateTimeKind.Utc);
}
