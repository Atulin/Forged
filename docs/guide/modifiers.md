# Modifiers & Extensions

Forged generators compose through two layers of fluent transforms:

1. **Built-in methods** on `Generator<T>` for the common cases (collections, memoization,
   nullability, refinement).
2. **Extension methods** in the `Forged.Core.Extensions` namespace that specialize generators
   by type (strings, `DateTime`, structs, enumerables).

All of these produce a new generator; nothing mutates the original, so generators are safe to
reuse and share.

## Built-in transforms on `Generator<T>`

### Refining values

```csharp
var joined = f.Text.Alpha(3, 5).Refine(x => string.Join("-", x));
```

`Refine<TNew>(Func<T, TNew>)` maps each generated value to a new one (that demo joins words into
a full name, or adds a `"Von "` prefix when a `CoinToss` lands true).

### Collections

| Member | Returns | Notes |
| ------ | ------- | ----- |
| `Enumerable(length)` / `Enumerable(min, max)` | `IEnumerable<T>` | |
| `Array(length)` / `Array(min, max)` | `T[]` | |
| `List(length)` / `List(min, max)` | `List<T>` | |
| `HashSet(length)` / `HashSet(min, max)` | `HashSet<T>` | |

```csharp
MiddleNames = f => f.Text.Alpha(3, 5).ToLower().Capitalize().List(0, 3),
Tags = f => f.Text.Pronounceable(1, 2).Array(2, 6),
```

Collection min/max bounds are inclusive on both ends.

### Nullability

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `OrDefault(float probability)` | `T?` | Produces `default(T)` with the given probability, else the value. |

```csharp
Nickname = f => f.Internet.Username().OrDefault(.2f),
```

### Fallback alternatives

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Or(T other, float probability)` | `T` | Produces `other` with the given probability, else the value. |

### Memoization

`Memo(out MemoValueGenerator<T>)` captures one generated value per `Get()` cycle so it can be
reused inside the same generated object. This is how a faker makes `FullName` consistent with
`FirstName` + `LastName`:

```csharp
MemoValueGenerator<string> first = null!;
MemoValueGenerator<string> last = null!;

var faker = new PersonFaker
{
    FirstName = f => f.Text.Pronounceable(1, 3).Capitalize().Memo(out first),
    LastName = f => f.Text.Pronounceable(1, 3).Capitalize().Memo(out last),
    FullName = f => f.Basic.Func(() => $"{first} {last}"),
};
```

> [!WARNING]
> `out var` isn't allowed inside an object initializer, so declare the memo variables *before*
> the faker initializer and assign them via `out` as shown above.

`MemoValueGenerator<T>` is itself a `Generator<T>` — using it later (`$"{first} {last}"`)
re-emits the captured value for the current generation.

### Casting

```csharp
IGenerator<Cat?> asCat = animalGen.Cast<Cat>();   // where Cat : Animal
```

`Cast<TOut>()` where `TOut : T` does a value cast (`(TOut)value`) per generation; it returns
`IGenerator<TOut?>`.

## Extension methods

These live in the `Forged.Core.Extensions` namespace (import it explicitly — the package does
not add a global `using`).

### Strings (`Generator<string>`)

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `ToUpper()` / `ToLower()` | — | Case conversion. |
| `ToTitleCase(CultureInfo? = null)` | — | Invariant by default. |
| `Capitalize(CultureInfo? = null)` | — | Upper-cases only the first letter. |
| `Sentencify(int length)` / `Sentencify(int min, int max)` | — | Join words into a capitalised, terminated sentence. |
| `Range(int start, int? end = null)` / `Range(Range)` | — | Substring (`end` exclusive). |
| `Replace(string, string)` / `Replace(char, char)` / `Replace(Regex, string)` | — | Text replacement; the `Regex` overload uses source-generated regex. |

```csharp
Slug = f => f.Text.Waffle(1).ToLower().Replace(" ", "-"),
Initial = f => f.Person.FirstName().Range(0, 1).Capitalize(),
Headline = f => f.Person.FirstName().ToTitleCase(),
```

### `DateTime` (`Generator<DateTime>`)

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `ToUtc()` | — | |
| `ToLocal()` | — | |
| `ToDateOnly()` | `Generator<DateOnly>` | |
| `ToTimeOnly()` | `Generator<TimeOnly>` | |
| `TruncateToDate()` | — | Midnight. |

### Structs (`Generator<T> where T : struct`)

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `OrNull(float probability)` | `Generator<T?>` | Null with the given probability. |
| `Nullable()` | `Generator<T?>` | Always a nullable `T`. |

```csharp
DateOfBirth = f => f.Temporal.Between(ref).Nullable(),   // DateTime -> DateTime?
MaybeLuck = f => f.Random.Number<int>(1, 10).OrNull(.5f),
```

### Enumerables & collections

On `Generator<IEnumerable<T>>` and `Generator<ICollection<T>>`:

| Member | Where | Returns |
| ------ | ----- | ------- |
| `AsList()` | both | `Generator<List<T>>` |
| `AsHashSet()` | both | `Generator<HashSet<T>>` |
| `AsDictionary(keySelector, valueSelector)` | `IEnumerable<T>` only | `Generator<Dictionary<TKey, TValue>>` |
| `Shuffle()` | `IEnumerable<T>` only | `Generator<IEnumerable<T>>`, reorders using the forge's RNG |

```csharp
Lookup = f => f.Text.Pronounceable(1, 2).Enumerable(3).AsDictionary(w => w, w => w.Length),
Deck = f => f.Random.Number<int>(1, 100).Enumerable(5).Shuffle(),
```

### `Generator<T>.ToString`

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `ToString<T>()` | `Generator<string?>` | Plain `value?.ToString()`. |
| `ToString<T>(string format, CultureInfo? = null)` | `Generator<string>`; `T : ISpanFormattable` | Formatted (e.g. `"N2"`). |

```csharp
Price = f => f.Random.Number<decimal>(10m, 5000m).ToString("C2", CultureInfo.GetCultureInfo("en-US")),
```