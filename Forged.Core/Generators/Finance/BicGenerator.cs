using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// The <c>BicGenerator</c> class is responsible for generating valid or test Bank Identifier Codes (BICs).
/// It leverages the underlying forge system to handle randomness, localization, and configuration.
/// </summary>
/// <remarks>
/// A BIC, or SWIFT code, is used to uniquely identify banks and financial institutions globally.
/// This generator can produce either valid codes or test codes depending on the input parameters.
/// </remarks>
public sealed class BicGenerator(bool valid, bool test, Forge forge) : Generator<string>(forge)
{
	private Dictionary<string, int>? _countries;

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

	private static readonly char[] UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

	public override string Generate()
	{
		if (test)
		{
			return Rng.GetItem(TestBics);
		}

		_countries ??= FileLoader.LoadData("finance/iban_countries", CommonContext.Default.DictionaryStringInt32);

		var bank = Rng.GetItems(UppercaseLetters, 4);
		var country = valid ? Rng.GetItem(_countries.Keys.ToArray()) : "ZZ";
		var location = Rng.GetItems(UppercaseLetters, 2);

		ReadOnlySpan<char> body = [.. bank, .. country, .. location, .."XXX"];

		return new string(body);
	}
}