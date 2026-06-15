namespace Forged.Core.Generators.Utility.Temporal;

/// <summary>
/// Converts <see cref="DateTime"/> values to local time.
/// </summary>
/// <param name="innerGenerator">The inner generator that produces <see cref="DateTime"/> values.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class ToLocalGenerator(Generator<DateTime> innerGenerator, Forge forge) : Generator<DateTime>(forge)
{
	/// <summary>
	/// Generates a <see cref="DateTime"/> value with local kind.
	/// </summary>
	/// <returns>A <see cref="DateTime"/> value with <see cref="DateTimeKind.Local"/>.</returns>
	public override DateTime Generate()
		=> DateTime.SpecifyKind(innerGenerator.Generate(), DateTimeKind.Local);
}
