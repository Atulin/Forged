using Forged.Core.Core;

namespace Forged.Core.Generators.Person;

/// <summary>
/// A specialized generator for creating full names with various customizable patterns and probabilities.
/// Supports generating names with prefixes, infixes, suffixes, middle names, hyphenated names,
/// and compound names based on weighted probabilities.
/// </summary>
/// <remarks>
/// The <see cref="NameGenerator"/> class extends the base <see cref="Generator{T}"/> to produce
/// string-based name data. It utilizes various configurable parameters to influence the generated
/// structure and format of the name. The component relies on external data sources for name segments
/// and supports locale-specific name generation.
/// </remarks>
/// <param name="male">Weight assigned for generating male first names.</param>
/// <param name="female">Weight assigned for generating female first names.</param>
/// <param name="middle">Probability of including a middle name.</param>
/// <param name="hyphenated">Probability of creating hyphenated names.</param>
/// <param name="compound">Probability of generating compound names.</param>
/// <param name="prefix">Probability of including a prefix in the name.</param>
/// <param name="infix">Probability of including an infix in the name.</param>
/// <param name="suffix">Probability of appending a suffix to the name.</param>
/// <param name="forge">The instance of <see cref="Forge"/> that provides access to randomization, localization,
/// and data-loading utilities required for name generation.</param>
public sealed class NameGenerator(
	float male,
	float female,
	float middle,
	float hyphenated,
	float compound,
	float prefix,
	float infix,
	float suffix,
	Forge forge
) : Generator<string>(forge)
{
	private readonly LastNameGenerator _lastNameGenerator = new(hyphenated, compound, forge);

	public override string Generate()
	{
		var segments = new List<string>();
		
		var weight = male + female;
		var roll = Rng.NextDouble() * weight;
		
		var firstNames = roll < male 
			? FileLoader.LoadData(Locale.Name, "person/name/male_first", CommonContext.Default.ListString) 
			: FileLoader.LoadData(Locale.Name, "person/name/female_first", CommonContext.Default.ListString);

		if (Rng.NextDouble() < prefix)
		{
			var prefixes = FileLoader.LoadData(Locale.Name, "person/name/prefix", CommonContext.Default.ListString);
			segments.Add(Rng.GetItem(prefixes));
		}
		
		segments.Add(Rng.GetItem(firstNames));

		if (Rng.NextDouble() < middle)
		{
			segments.Add(Rng.GetItem(firstNames));
		}
		
		if (Rng.NextDouble() < infix)
		{
			var infixes = FileLoader.LoadData(Locale.Name, "person/name/infix", CommonContext.Default.ListString);
			segments.Add(Rng.GetItem(infixes));
		}
		
		segments.Add(_lastNameGenerator.Generate());
		
		if (Rng.NextDouble() < suffix)
		{
			var suffixes = FileLoader.LoadData(Locale.Name, "person/name/suffix", CommonContext.Default.ListString);
			segments.Add(Rng.GetItem(suffixes));
		}

		return string.Join(" ", segments).Replace("* ", "");
	}
}