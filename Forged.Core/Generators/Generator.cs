using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;

namespace Forged.Core.Generators;

public abstract class Generator<T> : IGenerator<T>
{
	/// <summary>The Random instance used by this generator and all generators derived from it.</summary>
	public System.Random Rng { get; }

	protected Generator(System.Random rng)
	{
		Rng = rng;
	}

	public abstract T Generate();

	public Generator<T?> OrDefault(float probability)
		=> new NullableOrGenerator<T>(this, default, probability, Rng);

	public Generator<T> Or(T other, float probability)
		=> new OrGenerator<T>(this, other, probability, Rng);

	public Generator<TNew> Refine<TNew>(Func<T, TNew> refiner)
		=> new RefineGenerator<T, TNew>(this, refiner, Rng);

	public Generator<T[]> Array(int length)
		=> new ArrayGenerator<T>(this, length, Rng);

	public Generator<IEnumerable<T>> Enumerable(int length)
		=> new EnumerableGenerator<T>(this, length, Rng);

	public Generator<ICollection<T>> Collection(int length)
		=> new CollectionGenerator<T>(this, length, Rng);

	public Generator<List<T>> List(int length)
		=> new ListGenerator<T>(this, length, Rng);

	public Generator<HashSet<T>> HashSet(int length)
		=> new HashSetGenerator<T>(this, length, Rng);

	/// <summary>
	/// Implicitly lifts a Generator&lt;T&gt; (where T is a value type) to
	/// Generator&lt;T?&gt;, allowing it to be assigned to nullable properties
	/// without calling .OrNull(0f) or .Refine().
	/// The generated values are never null — this is a pure type-widening lift.
	/// </summary>
	public static implicit operator Generator<T?>(Generator<T> generator)
		where T : struct
		=> new NullableOrValueGenerator<T>(generator, 0f, generator.Rng);
}
