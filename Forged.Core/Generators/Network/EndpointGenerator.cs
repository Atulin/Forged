using System.Net;
using Forged.Core.Core;

namespace Forged.Core.Generators.Network;

/// <summary>
/// Generates random network endpoint data including IP addresses and ports.
/// </summary>
public sealed class EndpointGenerator(float ipv6Chance, float publicChance, float wellKnownPortChance, Forge forge) : Generator<IPEndPoint>(forge)
{
	public override IPEndPoint Generate()
	{
		var ip = Rng.Chance(ipv6Chance) 
			? Forge.Network.Ipv6(publicChance) 
			: Forge.Network.Ipv4(publicChance);
		
		var port = Forge.Network.Port(wellKnownPortChance);
		
		return new IPEndPoint(ip, port);
	}
}