Imagine, if you will:

```csharp
[Fake]
public class Foo
{
    public required string Bar { get; set; }
    public int? Baz { get; set; }
    public required bool? Quz { get; set; }
}
```

generates


```csharp
public class FooFaker : Faker<Foo>
{
    public required Func<Forge, Generator<string>> Bar { get; init; }
    public Func<Forge, Generator<int?>>? Baz { get; init; }
    public required Func<Forge, Generator<bool?>> Quz { get; init; }

    private Generator<string>? _barGenerator;
    private Generator<int?>? _bazGenerator;
    private Generator<bool?>? _quzGenerator;

    public override Foo Get()
    {
        _barGenerator ??= Bar(this.Forge);
        _bazGenerator ??= Baz?.Invoke(this.Forge);
        _quzGenerator ??= Quz(this.Forge);

        return new Foo
        {
            Bar = _barGenerator.Generate(),
            Baz = _bazGenerator?.Generate(),
            Quz = _quzGenerator.Generate(),
        };
    }

    public override IEnumerable<Foo> Get(int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return Get();
        }
    }
}
```

and you use it with

```csharp
var faker = new FooFaker {
    Bar = f => f.Name.First(),
    // no `Baz`, it's fine
    // no `Quz` would not be allowed, so you need this:
    Quz = f => f.Random.Pick(true, false, null)
};
var foos = faker.Get(10);
```

or, optionally, for a seeded random:

```csharp
var faker = new FooFaker(new Random(12345)) {
    Bar = f => f.Name.First(),
    Quz = f => f.Random.Pick(true, false, null)
};
var foos = faker.Get(10);
```

<details>
<summary>Old impl that had no chance to work</summary>

generates

```csharp
public class FooFaker : Faker<Foo>
{
    public required Generator<string> Bar { get; init; }
    public Generator<int?>? Baz { get; init; }
    public required Generator<bool?> Quz { get; init; }
    
    public override IEnumerable<Foo> Get(int count)
    {
        // ...
    }
}
```

and you use it with

```csharp
var faker = new FooFaker {
    Bar = Name.First(),
    // no `Baz`, it's fine
    // no `Quz` would not be allowed, so you need this:
    Quz = Random.Pick(true, false, null)
};
var foos = faker.Get(10);
```

</details>

Needs 2 parts: generator and base faker, so some `Forged.Generator` and `Forged.Core`
packaged neatly into one `Forged` nuggie

## Generator

Takes a decorated class and generates a `[Name]Faker` class, that:
1. Has properties with all the same names
2. Each property inherits the `required` and nullability of the original
3. Each property is of type `Func<Forge, Generator<T>>`, where `T` is the type of the original property

## Core

Contains a basic `Faker<T>` class that looks something like:

```csharp
public sealed class Forge
{
    public Random Rng { get; }
    public ForgeName Name { get; }
    public ForgeRandom Random { get; }

    public Forge(Random? random = null)
    {
        Rng = random ?? Global.Random;
        Name = new ForgeName(this);
        Random = new ForgeRandom(this);
    }
}

public class ForgeName 
{
    public Generator<string> First();
    public Generator<string> Last();
    public Generator<string> Full();
}

public class ForgeRandom 
{
    public Generator<T> Pick<T>(params T[] values);
    public Generator<int> Next(int min, int max);
    // ...
}
```
