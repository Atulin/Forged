using System.Collections.Concurrent;

namespace Forged.Core.Generators.Random;

/// <summary>
/// A generator for producing random values from a specified enumeration type.
/// </summary>
/// <typeparam name="T">
/// The enumeration type from which values will be randomly selected.
/// This type must be a struct and must implement the <see cref="System.Enum"/> constraint.
/// </typeparam>
public sealed class PickEnumGenerator<T>(Forge forge) : Generator<T>(forge) where T : struct, Enum
{
	private static class Cache
	{
		public static readonly ConcurrentDictionary<Type, T[]> Values = new();
	}

	public override T Generate()
	{
		var values = Cache.Values.GetOrAdd(typeof(T), _ => Enum.GetValues<T>());
		return values[Rng.Next(values.Length)];
	}
}