using Forged.Core;
using Forged.Core.Generators;

var f = new Foo
{
	Name = Foo.Text.Alphanumeric(10)
	//     ^^^ I really thought I can avoid this 😭
};

return;

internal class Foo : Forged<Foo>
{
	public required Generator<string> Name { get; set; }

}