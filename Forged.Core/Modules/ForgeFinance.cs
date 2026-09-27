using Forged.Core.Generators;
using Forged.Core.Generators.Finance;
using CardOperatorFlag = Forged.Core.Generators.Finance.CardOperator;

namespace Forged.Core.Modules;

/// <summary>
/// Provides methods for generating financial and banking data.
/// </summary>
public sealed class ForgeFinance(Forge forge)
{
	/// <summary>
	/// Creates a generator that produces random currency names.
	/// </summary>
	/// <returns>A generator that produces currency names, such as "Euro" or "Japanese Yen".</returns>
	public Generator<string> Currency() => CurrencyName();

	/// <summary>
	/// Creates a generator that produces random ISO 4217 alphabetic currency codes.
	/// </summary>
	/// <returns>A generator that produces three-letter currency codes, such as "EUR" or "JPY".</returns>
	public Generator<string> CurrencyCode() => new CurrencyGenerator(CurrencyValue.Code, forge);

	/// <summary>
	/// Creates a generator that produces random currency names.
	/// </summary>
	/// <returns>A generator that produces currency names, such as "Euro" or "Japanese Yen".</returns>
	public Generator<string> CurrencyName() => new CurrencyGenerator(CurrencyValue.Name, forge);

	/// <summary>
	/// Creates a generator that produces random currency symbols.
	/// </summary>
	/// <returns>A generator that produces currency symbols, such as "€" or "¥".</returns>
	public Generator<string> CurrencySymbol() => new CurrencyGenerator(CurrencyValue.Symbol, forge);

	/// <summary>
	/// Creates a generator that produces random ISO 4217 numeric currency codes.
	/// </summary>
	/// <returns>A generator that produces three-digit numeric currency codes, such as "978" or "392".</returns>
	public Generator<string> CurrencyNumericCode() => new CurrencyGenerator(CurrencyValue.NumericCode, forge);

	/// <summary>
	/// Creates a generator that produces random IBANs for a randomly chosen supported country.
	/// </summary>
	/// <param name="valid">Whether to calculate a valid mod-97 check digit. Defaults to true. When false, the check digit is intentionally skewed to produce an unrecognizable IBAN.</param>
	/// <param name="test">Whether to return a known test IBAN instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces IBANs, including the country code and mod-97 check digits.</returns>
	public Generator<string> Iban(bool valid = true, bool test = false)
		=> new IbanGenerator(null, valid, test, forge);

	/// <summary>
	/// Creates a generator that produces random IBANs for the specified country.
	/// </summary>
	/// <param name="countryCode">The ISO 3166-1 alpha-2 country code to generate IBANs for. Matching is case-insensitive, so "gb" and "GB" are equivalent.</param>
	/// <param name="valid">Whether to calculate a valid mod-97 check digit. Defaults to true. When false, the check digit is intentionally skewed to produce an unrecognizable IBAN.</param>
	/// <param name="test">Whether to return a known test IBAN instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces IBANs for the requested country, including the country code and mod-97 check digits.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if the country code is not a supported IBAN country.</exception>
	public Generator<string> Iban(string countryCode, bool valid = true, bool test = false)
		=> new IbanGenerator(countryCode, valid, test, forge);

	/// <summary>
	/// Creates a generator that produces random Business Identifier Codes (BICs), also known as SWIFT codes.
	/// </summary>
	/// <param name="valid">Whether to use a real country code in the second position. Defaults to true. When false, the placeholder "ZZ" is used instead, producing a recognizable but invalid BIC.</param>
	/// <param name="test">Whether to return a known test BIC instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces eight- or eleven-character BICs, including the "XXX" primary branch suffix.</returns>
	public Generator<string> Bic(bool valid = true, bool test = false)
		=> new BicGenerator(valid, test, forge);

	/// <summary>
	/// Creates a generator that produces random SWIFT codes. This is an alias for <see cref="Bic"/>.
	/// </summary>
	/// <param name="valid">Whether to use a real country code in the second position. Defaults to true.</param>
	/// <param name="test">Whether to return a known test code instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces SWIFT codes.</returns>
	public Generator<string> Swift(bool valid = true, bool test = false)
		=> Bic(valid, test);

	/// <summary>
	/// Creates a generator that produces random nine-digit ABA bank routing numbers.
	/// </summary>
	/// <param name="valid">Whether to calculate a valid mod-10 checksum. Defaults to true. When false, the checksum digit is intentionally skewed to produce an unrecognizable routing number.</param>
	/// <param name="test">Whether to return a known test routing number instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces routing numbers, including the mod-10 checksum digit.</returns>
	public Generator<string> RoutingNumber(bool valid = true, bool test = false)
		=> new RoutingNumberGenerator(valid, test, forge);

	/// <summary>
	/// Creates a generator that produces account numbers of a fixed length.
	/// </summary>
	/// <param name="length">The exact number of digits to generate. Defaults to 8.</param>
	/// <returns>A generator that produces numeric account numbers of the requested length.</returns>
	public Generator<string> AccountNumber(int length = 8)
		=> new AccountNumberGenerator(length, length, forge);

	/// <summary>
	/// Creates a generator that produces account numbers of a variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of digits to generate (inclusive).</param>
	/// <param name="maxLength">The maximum number of digits to generate (inclusive).</param>
	/// <returns>A generator that produces numeric account numbers within the requested length range.</returns>
	public Generator<string> AccountNumber(int minLength, int maxLength)
		=> new AccountNumberGenerator(minLength, maxLength, forge);

