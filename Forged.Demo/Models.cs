using Forged.Core;

namespace Forged.Demo;

[Fake]
public class Person
{
	public required Guid Id { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public List<string>? MiddleNames { get; set; }
	public bool IsActive { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? EmailAddress { get; set; }
	public string? Bio { get; set; }
}