namespace Forged.Core.Generators.Text;

/// <summary>
/// Generates random GUIDs.
/// </summary>
/// <param name="kind">The kind of GUID to generate.</param>
/// <param name="rng">The random number generator to use.</param>
public sealed class GuidGenerator(GuidGenerator.Kind kind, System.Random rng) : Generator<Guid>(rng)
{
	/// <summary>
	/// Generates a random GUID.
	/// </summary>
	/// <returns>A random GUID of the specified kind.</returns>
	public override Guid Generate()
	{
		return kind switch
		{
			Kind.V7 => Guid.CreateVersion7(),
			_ => Guid.NewGuid(),
		};
	}

	/// <summary>
	/// The kind of GUID to generate.
	/// </summary>
	public enum Kind
	{
		/// <summary>Generate a version 4 (random) GUID.</summary>
		V4,
		/// <summary>Generate a version 7 (time-based) GUID.</summary>
		V7,
	}
}
