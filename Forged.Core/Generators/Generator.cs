using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;

namespace Forged.Core.Generators;

public abstract class Generator<T>
{
	public required System.Random Rng { get; init; }
	
	public abstract T Generate();
	
	public Generator<T?> OrDefault(float probability) 
		=> new NullableOrGenerator<T>(this, default, probability) { Rng = Rng };
    
	public Generator<T> Or(T other, float probability) 
		=> new OrGenerator<T>(this, other, probability) { Rng = Rng };
	
	public Generator<TNew> Refine<TNew>(Func<T, TNew> refiner)
		=> new RefineGenerator<T, TNew>(this, refiner) { Rng = Rng };
	
	public Generator<T[]> Array(int length)
		=> new ArrayGenerator<T>(this, length) { Rng = Rng };
	
	public Generator<IEnumerable<T>> Enumerable(int length)
		=> new EnumerableGenerator<T>(this, length) { Rng = Rng };
	
	public Generator<ICollection<T>> Collection(int length)
		=> new CollectionGenerator<T>(this, length) { Rng = Rng };
}