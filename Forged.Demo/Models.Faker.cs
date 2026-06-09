using Forged.Core;
using Forged.Core.Generators;

namespace Forged.Demo;

// Simulated Source Generator Output
public class FooFaker(Random? random = null) : Faker<Foo>(random)
{
    public required Func<Forge, Generator<string>> Bar { get; init; }
    public Func<Forge, Generator<int?>>? Baz { get; init; }
    public required Func<Forge, Generator<bool?>> Quz { get; init; }
    public required Func<Forge, Generator<DateTime>> Timestamp { get; init; }

    private Generator<string>? _barGenerator;
    private Generator<int?>? _bazGenerator;
    private Generator<bool?>? _quzGenerator;
    private Generator<DateTime>? _timestampGenerator;

    public override Foo Get()
    {
        _barGenerator ??= Bar(Forge);
        _bazGenerator ??= Baz?.Invoke(Forge);
        _quzGenerator ??= Quz(Forge);
        _timestampGenerator ??= Timestamp(Forge);

        return new Foo
        {
            Bar = _barGenerator.Generate(),
            Baz = _bazGenerator?.Generate(),
            Quz = _quzGenerator.Generate(),
            Timestamp = _timestampGenerator.Generate(),
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
