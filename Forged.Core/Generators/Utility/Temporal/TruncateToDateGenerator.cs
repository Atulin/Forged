namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Truncates <see cref="DateTime"/> values to date precision (removes time component).
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="rng">The random number generator to use (not used by this generator).</param>
public sealed class TruncateToDateGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<DateTime>(rng)
{
	/// <summary>
	/// Generates a <see cref="DateTime"/> value with the time component truncated.
	/// </summary>
	/// <returns>A <see cref="DateTime"/> value with only the date component.</returns>
	public override DateTime Generate() => innerGenerator.Generate().Date;
}
