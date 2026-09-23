using System.Net;
using Forged.Core.Core;

namespace Forged.Core.Generators.Network;

/// <summary>
/// A generator responsible for creating random IPv6 addresses, optionally restricted to the range of public/global unicast addresses.
/// </summary>
/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public/global unicast IPv6 address. A chance of 0 or 1 short-circuits the random roll.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class Ipv6Generator(float publicChance, Forge forge) : Generator<IPAddress>(forge)
{
	public override IPAddress Generate()
	{
		Span<byte> bytes = stackalloc byte[16];
		Rng.NextBytes(bytes);

		if (Rng.Chance(publicChance))
		{
			// Public IPv6 addresses (Global Unicast) use the prefix 2000::/3.
			// The first byte binary ranges: 0010 0000 (0x20) to 0011 1111 (0x3F).
			bytes[0] = (byte)Rng.Next(0x20, 0x40);
		}

		return new IPAddress(bytes);
	}
}