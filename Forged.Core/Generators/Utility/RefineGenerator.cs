namespace Forged.Core.Generators.Utility;

public sealed class RefineGenerator<T, TNew>(Generator<T> innerGenerator, Func<T, TNew> refiner, System.Random rng) : Generator<TNew>(rng)
{
	public override TNew Generate()
	{
		var value = innerGenerator.Generate();
		return refiner(value);
	}
}
