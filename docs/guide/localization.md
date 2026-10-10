# Determinism, Localization & Gotchas

## Deterministic generation

Every `Forge` wraps an RNG. Pass your own `Random` instance and the same seed always reproduces
the same sequence:

```csharp
var forge = new Forge(new Random(42), CultureInfo.InvariantCulture);
var faker = new PersonFaker { /* ... same configuration ... */ };

var first = faker.Get();
var second = faker.Get();
// first == second, every run
```

Two `Forge` instances built with the **same seed and locale will produce identical data** — the
basis for golden-file style tests. Without a seed (default `Forge`), `Random.Shared` and
`InvariantCulture` are used and every run differs.

> [!TIP]
> Forge with a new `Random()` (no seed) per run when you want varied fixtures, e.g. in
> `[SetUp]`/`[TestInitialize]`, and a *seeded* `Forge` inside tests that assert structure.

## Localization

Pass a `CultureInfo` to a `Forge` and generators pull their word lists from locale data files.

```csharp
var frenchForge = new Forge(new Random(7), new CultureInfo("fr-FR"));
var faker = new PersonFaker
{
    FirstName = f => f.Person.FirstName(),
    /* ... */
};
```

### Data availability

Locale data ships as embedded JSON resources under `Forged.Core/Locales/...`. **Only the `en`
locale is shipped with the package** as of v1.2.0, but the lookup logic supports any culture:

- Lookup order is **exact locale → base language → `en`**.
  - `fr-FR` ⇒ tries `fr-FR`, then `fr`, then `en`.
  - `fr` ⇒ tries `fr`, then `en`.
  - `en-GB` ⇒ tries `en-GB`, then `en`.
- If no file matches any of those, the generator falls back to a reasonable default (the file
  loader's embedded resources), so generation never fails for an unsupported locale.
- Files are parsed once and **cached in a static `ConcurrentDictionary`** keyed by
  `(locale, file)` and shared across all `Forge` instances for the process lifetime. Repeated
  `Get()` calls reuse the parsed data — cheap to call, but note the cache is process-global, so
  hot-reload/edited `.json` data in tests won't be re-read without a fresh process.

**If you add your own locale data**, place files at the expected resource path for your culture
(e.g. `Locales/fr/person/name/last.jsonc`) and build them into the library — or contribute them
upstream. Structure mirrors the `en` files (JSON, comments allowed).

## Gotchas

### `Faker.Get(min, max)` max is exclusive

`Get(3, 6)` yields between 3 and 5 items — the upper bound behaves like `Random.Next(min, max)`.

```csharp
faker.Get(3, 6).Count();      // 3, 4, or 5
faker.Get(3, 6).Count() <= 5; // always true
```

### `out var` is not allowed in object initializers

To use `Memo`, declare the memo variables first:

```csharp
MemoValueGenerator<string> first = null!;
MemoValueGenerator<string> last = null!;

var faker = new PersonFaker
{
    FirstName = f => f.Text.Pronounceable(1, 3).Capitalize().Memo(out first),
    LastName  = f => f.Text.Pronounceable(1, 3).ToLower().Capitalize().Memo(out last),
    FullName  = f => f.Basic.Func(() => $"{first} {last}"),
};
```

### Only public settable properties are faked

The source generator looks at properties with a public `set`/`init`. Get-only properties and
public fields are ignored — they keep whatever initial value the class sets. This is by design:
Forged never writes through reflection, so fakes can't mutate readonly state.

`required` properties behave differently: the generator emits a compile-time assignment for
them, so **you must configure a generator for every `required` member** or your code won't
compile.

### Nullable value type members need `.Nullable()` / `.OrNull()`

A `DateTime?` property assigned a `Generator<DateTime>` is a type mismatch. Convert explicitly:

```csharp
DateOfBirth = f => f.Temporal.Past().Nullable(),   // DateTime -> DateTime?
MaybeLogout = f => f.Temporal.Past().OrNull(.5f),
```

Reference-type `string?` members can take a `Generator<string>` directly, or use `.OrDefault(p)`
for a chance of null:

```csharp
Nickname = f => f.Internet.Username().OrDefault(.2f),   // string? with 20% null
```

### The extension namespace must be imported

String, struct, date-time and collection transforms live in `Forged.Core.Extensions`. Remove the
`using` and `.Capitalize()`, `.Nullable()`, `.AsList()`, `.ToUtc()` etc. won't resolve.

### `Email`'s second parameter is the local part generator

Despite its `provider` name, the second argument to `Forge.Internet.Email` feeds the username
before the `@`. Pass a name generator to base email addresses on real names; the domain comes
from `EmailKind`.

### GUID kind decides ordering

`f.Text.Guid()` defaults to V4 (random). For time-ordered values (better index locality), use
`GuidGenerator.Kind.V7` explicitly: `f.Text.Guid(GuidGenerator.Kind.V7)`.

### Single `Generator` instances are reusable

Generators are immutable factories. The same configured generator can be assigned to several
properties or called many times; every `Generate()` draws fresh values. Only `Literal` always
returns the same value, and only `Memo` pins one value per `Get()` cycle.