using System.Net;
using System.Net.NetworkInformation;
using Forged.Core.Core;
using Forged.Core.Generators;
using Forged.Core.Generators.Network;

namespace Forged.Core.Modules;

public sealed class ForgeNetwork(Forge forge)
{
	/// <summary>
	/// Generates an IPv4 address based on the provided parameters.
	/// </summary>
	/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IPv4 address. Defaults to 1.0.</param>
	/// <returns>A generator instance that produces IPv4 addresses.</returns>
	public Generator<IPAddress> Ipv4(float publicChance = 1.0f)
		=> new Ipv4Generator(publicChance, forge);

	/// <summary>
	/// Generates an IPv6 address based on the provided parameters.
	/// </summary>
	/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IPv6 address. Defaults to 1.0.</param>
	/// <returns>A generator instance that produces IPv6 addresses.</returns>
	public Generator<IPAddress> Ipv6(float publicChance = 1.0f)
		=> new Ipv6Generator(publicChance, forge);

	/// <summary>
	/// Generates an IP address, either IPv4 or IPv6, based on the provided parameters.
	/// </summary>
	/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IP address. Defaults to 1.0.</param>
	/// <param name="ipv6Chance">The probability (0 to 1) of generating an IPv6 address as opposed to IPv4. Defaults to 0.45.</param>
	/// <returns>A generator instance that produces IP addresses.</returns>
	public Generator<IPAddress> IpAddress(float publicChance = 1.0f, float ipv6Chance = .45f)
		=> forge.Basic.Func(() => forge.Rng.Chance(ipv6Chance)
			? Ipv6(publicChance).Generate()
			: Ipv4(publicChance).Generate());

	/// <summary>
	/// Generates a network port number based on the provided parameters.
	/// </summary>
	/// <param name="wellKnownChance">The probability (0.0 to 1.0) of including well-known ports (1–1023). Defaults to 0.0.</param>
	/// <returns>A generator instance that produces network port numbers.</returns>
	public Generator<int> Port(float wellKnownChance = 0.0f)
		=> new PortGenerator(wellKnownChance, forge);

	/// <summary>
	/// Generates an IPv4 endpoint, consisting of an IPv4 address and a port number.
	/// </summary>
	/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IPv4 address. Defaults to 1.0.</param>
	/// <param name="wellKnownPortChance">The probability (0.0 to 1.0) of generating a well-known port (0-1023). Defaults to 0.0.</param>
	/// <returns>A generator instance that produces IPv4 endpoints.</returns>
	public Generator<IPEndPoint> Ipv4Endpoint(float publicChance = 1.0f, float wellKnownPortChance = 0.0f)
		=> Endpoint(0.0f, publicChance, wellKnownPortChance);

	/// <summary>
	/// Generates an IPv6 endpoint that includes both an IPv6 address and a port.
	/// </summary>
	/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IPv6 address. Defaults to 1.0.</param>
	/// <param name="wellKnownPortChance">The probability (0.0 to 1.0) of generating a well-known port. Defaults to 0.0.</param>
	/// <returns>A generator instance that produces IPv6 endpoints.</returns>
	public Generator<IPEndPoint> Ipv6Endpoint(float publicChance = 1.0f, float wellKnownPortChance = 0.0f)
		=> Endpoint(1.0f, publicChance, wellKnownPortChance);

	/// <summary>
	/// Generates an IP endpoint (IPEndPoint) based on the specified parameters.
	/// </summary>
	/// <param name="ipv6Chance">The probability (0.0 to 1.0) of generating an IPv6 endpoint. Defaults to 0.45.</param>
	/// <param name="publicChance">The probability (0.0 to 1.0) of generating a public IP address. Defaults to 1.0.</param>
	/// <param name="wellKnownPortChance">The probability (0.0 to 1.0) of generating an endpoint with a well-known port. Defaults to 0.0.</param>
	/// <returns>A generator instance that produces IPEndPoint objects.</returns>
	public Generator<IPEndPoint> Endpoint(float ipv6Chance = .45f, float publicChance = 1.0f, float wellKnownPortChance = 0.0f)
		=> new EndpointGenerator(ipv6Chance, publicChance, wellKnownPortChance, forge);

	/// <summary>
	/// Generates URI schemes based on the provided parameters.
	/// </summary>
	/// <param name="strip">Indicates whether to strip non-standard schemes from the output. Defaults to false.</param>
	/// <returns>A generator instance that produces URI schemes.</returns>
	public Generator<string> UriScheme(bool strip = false)
		=> new UriScheme(strip, forge);

	/// <summary>
	/// Generates a MAC (Media Access Control) address based on the provided parameters.
	/// </summary>
	/// <param name="locallyAdministeredChance">The probability (0.0 to 1.0) of generating a locally administered MAC address. Defaults to 0.0.</param>
	/// <returns>A generator instance that produces MAC addresses.</returns>
	public Generator<PhysicalAddress> MacAddress(float locallyAdministeredChance = 0.0f)
		=> new MacAddressGenerator(locallyAdministeredChance, forge);
}