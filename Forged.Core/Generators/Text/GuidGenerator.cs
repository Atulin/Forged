namespace Forged.Core.Generators.Text;

public sealed class GuidGenerator(GuidGenerator.Kind kind, System.Random rng) : Generator<Guid>(rng)
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
