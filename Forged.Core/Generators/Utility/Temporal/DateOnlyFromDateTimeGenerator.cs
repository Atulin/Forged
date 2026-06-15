namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Transforms <see cref="DateTime"/> values to <see cref="DateOnly"/> values.
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class DateOnlyFromDateTimeGenerator(Generator<DateTime> innerGenerator, Forge forge) : Generator<DateOnly>(forge)
{
	/// <summary>
	/// Generates a <see cref="DateOnly"/> value from a <see cref="DateTime"/> value.
	/// </summary>
	/// <returns>A <see cref="DateOnly"/> value extracted from the inner generator's output.</returns>
	public override DateOnly Generate() => DateOnly.FromDateTime(innerGenerator.Generate());
}
