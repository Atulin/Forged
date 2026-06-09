namespace Forged.Core.Generators.Utility;

public class RefineGenerator<T>(Generator<T> innerGenerator, Func<T, T> refiner) : Generator<T>
{
	public override T Generate()
	{
		var value = innerGenerator.Generate();
		return refiner(value);
	}
}