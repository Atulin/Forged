namespace Forged.Core.Generators.Utility.Temporal;

public sealed class TruncateToDateGenerator(Generator<DateTime> innerGenerator) : Generator<DateTime>
{
	public override DateTime Generate()
		=> innerGenerator.Generate().Date;
}
