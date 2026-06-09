# Forged

A fast, strict, and strongly-typed data generator (faker) for C# powered by Source Generators.

Forged allows you to declaratively define how your models should be faked, leveraging the C# compiler to enforce required properties, nullability, and type-safety.

## Features

- 🚀 **Source Generated**: No reflection, fast at runtime, and fully trim/AOT compatible.
- 🛡️ **Strict & Type-Safe**: Respects your class properties. If a property in your model is `required`, the faker will force you to provide a generator for it at compile time.
- 🌊 **Fluent API**: A clean and readable fluent API for configuring generators and their modifiers.
- 🎲 **Deterministic**: Pass a seeded `Random` instance to the faker to generate the exact same data every time.

## Quick Start

1. Add the package to your project.
2. Decorate your model with `[Fake]`:

```csharp
using Forged.Core;

namespace MyProject;

[Fake]
public class Person
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public List<string>? MiddleNames { get; set; }
    public bool IsActive { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
```

3. The source generator will automatically create a `{ModelName}Faker` class for you. Configure it and generate data!

```csharp
using Forged.Core.Generators; 
using Forged.Core.Generators.Text; 

// The faker properties match your model's properties!
var faker = new PersonFaker
{
    Id = f => f.Text.Guid(GuidGenerator.Kind.V7),
    FirstName = f => f.Text.Alphanumeric(10),
    LastName = f => f.Text.Alphanumeric(10),
    
    // Non-required properties can be omitted, but you can still provide a generator:
    MiddleNames = f => f.Text
        .Alphanumeric(5)
        .Collection(3)
        .Refine(c => c.ToList()) // Map to List<string>
        .OrDefault(0.5f),        // 50% chance of being default (null)
        
    DateOfBirth = f => f.Temporal.Past().OrNull(0.2f), // 20% chance to be null
    IsActive = f => f.Random.Pick(true, false)
};

// Generate a single item
var person = faker.Get();

// Generate multiple items
var people = faker.Get(5);
```

### Deterministic Generation

If you need reproducible results (e.g., in unit tests), you can provide a seeded `Random` instance to the faker:

```csharp
var faker = new PersonFaker(new Random(12345)) 
{
    // ...
};
```

## Available Generators

The `Forge` instance (`f` in the lambda expressions) provides access to built-in generators categorized by modules:

### `Random`
- `Pick(params T[])` / `Pick(T[], count)` - Pick one or more items from a predefined set.
- `Number(min, max)` - Generate a random number between min and max.
- `WeightedPick(items, weights)` - Pick an item based on custom weights.

### `Temporal`
- `Between(min, max)` - Generate a `DateTime` between two dates.
- `Past(earliest)` / `Future(latest)` - Generate a `DateTime` in the past or future.
- `DateBetween`, `DateInPast`, `DateInFuture` - Same as above, but for `DateOnly`.
- `TimeBetween` - Generate a `TimeOnly`.

### `Text`
- `Alphanumeric(length)` - Generate a random alphanumeric string.
- `Hex(length)` - Generate a random hexadecimal string.
- `Guid(kind)` - Generate a Guid (supports V4 and V7).
- `Template(template)` - Generate a string from a template.

## Modifiers & Extensions

Any `Generator<T>` can be customized and composed using fluent methods:

### General Modifiers
- `.Or(other, probability)` / `.OrDefault(probability)` / `.OrNull(probability)` - Introduce a chance to return an alternative value or null.
- `.Refine(mappingFunc)` - Transform the generated value to another type or format (e.g., `IEnumerable<T>` to `List<T>`).
- `.Array(length)` / `.Enumerable(length)` / `.Collection(length)` - Generate a collection of items using the current generator.

### Specific Extensions
- **String specific**: `.ToUpper()`, `.ToLower()`, `.ToTitleCase()`
- **Temporal specific**: `.ToUtc()`, `.ToLocal()`, `.ToDateOnly()`, `.ToTimeOnly()`, `.TruncateToDate()`
- **Formatting**: `.ToString()`, `.ToString(format, cultureInfo)`
