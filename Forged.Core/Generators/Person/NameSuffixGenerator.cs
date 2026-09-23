using Forged.Core.Core;

namespace Forged.Core.Generators.Person;

/// <summary>
/// Represents a generator for creating name suffixes such as "Jr.", "Sr.", "III", etc.
/// </summary>
public sealed class NameSuffixGenerator(Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var list = FileLoader.LoadData(Locale.Name, "person/name/suffix", CommonContext.Default.ListString);
		return Rng.GetItem(list);
	}
}