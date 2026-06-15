using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Forged.Core.Generators.Person;

/// <summary>
/// Generates random usernames with configurable options for prefixes, suffixes, and leet-speak substitutions.
/// </summary>
/// <param name="prefixChance">The probability (0.0 to 1.0) of including a prefix in the username.</param>
/// <param name="suffixChance">The probability (0.0 to 1.0) of including a suffix in the username.</param>
/// <param name="leetChance">The probability (0.0 to 1.0) of replacing characters with leet-speak equivalents.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class UsernameGenerator(float prefixChance, float suffixChance, float leetChance, Forge forge) : Generator<string>(forge)
{
	private readonly FrozenDictionary<char, string[]> _leetReplacements = new Dictionary<char, string[]>
	{
		['a'] = ["4", "@", "^"],
		['b'] = ["8", "I3", "l3"],
		['c'] = ["(", "[", "<", "¢"],
		['d'] = [")", "I)", "l)"],
		['e'] = ["3"],
		['f'] = ["|=", "ph"],
		['g'] = ["6", "9", "&"],
		['h'] = ["#", "|-|", "]-[", "4"],
		['i'] = ["1", "!", "|"],
		['j'] = ["_|", "_("],
		['k'] = ["X", "|<", "|("],
		['l'] = ["1", "|", "|_"],
		['m'] = [@"/\/\", @"\|\/\|", "^^"],
		['n'] = ["/\\/", @"\|/\|", "^/"],
		['o'] = ["0", "()"],
		['p'] = ["|*", "|o", "|²"],
		['q'] = ["9", "(,)"],
		['r'] = ["2", "|2", "|?"],
		['s'] = ["5", "$", "z", "Z"],
		['t'] = ["7", "+", "†"],
		['u'] = ["|_|", "v", "V"],
		['v'] = ["\\/"],
		['w'] = [@"\/\/", "vv", "VV"],
		['x'] = ["><", "}{"],
		['y'] = ["'/", "j"],
		['z'] = ["2"],
	}.ToFrozenDictionary();

	/// <summary>
	/// Generates a random username.
	/// </summary>
	/// <returns>A randomly generated username that may include a prefix, suffix, and leet-speak substitutions based on the configured probabilities.</returns>
	public override string Generate()
	{
		var data = FileLoader.LoadData(Locale.Name, "username", UserDataContext.Default.UserData);

		var usePrefix = Rng.NextSingle() < prefixChance;
		var useSuffix = Rng.NextSingle() < suffixChance;

		var prefix = usePrefix ? data.Prefixes[Rng.Next(data.Prefixes.Length)] : string.Empty;
		var suffix = useSuffix ? data.Suffixes[Rng.Next(data.Suffixes.Length)] : string.Empty;
		var core = data.Cores[Rng.Next(data.Cores.Length)].AsSpan();

		if (!(leetChance > 0))
		{
			return $"{prefix}{new string(core)}{suffix}";
		}
		
		var newCore = new List<char>(core.Length);
		foreach (var ch in core)
		{
			if (Rng.NextSingle() < leetChance)
			{
				if (_leetReplacements.TryGetValue(ch, out var replacement))
				{
					newCore.AddRange(Rng.GetItems(replacement, 1)[0]);
				}
				
				newCore.Add(ch);
			}
			else
			{
				newCore.Add(ch);
			}
		}

		return $"{prefix}{new string(newCore.ToArray())}{suffix}";
	}
}

internal sealed record UserData(string[] Prefixes, string[] Cores, string[] Suffixes);

[UsedImplicitly]
[JsonSerializable(typeof(UserData))]
[JsonSourceGenerationOptions(AllowTrailingCommas = true, ReadCommentHandling = JsonCommentHandling.Skip, PropertyNameCaseInsensitive = true)]
internal sealed partial class UserDataContext : JsonSerializerContext;