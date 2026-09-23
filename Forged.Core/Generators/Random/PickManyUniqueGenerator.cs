using Forged.Core.Core;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Generates arrays of random unique items by picking multiple from a specified collection.
/// </summary>
/// <typeparam name="T">The type of items to pick from.</typeparam>
public sealed class PickManyUniqueGenerator<T>(T[] items, int minLength, int maxLength, Forge forge) : Generator<T[]>(forge)
{
	/// <summary>
	/// Generates an array of randomly selected unique items from the collection.
	/// </summary>
	/// <returns>An array of randomly selected unique items.</returns>
	public override T[] Generate()
	{
		var length = minLength == maxLength 
			? minLength 
			: Rng.Next(minLength, maxLength);
		
		var set = new HashSet<T>(items);
		var shuffled = Rng.GetShuffled([.. set]).Take(length);
		
		return [.. shuffled];
	}
}
