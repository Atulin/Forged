using Forged.Core.Modules;

namespace Forged.Core;

public sealed class Forge
{
    public Random Rng { get; }
    public ForgeRandom Random { get; }
    public ForgeTemporal Temporal { get; }
    public ForgeText Text { get; }

    public Forge(Random? random = null)
    {
        Rng = random ?? System.Random.Shared;
        Random = new ForgeRandom(this);
        Temporal = new ForgeTemporal(this);
        Text = new ForgeText(this);
    }
}
