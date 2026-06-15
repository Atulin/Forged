namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates random items by picking one from a specified collection.
/// </summary>
/// <typeparam name="T">The type of items to pick from.</typeparam>
public sealed class PickOneGenerator<T>(T[] items, Forge forge) : Generator<T>(forge)
{
	/// <summary>
	/// Generates a random item from the collection.
	/// </summary>
	/// <returns>A randomly selected item from the collection.</returns>
	public override T Generate() => items[Rng.Next(items.Length)];
}
