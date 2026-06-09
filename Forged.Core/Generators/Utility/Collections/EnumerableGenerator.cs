namespace Forged.Core.Generators.Utility.Collections;

public class EnumerableGenerator<T>(Generator<T> innerGenerator, int length) : Generator<IEnumerable<T>>
{
	public override IEnumerable<T> Generate()
	{
		for (var i = 0; i < length; i++)
		{
			yield return innerGenerator.Generate();
		}
	}
}