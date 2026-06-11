using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;

namespace Forged.Core.Generators;

public abstract class Generator<T>(System.Random rng) : IGenerator<T>
{
	public System.Random Rng { get; } = rng;

	public abstract T Generate();

	public Generator<T> Literal(T literal)
		=> new LiteralGenerator<T>(literal, Rng);
	
	public Generator<T?> OrDefault(float probability)
		=> new NullableOrGenerator<T>(this, default, probability, Rng);

	public Generator<T> Or(T other, float probability)
		=> new OrGenerator<T>(this, other, probability, Rng);

	public Generator<TNew> Refine<TNew>(Func<T, TNew> refiner)
		=> new RefineGenerator<T, TNew>(this, refiner, Rng);
	
	public Generator<IEnumerable<T>> Enumerable(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng);
	
	public Generator<IEnumerable<T>> Enumerable(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng);
	
	public Generator<T[]> Array(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng).Refine<T[]>(static e => e.ToArray());
	
	public Generator<T[]> Array(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng).Refine<T[]>(static e => e.ToArray());

	public Generator<List<T>> List(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng).Refine<List<T>>(static e => e.ToList());
	
	public Generator<List<T>> List(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng).Refine<List<T>>(static e => e.ToList());

	public Generator<HashSet<T>> HashSet(int length)
		=> new EnumerableGenerator<T>(this, length, length, Rng).Refine<HashSet<T>>(static e => e.ToHashSet());
	
	public Generator<HashSet<T>> HashSet(int minLength, int maxLength)
		=> new EnumerableGenerator<T>(this, minLength, maxLength, Rng).Refine<HashSet<T>>(static e => e.ToHashSet());
}
