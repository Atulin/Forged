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
    public required Func<Generator<string>> Bar { get; init; }
    public Func<Generator<int?>>? Baz { get; init; }
    public required Func<Generator<bool?>> Quz { get; init; }

    public override Foo Get()
    {
        return new Foo
        {
            Bar = this.Bar(generator).Generate(),
            Baz = this.Baz?.Invoke(generator)?.Generate(),
            Quz = this.Quz(generator).Generate(),
        }
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
3. Each property is of type `Generator<T>`, where `T` is the type of the original property

## Core

Contains a basic `Faker<T>` class that looks something like:

```csharp
public abstract class Faker<T>
{
    protected static class Name
    {
        public static string First();
        public static string Last();
        public static string Full();
    }
    
    protected static class Random
    {
        public static T Pick<T>(params T[] values);
        public static int Next(int min, int max);
        // ...
    }
    
    // ...
    
    public abstract IEnumerable<T> Get(int count);
}
```
