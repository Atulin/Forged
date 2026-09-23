using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Forged.Core;
using Forged.Core.Generators;

namespace Forged.Tests;

public class ForgeNetworkTests
{
	private static Forge NewForge(int seed)
		=> new(new Random(seed), CultureInfo.InvariantCulture);

	private static List<T> Draw<T>(Generator<T> generator, int count)
	{
		var values = new List<T>(count);
		for (var i = 0; i < count; i++)
		{
			values.Add(generator.Generate());
		}
		return values;
	}

	private static bool IsPrivateIpv4(IPAddress address)
	{
		var b = address.GetAddressBytes();
		return b switch
		{
			[10, ..] => true,
			[172, >= 16 and <= 31, ..] => true,
			[192, 168, ..] => true,
			_ => false,
		};
	}

	private static bool IsPublicIpv4(IPAddress address)
	{
		var b = address.GetAddressBytes();
		return b switch
		{
			// 0.0.0.0/8 (This host / Software defaults)
			[0, ..] => false,
			// 10.0.0.0/8 (Private)
			[10, ..] => false,
			// 100.64.0.0/10 (Carrier-grade NAT)
			[100, >= 64 and <= 127, ..] => false,
			// 127.0.0.0/8 (Loopback)
			[127, ..] => false,
			// 169.254.0.0/16 (Link-local / APIPA)
			[169, 254, ..] => false,
			// 172.16.0.0/12 (Private)
			[172, >= 16 and <= 31, ..] => false,
			// 192.0.0.0/24 (IETF Protocol Assignments)
			[192, 0, 0, ..] => false,
			// 192.0.2.0/24 (TEST-NET-1 Documentation)
			[192, 0, 2, ..] => false,
			// 192.88.99.0/24 (6to4 Relay Anycast)
			[192, 88, 99, ..] => false,
			// 192.168.0.0/16 (Private)
			[192, 168, ..] => false,
			// 198.18.0.0/15 (Benchmarking)
			[198, 18 or 19, ..] => false,
			// 198.51.100.0/24 (TEST-NET-2 Documentation)
			[198, 51, 100, ..] => false,
			// 203.0.113.0/24 (TEST-NET-3 Documentation)
			[203, 0, 113, ..] => false,
			// 255.255.255.255 (Broadcast)
			[255, 255, 255, 255] => false,
			// 224.0.0.0/4 (Class D - Multicast) & 240.0.0.0/4 (Class E - Reserved)
			[>= 224, ..] => false,
			_ => true,
		};
	}

	[Test]
	public async Task Ipv4_DefaultChance_ProducesValidIpv4Addresses()
	{
		var forge = NewForge(2001);
		var values = Draw(forge.Network.Ipv4(), 100);

		foreach (var value in values)
		{
			await Assert.That(value).IsNotNull();
			await Assert.That(value.AddressFamily).IsEqualTo(AddressFamily.InterNetwork);
			await Assert.That(value.GetAddressBytes()).Count().IsEqualTo(4);
		}
	}

	[Test]
	public async Task Ipv4_PublicChanceOne_ProducesOnlyPublicAddresses()
	{
		var forge = NewForge(2002);
		var values = Draw(forge.Network.Ipv4(1f), 200);

		foreach (var value in values)
		{
			await Assert.That(IsPublicIpv4(value)).IsTrue();
		}
	}

	[Test]
	public async Task Ipv4_ChanceZero_CanProduceReservedPrivateAddresses()
	{
		var forge = NewForge(2003);
		var values = Draw(forge.Network.Ipv4(0f), 200);

		await Assert.That(values.Any(IsPrivateIpv4)).IsTrue();
		await Assert.That(values.Any(v => !IsPrivateIpv4(v))).IsTrue();
	}

	[Test]
	public async Task Ipv4_ProducesVaryingAddresses()
	{
		var forge = NewForge(2004);
		var values = Draw(forge.Network.Ipv4(), 200);

		await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
	}

	[Test]
	public async Task Ipv4_IsDeterministic_ForTheSameSeed()
	{
		var forge1 = NewForge(2005);
		var forge2 = NewForge(2005);

		var expected = forge1.Network.Ipv4().Generate();
		var actual = forge2.Network.Ipv4().Generate();

		await Assert.That(actual).IsEqualTo(expected);
	}

