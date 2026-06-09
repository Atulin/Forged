namespace Forged.Core.Generators.Utility;

public sealed class NullableOrValueGenerator<T>(Generator<T> innerGenerator, float probability = 0.5f) : Generator<T?> where T : struct
{
	public override T? Generate() => Rng.NextDouble() < probability ? null : innerGenerator.Generate();
}