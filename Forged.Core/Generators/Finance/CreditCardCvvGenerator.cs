using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// Provides functionality to generate random CVV (Card Verification Value) codes for credit cards
/// based on the specified card operator and configuration.
/// </summary>
/// <remarks>
/// This class utilizes random number generation and predefined specifications for
/// supported card operators to generate valid CVV codes applicable to various credit card types.
/// </remarks>
/// <param name="operators">
/// Specifies the card operators (e.g., Visa, Mastercard, etc.) for which the CVV is being generated.
/// Can accept multiple operators as a bitwise combination.
/// </param>
/// <param name="forge">
/// An instance of the main Forge class, providing dependencies for random number generation and locale support.
/// </param>
public sealed class CreditCardCvvGenerator(CardOperator operators, Forge forge) : Generator<string>(forge)
{
	private readonly CardOperatorSpec[] Specs = [.. CardOperatorCatalog.ResolveSpec(operators)];

	public override string Generate()
	{
		var spec = Rng.GetItem(Specs);
		return Rng.GetString(CharPool.Get(CharKind.Numeric), spec.CvvLength);
	}
}
