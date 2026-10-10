# Examples

## The demo output

[`Forged.Demo`](https://github.com/Atulin/Forged/tree/main/Forged.Demo) spits out five fake
`Person` objects. A representative run:

```json
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
  "Bio": "The farmer quietly noticed the stone wall. Beyond the orchard, the shepherd followed.",
  "Luck": 12
}
{
  "Id": "01a12341-324e-7c23-9fe0-84f70c8c4788",
  "FirstName": "Raest",
  "LastName": "Thacraobliem",
  "FullName": "Raest Thacraobliem",
  "MiddleNames": [ "Tbysm", "Vrj" ],
  "Nickname": "W@rriorMonster",
  "Email": "raethacraobliem@gmail.com",
  "IsActive": false,
  "DateOfBirth": "1959-02-27T16:06:12.2224526Z",
  "Bio": "The child traced toward the field. An aging priest noticed the silent the lantern.",
  "Luck": 12
}
{
  "Id": "01a12341-324e-7cf6-9c3a-28ee75757c9f",
  "FirstName": "Blaoldprailoost",
  "LastName": "Tes-Meardbigrik",
  "FullName": "Blaoldprailoost Tes-Meardbigrik",
  "MiddleNames": [ "Ajyp" ],
  "Nickname": "ItsMeWraithNoob",
  "Email": "blatesmeardbigrik@outlook.com",
  "IsActive": true,
  "DateOfBirth": "1951-01-01T17:57:39.462885Z",
  "Bio": "The young woman quietly traced the field. The innkeeper traced the wooden fence beneath the forgotten sky.",
  "Luck": 11
}
```

## Takeaways

- **Memo keeps names consistent.** `FirstName` and `LastName` each end in `.Memo(out …)`, and
  `FullName` composes them with `Basic.Func`. Change one and the whole record stays coherent.
- **Collections with bounds.** `MiddleNames` is `List(0, 3)`, so it shows `[]`, one, or several.
- **Nullable members.** `Nickname` is `Username().OrDefault(.2f)` and `DateOfBirth` is
  `Temporal...().Nullable()`, so nulls appear naturally.
- **Composition over magic.** `Email` builds a local part from `first.Range(0, 3) + last`,
  strips non-word characters with `.Replace(regex, "")`, lowers it, then plugs a known provider.
- **Type-safe generation.** `Id` and `Luck` are `required` (init/set), and the generator
  assigns them at compile time — run `dotnet build` with a missing member and you'll get a
  compile error, not a runtime surprise.