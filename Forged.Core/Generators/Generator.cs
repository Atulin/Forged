using Forged.Core.Generators.Utility;
using Forged.Core.Generators.Utility.Collections;

namespace Forged.Core.Generators;

public abstract class Generator<T>
{
	public System.Random Rng { get; set; } = Global.Random;
	
	public abstract T Generate();
	
	public Generator<T?> OrNull(float probability) 
		=> new NullableOrGenerator<T>(this, default, probability);
    
	public Generator<T?> OrDefault(float probability) 
		=> new NullableOrGenerator<T>(this, default, probability);
    
	public Generator<T> Or(T other, float probability) 
		=> new OrGenerator<T>(this, other, probability);
	
	public Generator<TNew> Refine<TNew>(Func<T, TNew> refiner)
		=> new RefineGenerator<T, TNew>(this, refiner);
	
	public Generator<T[]> Array(int length)
		=> new ArrayGenerator<T>(this, length);
	
	public Generator<IEnumerable<T>> Enumerable(int length)
		=> new EnumerableGenerator<T>(this, length);
	
	public Generator<ICollection<T>> Collection(int length)
		=> new CollectionGenerator<T>(this, length);
}