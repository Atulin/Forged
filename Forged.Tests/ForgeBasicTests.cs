using System.Globalization;
using Forged.Core;
using Forged.Core.Extensions;

namespace Forged.Tests;

public class ForgeBasicTests
{
    private static Forge NewForge(int seed = 1234)
        => new(new Random(seed), CultureInfo.InvariantCulture);

    [Test]
    public async Task Literal_AlwaysReturnsTheGivenValue()
    {
        var forge = NewForge();
        var generator = forge.Basic.Literal(42);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo(42);
        }
    }

    [Test]
    public async Task Literal_SupportsReferenceTypes()
    {
        var forge = NewForge();
        var generator = forge.Basic.Literal("hello");

        await Assert.That(generator.Generate()).IsEqualTo("hello");
    }

    [Test]
    public async Task Generator_ImplicitlyConvertsToItsGeneratedValue()
    {
        var forge = NewForge();
        int value = forge.Basic.Literal(7);

        await Assert.That(value).IsEqualTo(7);
    }

    [Test]
    public async Task Func_InvokesTheSuppliedFunctionOnEveryGenerate()
    {
        var forge = NewForge();
        var invocationCount = 0;
        var generator = forge.Basic.Func(() =>
        {
            invocationCount++;
            return "called";
        });

        await Assert.That(generator.Generate()).IsEqualTo("called");
        await Assert.That(generator.Generate()).IsEqualTo("called");
        await Assert.That(invocationCount).IsEqualTo(2);
    }

    [Test]
    public async Task Generator_ToString_ReturnsStringOfTheGeneratedValue()
    {
        var forge = NewForge();
        var generator = forge.Basic.Literal(42);

        await Assert.That(generator.ToString()).IsEqualTo("42");
    }

    [Test]
    public async Task ToString_ProducesStringValueFromGenerator()
    {
        var forge = NewForge();
        var generator = GeneratorExtensions.ToString(forge.Basic.Literal(123));

        await Assert.That(generator.Generate()).IsEqualTo("123");
    }

    [Test]
    public async Task ToString_WithFormat_UsesTheSuppliedFormat()
    {
        var forge = NewForge();
        var generator = forge.Random.Number<double>(3.14159, 3.14159).ToString("0.00");

        await Assert.That(generator.Generate()).IsEqualTo("3.14");
    }
}