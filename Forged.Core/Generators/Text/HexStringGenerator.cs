namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates random hexadecimal strings.
/// </summary>
/// <param name="minLength">The minimum length of the string to generate.</param>
/// <param name="maxLength">The maximum length of the string to generate.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class HexStringGenerator(int minLength, int maxLength, Forge forge) : Generator<string>(forge)
{
	/// <summary>
	/// Generates a random hexadecimal string.
	/// </summary>
	/// <returns>A random hexadecimal string.</returns>
	public override string Generate()
	{
		var length = minLength == maxLength 
			? minLength 
			: Rng.Next(minLength, maxLength + 1);
		
		return Rng.GetHexString(length);
	}
}
