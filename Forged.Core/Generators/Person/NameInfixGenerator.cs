using Forged.Core.Core;

namespace Forged.Core.Generators.Person;

/// <summary>
/// A generator for producing infixes used in personal names based on the specified locale.
/// </summary>
public sealed class NameInfixGenerator(Forge forge) : Generator<string>(forge)
{
	public override string Generate()
	{
		var list = FileLoader.LoadData(Locale.Name, "person/name/infix", CommonContext.Default.ListString);
		return Rng.GetItem(list).TrimEnd('*');
	}
}