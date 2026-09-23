namespace Forged.Core.Core;

internal static class CharPool
{
	public const int MaxLength = 128;

	private static readonly char[] AsciiChars = [.. Enumerable.Range(0, 128).Select(i => (char)i)];
	
	private static readonly Range PrintableRange = 32..127;
	private static readonly Range NumericRange = 48..58;
	private static readonly Range UppercaseRange = 65..91;
	private static readonly Range LowercaseRange = 97..123;
	
	private static readonly char[] Printable = AsciiChars[PrintableRange];
	private static readonly char[] Alphanumeric = [..AsciiChars[NumericRange], ..AsciiChars[UppercaseRange], ..AsciiChars[LowercaseRange]];
	private static readonly char[] Alphabetical = [..AsciiChars[UppercaseRange], ..AsciiChars[LowercaseRange]];
	private static readonly char[] Numeric = AsciiChars[NumericRange];

	public static int FillCharPool(Span<char> span, CharKind kind)
	{
		var asciiSpan = AsciiChars.AsSpan();
		
		ReadOnlySpan<char> pool = kind switch
		{
			CharKind.Ascii => asciiSpan,
			CharKind.Printable => Printable,
			CharKind.Alphanumeric => Alphanumeric,
			CharKind.Alphabetical => Alphabetical,
			CharKind.Numeric => Numeric,
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
		};
		
		pool.CopyTo(span);
		return pool.Length;
	}
}

public enum CharKind
{
	Ascii,
	Printable,
	Alphanumeric,
	Alphabetical,
	Numeric,
}