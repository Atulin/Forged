namespace Forged.Core.Generators.Utility.Collections;

/// <summary>
/// Represents a generator that produces shuffled sequences of elements. This generator wraps an existing generator
/// that produces collections and applies a random shuffle to the generated collection before returning it.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public sealed class ShuffleGenerator<T>(Generator<IEnumerable<T>> innerGenerator, Forge forge) : Generator<IEnumerable<T>>(forge)
{
	public override IEnumerable<T> Generate()
	{
		var collection = innerGenerator.Generate().ToArray();
		Rng.Shuffle(collection.AsSpan());
		return collection;
	}
}