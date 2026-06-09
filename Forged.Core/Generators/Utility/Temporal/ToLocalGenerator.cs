namespace Forged.Core.Generators.Utility.Temporal;

public sealed class ToLocalGenerator(Generator<DateTime> innerGenerator) : Generator<DateTime>
{
	public override DateTime Generate()
		=> DateTime.SpecifyKind(innerGenerator.Generate(), DateTimeKind.Local);
}
