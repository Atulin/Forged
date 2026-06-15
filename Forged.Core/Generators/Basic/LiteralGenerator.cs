namespace Forged.Core.Generators.Basic;

/// <summary>
/// Generates a constant value that never changes.
/// </summary>
/// <typeparam name="T">The type of the literal value.</typeparam>
/// <param name="literal">The constant value to always return.</param>
/// <param name="rng">The random number generator to use (not used by this generator).</param>
public sealed class LiteralGenerator<T>(T literal, System.Random rng) : Generator<T>(rng)
{
	/// <summary>
	/// Generates the literal value.
	/// </summary>
	/// <returns>The constant literal value.</returns>
	public override T Generate() => literal;
}
