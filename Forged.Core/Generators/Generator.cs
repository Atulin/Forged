using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;

namespace Forged.Core.Generators;

/// <summary>
/// Base class for all generators. Provides common methods for transforming and combining generators.
/// </summary>
/// <typeparam name="T">The type of value generated.</typeparam>
public abstract class Generator<T>(System.Random rng) : IGenerator<T>
{
	/// <summary>
	/// Gets the underlying random number generator.
	/// </summary>
	public System.Random Rng { get; } = rng;

	/// <summary>
	/// Generates a random value of type <typeparamref name="T"/>
	/// </summary>
	/// <returns>A randomly generated value.</returns>
	public abstract T Generate();

	/// <summary>
	/// Creates a generator that always produces the specified literal value.
	/// </summary>
	/// <param name="literal">The literal value to always produce.</param>
	/// <returns>A generator that always produces the specified value.</returns>
	public Generator<T> Literal(T literal)
		=> new LiteralGenerator<T>(literal, Rng);
	
	/// <summary>
	/// Creates a generator that produces the default value with a specified probability,
	/// otherwise produces values from the current generator.
	/// </summary>
	/// <param name="probability">The probability (0.0 to 1.0) of producing the default value.</param>
	/// <returns>A generator that may produce default values.</returns>
	public Generator<T?> OrDefault(float probability)
		=> new NullableOrGenerator<T>(this, default, probability, Rng);

	/// <summary>
	/// Creates a generator that produces a specified alternative value with a specified probability,
	/// otherwise produces values from the current generator.
	/// </summary>
	/// <param name="other">The alternative value to produce.</param>
	/// <param name="probability">The probability (0.0 to 1.0) of producing the alternative value.</param>
	/// <returns>A generator that may produce alternative values.</returns>
	public Generator<T> Or(T other, float probability)
		=> new OrGenerator<T>(this, other, probability, Rng);

	/// <summary>
	/// Creates a generator that transforms the generated values using a refiner function.
	/// </summary>
	/// <typeparam name="TNew">The new type after transformation.</typeparam>
	/// <param name="refiner">A function that transforms the generated value.</param>
	/// <returns>A generator that produces transformed values.</returns>
	public Generator<TNew> Refine<TNew>(Func<T, TNew> refiner)
		=> new RefineGenerator<T, TNew>(this, refiner, Rng);
	
	/// <summary>
	/// Creates a generator that produces an enumerable of generated values with a fixed length.
	/// </summary>
	/// <param name="length">The exact number of values to generate.</param>
	/// <returns>A generator that produces enumerables of generated values.</returns>
	public Generator<IEnumerable<T>> Enumerable(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng);
	
	/// <summary>
	/// Creates a generator that produces an enumerable of generated values with a variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of values to generate.</param>
	/// <param name="maxLength">The maximum number of values to generate.</param>
	/// <returns>A generator that produces enumerables of generated values.</returns>
	public Generator<IEnumerable<T>> Enumerable(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng);
	
	/// <summary>
	/// Creates a generator that produces an array of generated values with a fixed length.
	/// </summary>
	/// <param name="length">The exact number of values to generate.</param>
	/// <returns>A generator that produces arrays of generated values.</returns>
	public Generator<T[]> Array(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng).Refine<T[]>(static e => e.ToArray());
	
	/// <summary>
	/// Creates a generator that produces an array of generated values with a variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of values to generate.</param>
	/// <param name="maxLength">The maximum number of values to generate.</param>
	/// <returns>A generator that produces arrays of generated values.</returns>
	public Generator<T[]> Array(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng).Refine<T[]>(static e => e.ToArray());
	
	/// <summary>
	/// Creates a generator that produces a list of generated values with a fixed length.
	/// </summary>
	/// <param name="length">The exact number of values to generate.</param>
	/// <returns>A generator that produces lists of generated values.</returns>
	public Generator<List<T>> List(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng).Refine<List<T>>(static e => e.ToList());
	
	/// <summary>
	/// Creates a generator that produces a list of generated values with a variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of values to generate.</param>
	/// <param name="maxLength">The maximum number of values to generate.</param>
	/// <returns>A generator that produces lists of generated values.</returns>
	public Generator<List<T>> List(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng).Refine<List<T>>(static e => e.ToList());
	
	/// <summary>
	/// Creates a generator that produces a hash set of generated values with a fixed length.
	/// </summary>
	/// <param name="length">The exact number of values to generate.</param>
	/// <returns>A generator that produces hash sets of generated values.</returns>
	public Generator<HashSet<T>> HashSet(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng).Refine<HashSet<T>>(static e => e.ToHashSet());
	
	/// <summary>
	/// Creates a generator that produces a hash set of generated values with a variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of values to generate.</param>
	/// <param name="maxLength">The maximum number of values to generate.</param>
	/// <returns>A generator that produces hash sets of generated values.</returns>
	public Generator<HashSet<T>> HashSet(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng).Refine<HashSet<T>>(static e => e.ToHashSet());
}
