using System.Text.Json;
using Forged.Core.Generators;
using Forged.Core.Generators.Text;
using Forged.Demo;

var faker = new PersonFaker
{
	Id = f => f.Text.Guid(GuidGenerator.Kind.V7),

	FirstName = f => f.Text
		.Pronounceable(1, 3)
		.Capitalize(),

	LastName = f => f.Text
		.Pronounceable(1, 3)
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
		.Pick(true, false),

	Bio = f => f.Text
		.Lorem(20, 100)
		.Sentencify(8, 20)
};

var people = faker.Get(5);

Console.WriteLine("Generated people:");
foreach (var person in people)
{
	Console.WriteLine(JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true }));
}