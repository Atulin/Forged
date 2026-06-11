namespace Forged.Core.Generators.Text;

public sealed class AlphaStringGenerator(int minLength, int maxLength, System.Random rng) : Generator<string>(rng)
{
	public override string Generate()
	{
		var length = minLength == maxLength 
			? minLength 
			: Rng.Next(minLength, maxLength + 1);
		
		return Rng.GetString(Constants.AlphanumericString.AsSpan()[..^10], length);
	}
}
