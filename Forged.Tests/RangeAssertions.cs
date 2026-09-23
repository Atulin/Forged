using TUnit.Assertions.Core;

namespace Forged.Tests;

public static class RangeAssertions
{
    public static Assertion<T> IsInRange<T>(this IAssertionSource<T> source, T min, T max)
        where T : IComparable<T>
        => source.IsGreaterThanOrEqualTo(min).And.IsLessThanOrEqualTo(max);
}