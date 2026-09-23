using Forged.Core;

namespace Forged.Tests;

/// <summary>Record model used to verify <c>[Fake]</c> produces a faker named <c>PersonFaker</c>.</summary>
[Fake]
public sealed class Person
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
}

/// <summary>Record model used to verify that <c>[Fake]</c> and <c>[Faker&lt;T&gt;]</c> work for records and init-only setters.</summary>
[Fake]
public sealed record Address
{
    public required string Street { get; init; }
    public string City { get; init; } = string.Empty;
    public string? Country { get; set; }
}

/// <summary>Explicit faker generated from the target model <see cref="Address"/>.</summary>
[Faker<Address>]
public partial class AddressGenerator
{
}

/// <summary>Struct model used to verify <c>[Fake]</c> produces a faker for structs.</summary>
[Fake]
public struct Credentials
{
    public required string Username { get; set; }
    public string Password { get; set; }
}