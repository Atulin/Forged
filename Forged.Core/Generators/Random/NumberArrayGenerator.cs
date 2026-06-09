using System.Numerics;

namespace Forged.Core.Generators.Random;

public sealed class NumberArrayGenerator<T>(int count, T? min = null, T? max = null) : Generator<T[]> where T : struct, INumber<T>, IMinMaxValue<T>
{
	private readonly T _min = min ?? T.MinValue;
	private readonly T _max = max ?? T.MaxValue;
	
	public override T[] Generate()
	{
		return PrivateGenerate().ToArray();
	}

	private IEnumerable<T> PrivateGenerate()
	{
		var range = _max - _min;

		for (var i = 0; i < count; i++)
		{
			var ratio = Rng.NextDouble();
			var scale = T.CreateChecked<double>(ratio);
		
			yield return _min + range * scale;
		}
	}
}