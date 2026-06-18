using System.Globalization;
using Forged.Core.Generators.Utility.Text;

namespace Forged.Core.Generators.Extensions;

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
