using System.Globalization;
using Forged.Core.Core;
using Forged.Core.Modules;

namespace Forged.Core;

/// <summary>
/// The main entry point for generating fake data. Provides access to all generator modules.
/// </summary>
public sealed class Forge
{
    /// <summary>
    /// Gets the underlying random number generator.
    /// </summary>
    public Random Rng { get; }

    /// <summary>
    /// Gets or sets the locale used for generating localized data.
    /// </summary>
    public CultureInfo Locale { get; }
    
    internal FileLoader FileLoader { get; }
    
    /// <summary>
    /// Gets the random data generation module.
    /// </summary>
    public ForgeRandom Random { get; }
    
    /// <summary>
    /// Gets the temporal (date/time) generation module.
    /// </summary>
    public ForgeTemporal Temporal { get; }
    
    /// <summary>
    /// Gets the text generation module.
    /// </summary>
    public ForgeText Text { get; }

    /// <summary>
    /// Gets the module for generating basic random or deterministic data.
    /// </summary>
    public ForgeBasic Basic { get; }

    /// <summary>
    /// Gets the module for generating person-related data.
    /// </summary>
    public ForgePerson Person { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Forge"/> class.
    /// </summary>
    /// <param name="random">The random number generator to use. If null, <see cref="System.Random.Shared"/> is used.</param>
    /// <param name="locale">The locale to use for generating locale-specific data. If null, <see cref="CultureInfo.InvariantCulture"/> is used.</param>
    public Forge(Random? random, CultureInfo? locale)
    {
	    Locale = locale ?? CultureInfo.InvariantCulture;
	    Rng = random ?? System.Random.Shared;
	    FileLoader = new FileLoader();
	    Random = new ForgeRandom(this);
	    Temporal = new ForgeTemporal(this);
	    Text = new ForgeText(this);
        Basic = new ForgeBasic(this);
        Person = new ForgePerson(this);
    }
}
