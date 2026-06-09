using System.Numerics;

namespace Forged.Core.Generators.Random;

public sealed class NumberGenerator<T>(T? min = null, T? max = null) : Generator<T> where T : struct, INumber<T>, IMinMaxValue<T>
{
	private readonly T _min = min ?? T.MinValue;
	private readonly T _max = max ?? T.MaxValue;
	
	public override T Generate()
	{
		var range = _max - _min;
		var ratio = Rng.NextDouble();
		var scale = T.CreateChecked<double>(ratio);
		
		return _min + range * scale;
	}
}