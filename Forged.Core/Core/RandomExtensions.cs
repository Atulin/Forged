namespace Forged.Core.Core;

internal static class RandomExtensions
{
	extension(Random rng)
	{
		internal bool Chance(float chance)
		{
			return rng.NextSingle() < chance;
		}

		internal T GetItem<T>(params T[] items)
		{
			return items[rng.Next(items.Length)];
		}
	}
}