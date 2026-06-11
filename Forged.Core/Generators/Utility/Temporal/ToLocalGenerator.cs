namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Converts <see cref="DateTime"/> values to local time.
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="rng">The random number generator to use (not used by this generator).</param>
public sealed class ToLocalGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<DateTime>(rng)
{
	/// <summary>
	/// Generates a <see cref="DateTime"/> value with local kind.
	/// </summary>
	/// <returns>A <see cref="DateTime"/> value with <see cref="DateTimeKind.Local"/>.</returns>
	public override DateTime Generate()
		=> DateTime.SpecifyKind(innerGenerator.Generate(), DateTimeKind.Local);
}
