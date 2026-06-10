namespace Forged.Core.Generators.Utility.Temporal;

public sealed class DateOnlyFromDateTimeGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<DateOnly>(rng)
{
	public override DateOnly Generate() => DateOnly.FromDateTime(innerGenerator.Generate());
}
