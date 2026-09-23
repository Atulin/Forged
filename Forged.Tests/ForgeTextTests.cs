using System.Globalization;
using System.Text.RegularExpressions;
using Forged.Core;
using Forged.Core.Core;
using Forged.Core.Generators;
using Forged.Core.Generators.Text;

namespace Forged.Tests;

public class ForgeTextTests
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

    [Test]
    public async Task Alphanumeric_FixedLength_MatchesLengthAndAlphabet()
    {
        var forge = NewForge(1);
        var values = Draw(forge.Text.Alphanumeric(10), 50);

        foreach (var value in values)
        {
            await Assert.That(value).Length().IsEqualTo(10);
            await Assert.That(value.All(char.IsLetterOrDigit)).IsTrue();
        }
    }

    [Test]
    public async Task Alphanumeric_VariableLength_StaysWithinRange()
    {
        var forge = NewForge(2);
        var values = Draw(forge.Text.Alphanumeric(5, 10), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Length).IsInRange(5, 10);
        }
    }

    [Test]
    public async Task Alpha_FixedLength_ContainsOnlyLetters()
    {
        var forge = NewForge(3);
        var values = Draw(forge.Text.Alpha(8), 50);

        foreach (var value in values)
        {
            await Assert.That(value).Length().IsEqualTo(8);
            await Assert.That(value.All(char.IsLetter)).IsTrue();
        }
    }

    [Test]
    public async Task Alpha_VariableLength_StaysWithinRange()
    {
        var forge = NewForge(4);
        var values = Draw(forge.Text.Alpha(3, 7), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Length).IsInRange(3, 7);
        }
    }

    [Test]
    public async Task Pronounceable_ProducesNonEmptyLetterOnlyStrings()
    {
        var forge = NewForge(5);
        var values = Draw(forge.Text.Pronounceable(3), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(value.All(char.IsLetter)).IsTrue();
        }
    }

    [Test]
    public async Task Lorem_FixedLength_ProducesExactlyThatManyWords()
    {
        var forge = NewForge(6);
        var value = forge.Text.Lorem(5).Generate();

        await Assert.That(value.Split(' ')).Count().IsEqualTo(5);
    }

    [Test]
    public async Task Lorem_VariableLength_StaysWithinRange()
    {
        var forge = NewForge(7);
        var values = Draw(forge.Text.Lorem(3, 7), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Split(' ').Length).IsInRange(3, 7);
        }
    }

    [Test]
    public async Task Lorem_WithStarter_StartsWithTheClassicOpenings()
    {
        var forge = NewForge(8);
        var value = forge.Text.Lorem(5, new LoremIpsumGenerator.Options(Starter: 3)).Generate();

        await Assert.That(value.StartsWith("lorem ipsum dolor ")).IsTrue();
        await Assert.That(value.Split(' ')).Count().IsEqualTo(8);
    }

    [Test]
    public async Task Waffle_ProducesExactlyTheRequestedNumberOfSentences()
    {
        var forge = NewForge(9);
        var value = forge.Text.Waffle(5, WaffleStyle.Technical).Generate();

        await Assert.That(value.Split('.').Length - 1).IsEqualTo(5);
        await Assert.That(value.EndsWith('.')).IsTrue();
    }

    [Test]
    public async Task Waffle_FictionStyle_AlsoProducesSentences()
    {
        var forge = NewForge(10);
        var value = forge.Text.Waffle(3, WaffleStyle.Fiction).Generate();

        await Assert.That(value.Split('.').Length - 1).IsEqualTo(3);
    }

    [Test]
    public async Task Waffle_ZeroSentences_ProducesEmptyString()
    {
        var forge = NewForge(11);
        await Assert.That(forge.Text.Waffle(0).Generate()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Hex_FixedLength_ContainsOnlyHexDigits()
    {
        var forge = NewForge(12);
        var value = forge.Text.Hex(16).Generate();

        await Assert.That(value).Length().IsEqualTo(16);
        await Assert.That(value.All(c => c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F')).IsTrue();
    }

    [Test]
    public async Task Hex_VariableLength_StaysWithinRange()
    {
        var forge = NewForge(13);
        for (var i = 0; i < 50; i++)
        {
            await Assert.That(forge.Text.Hex(4, 12).Generate().Length).IsInRange(4, 12);
        }
    }

    [Test]
    public async Task Guid_V4_ProducesUniqueValidGuidValues()
    {
        var forge = NewForge(14);
        var values = Draw(forge.Text.Guid(GuidGenerator.Kind.V4), 20);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEqualTo(Guid.Empty);
        }

        await Assert.That(values.Distinct().Count()).IsEqualTo(20);
    }

    [Test]
    public async Task Guid_V7_ProducesVersionSevenGuids()
    {
        var forge = NewForge(15);
        var value = forge.Text.Guid(GuidGenerator.Kind.V7).Generate();

        await Assert.That(value.ToString("N")[12]).IsEqualTo('7');
    }

    [Test]
    public async Task Template_ReproducesThePlaceholderPattern()
    {
        var forge = NewForge(16);
        var value = forge.Text.Template("###-???-***").Generate();

        await Assert.That(Regex.IsMatch(value, @"^\d{3}-[A-Za-z]{3}-[A-Za-z0-9]{3}$")).IsTrue();
    }

    [Test]
    public async Task Template_EscapedPlaceholder_IsRenderedAsLiteral()
    {
        var forge = NewForge(17);
        var value = forge.Text.Template("\\###").Generate();

        await Assert.That(Regex.IsMatch(value, @"^#\d{2}$")).IsTrue();
    }

    [Test]
    public async Task Template_WithoutPlaceholders_ReturnsTheTemplateAsIs()
    {
        var forge = NewForge(18);
        await Assert.That(forge.Text.Template("static-text").Generate()).IsEqualTo("static-text");
    }

    [Test]
    public async Task Char_AsciiKind_ProducesOnlyAsciiCharacters()
    {
        var forge = NewForge(19);
        var values = Draw(forge.Text.Char(CharKind.Ascii), 200);

        foreach (var value in values)
        {
            await Assert.That((int)value).IsInRange(0, 127);
        }
    }

    [Test]
    public async Task Char_PrintableKind_ProducesOnlyPrintableCharacters()
    {
        var forge = NewForge(20);
        var values = Draw(forge.Text.Char(CharKind.Printable), 200);

        foreach (var value in values)
        {
            await Assert.That((int)value).IsInRange(32, 126);
        }
    }

    [Test]
    public async Task Char_AlphanumericKind_ProducesOnlyLettersAndDigits()
    {
        var forge = NewForge(21);
        var values = Draw(forge.Text.Char(CharKind.Alphanumeric), 200);

        foreach (var value in values)
        {
            await Assert.That(char.IsLetterOrDigit(value)).IsTrue();
        }
    }

    [Test]
    public async Task Char_AlphabeticalKind_ProducesOnlyLetters()
    {
        var forge = NewForge(22);
        var values = Draw(forge.Text.Char(CharKind.Alphabetical), 200);

        foreach (var value in values)
        {
            await Assert.That(char.IsLetter(value)).IsTrue();
        }
    }

    [Test]
    public async Task Char_NumericKind_ProducesOnlyDigits()
    {
        var forge = NewForge(23);
        var values = Draw(forge.Text.Char(CharKind.Numeric), 200);

        foreach (var value in values)
        {
            await Assert.That(char.IsDigit(value)).IsTrue();
        }
    }

    [Test]
    public async Task Chars_ExactCount_ReturnsThatManyCharactersFromThePool()
    {
        var forge = NewForge(24);
        var values = Draw(forge.Text.Chars(8, CharKind.Alphanumeric), 30);

        foreach (var value in values)
        {
            await Assert.That(value).Count().IsEqualTo(8);
            await Assert.That(value.All(char.IsLetterOrDigit)).IsTrue();
        }
    }

    [Test]
    public async Task Chars_ZeroCount_ReturnsEmptyArray()
    {
        var forge = NewForge(25);
        await Assert.That(forge.Text.Chars(0, 0, CharKind.Alphanumeric).Generate()).Count().IsEqualTo(0);
    }

    [Test]
    public async Task Chars_VariableCount_StaysWithinCountBounds()
    {
        var forge = NewForge(26);
        for (var i = 0; i < 30; i++)
        {
            var value = forge.Text.Chars(2, 5, CharKind.Alphanumeric).Generate();
            await Assert.That(value.Length).IsInRange(2, 5);
            await Assert.That(value.All(char.IsLetterOrDigit)).IsTrue();
        }
    }

    [Test]
    public async Task Emoji_ProducesNonEmptyValues_FromTheLocaleData()
    {
        var forge = NewForge(27);
        var values = Draw(forge.Text.Emoji(), 40);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
        }

        await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
    }

    [Test]
    public async Task Emoji_IsDeterministic_ForTheSameSeed()
    {
        var forge1 = NewForge(28);
        var forge2 = NewForge(28);

        var expected = forge1.Text.Emoji().Generate();
        var actual = forge2.Text.Emoji().Generate();

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task EmojiRunes_RuneArrayMatchesEnumeratedRunesOfTheString()
    {
        var forge = NewForge(29);
        var values = Draw(forge.Text.EmojiRunes(), 20);

        foreach (var value in values)
        {
            var text = string.Concat(value.Select(r => r.ToString()));
            var runeCount = 0;
            foreach (var _ in text.EnumerateRunes())
            {
                runeCount++;
            }
            await Assert.That(value).Count().IsEqualTo(runeCount);
            await Assert.That(text).IsNotEmpty();
        }
    }
}