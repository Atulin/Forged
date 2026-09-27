using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Globalization;
using NetEscapades.EnumGenerators;

namespace Forged.Core.Generators.Finance;

[Flags]
[EnumExtensions]
public enum CardOperator
{
	None = 0,
	Visa = 1 << 0,
	Mastercard = 1 << 1,
	AmericanExpress = 1 << 2,
	Discover = 1 << 3,
	DinersClub = 1 << 4,
	JCB = 1 << 5,
	UnionPay = 1 << 6,
	All = Visa | Mastercard | AmericanExpress | Discover | DinersClub | JCB | UnionPay,
	Any = All,
}

internal sealed record CardOperatorSpec(
	string[] Prefixes,
	int Length,
	int CvvLength,
	string TestNumber);

internal static class CardOperatorCatalog
{
	private static readonly string[] JcbPrefixes =
		[.. Enumerable.Range(3528, 62).Select(prefix => prefix.ToString(CultureInfo.InvariantCulture))];

	private static readonly FrozenDictionary<CardOperator, CardOperatorSpec> Specs = new Dictionary<CardOperator, CardOperatorSpec>
	{
		[CardOperator.Visa] = new(["4"], 16, 3, "4111111111111111"),
		[CardOperator.Mastercard] = new(["51", "52", "53", "54", "55"], 16, 3, "5555555555554444"),
		[CardOperator.AmericanExpress] = new(["34", "37"], 15, 4, "378282246310005"),
		[CardOperator.Discover] = new(["6011", "65"], 16, 3, "6011111111111117"),
		[CardOperator.DinersClub] = new(["300", "301", "302", "303", "304", "305", "36", "38"], 14, 3, "30569309025904"),
		[CardOperator.JCB] = new(JcbPrefixes, 16, 3, "3530111333300000"),
		[CardOperator.UnionPay] = new(["62"], 16, 3, "6212345678901232"),
	}.ToFrozenDictionary();

	public static ImmutableArray<CardOperator> Operators { get; } = [.. Specs.Keys];

	public static IEnumerable<CardOperatorSpec> ResolveSpec(CardOperator operators)
		=> ResolveOperator(operators).Select(flag => Specs[flag]);

	public static IEnumerable<CardOperator> ResolveOperator(CardOperator operators)
	{
		if ((operators & ~CardOperator.All) != 0 || operators == CardOperator.None)
		{
			throw new ArgumentOutOfRangeException(nameof(operators), operators, "At least one valid card operator is required.");
		}

		foreach (var flag in CardOperator.GetValues())
		{
			if (flag == 0 || (flag & flag - 1) != 0)
			{
				continue;
			}
			if (operators.HasFlagFast(flag))
			{
				yield return flag;
			}
		}
	}
}
