namespace Forged.Core.Generators.Utility.Collections;

/// <summary>
/// Generates enumerables of values from an inner generator.
/// </summary>
/// <typeparam name="T">The type of values to generate.</typeparam>
/// <param name="innerGenerator">The inner generator to use for generating individual values.</param>
/// <param name="minLength">The minimum number of values to generate.</param>
/// <param name="maxLength">The maximum number of values to generate.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class EnumerableGenerator<T>(Generator<T> innerGenerator, int minLength, int maxLength, System.Random rng) : Generator<IEnumerable<T>>(rng)
{
	/// <summary>
	/// Generates an enumerable of random values.
	/// </summary>
	/// <returns>An enumerable of randomly generated values.</returns>
	public override IEnumerable<T> Generate()
	{
		var length = minLength == maxLength 
			? minLength 
			: Rng.Next(minLength, maxLength + 1);
		
		for (var i = 0; i < length; i++)
		{
			yield return innerGenerator.Generate();
		}
	}
}
