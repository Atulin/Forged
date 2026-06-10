namespace Forged.Core.Generators.Utility.Collections;

public sealed class EnumerableGenerator<T>(Generator<T> innerGenerator, int length, System.Random rng) : Generator<IEnumerable<T>>(rng)
{
	public override IEnumerable<T> Generate()
	{
		for (var i = 0; i < length; i++)
		{
			yield return innerGenerator.Generate();
		}
	}
}
