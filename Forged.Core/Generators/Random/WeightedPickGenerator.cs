namespace Forged.Core.Generators.Random;

public sealed class WeightedPickGenerator<T>(T[] items, float[] weights) : Generator<T>
{
	public WeightedPickGenerator((T item, float weight)[] items)
		: this(
			items.Select(i => i.item).ToArray(),
			items.Select(i => i.weight).ToArray()
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