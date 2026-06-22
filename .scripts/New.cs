#:package Spectre.Console@0.57.0

using System.Collections.Immutable;
using Spectre.Console;

var isUtility = AnsiConsole.Confirm("Is this a utility generator?", false);

var modules = Directory.GetDirectories(isUtility ? "Forged.Core/Generators/Utility" : "Forged.Core/Generators")
	.Select(p => p.Split('/', '\\')[^1])
	.ToImmutableSortedSet(StringComparer.OrdinalIgnoreCase);

var chosenModule = AnsiConsole.Prompt(
	new SelectionPrompt<string>()
		.Title("Select the module to add the generator to:")
		.AddChoices([..modules, "Other"]));

var newModule = false;
if (chosenModule == "Other")
{
	chosenModule = AnsiConsole.Ask<string>("What module should the generator be in?");
	newModule = true;
}

var chosenName = AnsiConsole.Ask<string>("What should the generator be called?");

if (newModule)
{
	await File.WriteAllTextAsync($"Forged.Core/Modules/Forge{chosenModule}.cs", ForgeTemplate(chosenModule, chosenName));
}

if (isUtility)
{
	Directory.CreateDirectory($"Forged.Core/Generators/Utility/{chosenModule}");
	await File.WriteAllTextAsync($"Forged.Core/Generators//Utility/{chosenModule}/{chosenName}Generator.cs", UtilityTemplate(chosenModule, chosenName));
}
else
{
	Directory.CreateDirectory($"Forged.Core/Generators/{chosenModule}");
	await File.WriteAllTextAsync($"Forged.Core/Generators/{chosenModule}/{chosenName}Generator.cs", Template(chosenModule, chosenName));
}
return;

string Template(string module, string name) => $$"""
     namespace Forged.Core.Generators.{{module}};
     
     /// <summary>
     /// </summary>
     public sealed class {{name}}Generator<T>(Forge forge) : Generator<T>(forge)
     {
        public override T Generate() => throw new NotImplementedException();
     }
     """;

string UtilityTemplate(string module, string name) => $$"""
     namespace Forged.Core.Generators.Utility.{{module}};

     /// <summary>
     /// </summary>
     public sealed class {{name}}Generator<T>(IGenerator<T> innerGenerator, Forge forge) : Generator<T>(forge)
     {
        public override T Generate() => throw new NotImplementedException();
     }
     """;

string ForgeTemplate(string module, string name) => $$"""
	using Forged.Core.Generators;
	using Forged.Core.Generators.{{module}};
	
	namespace Forged.Core.Modules;
	
	public sealed class Forge{{module}}(Forge forge)
	{
		/// <summary>
		/// </summary>
		public Generator<T> {{name}}<T>()
			=> new {{name}}Generator<T>(forge.Rng);
	}
	""";
                     