using Forged.Core.Generators.Utility;

namespace Forged.Core.Generators;

public static class GeneratorExtensions
{
	public static Generator<string> ToUpper(this Generator<string> generator)
		=> new UppercaseGenerator(generator);

	public static Generator<string> ToLower(this Generator<string> generator)
		=> new LowercaseGenerator(generator);
}