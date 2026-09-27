using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

public enum CurrencyValue
{
	Code,
	Name,
	Symbol,
	NumericCode,
}

internal sealed record CurrencyDefinition(string Code, string Name, string Symbol, string NumericCode);

/// <summary>
/// Generates currency-related values such as code, name, symbol, or numeric code.
/// </summary>
/// <remarks>
/// This generator produces currency data based on the specified <see cref="CurrencyValue"/> type.
/// The returned value varies depending on the selected option, such as currency code or symbol.
/// </remarks>
public sealed class CurrencyGenerator(CurrencyValue value, Forge forge) : Generator<string>(forge)
{
	private static readonly CurrencyDefinition[] Currencies =
	[
		new("USD", "United States Dollar", "$", "840"),
		new("EUR", "Euro", "€", "978"),
		new("GBP", "British Pound", "£", "826"),
		new("JPY", "Japanese Yen", "¥", "392"),
		new("CNY", "Chinese Yuan", "¥", "156"),
		new("CHF", "Swiss Franc", "CHF", "756"),
		new("CAD", "Canadian Dollar", "C$", "124"),
		new("AUD", "Australian Dollar", "A$", "036"),
		new("NZD", "New Zealand Dollar", "NZ$", "554"),
		new("HKD", "Hong Kong Dollar", "HK$", "344"),
		new("SGD", "Singapore Dollar", "S$", "702"),
		new("INR", "Indian Rupee", "₹", "356"),
		new("BRL", "Brazilian Real", "R$", "986"),
		new("MXN", "Mexican Peso", "MX$", "484"),
		new("ZAR", "South African Rand", "R", "710"),
		new("SEK", "Swedish Krona", "kr", "752"),
		new("NOK", "Norwegian Krone", "kr", "578"),
		new("DKK", "Danish Krone", "kr", "208"),
		new("PLN", "Polish Złoty", "zł", "985"),
		new("CZK", "Czech Koruna", "Kč", "203"),
		new("HUF", "Hungarian Forint", "Ft", "348"),
		new("ILS", "Israeli New Shekel", "₪", "376"),
		new("TRY", "Turkish Lira", "₺", "949"),
		new("THB", "Thai Baht", "฿", "764"),
		new("VND", "Vietnamese Dong", "₫", "704"),
		new("PHP", "Philippine Peso", "₱", "608"),
		new("KRW", "South Korean Won", "₩", "410"),
		new("IDR", "Indonesian Rupiah", "Rp", "360"),
		new("MYR", "Malaysian Ringgit", "RM", "458"),
		new("NGN", "Nigerian Naira", "₦", "566"),
		new("KES", "Kenyan Shilling", "KSh", "404"),
		new("PKR", "Pakistani Rupee", "₨", "586"),
		new("BDT", "Bangladeshi Taka", "৳", "050"),
		new("EGP", "Egyptian Pound", "E£", "818"),
		new("SAR", "Saudi Riyal", "ر.س", "682"),
		new("AED", "UAE Dirham", "د.إ", "784"),
	];

	public override string Generate()
	{
		var currency = Rng.GetItem(Currencies);
		return value switch
		{
			CurrencyValue.Code => currency.Code,
			CurrencyValue.Name => currency.Name,
			CurrencyValue.Symbol => currency.Symbol,
			CurrencyValue.NumericCode => currency.NumericCode,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}
}
