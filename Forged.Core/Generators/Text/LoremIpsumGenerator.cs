namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates random Lorem Ipsum text.
/// </summary>
/// <param name="minWords">The minimum number of words to generate.</param>
/// <param name="maxWords">The maximum number of words to generate.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class LoremIpsumGenerator(int minWords, int maxWords, Forge forge) : Generator<string>(forge)
{
	private static readonly string[] Lipsum =
	[
		// repeat common words for higher chance
		"lorem", "ipsum", "dolor", "sit", "amet",
		"lorem", "ipsum", "dolor",
		
		// base lipsum corpus
		"lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
		"sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
		"magna", "aliqua", "enim", "ad", "minim", "veniam", "quis", "nostrud",
		"exercitation", "ullamco", "laboris", "nisi", "aliquip", "ex", "ea", "commodo",
		"consequat", "duis", "aute", "irure", "in", "reprehenderit", "voluptate",
		"velit", "esse", "cillum", "fugiat", "nulla", "pariatur", "excepteur",
		"sint", "occaecat", "cupidatat", "non", "proident", "sunt", "culpa",
		"qui", "officia", "deserunt", "mollit", "anim", "id", "est", "laborum",

		// extended Latin-like filler vocabulary
		"praesent", "venenatis", "lectus", "aenean", "scelerisque", "phasellus",
		"viverra", "turpis", "massa", "ultricies", "faucibus", "pulvinar",
		"elementum", "nunc", "lobortis", "facilisis", "iaculis", "gravida",
		"porttitor", "congue", "placerat", "hendrerit", "integer", "neque",
		"vehicula", "sagittis", "suscipit", "malesuada", "ornare", "condimentum",
		"tincidunt", "augue", "risus", "bibendum", "phasellus", "euismod",
		"interdum", "fermentum", "dictum", "nibh", "blandit", "curae",
		"lacinia", "mauris", "accumsan", "imperdiet", "phasellus", "dapibus",
		"eros", "cras", "sollicitudin", "habitasse", "platea", "dictumst",
		"maecenas", "rutrum", "varius", "quam", "vitae", "sapien", "tellus",
		"mauris", "pretium", "fusce", "ornare", "donec", "ullamcorper"
	];

	/// <summary>
	/// Generates a random Lorem Ipsum string.
	/// </summary>
	/// <returns>A random Lorem Ipsum string with the specified number of words.</returns>
	public override string Generate()
	{
		var length = minWords == maxWords
			? minWords
			: Rng.Next(minWords, maxWords + 1);

		return string.Join(" ", Rng.GetItems(Lipsum, length));
	}
}
