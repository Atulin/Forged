# Generator Reference

Every module method returns a `Generator<T>` — an immutable, reusable factory. Generators don't
produce anything until they're invoked: `Generate()`, an implicit cast to `T`, or a faker's
`Get()` call them for you.

```csharp
var fullName = forge.Person.FullName();   // Generator<string>
var name = fullName.Generate();           // explicit
string other = fullName;                  // implicit conversion
```

## Basic

Literal and functional values.

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Literal` | `Literal<T>(T literal)` | Always returns the same value. |
| `Func` | `Func<T>(Func<T> func)` | Calls the function each time it's used. |

```csharp
FirstName = f => f.Basic.Literal("Ada"),
CreatedBy = f => f.Basic.Func(() => Environment.UserName),
```

## Random

Selection and distribution tools.

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `CoinToss` | `CoinToss()` | `bool`, fair 50/50. |
| `Pick` | `Pick<T>(params T[] items)` | Uniform single draw from a fixed list. |
| `Pick` | `Pick<T>(T[] items, int count)` | Array of exactly `count` draws (with replacement). |
| `Pick` | `Pick<T>(T[] items, int minCount, int maxCount)` | Array of `minCount..maxCount` draws. |
| `PickUnique` | `PickUnique<T>(T[] items, int count)` / `(items, minCount, maxCount)` | Draws without replacement. |
| `Pick` (enum) | `Pick<T>() where T : struct, Enum` | Uniform enum value. |
| `Number` | `Number<T>(T? min = null, T? max = null)` | Uniform numeric, inclusive; defaults to the type's min/max. Any `INumber<T>`. |
| `Uniform` | `Uniform<T>(T? min = null, T? max = null)` | Alias of `Number`. |
| `Normal` | `Normal<T>(T? mean = null, T? stdDev = null)` | Mean 0 / std-dev 1 by default. |
| `LogNormal` | `LogNormal<T>(T? mean = null, T? stdDev = null)` | Positive values from the underlying normal. |
| `Exponential` | `Exponential<T>(T? rate = null)` | Non-negative; rate 1 by default. |
| `Bernoulli` | `Bernoulli<T>(T? probability = null)` | Produces `1` with `probability` (0.5 by default), else `0`. |
| `Poisson` | `Poisson<T>(T? lambda = null)` | Non-negative integer values; lambda 1 by default. |
| `WeightedPick` | `WeightedPick<T>(T[] items, float[] weights)` | One weight per item, higher = more likely. |
| `WeightedPick` | `WeightedPick<T>((T item, float weight)[] items)` | Item/weight tuple form. |
| `Dice` | `Dice(string expression, RoundingMode mode = Round)` | Classic dice notation, e.g. `"2d6+1d10-3"`. |

```csharp
Gender = f => f.Random.Pick("M", "F", "X"),
Handful = f => f.Random.Pick(new[] { 1, 2, 3, 4 }, 2),
EnumVal = f => f.Random.Pick<MyEnum>(),
Score = f => f.Random.Dice("2d6+1d10-3"),
```

## Temporal

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Between` | `Between(DateTime? min = null, DateTime? max = null)` | Inclusive; defaults to `DateTime.MinValue..MaxValue`. |
| `Past` | `Past(DateTime? earliest = null)` | `earliest..DateTime.UtcNow`; default earliest = one year ago. |
| `Future` | `Future(DateTime? latest = null)` | `DateTime.UtcNow..latest`; default latest = one year from now. |
| `DateBetween` / `DateInPast` / `DateInFuture` | Same shapes for `DateOnly`. | |
| `TimeBetween` | `TimeBetween(TimeOnly? min = null, TimeOnly? max = null)` | Time of day. |
| `DateTimeOffsetBetween` / `InPast` / `InFuture` | Same shapes for `DateTimeOffset`. | `DateTimeOffset.UtcNow` used as the reference. |
| `TimeSpanBetween` | `TimeSpanBetween(TimeSpan? min = null, TimeSpan? max = null)` | Durations. |

```csharp
DateOfBirth = f => f.Temporal.Between(new DateTime(1950, 1, 1), DateTime.Today),
LastLogin = f => f.Temporal.Past(),
```

## Text

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Alphanumeric` | `Alphanumeric(int length)` or `(int min, int max)` | `[A-Za-z0-9]`. |
| `Alpha` | `Alpha(int length)` or `(int min, int max)` | Letters only. |
| `Pronounceable` | `Pronounceable(int length)` or `(int min, int max)` | Readable pseudo-words; the length is in *syllables*. |
| `Lorem` | `Lorem(int length, LoremIpsumGenerator.Options? options = null)` or `(min, max, options)` | Classic lorem ipsum; length in *words*. |
| `Waffle` | `Waffle(int sentences, WaffleStyle style = Technical)` | Natural-sounding filler. |
| `Hex` | `Hex(int length)` or `(int min, int max)` | Hex digits. |
| `Guid` | `Guid(GuidGenerator.Kind kind = V4)` | `V4` (random) or `V7` (time-ordered). |
| `Template` | `Template(string template)` | `#` digit, `?` letter, `*` alnum; `\` escapes the next char. |
| `Char` | `Char(CharKind kind = Printable)` | Single char: `Printable`, `Alphanumeric`, `Alphabetical`, `Numeric`, … |
| `Chars` | `Chars(int count, CharKind kind = Printable)` or `(min, max, kind)` | `char[]`. |
| `Emoji` | `Emoji()` | A single emoji. |
| `EmojiRunes` | `EmojiRunes()` | The emoji as a `Rune[]` (handles multi-codepoint emoji). |

