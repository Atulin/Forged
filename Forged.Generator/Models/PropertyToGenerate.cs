namespace Forged.Generator.Models;

public record PropertyToGenerate(
	string Name,
	string Type, 
	bool IsRequired
);