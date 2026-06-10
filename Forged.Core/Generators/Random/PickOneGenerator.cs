namespace Forged.Core.Generators.Random;

public sealed class PickOneGenerator<T>(T[] items, System.Random rng) : Generator<T>(rng)
{
	public override T Generate() => items[Rng.Next(items.Length)];
}
