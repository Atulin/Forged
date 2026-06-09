namespace Forged.Core.Generators.Text;

public sealed class GuidGenerator(GuidGenerator.Kind kind = GuidGenerator.Kind.V4) : Generator<Guid>
{
	public override Guid Generate()
	{
		return kind switch
		{
			Kind.V7 => Guid.CreateVersion7(),
			_ => Guid.NewGuid(),
		};
	}

	public enum Kind
	{
		V4,
		V7,
	}
}