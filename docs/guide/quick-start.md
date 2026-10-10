# Quick Start

Forged lets you declaratively describe how your models should be faked, and the C# compiler
(enforced by the source generator) makes sure you can't leave required members unconfigured.

## Installation

Add the NuGet package to your project:

```bash
dotnet add package Atulin.Forged
```

Forged targets **.NET 10** and ships its analyzer (`Forged.Generator.dll`) as a NuGet
analyzer, so adding the package is all you need — no separate analyzer reference required.

## 1. Decorate your model

```csharp
using Forged.Core;

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

Alternatively, declare a partial faker class with `[Faker<T>]`:

```csharp
[Faker<Person>]
public partial class PersonFaker;
```

> [!TIP]
> Both approaches generate the same thing. `[Fake]` produces a class named `{ModelName}Faker`
> (e.g. `PersonFaker`). `[Faker<T>]` produces a faker with the **exact name of the partial class
> you declare** — so `PersonFaker` above stays `PersonFaker`, not `PersonPersonFaker`.

## 2. Configure the faker

The generated faker exposes one **assignable property per property** on your model. Each expects
a `Func<Forge, IGenerator<T>>`:

```csharp
using Forged.Core.Generators;
using Forged.Core.Generators.Text;

var faker = new PersonFaker
{
    Id = f => f.Text.Guid(GuidGenerator.Kind.V7),
    FirstName = f => f.Text.Alphanumeric(10),
    LastName = f => f.Text.Alphanumeric(10),

    // Non-required members may be omitted; you can still give them a generator.
    MiddleNames = f => f.Text
        .Alphanumeric(5)
        .List(0, 3),          // a List<string> of 0 to 3 items
    DateOfBirth = f => f.Temporal.Past().OrNull(0.2f), // 20% chance to be null
    IsActive = f => f.Random.CoinToss(),
};
```

Only **public properties with a setter** (including `init`) participate in faking. Get-only
properties and public fields are ignored, and `required` properties must be assigned a generator
or you'll get a compile error.

## 3. Generate data

```csharp
var person = faker.Get();        // a single instance
var people = faker.Get(5);       // exactly 5 instances
var some = faker.Get(3, 6);      // 3, 4, or 5 instances (max is exclusive)
```

> [!WARNING]
> `Get(min, max)` is backed by `Random.Next(min, max)`, so the **maximum is exclusive** —
> `Get(3, 6)` yields between 3 and 5 instances, never 6.

The generated `Get()` uses an object initializer, which means `required` properties with a
nullable value **only** occur when the generator itself produces a default/null value. Properties
with no setter can't be assigned, but if they're initialized inline on your model they'll keep
that default.

## The `Forge` entry point

`f` (the `Forge` instance passed into every lambda) is the root for all generator modules:

| Module | Purpose |
| ------ | ------- |
| `f.Basic` | Literal and `Func<T>` values |
| `f.Random` | Coins, picks, numbers, distributions, dice |
| `f.Temporal` | Dates, times, offsets, timespans |
| `f.Text` | Strings, GUIDs, Lorem Ipsum, waffle, templates, emoji |
| `f.Internet` | Usernames, domains, emails |
| `f.Network` | IPs, ports, endpoints, MAC addresses |
| `f.Finance` | Currencies, IBANs, BICs, card numbers, routing numbers |
| `f.Person` | Names, prefixes, suffixes |

::: code-group

```csharp [Serializing output]
var people = faker.Get(5);
Console.WriteLine(
    JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true })
);
```

```json [Example output]
{
  "Id": "01a12341-3207-7ce2-8721-d99eb6007a8d",
  "FirstName": "Brox",
  "LastName": "Broxfif",
  "FullName": "Brox Broxfif",
  "MiddleNames": [],
  "Nickname": "Titan",
  "Email": "brobroxfif@icloud.com",
  "IsActive": true,
  "DateOfBirth": "1974-03-22T08:24:20.9350787Z",
  "Luck": 12
}
```

:::

Next: the full [generator reference](/guide/generators).