using Forged.Generator.Helpers;

namespace Forged.Tests;

public class IndentedWriterTests
{
    [Test]
    public async Task EmptyWriter_ProducesEmptyString()
    {
        var writer = new IndentedWriter();
        await Assert.That(writer.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Write_WithNoIndent_OutputsRawText()
    {
        var writer = new IndentedWriter();
        writer.Write("a\nb\nc");
        await Assert.That(writer.ToString()).IsEqualTo("a\nb\nc");
    }

    [Test]
    public async Task WriteLine_AppendsTrailingNewline()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("hello");
        await Assert.That(writer.ToString()).IsEqualTo("hello\n");
    }

    [Test]
    public async Task WriteLine_NoArguments_WritesJustANewline()
    {
        var writer = new IndentedWriter();
        writer.Write("a").WriteLine().Write("b");
        await Assert.That(writer.ToString()).IsEqualTo("a\nb");
    }

    [Test]
    public async Task Indent_IndentsSubsequentLines()
    {
        var writer = new IndentedWriter();
        writer.Indent();
        writer.WriteLine("a");
        writer.WriteLine("b");
        writer.Dedent();
        writer.WriteLine("c");

        await Assert.That(writer.ToString()).IsEqualTo("\ta\n\tb\nc\n");
    }

    [Test]
    public async Task Write_MultiLineText_IndentsEveryLine()
    {
        var writer = new IndentedWriter();
        writer.Indent();
        writer.Write("a\nb\nc");
        await Assert.That(writer.ToString()).IsEqualTo("\ta\n\tb\n\tc");
    }

    [Test]
    public async Task Write_LeadingNewline_IsLeftBlankAndIndentsFollowingContent()
    {
        var writer = new IndentedWriter();
        writer.Indent();
        writer.Write("\na");
        await Assert.That(writer.ToString()).IsEqualTo("\n\ta");
    }

    [Test]
    public async Task Indented_RestoresPreviousIndentLevel()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("a");
        writer.Indented(() => writer.WriteLine("b"));
        writer.WriteLine("c");

        await Assert.That(writer.ToString()).IsEqualTo("a\n\tb\nc\n");
    }

    [Test]
    public async Task Dedent_NeverGoesBelowZero()
    {
        var writer = new IndentedWriter();
        writer.Dedent().Dedent().Write("x");
        await Assert.That(writer.ToString()).IsEqualTo("x");
    }

    [Test]
    public async Task Block_IndentsTheBodyBetweenOpenAndClose()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("void M()");
        writer.Block(() => writer.WriteLine("body"));
        writer.WriteLine("end");

        await Assert.That(writer.ToString()).IsEqualTo("void M()\n{\n\tbody\n}\nend\n");
    }

    [Test]
    public async Task Block_CustomOpenAndCloseTokens_AreWrittenVerbatim()
    {
        var writer = new IndentedWriter();
        writer.Block(() => writer.WriteLine("body"), open: "namespace N {", close: "}");
        await Assert.That(writer.ToString()).IsEqualTo("namespace N {\n\tbody\n}\n");
    }

    [Test]
    public async Task InitBlock_ClosesWithSemicolon()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("var x = new Foo");
        writer.InitBlock(() => writer.WriteLine("A = 1"));

        await Assert.That(writer.ToString()).IsEqualTo("var x = new Foo\n{\n\tA = 1\n};\n");
    }

    [Test]
    public async Task BodyBlock_UsesPlainClosingBrace()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("class C");
        writer.BodyBlock(() => writer.WriteLine("// comment"));

        await Assert.That(writer.ToString()).IsEqualTo("class C\n{\n\t// comment\n}\n");
    }

    [Test]
    public async Task NestedBlocks_IncrementTheIndentPerLevel()
    {
        var writer = new IndentedWriter();
        writer.Block(() =>
        {
            writer.WriteLine("a");
            writer.Block(() => writer.WriteLine("b"), open: "namespace N {", close: "}");
        });

        await Assert.That(writer.ToString()).IsEqualTo("{\n\ta\n\tnamespace N {\n\t\tb\n\t}\n}\n");
    }

    [Test]
    public async Task FormattedWriteLine_OneArg_FormatsTheText()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("value: {0}", 42);
        await Assert.That(writer.ToString()).IsEqualTo("value: 42\n");
    }

    [Test]
    public async Task FormattedWriteLine_TwoArgs_FormatsTheText()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("{0}-{1}", "a", "b");
        await Assert.That(writer.ToString()).IsEqualTo("a-b\n");
    }

    [Test]
    public async Task FormattedWriteLine_ThreeArgs_FormatsTheText()
    {
        var writer = new IndentedWriter();
        writer.WriteLine("{0}{1}{2}", "a", "b", "c");
        await Assert.That(writer.ToString()).IsEqualTo("abc\n");
    }

    [Test]
    public async Task FluentCalls_ReturnTheSameInstance()
    {
        var writer = new IndentedWriter();
        var result = writer.WriteLine("a").Indent().WriteLine("b");

        await Assert.That(ReferenceEquals(result, writer)).IsTrue();
        await Assert.That(writer.ToString()).IsEqualTo("a\n\tb\n");
    }

    [Test]
    public async Task CustomInitialCapacity_DoesNotChangeOutput()
    {
        var writer = new IndentedWriter(16);
        writer.Write("hello");
        await Assert.That(writer.ToString()).IsEqualTo("hello");
    }
}