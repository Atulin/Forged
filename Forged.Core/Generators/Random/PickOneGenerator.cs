namespace Forged.Core.Generators.Random;

public sealed class PickOneGenerator<T>(T[] items) : Generator<T>
{
	public override T Generate() => items[Rng.Next(items.Length)];
}