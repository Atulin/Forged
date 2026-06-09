namespace Forged.Core.Generators.Utility.Collections;

public class ArrayGenerator<T>(Generator<T> innerGenerator, int length) : Generator<T[]>
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