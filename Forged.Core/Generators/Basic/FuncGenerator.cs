namespace Forged.Core.Generators.Basic;

/// <summary>
/// Generates values by invoking a provided function.
/// </summary>
/// <typeparam name="T">The type of the generated value.</typeparam>
public sealed class FuncGenerator<T>(Func<T> func, Forge forge) : Generator<T>(forge)
{
	/// <summary>
	/// Generates a value by invoking the function.
	/// </summary>
	public override T Generate() => func();
}
