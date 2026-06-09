namespace Forged.Core.Generators.Text;

public sealed class HexStringGenerator(int length) : Generator<string>
{
	public override string Generate() => Rng.GetHexString(length);
}