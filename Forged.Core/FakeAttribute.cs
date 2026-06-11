namespace Forged.Core;

/// <summary>
/// Marker attribute for classes or structs that represent fake data models.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class FakeAttribute : Attribute
{
}
