using Forged.Core.Generators;
using Forged.Core.Generators.Temporal;

namespace Forged.Core;

public partial class Forged<TModel>
{
	public static class Temporal
	{
		public static Generator<DateTime> Between(DateTime? min = null, DateTime? max = null)
			=> new DateTimeGenerator(min, max);

		public static Generator<DateTime> Past(DateTime? earliest = null)
			=> new DateTimeInPastGenerator(earliest);

		public static Generator<DateTime> Future(DateTime? latest = null)
			=> new DateTimeInFutureGenerator(latest);

		public static Generator<DateOnly> DateBetween(DateOnly? min = null, DateOnly? max = null)
			=> new DateOnlyGenerator(min, max);

		public static Generator<DateOnly> DateInPast(DateOnly? earliest = null)
			=> new DateOnlyInPastGenerator(earliest);

		public static Generator<DateOnly> DateInFuture(DateOnly? latest = null)
			=> new DateOnlyInFutureGenerator(latest);

		public static Generator<TimeOnly> TimeBetween(TimeOnly? min = null, TimeOnly? max = null)
			=> new TimeOnlyGenerator(min, max);
	}
}
