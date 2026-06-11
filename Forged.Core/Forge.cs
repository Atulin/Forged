using Forged.Core.Modules;

namespace Forged.Core;

public sealed class Forge
{
    public Random Rng { get; }
    public ForgeRandom Random { get; }
    public ForgeTemporal Temporal { get; }
    public ForgeText Text { get; }
    
    public int Next(int min, int max) => Rng.Next(min, max);
    public bool Coinflip() => Rng.Next(0, 2) == 1;
    
    
    public Forge(Random? random = null)
    {
        Rng = random ?? System.Random.Shared;
        Random = new ForgeRandom(this);
        Temporal = new ForgeTemporal(this);
        Text = new ForgeText(this);
    }
}
