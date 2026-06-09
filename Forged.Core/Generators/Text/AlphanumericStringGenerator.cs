namespace Forged.Core.Generators.Text;

public sealed class AlphanumericStringGenerator(int length) : Generator<string>
{
	public override string Generate() => Rng.GetString(Constants.AlphanumericString, length);
}