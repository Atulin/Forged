using System.Globalization;
using System.Text.RegularExpressions;
using Forged.Core;
using Forged.Core.Generators;
using Forged.Core.Generators.Finance;

namespace Forged.Tests;

public class ForgeFinanceTests
{
	private static readonly string[] TestIbans =
	[
		"GB82WEST12345698765432",
		"DE89370400440532013000",
		"FR1420041010050500013M02606",
		"NL91ABNA0417164300",
		"CH9300762011623852957",
		"ES9121000418450200051332",
		"IT60X0542811101000000123456",
		"BE68539007547034",
		"PT50000201231234567890154",
	];

	private static readonly string[] TestBics =
	[
		"DEUTDEFF",
		"COBADEFFXXX",
		"NEDSZAJJXXX",
		"CHASUS33",
		"BOFAUS3N",
		"CITIUS33",
		"HSBCSG2",
	];

	private static readonly string[] TestRoutingNumbers =
	[
		"021000021",
		"011401533",
		"121000248",
		"026009593",
		"071000013",
		"111000025",
		"061000104",
		"322271627",
	];

	private static readonly string[] TestCardNumbers =
	[
		"4111111111111111",
		"5555555555554444",
		"378282246310005",
		"6011111111111117",
		"30569309025904",
		"3530111333300000",
		"6212345678901232",
	];

	private static Forge NewForge(int seed)
		=> new(new Random(seed), CultureInfo.InvariantCulture);

	private static List<T> Draw<T>(Generator<T> generator, int count)
	{
		var values = new List<T>(count);
		for (var i = 0; i < count; i++)
		{
			values.Add(generator.Generate());
		}
		return values;
	}

	private static bool IsValidIban(string value)
	{
		var rearranged = value[4..] + value[..4];
		var remainder = 0;
		foreach (var character in rearranged)
		{
			var digits = char.IsAsciiLetter(character)
				? (character - 'A' + 10).ToString(CultureInfo.InvariantCulture)
				: character.ToString(CultureInfo.InvariantCulture);
			foreach (var digit in digits)
			{
				remainder = (remainder * 10 + digit - '0') % 97;
			}
		}
		return remainder == 1;
	}

	private static bool IsValidLuhn(string value)
	{
		var sum = 0;
		for (var i = value.Length - 1; i >= 0; i--)
		{
			var digit = value[i] - '0';
			if ((value.Length - 1 - i) % 2 == 1)
			{
				digit *= 2;
				if (digit > 9)
				{
					digit -= 9;
				}
			}
			sum += digit;
		}
		return sum % 10 == 0;
	}

	private static bool IsValidRoutingNumber(string value)
	{
		var digits = value.Select(character => character - '0').ToArray();
		var sum = 3 * (digits[0] + digits[3] + digits[6])
		          + 7 * (digits[1] + digits[4] + digits[7])
		          + digits[2] + digits[5] + digits[8];
		return sum % 10 == 0;
	}

	[Test]
	public async Task Currency_ProducesRecognizedValues()
	{
		var forge = NewForge(3001);
		var codes = Draw(forge.Finance.CurrencyCode(), 100);
		var names = Draw(forge.Finance.CurrencyName(), 100);
		var symbols = Draw(forge.Finance.CurrencySymbol(), 100);
		var numericCodes = Draw(forge.Finance.CurrencyNumericCode(), 100);
		var aliases = Draw(forge.Finance.Currency(), 100);

		foreach (var value in codes)
		{
			await Assert.That(Regex.IsMatch(value, "^[A-Z]{3}$")).IsTrue();
		}
		foreach (var value in numericCodes)
		{
			await Assert.That(Regex.IsMatch(value, "^[0-9]{3}$")).IsTrue();
		}
		foreach (var value in names.Concat(symbols).Concat(aliases))
		{
			await Assert.That(value).IsNotEmpty();
		}
	}

	[Test]
	public async Task Currency_IsDeterministic_ForTheSameSeed()
	{
		var expected = NewForge(3002).Finance.CurrencyCode().Generate();
		var actual = NewForge(3002).Finance.CurrencyCode().Generate();

		await Assert.That(actual).IsEqualTo(expected);
	}

