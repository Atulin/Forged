namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates random alphabetic (letters only) strings.
/// </summary>
/// <param name="minLength">The minimum length of the string to generate.</param>
/// <param name="maxLength">The maximum length of the string to generate.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class AlphaStringGenerator(int minLength, int maxLength, Forge forge) : Generator<string>(forge)
{
	/// <summary>
	/// Generates a random alphabetic string.
	/// </summary>
	/// <returns>A random alphabetic string.</returns>
	public override string Generate()
	{
		var length = minLength == maxLength 
			? minLength 
			: Rng.Next(minLength, maxLength + 1);
		
		return Rng.GetString(Constants.AlphanumericString.AsSpan()[..^10], length);
	}
}
