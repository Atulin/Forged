using Forged.Core.Generators;
using Forged.Core.Generators.Basic;

namespace Forged.Core.Modules;

/// <summary>
/// Provides a set of basic generator methods for creating random or deterministic data.
/// </summary>
public sealed class ForgeBasic(Forge forge)
{
	/// <summary>
	/// Creates a generator that always produces the specified literal value.
	/// </summary>
	/// <param name="literal">The literal value to always produce.</param>
	/// <returns>A generator that always produces the specified value.</returns>
	public Generator<T> Literal<T>(T literal)
		=> new LiteralGenerator<T>(literal, forge.Rng);
}