	[Test]
	public async Task Iban_Default_ProducesValidIbans()
	{
		var forge = NewForge(3010);
		var values = Draw(forge.Finance.Iban(), 200);

		foreach (var value in values)
		{
			await Assert.That(Regex.IsMatch(value, "^[A-Z]{2}[0-9]{2}[A-Z0-9]+$")).IsTrue();
			await Assert.That(IsValidIban(value)).IsTrue();
		}
	}

	[Test]
	public async Task Iban_WithCountry_ProducesCountrySpecificIbans()
	{
		var forge = NewForge(3011);
		var values = Draw(forge.Finance.Iban("GB"), 100);

		foreach (var value in values)
		{
			await Assert.That(value.StartsWith("GB", StringComparison.Ordinal)).IsTrue();
			await Assert.That(value.Length).IsEqualTo(22);
			await Assert.That(IsValidIban(value)).IsTrue();
		}
	}

	[Test]
	public async Task Iban_Invalid_ProducesChecksumInvalidIbans()
	{
		var forge = NewForge(3012);
		var values = Draw(forge.Finance.Iban("DE", valid: false), 100);

		foreach (var value in values)
		{
			await Assert.That(IsValidIban(value)).IsFalse();
		}
	}

	[Test]
	public async Task Iban_Test_ProducesKnownIbans()
	{
		var forge = NewForge(3013);
		var values = Draw(forge.Finance.Iban(test: true), 100);

		foreach (var value in values)
		{
			await Assert.That(TestIbans.Contains(value)).IsTrue();
		}
	}

	[Test]
	public async Task Iban_UnsupportedCountry_Throws()
	{
		var forge = NewForge(3014);

		await Assert.That(() => forge.Finance.Iban("ZZ")).Throws<ArgumentOutOfRangeException>();
	}

	[Test]
	public async Task Bic_Default_ProducesValidBics()
	{
		var forge = NewForge(3020);
		var values = Draw(forge.Finance.Bic(), 100);

		foreach (var value in values)
		{
			await Assert.That(Regex.IsMatch(value, "^[A-Z]{4}[A-Z]{2}[A-Z]{5}$")).IsTrue();
		}
	}

	[Test]
	public async Task Bic_Invalid_ProducesRecognizableInvalidBics()
	{
		var forge = NewForge(3021);
		var values = Draw(forge.Finance.Bic(valid: false), 100);

		foreach (var value in values)
		{
			await Assert.That(Regex.IsMatch(value, "^[A-Z]{4}ZZ[A-Z]{5}$")).IsTrue();
		}
	}

	[Test]
	public async Task Bic_TestAndSwiftAlias_ProduceKnownValuesAndSharedOutput()
	{
		var forge = NewForge(3022);
		var values = Draw(forge.Finance.Bic(test: true), 100);

		foreach (var value in values)
		{
			await Assert.That(TestBics.Contains(value)).IsTrue();
		}
		await Assert.That(NewForge(3023).Finance.Swift().Generate())
			.IsEqualTo(NewForge(3023).Finance.Swift().Generate());
	}

	[Test]
	public async Task RoutingNumber_Default_ProducesValidAbaNumbers()
	{
		var forge = NewForge(3030);
		var values = Draw(forge.Finance.RoutingNumber(), 200);

		foreach (var value in values)
		{
			await Assert.That(value.Length).IsEqualTo(9);
			await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
			await Assert.That(IsValidRoutingNumber(value)).IsTrue();
		}
	}

	[Test]
	public async Task RoutingNumber_Invalid_ProducesChecksumInvalidNumbers()
	{
		var forge = NewForge(3031);
		var values = Draw(forge.Finance.RoutingNumber(valid: false), 100);

		foreach (var value in values)
		{
			await Assert.That(IsValidRoutingNumber(value)).IsFalse();
		}
	}

	[Test]
	public async Task RoutingNumber_Test_ProducesKnownNumbers()
	{
		var forge = NewForge(3032);
		var values = Draw(forge.Finance.RoutingNumber(test: true), 100);

		foreach (var value in values)
		{
			await Assert.That(TestRoutingNumbers.Contains(value)).IsTrue();
		}
	}

