namespace Forged.Core.Generators.Utility.Text;

public sealed class UppercaseGenerator(Generator<string> innerGenerator, System.Random rng) : Generator<string>(rng)
{
	public override string Generate() => innerGenerator.Generate().ToUpperInvariant();
}
