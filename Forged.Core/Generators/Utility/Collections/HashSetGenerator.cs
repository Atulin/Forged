namespace Forged.Core.Generators.Utility.Collections;

public sealed class HashSetGenerator<T>(Generator<T> innerGenerator, int length, System.Random rng) : Generator<HashSet<T>>(rng)
{
	public override HashSet<T> Generate()
	{
		var set = new HashSet<T>(length);
		for (var i = 0; i < length; i++)
		{
			set.Add(innerGenerator.Generate());
		}
		return set;
	}
}
