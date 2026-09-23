using Forged.Core.Core;

namespace Forged.Core.Generators.Network;

/// <summary>
/// A generator for producing random port numbers. It provides options to include or exclude well-known ports.
/// </summary>
/// <param name="wellKnownChance">The probability (0.0 to 1.0) of including well-known ports (1–1023). A chance of 0 or 1 short-circuits the random roll.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class PortGenerator(float wellKnownChance, Forge forge) : Generator<int>(forge)
{
	public override int Generate() => Rng.Chance(wellKnownChance)
		? Rng.Next(1, 65535)
		: Rng.Next(1024, 65535);
}