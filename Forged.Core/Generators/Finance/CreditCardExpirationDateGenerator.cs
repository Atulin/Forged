using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// Generates credit card expiration dates within a defined range.
/// This generator supports producing both past and future expiration dates
/// based on a specified probability and reference date.
/// Inherits functionality from the base Generator class.
/// </summary>
public sealed class CreditCardExpirationDateGenerator(
	DateTimeOffset? today,
	float expirationChance,
	Forge forge) : Generator<DateTimeOffset>(forge)
{
	private readonly DateTimeOffset Today = today ?? DateTimeOffset.Now;
	private static readonly DateTimeOffset MinimumDate = new(1, 1, 1, 0, 0, 0, TimeSpan.Zero);

	public override DateTimeOffset Generate()
	{
		var monthOffset = Rng.Chance(expirationChance)
			? -Rng.Next(1, 241)
			: Rng.Next(0, 241);
		var minimumMonth = MinimumDate.Year * 12L + MinimumDate.Month - 1;
		var maximumMonth = DateTimeOffset.MaxValue.Year * 12L + DateTimeOffset.MaxValue.Month - 1;
		var monthIndex = Math.Clamp(Today.Year * 12L + Today.Month - 1 + monthOffset, minimumMonth, maximumMonth);
		var year = (int)(monthIndex / 12);
		var month = (int)(monthIndex % 12) + 1;
		return new DateTimeOffset(year, month, 1, 0, 0, 0, Today.Offset);
	}
}
