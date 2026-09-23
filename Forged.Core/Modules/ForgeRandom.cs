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
	/// Creates a generator that picks a fixed number of random unique items from the specified collection.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">The collection of items to pick from.</param>
	/// <param name="count">The exact number of items to pick each time.</param>
	/// <returns>A generator that produces arrays of random unique items from the collection.</returns>
	public Generator<T[]> PickUnique<T>(T[] items, int count)
		=> new PickManyUniqueGenerator<T>(items, count, count, forge);

	/// <summary>
	/// Creates a generator that picks a variable number of random unique items from the specified collection.
	/// </summary>
	/// <typeparam name="T">The type of items to pick from.</typeparam>
	/// <param name="items">The collection of items to pick from.</param>
	/// <param name="minCount">The minimum number of items to pick.</param>
	/// <param name="maxCount">The maximum number of items to pick.</param>
	/// <returns>A generator that produces arrays of random unique items from the collection.</returns>
	public Generator<T[]> PickUnique<T>(T[] items, int minCount, int maxCount)
		=> new PickManyUniqueGenerator<T>(items, minCount, maxCount, forge);

	/// <summary>
	/// Creates a generator that randomly selects a single value from the provided enum.
	/// </summary>
	/// <typeparam name="T">The type of the enum to select from.</typeparam>
	/// <returns>A generator that produces a randomly selected enum value.</returns>
	public Generator<T> Pick<T>() where T : struct, Enum
		=> new PickEnumGenerator<T>(forge);

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
	/// Creates a generator that produces random numeric values following a uniform distribution within a specified range.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="min">The minimum value (inclusive). If null, uses the minimum value of the type.</param>
	/// <param name="max">The maximum value (inclusive). If null, uses the maximum value of the type.</param>
	/// <returns>A generator that produces random numeric values from a uniform distribution.</returns>
	public Generator<T> Uniform<T>(T? min = null, T? max = null) where T : struct, INumber<T>, IMinMaxValue<T>
		=> Number(min, max);

	/// <summary>
	/// Creates a generator that produces random numeric values following a normal (Gaussian) distribution.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="mean">The mean of the distribution. If null, uses 0.</param>
	/// <param name="stdDev">The standard deviation of the distribution. If null, uses 1.</param>
	/// <returns>A generator that produces random numeric values from a normal distribution.</returns>
	public Generator<T> Normal<T>(T? mean = null, T? stdDev = null) where T : struct, INumber<T>
		=> new NormalGenerator<T>(mean, stdDev, forge);

	/// <summary>
	/// Creates a generator that produces random numeric values following an exponential distribution.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="rate">The rate of the distribution (inverse of the mean). If null, uses 1.</param>
	/// <returns>A generator that produces random non-negative numeric values from an exponential distribution.</returns>
	public Generator<T> Exponential<T>(T? rate = null) where T : struct, INumber<T>
		=> new ExponentialGenerator<T>(rate, forge);

	/// <summary>
	/// Creates a generator that produces random numeric values following a Bernoulli distribution.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="probability">The probability of producing 1. If null, uses 0.5.</param>
	/// <returns>A generator that produces 1 with the specified probability and otherwise 0.</returns>
	public Generator<T> Bernoulli<T>(T? probability = null) where T : struct, INumber<T>
		=> new BernoulliGenerator<T>(probability, forge);

	/// <summary>
	/// Creates a generator that produces random non-negative integer values following a Poisson distribution.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="lambda">The mean (and variance) of the distribution. If null, uses 1.</param>
	/// <returns>A generator that produces random non-negative integer values from a Poisson distribution.</returns>
	public Generator<T> Poisson<T>(T? lambda = null) where T : struct, INumber<T>
		=> new PoissonGenerator<T>(lambda, forge);

	/// <summary>
	/// Creates a generator that produces random positive numeric values following a log-normal distribution.
	/// </summary>
	/// <typeparam name="T">The numeric type to generate.</typeparam>
	/// <param name="mean">The mean of the underlying normal distribution (natural logarithm). If null, uses 0.</param>
	/// <param name="stdDev">The standard deviation of the underlying normal distribution. If null, uses 1.</param>
	/// <returns>A generator that produces random positive numeric values from a log-normal distribution.</returns>
	public Generator<T> LogNormal<T>(T? mean = null, T? stdDev = null) where T : struct, INumber<T>
		=> new LogNormalGenerator<T>(mean, stdDev, forge);

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

	/// <summary>
	/// Creates a random integer generator based on a dice expression, allowing for complex dice roll calculations with optional rounding.
	/// </summary>
	/// <param name="expression">The dice expression to evaluate (e.g., "2d6+1d10-3"), which specifies the dice type, quantity, and modifiers.</param>
	/// <param name="mode">The rounding mode to apply to the result (Floor, Round, or Ceiling). Defaults to RoundingMode.Round.</param>
	/// <returns>A generator that produces integers based on the specified dice expression and rounding mode.</returns>
	public Generator<int> Dice(string expression, RoundingMode mode = RoundingMode.Round)
		=> new DiceGenerator(expression, mode, forge);
}
