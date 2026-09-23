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

		internal T GetItem<T>(List<T> items)
		{
			return items[rng.Next(items.Count)];
		}
		
		internal IEnumerable<T> GetShuffled<T>(params T[] items)
		{
			var indexes = Enumerable.Range(0, items.Length).ToArray();
			rng.Shuffle(indexes);
			foreach (var i in indexes)
			{
				yield return items[i];
			}
		}
	}
}