```csharp
Id = f => f.Text.Guid(GuidGenerator.Kind.V7),
Sku = f => f.Text.Template("###-???-***"),
Bio = f => f.Text.Waffle(2),
```

## Internet

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Username` | `Username(float prefixChance = 0.5f, float suffixChance = 0.5f, float leetChance = 0.1f)` | e.g. `TribalFox42`. |
| `Domain` | `Domain(float ccSldChance = 0.0f)` | `example.com` style. |
| `Email` | `Email(EmailKind kind = Random, IGenerator<string>? provider = null)` | See below. |

```csharp
Email = f => f.Internet.Email(),
Contact = f => f.Internet.Email(EmailKind.Example),
WorkEmail = f => f.Internet.Email(EmailKind.Known, f.Text.Alpha(8).ToLower()),
```

`Email`'s second parameter supplies the **local part** (the name before `@`). It's named `provider`
for historical reasons but it is a generator for the username — pass a name generator to base the
address on real names. When omitted, a username generator is used.

### `EmailKind`

| Kind | Behavior |
| ---- | -------- |
| `Known` | Real provider domains (`gmail.com`, `icloud.com`, …) from locale data. |
| `Example` | `example.com` / `example.org` / `example.net`. |
| `Random` (default) | Pronounceable local part + randomly generated domain. |

## Network

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Ipv4` | `Ipv4(float publicChance = 1.0f)` | |
| `Ipv6` | `Ipv6(float publicChance = 1.0f)` | |
| `IpAddress` | `IpAddress(float publicChance = 1.0f, float ipv6Chance = 0.45f)` | IPv4 or IPv6. |
| `Port` | `Port(float wellKnownChance = 0.0f)` | 1–65535. |
| `Endpoint` | `Endpoint(float ipv6Chance = 0.45f, float publicChance = 1.0f, float wellKnownPortChance = 0.0f)` | `IPEndPoint`; plus `Ipv4Endpoint` / `Ipv6Endpoint` shortcuts. |
| `UriScheme` | `UriScheme(bool strip = false)` | |
| `MacAddress` | `MacAddress(float locallyAdministeredChance = 0.0f)` | `PhysicalAddress`. |

## Finance

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `Currency` | `Currency()` | Alias of `CurrencyName()`. |
| `CurrencyCode` / `CurrencyName` / `CurrencySymbol` / `CurrencyNumericCode` | — | ISO 4217 projections. |
| `Iban` | `Iban(bool valid = true, bool test = false)` or `Iban(string countryCode, ...)` | Valid mod-97 check digit unless `valid: false`; `test: true` returns a known test IBAN. |
| `Bic` / `Swift` | `Bic(bool valid = true, bool test = false)` | 8- or 11-char SWIFT/BIC. |
| `RoutingNumber` | `RoutingNumber(bool valid = true, bool test = false)` | ABA 9-digit, mod-10 checksum. |
| `AccountNumber` | `AccountNumber(int length = 8)` or `AccountNumber(int minLength, int maxLength)` | Digits only. |
| `CardOperator` | `CardOperator()` | A single `CardOperator` flag value. |
| `CreditCardNumber` | `CreditCardNumber(CardOperatorFlag cardOperator = All, bool valid = true, bool test = false)` | Luhn-valid unless `valid: false`. |
| `CreditCardCvv` / `Cvv` / `Cvc` | `CreditCardCvv(CardOperatorFlag cardOperator = All)` | 3- or 4-digit, network-dependent. |
| `CreditCardExpirationDate` / `ExpirationDate` | `CreditCardExpirationDate(DateTimeOffset? today = null, float expirationChance = 0.5f)` | First of month; ~50% already expired by default. |

`CardOperator` is defined in the `Forged.Core.Generators.Finance` namespace.

## Person

| Member | Signature | Notes |
| ------ | --------- | ----- |
| `FirstName` | `FirstName(float male = 0.5f, float female = 0.5f)` | |
| `LastName` | `LastName(float hyphenated = 0.04f, float compound = 0.02f)` | |
| `NamePrefix` | `NamePrefix()` | `Dr.`, `Mr.`, … |
| `NameInfix` | `NameInfix()` | `van`, `de`, … |
| `NameSuffix` | `NameSuffix()` | `Jr.`, `Sr.`, … |
| `FullName` | `FullName(float male = 0.5f, float female = 0.5f, float middle = 0.8f, float hyphenated = 0.04f, float compound = 0.02f, float prefix = 0f, float infix = 0.03f, float suffix = 0.04f)` | Composes parts by probability. |

All person data comes from locale files, so `FirstName`, `LastName`, `NamePrefix`, `NameInfix`,
`NameSuffix` and their compound full-name forms reflect the active locale's data set.