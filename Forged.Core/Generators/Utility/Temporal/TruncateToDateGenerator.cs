namespace Forged.Core.Generators.Utility.Temporal;

public sealed class TruncateToDateGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<DateTime>(rng)
{
	public override DateTime Generate() => innerGenerator.Generate().Date;
}
