using PySharpCompiler.Classes;
using PySharpCompiler.Components;
using System.Text;

namespace PySharpCompiler.Tests.LexerTests;

public class LexerTests
{
    [Fact]
    public void PlusOperatorTest()
    {
        CodeReader reader = GetCodeReader("+");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Plus,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void MinusOperatorTest()
    {
        CodeReader reader = GetCodeReader("-");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Minus,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void TypeArrowOperatorTest()
    {
        CodeReader reader = GetCodeReader("->");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.TypeArrow,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void MultiplyOperatorTest()
    {
        CodeReader reader = GetCodeReader("*");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Multiply,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void DivideOperatorTest()
    {
        CodeReader reader = GetCodeReader("/");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Divide,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void IsEqualOperatorTest()
    {
        CodeReader reader = GetCodeReader("==");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.IsEqual,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void AssignOperatorTest()
    {
        CodeReader reader = GetCodeReader("=");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Assign,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void NegateOperatorTest()
    {
        CodeReader reader = GetCodeReader("!");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Negate,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void IsNotEqualOperatorTest()
    {
        CodeReader reader = GetCodeReader("!=");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.IsNotEqual,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void MoreThanOperatorTest()
    {
        CodeReader reader = GetCodeReader(">");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.MoreThan,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void MoreOrEqualOperatorTest()
    {
        CodeReader reader = GetCodeReader(">=");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.MoreOrEqual,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void LessThanOperatorTest()
    {
        CodeReader reader = GetCodeReader("<");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.LessThan,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void LessOrEqualOperatorTest()
    {
        CodeReader reader = GetCodeReader("<=");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.LessOrEqual,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void BindFrontOperatorTest()
    {
        CodeReader reader = GetCodeReader("%");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.BindFront,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void ListLengthOperatorTest()
    {
        CodeReader reader = GetCodeReader("|");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.ListLength,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void PipeOperatorTest()
    {
        CodeReader reader = GetCodeReader("|>");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Pipe,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void SemicolonTest()
    {
        CodeReader reader = GetCodeReader(";");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Semicolon,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void OpenSquareTest()
    {
        CodeReader reader = GetCodeReader("[");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.SquareOpen,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void SquareCloseTest()
    {
        CodeReader reader = GetCodeReader("]");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.SquareClose,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void RoundOpenTest()
    {
        CodeReader reader = GetCodeReader("(");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.RoundOpen,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void RoundCloseTest()
    {
        CodeReader reader = GetCodeReader(")");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.RoundClose,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void CurlyOpenTest()
    {
        CodeReader reader = GetCodeReader("{");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.CurlyOpen,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void CurlyCloseTest()
    {
        CodeReader reader = GetCodeReader("}");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.CurlyClose,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void CommaTest() {
        CodeReader reader = GetCodeReader(",");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Comma,
            value = null
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void BoolTest()
    {
        CodeReader reader = GetCodeReader("bool");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.BoolType, lexer.GetCurrentToken().type);
    }
    [Fact]
    public void IntTypeTest()
    {
        CodeReader reader = GetCodeReader("int");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.IntType, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void FloatTypeTest()
    {
        CodeReader reader = GetCodeReader("float");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.FloatType, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void StringTypeTest()
    {
        CodeReader reader = GetCodeReader("string");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.StringType, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void FunctionTypeTest()
    {
        CodeReader reader = GetCodeReader("function");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.FunctionType, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void ListTypeTest()
    {
        CodeReader reader = GetCodeReader("list");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.ListType, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void MutTest()
    {
        CodeReader reader = GetCodeReader("mut");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.Mutable, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void TrueTest()
    {
        CodeReader reader = GetCodeReader("true");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.True, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void FalseTest()
    {
        CodeReader reader = GetCodeReader("false");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.False, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void ReturnTest()
    {
        CodeReader reader = GetCodeReader("return");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.Return, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void WhileTest()
    {
        CodeReader reader = GetCodeReader("while");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.While, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void OrTest()
    {
        CodeReader reader = GetCodeReader("or");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.Or, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void AndTest()
    {
        CodeReader reader = GetCodeReader("and");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.And, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void IfTest()
    {
        CodeReader reader = GetCodeReader("if");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.If, lexer.GetCurrentToken().type);
    }

    [Fact]
    public void ElseTest()
    {
        CodeReader reader = GetCodeReader("else");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(TokenType.Else, lexer.GetCurrentToken().type);
    }

    public CodeReader GetCodeReader(string testContent)
    {
        byte[] mockFile = Encoding.UTF8.GetBytes(testContent);
        var memorySteam = new MemoryStream(mockFile);
        var streamReader = new StreamReader(memorySteam);
        CodeReader codeReader = new CodeReader(streamReader);
        return codeReader;
    }

    [Fact]
    public void StringTest()
    {
        CodeReader reader = GetCodeReader("'ab\\'c'");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.String,
            value = "ab'c"
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void StringEscapeSymbolTest()
    {
        CodeReader reader = GetCodeReader("'\\nHello\\tWorld'");

        Lexer lexer = new Lexer(reader);

        var token = lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.String,
            value = "\nHello\tWorld"
        }, token);
    }

    [Fact]
    public void IntTest()
    {
        CodeReader reader = GetCodeReader("2241");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Int,
            value = 2241
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void ZeroIntTest()
    {
        CodeReader reader = GetCodeReader("0");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Int,
            value = 0
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void UnnecessaryZeroesIntTest()
    {
        CodeReader reader = GetCodeReader("000123");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Int,
            value = 123
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void IntTooBigTest()
    {
        CodeReader reader = GetCodeReader("170412491027012479");

        Lexer lexer = new Lexer(reader);

        Assert.Throws<Exception>(() => lexer.NextToken() );
    }

    [Fact]
    public void FloatTest()
    {
        CodeReader reader = GetCodeReader("0.00000303");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Float,
            value = 0.00000303
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void DotFloatTest()
    {
        CodeReader reader = GetCodeReader(".1");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Float,
            value = 0.1
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void DotFollowedFloatTest()
    {
        CodeReader reader = GetCodeReader("1. ");

        Lexer lexer = new Lexer(reader);

        lexer.NextToken();
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.Float,
            value = 1.0
        }, lexer.GetCurrentToken());
    }

    [Fact]
    public void FloatTooBigTest()
    {
        CodeReader reader = GetCodeReader(".24142121414212442141241241242114362412170412491027012479");

        Lexer lexer = new Lexer(reader);

        Assert.Throws<Exception>(() => lexer.NextToken());
    }

    [Fact]
    public void CodeTest()
    {
        CodeReader reader = GetCodeReader("int three = addition(1, 2);");

        Lexer lexer = new Lexer(reader);

        Assert.Equivalent(new Token
        {
            pos = new Position (1, 1),
            type = TokenType.IntType,
            value = null
        }, lexer.NextToken());

        lexer.NextToken();

        Assert.Equivalent(new Token
        {
            pos = new Position (1, 11),
            type = TokenType.Assign,
            value = null
        }, lexer.NextToken());

        Assert.Equivalent(new Token
        {
            pos = new Position (1, 13),
            type = TokenType.Identifier,
            value = "addition"
        }, lexer.NextToken());

        Assert.Equivalent(new Token
        {
            pos = new Position (1, 21),
            type = TokenType.RoundOpen,
            value = null
        }, lexer.NextToken());

        Assert.Equivalent(new Token
        {
            pos = new Position (1, 22),
            type = TokenType.Int,
            value = 1
        }, lexer.NextToken());
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 23),
            type = TokenType.Comma,
            value = null
        }, lexer.NextToken());
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 25),
            type = TokenType.Int,
            value = 2
        }, lexer.NextToken());
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 26),
            type = TokenType.RoundClose,
            value = null
        }, lexer.NextToken());
        Assert.Equivalent(new Token
        {
            pos = new Position (1, 27),
            type = TokenType.Semicolon,
            value = null
        }, lexer.NextToken());
    }
}