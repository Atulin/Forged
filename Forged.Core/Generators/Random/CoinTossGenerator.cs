namespace Forged.Core.Generators.Random;

/// <summary>
/// Represents a generator for simulating coin tosses, producing random boolean values.
/// </summary>
/// <typeparam name="T">The type of value generated, constrained to boolean.</typeparam>
public sealed class CoinTossGenerator<T>(System.Random rng) : Generator<bool>(rng)
{
	/// <summary>
	/// Generates a random boolean value representing the outcome of a coin toss.
	/// </summary>
	/// <returns>A randomly generated boolean value, where true and false represent the two possible outcomes of a coin toss.</returns>
	public override bool Generate() => Rng.Next(2) == 1;
}
