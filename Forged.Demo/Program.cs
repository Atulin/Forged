using System.Text.Json;
using System.Text.RegularExpressions;
using Forged.Core.Generators.Extensions;
using Forged.Core.Generators.Internet;
using Forged.Core.Generators.Text;
using Forged.Core.Generators.Utility;
using Forged.Demo;

// Alas, `out var` does not work in object initializers
MemoValueGenerator<string> first = null!;
MemoValueGenerator<string> last = null!;

var faker = new PersonFaker
{
	// Generate a random GUID v7
	Id = f => f.Text
		.Guid(GuidGenerator.Kind.V7),

	// Generate a pronounceable first name
	FirstName = f => f.Text
		.Pronounceable(1, 3) // pronounceable word with a minimum of 1 and a maximum of 3 syllables
		.Capitalize()        // capitalize the first letter
		.Memo(out first),    // store the first name in a variable

	// Generate a pronounceable last name comprising one or two parts, e.g. "Skłodowska-Curie"
	LastName = f => f.Text
		.Pronounceable(1, 3) // pronounceable word with a minimum of 1 and a maximum of 3 syllables
		.ToLower()           // convert to lowercase
		.Capitalize()        // capitalize the first letter
		.Array(1, 2)		 // get an array of 1 or 2 of those words
		.Refine(x => string.Join("-", x)) // join the array into a single string
		.Refine(x => f.Random.CoinToss() ? $"Von {x}" : x) // add "Von" prefix if coin toss is true
		.Memo(out last),     // store the last name in a variable
	
	// Generate a full name, combining first and last names dynamically using Func
	FullName = f => f.Basic.Func(() => $"{first} {last}"),
	
	// Generate a list of random middle names
	MiddleNames = f => f.Text
		.Alpha(3, 5)  // strings of 3 to 5 random letters
		.ToLower()    // convert to lowercase
		.Capitalize() // capitalize the first letter
		.List(0, 3),  // generate a list of 0 to 3 names
	
	// Generate a random username, with 20% chance of being null
	Nickname = f => f.Internet
		.Username()
		.OrDefault(.2f),
	
	// Generate a random email address using a known provider and user's name and surname
	Email = f => f.Internet
		.Email(EmailKind.Known, f.Basic
			.Func(() => $"{first.Range(0, 3)}{last}") // Base the email on first 3 letters of first name and a full last name
			.Replace(new Regex("\\W"), "")   // Remove all non-alphanumeric characters
			.ToLower()                                // Convert to lowercase
		),

	// Generate a random date of birth
	DateOfBirth = f => f.Temporal
		.Between(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-18)) // a date between 80 and 18 years ago
		.Nullable(), // make it nullable (necessary for value types if the property is nullable)

	// Generate a random status
	IsActive = f => f.Random
		.Pick(true, false), // pick a random value from the list
	
	// Alternatively, we can use the CoinToss generator
	// IsActive = f => f.Random.CoinToss(),
	
	// Or even something like
	// IsActive = f => f.Basic.Literal(f.Random.CoinToss()),

	// Generate a random bio
	Bio = f => f.Text
		.Waffle(f.Random.Number<int>(3, 6), WaffleStyle.Fiction)
};

// Get 5 random people
var people = faker.Get(5);

Console.WriteLine("Generated people:");
foreach (var person in people)
{
	Console.WriteLine(JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true }));
}