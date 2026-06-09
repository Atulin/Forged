using System.Numerics;
using Forged.Core.Generators;
using Forged.Core.Generators.Random;

namespace Forged.Core;

public partial class Forged<TModel>
{
	public static class Random
	{
		public static Generator<T> Pick<T>(params T[] items) => new PickOneGenerator<T>(items);

		public static Generator<T[]> Pick<T>(T[] items, int count) => new PickManyGenerator<T>(items, count);

		public static Generator<T> Number<T>(T? min = null, T? max = null) where T : struct, INumber<T>, IMinMaxValue<T> => new NumberGenerator<T>(min, max);
		
		public static Generator<T> WeightedPick<T>(T[] items, float[] weights) => new WeightedPickGenerator<T>(items, weights);
	}
}