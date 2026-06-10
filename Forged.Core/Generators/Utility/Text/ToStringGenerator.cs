namespace Forged.Core.Generators.Utility.Text;

public sealed class ToStringGenerator<T>(Generator<T> innerGenerator, System.Random rng) : Generator<string?>(rng)
{
	public override string? Generate() => innerGenerator.Generate()?.ToString();
}
