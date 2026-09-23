using System.Globalization;
using Forged.Core;
using Forged.Core.Core;
using Forged.Core.Generators;
using Forged.Core.Modules;

namespace Forged.Tests;

public class ForgePersonTests
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

    private static HashSet<string> LoadCorpus(string file)
        => new(new FileLoader().LoadData("en", $"person/name/{file}", CommonContext.Default.ListString));

    private static readonly HashSet<string> MaleFirstNames = LoadCorpus("male_first");
    private static readonly HashSet<string> FemaleFirstNames = LoadCorpus("female_first");
    private static readonly HashSet<string> Prefixes = LoadCorpus("prefix");
    private static readonly HashSet<string> Infixes = LoadCorpus("infix");
    private static readonly HashSet<string> Suffixes = LoadCorpus("suffix");

    [Test]
    public async Task FirstName_Default_ProducesNamesFromTheCorpus()
    {
        var person = new ForgePerson(NewForge(1));
        var values = Draw(person.FirstName(), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(MaleFirstNames.Contains(value) || FemaleFirstNames.Contains(value)).IsTrue();
        }
    }

    [Test]
    public async Task FirstName_MaleOnly_ProducesOnlyMaleNames()
    {
        var person = new ForgePerson(NewForge(2));
        var values = Draw(person.FirstName(male: 1f, female: 0f), 100);

        foreach (var value in values)
        {
            await Assert.That(MaleFirstNames.Contains(value)).IsTrue();
        }
    }

    [Test]
    public async Task FirstName_FemaleOnly_ProducesOnlyFemaleNames()
    {
        var person = new ForgePerson(NewForge(3));
        var values = Draw(person.FirstName(male: 0f, female: 1f), 100);

        foreach (var value in values)
        {
            await Assert.That(FemaleFirstNames.Contains(value)).IsTrue();
        }
    }

    [Test]
    public async Task LastName_Single_ContainsNoSeparator()
    {
        var person = new ForgePerson(NewForge(4));
        var values = Draw(person.LastName(hyphenated: 0f, compound: 0f), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(value.Contains('-')).IsFalse();
            await Assert.That(value.Contains(' ')).IsFalse();
        }
    }

    [Test]
    public async Task LastName_HyphenatedOnly_EveryValueContainsHyphen()
    {
        var person = new ForgePerson(NewForge(5));
        var values = Draw(person.LastName(hyphenated: 1f, compound: 0f), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Contains('-')).IsTrue();
        }
    }

    [Test]
    public async Task LastName_CompoundOnly_EveryValueContainsSpace()
    {
        var person = new ForgePerson(NewForge(6));
        var values = Draw(person.LastName(hyphenated: 0f, compound: 1f), 50);

        foreach (var value in values)
        {
            await Assert.That(value.Contains(' ')).IsTrue();
        }
    }

    [Test]
    public async Task NamePrefix_ProducesValuesFromTheCorpus()
    {
        var person = new ForgePerson(NewForge(7));
        var values = Draw(person.NamePrefix(), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(Prefixes.Contains(value)).IsTrue();
        }
    }

    [Test]
    public async Task NameInfix_ProducesValuesFromTheCorpus_WithoutTheGlueMarker()
    {
        var person = new ForgePerson(NewForge(8));
        var values = Draw(person.NameInfix(), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(Infixes.Contains(value) || Infixes.Contains(value + '*')).IsTrue();
            await Assert.That(value.Contains('*')).IsFalse();
        }
    }

    [Test]
    public async Task NameSuffix_ProducesValuesFromTheCorpus()
    {
        var person = new ForgePerson(NewForge(9));
        var values = Draw(person.NameSuffix(), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(Suffixes.Contains(value)).IsTrue();
        }
    }

    [Test]
    public async Task FullName_ForcedPrefixAndSuffix_BeginsWithPrefixAndEndsWithSuffix()
    {
        var person = new ForgePerson(NewForge(10));
        var values = Draw(person.FullName(
            middle: 0f,
            hyphenated: 0f,
            compound: 0f,
            prefix: 1f,
            infix: 0f,
            suffix: 1f
        ), 50);

        foreach (var value in values)
        {
            var tokens = value.Split(' ');
            await Assert.That(tokens).Count().IsEqualTo(4);
            await Assert.That(Prefixes.Contains(tokens[0])).IsTrue();
            await Assert.That(Suffixes.Contains(tokens[^1])).IsTrue();
        }
    }

    [Test]
    public async Task FullName_ForcedMiddleName_MiddleNameIsAFirstName()
    {
        var person = new ForgePerson(NewForge(11));
        var values = Draw(person.FullName(
            middle: 1f,
            hyphenated: 0f,
            compound: 0f,
            prefix: 0f,
            infix: 0f,
            suffix: 0f
        ), 50);

        foreach (var value in values)
        {
            var tokens = value.Split(' ');
            await Assert.That(tokens).Count().IsEqualTo(3);
            await Assert.That(MaleFirstNames.Contains(tokens[0]) || FemaleFirstNames.Contains(tokens[0])).IsTrue();
            await Assert.That(MaleFirstNames.Contains(tokens[1]) || FemaleFirstNames.Contains(tokens[1])).IsTrue();
        }
    }

    [Test]
    public async Task FullName_ForcedInfix_NeverLeavesTheAsteriskMarkerInTheOutput()
    {
        var person = new ForgePerson(NewForge(12));
        var values = Draw(person.FullName(infix: 1f), 200);

        foreach (var value in values)
        {
            await Assert.That(value.Contains('*')).IsFalse();
        }
    }

    [Test]
    public async Task FullName_Default_ProducesNonEmptyNamesWithoutMarkers()
    {
        var person = new ForgePerson(NewForge(13));
        var values = Draw(person.FullName(), 50);

        foreach (var value in values)
        {
            await Assert.That(value).IsNotEmpty();
            await Assert.That(value.Contains('*')).IsFalse();
        }
    }

    [Test]
    public async Task FullName_IsDeterministic_ForTheSameSeed()
    {
        var expected = new ForgePerson(NewForge(14)).FullName().Generate();
        var actual = new ForgePerson(NewForge(14)).FullName().Generate();

        await Assert.That(actual).IsEqualTo(expected);
    }
}