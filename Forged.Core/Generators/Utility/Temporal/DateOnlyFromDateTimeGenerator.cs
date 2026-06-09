namespace Forged.Core.Generators.Utility.Temporal;

public sealed class DateOnlyFromDateTimeGenerator(Generator<DateTime> innerGenerator) : Generator<DateOnly>
{
	public override DateOnly Generate()
		=> DateOnly.FromDateTime(innerGenerator.Generate());
}
