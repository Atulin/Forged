using Forged.Core.Generators;
using Forged.Core.Generators.Utility;

namespace Forged.Core.Extensions;

/// <summary>
/// Extension methods for ICollection generators.
/// </summary>
public static class CollectionGeneratorExtensions
{
	/// <param name="generator">The generator to convert.</param>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	extension<T>(Generator<ICollection<T>> generator)
	{
		/// <summary>
		/// Converts collections from a generator to lists.
		/// </summary>
		/// <returns>A generator that produces lists.</returns>
		public Generator<List<T>> AsList()
			=> new RefineGenerator<ICollection<T>, List<T>>(generator, static c => c.ToList(), generator.Forge);
		
		/// <summary>
		/// Converts collections from a generator to hash sets.
		/// </summary>
		/// <returns>A generator that produces hash sets.</returns>
		public Generator<HashSet<T>> AsHashSet()
			=> new RefineGenerator<ICollection<T>, HashSet<T>>(generator, static c => c.ToHashSet(), generator.Forge);
	}
}
