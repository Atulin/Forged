using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// Generates random US-based bank routing numbers adhering to the standard routing number structure.
/// This generator can generate both valid and invalid routing numbers as well as test routing numbers
/// for specific use cases.
/// </summary>
/// <remarks>
/// Routing numbers consist of a 9-digit numeric structure, including a prefix, a body, and a checksum
/// value. The checksum is computed using Mod 10 arithmetic, and the generator ensures proper validity
/// where applicable.
/// </remarks>
/// <param name="valid">
/// Determines whether the generated routing number is valid. If set to false, the checksum is intentionally
/// made invalid.
/// </param>
/// <param name="test">
/// Specifies whether the generator should return a predefined test routing number. Test routing numbers
/// are selected from a set of predefined numbers for testing purposes.
/// </param>
/// <param name="forge">
/// An instance of the <see cref="Forge"/> class, providing access to randomization and localization
/// utilities.
/// </param>
public sealed class RoutingNumberGenerator(bool valid, bool test, Forge forge) : Generator<string>(forge)
{
	private static readonly string[] Prefixes =
	[
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12",
		"21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32",
		"61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "80",
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

	public override string Generate()
	{
		if (test)
		{
			return Rng.GetItem(TestRoutingNumbers);
		}

		var body = $"{Rng.GetItem(Prefixes)}{Rng.Digits(6)}";
		var checkDigit = (10 - Checksum($"{body}0") % 10) % 10;
		if (!valid)
		{
			checkDigit = (checkDigit + 1) % 10;
		}
		return $"{body}{checkDigit}";
	}

	private static int Checksum(string value)
	{
		var digits = value.Select(character => character - '0').ToArray();
		return 3 * (digits[0] + digits[3] + digits[6])
			+ 7 * (digits[1] + digits[4] + digits[7])
			+ digits[2] + digits[5] + digits[8];
	}
}
