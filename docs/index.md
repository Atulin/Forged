---
layout: home

hero:
  name: "Forged"
  text: "A fast, strict, and strongly-typed data generator (faker) for C#."
  tagline: "Powered by Source Generators. No reflection, no runtime surprises — just declarative, type-safe fake data."
  actions:
    - theme: brand
      text: Quick Start
      link: /guide/quick-start
    - theme: alt
      text: View on GitHub
      link: https://github.com/Atulin/Forged

features:
  - title: Source Generated
    details: Fakers are generated at compile time. No reflection at runtime, fully trim/AOT compatible, and fast.
  - title: Strict & Type-Safe
    details: Required properties must be given a generator — the compiler enforces it. Setter-less properties can't be faked, so your model stays honest.
  - title: Fluent API
    details: 'Compose generators and modifiers in a clean, readable pipeline: length ranges, nullability, collections, transformations.'
  - title: Deterministic
    details: Pass a seeded Random instance and get the exact same data every time — perfect for tests and reproducible fixtures.
  - title: Localized
    details: Pass a CultureInfo to drive locale-aware generators and modifiers. Data falls back gracefully when a locale isn't shipped.
  - title: Built-in Domains
    details: Person, Internet, Finance, Network, Temporal, Text, and Statistical Random modules are included out of the box.
---
