namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Transforms <see cref="DateTime"/> values to <see cref="TimeOnly"/> values.
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class TimeOnlyFromDateTimeGenerator(Generator<DateTime> innerGenerator, Forge forge) : Generator<TimeOnly>(forge)
{
	/// <summary>
	/// Generates a <see cref="TimeOnly"/> value from a <see cref="DateTime"/> value.
	/// </summary>
	/// <returns>A <see cref="TimeOnly"/> value extracted from the inner generator's output.</returns>
	public override TimeOnly Generate() => TimeOnly.FromDateTime(innerGenerator.Generate());
}
