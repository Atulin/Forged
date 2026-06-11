namespace Forged.Core;

/// <summary>
/// Base class for creating fake data models. Inherit from this class to define custom model generators.
/// </summary>
/// <typeparam name="TModel">The type of model to generate.</typeparam>
/// <param name="random">The random number generator to use for generating data.</param>
public abstract class Faker<TModel>(Random? random = null)
{
    /// <summary>
    /// Gets the underlying <see cref="Forge"/> instance used for generating data.
    /// </summary>
    public Forge Forge { get; } = new(random);

    /// <summary>
    /// Generates a single fake model instance.
    /// </summary>
    /// <returns>A single fake model instance.</returns>
    public abstract TModel Get();
    
    /// <summary>
    /// Generates multiple fake model instances.
    /// </summary>
    /// <param name="count">The number of instances to generate.</param>
    /// <returns>An enumerable of fake model instances.</returns>
    public abstract IEnumerable<TModel> Get(int count);
}
