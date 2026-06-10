namespace Forged.Core.Generators.Random;

public sealed class PickManyGenerator<T>(T[] items, int length, System.Random rng) : Generator<T[]>(rng)
{
	public override T[] Generate() => Rng.GetItems(items, length);
}
