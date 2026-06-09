namespace Forged.Core.Generators.Utility.Text;

public sealed class LowercaseGenerator(Generator<string> innerGenerator) : Generator<string>
{
	public override string Generate() => innerGenerator.Generate().ToLowerInvariant();
}