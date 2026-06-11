namespace Forged.Core.Generators.Utility;

/// <summary>
/// Generates either a value from an inner generator or a fallback value based on a probability.
/// </summary>
/// <typeparam name="T">The type of value to generate.</typeparam>
/// <param name="innerGenerator">The inner generator to use when not returning the fallback.</param>
/// <param name="fallback">The fallback value to return when the probability condition is met.</param>
/// <param name="probability">The probability (0.0 to 1.0) of returning the fallback value.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class OrGenerator<T>(Generator<T> innerGenerator, T fallback, float probability, System.Random rng) : Generator<T>(rng)
{
	/// <summary>
	/// Generates a value from the inner generator or returns the fallback.
	/// </summary>
	/// <returns>Either the fallback value or a value from the inner generator.</returns>
	public override T Generate() => Rng.NextDouble() < probability ? fallback : innerGenerator.Generate();
}
