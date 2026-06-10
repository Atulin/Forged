namespace Forged.Core.Generators.Utility;

public sealed class OrGenerator<T>(Generator<T> innerGenerator, T fallback, float probability, System.Random rng) : Generator<T>(rng)
{
	public override T Generate() => Rng.NextDouble() < probability ? fallback : innerGenerator.Generate();
}
