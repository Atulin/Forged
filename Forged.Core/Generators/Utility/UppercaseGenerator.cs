namespace Forged.Core.Generators.Utility;

public sealed class UppercaseGenerator(Generator<string> innerGenerator) : Generator<string>
{
	public override string Generate() => innerGenerator.Generate().ToUpperInvariant();
}