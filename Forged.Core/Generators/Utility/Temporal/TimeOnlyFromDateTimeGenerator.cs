namespace Forged.Core.Generators.Utility.Temporal;

public sealed class TimeOnlyFromDateTimeGenerator(Generator<DateTime> innerGenerator) : Generator<TimeOnly>
{
	public override TimeOnly Generate()
		=> TimeOnly.FromDateTime(innerGenerator.Generate());
}
