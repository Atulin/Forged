using Forged.Core.Generators;
using Forged.Core.Generators.Internet;

namespace Forged.Core.Modules;

/// <summary>
/// Provides methods for generating internet-related data.
/// </summary>
public sealed class ForgeInternet(Forge forge)
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

	/// <summary>
	/// Creates a generator that produces random domain names.
	/// </summary>
	/// <param name="ccSldChance">The probability (0.0 to 1.0) of including a functional second-level domain (ccSLD) in the domain name. Defaults to 0.0.</param>
	/// <returns>A generator that produces random domain names.</returns>
	public Generator<string> Domain(float ccSldChance = 0.0f)
		=> new DomainGenerator(ccSldChance, forge);

	/// <summary>
	/// Creates a generator that produces random email addresses based on the specified parameters.
	/// </summary>
	/// <param name="kind">The type of email address to generate. Defaults to <see cref="EmailKind.Random"/>.</param>
	/// <param name="provider">An optional generator to supply email provider names. Defaults to null, which uses a username generator.</param>
	/// <returns>A generator that produces random email addresses.</returns>
	public Generator<string> Email(EmailKind kind = EmailKind.Random, IGenerator<string>? provider = null)
		=> new EmailGenerator(kind, provider, forge);
}