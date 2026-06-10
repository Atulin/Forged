namespace Forged.Core.Generators.Utility;

public sealed class NullableOrValueGenerator<T>(Generator<T> innerGenerator, float probability, System.Random rng) : Generator<T?>(rng) where T : struct
{
	public override T? Generate() => Rng.NextDouble() < probability ? null : innerGenerator.Generate();
}
