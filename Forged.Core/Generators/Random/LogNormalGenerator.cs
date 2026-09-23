using System.Numerics;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random positive numeric values following a log-normal distribution, i.e. whose natural logarithm is normally distributed.
/// </summary>
/// <typeparam name="T">The numeric type to generate (must implement INumber&lt;T&gt;).</typeparam>
public sealed class LogNormalGenerator<T>(T? mean, T? stdDev, Forge forge) : Generator<T>(forge) where T : struct, INumber<T>
{
	private readonly double _mean = mean is null ? 0 : double.CreateTruncating(mean.Value);
	private readonly double _stdDev = stdDev is null ? 1 : double.CreateTruncating(stdDev.Value);

	/// <summary>
	/// Generates a random numeric value from a log-normal distribution using the Box-Muller transform.
	/// </summary>
	/// <returns>A positive random numeric value whose natural logarithm follows a normal distribution with the specified mean and standard deviation.</returns>
	public override T Generate()
	{
		var u1 = 1.0 - Rng.NextDouble();
		var u2 = Rng.NextDouble();
		var z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

		return T.CreateSaturating(Math.Exp(_mean + _stdDev * z));
	}
}