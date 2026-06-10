namespace Forged.Core.Generators.Utility.Collections;

public sealed class ListGenerator<T>(Generator<T> innerGenerator, int length, System.Random rng) : Generator<List<T>>(rng)
{
	public override List<T> Generate()
	{
		var list = new List<T>(length);
		for (var i = 0; i < length; i++)
		{
			list.Add(innerGenerator.Generate());
		}
		return list;
	}
}
