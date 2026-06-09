namespace Forged.Core.Generators.Random;

public sealed class PickManyGenerator<T>(T[] items, int length) : Generator<T[]>
{
	public override T[] Generate() => Rng.GetItems(items, length);
}