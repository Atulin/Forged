using Forged.Core.Generators;
using Forged.Core.Generators.Text;

namespace Forged.Core;

public partial class Forged<TModel>
{
	public static class Text
	{
		public static Generator<string> Alphanumeric(int length) => new AlphanumericStringGenerator(length);

		public static Generator<string> Hex(int length) => new HexStringGenerator(length);
		
		public static Generator<Guid> Guid(GuidGenerator.Kind kind = GuidGenerator.Kind.V4) => new GuidGenerator(kind);

		public static Generator<string> Template(string template) => new TemplateStringGenerator(template);
	}
}