	/// <summary>
	/// Creates a generator that produces a single randomly selected card operator.
	/// </summary>
	/// <returns>A generator that produces a <see cref="CardOperatorFlag"/> value.</returns>
	public Generator<CardOperatorFlag> CardOperator()
		=> new CardOperatorGenerator(CardOperatorFlag.All, forge);

	/// <summary>
	/// Creates a generator that produces random credit card numbers for the selected networks.
	/// </summary>
	/// <param name="cardOperator">The card networks to generate numbers for, combined as flags. Defaults to <see cref="CardOperatorFlag.All"/>.</param>
	/// <param name="valid">Whether to calculate a valid Luhn check digit. Defaults to true. When false, the check digit is intentionally skewed to produce an unrecognized card number.</param>
	/// <param name="test">Whether to return a known test card number instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces network-specific card numbers, including the Luhn check digit.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if no valid card operator is supplied.</exception>
	public Generator<string> CreditCardNumber(
		CardOperatorFlag cardOperator = CardOperatorFlag.All,
		bool valid = true,
		bool test = false)
		=> new CreditCardNumberGenerator(cardOperator, valid, test, forge);

	/// <summary>
	/// Creates a generator that produces random credit card numbers for the selected networks. This overload
	/// exists so that <c>valid</c> can be passed by name without also naming the card operator.
	/// </summary>
	/// <param name="valid">Whether to calculate a valid Luhn check digit. Defaults to true. When false, the check digit is intentionally skewed to produce an unrecognized card number.</param>
	/// <param name="cardOperator">The card networks to generate numbers for, combined as flags. Defaults to <see cref="CardOperatorFlag.All"/>.</param>
	/// <param name="test">Whether to return a known test card number instead of a generated one. Defaults to false.</param>
	/// <returns>A generator that produces network-specific card numbers, including the Luhn check digit.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if no valid card operator is supplied.</exception>
	public Generator<string> CreditCardNumber(
		bool valid,
		CardOperatorFlag cardOperator = CardOperatorFlag.All,
		bool test = false)
		=> new CreditCardNumberGenerator(cardOperator, valid, test, forge);

	/// <summary>
	/// Creates a generator that produces random card verification values (CVVs) of a network-appropriate length.
	/// </summary>
	/// <param name="cardOperator">The card networks to generate CVVs for, combined as flags. Defaults to <see cref="CardOperatorFlag.All"/>.</param>
	/// <returns>A generator that produces three- or four-digit CVVs, depending on the selected network.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if no valid card operator is supplied.</exception>
	public Generator<string> CreditCardCvv(CardOperatorFlag cardOperator = CardOperatorFlag.All)
		=> new CreditCardCvvGenerator(cardOperator, forge);

	/// <summary>
	/// Creates a generator that produces random card verification values. This is an alias for <see cref="CreditCardCvv"/>.
	/// </summary>
	/// <param name="cardOperator">The card networks to generate CVVs for, combined as flags. Defaults to <see cref="CardOperatorFlag.All"/>.</param>
	/// <returns>A generator that produces three- or four-digit CVVs.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if no valid card operator is supplied.</exception>
	public Generator<string> Cvv(CardOperatorFlag cardOperator = CardOperatorFlag.All)
		=> CreditCardCvv(cardOperator);

	/// <summary>
	/// Creates a generator that produces random card verification codes. This is an alias for <see cref="CreditCardCvv"/>.
	/// </summary>
	/// <param name="cardOperator">The card networks to generate codes for, combined as flags. Defaults to <see cref="CardOperatorFlag.All"/>.</param>
	/// <returns>A generator that produces three- or four-digit codes.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if no valid card operator is supplied.</exception>
	public Generator<string> Cvc(CardOperatorFlag cardOperator = CardOperatorFlag.All)
		=> CreditCardCvv(cardOperator);

	/// <summary>
	/// Creates a generator that produces random card expiration dates, normalized to the first day of the month.
	/// </summary>
	/// <param name="today">The reference date the expiration date is offset from. Defaults to null, which uses the current date and time.</param>
	/// <param name="expirationChance">The probability (0.0 to 1.0) that the generated date falls in the past, producing an already expired card. Defaults to 0.5. Dates otherwise fall on or after the reference date.</param>
	/// <returns>A generator that produces expiration dates up to roughly twenty years before or after the reference date.</returns>
	public Generator<DateTimeOffset> CreditCardExpirationDate(
		DateTimeOffset? today = null,
		float expirationChance = .5f)
		=> new CreditCardExpirationDateGenerator(today, expirationChance, forge);

	/// <summary>
	/// Creates a generator that produces random card expiration dates. This is an alias for <see cref="CreditCardExpirationDate"/>.
	/// </summary>
	/// <param name="today">The reference date the expiration date is offset from. Defaults to null, which uses the current date and time.</param>
	/// <param name="expirationChance">The probability (0.0 to 1.0) that the generated date falls in the past. Defaults to 0.5.</param>
	/// <returns>A generator that produces expiration dates up to roughly twenty years before or after the reference date.</returns>
	public Generator<DateTimeOffset> ExpirationDate(
		DateTimeOffset? today = null,
		float expirationChance = .5f)
		=> CreditCardExpirationDate(today, expirationChance);
}
