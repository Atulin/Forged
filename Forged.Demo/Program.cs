using System.Text.Json;
using Forged.Core.Generators;
using Forged.Demo;

var faker = new FooFaker(new Random(12345))
{
    Bar = f => f.Text.Alphanumeric(10),
    Baz = f => f.Random.Number<int>(1, 100).OrNull(0.2f),
    Quz = f => f.Random.Pick<bool?>(true, false, null),
    Timestamp = f => f.Temporal.Past()
};

var foos = faker.Get(5);

Console.WriteLine("Generated Foos:");
foreach (var foo in foos)
{
    Console.WriteLine(JsonSerializer.Serialize(foo, new JsonSerializerOptions { WriteIndented = true }));
}