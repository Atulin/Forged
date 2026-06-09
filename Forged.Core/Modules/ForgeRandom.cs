using System.Numerics;
using Forged.Core.Generators;
using Forged.Core.Generators.Random;

namespace Forged.Core.Modules;

public sealed class ForgeRandom(Forge forge)
{
	public Generator<T> Pick<T>(params T[] items) 
		=> new PickOneGenerator<T>(items) { Rng = forge.Rng };

    public Generator<T[]> Pick<T>(T[] items, int count) 
	    => new PickManyGenerator<T>(items, count) { Rng = forge.Rng };

    public Generator<T> Number<T>(T? min = null, T? max = null) where T : struct, INumber<T>, IMinMaxValue<T> 
	    => new NumberGenerator<T>(min, max) { Rng = forge.Rng };
    
    public Generator<T> WeightedPick<T>(T[] items, float[] weights) 
	    => new WeightedPickGenerator<T>(items, weights) { Rng = forge.Rng };
}
