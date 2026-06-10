namespace Forged.Core.Generators.Utility.Temporal;

public sealed class ToUtcGenerator(Generator<DateTime> innerGenerator, System.Random rng) : Generator<DateTime>(rng)
{
	public override DateTime Generate()
		=> DateTime.SpecifyKind(innerGenerator.Generate(), DateTimeKind.Utc);
}
