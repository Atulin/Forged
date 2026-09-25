using System.Globalization;
using Forged.Core;
using Forged.Core.Generators.Internet;

namespace Forged.Tests;

public class EndToEndTests
{
    private static Forge NewForge(int seed)
        => new(new Random(seed), CultureInfo.InvariantCulture);

    [Test]
    public async Task Faker_FillsEveryPropertyOnTheModel()
    {
        var forge = NewForge(90);
        var faker = new PersonFaker
        {
            Id = _ => forge.Text.Guid(),
            FirstName = _ => forge.Basic.Literal("Alice"),
            LastName = _ => forge.Basic.Literal("Smith"),
            Age = _ => forge.Random.Number<int>(30, 40),
            Email = _ => forge.Internet.Email(EmailKind.Example),
        };

        var person = faker.Get();

        await Assert.That(person.Id).IsNotEqualTo(Guid.Empty);
        await Assert.That(person.FirstName).IsEqualTo("Alice");
        await Assert.That(person.LastName).IsEqualTo("Smith");
        await Assert.That(person.Age).IsInRange(30, 40);
        await Assert.That(person.Email).IsNotNull();
    }

    [Test]
    public async Task Faker_GetWithCount_ReturnsExactlyThatManyModels()
    {
        var forge = NewForge(91);
        var faker = new PersonFaker
        {
            Id = _ => forge.Text.Guid(),
            FirstName = _ => forge.Basic.Literal("A"),
            LastName = _ => forge.Basic.Literal("B"),
        };

        var people = faker.Get(5).ToList();

        await Assert.That(people).Count().IsEqualTo(5);
        foreach (var person in people)
        {
            await Assert.That(person.FirstName).IsEqualTo("A");
        }
    }

    [Test]
    public async Task Faker_GetWithinRange_StaysWithinTheInclusiveRange()
    {
        var forge = NewForge(92);
        var faker = new PersonFaker
        {
            Id = _ => forge.Text.Guid(),
            FirstName = _ => forge.Basic.Literal("A"),
            LastName = _ => forge.Basic.Literal("B"),
        };

        for (var i = 0; i < 20; i++)
        {
            var count = faker.Get(3, 6).Count();
            await Assert.That(count).IsInRange(3, 6);
        }
    }

    [Test]
    public async Task Faker_IsDeterministic_ForTheSameSeed()
    {
        var forge1 = new Forge(new Random(77), CultureInfo.InvariantCulture);
        var faker1 = new PersonFaker
        {
            Id = _ => forge1.Basic.Literal(Guid.Empty),
            FirstName = _ => forge1.Text.Alpha(4),
            LastName = _ => forge1.Text.Alpha(5),
        };

        var forge2 = new Forge(new Random(77), CultureInfo.InvariantCulture);
        var faker2 = new PersonFaker
        {
            Id = _ => forge1.Basic.Literal(Guid.Empty),
            FirstName = _ => forge2.Text.Alpha(4),
            LastName = _ => forge2.Text.Alpha(5),
        };

        var first = faker1.Get();
        var second = faker2.Get();

        await Assert.That(second.FirstName).IsEqualTo(first.FirstName);
        await Assert.That(second.LastName).IsEqualTo(first.LastName);
    }

    [Test]
    public async Task MemoizedProperties_StayConsistentWithinOneModel()
    {
        var forge = NewForge(93);
        var firstName = forge.Text.Alpha(6);
        var firstNameMemo = firstName.Memo(out var memoizedFirst);

        var faker = new PersonFaker
        {
            Id = _ => forge.Text.Guid(),
            FirstName = _ => firstNameMemo,
            LastName = _ => forge.Basic.Func(() => memoizedFirst.Generate() + "-smith"),
        };

        var person = faker.Get();

        await Assert.That(person.LastName.StartsWith(person.FirstName)).IsTrue();
        await Assert.That(person.LastName).IsEqualTo(person.FirstName + "-smith");
    }

    [Test]
    public async Task GeneratedReferences_ResolveOncePerGet()
    {
        var forge = NewForge(95);
        var calls = 0;
        var faker = new PersonFaker
        {
            Id = _ => forge.Text.Guid(),
            FirstName = _ => forge.Basic.Literal("Alice"),
            LastName = f => f.Basic.Func(() => $"{f.Generated.FirstName}-{++calls}"),
        };

        var first = faker.Get();
        var second = faker.Get();

        await Assert.That(first.LastName).IsEqualTo("Alice-1");
        await Assert.That(second.LastName).IsEqualTo("Alice-2");
    }

    [Test]
    public async Task LocaleFallback_UnknownLocale_UsesTheEnglishLocaleData()
    {
        var forge = new Forge(new Random(94), new CultureInfo("de-DE"));
        var value = forge.Text.Pronounceable(8).Generate();

        await Assert.That(value).IsNotEmpty();
        await Assert.That(value.All(char.IsLetter)).IsTrue();
    }
}