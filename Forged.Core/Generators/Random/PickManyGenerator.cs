namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates arrays of random items by picking multiple from a specified collection.
/// </summary>
/// <typeparam name="T">The type of items to pick from.</typeparam>
public sealed class PickManyGenerator<T>(T[] items, int minLength, int maxLength, Forge forge) : Generator<T[]>(forge)
{
	/// <summary>
	/// Generates an array of randomly selected items from the collection.
	/// </summary>
	/// <returns>An array of randomly selected items.</returns>
	public override T[] Generate() => minLength == maxLength 
		? Rng.GetItems(items, minLength) 
		: Rng.GetItems(items, Rng.Next(minLength, maxLength));
}
