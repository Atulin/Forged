namespace Forged.Core.Generators.Utility.Collections;

public class CollectionGenerator<T>(Generator<T> innerGenerator, int length) : Generator<ICollection<T>>
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