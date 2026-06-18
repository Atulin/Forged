using System.Globalization;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators.Extensions;

/// <summary>
/// Provides general extension methods for the <see cref="Generator{T}"/> class.
/// </summary>
public static class GeneratorExtensions
{
	/// <summary>
	/// Converts values from a generator to their string representation.
	/// </summary>
	/// <typeparam name="T">The type to convert to string.</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <returns>A generator that produces string representations.</returns>
	public static Generator<string?> ToString<T>(this Generator<T> generator)
		=> new ToStringGenerator<T>(generator, generator.Forge);

	/// <summary>
	/// Converts values from a generator to formatted strings.
	/// </summary>
	/// <typeparam name="T">The type to format (must implement ISpanFormattable).</typeparam>
	/// <param name="generator">The generator to convert.</param>
	/// <param name="format">The format string to use.</param>
	/// <param name="cultureInfo">The culture to use for formatting. If null, uses invariant culture.</param>
	/// <returns>A generator that produces formatted strings.</returns>
	public static Generator<string> ToString<T>(this Generator<T> generator, string format, CultureInfo? cultureInfo = null) where T : ISpanFormattable
		=> new ToFormattedStringGenerator<T>(generator, format, generator.Forge, cultureInfo);
}
