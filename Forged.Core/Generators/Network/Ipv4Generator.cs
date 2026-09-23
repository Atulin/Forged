using System.Net;
using Forged.Core.Core;

namespace Forged.Core.Generators.Network;

/// <summary>
/// A generator for creating random IPv4 addresses. Supports generating both
/// public and private IP ranges based on the specified configuration.
/// </summary>
/// <remarks>
/// This class derives from <see cref="Generator{IPAddress}"/> and utilizes
/// its underlying Forge instance to handle random number generation.
/// When configured to generate only public IPs, it excludes ranges
/// reserved for private, multicast, loopback, and other special purposes
/// as defined by standard IP allocation guidelines.
/// </remarks>
/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IPv4 address. A chance of 0 or 1 short-circuits the random roll.</param>
/// <param name="forge">The Forge instance to use.</param>
public sealed class Ipv4Generator(float publicChance, Forge forge) : Generator<IPAddress>(forge)
{
	public override IPAddress Generate()
	{
		Span<byte> bytes = stackalloc byte[4];
		Rng.NextBytes(bytes);

		if (!Rng.Chance(publicChance))
		{
			return new IPAddress(bytes);
		}

		do
		{
			switch (bytes)
			{
				// Exclude 0.x.x.x ("This host" / Software defaults)
				case [0, ..]:
				// Exclude 10.0.0.0/8 (Private)
				case [10, ..]:
				// Exclude 100.64.0.0/10 (Carrier-grade NAT)
				case [100, >= 64 and <= 127, ..]:
				// Exclude 127.0.0.0/8 (Loopback)
				case [127, ..]:
				// Exclude 169.254.0.0/16 (Link-local / APIPA)
				case [169, 254, ..]:
				// Exclude 172.16.0.0/12 (Private)
				case [172, >= 16 and <= 31, ..]:
				// Exclude 192.0.0.0/24 (IETF Protocol Assignments)
				case [192, 0, 0, ..]:
				// Exclude 192.0.2.0/24 (TEST-NET-1 Documentation)
				case [192, 0, 2, ..]:
				// Exclude 192.88.99.0/24 (6to4 Relay Anycast)
				case [192, 88, 99, ..]:
				// Exclude 192.168.0.0/16 (Private)
				case [192, 168, ..]:
				// Exclude 198.18.0.0/15 (Benchmarking)
				case [198, 18 or 19, ..]:
				// Exclude 198.51.100.0/24 (TEST-NET-2 Documentation)
				case [198, 51, 100, ..]:
				// Exclude 203.0.113.0/24 (TEST-NET-3 Documentation)
				case [203, 0, 113, ..]:
				// Exclude 255.255.255.255 (Broadcast)
				case [255, 255, 255, 255]:
				// Exclude 224.0.0.0/4 (Class D - Multicast) & 240.0.0.0/4 (Class E - Reserved)
				case [>= 224, ..]:
					Rng.NextBytes(bytes);
					continue;
				
				default:
					return new IPAddress(bytes);
			}
		} while (true);
	}
}