using Forged.Core.Generators.Random.DiceGeneratorHelpers;

namespace Forged.Core.Generators.Random;

/// <summary>
/// Represents a generator for simulating dice rolls based on a provided expression.
/// Parses and evaluates mathematical expressions, generates random values, and applies a rounding mode
/// to shape the result.
/// </summary>
/// <remarks>
/// This generator is designed to handle dice notation (e.g. "2d6+3") and mathematical expressions.
/// It relies on the provided random number generator and rounding mode to produce results.
/// </remarks>
/// <param name="expression">The dice expression or mathematical formula to evaluate, such as "2d6+1".</param>
/// <param name="mode">Specifies the rounding behavior to apply to calculated results (e.g. Floor, Round, Ceiling).</param>
/// <param name="forge">The Forge instance that provides dependencies like random number generators and configurations.</param>
/// <example>
/// The DiceGenerator can be used to simulate randomized dice rolls in games or statistical simulations.
/// </example>
public sealed class DiceGenerator(string expression, RoundingMode mode, Forge forge) : Generator<int>(forge)
{
	public override int Generate()
	{
		var result = DiceParser.Evaluate(expression, Rng);
		var rounded = mode switch
		{

			RoundingMode.Floor => Math.Floor(result),
			RoundingMode.Round => Math.Round(result),
			RoundingMode.Ceiling => Math.Ceiling(result),
			_ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
		};
		return (int)rounded;
	}
}

/// <summary>
/// Specifies the rounding behavior to apply when processing numerical results.
/// </summary>
/// <remarks>
/// This enumeration defines rounding modes such as Floor, Round, and Ceiling,
/// which determine how fractional values are adjusted when converted to integers.
/// </remarks>
public enum RoundingMode
{
	Floor,
	Round,
	Ceiling
}