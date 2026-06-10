namespace Forged.Core.Generators;

/// <summary>
/// Covariant read-only view of a generator. Allows Generator&lt;Derived&gt; to be
/// used where IGenerator&lt;Base&gt; is expected (e.g. reference-type upcasting).
/// </summary>
public interface IGenerator<out T>
{
	T Generate();
}
