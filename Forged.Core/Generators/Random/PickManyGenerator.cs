namespace Forged.Core.Generators.Random;

public sealed class PickManyGenerator<T>(T[] items, int minLength, int maxLength, System.Random rng) : Generator<T[]>(rng)
{
	public override T[] Generate() => minLength == maxLength 
		? Rng.GetItems(items, minLength) 
		: Rng.GetItems(items, Rng.Next(minLength, maxLength));
}
