using Forged.Core.Core;

namespace Forged.Core.Generators.Text;

/// <summary>
/// A generator class for producing arrays of characters with customizable properties such as
/// character kind and range for the number of characters to generate.
/// </summary>
public sealed class CharsGenerator(int minCount, int maxCount, CharKind kind, Forge forge) : Generator<char[]>(forge)
{
	public override char[] Generate()
	{
		Span<char> pool = stackalloc char[CharPool.MaxLength];
		var poolLength = CharPool.FillCharPool(pool, kind);
		
		var count = minCount == maxCount 
			? minCount 
			: Rng.Next(minCount, maxCount);
		
		return Rng.GetItems(pool[..poolLength], count);
	}
}