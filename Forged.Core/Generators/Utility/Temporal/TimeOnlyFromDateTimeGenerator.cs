namespace Forged.Core.Generators.Utility.Temporal;

public sealed class TimeOnlyFromDateTimeGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<TimeOnly>(rng)
{
	public override TimeOnly Generate() => TimeOnly.FromDateTime(innerGenerator.Generate());
}
