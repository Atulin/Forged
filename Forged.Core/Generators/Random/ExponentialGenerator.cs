using System.Numerics;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random numeric values following an exponential distribution.
/// </summary>
/// <typeparam name="T">The numeric type to generate (must implement INumber&lt;T&gt;).</typeparam>
public sealed class ExponentialGenerator<T>(T? rate, Forge forge) : Generator<T>(forge) where T : struct, INumber<T>
{
	private readonly double _rate = rate is null ? 1 : Validate(double.CreateTruncating(rate.Value));

	private static double Validate(double rate)
		=> rate > 0
			? rate
			: throw new ArgumentOutOfRangeException(nameof(rate), rate, "The rate must be greater than zero.");

	/// <summary>
	/// Generates a random numeric value from an exponential distribution using inverse transform sampling.
	/// </summary>
	/// <returns>A non-negative random numeric value from an exponential distribution with the specified rate.</returns>
	public override T Generate()
	{
		var u = 1.0 - Rng.NextDouble();
		var value = -Math.Log(u) / _rate;

		return T.CreateSaturating(value);
	}
}