	[Test]
	public async Task AccountNumber_ProducesDigitsInTheRequestedRange()
	{
		var forge = NewForge(3040);
		var fixedLength = Draw(forge.Finance.AccountNumber(12), 100);
		var variableLength = Draw(forge.Finance.AccountNumber(6, 10), 200);

		foreach (var value in fixedLength)
		{
			await Assert.That(value.Length).IsEqualTo(12);
			await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
		}
		foreach (var value in variableLength)
		{
			await Assert.That(value.Length).IsInRange(6, 10);
			await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
		}
	}

	[Test]
	public async Task CardOperator_ProducesSupportedOperators()
	{
		var forge = NewForge(3050);
		var values = Draw(forge.Finance.CardOperator(), 200);

		foreach (var value in values)
		{
			await Assert.That(value != CardOperator.None).IsTrue();
			await Assert.That((value & ~CardOperator.All) == 0).IsTrue();
		}
		await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
	}

	[Test]
	public async Task CreditCardNumber_ProducesNetworkSpecificLuhnNumbers()
	{
		var forge = NewForge(3060);
		var operators = new[]
		{
			CardOperator.Visa,
			CardOperator.Mastercard,
			CardOperator.AmericanExpress,
			CardOperator.Discover,
			CardOperator.DinersClub,
			CardOperator.JCB,
			CardOperator.UnionPay,
		};

		foreach (var cardOperator in operators)
		{
			var values = Draw(forge.Finance.CreditCardNumber(cardOperator), 50);
			foreach (var value in values)
			{
				await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
				await Assert.That(IsValidLuhn(value)).IsTrue();
				var hasExpectedPrefix = cardOperator switch
				{
					CardOperator.Visa => value.StartsWith('4'),
					CardOperator.Mastercard => value.StartsWith("51", StringComparison.Ordinal)
					                           || value.StartsWith("52", StringComparison.Ordinal)
					                           || value.StartsWith("53", StringComparison.Ordinal)
					                           || value.StartsWith("54", StringComparison.Ordinal)
					                           || value.StartsWith("55", StringComparison.Ordinal),
					CardOperator.AmericanExpress => value.StartsWith("34", StringComparison.Ordinal)
					                                || value.StartsWith("37", StringComparison.Ordinal),
					CardOperator.Discover => value.StartsWith("6011", StringComparison.Ordinal)
					                         || value.StartsWith("65", StringComparison.Ordinal),
					CardOperator.DinersClub => value.StartsWith("30", StringComparison.Ordinal)
					                           || value.StartsWith("36", StringComparison.Ordinal)
					                           || value.StartsWith("38", StringComparison.Ordinal),
					CardOperator.JCB => value.StartsWith("35", StringComparison.Ordinal),
					CardOperator.UnionPay => value.StartsWith("62", StringComparison.Ordinal),
					_ => false,
				};
				await Assert.That(hasExpectedPrefix).IsTrue();
			}
		}
	}

	[Test]
	public async Task CreditCardNumber_Invalid_ProducesLuhnInvalidNumbers()
	{
		var forge = NewForge(3061);
		var values = Draw(forge.Finance.CreditCardNumber(CardOperator.Visa, valid: false), 100);

		foreach (var value in values)
		{
			await Assert.That(IsValidLuhn(value)).IsFalse();
		}
	}

	[Test]
	public async Task CreditCardNumber_Test_ProducesKnownNumbers()
	{
		var forge = NewForge(3062);
		var values = Draw(forge.Finance.CreditCardNumber(test: true), 100);

		foreach (var value in values)
		{
			await Assert.That(TestCardNumbers.Contains(value)).IsTrue();
		}
	}

	[Test]
	public async Task CreditCardCvv_ProducesNetworkAppropriateLengths()
	{
		var forge = NewForge(3070);
		var threeDigit = Draw(forge.Finance.Cvv(CardOperator.Visa), 100);
		var fourDigit = Draw(forge.Finance.Cvc(CardOperator.AmericanExpress), 100);

		foreach (var value in threeDigit)
		{
			await Assert.That(value.Length).IsEqualTo(3);
			await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
		}
		foreach (var value in fourDigit)
		{
			await Assert.That(value.Length).IsEqualTo(4);
			await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
		}
	}

