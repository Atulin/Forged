namespace Forged.Generator.Models;

public sealed record TypeToGenerate(
	string Namespace, 
	string Name, 
	EquatableReadOnlyList<PropertyToGenerate> Properties
);