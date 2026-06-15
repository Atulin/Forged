using Forged.Core.Generators;
using Forged.Core.Generators.Person;

namespace Forged.Core.Modules;

/// <summary>
/// Provides methods for generating person-related data.
/// </summary>
public sealed class ForgePerson(Forge forge)
{
	/// <summary>
	/// Creates a generator that produces random usernames.
	/// </summary>
	/// <param name="prefixChance">The probability (0.0 to 1.0) of including a prefix in the username. Defaults to 0.5.</param>
	/// <param name="suffixChance">The probability (0.0 to 1.0) of including a suffix in the username. Defaults to 0.5.</param>
	/// <param name="leetChance">The probability (0.0 to 1.0) of replacing characters with leet-speak equivalents. Defaults to 0.1.</param>
	/// <returns>A generator that produces random usernames.</returns>
	public Generator<string> Username(float prefixChance = 0.5f, float suffixChance = 0.5f, float leetChance = 0.1f)
		=> new UsernameGenerator(prefixChance, suffixChance, leetChance, forge);
}