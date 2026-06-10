namespace Forged.Core.Generators.Utility.Collections;

public sealed class ArrayGenerator<T>(Generator<T> innerGenerator, int length, System.Random rng) : Generator<T[]>(rng)
{
	public override T[] Generate()
	{
		var arr = new T[length];
		for (var i = 0; i < length; i++)
		{
			arr[i] = innerGenerator.Generate();
		}
		return arr;
	}
}
