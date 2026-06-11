namespace Forged.Core.Generators.Utility.Collections;

public sealed class EnumerableGenerator<T>(Generator<T> innerGenerator, int minLength, int maxLength, System.Random rng) : Generator<IEnumerable<T>>(rng)
{
	public override IEnumerable<T> Generate()
	{
		var length = minLength == maxLength 
			? minLength 
			: Rng.Next(minLength, maxLength + 1);
		
		for (var i = 0; i < length; i++)
		{
			yield return innerGenerator.Generate();
		}
	}
}
