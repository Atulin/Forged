namespace Forged.Core.Generators.Utility.Temporal;

public sealed class ToUtcGenerator(Generator<DateTime> innerGenerator) : Generator<DateTime>
{
	public override DateTime Generate()
		=> DateTime.SpecifyKind(innerGenerator.Generate(), DateTimeKind.Utc);
}
