using System.Globalization;
using System.Text.RegularExpressions;
using Forged.Core;
using Forged.Core.Generators;
using Forged.Core.Generators.Internet;

namespace Forged.Tests;

public class ForgeInternetTests
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
    public async Task Username_WithoutPrefixSuffixOrLeet_IsAPureAlphabeticCore()
    {
        var forge = NewForge(41);
        var values = Draw(forge.Internet.Username(0f, 0f, 0f), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(value.All(char.IsLetter)).IsTrue();
        }
    }

    [Test]
    public async Task Username_WithPrefixAndSuffix_IncludesBoth()
    {
        var forge = NewForge(42);
        var value = forge.Internet.Username(1f, 1f, 0f).Generate();

        await Assert.That(value).IsNotEmpty();
        await Assert.That(value.All(char.IsLetterOrDigit)).IsTrue();
    }

    [Test]
    public async Task Username_DefaultOptions_ProduceValidUsernames()
    {
        var forge = NewForge(43);
        var values = Draw(forge.Internet.Username(), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
        }
    }

    [Test]
    public async Task Domain_WithoutCcSld_HasNoSecondLevelPrefix()
    {
        var forge = NewForge(44);
        var values = Draw(forge.Internet.Domain(0f), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Contains('.')).IsFalse();
            await Assert.That(value.All(char.IsLower)).IsTrue();
        }
    }

    [Test]
    public async Task Domain_WithCcSldChanceOne_AlwaysIncludesASecondLevelPrefix()
    {
        var forge = NewForge(45);
        var values = Draw(forge.Internet.Domain(1f), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Split('.')).Count().IsEqualTo(2);
            await Assert.That(Regex.IsMatch(value, @"^[a-z]+\.[a-z]+$")).IsTrue();
        }
    }

    [Test]
    public async Task Email_ExampleKind_UsesExampleDomains()
    {
        var forge = NewForge(46);
        var values = Draw(forge.Internet.Email(EmailKind.Example), 30);

        foreach (var value in values)
        {
            await Assert.That(Regex.IsMatch(value, @"^.+@example\.(com|org|net)$")).IsTrue();
        }
    }

    [Test]
    public async Task Email_KnownKind_UsesKnownProviders()
    {
        var forge = NewForge(47);
        var values = Draw(forge.Internet.Email(EmailKind.Known), 30);

        foreach (var value in values)
        {
            await Assert.That(Regex.IsMatch(value, "^[^@]+@[^@]+$")).IsTrue();
        }
    }

    [Test]
    public async Task Email_RandomKind_HasLocalAndDottedDomain()
    {
        var forge = NewForge(48);
        var values = Draw(forge.Internet.Email(EmailKind.Random), 30);

        foreach (var value in values)
        {
            var parts = value.Split('@');
            await Assert.That(parts).Count().IsEqualTo(2);
            await Assert.That(parts[0]).IsNotEmpty();
            await Assert.That(parts[1].Contains('.')).IsTrue();
        }
    }

    [Test]
    public async Task Email_CustomProvider_IsUsedAsTheLocalPart()
    {
        var forge = NewForge(49);
        var value = forge.Internet.Email(EmailKind.Example, forge.Basic.Literal("bob")).Generate();

        await Assert.That(Regex.IsMatch(value, @"^bob@example\.(com|org|net)$")).IsTrue();
    }

    [Test]
    public async Task Email_InvalidKind_Throws()
    {
        var forge = NewForge(50);
        var generator = new EmailGenerator((EmailKind)int.MaxValue, null, forge);

        await Assert.That(generator.Generate).Throws<ArgumentOutOfRangeException>();
    }
}