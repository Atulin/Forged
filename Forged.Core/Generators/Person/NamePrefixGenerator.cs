using Forged.Core.Core;

namespace Forged.Core.Generators.Person;

/// <summary>
/// Represents a generator for creating name prefixes.
/// </summary>
public sealed class NamePrefixGenerator(Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var list = FileLoader.LoadData(Locale.Name, "person/name/prefix", CommonContext.Default.ListString);
		return Rng.GetItem(list);
	}
}