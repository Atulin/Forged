using System.Text.Json;
using System.Text.Json.Serialization;
using Forged.Core.Core;
using JetBrains.Annotations;

namespace Forged.Core.Generators.Internet;

/// <summary>
/// Generates random domain.
/// </summary>
public sealed class DomainGenerator(float ccSldChance, Forge forge) : Generator<string>(forge)
{
	/// <summary>
	/// Generates a random domain.
	/// </summary>
	/// <returns>A random domain.</returns>
	public override string Generate()
	{
		var data = FileLoader.LoadData("en", "internet/domains", DomainDataContext.Default.DomainData);

		var domain = Rng.GetItem(data.Tld);
		if (ccSldChance <= 0 || !Rng.Chance(ccSldChance))
		{
			return domain;
		}

		var slds = data.CcSld.Except([domain]).ToArray();
		if (slds.Length == 0)
		{
			return domain;
		}

		var sld = Rng.GetItem(slds);

		return $"{sld}.{domain}";
	}
}

internal sealed record DomainData(string[] Tld, string[] CcSld);

[UsedImplicitly]
[JsonSerializable(typeof(DomainData))]
[JsonSourceGenerationOptions(
	AllowTrailingCommas = true,
	ReadCommentHandling = JsonCommentHandling.Skip,
	PropertyNameCaseInsensitive = true
)]
internal sealed partial class DomainDataContext : JsonSerializerContext;