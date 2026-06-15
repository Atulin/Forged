using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.FileProviders;

namespace Forged.Core.Core;

public sealed class FileLoader
{
	private readonly EmbeddedFileProvider _fileProvider = new(typeof(FileLoader).Assembly);

	private static class Cache<T> where T : class
	{
		public static readonly ConcurrentDictionary<(string locale, string file), T> Data = new();
	}

	public T LoadData<T>(string locale, string file, JsonTypeInfo<T> typeInfo) where T : class
	{
		return Cache<T>.Data.GetOrAdd((locale, file), static (target, ctx) => {
			var (provider, info) = ctx;
			var (locale, file) = target;

			if (file.EndsWith(".json5"))
			{
				file = file[..^6];
			}

			if (file.EndsWith(".json"))
			{
				file = file[..^5];
			}

			var fileInfo = provider.GetFileInfo($"Locales/{locale}/{file}.json5");

			if (!fileInfo.Exists && locale.Contains('-'))
			{
				var baseLocale = locale.Split('-')[0];
				fileInfo = provider.GetFileInfo($"Locales/{baseLocale}/{file}.json5");
			}

			if (!fileInfo.Exists)
			{
				fileInfo = provider.GetFileInfo($"Locales/en/{file}.json5");
			}

			using var stream = fileInfo.CreateReadStream();
			return JsonSerializer.Deserialize(stream, info) ?? throw new JsonException($"Failed to deserialize {file} data for {target}");
		}, factoryArgument: (_fileProvider, typeInfo));
	}
}