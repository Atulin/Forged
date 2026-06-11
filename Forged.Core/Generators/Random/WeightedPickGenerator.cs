namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random items from a collection using specified weights.
/// </summary>
/// <typeparam name="T">The type of items to pick from.</typeparam>
public sealed class WeightedPickGenerator<T>(T[] items, float[] weights, System.Random rng) : Generator<T>(rng)
{
	/// <summary>
	/// Initializes a new instance with item-weight pairs.
	/// </summary>
	/// <param name="items">An array of tuples containing items and their corresponding weights.</param>
	/// <param name="rng">The random number generator to use.</param>
	public WeightedPickGenerator((T item, float weight)[] items, System.Random rng)
		: this(
			items.Select(i => i.item).ToArray(),
			items.Select(i => i.weight).ToArray(),
			rng
		)
	{}

	/// <summary>
	/// Generates a random item based on the specified weights.
	/// </summary>
	/// <returns>A randomly selected item based on weights.</returns>
	/// <exception cref="ArgumentException">Thrown when items and weights arrays have different lengths.</exception>
	public override T Generate()
	{
		if (items.Length != weights.Length)
		{
			throw new ArgumentException("Items and weights must have the same length.");
		}

		var totalWeight = weights.Sum();
		var randomValue = Rng.NextDouble() * totalWeight;

		for (var i = 0; i < items.Length; i++)
		{
			randomValue -= weights[i];
			if (randomValue <= 0)
			{
				return items[i];
			}
		}

		return items[^1];
	}
}
