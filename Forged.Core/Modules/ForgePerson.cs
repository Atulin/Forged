using Forged.Core.Generators;
using Forged.Core.Generators.Person;

namespace Forged.Core.Modules;

public sealed class ForgePerson(Forge forge)
{
	/// <summary>
	/// Generates a first name based on the specified gender probabilities.
	/// </summary>
	/// <param name="male">The probability for generating a male-associated first name. Value should be between 0 and 1.</param>
	/// <param name="female">The probability for generating a female-associated first name. Value should be between 0 and 1.</param>
	/// <returns>A generator that produces first names based on the specified gender probabilities.</returns>
	public Generator<string> FirstName(float male = .5f, float female = .5f)
		=> new FirstNameGenerator(male, female, forge);

	/// <summary>
	/// Generates a last name based on the specified probabilities for hyphenation and compound names.
	/// </summary>
	/// <param name="hyphenated">The probability of generating a hyphenated last name. Value should be between 0 and 1.</param>
	/// <param name="compound">The probability of generating a compound last name. Value should be between 0 and 1.</param>
	/// <returns>A generator that produces last names based on the specified probabilities.</returns>
	public Generator<string> LastName(float hyphenated = .04f, float compound = .02f)
		=> new LastNameGenerator(hyphenated, compound, forge);

	/// <summary>
	/// Generates a name prefix commonly used in names such as titles, honorifics, or qualifiers.
	/// </summary>
	/// <returns>A generator that produces name prefixes based on the configured locale or data set.</returns>
	public Generator<string> NamePrefix()
		=> new NamePrefixGenerator(forge);

	/// <summary>
	/// Generates an infix for a name, such as a middle name or title component, using locale and other configured values.
	/// </summary>
	/// <returns>A generator that produces name infixes.</returns>
	public Generator<string> NameInfix()
		=> new NameInfixGenerator(forge);

	/// <summary>
	/// Generates name suffixes commonly associated with formal titles, positions, or academic credentials.
	/// </summary>
	/// <returns>A generator that produces name suffixes.</returns>
	public Generator<string> NameSuffix()
		=> new NameSuffixGenerator(forge);

	/// <summary>
	/// Generates a full name based on the specified probabilities for components such as gender, middle names, hyphenation, and affixes.
	/// </summary>
	/// <param name="male">The probability for generating a male-associated first name. Value should be between 0 and 1.</param>
	/// <param name="female">The probability for generating a female-associated first name. Value should be between 0 and 1.</param>
	/// <param name="middle">The probability for including a middle name. Value should be between 0 and 1.</param>
	/// <param name="hyphenated">The probability for generating a hyphenated last name. Value should be between 0 and 1.</param>
	/// <param name="compound">The probability for generating a compound last name. Value should be between 0 and 1.</param>
	/// <param name="prefix">The probability for including a name prefix (e.g., Dr., Mr., Mrs.). Value should be between 0 and 1.</param>
	/// <param name="infix">The probability for including a name infix (e.g., "van", "de"). Value should be between 0 and 1.</param>
	/// <param name="suffix">The probability for including a name suffix (e.g., Jr., Sr., III). Value should be between 0 and 1.</param>
	/// <returns>A generator that produces full names based on the specified probabilities for name components.</returns>
	public Generator<string> FullName(
		float male = .5f,
		float female = .5f,
		float middle = .8f,
		float hyphenated = .04f,
		float compound = .02f,
		float prefix = 0f,
		float infix = .03f,
		float suffix = .04f
	)
		=> new NameGenerator(male, female, middle, hyphenated, compound, prefix, infix, suffix, forge);
}