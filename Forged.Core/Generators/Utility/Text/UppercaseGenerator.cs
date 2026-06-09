namespace Forged.Core.Generators.Utility.Text;

public sealed class UppercaseGenerator(Generator<string> innerGenerator) : Generator<string>
{
	public override string Generate() => innerGenerator.Generate().ToUpperInvariant();
}