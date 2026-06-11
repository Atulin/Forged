using Forged.Core.Generators;
using Forged.Core.Generators.Text;

namespace Forged.Core.Modules;

public sealed class ForgeText(Forge forge)
{
	public Generator<string> Alphanumeric(int length)
		=> new AlphanumericStringGenerator(length, length, forge.Rng);
	public Generator<string> Alphanumeric(int minLength, int maxLength)
		=> new AlphanumericStringGenerator(minLength, maxLength, forge.Rng);
	
	public Generator<string> Alpha(int length)
		=> new AlphaStringGenerator(length, length, forge.Rng);
	public Generator<string> Alpha(int minLength, int maxLength)
		=> new AlphaStringGenerator(minLength, maxLength, forge.Rng);
	
	public Generator<string> Pronounceable(int length)
		=> new PronounceableStringGenerator(length, length, forge.Rng);
	public Generator<string> Pronounceable(int minLength, int maxLength)
		=> new PronounceableStringGenerator(minLength, maxLength, forge.Rng);
	
	public Generator<string> Lorem(int length)
		=> new LoremIpsumGenerator(length, length, forge.Rng);
	public Generator<string> Lorem(int minLength, int maxLength)
		=> new LoremIpsumGenerator(minLength, maxLength, forge.Rng);

	public Generator<string> Hex(int length)
		=> new HexStringGenerator(length, length, forge.Rng);
	public Generator<string> Hex(int minLength, int maxLength)
		=> new HexStringGenerator(minLength, maxLength, forge.Rng);

	public Generator<Guid> Guid(GuidGenerator.Kind kind = GuidGenerator.Kind.V4)
		=> new GuidGenerator(kind, forge.Rng);

	public Generator<string> Template(string template)
		=> new TemplateStringGenerator(template, forge.Rng);
}
