using System.Numerics;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random numeric values within a specified range.
/// </summary>
/// <typeparam name="T">The numeric type to generate (must implement INumber&lt;T&gt; and IMinMaxValue&lt;T&gt;).</typeparam>
public sealed class NumberGenerator<T>(T? min, T? max, Forge forge) : Generator<T>(forge) where T : struct, INumber<T>, IMinMaxValue<T>
{
	private readonly T _min = min ?? T.MinValue;
	private readonly T _max = max ?? T.MaxValue;

	/// <summary>
	/// Generates a random numeric value.
	/// </summary>
	/// <returns>A random numeric value between the specified minimum and maximum.</returns>
	public override T Generate()
	{
		var min = double.CreateTruncating(_min);
		var max = double.CreateTruncating(_max);
		var value = min + (max - min) * Rng.NextDouble();

		return T.CreateSaturating(value);
	}
}
