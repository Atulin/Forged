using Forged.Core.Generators;
using Forged.Core.Generators.Utility;

namespace Forged.Core.Extensions;

/// <summary>
/// Extension methods for struct generators.
/// </summary>
public static class StructGeneratorExtensions
{
	/// <summary>
	/// Extension methods for struct generators.
	/// </summary>
	extension<T>(Generator<T> generator) where T : struct
	{
		/// <summary>
		/// Creates a generator that produces null values with a specified probability.
		/// </summary>
		/// <param name="probability">The probability (0.0 to 1.0) of producing null.</param>
		/// <returns>A generator that may produce null values.</returns>
		public Generator<T?> OrNull(float probability)
			=> new NullableOrValueGenerator<T>(generator, probability, generator.Forge);
		
		/// <summary>
		/// Creates a generator that produces nullable values.
		/// </summary>
		/// <returns>A generator that produces nullable values.</returns>
		public Generator<T?> Nullable()
			=> generator.Refine(x => (T?)x);
	}
}
