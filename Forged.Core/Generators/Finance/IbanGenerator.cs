using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// Provides functionality to generate International Bank Account Numbers (IBAN) as strings.
/// </summary>
/// <remarks>
/// This class generates IBANs for supported countries based on provided parameters. It can create valid or
/// invalid IBANs depending on the use case and can also return predefined test IBANs.
/// </remarks>
public sealed class IbanGenerator : Generator<string>
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

	private static readonly char[] AlphanumericPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

	private readonly bool Valid;
	private readonly bool Test;
	private readonly Dictionary<string, int> Countries;
	private readonly string[] CountryCodes;
	private readonly string? Code;
	private readonly string[] FilteredTestIbans;

	/// <summary>
	/// Initializes a new instance of the <see cref="IbanGenerator"/> class.
	/// </summary>
	/// <param name="countryCode">The ISO 3166-1 alpha-2 country code to generate the IBAN for. Defaults to null, which picks a supported country at random.</param>
	/// <param name="valid">Whether to generate a valid mod-97 check digit. When false, the check digit is intentionally skewed to produce an invalid IBAN.</param>
	/// <param name="test">Whether to return known test IBANs instead of generated ones.</param>
	/// <param name="forge">The Forge instance to use.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if the country code is not a supported IBAN country.</exception>
	public IbanGenerator(string? countryCode, bool valid, bool test, Forge forge) : base(forge)
	{
		Valid = valid;
		Test = test;
		Countries = FileLoader.LoadData(Locale, "finance/iban_countries", CommonContext.Default.DictionaryStringInt32);
		CountryCodes = [.. Countries.Keys];
		Code = Validate(countryCode);
		FilteredTestIbans = Code is null
			? TestIbans
			: [.. TestIbans.Where(iban => iban.StartsWith(Code, StringComparison.Ordinal))];
	}

	private string? Validate(string? countryCode)
	{
		if (countryCode is null)
		{
			return null;
		}

		var code = countryCode.ToUpperInvariant();
		return Countries.ContainsKey(code)
			? code
			: throw new ArgumentOutOfRangeException(nameof(countryCode), countryCode, "Unsupported IBAN country code.");
	}

	public override string Generate()
	{
		if (Test && FilteredTestIbans.Length > 0)
		{
			return Rng.GetItem(FilteredTestIbans);
		}

		var code = Code ?? Rng.GetItem(CountryCodes);
		var ibanLength = Countries[code];

		Span<char> body = stackalloc char[ibanLength];
		body[0] = code[0];
		body[1] = code[1];
		body[2] = '0';
		body[3] = '0';

		for (var i = 4; i < ibanLength; i++)
		{
			body[i] = Rng.GetItem(AlphanumericPool);
		}

		var rearranged = string.Concat(body[4..], body[..4]);
		var checkDigits = 98 - Mod97(rearranged);
		if (!Valid)
		{
			checkDigits = checkDigits == 0 ? 1 : checkDigits - 1;
		}

		body[2] = (char)('0' + checkDigits / 10);
		body[3] = (char)('0' + checkDigits % 10);

		return body.ToString();
	}

	private static int Mod97(ReadOnlySpan<char> value)
	{
		var remainder = 0;
		foreach (var character in value)
		{
			var digits = char.IsAsciiLetter(character)
				? (character - 'A' + 10).ToString()
				: character.ToString();

			foreach (var digit in digits)
			{
				remainder = (remainder * 10 + digit - '0') % 97;
			}
		}
		return remainder;
	}
}
