namespace Forged.Core.Generators.Text;

public sealed class HexStringGenerator(int length, System.Random rng) : Generator<string>(rng)
{
	public override string Generate() => Rng.GetHexString(length);
}
