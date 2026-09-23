using Forged.Core.Core;

namespace Forged.Core.Generators.Person;

/// <summary>
/// A sealed generator class designed to produce randomized last names based on specified probabilities
/// for hyphenated names, compound names, or single names.
/// </summary>
public sealed class LastNameGenerator(float hyphenated, float compound, Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var list = FileLoader.LoadData(Locale.Name, "person/name/last", CommonContext.Default.ListString);

		var comp = hyphenated + compound;

		return Rng.NextDouble() switch
		{
			var r when r < hyphenated => $"{Rng.GetItem(list)}-{Rng.GetItem(list)}",
			var r when r < comp => $"{Rng.GetItem(list)} {Rng.GetItem(list)}",
			_ => Rng.GetItem(list)
		};
	}
}