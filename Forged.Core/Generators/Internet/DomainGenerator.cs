using System.Text.Json;
using System.Text.Json.Serialization;
using Forged.Core.Core;
using JetBrains.Annotations;

namespace Forged.Core.Generators.Internet;

/// <summary>
/// Generates random domain.
/// </summary>
public sealed class DomainGenerator : Generator<string>
{
	private readonly float _ccSldChance;
	private readonly Lazy<DomainData> _cache;

	/// <summary>
	/// Generates random domain.
	/// </summary>
	/// <param name="ccSldChance">Chance to prepend the domain with a functional second-level domain</param>
	/// <param name="forge">The Forge instance to use.</param>
	public DomainGenerator(float ccSldChance, Forge forge) : base(forge)
	{
		_ccSldChance = ccSldChance;
		_cache = new(() => FileLoader.LoadData("en", "internet/domains", DomainDataContext.Default.DomainData));
	}

	/// <summary>
	/// Generates a random domain.
	/// </summary>
	/// <returns>A random domain.</returns>
	public override string Generate()
	{
		var data = _cache.Value;

		var domain = Rng.GetItem(data.Tld);
		if (_ccSldChance  <= 0 || Rng.Chance(_ccSldChance))
		{
			return domain;
		}

		var slds = data.CcSld.Except([domain]).ToArray();
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