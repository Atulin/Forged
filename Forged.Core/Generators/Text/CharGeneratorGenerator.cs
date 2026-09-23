using Forged.Core.Core;

namespace Forged.Core.Generators.Text;

/// <summary>
/// A text generator that produces random characters based on the specified character type.
/// </summary>
/// <remarks>
/// The <see cref="CharGenerator"/> class is used to generate single random characters based on a pool defined by
/// the provided <see cref="CharKind"/>. The output can vary from alphanumeric characters to printable symbols
/// depending on the character type used.
/// </remarks>
/// <param name="kind">The kind of character pool to be used for generation, such as ASCII, Printable, Alphanumeric, etc.</param>
/// <param name="forge">The instance of the <see cref="Forge"/> class providing shared random number generation and locale services.</param>
public sealed class CharGenerator(CharKind kind, Forge forge) : Generator<char>(forge)
{
	public override char Generate()
	{
		Span<char> pool = stackalloc char[CharPool.MaxLength];
		var count = CharPool.FillCharPool(pool, kind);
		return pool[Rng.Next(count)];
	}
}