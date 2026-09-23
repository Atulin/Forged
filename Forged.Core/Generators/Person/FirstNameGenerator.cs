using Forged.Core.Core;

namespace Forged.Core.Generators.Person;

/// <summary>
/// </summary>
public sealed class FirstNameGenerator(float female, float male, Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var weight = male + female;
		var roll = Rng.NextDouble() * weight;
		
		var list = roll < male 
			? FileLoader.LoadData(Locale.Name, "person/name/male_first", CommonContext.Default.ListString) 
			: FileLoader.LoadData(Locale.Name, "person/name/female_first", CommonContext.Default.ListString);

		return Rng.GetItem(list);
	}
}