namespace Forged.Core.Generators.Utility;

public sealed class OrGenerator<T>(Generator<T> innerGenerator, T fallback, float probability = 0.5f) : Generator<T>
{
	public override T Generate() => Rng.NextDouble() < probability ? fallback : innerGenerator.Generate();
}