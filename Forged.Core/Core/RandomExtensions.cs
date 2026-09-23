using System.Collections.Immutable;

namespace Forged.Core.Core;

internal static class RandomExtensions
{
	extension(Random rng)
	{
		internal bool Chance(float chance) => chance switch
		{
			<= 0 => false,
			> 1 => true,
			_ => rng.NextSingle() < chance
		};

		internal T GetItem<T>(params T[] items) => items[rng.Next(items.Length)];

		internal T GetItem<T>(List<T> items) => items[rng.Next(items.Count)];

		internal T GetItem<T>(ImmutableArray<T> items) => items[rng.Next(items.Length)];

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