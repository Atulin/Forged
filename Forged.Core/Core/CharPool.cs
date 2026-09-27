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

	private static readonly char[] Alphanumeric =
	[
		.. AsciiChars[NumericRange],
		.. AsciiChars[UppercaseRange],
		.. AsciiChars[LowercaseRange]
	];

	private static readonly char[] Alphabetical =
	[
		.. AsciiChars[UppercaseRange],
		.. AsciiChars[LowercaseRange]
	];

	private static readonly char[] Numeric = AsciiChars[NumericRange];

	public static char[] Get(CharKind kind) => kind switch
	{
		CharKind.Ascii => AsciiChars,
		CharKind.Printable => Printable,
		CharKind.Alphanumeric => Alphanumeric,
		CharKind.Alphabetical => Alphabetical,
		CharKind.Uppercase => AsciiChars[UppercaseRange],
		CharKind.Lowercase => AsciiChars[LowercaseRange],
		CharKind.Numeric => Numeric,
		_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
	};

	public static int Fill(Span<char> span, CharKind kind)
	{
		var asciiSpan = AsciiChars.AsSpan();

		ReadOnlySpan<char> pool = kind switch
		{
			CharKind.Ascii => asciiSpan,
			CharKind.Printable => Printable,
			CharKind.Alphanumeric => Alphanumeric,
			CharKind.Alphabetical => Alphabetical,
			CharKind.Uppercase => asciiSpan[UppercaseRange],
			CharKind.Lowercase => asciiSpan[LowercaseRange],
			CharKind.Numeric => Numeric,
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
		};

		pool.CopyTo(span);
		return pool.Length;
	}
}

public enum CharKind
{
	/// <summary>
	/// Represents the ASCII character set, encompassing characters with
	/// integer values in the range from 0 to 127 inclusive.
	/// </summary>
	Ascii,

	/// <summary>
	/// Represents all printable characters in the ASCII character set, encompassing
	/// characters with integer values in the range from 32 to 126 inclusive.
	/// </summary>
	Printable,

	/// <summary>
	/// Represents alphanumeric characters, which include both letters
	/// (uppercase and lowercase) and numeric digits.
	/// </summary>
	Alphanumeric,

	/// <summary>
	/// Represents alphabetic characters, encompassing both uppercase and lowercase letters
	/// as defined by the Unicode standard.
	/// </summary>
	Alphabetical,

	/// <summary>
	/// Represents the set of uppercase alphabetical characters. These characters
	/// typically range from 'A' to 'Z' in the ASCII character set.
	/// </summary>
	Uppercase,

	/// <summary>
	/// Represents the set of lowercase alphabetical characters.
	/// </summary>
	Lowercase,

	/// <summary>
	/// Represents the set of numeric characters, encompassing digits
	/// with integer values in the range from 0 ('0') to 9 ('9') inclusive.
	/// </summary>
	Numeric,
}
