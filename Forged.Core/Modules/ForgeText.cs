using Forged.Core.Generators;
using Forged.Core.Generators.Text;

namespace Forged.Core.Modules;

/// <summary>
/// Provides methods for generating text data.
/// </summary>
public sealed class ForgeText(Forge forge)
{
	/// <summary>
	/// Creates a generator that produces random alphanumeric strings of a fixed length.
	/// </summary>
	/// <param name="length">The exact length of the string to generate.</param>
	/// <returns>A generator that produces random alphanumeric strings.</returns>
	public Generator<string> Alphanumeric(int length)
		=> new AlphanumericStringGenerator(length, length, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random alphanumeric strings of variable length.
	/// </summary>
	/// <param name="minLength">The minimum length of the string.</param>
	/// <param name="maxLength">The maximum length of the string.</param>
	/// <returns>A generator that produces random alphanumeric strings.</returns>
	public Generator<string> Alphanumeric(int minLength, int maxLength)
		=> new AlphanumericStringGenerator(minLength, maxLength, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random alphabetic strings of a fixed length.
	/// </summary>
	/// <param name="length">The exact length of the string to generate.</param>
	/// <returns>A generator that produces random alphabetic strings.</returns>
	public Generator<string> Alpha(int length)
		=> new AlphaStringGenerator(length, length, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random alphabetic strings of variable length.
	/// </summary>
	/// <param name="minLength">The minimum length of the string.</param>
	/// <param name="maxLength">The maximum length of the string.</param>
	/// <returns>A generator that produces random alphabetic strings.</returns>
	public Generator<string> Alpha(int minLength, int maxLength)
		=> new AlphaStringGenerator(minLength, maxLength, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random pronounceable strings of a fixed length.
	/// </summary>
	/// <param name="length">The exact number of syllables in the string.</param>
	/// <returns>A generator that produces random pronounceable strings.</returns>
	public Generator<string> Pronounceable(int length)
		=> new PronounceableStringGenerator(length, length, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random pronounceable strings of variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of syllables.</param>
	/// <param name="maxLength">The maximum number of syllables.</param>
	/// <returns>A generator that produces random pronounceable strings.</returns>
	public Generator<string> Pronounceable(int minLength, int maxLength)
		=> new PronounceableStringGenerator(minLength, maxLength, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random Lorem Ipsum text of a fixed length.
	/// </summary>
	/// <param name="length">The exact number of words to generate.</param>
	/// <returns>A generator that produces random Lorem Ipsum text.</returns>
	public Generator<string> Lorem(int length)
		=> new LoremIpsumGenerator(length, length, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random Lorem Ipsum text of variable length.
	/// </summary>
	/// <param name="minLength">The minimum number of words.</param>
	/// <param name="maxLength">The maximum number of words.</param>
	/// <returns>A generator that produces random Lorem Ipsum text.</returns>
	public Generator<string> Lorem(int minLength, int maxLength)
		=> new LoremIpsumGenerator(minLength, maxLength, forge.Rng);

	/// <summary>
	/// Creates a generator that produces random hexadecimal strings of a fixed length.
	/// </summary>
	/// <param name="length">The exact length of the hex string.</param>
	/// <returns>A generator that produces random hexadecimal strings.</returns>
	public Generator<string> Hex(int length)
		=> new HexStringGenerator(length, length, forge.Rng);
	
	/// <summary>
	/// Creates a generator that produces random hexadecimal strings of variable length.
	/// </summary>
	/// <param name="minLength">The minimum length of the hex string.</param>
	/// <param name="maxLength">The maximum length of the hex string.</param>
	/// <returns>A generator that produces random hexadecimal strings.</returns>
	public Generator<string> Hex(int minLength, int maxLength)
		=> new HexStringGenerator(minLength, maxLength, forge.Rng);

	/// <summary>
	/// Creates a generator that produces random GUIDs.
	/// </summary>
	/// <param name="kind">The kind of GUID to generate. Defaults to V4.</param>
	/// <returns>A generator that produces random GUIDs.</returns>
	public Generator<Guid> Guid(GuidGenerator.Kind kind = GuidGenerator.Kind.V4)
		=> new GuidGenerator(kind, forge.Rng);

	/// <summary>
	/// Creates a generator that produces strings based on a template with placeholders.
	/// </summary>
	/// <param name="template">The template string containing placeholders.</param>
	/// <returns>A generator that produces strings from the template.</returns>
	public Generator<string> Template(string template)
		=> new TemplateStringGenerator(template, forge.Rng);
}
