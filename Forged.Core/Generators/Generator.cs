using Forged.Core.Generators.Utility;

namespace Forged.Core.Generators;

public abstract class Generator<T>
{
	public System.Random Rng { protected get; init; } = System.Random.Shared;
	
	public abstract T Generate();
	
	public Generator<T?> OrNull(float probability) 
		=> new NullableOrGenerator<T>(this, default, probability);
    
	public Generator<T?> OrDefault(float probability) 
		=> new NullableOrGenerator<T>(this, default, probability);
    
	public Generator<T> Or(T other, float probability) 
		=> new OrGenerator<T>(this, other, probability);
	
	public Generator<T> Refine(Func<T, T> refiner)
		=> new RefineGenerator<T>(this, refiner);
}