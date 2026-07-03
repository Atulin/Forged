namespace Forged.Core;

/// <summary>
/// Marker attribute for classes or structs that represent fakers.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class FakerAttribute<TModel> : Attribute;
