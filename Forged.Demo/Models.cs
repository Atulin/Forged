using Forged.Core;

namespace Forged.Demo;

[Fake]
public class Person
{
	public required Guid Id { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string FullName { get; set;}
	public List<string>? MiddleNames { get; set; }
	public string? Nickname { get; set; }
	public required string Email { get; set; }
	public bool IsActive { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? Bio { get; set; }
	public required int Luck { get; init; }
}

public class Address
{
	public required string StreetName { get; init; }
	public required string StreetNumber { get; init; }
	public required string City { get; init; }
}

[Faker<Address>]
public partial class AddressGenerator;