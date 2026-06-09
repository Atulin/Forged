using System.Text.Json;
using Forged.Core.Generators;
using Forged.Core.Generators.Text;
using Forged.Demo;

var faker = new PersonFaker
{
	Id = f => f.Text.Guid(GuidGenerator.Kind.V7),
	FirstName = f => f.Text.Alphanumeric(10),
	LastName = f => f.Text.Alphanumeric(10),
	MiddleNames = f => f.Text
		.Alphanumeric(5)
		.Collection(3)
		.Refine(c => c.ToList())
		.OrDefault(0f),
	DateOfBirth = f => f.Temporal.Past().OrNull(0f),
	IsActive = f => f.Random.Pick(true, false)
};
 
var people = faker.Get(5);

Console.WriteLine("Generated Foos:");
foreach (var person in people)
{
    Console.WriteLine(JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true }));
}