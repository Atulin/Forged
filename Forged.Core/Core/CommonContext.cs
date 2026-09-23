using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Forged.Core.Core;

[JsonSerializable(typeof(List<string>))]
[JsonSourceGenerationOptions(
	ReadCommentHandling = JsonCommentHandling.Skip,
	AllowTrailingCommas = true,
	WriteIndented = true
)]
[UsedImplicitly]
internal sealed partial class CommonContext : JsonSerializerContext;