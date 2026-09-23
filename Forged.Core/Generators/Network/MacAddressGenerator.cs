using System.Net.NetworkInformation;
using Forged.Core.Core;

namespace Forged.Core.Generators.Network;

/// <summary>
/// A generator for producing random MAC (Media Access Control) addresses.
/// </summary>
/// <remarks>
/// The generated MAC address can be either globally unique or locally administered
/// based on the specified probability for locally administered addresses.
/// </remarks>
/// <example>
/// The MAC address is generated in accordance with the IEEE 802 standard:
/// - The least significant bit of the first byte determines whether the address is
/// unicast (0) or multicast (1).
/// - The second least significant bit of the first byte determines whether the address is
/// globally unique (0) or locally administered (1).
/// </example>
public sealed class MacAddressGenerator(float locallyAdministeredChance, Forge forge) : Generator<PhysicalAddress>(forge)
{
	public override PhysicalAddress Generate()
	{
		Span<byte> bytes = stackalloc byte[6];
		Rng.NextBytes(bytes);

		bytes[0] = Rng.Chance(locallyAdministeredChance) 
			// Set Unicast bit (LSB = 0) and Locally Administered bit (2nd LSB = 1)
			? (byte)(bytes[0] & 0xFE | 0x02) 
			// Ensure Unicast bit only (LSB = 0)
			: (byte)(bytes[0] & 0xFE);

		return new PhysicalAddress([..bytes]);
	}
}