	[Test]
	public async Task Ipv6_DefaultChance_ProducesValidIpv6Addresses()
	{
		var forge = NewForge(2010);
		var values = Draw(forge.Network.Ipv6(), 100);

		foreach (var value in values)
		{
			await Assert.That(value).IsNotNull();
			await Assert.That(value.AddressFamily).IsEqualTo(AddressFamily.InterNetworkV6);
			await Assert.That(value.GetAddressBytes()).Count().IsEqualTo(16);
		}
	}

	[Test]
	public async Task Ipv6_PublicChanceOne_ProducesGlobalUnicastAddresses()
	{
		var forge = NewForge(2011);
		var values = Draw(forge.Network.Ipv6(1f), 200);

		foreach (var value in values)
		{
			// Global Unicast uses the 2000::/3 prefix: first byte in the 0x20..0x3F range.
			var first = value.GetAddressBytes()[0];
			await Assert.That((int)first).IsInRange(0x20, 0x3F);
		}
	}

	[Test]
	public async Task Ipv6_ChanceZero_CanProduceNonGlobalUnicastAddresses()
	{
		var forge = NewForge(2012);
		var values = Draw(forge.Network.Ipv6(0f), 200);

		await Assert.That(values.Any(v => v.GetAddressBytes()[0] is < 0x20 or >= 0x40)).IsTrue();
	}

	[Test]
	public async Task Ipv6_ProducesVaryingAddresses()
	{
		var forge = NewForge(2013);
		var values = Draw(forge.Network.Ipv6(), 200);

		await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
	}

	[Test]
	public async Task IpAddress_Ipv6ChanceZero_ProducesOnlyIpv4()
	{
		var forge = NewForge(2020);
		var values = Draw(forge.Network.IpAddress(1f, 0f), 100);

		foreach (var value in values)
		{
			await Assert.That(value.AddressFamily).IsEqualTo(AddressFamily.InterNetwork);
		}
	}

	[Test]
	public async Task IpAddress_DefaultChance_ProducesBothFamiliesOverManyDraws()
	{
		var forge = NewForge(2021);
		var values = Draw(forge.Network.IpAddress(), 200);
		var v4 = values.Count(v => v.AddressFamily == AddressFamily.InterNetwork);
		var v6 = values.Count(v => v.AddressFamily == AddressFamily.InterNetworkV6);

		await Assert.That(v4).IsGreaterThan(0);
		await Assert.That(v6).IsGreaterThan(0);
	}

	[Test]
	public async Task Port_WellKnownDisabled_ProducesPortsAbove1023()
	{
		var forge = NewForge(2030);
		var values = Draw(forge.Network.Port(0f), 200);

		foreach (var value in values)
		{
			await Assert.That(value).IsInRange(1024, 65534);
		}
	}

	[Test]
	public async Task Port_WellKnownEnabled_CanProduceWellKnownPorts()
	{
		var forge = NewForge(2031);
		var values = Draw(forge.Network.Port(1f), 300);

		foreach (var value in values)
		{
			await Assert.That(value).IsInRange(1, 65534);
		}

		await Assert.That(values.Any(v => v < 1024)).IsTrue();
	}

	[Test]
	public async Task Port_ProducesVaryingValues()
	{
		var forge = NewForge(2032);
		var values = Draw(forge.Network.Port(), 200);

		await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
	}

	[Test]
	public async Task Port_IsDeterministic_ForTheSameSeed()
	{
		var forge1 = NewForge(2033);
		var forge2 = NewForge(2033);

		var expected = forge1.Network.Port().Generate();
		var actual = forge2.Network.Port().Generate();

		await Assert.That(actual).IsEqualTo(expected);
	}

	[Test]
	public async Task Endpoint_Ipv6ChanceZero_ProducesIpv4Endpoints()
	{
		var forge = NewForge(2040);
		var values = Draw(forge.Network.Endpoint(0f, 1f, 0f), 100);

		foreach (var value in values)
		{
			await Assert.That(value.Address.AddressFamily).IsEqualTo(AddressFamily.InterNetwork);
			await Assert.That(value.Port).IsInRange(1024, 65534);
		}
	}

	[Test]
	public async Task Endpoint_Ipv6ChanceOne_ProducesIpv6Endpoints()
	{
		var forge = NewForge(2041);
		var values = Draw(forge.Network.Endpoint(1f, 1f), 100);

		foreach (var value in values)
		{
			await Assert.That(value.Address.AddressFamily).IsEqualTo(AddressFamily.InterNetworkV6);
			await Assert.That(value.Port).IsInRange(1024, 65534);
		}
	}