	[Test]
	public async Task CreditCardExpirationDate_ChanceZero_ProducesCurrentOrFutureMonths()
	{
		var forge = NewForge(3080);
		var today = new DateTimeOffset(2030, 6, 15, 12, 30, 0, TimeSpan.FromHours(2));
		var values = Draw(forge.Finance.ExpirationDate(today, 0f), 100);
		var currentMonth = new DateTimeOffset(2030, 6, 1, 0, 0, 0, today.Offset);

		foreach (var value in values)
		{
			await Assert.That(value).IsGreaterThanOrEqualTo(currentMonth);
			await Assert.That(value.Day).IsEqualTo(1);
			await Assert.That(value.Offset).IsEqualTo(today.Offset);
		}
	}

	[Test]
	public async Task CreditCardExpirationDate_ChanceOne_ProducesPastMonths()
	{
		var forge = NewForge(3081);
		var today = new DateTimeOffset(2030, 6, 15, 12, 30, 0, TimeSpan.Zero);
		var values = Draw(forge.Finance.CreditCardExpirationDate(today, 1f), 100);
		var currentMonth = new DateTimeOffset(2030, 6, 1, 0, 0, 0, today.Offset);

		foreach (var value in values)
		{
			await Assert.That(value).IsLessThan(currentMonth);
		}
	}

	[Test]
	public async Task FinanceGenerators_AreDeterministic_ForTheSameSeed()
	{
		var forge = NewForge(3090);
		var otherForge = NewForge(3090);

		await Assert.That(forge.Finance.Iban().Generate()).IsEqualTo(otherForge.Finance.Iban().Generate());
		await Assert.That(forge.Finance.Bic().Generate()).IsEqualTo(otherForge.Finance.Bic().Generate());
		await Assert.That(forge.Finance.RoutingNumber().Generate()).IsEqualTo(otherForge.Finance.RoutingNumber().Generate());
		await Assert.That(forge.Finance.AccountNumber(14).Generate()).IsEqualTo(otherForge.Finance.AccountNumber(14).Generate());
		await Assert.That(forge.Finance.CreditCardNumber().Generate()).IsEqualTo(otherForge.Finance.CreditCardNumber().Generate());
		await Assert.That(forge.Finance.CreditCardCvv().Generate()).IsEqualTo(otherForge.Finance.CreditCardCvv().Generate());
	}

	[Test]
	public async Task CreditCardNumber_ValidFirstOverload_ProducesLuhnValidNumbers()
	{
		var forge = NewForge(3100);
		var values = Draw(forge.Finance.CreditCardNumber(valid: true, CardOperator.Visa), 50);

		foreach (var value in values)
		{
			await Assert.That(value.All(char.IsAsciiDigit)).IsTrue();
			await Assert.That(IsValidLuhn(value)).IsTrue();
			await Assert.That(value.StartsWith('4')).IsTrue();
		}
	}

	[Test]
	public async Task CreditCardNumber_ValidFalseFirstOverload_ProducesLuhnInvalidNumbers()
	{
		var forge = NewForge(3101);
		var values = Draw(forge.Finance.CreditCardNumber(valid: false, CardOperator.Mastercard), 50);

		foreach (var value in values)
		{
			await Assert.That(IsValidLuhn(value)).IsFalse();
		}
	}

	[Test]
	public async Task CardOperator_None_Throws()
	{
		var forge = NewForge(3110);

		await Assert.That(() => forge.Finance.CreditCardNumber(CardOperator.None)).Throws<ArgumentOutOfRangeException>();
		await Assert.That(() => forge.Finance.CreditCardCvv(CardOperator.None)).Throws<ArgumentOutOfRangeException>();
		await Assert.That(() => new CardOperatorGenerator(CardOperator.None, forge)).Throws<ArgumentOutOfRangeException>();
	}

	[Test]
	public async Task CreditCardExpirationDate_WithNullToday_UsesCurrentTime()
	{
		var before = DateTimeOffset.Now;
		var forge = NewForge(3120);
		var values = Draw(forge.Finance.ExpirationDate(null, 0f), 20);
		var after = DateTimeOffset.Now;

		foreach (var value in values)
		{
			var currentMonthStart = new DateTimeOffset(before.Year, before.Month, 1, 0, 0, 0, value.Offset);
			await Assert.That(value).IsGreaterThanOrEqualTo(currentMonthStart);
		}
		await Assert.That(values).IsNotEmpty();
		_ = before;
		_ = after;
	}
}
