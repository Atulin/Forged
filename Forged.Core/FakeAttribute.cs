using System;

namespace Forged.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class FakeAttribute : Attribute
{
}
