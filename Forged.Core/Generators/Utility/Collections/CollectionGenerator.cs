namespace Forged.Core.Generators.Utility.Collections;

public sealed class CollectionGenerator<T>(Generator<T> innerGenerator, int length, System.Random rng) : Generator<ICollection<T>>(rng)
{
	public override ICollection<T> Generate()
	{
		var collection = new List<T>(length);
		for (var i = 0; i < length; i++)
		{
			collection.Add(innerGenerator.Generate());
		}
		return collection;
	}
}
