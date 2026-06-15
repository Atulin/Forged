using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Forged.Core.Core;
using JetBrains.Annotations;
using NetEscapades.EnumGenerators;

namespace Forged.Core.Generators.Text;

/// <summary>
/// </summary>
public sealed class WaffleGenerator(int sentences, WaffleStyle style, Forge forge) : Generator<string>(forge)
{
	private static readonly ConcurrentDictionary<(string locale, string file), List<TemplateCompiler.Token>[]> Cache = new();

	public override string Generate()
	{
		var data = FileLoader.LoadData(Locale.Name, "waffle", SentenceCorpusContext.Default.DictionaryWaffleStyleSentenceCorpus);
		var corpus = data[style] ?? throw new InvalidOperationException($"No corpus found for style {style}.");

		var templates = Cache.GetOrAdd(
			(Locale.Name, style.ToStringFast()),
			static (_, ctx) => ctx.Select(TemplateCompiler.Compile).ToArray(),
			factoryArgument: corpus.Templates
		);

		var sb = new StringBuilder();
		for (var i = 0; i <= sentences; i++)
		{
			var template = templates[Rng.Next(templates.Length)];
			var rendered = TemplateCompiler.Render(template, new Dictionary<string, Func<string>>
			{
				["subject"] = () => corpus.Subjects[Rng.Next(corpus.Subjects.Length)],
				["verb"] = () => corpus.Verbs[Rng.Next(corpus.Verbs.Length)],
				["object"] = () => corpus.Objects[Rng.Next(corpus.Objects.Length)],
				["adjective"] = () => corpus.Adjectives[Rng.Next(corpus.Adjectives.Length)],
			}).AsSpan();
			sb.Append(char.ToUpperInvariant(rendered[0]));
			sb.Append(rendered[1..]);
			sb.Append(' ');
		}

		if (sb[^1] == ' ')
		{
			sb.Remove(sb.Length - 1, 1);
		}
		
		return sb.ToString();
	}
}

[EnumExtensions]
public enum WaffleStyle
{
	Fiction,
	Technical,
}

[UsedImplicitly]
internal sealed record SentenceCorpus(string[] Subjects, string[] Verbs, string[] Objects, string[] Adjectives, string[] Templates);

[UsedImplicitly]
[JsonSerializable(typeof(Dictionary<WaffleStyle, SentenceCorpus>))]
[JsonSourceGenerationOptions(
	AllowTrailingCommas = true,
	ReadCommentHandling = JsonCommentHandling.Skip,
	PropertyNameCaseInsensitive = true,
	UseStringEnumConverter = true
)]
internal sealed partial class SentenceCorpusContext : JsonSerializerContext;