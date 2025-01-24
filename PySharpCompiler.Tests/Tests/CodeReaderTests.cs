using PySharpCompiler.Classes;
using PySharpCompiler.Components;
using System.IO;
using System.Text;

namespace PySharpCompiler.Tests.CodeReaderTests;

public class CodeReaderTests
{

    [Fact]
    public void TestPosition()
    {
        StreamReader reader = GetStream("abc\nhello\nthis");

        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        Assert.Equivalent(new Position(1, 1), codeReader.GetPosition());

        char currentChar = codeReader.CurrentChar();

        while (currentChar != 't')
        {
            codeReader.NextChar();
            currentChar = codeReader.CurrentChar();
        }

        Assert.Equivalent(new Position (3, 1), codeReader.GetPosition());
        codeReader.NextChar();
        Assert.Equivalent(new Position (3, 2), codeReader.GetPosition());
    }

    [Fact]
    public void TestPositionWithNewlineOverride()
    {
        StreamReader reader = GetStream("abc\r\nhello\nthis");
        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        char currentChar = codeReader.CurrentChar();

        while (currentChar != 'h')
        {
            codeReader.NextChar();
            currentChar = codeReader.CurrentChar();
        }

        Assert.Equivalent(new Position (2, 1), codeReader.GetPosition());
    }

    [Fact]
    public void TestPositionMultipleNewlines()
    {
        StreamReader reader = GetStream("\n\n\n\n\n\nhello");
        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        char currentChar = codeReader.CurrentChar();

        while (currentChar != 'h')
        {
            codeReader.NextChar();
            currentChar = codeReader.CurrentChar();
        }

        Assert.Equivalent(new Position (7, 1), codeReader.GetPosition());
    }

    [Fact]
    public void TestEOF()
    {
        StreamReader reader = GetStream("a");

        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        Assert.Equal('a', codeReader.CurrentChar());

        codeReader.NextChar();

        Assert.Equal('\0', codeReader.CurrentChar());
    }

    
    [Fact]
    public void TestSimpleNewline()
    {
        StreamReader reader = GetStream("\n");

        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        Assert.Equal('\n', codeReader.CurrentChar());
    }

    [Fact]
    public void TestWindowsNewline()
    {
        StreamReader reader = GetStream("\r\n");

        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        Assert.Equal('\n', codeReader.CurrentChar());
    }

    [Fact]
    public void TestSingleRNewline()
    {
        StreamReader reader = GetStream("\r");

        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        Assert.Equal('\n', codeReader.CurrentChar());
    }

    [Fact]
    public void TestNRNewline() {
        StreamReader reader = GetStream("\n\r");

        CodeReader codeReader = new CodeReader(reader);
        codeReader.NextChar();

        Assert.Equal('\n', codeReader.CurrentChar());
    }

    public StreamReader GetStream(string testContent)
    {
        byte[] mockFile = Encoding.UTF8.GetBytes(testContent);
        var memorySteam = new MemoryStream(mockFile);
        var stream = new StreamReader(memorySteam);
        return stream;
    }
}