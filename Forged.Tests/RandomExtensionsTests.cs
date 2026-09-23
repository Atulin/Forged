using System.Collections.Immutable;
using Forged.Core.Core;

namespace Forged.Tests;

public class RandomExtensionsTests
{
	private static Random NewRng(int seed) => new(seed);

	[Test]
	public async Task Chance_NonPositive_AlwaysReturnsFalse()
	{
		var rng = NewRng(1);
		for (var i = 0; i < 50; i++)
		{
			await Assert.That(rng.Chance(0f)).IsFalse();
			await Assert.That(rng.Chance(-1f)).IsFalse();
			await Assert.That(rng.Chance(float.NegativeInfinity)).IsFalse();
			await Assert.That(rng.Chance(float.NaN)).IsFalse();
		}
	}

	[Test]
	public async Task Chance_AboveOne_AlwaysReturnsTrue()
	{
		var rng = NewRng(2);
		for (var i = 0; i < 50; i++)
		{
			await Assert.That(rng.Chance(1.5f)).IsTrue();
			await Assert.That(rng.Chance(2f)).IsTrue();
			await Assert.That(rng.Chance(float.PositiveInfinity)).IsTrue();
		}
	}

	[Test]
	public async Task Chance_OfOne_AlwaysReturnsTrue()
	{
		var rng = NewRng(3);
		for (var i = 0; i < 100; i++)
		{
			await Assert.That(rng.Chance(1f)).IsTrue();
		}
	}

	[Test]
	public async Task Chance_MidRange_ProducesBothOutcomesOverManyDraws()
	{
		var rng = NewRng(4);
		var values = new List<bool>(1000);
		for (var i = 0; i < 1000; i++)
		{
			values.Add(rng.Chance(0.65f));
		}

		await Assert.That(values.Count(v => v)).IsGreaterThan(0);
		await Assert.That(values.Count(v => !v)).IsGreaterThan(0);
	}

	[Test]
	public async Task Chance_IsDeterministic_ForTheSameSeed()
	{
		var expected = new List<bool>(50);
		var actual = new List<bool>(50);
		for (var i = 0; i < 50; i++)
		{
			expected.Add(NewRng(5).Chance(0.5f));
			actual.Add(NewRng(5).Chance(0.5f));
		}

		await Assert.That(actual.SequenceEqual(expected)).IsTrue();
	}

	[Test]
	public async Task GetItem_FromParamsArray_ProducesAllSuppliedItems()
	{
		var rng = NewRng(10);
		var values = new List<string>(200);
		for (var i = 0; i < 200; i++)
		{
			values.Add(rng.GetItem("a", "b", "c"));
		}

		await Assert.That(values.All(v => v is "a" or "b" or "c")).IsTrue();
		await Assert.That(values.Distinct().Count()).IsEqualTo(3);
	}

	[Test]
	public async Task GetItem_FromList_ProducesOnlySuppliedItems()
	{
		var rng = NewRng(11);
		var items = new List<int> { 1, 2, 3, 4 };
		var values = new List<int>(200);
		for (var i = 0; i < 200; i++)
		{
			values.Add(rng.GetItem(items));
		}

		await Assert.That(values.All(items.Contains)).IsTrue();
		await Assert.That(values.Distinct().Count()).IsEqualTo(4);
	}

	[Test]
	public async Task GetItem_FromImmutableArray_ProducesOnlySuppliedItems()
	{
		var rng = NewRng(12);
		var items = ImmutableArray.Create("x", "y", "z");
		var values = new List<string>(200);
		for (var i = 0; i < 200; i++)
		{
			values.Add(rng.GetItem(items));
		}

		await Assert.That(values.All(items.Contains)).IsTrue();
		await Assert.That(values.Distinct().Count()).IsEqualTo(3);
	}

	[Test]
	public async Task GetItem_IsDeterministic_ForTheSameSeed()
	{
		var expected = NewRng(13).GetItem(10, 20, 30);
		var actual = NewRng(13).GetItem(10, 20, 30);

		await Assert.That(actual).IsEqualTo(expected);
	}

	[Test]
	public async Task GetShuffled_IsAPermutationOfTheInput()
	{
		var rng = NewRng(20);
		var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		var result = rng.GetShuffled(input).ToList();

		await Assert.That(result).Count().IsEqualTo(input.Length);
		await Assert.That(result.OrderBy(x => x).SequenceEqual(input.OrderBy(x => x))).IsTrue();
		await Assert.That(result.Distinct().Count()).IsEqualTo(input.Length);
	}

	[Test]
	public async Task GetShuffled_ProducesDifferentOrderingsOverManyDraws()
	{
		var rng = NewRng(21);
		var input = Enumerable.Range(1, 10).ToArray();

		var orderings = new List<string>(100);
		for (var i = 0; i < 100; i++)
		{
			orderings.Add(string.Join(",", rng.GetShuffled(input)));
		}

		await Assert.That(orderings.Distinct().Count()).IsGreaterThan(1);
	}

	[Test]
	public async Task GetShuffled_IsDeterministic_ForTheSameSeed()
	{
		var input = Enumerable.Range(1, 10).ToArray();

		var expected = NewRng(22).GetShuffled(input).ToArray();
		var actual = NewRng(22).GetShuffled(input).ToArray();

		await Assert.That(actual.SequenceEqual(expected)).IsTrue();
	}

	[Test]
	public async Task GetShuffled_SingleItem_ReturnsThatItemAlone()
	{
		var rng = NewRng(23);
		var result = rng.GetShuffled("only").ToArray();

		await Assert.That(result).Count().IsEqualTo(1);
		await Assert.That(result[0]).IsEqualTo("only");
	}
}