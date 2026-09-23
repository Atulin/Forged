using System.Numerics;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random numeric values following a Bernoulli distribution, producing 1 with a specified probability and otherwise 0.
/// </summary>
/// <typeparam name="T">The numeric type to generate (must implement INumber&lt;T&gt;).</typeparam>
public sealed class BernoulliGenerator<T>(T? probability, Forge forge) : Generator<T>(forge) where T : struct, INumber<T>
{
	private readonly double _probability = probability is null ? 0.5 : Validate(double.CreateTruncating(probability.Value));

	private static double Validate(double probability)
		=> probability is >= 0 and <= 1
			? probability
			: throw new ArgumentOutOfRangeException(nameof(probability), probability, "The probability must be between 0 and 1.");

	/// <summary>
	/// Generates a random numeric value from a Bernoulli distribution.
	/// </summary>
	/// <returns>1 with the specified probability, otherwise 0.</returns>
	public override T Generate()
	{
		var value = Rng.NextDouble() < _probability ? 1 : 0;

		return T.CreateSaturating(value);
	}
}