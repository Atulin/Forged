using System.Numerics;
using Forged.Core.Generators;
using Forged.Core.Generators.Random;

namespace Forged.Core.Modules;

/// <summary>
/// Provides methods for generating random data.
/// </summary>
public sealed class ForgeRandom(Forge forge)
{
	/// <summary>
	/// Creates a generator that simulates a coin toss, producing a random boolean value representing heads (true) or tails (false).
	/// </summary>
	public Generator<bool> CoinToss()
		=> new CoinTossGenerator<bool>(forge);
	
	/// <summary>
	/// Creates a generator that picks a random item from the specified collection.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">The collection of items to pick from.</param>
	/// <returns>A generator that produces random items from the collection.</returns>
	public Generator<T> Pick<T>(params T[] items)
		=> new PickOneGenerator<T>(items, forge);

	/// <summary>
	/// Creates a generator that picks a fixed number of random items from the specified collection.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">The collection of items to pick from.</param>
	/// <param name="count">The exact number of items to pick each time.</param>
	/// <returns>A generator that produces arrays of random items from the collection.</returns>
	public Generator<T[]> Pick<T>(T[] items, int count)
		=> new PickManyGenerator<T>(items, count, count, forge);

	/// <summary>
	/// Creates a generator that picks a variable number of random items from the specified collection.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">The collection of items to pick from.</param>
	/// <param name="minCount">The minimum number of items to pick.</param>
	/// <param name="maxCount">The maximum number of items to pick.</param>
	/// <returns>A generator that produces arrays of random items from the collection.</returns>
	public Generator<T[]> Pick<T>(T[] items, int minCount, int maxCount)
		=> new PickManyGenerator<T>(items, minCount, maxCount, forge);

	/// <summary>
	/// Creates a generator that produces random numeric values within a specified range.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="min">The minimum value (inclusive). If null, uses the minimum value of the type.</param>
	/// <param name="max">The maximum value (inclusive). If null, uses the maximum value of the type.</param>
	/// <returns>A generator that produces random numeric values.</returns>
	public Generator<T> Number<T>(T? min = null, T? max = null) where T : struct, INumber<T>, IMinMaxValue<T>
		=> new NumberGenerator<T>(min, max, forge);

	/// <summary>
	/// Creates a generator that picks random items from a collection using specified weights.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">The collection of items to pick from.</param>
	/// <param name="weights">The weights corresponding to each item (higher weight = higher probability).</param>
	/// <returns>A generator that produces random items based on their weights.</returns>
	public Generator<T> WeightedPick<T>(T[] items, float[] weights)
		=> new WeightedPickGenerator<T>(items, weights, forge);

	/// <summary>
	/// Creates a generator that picks random items from a collection of item-weight pairs.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">An array of tuples containing items and their corresponding weights.</param>
	/// <returns>A generator that produces random items based on their weights.</returns>
	public Generator<T> WeightedPick<T>((T item, float weight)[] items)
		=> new WeightedPickGenerator<T>(items, forge);
}
