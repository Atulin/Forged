namespace Forged.Core;

public abstract class Faker<TModel>(Random? random = null)
{
    public Forge Forge { get; } = new(random);

    public abstract TModel Get();
    
    public abstract IEnumerable<TModel> Get(int count);
}
