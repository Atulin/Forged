namespace Forged.Core.Generators.Utility;

public sealed class LiteralGenerator<T>(T literal, System.Random rng) : Generator<T>(rng)
{
	public override T Generate() => literal;
}
