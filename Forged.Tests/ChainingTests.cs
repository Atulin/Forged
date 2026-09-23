using System.Globalization;
using System.Text.RegularExpressions;
using Forged.Core;
using Forged.Core.Extensions;

namespace Forged.Tests;

public class ChainingTests
{
    private static Forge NewForge(int seed)
        => new(new Random(seed), CultureInfo.InvariantCulture);

    [Test]
    public async Task Refine_TransformsTheGeneratedValue()
    {
        var forge = NewForge(51);
        var generator = forge.Basic.Literal(3).Refine(x => x * 2);

        await Assert.That(generator.Generate()).IsEqualTo(6);
    }

    [Test]
    public async Task Refine_CanChangeTheGeneratedType()
    {
        var forge = NewForge(52);
        var generator = forge.Text.Alphanumeric(5)
            .Array(3)
            .Refine(chars => string.Join("", chars));

        var value = generator.Generate();
        await Assert.That(value).Length().IsEqualTo(15);
        await Assert.That(value.All(char.IsLetterOrDigit)).IsTrue();
    }

    [Test]
    public async Task Or_WithProbabilityZero_AlwaysProducesTheInnerValue()
    {
        var forge = NewForge(53);
        var generator = forge.Basic.Literal("inner").Or("fallback", 0f);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo("inner");
        }
    }

    [Test]
    public async Task Or_WithProbabilityOne_AlwaysProducesTheFallback()
    {
        var forge = NewForge(54);
        var generator = forge.Basic.Literal("inner").Or("fallback", 1f);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo("fallback");
        }
    }

    [Test]
    public async Task OrDefault_WithProbabilityZero_AlwaysProducesTheInnerValue()
    {
        var forge = NewForge(55);
        var generator = forge.Basic.Literal("value").OrDefault(0f);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo("value");
        }
    }

    [Test]
    public async Task OrDefault_WithProbabilityOne_AlwaysProducesNull()
    {
        var forge = NewForge(56);
        var generator = forge.Basic.Literal("value").OrDefault(1f);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsNull();
        }
    }

    [Test]
    public async Task Enumerable_FixedLength_ProducesExactCount()
    {
        var forge = NewForge(57);
        var values = forge.Basic.Literal(7).Enumerable(3).Generate().ToList();

        await Assert.That(values).Count().IsEqualTo(3);
        foreach (var value in values)
        {
            await Assert.That(value).IsEqualTo(7);
        }
    }

    [Test]
    public async Task Enumerable_VariableLength_StaysWithinInclusiveRange()
    {
        var forge = NewForge(58);
        for (var i = 0; i < 50; i++)
        {
            var count = forge.Basic.Literal(1).Enumerable(2, 4).Generate().Count();
            await Assert.That(count).IsInRange(2, 4);
        }
    }

    [Test]
    public async Task Array_FixedLength_ProducesExactCount()
    {
        var forge = NewForge(59);
        var values = forge.Basic.Literal("x").Array(4).Generate();

        await Assert.That(values).Count().IsEqualTo(4);
    }

    [Test]
    public async Task Array_VariableLength_StaysWithinInclusiveRange()
    {
        var forge = NewForge(60);
        for (var i = 0; i < 50; i++)
        {
            await Assert.That(forge.Basic.Literal(1).Array(1, 3).Generate().Length).IsInRange(1, 3);
        }
    }

    [Test]
    public async Task List_FixedLength_ProducesExactCount()
    {
        var forge = NewForge(61);
        var values = forge.Basic.Literal(9).List(5).Generate();

        await Assert.That(values).Count().IsEqualTo(5);
    }

    [Test]
    public async Task HashSet_DeduplicatesValues()
    {
        var forge = NewForge(62);
        var values = forge.Basic.Literal(5).HashSet(10).Generate();

        await Assert.That(values).Count().IsEqualTo(1);
        await Assert.That(values.Contains(5)).IsTrue();
    }

    [Test]
    public async Task Memo_ExposesTheLatestGeneratedValue()
    {
        var forge = NewForge(63);
        var memoGenerator = forge.Text.Alpha(6).Memo(out var memoizedValue);

        for (var i = 0; i < 20; i++)
        {
            var drawn = memoGenerator.Generate();
            await Assert.That(memoizedValue.Generate()).IsEqualTo(drawn);
        }
    }

    [Test]
    public async Task Memo_OnlyRegeneratesWhenTheMemoGeneratorIsInvoked()
    {
        var forge = NewForge(64);
        var memoGenerator = forge.Random.Number<int>(1, 100_000).Memo(out var memoizedValue);

        var first = memoGenerator.Generate();
        var observedSecond = memoizedValue.Generate();

        await Assert.That(observedSecond).IsEqualTo(first);

        var second = memoGenerator.Generate();
        await Assert.That(memoizedValue.Generate()).IsEqualTo(second);
    }

    [Test]
    public async Task Cast_ConvertsToTheTargetType()
    {
        var forge = NewForge(65);
        var generator = forge.Basic.Literal<object>("hello").Cast<string>();

        await Assert.That(generator.Generate()).IsEqualTo("hello");
    }

    [Test]
    public async Task StructOrNull_WithProbabilityOne_AlwaysProducesNull()
    {
        var forge = NewForge(66);
        var generator = forge.Random.Number<int>(1, 10).OrNull(1f);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsNull();
        }
    }

    [Test]
    public async Task StructOrNull_WithProbabilityZero_AlwaysProducesAValue()
    {
        var forge = NewForge(67);
        var generator = forge.Random.Number<int>(1, 10).OrNull(0f);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsNotNull();
        }
    }

    [Test]
    public async Task StructNullable_BoxesStructValuesIntoNullable()
    {
        var forge = NewForge(68);
        var generator = forge.Random.Number<int>(42, 42).Nullable();

        for (var i = 0; i < 10; i++)
        {
            await Assert.That(generator.Generate()).IsNotNull();
        }
    }

    [Test]
    public async Task AsList_ConvertsEnumerableGeneratorToLists()
    {
        var forge = NewForge(69);
        var values = forge.Basic.Literal(1).Enumerable(3).AsList().Generate();

        await Assert.That(values).Count().IsEqualTo(3);
    }

    [Test]
    public async Task AsHashSet_ConvertsEnumerableGeneratorToHashSets()
    {
        var forge = NewForge(70);
        var values = forge.Basic.Literal(2).Enumerable(5).AsHashSet().Generate();

        await Assert.That(values).Count().IsEqualTo(1);
    }

    [Test]
    public async Task AsDictionary_KeysAndValuesFollowTheSelectors()
    {
        var forge = NewForge(71);
        var generator = forge.Basic.Literal(new[] { 1, 2, 3 })
            .Refine(IEnumerable<int> (numbers) => numbers)
            .AsDictionary(key => key, value => value * 10);

        var dict = generator.Generate();
        await Assert.That(dict.Count).IsEqualTo(3);
        await Assert.That(dict[1]).IsEqualTo(10);
        await Assert.That(dict[2]).IsEqualTo(20);
        await Assert.That(dict[3]).IsEqualTo(30);
    }

    [Test]
    public async Task Shuffle_PreservesContentAndCount()
    {
        var forge = NewForge(72);
        var generator = forge.Basic.Literal(new[] { 1, 2, 3, 4, 5 })
            .Refine(IEnumerable<int> (x) => x)
            .Shuffle();

        for (var i = 0; i < 30; i++)
        {
            var shuffled = generator.Generate().ToList();
            await Assert.That(shuffled).Count().IsEqualTo(5);
            var sorted = shuffled.OrderBy(x => x).ToList();
            for (var expected = 1; expected <= 5; expected++)
            {
                await Assert.That(sorted[expected - 1]).IsEqualTo(expected);
            }
        }
    }

    [Test]
    public async Task StringToUpper_UpperCasesGeneratedStrings()
    {
        var forge = NewForge(73);
        var value = forge.Text.Alpha(6).ToUpper().Generate();

        await Assert.That(value).IsEqualTo(value.ToUpperInvariant());
        await Assert.That(value.Any(char.IsLower)).IsFalse();
    }

    [Test]
    public async Task StringToLower_LowerCasesGeneratedStrings()
    {
        var forge = NewForge(74);
        var value = forge.Text.Alpha(6).ToLower().Generate();

        await Assert.That(value.Any(char.IsUpper)).IsFalse();
    }

    [Test]
    public async Task StringRange_ReturnsTheRequestedSubstring()
    {
        var forge = NewForge(75);
        var generator = forge.Basic.Literal("abcdef").Range(2, 4);

        await Assert.That(generator.Generate()).IsEqualTo("cd");
    }

    [Test]
    public async Task StringRange_WithoutEnd_ReturnsTheTail()
    {
        var forge = NewForge(76);
        var generator = forge.Basic.Literal("abcdef").Range(2);

        await Assert.That(generator.Generate()).IsEqualTo("cdef");
    }

    [Test]
    public async Task StringReplace_ReplacesAllOccurrences()
    {
        var forge = NewForge(77);
        var generator = forge.Basic.Literal("a-b-c").Replace("-", "/");

        await Assert.That(generator.Generate()).IsEqualTo("a/b/c");
    }

    [Test]
    public async Task StringReplace_CharOverload_ReplacesAllCharacters()
    {
        var forge = NewForge(78);
        var generator = forge.Basic.Literal("abc").Replace('b', 'X');

        await Assert.That(generator.Generate()).IsEqualTo("aXc");
    }

    [Test]
    public async Task StringReplace_RegexOverload_ReplacesMatchedSubstrings()
    {
        var forge = NewForge(79);
        var generator = forge.Basic.Literal("abc123").Replace(new Regex(@"\d+"), "#");

        await Assert.That(generator.Generate()).IsEqualTo("abc#");
    }

    [Test]
    public async Task StringCapitalize_CapitalizesTheFirstCharacter()
    {
        var forge = NewForge(80);
        var value = forge.Text.Alpha(5).ToLower().Capitalize().Generate();

        await Assert.That(char.IsUpper(value[0])).IsTrue();
    }

    [Test]
    public async Task StringToTitleCase_TitleCasesTheResult()
    {
        var forge = NewForge(81);
        var value = forge.Basic.Literal("hello world").ToTitleCase().Generate();

        await Assert.That(value).IsEqualTo("Hello World");
    }

    [Test]
    public async Task Sentencify_SingleSentence_IsCapitalizedAndPunctuated()
    {
        var forge = NewForge(82);
        var value = forge.Basic.Literal("a b").Sentencify(2, 2).Generate();

        await Assert.That(value).IsEqualTo("A b.");
    }

    [Test]
    public async Task Sentencify_MultiSentence_EverySentenceIsCapitalized()
    {
        var forge = NewForge(83);
        var value = forge.Basic.Literal("a b c d e f g h").Sentencify(2, 2).Generate();

        await Assert.That(value.EndsWith('.')).IsTrue();
        var sentences = value.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        await Assert.That(sentences.Length).IsGreaterThan(1);
        foreach (var sentence in sentences)
        {
            await Assert.That(char.IsUpper(sentence[0])).IsTrue();
        }
    }

    [Test]
    public async Task DateTimeToUtc_SetsUtcKind()
    {
        var forge = NewForge(84);
        var generator = forge.Temporal.Between(
            new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        ).ToUtc();

        var value = generator.Generate();
        await Assert.That(value.Kind).IsEqualTo(DateTimeKind.Utc);
    }

    [Test]
    public async Task DateTimeToLocal_SetsLocalKind()
    {
        var forge = NewForge(85);
        var inner = forge.Temporal.Between(
            new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        );

        var value = inner.ToLocal().Generate();
        await Assert.That(value.Kind).IsEqualTo(DateTimeKind.Local);
    }

    [Test]
    public async Task DateTimeToDateOnly_ExtractsTheDateComponent()
    {
        var forge = NewForge(86);
        var value = forge.Temporal.Between(
            new DateTime(2000, 5, 15, 10, 30, 0, DateTimeKind.Utc),
            new DateTime(2000, 5, 15, 10, 30, 0, DateTimeKind.Utc)
        )
            .ToDateOnly()
            .Generate();

        await Assert.That(value).IsEqualTo(new DateOnly(2000, 5, 15));
    }

    [Test]
    public async Task DateTimeToTimeOnly_ExtractsTheTimeComponent()
    {
        var forge = NewForge(87);
        var value = forge.Temporal.Between(
            new DateTime(2000, 5, 15, 10, 30, 0, DateTimeKind.Utc),
            new DateTime(2000, 5, 15, 10, 30, 0, DateTimeKind.Utc)
        )
            .ToTimeOnly()
            .Generate();

        await Assert.That(value).IsEqualTo(new TimeOnly(10, 30));
    }

    [Test]
    public async Task DateTimeTruncateToDate_ZeroesOutTheTimeComponent()
    {
        var forge = NewForge(88);
        var generator = forge.Temporal.Between(
            new DateTime(2000, 5, 15, 10, 30, 0, DateTimeKind.Utc),
            new DateTime(2000, 5, 15, 10, 30, 0, DateTimeKind.Utc)
        ).TruncateToDate();

        var value = generator.Generate();
        await Assert.That(value).IsEqualTo(new DateTime(2000, 5, 15, 0, 0, 0, DateTimeKind.Utc));
    }

    [Test]
    public async Task List_VariableLength_StaysWithinInclusiveRange()
    {
        var forge = NewForge(89);
        for (var i = 0; i < 50; i++)
        {
            var count = forge.Basic.Literal(1).List(2, 4).Generate().Count;
            await Assert.That(count).IsInRange(2, 4);
        }
    }

    [Test]
    public async Task HashSet_VariableLength_StaysWithinInclusiveRange()
    {
        var forge = NewForge(90);
        for (var i = 0; i < 50; i++)
        {
            var count = forge.Basic.Literal(1).HashSet(1, 3).Generate().Count;
            await Assert.That(count).IsInRange(1, 3);
        }
    }

    [Test]
    public async Task StringRange_StartAtOrBeyondEnd_ReturnsEmpty()
    {
        var forge = NewForge(91);
        var value = forge.Basic.Literal("abcdef").Range(5, 2).Generate();

        await Assert.That(value).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task StringRange_WhenRangeExceedsLength_Throws()
    {
        var forge = NewForge(92);
        var generator = forge.Basic.Literal("abc").Range(0, 10);

        await Assert.That(() => generator.Generate()).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Sentencify_WordBeforeSentenceBreak_KeepsPlainCase()
    {
        var forge = NewForge(93);
        var value = forge.Basic.Literal("a b c").Sentencify(3, 3).Generate();

        await Assert.That(value).IsEqualTo("A b c.");
    }

    [Test]
    public async Task Sentencify_SingleArgumentOverload_ProducesFormattedSentence()
    {
        var forge = NewForge(94);
        var value = forge.Basic.Literal("a b").Sentencify(2).Generate();

        await Assert.That(value).IsEqualTo("A b.");
    }

    [Test]
    public async Task Sentencify_ShortInput_AppendsTrailingPeriod()
    {
        var forge = NewForge(95);
        var value = forge.Basic.Literal("a b").Sentencify(3, 3).Generate();

        await Assert.That(value).IsEqualTo("A b.");
    }

    [Test]
    public async Task CollectionAsList_ConvertsICollectionGeneratorsToLists()
    {
        var forge = NewForge(96);
        var generator = forge.Basic.Literal(new List<int> { 1, 2, 3 })
            .Refine(ICollection<int> (c) => c)
            .AsList();

        var values = generator.Generate();
        await Assert.That(values).Count().IsEqualTo(3);
        await Assert.That(values.Sum()).IsEqualTo(6);
    }

    [Test]
    public async Task CollectionAsHashSet_ConvertsICollectionGeneratorsToHashSets()
    {
        var forge = NewForge(97);
        var generator = forge.Basic.Literal(new List<int> { 1, 2, 2, 3 })
            .Refine(ICollection<int> (c) => c)
            .AsHashSet();

        var values = generator.Generate();
        await Assert.That(values).Count().IsEqualTo(3);
    }
}