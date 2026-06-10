using Forged.Core.Generators;
using Forged.Core.Generators.Text;

namespace Forged.Core.Modules;

public sealed class ForgeText(Forge forge)
{
	public Generator<string> Alphanumeric(int length)
		=> new AlphanumericStringGenerator(length, forge.Rng);

	public Generator<string> Hex(int length)
		=> new HexStringGenerator(length, forge.Rng);

	public Generator<Guid> Guid(GuidGenerator.Kind kind = GuidGenerator.Kind.V4)
		=> new GuidGenerator(kind, forge.Rng);

	public Generator<string> Template(string template)
		=> new TemplateStringGenerator(template, forge.Rng);
}
