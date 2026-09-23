using System.Numerics;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random non-negative integer values following a Poisson distribution.
/// </summary>
/// <typeparam name="T">The numeric type to generate (must implement INumber&lt;T&gt;).</typeparam>
public sealed class PoissonGenerator<T>(T? lambda, Forge forge) : Generator<T>(forge) where T : struct, INumber<T>
{
	private readonly double _lambda = lambda is null ? 1 : Validate(double.CreateTruncating(lambda.Value));

	private static double Validate(double lambda)
		=> lambda >= 0
			? lambda
			: throw new ArgumentOutOfRangeException(nameof(lambda), lambda, "The lambda (mean) must be greater than or equal to zero.");

	/// <summary>
	/// Generates a random non-negative integer value from a Poisson distribution using Knuth's algorithm
	/// (a normal approximation is used when the lambda is large enough that the exponential underflows).
	/// </summary>
	/// <returns>A random non-negative integer value from a Poisson distribution with the specified lambda.</returns>
	public override T Generate()
	{
		if (_lambda == 0)
		{
			return T.CreateSaturating(0);
		}

		var threshold = Math.Exp(-_lambda);
		if (threshold == 0)
		{
			var u1 = 1.0 - Rng.NextDouble();
			var u2 = Rng.NextDouble();
			var z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
			var value = Math.Max(0, Math.Round(_lambda + Math.Sqrt(_lambda) * z));

			return T.CreateSaturating(value);
		}

		var count = 0;
		var product = 1.0;
		do
		{
			count++;
			product *= Rng.NextDouble();
		} while (product > threshold);

		return T.CreateSaturating(count - 1);
	}
}