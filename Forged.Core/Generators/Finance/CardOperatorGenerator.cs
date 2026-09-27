using Forged.Core.Core;

namespace Forged.Core.Generators.Finance;

/// <summary>
/// A specialized generator dedicated to producing instances of card operators.
/// Combines enumeration filtering and random selection to generate credit card operators based on the given configuration.
/// </summary>
public sealed class CardOperatorGenerator(CardOperator operators, Forge forge) : Generator<CardOperator>(forge)
{
	private readonly CardOperator[] _specs = [.. CardOperatorCatalog.ResolveOperator(operators)];

	public override CardOperator Generate() => Rng.GetItem(_specs);
}