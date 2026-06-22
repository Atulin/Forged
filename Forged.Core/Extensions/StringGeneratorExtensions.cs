using System.Globalization;
using System.Text.RegularExpressions;
using Forged.Core.Generators;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Extensions;

/// <summary>
/// Extension methods for string generators.
/// </summary>
public static class StringGeneratorExtensions
{
	/// <summary>
	/// Extension methods for string generators.
	/// </summary>
	extension(Generator<string> generator)
	{
		/// <summary>
		/// Creates a generator that converts strings to uppercase.
		/// </summary>
		/// <returns>A generator that produces uppercase strings.</returns>
		public Generator<string> ToUpper()
			=> new UppercaseGenerator(generator, generator.Forge);
		
		/// <summary>
		/// Creates a generator that converts strings to lowercase.
		/// </summary>
		/// <returns>A generator that produces lowercase strings.</returns>
		public Generator<string> ToLower()
			=> new LowercaseGenerator(generator, generator.Forge);

		/// <summary>
		/// Creates a generator that produces substrings of a specified range of characters.
		/// </summary>
		/// <param name="start">The starting index of the range, inclusive.</param>
		/// <param name="end">
		/// The ending index of the range, exclusive. If null, the generator will include all characters up to the length of the string.
		/// </param>
		/// <returns>A generator that produces substrings within the specified range.</returns>
		public Generator<string> Range(int start, int? end = null)
			=> new RangeGenerator(generator, start, end, generator.Forge);

		/// <summary>
		/// Creates a generator that replaces all occurrences of a specified string in the generated text with another string.
		/// </summary>
		/// <param name="oldValue">The string to be replaced in the generated text.</param>
		/// <param name="newValue">The string that will replace all occurrences of <paramref name="oldValue"/>.</param>
		/// <returns>A generator that produces strings with the specified replacements applied.</returns>
		public Generator<string> Replace(string oldValue, string newValue)
			=> new ReplaceGenerator(generator, oldValue, newValue, generator.Forge);

		/// <summary>
		/// Creates a generator that replaces all occurrences of a specified char in the generated text with another char.
		/// </summary>
		/// <param name="oldValue">The char to be replaced in the generated text.</param>
		/// <param name="newValue">The char that will replace all occurrences of <paramref name="oldValue"/>.</param>
		/// <returns>A generator that produces strings with the specified replacements applied.</returns>
		public Generator<string> Replace(char oldValue, char newValue)
			=> new ReplaceGenerator(generator, oldValue.ToString(), newValue.ToString(), generator.Forge);

		/// <summary>
		/// Creates a generator that replaces substrings in the generated strings
		/// based on a provided regular expression.
		/// </summary>
		/// <param name="regex">The regular expression used to find substrings to replace.</param>
		/// <param name="newValue">The value to replace matched substrings with.</param>
		/// <returns>A generator that produces strings with substrings replaced according to the specified regular expression and replacement value.</returns>
		public Generator<string> Replace(Regex regex, string newValue)
			=> new RegexReplaceGenerator(generator, regex, newValue, generator.Forge);
		
		/// <summary>
		/// Creates a generator that converts strings to title case.
		/// </summary>
		/// <param name="cultureInfo">The culture to use for title case conversion. If null, uses invariant culture.</param>
		/// <returns>A generator that produces title case strings.</returns>
		public Generator<string> ToTitleCase(CultureInfo? cultureInfo = null)
			=> new TitleCaseGenerator(generator, generator.Forge, cultureInfo);
		
		/// <summary>
		/// Creates a generator that capitalizes the first character of strings.
		/// </summary>
		/// <param name="cultureInfo">The culture to use for capitalization. If null, uses invariant culture.</param>
		/// <returns>A generator that produces strings with the first character capitalized.</returns>
		public Generator<string> Capitalize(CultureInfo? cultureInfo = null)
			=> new CapitalizeGenerator(generator, generator.Forge, cultureInfo);
		
		/// <summary>
		/// Creates a generator that converts strings into properly formatted sentences.
		/// </summary>
		/// <param name="sentenceLength">The exact number of words per sentence.</param>
		/// <param name="cultureInfo">The culture to use for capitalization. If null, uses invariant culture.</param>
		/// <returns>A generator that produces properly formatted sentences.</returns>
		public Generator<string> Sentencify(int sentenceLength, CultureInfo? cultureInfo = null)
			=> new SentencifyGenerator(generator, sentenceLength, sentenceLength, generator.Forge, cultureInfo);
		
		/// <summary>
		/// Creates a generator that converts strings into properly formatted sentences with variable length.
		/// </summary>
		/// <param name="minSentenceLength">The minimum number of words per sentence.</param>
		/// <param name="maxSentenceLength">The maximum number of words per sentence.</param>
		/// <param name="cultureInfo">The culture to use for capitalization. If null, uses invariant culture.</param>
		/// <returns>A generator that produces properly formatted sentences.</returns>
		public Generator<string> Sentencify(int minSentenceLength, int maxSentenceLength, CultureInfo? cultureInfo = null)
			=> new SentencifyGenerator(generator, minSentenceLength, maxSentenceLength, generator.Forge, cultureInfo);
	}
}
