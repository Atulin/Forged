using System.Globalization;
using Forged.Core;
using Forged.Core.Generators;

namespace Forged.Tests;

public class ForgeTemporalTests
{
    private static readonly DateTime Jan2000Utc = new(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime Jan2001Utc = new(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
    public async Task Between_ProducesValuesWithinTheSuppliedBounds_WithUtcKind()
    {
        var forge = NewForge(21);
        var values = Draw(forge.Temporal.Between(Jan2000Utc, Jan2001Utc), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(Jan2000Utc).And.IsLessThanOrEqualTo(Jan2001Utc);
            await Assert.That(value.Kind).IsEqualTo(DateTimeKind.Utc);
        }
    }

    [Test]
    public async Task Between_ProducesVaryingValues()
    {
        var forge = NewForge(22);
        var values = Draw(forge.Temporal.Between(Jan2000Utc, Jan2001Utc), 50);

        await Assert.That(values.Distinct().Count()).IsGreaterThan(1);
    }

    [Test]
    public async Task Past_ProducesOnlyPastDates()
    {
        var forge = NewForge(23);
        var generator = forge.Temporal.Past();
        var now = DateTime.UtcNow;

        for (var i = 0; i < 100; i++)
        {
            await Assert.That(generator.Generate()).IsLessThanOrEqualTo(now);
        }
    }

    [Test]
    public async Task Past_WithEarliestBound_StaysWithinRange()
    {
        var forge = NewForge(24);
        var values = Draw(forge.Temporal.Past(Jan2000Utc), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(Jan2000Utc).And.IsLessThanOrEqualTo(DateTime.UtcNow);
        }
    }

    [Test]
    public async Task Future_ProducesOnlyFutureDates()
    {
        var forge = NewForge(25);
        var generator = forge.Temporal.Future();
        var now = DateTime.UtcNow;

        for (var i = 0; i < 100; i++)
        {
            await Assert.That(generator.Generate()).IsGreaterThanOrEqualTo(now);
        }
    }

    [Test]
    public async Task Future_WithLatestBound_StaysWithinRange()
    {
        var forge = NewForge(26);
        var latest = DateTime.UtcNow.AddDays(30);
        var values = Draw(forge.Temporal.Future(latest), 100);

        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(DateTime.UtcNow).And.IsLessThanOrEqualTo(latest);
        }
    }

    [Test]
    public async Task DateBetween_ProducesDatesWithinTheSuppliedBounds()
    {
        var forge = NewForge(27);
        var from = new DateOnly(2020, 1, 1);
        var to = new DateOnly(2020, 12, 31);

        var values = Draw(forge.Temporal.DateBetween(from, to), 100);
        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(from).And.IsLessThanOrEqualTo(to);
        }
    }

    [Test]
    public async Task DateInPast_ProducesDatesOnOrBeforeToday()
    {
        var forge = NewForge(28);
        var generator = forge.Temporal.DateInPast();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        for (var i = 0; i < 100; i++)
        {
            await Assert.That(generator.Generate()).IsLessThanOrEqualTo(today);
        }
    }

    [Test]
    public async Task DateInFuture_ProducesDatesOnOrAfterToday()
    {
        var forge = NewForge(29);
        var generator = forge.Temporal.DateInFuture();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        for (var i = 0; i < 100; i++)
        {
            await Assert.That(generator.Generate()).IsGreaterThanOrEqualTo(today);
        }
    }

    [Test]
    public async Task TimeBetween_ProducesTimesWithinTheSuppliedBounds()
    {
        var forge = NewForge(30);
        var from = new TimeOnly(8, 0);
        var to = new TimeOnly(17, 0);

        var values = Draw(forge.Temporal.TimeBetween(from, to), 100);
        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(from).And.IsLessThanOrEqualTo(to);
        }
    }

    [Test]
    public async Task DateTimeOffsetBetween_ProducesValuesWithinTheSuppliedBounds()
    {
        var forge = NewForge(31);
        var from = new DateTimeOffset(Jan2000Utc);
        var to = new DateTimeOffset(Jan2001Utc);

        var values = Draw(forge.Temporal.DateTimeOffsetBetween(from, to), 100);
        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(from).And.IsLessThanOrEqualTo(to);
        }
    }

    [Test]
    public async Task DateTimeOffsetInPast_ProducesOnlyPastValues()
    {
        var forge = NewForge(32);
        var generator = forge.Temporal.DateTimeOffsetInPast();
        var now = DateTimeOffset.UtcNow;

        for (var i = 0; i < 100; i++)
        {
            await Assert.That(generator.Generate()).IsLessThanOrEqualTo(now);
        }
    }

    [Test]
    public async Task DateTimeOffsetInFuture_ProducesOnlyFutureValues()
    {
        var forge = NewForge(33);
        var generator = forge.Temporal.DateTimeOffsetInFuture();
        var now = DateTimeOffset.UtcNow;

        for (var i = 0; i < 100; i++)
        {
            await Assert.That(generator.Generate()).IsGreaterThanOrEqualTo(now);
        }
    }

    [Test]
    public async Task TimeSpanBetween_ProducesValuesWithinTheSuppliedBounds()
    {
        var forge = NewForge(34);
        var from = new TimeSpan(1, 0, 0);
        var to = new TimeSpan(2, 0, 0);

        var values = Draw(forge.Temporal.TimeSpanBetween(from, to), 100);
        foreach (var value in values)
        {
            await Assert.That(value).IsGreaterThanOrEqualTo(from).And.IsLessThanOrEqualTo(to);
        }
    }
}