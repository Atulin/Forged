using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// Generates random credit card numbers based on the specified card operator, validity, and testing criteria.
/// </summary>
/// 
/// <remarks>
/// This class is utilized to produce credit card numbers that adhere to various specifications, such as
/// the card operator type, inclusion of valid or invalid check digits, and whether the result is a
/// pre-defined test number or randomly generated. The implementation uses the Luhn algorithm to ensure
/// validity of the generated numbers.
/// </remarks>
/// 
/// <param name="operators">
/// Specifies the card operators (e.g., Visa, Mastercard, etc.) for which the credit card number will be generated.
/// Accepts a combination of flags from the CardOperator enumeration.
/// </param>
/// 
/// <param name="valid">
/// Defines whether the generated credit card number should be valid according to the Luhn check digit algorithm.
/// Setting this to 'false' results in an intentionally invalid number.
/// </param>
/// 
/// <param name="test">
/// A boolean that determines whether to generate a pre-defined test credit card number or a random one.
/// When set to 'true', the output is a test number; otherwise, a randomly generated number is returned.
/// </param>
/// 
/// <param name="forge">
/// A reference to the Forge instance, which provides the necessary randomization utilities and configurations.
/// </param>
/// 
/// <example>
/// This class does not provide direct examples. For usage, explore methods that instantiate and utilize this generator.
/// </example>
/// 
/// <seealso cref="CardOperator" />
/// <seealso cref="Forge" />
/// <seealso cref="Generator{T}" />
public sealed class CreditCardNumberGenerator(
	CardOperator operators,
	bool valid,
	bool test,
	Forge forge) : Generator<string>(forge)
{
	private readonly CardOperatorSpec[] _specs = [.. CardOperatorCatalog.ResolveSpec(operators)];

	public override string Generate()
	{
		var spec = Rng.GetItem(_specs);
		if (test)
		{
			return spec.TestNumber;
		}

		var prefix = Rng.GetItem(spec.Prefixes);
		var body = prefix + Rng.Digits(spec.Length - prefix.Length - 1);
		var checkDigit = LuhnCheckDigit(body);
		if (!valid)
		{
			checkDigit = (checkDigit + 1) % 10;
		}
		return $"{body}{checkDigit}";
	}

	private static int LuhnCheckDigit(string payload)
	{
		var sum = 0;
		var doubleDigit = true;
		for (var i = payload.Length - 1; i >= 0; i--)
		{
			var value = payload[i] - '0';
			if (doubleDigit)
			{
				value *= 2;
				if (value > 9)
				{
					value -= 9;
				}
			}
			sum += value;
			doubleDigit = !doubleDigit;
		}
		return (10 - sum % 10) % 10;
	}
}
