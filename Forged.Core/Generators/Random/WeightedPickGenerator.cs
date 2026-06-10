namespace Forged.Core.Generators.Random;

public sealed class WeightedPickGenerator<T>(T[] items, float[] weights, System.Random rng) : Generator<T>(rng)
{
	public WeightedPickGenerator((T item, float weight)[] items, System.Random rng)
		: this(
			items.Select(i => i.item).ToArray(),
			items.Select(i => i.weight).ToArray(),
			rng
		)
	{}

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
