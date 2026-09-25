using System.Collections.Immutable;
using Forged.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Assembly = System.Reflection.Assembly;

namespace Forged.Tests;

public class SourceGeneratorTests
{
    private static IEnumerable<MetadataReference> GetReferences()
    {
        var trusted = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;
        var refs = new Dictionary<string, MetadataReference>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in trusted?.Split(Path.PathSeparator) ?? [])
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                continue;
            }

            var name = Path.GetFileNameWithoutExtension(path);
            if (name == "Forged.Core")
            {
                continue;
            }

            refs.TryAdd(name, MetadataReference.CreateFromFile(path));
        }

        refs["Forged.Core"] = MetadataReference.CreateFromFile(typeof(FakeAttribute).Assembly.Location);
        return refs.Values;
    }

    private static ForgedGeneratorProxy LoadForgedGenerator()
    {
        var generatorPath = Path.Combine(AppContext.BaseDirectory, "Forged.Generator.dll");
        var generatorAssembly = Assembly.LoadFrom(generatorPath);
        var generator = (IIncrementalGenerator)Activator.CreateInstance(
            generatorAssembly.GetType("Forged.Generator.ForgedGenerator", throwOnError: true)!)!;
        return new ForgedGeneratorProxy(generator);
    }

    private static GeneratorDriverRunResult RunGenerator(string source, out Compilation output, out ImmutableArray<Diagnostic> allDiagnostics)
    {
        var generator = LoadForgedGenerator();
        var tree = CSharpSyntaxTree.ParseText(
            source,
            path: "SourceGenInput.cs",
            options: CSharpParseOptions.Default);

        var compilation = CSharpCompilation.Create(
            "Forged.SourceGenTests",
            [tree],
            GetReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(generator).RunGeneratorsAndUpdateCompilation(
            compilation,
            out var updatedCompilation,
            out var runDiagnostics);

        output = updatedCompilation;
        allDiagnostics = [.. runDiagnostics, .. output.GetDiagnostics()];
        return driver.GetRunResult();
    }

    private const string PersonSource = """
        #nullable enable
        using Forged.Core;

        namespace Forged.Tests.SourceGen;

        [Fake]
        public sealed class Person
        {
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public int Age { get; set; }
            public string? Email { get; set; }
        }

        public static class PersonUsage
        {
            public static PersonFaker Create()
                => new()
                {
                    FirstName = f => f.Basic.Literal("Alice"),
                    LastName = f => f.Basic.Literal("Smith")
                };
        }
        """;

    [Test]
    public async Task FakeAttribute_GeneratesAFakerClass_WithTheExpectedHintName()
    {
        var runResult = RunGenerator(PersonSource, out _, out var diagnostics);

        await Assert.That(diagnostics).IsEmpty();
        await Assert.That(runResult.Results).Count().IsEqualTo(1);

        var generated = runResult.Results[0].GeneratedSources;
        await Assert.That(generated.Select(s => s.HintName)).Contains("PersonFaker.g.cs");

        var fakerSource = generated.Single(s => s.HintName == "PersonFaker.g.cs").SourceText.ToString();
        await Assert.That(fakerSource).Contains("public partial class PersonFaker : global::Forged.Core.Faker<Person>");
        await Assert.That(fakerSource).Contains("public override Person Get()");
        await Assert.That(fakerSource).Contains("public PersonFaker(global::System.Random? random = null, global::System.Globalization.CultureInfo? locale = null) : base(random, locale)");
    }

    [Test]
    public async Task RequiredMembers_AreEmiAsRequiredProperties()
    {
        var runResult = RunGenerator(PersonSource, out _, out _);
        var generated = runResult.Results[0].GeneratedSources;
        var fakerSource = generated.Single(s => s.HintName == "PersonFaker.g.cs").SourceText.ToString();

        await Assert.That(fakerSource).Contains("public required virtual global::System.Func<global::Forged.Core.GenerationContext<Person>, global::Forged.Core.Generators.IGenerator<string>> FirstName { get; init; }");
    }

    [Test]
    public async Task OptionalMembers_BecomeNullableDelegateProperties()
    {
        var runResult = RunGenerator(PersonSource, out _, out _);
        var generated = runResult.Results[0].GeneratedSources;
        var fakerSource = generated.Single(s => s.HintName == "PersonFaker.g.cs").SourceText.ToString();

        await Assert.That(fakerSource).Contains("virtual global::System.Func<global::Forged.Core.GenerationContext<Person>, global::Forged.Core.Generators.IGenerator<int>>? Age { get; init; }");
        await Assert.That(fakerSource).Contains("virtual global::System.Func<global::Forged.Core.GenerationContext<Person>, global::Forged.Core.Generators.IGenerator<string?>>? Email { get; init; }");
    }

    [Test]
    public async Task GeneratedGet_AppliesTheConfiguredGenerators()
    {
        var runResult = RunGenerator(PersonSource, out _, out _);
        var fakerSource = runResult.Results[0].GeneratedSources
            .Single(s => s.HintName == "PersonFaker.g.cs").SourceText.ToString();

        await Assert.That(fakerSource).Contains("using var __forgedScope = base.Forge.BeginGeneration();");
        await Assert.That(fakerSource).Contains("__forgedScope.Register<string>(nameof(FirstName), firstNameGenerator);");
        await Assert.That(fakerSource).Contains("FirstName = __forgedScope.Get<string>(nameof(FirstName)),");
        await Assert.That(fakerSource).Contains("Age = __forgedScope.GetOrDefault<int>(nameof(Age)),");
    }

    private const string GeneratedReferenceSource = """
        #nullable enable
        using Forged.Core;

        namespace Forged.Tests.SourceGen.Generated;

        [Fake]
        public sealed class GeneratedPerson
        {
            public required string FullName { get; set; }
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public required string Forge { get; set; }
            public required string Context { get; set; }
        }

        public static class GeneratedPersonUsage
        {
            public static GeneratedPersonFaker Create()
                => new()
                {
                    FirstName = f => f.Basic.Literal("Ada"),
                    LastName = f => f.Basic.Literal("Lovelace"),
                    FullName = f => f.Basic.Func(() => $"{f.Generated.FirstName} {f.Generated.LastName}"),
                    Forge = f => f.Basic.Literal("forge"),
                    Context = f => f.Basic.Literal("context")
                };
        }
        """;

    [Test]
    public async Task GeneratedFacade_ReferencesPropertiesWithoutOutVariables()
    {
        var runResult = RunGenerator(GeneratedReferenceSource, out var compilation, out var diagnostics);

        await Assert.That(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)).IsEmpty();

        var generated = runResult.Results[0].GeneratedSources;
        await Assert.That(generated.Select(s => s.HintName)).Contains("ForgedGeneratedValues.Forged.Tests.SourceGen.Generated.g.cs");

        var facadeSource = generated.Single(s => s.HintName == "ForgedGeneratedValues.Forged.Tests.SourceGen.Generated.g.cs").SourceText.ToString();
        await Assert.That(facadeSource).Contains("extension(global::Forged.Core.GenerationContext<GeneratedPerson> context)");
        await Assert.That(facadeSource).Contains("public ForgedGeneratedGeneratedPersonValues Generated => new(context.Forge);");
        await Assert.That(facadeSource).Contains("public global::Forged.Core.Generators.Generator<string> FirstName");

        using var stream = new MemoryStream();
        var emit = compilation.Emit(stream);
        await Assert.That(emit.Success).IsTrue();

        var assembly = Assembly.Load(stream.ToArray());
        var usageType = assembly.GetType("Forged.Tests.SourceGen.Generated.GeneratedPersonUsage", throwOnError: true)!;
        var faker = usageType.GetMethod("Create")!.Invoke(null, null)!;
        var person = faker.GetType().GetMethod("Get", Type.EmptyTypes)!.Invoke(faker, null)!;

        await Assert.That(person.GetType().GetProperty("FirstName")!.GetValue(person)).IsEqualTo("Ada");
        await Assert.That(person.GetType().GetProperty("LastName")!.GetValue(person)).IsEqualTo("Lovelace");
        await Assert.That(person.GetType().GetProperty("FullName")!.GetValue(person)).IsEqualTo("Ada Lovelace");
        await Assert.That(person.GetType().GetProperty("Forge")!.GetValue(person)).IsEqualTo("forge");
        await Assert.That(person.GetType().GetProperty("Context")!.GetValue(person)).IsEqualTo("context");
    }

    private const string ModelIsolationSource = """
        #nullable enable
        using Forged.Core;

        namespace Forged.Tests.SourceGen.Isolation;

        [Fake]
        public sealed class IsolatedPerson
        {
            public required string FirstName { get; set; }
        }

        [Fake]
        public sealed class OtherAddress
        {
            public required string Street { get; set; }
        }

        public static class IsolatedPersonUsage
        {
            public static IsolatedPersonFaker Create()
                => new()
                {
                    FirstName = f => f.Generated.OtherAddress
                };
        }
        """;

    [Test]
    public async Task GeneratedFacade_IsScopedToTheFakerModel()
    {
        RunGenerator(ModelIsolationSource, out _, out var diagnostics);

        var errors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        await Assert.That(errors.Select(d => d.Id)).Contains("CS1061");
    }

    private const string FakerForSource = """
        using Forged.Core;

        namespace Forged.Tests.SourceGen;

        [Fake]
        public sealed class Address
        {
            public required string Street { get; set; }
            public string City { get; set; } = string.Empty;
        }

        [Faker<Address>]
        public partial class AddressGenerator
        {
        }
        """;

    [Test]
    public async Task FakerAttribute_AddsAPartialDeclarationToTheExistingFakerType()
    {
        var runResult = RunGenerator(FakerForSource, out _, out var diagnostics);

        await Assert.That(diagnostics).IsEmpty();
        var generated = runResult.Results[0].GeneratedSources;
        await Assert.That(generated.Select(s => s.HintName)).Contains("AddressGenerator.g.cs");

        var fakerSource = generated.Single(s => s.HintName == "AddressGenerator.g.cs").SourceText.ToString();
        await Assert.That(fakerSource).Contains("public partial class AddressGenerator : global::Forged.Core.Faker<Address>");
        await Assert.That(fakerSource).Contains("partial class AddressGenerator");
        await Assert.That(fakerSource).Contains("public required virtual global::System.Func<global::Forged.Core.GenerationContext<Address>, global::Forged.Core.Generators.IGenerator<string>> Street { get; init; }");
    }

    private const string StructSource = """
        using Forged.Core;

        namespace Forged.Tests.SourceGen;

        [Fake]
        public struct Credentials
        {
            public required string Username { get; set; }
            public string Password { get; set; }
        }
        """;

    [Test]
    public async Task FakeStruct_GeneratesAFakerAsWell()
    {
        var runResult = RunGenerator(StructSource, out _, out var diagnostics);

        await Assert.That(diagnostics).IsEmpty();
        var generated = runResult.Results[0].GeneratedSources;
        await Assert.That(generated.Select(s => s.HintName)).Contains("CredentialsFaker.g.cs");

        var fakerSource = generated.Single(s => s.HintName == "CredentialsFaker.g.cs").SourceText.ToString();
        await Assert.That(fakerSource).Contains("partial class CredentialsFaker : global::Forged.Core.Faker<Credentials>");
    }

    private const string MissingRequiredMemberSource = """
        using Forged.Core;

        namespace Forged.Tests.SourceGen;

        [Fake]
        public sealed class Account
        {
            public required string Owner { get; set; }
            public string? Nickname { get; set; }
        }

        public static class Usage
        {
            public static AccountFaker Create() => new();
        }
        """;

    [Test]
    public async Task OmittingARequiredGenerator_ProducesACompilerError()
    {
        RunGenerator(MissingRequiredMemberSource, out _, out var diagnostics);

        var errors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        await Assert.That(errors.Select(d => d.Id)).Contains("CS9035");
    }

    private const string MissingOptionalMemberSource = """
        using Forged.Core;

        namespace Forged.Tests.SourceGen;

        [Fake]
        public sealed class Profile
        {
            public required string Name { get; set; }
            public string? Bio { get; set; }
        }

        public static class Usage
        {
            public static ProfileFaker Create() => new() { Name = null! };
        }
        """;

    [Test]
    public async Task OmittingAnOptionalGenerator_CompilesCleanly()
    {
        RunGenerator(MissingOptionalMemberSource, out _, out var diagnostics);

        var errors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        await Assert.That(errors).IsEmpty();
    }

    [Test]
    public async Task GeneratedFaker_IsInstantiableAtRuntime()
    {
        RunGenerator(PersonSource, out var compilation, out var diagnostics);
        await Assert.That(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)).IsEmpty();

        using var stream = new MemoryStream();
        var emit = compilation.Emit(stream);
        await Assert.That(emit.Success).IsTrue();

        var assembly = Assembly.Load(stream.ToArray());
        var usageType = assembly.GetType("Forged.Tests.SourceGen.PersonUsage", throwOnError: true)!;
        var faker = usageType.GetMethod("Create")!.Invoke(null, null)!;
        var person = faker.GetType().GetMethod("Get", Type.EmptyTypes)!.Invoke(faker, null)!;
        var personType = person.GetType();

        await Assert.That(personType.GetProperty("FirstName")!.GetValue(person)).IsEqualTo("Alice");
        await Assert.That(personType.GetProperty("LastName")!.GetValue(person)).IsEqualTo("Smith");
        await Assert.That(personType.GetProperty("Age")!.GetValue(person)).IsEqualTo(0);
        await Assert.That(personType.GetProperty("Email")!.GetValue(person)).IsNull();
    }

    private sealed class ForgedGeneratorProxy(IIncrementalGenerator generator) : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
            => generator.Initialize(context);
    }
}