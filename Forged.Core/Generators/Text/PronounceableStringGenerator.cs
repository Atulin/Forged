using System.Text;

namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates random pronounceable strings using syllable-based generation.
/// </summary>
/// <param name="minSyllables">The minimum number of syllables to generate.</param>
/// <param name="maxSyllables">The maximum number of syllables to generate.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class PronounceableStringGenerator(int minSyllables, int maxSyllables, System.Random rng) : Generator<string>(rng)
{
	private static readonly string[] Onsets =
	[
		"b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "z",
		"ch", "sh", "th", "bl", "cl", "fl", "gl", "pl", "br", "cr", "dr", "fr", "gr", "pr", "tr", "st"
	];

	private static readonly string[] Nuclei =
	[
		"a", "e", "i", "o", "u", "ae", "ai", "ao", "ea", "ee", "ei", "oo", "ou", "ie"
	];

	private static readonly string[] Codas =
	[
		"", "b", "d", "f", "g", "k", "l", "m", "n", "p", "r", "s", "t", "x", "z",
		"ch", "sh", "th", "ck", "ng", "nk", "nt", "st", "rd", "ld"
	];
	
	/// <summary>
	/// Generates a random pronounceable string.
	/// </summary>
	/// <returns>A random pronounceable string.</returns>
	public override string Generate()
	{
		var length = minSyllables == maxSyllables 
			? minSyllables 
			: Rng.Next(minSyllables, maxSyllables + 1);

		var word = new StringBuilder();

		for (var i = 0; i < length; i++)
		{
			var onset = Onsets[Rng.Next(Onsets.Length)];
			var nucleus = Nuclei[Rng.Next(Nuclei.Length)];
			var coda = Codas[Rng.Next(Codas.Length)];

			// Don't start a syllable with a heavy onset if it's in the middle
			if (i > 0 && onset.Length > 1 && Rng.Next(2) == 0)
			{
				onset = onset[0].ToString();
			}

			// Reduce the chance of a coda in the middle
			if (i < length - 1 && Rng.Next(3) == 0)
			{
				coda = "";
			}

			word.Append(onset).Append(nucleus).Append(coda);
		}

		return word.ToString();
	}
}
