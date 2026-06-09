using Forged.Core.Generators;
using Forged.Core.Generators.Temporal;

namespace Forged.Core.Modules;

public sealed class ForgeTemporal(Forge forge)
{
	public Generator<DateTime> Between(DateTime? min = null, DateTime? max = null)
        => new DateTimeGenerator(min, max) { Rng = forge.Rng };

    public Generator<DateTime> Past(DateTime? earliest = null)
        => new DateTimeInPastGenerator(earliest) { Rng = forge.Rng };

    public Generator<DateTime> Future(DateTime? latest = null)
        => new DateTimeInFutureGenerator(latest) { Rng = forge.Rng };

    public Generator<DateOnly> DateBetween(DateOnly? min = null, DateOnly? max = null)
        => new DateOnlyGenerator(min, max) { Rng = forge.Rng };

    public Generator<DateOnly> DateInPast(DateOnly? earliest = null)
        => new DateOnlyInPastGenerator(earliest) { Rng = forge.Rng };

    public Generator<DateOnly> DateInFuture(DateOnly? latest = null)
        => new DateOnlyInFutureGenerator(latest) { Rng = forge.Rng };

    public Generator<TimeOnly> TimeBetween(TimeOnly? min = null, TimeOnly? max = null)
        => new TimeOnlyGenerator(min, max) { Rng = forge.Rng };
}