	[Test]
	public async Task Endpoint_DefaultChance_ProducesBothFamiliesOverManyDraws()
	{
		var forge = NewForge(2042);
		var values = Draw(forge.Network.Endpoint(), 200);
		var v4 = values.Count(v => v.Address.AddressFamily == AddressFamily.InterNetwork);
		var v6 = values.Count(v => v.Address.AddressFamily == AddressFamily.InterNetworkV6);

		await Assert.That(v4).IsGreaterThan(0);
		await Assert.That(v6).IsGreaterThan(0);
	}

	[Test]
	public async Task Ipv4Endpoint_ProducesIpv4Endpoints()
	{
		var forge = NewForge(2043);
		var values = Draw(forge.Network.Ipv4Endpoint(), 100);

		foreach (var value in values)
		{
			await Assert.That(value.Address.AddressFamily).IsEqualTo(AddressFamily.InterNetwork);
		}
	}

	[Test]
	public async Task Ipv6Endpoint_ProducesIpv6Endpoints()
	{
		var forge = NewForge(2044);
		var values = Draw(forge.Network.Ipv6Endpoint(), 100);

		foreach (var value in values)
		{
			await Assert.That(value.Address.AddressFamily).IsEqualTo(AddressFamily.InterNetworkV6);
		}
	}

	private static readonly string[] Protocols =
	[
		"http://", "https://", "ftp://", "ftps://", "sftp://", "file:///",
		"mailto:", "tel:", "sms:", "ssh://", "git://", "magnet:",
	];

	private static readonly string[] StrippedSchemes =
	[
		"http", "https", "ftp", "ftps", "sftp", "file",
		"mailto", "tel", "sms", "ssh", "git", "magnet",
	];

	[Test]
	public async Task UriScheme_NotStripped_ProducesKnownProtocols()
	{
		var forge = NewForge(2050);
		var values = Draw(forge.Network.UriScheme(false), 100);

		foreach (var value in values)
		{
			await Assert.That(Protocols.Contains(value)).IsTrue();
		}
	}

	[Test]
	public async Task UriScheme_Stripped_OmitsColonAndDelimiters()
	{
		var forge = NewForge(2051);
		var values = Draw(forge.Network.UriScheme(true), 100);

		foreach (var value in values)
		{
			await Assert.That(value.Contains(':')).IsFalse();
			await Assert.That(value.All(char.IsLower)).IsTrue();
		}
	}

	[Test]
	public async Task UriScheme_Stripped_ProducesRecognizedSchemeNames()
	{
		var forge = NewForge(2052);
		var values = Draw(forge.Network.UriScheme(true), 100);

		foreach (var value in values)
		{
			await Assert.That(StrippedSchemes.Contains(value)).IsTrue();
		}
	}

	[Test]
	public async Task UriScheme_ProducesVaryingProtocols()
	{
		var forge = NewForge(2053);
		var values = Draw(forge.Network.UriScheme(), 200);

		await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
	}

	[Test]
	public async Task MacAddress_ProducesSixByteUnicastAddresses()
	{
		var forge = NewForge(2060);
		var values = Draw(forge.Network.MacAddress(0f), 100);

		foreach (var value in values)
		{
			var bytes = value.GetAddressBytes();
			await Assert.That(bytes).Count().IsEqualTo(6);
			await Assert.That(bytes[0] & 0x01).IsEqualTo(0);
		}
	}

	[Test]
	public async Task MacAddress_LocallyAdministered_ProducesUnicastLocallyAdministeredAddresses()
	{
		var forge = NewForge(2061);
		var values = Draw(forge.Network.MacAddress(1f), 100);

		foreach (var value in values)
		{
			var bytes = value.GetAddressBytes();
			await Assert.That(bytes).Count().IsEqualTo(6);
			await Assert.That(bytes[0] & 0x01).IsEqualTo(0);
			await Assert.That(bytes[0] & 0x02).IsEqualTo(0x02);
		}
	}

	[Test]
	public async Task MacAddress_ProducesVaryingAddresses()
	{
		var forge = NewForge(2062);
		var values = Draw(forge.Network.MacAddress(), 200);

		await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
	}
}