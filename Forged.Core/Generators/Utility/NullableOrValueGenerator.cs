namespace Forged.Core.Generators.Utility;

/// <summary>
/// Generates either a value from an inner generator or null based on a probability.
/// </summary>
/// <typeparam name="T">The value type to generate (must be a struct).</typeparam>
/// <param name="innerGenerator">The inner generator to use when not returning null.</param>
/// <param name="probability">The probability (0.0 to 1.0) of returning null.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class NullableOrValueGenerator<T>(Generator<T> innerGenerator, float probability, Forge forge) : Generator<T?>(forge) where T : struct
{
	/// <summary>
	/// Generates a value from the inner generator or returns null.
	/// </summary>
	/// <returns>Either null or a value from the inner generator.</returns>
	public override T? Generate() => Rng.NextDouble() < probability ? null : innerGenerator.Generate();
}
