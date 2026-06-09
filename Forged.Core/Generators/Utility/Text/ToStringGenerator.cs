namespace Forged.Core.Generators.Utility.Text;

public sealed class ToStringGenerator<T>(Generator<T> innerGenerator) : Generator<string?>
{
	public override string? Generate() 
		=> innerGenerator.Generate()?.ToString();
}