namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Truncates <see cref="DateTime"/> values to date precision (removes time component).
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class TruncateToDateGenerator(Generator<DateTime> innerGenerator, Forge forge) : Generator<DateTime>(forge)
{
	/// <summary>
	/// Generates a <see cref="DateTime"/> value with the time component truncated.
	/// </summary>
	/// <returns>A <see cref="DateTime"/> value with only the date component.</returns>
	public override DateTime Generate() => innerGenerator.Generate().Date;
}
