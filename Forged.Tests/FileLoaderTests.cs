using System.Text.Json;
using System.Text.Json.Serialization;
using Forged.Core.Core;

namespace Forged.Tests;

internal sealed record UserData(string[] Prefixes, string[] Cores, string[] Suffixes);

[JsonSerializable(typeof(UserData))]
[JsonSourceGenerationOptions(
	AllowTrailingCommas = true,
	ReadCommentHandling = JsonCommentHandling.Skip,
	PropertyNameCaseInsensitive = true
)]
internal sealed partial class UserDataContext : JsonSerializerContext;

public class FileLoaderTests
{
	[Test]
	public async Task LoadData_WithJsoncSuffix_StripsTheExtension()
	{
		var data = new FileLoader().LoadData("en", "internet/username.jsonc", UserDataContext.Default.UserData);

		await Assert.That(data.Prefixes).IsNotNull();
		await Assert.That(data.Cores).IsNotNull();
		await Assert.That(data.Suffixes).IsNotNull();
	}
	
	[Test]
	public async Task LoadData_WithJson5Suffix_StripsTheExtension()
	{
		var data = new FileLoader().LoadData("en", "internet/username.json5", UserDataContext.Default.UserData);

		await Assert.That(data.Prefixes).IsNotNull();
		await Assert.That(data.Cores).IsNotNull();
		await Assert.That(data.Suffixes).IsNotNull();
	}

	[Test]
	public async Task LoadData_WithJsonSuffix_StripsTheExtension()
	{
		var data = new FileLoader().LoadData("en", "internet/username.json", UserDataContext.Default.UserData);

		await Assert.That(data.Prefixes).IsNotNull();
	}

	[Test]
	public async Task LoadData_WithRegionalLocale_FallsBackToBaseAndEnglish()
	{
		var data = new FileLoader().LoadData("xx-YY", "internet/username", UserDataContext.Default.UserData);

		await Assert.That(data.Prefixes.Length).IsGreaterThan(0);
	}
}