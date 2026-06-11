using System.Text.Json;
using Forged.Core.Generators;
using Forged.Core.Generators.Text;
using Forged.Demo;

var faker = new PersonFaker
{
	Id = f => f.Text.Guid(GuidGenerator.Kind.V7),
	
	FirstName = f => f.Text
		.Alpha(10, 12)
		.ToLower()
		.Capitalize(),
	
	LastName = f => f.Text
		.Alpha(5, 10)
		.Array(1, 2)
		.Refine(x => string.Join(" ", x))
		.ToLower()
		.ToTitleCase(),
	
	MiddleNames = f => f.Text
		.Alpha(3, 5)
		.ToLower()
		.Capitalize()
		.List(0, 3),
	
	DateOfBirth = f => f.Temporal
		.Past()
		.Nullable(),
	
	IsActive = f => f.Random
		.Pick(true, false)
};
 
var people = faker.Get(5);

Console.WriteLine("Generated people:");
foreach (var person in people)
{
    Console.WriteLine(JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true }));
}