using System.Globalization;
using Forged.Core.Modules;

namespace Forged.Core;

public sealed class GenerationContext<TModel>
{
    public GenerationContext(Forge forge)
    {
        Forge = forge ?? throw new ArgumentNullException(nameof(forge));
    }

    public Forge Forge { get; }
    public Random Rng => Forge.Rng;
    public CultureInfo Locale => Forge.Locale;
    public ForgeRandom Random => Forge.Random;
    public ForgeTemporal Temporal => Forge.Temporal;
    public ForgeText Text => Forge.Text;
    public ForgeBasic Basic => Forge.Basic;
    public ForgeInternet Internet => Forge.Internet;
    public ForgePerson Person => Forge.Person;
    public ForgeNetwork Network
    {
        get => Forge.Network;
        set => Forge.Network = value;
    }
}
