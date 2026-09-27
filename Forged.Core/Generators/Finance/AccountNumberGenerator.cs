using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// Represents a generator for creating random account numbers within a specified length range.
/// </summary>
public sealed class AccountNumberGenerator(int minLength, int maxLength, Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var length = minLength == maxLength ? minLength : Rng.Next(minLength, maxLength + 1);
		return Rng.GetString(CharPool.Get(CharKind.Numeric), length);
	}
}
