namespace Forged.Core.Generators.Text;

public sealed class AlphanumericStringGenerator(int length, System.Random rng) : Generator<string>(rng)
{
	public override string Generate() => Rng.GetString(Constants.AlphanumericString, length);
}
