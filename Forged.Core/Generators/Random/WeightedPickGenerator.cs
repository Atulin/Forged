namespace Forged.Core.Generators.Random;

public sealed class WeightedPickGenerator<T> : Generator<T>
{
	private readonly T[] _items;
	private readonly float[] _weights;

	public WeightedPickGenerator(T[] items, float[] weights, System.Random rng) : base(rng)
	{
		_items = items;
		_weights = weights;
	}

	public WeightedPickGenerator((T item, float weight)[] items, System.Random rng)
		: this(
			items.Select(i => i.item).ToArray(),
			items.Select(i => i.weight).ToArray(),
			rng
		)
	{}

	public override T Generate()
	{
		if (_items.Length != _weights.Length)
		{
			throw new ArgumentException("Items and weights must have the same length.");
		}

		var totalWeight = _weights.Sum();
		var randomValue = Rng.NextDouble() * totalWeight;

		for (var i = 0; i < _items.Length; i++)
		{
			randomValue -= _weights[i];
			if (randomValue <= 0)
			{
				return _items[i];
			}
		}

		return _items[^1];
	}
}
