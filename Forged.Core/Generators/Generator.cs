using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;

namespace Forged.Core.Generators;

public abstract class Generator<T>(System.Random rng) : IGenerator<T>
{
	public System.Random Rng { get; } = rng;

	public abstract T Generate();

	public Generator<T?> OrDefault(float probability)
		=> new NullableOrGenerator<T>(this, default, probability, Rng);

	public Generator<T?> ToNullable()
		=> Refine(T? (x) => x);

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
}
