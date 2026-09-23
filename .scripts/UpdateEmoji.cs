using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using var client = new HttpClient();
var file = await client.GetStringAsync("https://www.unicode.org/Public/emoji/4.0/emoji-test.txt");
var span = file.AsSpan();

var emojis = new Dictionary<string, string>();
foreach (var line in span.EnumerateLines())
{
	if (line.Trim().IsEmpty || line[0] == '#')
	{
		continue;
	}

	var hash = line.IndexOf('#');
	var emojiEntry = line[(hash+1)..].Trim();
	
	var firstSpace = emojiEntry.IndexOf(' ');
	var emoji = emojiEntry[..firstSpace];
	var name = emojiEntry[(firstSpace + 1)..];
	
	emojis[name.ToString()] = emoji.ToString();
}

Console.WriteLine($"Found {emojis.Count} emojis");

const string path = "./Forged.Core/Locales/en/text/emoji.json5";

var json = JsonSerializer.Serialize(emojis, new DictionaryContext(new JsonSerializerOptions
{
	WriteIndented = true,
	Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
}).DictionaryStringString);
await File.WriteAllTextAsync(path, json, Encoding.UTF8);

return;

[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal sealed partial class DictionaryContext : JsonSerializerContext;