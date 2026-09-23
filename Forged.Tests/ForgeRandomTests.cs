using System.Globalization;
using Forged.Core;
using Forged.Core.Generators;
using Forged.Core.Generators.Random;
using Forged.Core.Generators.Random.DiceGeneratorHelpers;

namespace Forged.Tests;

public enum Color
{
    Red,
    Green,
    Blue,
}

public class ForgeRandomTests
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
    public async Task CoinToss_ProducesBothOutcomes_OverManyDraws()
    {
        var forge = NewForge(5678);
        var values = Draw(forge.Random.CoinToss(), 200);

        await Assert.That(values.Count(v => v)).IsGreaterThan(0);
        await Assert.That(values.Count(v => !v)).IsGreaterThan(0);
    }

    [Test]
    public async Task Pick_ReturnsItemsOnlyFromTheSuppliedCollection()
    {
        var forge = NewForge(99);
        var values = Draw(forge.Random.Pick(1, 2, 3), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(1, 3);
        }
    }

    [Test]
    public async Task Pick_PicksFromMultipleItems_OverManyDraws()
    {
        var forge = NewForge(771);
        var distinct = Draw(forge.Random.Pick("a", "b", "c"), 100).Distinct().ToList();

        await Assert.That(distinct.Count).IsGreaterThan(1);
    }

    [Test]
    public async Task Pick_WithExactCount_ReturnsThatManyItems()
    {
        var forge = NewForge(440);
        var values = forge.Random.Pick([1, 2, 3, 4, 5], 5).Generate();

        await Assert.That(values).Count().IsEqualTo(5);
        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(1, 5);
        }
    }

    [Test]
    public async Task Pick_WithMinAndMaxCount_StaysWithinRange()
    {
        var forge = NewForge(3);
        for (var i = 0; i < 30; i++)
        {
            var values = forge.Random.Pick(["x", "y", "z"], 2, 4).Generate();
            await Assert.That(values.Length).IsInRange(2, 4);
        }
    }

    [Test]
    public async Task Pick_ForEnum_ReturnsAValidEnumValue()
    {
        var forge = NewForge(81);
        var values = Draw(forge.Random.Pick<Color>(), 50);

        foreach (var value in values)
        {
            await Assert.That(Enum.IsDefined(value)).IsTrue();
        }
    }

    [Test]
    public async Task Number_IntegerType_ProducesValuesWithinTheRange_AndVaries()
    {
        var forge = NewForge(4242);
        var values = Draw(forge.Random.Number<int>(3, 6), 200);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(3, 6);
        }

        await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
    }

    [Test]
    public async Task Number_IntegerType_WithExplicitBounds_IncludesTheMinimum()
    {
        var forge = NewForge(5);
        var values = Draw(forge.Random.Number<int>(10, 10), 10);

        foreach (var value in values)
        {
            await Assert.That(value).IsEqualTo(10);
        }
    }

    [Test]
    public async Task Number_FloatingPointType_ProducesValuesWithinTheRange()
    {
        var forge = NewForge(2024);
        var values = Draw(forge.Random.Number<double>(0, 1), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(0, 1);
        }

        await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
    }

    [Test]
    public async Task Number_LongType_ProducesValuesWithinTheRange()
    {
        var forge = NewForge(808);
        var values = Draw(forge.Random.Number<long>(100_000, 200_000), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(100_000, 200_000);
        }
    }

    [Test]
    public async Task Number_WithNullBounds_UsesTheFullTypeRange()
    {
        var forge = NewForge(12);
        var values = Draw(forge.Random.Number<byte>(), 200);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(byte.MinValue, byte.MaxValue);
        }
    }

    [Test]
    public async Task Number_IsDeterministic_ForTheSameSeed()
    {
        var forge1 = NewForge(7);
        var forge2 = NewForge(7);

        var expected = forge1.Random.Number<int>(1, 100).Generate();
        var actual = forge2.Random.Number<int>(1, 100).Generate();

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Number_ExtremeUnsignedBounds_DoNotThrow()
    {
        var forge = NewForge(42);
        var values = Draw(forge.Random.Number<ulong>(ulong.MinValue, ulong.MaxValue), 1000);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(ulong.MinValue, ulong.MaxValue);
        }
    }

    [Test]
    public async Task Number_ExtremeUnsignedNearMaxBounds_DoNotThrow()
    {
        var forge = NewForge(42);
        var values = Draw(forge.Random.Number<ulong>(ulong.MaxValue - 1000, ulong.MaxValue), 1000);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(ulong.MaxValue - 1000, ulong.MaxValue);
        }
    }

    [Test]
    public async Task Number_ExtremeSignedBounds_DoNotThrow()
    {
        var forge = NewForge(42);
        var values = Draw(forge.Random.Number<long>(long.MinValue, long.MaxValue), 1000);

        foreach (var value in values)
        {
            await Assert.That(value).IsInRange(long.MinValue, long.MaxValue);
        }
    }

    [Test]
    public async Task WeightedPick_HonorsZeroWeights()
    {
        var forge = NewForge(97);
        var generator = forge.Random.WeightedPick([1, 2], [1f, 0f]);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo(1);
        }
    }

    [Test]
    public async Task WeightedPick_TupleOverload_HonorsWeights()
    {
        var forge = NewForge(97);
        var generator = forge.Random.WeightedPick([(10, 1f), (20, 0f)]);

        for (var i = 0; i < 50; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo(10);
        }
    }

    [Test]
    public async Task WeightedPick_RejectsMismatchedLengths()
    {
        var forge = NewForge(1);
        var generator = forge.Random.WeightedPick([1, 2], [1f]);

        await Assert.That(generator.Generate).Throws<ArgumentException>();
    }

    [Test]
    public async Task WeightedPick_WithNaNWeights_FallsBackToTheLastItem()
    {
        var forge = NewForge(1);
        var generator = forge.Random.WeightedPick([1, 2], [float.NaN, 1f]);

        for (var i = 0; i < 20; i++)
        {
            await Assert.That(generator.Generate()).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Dice_SingleDie_StaysWithinDieBounds()
    {
        var forge = NewForge(33);
        for (var i = 0; i < 100; i++)
        {
            await Assert.That(forge.Random.Dice("1d6").Generate()).IsInRange(1, 6);
        }
    }

    [Test]
    public async Task Dice_MultiDieWithModifier_StaysWithinExpectedBounds()
    {
        var forge = NewForge(22);
        for (var i = 0; i < 200; i++)
        {
            await Assert.That(forge.Random.Dice("2d6+3").Generate()).IsInRange(5, 15);
        }
    }

    [Test]
    public async Task Dice_ParenthesizedExpression_IsEvaluatedCorrectly()
    {
        var forge = NewForge(55);
        for (var i = 0; i < 100; i++)
        {
            await Assert.That(forge.Random.Dice("(1d6+1d6)*2").Generate()).IsInRange(4, 24);
        }
    }

    [Test]
    public async Task Dice_BareDie_DefaultsToOneDie()
    {
        var forge = NewForge(66);
        for (var i = 0; i < 100; i++)
        {
            await Assert.That(forge.Random.Dice("d20").Generate()).IsInRange(1, 20);
        }
    }

    [Test]
    public async Task Dice_NegativeModifier_IsApplied()
    {
        var forge = NewForge(77);
        for (var i = 0; i < 100; i++)
        {
            await Assert.That(forge.Random.Dice("1d6-1").Generate()).IsInRange(0, 5);
        }
    }

    [Test]
    public async Task Dice_InvalidExpression_Throws()
    {
        var forge = NewForge(88);
        await Assert.That(() => forge.Random.Dice("0d0").Generate()).Throws<InvalidOperationException>();
    }

[Test]
    public async Task Dice_InvalidRoundingMode_Throws()
    {
        var forge = NewForge(89);
        var generator = new DiceGenerator("1d6", (RoundingMode)int.MaxValue, forge);

        await Assert.That(generator.Generate).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DiceAlgebra_UnknownBinaryOperator_Throws()
    {
        var generator = new BinaryNode(new NumberNode(1), TokenType.End, new NumberNode(2));

        await Assert.That(() => generator.Evaluate(new Random(1))).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Dice_FloorMode_AlwaysFloorsTheResult()
    {
        var forge = NewForge(33);
        for (var i = 0; i < 20; i++)
        {
            await Assert.That(forge.Random.Dice("10/4", RoundingMode.Floor).Generate()).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Dice_CeilingMode_AlwaysCeilsTheResult()
    {
        var forge = NewForge(33);
        for (var i = 0; i < 20; i++)
        {
            await Assert.That(forge.Random.Dice("10/4", RoundingMode.Ceiling).Generate()).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Dice_UnaryMinus_ProducesNonPositiveResults()
    {
        var forge = NewForge(44);
        for (var i = 0; i < 100; i++)
        {
            await Assert.That(forge.Random.Dice("-d6").Generate()).IsInRange(-6, -1);
        }
    }

    [Test]
    public async Task Dice_MissingOperand_Throws()
    {
        var forge = NewForge(55);
        await Assert.That(() => forge.Random.Dice("2+").Generate()).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Dice_UnexpectedCharacterInExpression_Throws()
    {
        var forge = NewForge(66);
        await Assert.That(() => forge.Random.Dice("2d6 3").Generate()).Throws<InvalidOperationException>();
    }
}