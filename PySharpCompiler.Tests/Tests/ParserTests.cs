using Newtonsoft.Json.Linq;
using PySharpCompiler.Classes;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Expressions.Operators;
using PySharpCompiler.Classes.Statements;
using PySharpCompiler.Components;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using static System.Net.WebRequestMethods;

namespace PySharpCompiler.Tests.ParserTests;

public class ParserTests
{

    // FACTORS

    [Fact]
    public void TestParseIdentifier()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "identifier" },
            new Token { type = TokenType.EOF, pos = new Position(2, 1)}]);
        var parser = new Parser(new TestLexer(tokens));

        IdentifierExpression? identifier = (IdentifierExpression)parser.ParseIdentifier()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new IdentifierExpression("identifier", new Position(1, 1)), identifier);


        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        identifier = (IdentifierExpression)parser.ParseFactor()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new IdentifierExpression("identifier", new Position(1, 1)), identifier);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        identifier = (IdentifierExpression)parser.ParseIdentifier()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new IdentifierExpression("identifier", new Position(1, 1)), identifier);
    }

    [Fact]
    public void TestParseFunCallExpression()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "identifier" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 9)},
            new Token { type = TokenType.Identifier, pos = new Position(1, 10), value = "abc" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 14) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        FunctionCallExpression? identifier = (FunctionCallExpression)parser.ParseIdentifier()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new FunctionCall("identifier",
            new List<IExpression>([new IdentifierExpression("abc", new Position(1, 10))]), new Position(1, 9)), identifier);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        identifier = (FunctionCallExpression)parser.ParseFactor()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new FunctionCallExpression("identifier",
            new List<IExpression>([new IdentifierExpression("abc", new Position(1, 10))]), new Position(1, 9)), identifier);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        identifier = (FunctionCallExpression)parser.ParseExpression()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new FunctionCallExpression("identifier",
            new List<IExpression>([new IdentifierExpression("abc", new Position(1, 10))]), new Position(1, 9)), identifier);
    }

    [Fact]
    public void TestParseFunCallExpressionMoreArgs()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "identifier" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 9) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 10), value = "abc" },
            new Token { type = TokenType.Comma, pos = new Position(1, 14) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 15), value = "def" },
            new Token { type = TokenType.Comma, pos = new Position(1, 19) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 20), value = "ghi" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 24) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        FunctionCallExpression? identifier = (FunctionCallExpression)parser.ParseIdentifier()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new FunctionCallExpression("identifier",
            new List<IExpression>([new IdentifierExpression("abc", new Position(1, 10)),
                new IdentifierExpression("def", new Position(1, 15)),
                new IdentifierExpression("ghi", new Position(1, 20))]), new Position(1, 9)), identifier);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        identifier = (FunctionCallExpression)parser.ParseFactor()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new FunctionCallExpression("identifier",
            new List<IExpression>([new IdentifierExpression("abc", new Position(1, 10)),
                new IdentifierExpression("def", new Position(1, 15)),
                new IdentifierExpression("ghi", new Position(1, 20))]), new Position(1, 9)), identifier);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        identifier = (FunctionCallExpression)parser.ParseExpression()!;

        Assert.NotNull(identifier);
        Assert.Equivalent(new FunctionCallExpression("identifier",
            new List<IExpression>([new IdentifierExpression("abc", new Position(1, 10)),
                new IdentifierExpression("def", new Position(1, 15)),
                new IdentifierExpression("ghi", new Position(1, 20))]), new Position(1, 9)), identifier);
    }

    [Fact]
    public void TestParseFloat()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Float, pos = new Position(1, 1), value = 142.421 },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        FloatExpression? expr = parser.ParseFloat()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new FloatExpression(142.421, new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (FloatExpression)parser.ParseFactor()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new FloatExpression(142.421, new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        expr = (FloatExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new FloatExpression(142.421, new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseInt()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Int, pos = new Position(1, 1), value = 42 },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        IntegerExpression? expr = parser.ParseInteger()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new IntegerExpression(42, new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (IntegerExpression)parser.ParseFactor()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new IntegerExpression(42, new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        expr = (IntegerExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new IntegerExpression(42, new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseString()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.String, pos = new Position(1, 1), value = "test" },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        StringExpression? expr = parser.ParseString()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new StringExpression("test", new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (StringExpression)parser.ParseFactor()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new StringExpression("test", new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        expr = (StringExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new StringExpression("test", new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseBool()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.True, pos = new Position(1, 1)},
            new Token { type = TokenType.False, pos = new Position(1, 1) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        BooleanExpression? expr = parser.ParseBool()!;

        Assert.Equivalent(new BooleanExpression(true, new Position(1, 1)), expr);

        expr = parser.ParseBool()!;
        Assert.Equivalent(new BooleanExpression(false, new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (BooleanExpression)parser.ParseFactor()!;

        Assert.Equivalent(new BooleanExpression(true, new Position(1, 1)), expr);

        expr = parser.ParseBool()!;
        Assert.Equivalent(new BooleanExpression(false, new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        expr = (BooleanExpression)parser.ParseExpression()!;

        Assert.Equivalent(new BooleanExpression(true, new Position(1, 1)), expr);

        expr = parser.ParseBool()!;
        Assert.Equivalent(new BooleanExpression(false, new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseEmptyList()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.SquareOpen, pos = new Position(1, 1) },
            new Token { type = TokenType.SquareClose, pos = new Position(1, 2) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        ListExpression? expr = parser.ParseListExpression()!;

        Assert.Equivalent(new ListExpression(new List<IExpression>([]), new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (ListExpression)parser.ParseFactor()!;

        Assert.Equivalent(new ListExpression(new List<IExpression>([]), new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens) );

        expr = (ListExpression)parser.ParseExpression()!;

        Assert.Equivalent(new ListExpression(new List<IExpression>([]), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseList()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.SquareOpen, pos = new Position(1, 1) },
            new Token { type = TokenType.Int, pos = new Position(1, 2), value=1 },
            new Token { type = TokenType.Comma, pos = new Position(1, 3) },
            new Token { type = TokenType.Int, pos = new Position(1, 4), value = 2 },
            new Token { type = TokenType.SquareClose, pos = new Position(1, 5) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        ListExpression? expr = parser.ParseListExpression()!;

        Assert.Equivalent(new ListExpression(new List<IExpression>([
                new IntegerExpression(1, new Position(1, 2)),
            new IntegerExpression(2, new Position(1, 4))
        ]), new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (ListExpression)parser.ParseFactor()!;

        Assert.Equivalent(new ListExpression(new List<IExpression>([
                new IntegerExpression(1, new Position(1, 2)),
            new IntegerExpression(2, new Position(1, 4))
        ]), new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        expr = (ListExpression)parser.ParseExpression()!;

        Assert.Equivalent(new ListExpression(new List<IExpression>([
                new IntegerExpression(1, new Position(1, 2)),
            new IntegerExpression(2, new Position(1, 4))
        ]), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseListLength()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.ListLength, pos = new Position(1, 1) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 2), value = "abc"},
            new Token { type = TokenType.ListLength, pos = new Position(1, 5) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        ListLengthExpression? expr = parser.ParseListLength()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new ListLengthExpression("abc", new Position(1, 1)), expr);

        // With ParseFactor
        parser = new Parser(new TestLexer(tokens));

        expr = (ListLengthExpression)parser.ParseFactor()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new ListLengthExpression("abc", new Position(1, 1)), expr);

        // With ParseExpression
        parser = new Parser(new TestLexer(tokens));

        expr = (ListLengthExpression)parser.ParseListLength()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new ListLengthExpression("abc", new Position(1, 1)), expr);
    }

    // Complex expressions
    [Fact]
    public void TestParseUnary()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Negate, pos = new Position(1, 1) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 2), value = "cond" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 7) }]);
        var parser = new Parser(new TestLexer(tokens));

        MyUnaryExpression? expr = (MyUnaryExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new MyUnaryExpression(OperatorType.Negate, new IdentifierExpression("cond", new Position(1, 2)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseMultiplication()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Minus, pos = new Position(1, 1)},
            new Token { type = TokenType.Int, pos = new Position(1, 2), value = 1 },
            new Token { type = TokenType.Multiply, pos = new Position(1, 4) },
            new Token { type = TokenType.Int, pos = new Position(1, 6), value=10 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 8) }]);
        var parser = new Parser(new TestLexer(tokens));

        MultiplicativeExpression? expr = (MultiplicativeExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new MultiplicativeExpression(new MyUnaryExpression(OperatorType.Minus, new IntegerExpression(1, new Position(1, 2)), new Position(1,1)),
            OperatorType.Multiply, new IntegerExpression(10, new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseDivision()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Int, pos = new Position(1, 2), value = 4 },
            new Token { type = TokenType.Divide, pos = new Position(1, 4) },
            new Token { type = TokenType.Int, pos = new Position(1, 6), value = 2 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 8) }]);
        var parser = new Parser(new TestLexer(tokens));

        MultiplicativeExpression? expr = (MultiplicativeExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new MultiplicativeExpression(new IntegerExpression(4, new Position(1, 2)),
            OperatorType.Divide, new IntegerExpression(2, new Position(1, 6)), new Position(1, 2)), expr);
    }

    [Fact]
    public void TestParseBindFront()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "fun" },
            new Token { type = TokenType.BindFront, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "fun2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        MultiplicativeExpression? expr = (MultiplicativeExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new MultiplicativeExpression(new IdentifierExpression("fun", new Position(1, 1)),
            OperatorType.BindFront, new IdentifierExpression("fun2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParsePipe()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "fun" },
            new Token { type = TokenType.Pipe, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "fun2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        MultiplicativeExpression? expr = (MultiplicativeExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new MultiplicativeExpression(new IdentifierExpression("fun", new Position(1, 1)),
            OperatorType.Pipe, new IdentifierExpression("fun2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseMultiplicativeComplex()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "fun" },
            new Token { type = TokenType.BindFront, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value = "val" },
            new Token { type = TokenType.Divide, pos = new Position(1, 11)},
            new Token { type = TokenType.Int, pos = new Position(1, 14), value = 2 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 16) }]);
        var parser = new Parser(new TestLexer(tokens));

        MultiplicativeExpression? expr = (MultiplicativeExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new MultiplicativeExpression(
            new MultiplicativeExpression(
                new IdentifierExpression("fun", new Position(1, 1)),
                OperatorType.BindFront,
                new IdentifierExpression("val", new Position(1, 7)), new Position(1, 1)),
            OperatorType.Divide,
            new IntegerExpression(2, new Position(1, 14)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseAdditiveComplex()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Int, pos = new Position(1, 1), value = 3 },
            new Token { type = TokenType.Minus, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "obj" },
            new Token { type = TokenType.Plus, pos = new Position(1, 10) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 12), value = "val" },
            new Token { type = TokenType.Divide, pos = new Position(1, 16) },
            new Token { type = TokenType.Int, pos = new Position(1, 19), value = 2 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 21) }]);
        var parser = new Parser(new TestLexer(tokens));

        AdditiveExpression? expr = (AdditiveExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new AdditiveExpression(
            new AdditiveExpression(
                    new IntegerExpression(3, new Position(1, 1)),
                    OperatorType.Minus,
                    new IdentifierExpression("obj", new Position(1, 6)), new Position(1, 1)
                ),
            OperatorType.Plus,
            new MultiplicativeExpression(
                    new IdentifierExpression("val", new Position(1, 12)),
                    OperatorType.Divide,
                    new IntegerExpression(2, new Position(1, 19)), new Position(1, 12)
                ), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseMoreThan()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.MoreThan, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.MoreThan, new IdentifierExpression("val2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseMoreOrEqual()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.MoreOrEqual, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.MoreOrEqual, new IdentifierExpression("val2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseEqual()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.IsEqual, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.Equal, new IdentifierExpression("val2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseNotEqual()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.IsNotEqual, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.NotEqual, new IdentifierExpression("val2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseLessOrEqual()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.LessOrEqual, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.LessOrEqual, new IdentifierExpression("val2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseLessThan()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.LessThan, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.Less, new IdentifierExpression("val2", new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseRelationComplex()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "val" },
            new Token { type = TokenType.MoreThan, pos = new Position(1, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 6), value = "val2" },
            new Token { type = TokenType.Plus, pos = new Position(1, 11) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "val3" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 18) }]);
        var parser = new Parser(new TestLexer(tokens));

        RelationExpression? expr = (RelationExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new RelationExpression(new IdentifierExpression("val", new Position(1, 1)),
            OperatorType.MoreThan, new AdditiveExpression(
                    new IdentifierExpression("val2", new Position(1, 6)),
                    OperatorType.Plus,
                    new IdentifierExpression("val3", new Position(1, 13))
                , new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseConjunction()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.True, pos = new Position(1, 1) },
            new Token { type = TokenType.And, pos = new Position(1, 4) },
            new Token { type = TokenType.False, pos = new Position(1, 6) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        ConjunctionExpression? expr = (ConjunctionExpression)parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new ConjunctionExpression(new BooleanExpression(true, new Position(1, 1)),
            new BooleanExpression(false, new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestParseAlternative()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.True, pos = new Position(1, 1) },
            new Token { type = TokenType.Or, pos = new Position(1, 4) },
            new Token { type = TokenType.False, pos = new Position(1, 6) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        IExpression? expr = parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new AlternativeExpression(new BooleanExpression(true, new Position(1, 1)),
            new BooleanExpression(false, new Position(1, 6)), new Position(1, 1)), expr);
    }

    [Fact]
    public void TestNestedExpression()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 1) },
            new Token { type = TokenType.True, pos = new Position(1, 2) },
            new Token { type = TokenType.Or, pos = new Position(1, 5) },
            new Token { type = TokenType.False, pos = new Position(1, 7) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 12) },
            new Token { type = TokenType.And, pos = new Position(1, 14) },
            new Token { type = TokenType.False, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) }]);
        var parser = new Parser(new TestLexer(tokens));

        IExpression? expr = parser.ParseExpression()!;

        Assert.NotNull(expr);
        Assert.Equivalent(new ConjunctionExpression(new AlternativeExpression(
            new BooleanExpression(true, new Position(1, 2)), new BooleanExpression(false, new Position(1, 7)), new Position(1, 2)),
            new BooleanExpression(false, new Position(1, 18)), new Position(1, 1)), expr);
    }


    // Statements
    [Fact]
    public void TestFunDef()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Def, pos = new Position(1, 1)},
            new Token { type = TokenType.Identifier, pos = new Position(1, 5), value = "fnc" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 8) },
            new Token { type = TokenType.IntType, pos = new Position(1, 9)},
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "param" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 19) },
            new Token { type = TokenType.TypeArrow, pos = new Position(1, 20) },
            new Token { type = TokenType.IntType, pos = new Position(1, 24) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 26) },
            new Token { type = TokenType.Return, pos = new Position(2, 1) },
            new Token { type = TokenType.Identifier, pos = new Position(2, 7), value = "param" },
            new Token { type = TokenType.Semicolon, pos = new Position(2, 8) },
            new Token { type = TokenType.CurlyClose, pos = new Position(3, 1) },
            new Token { type = TokenType.Semicolon, pos = new Position(3, 2) },
            new Token { type = TokenType.EOF, pos = new Position(4, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        FunctionDefinition? funDef = parser.ParseFunctionDefinition();

        Assert.NotNull(funDef);

        Assert.Equal("fnc", funDef.Identifier);
        Assert.Equivalent(new PrimitiveType(DOMType.Int), funDef.ReturnType);
        Assert.Equivalent(new List<Param>([new Param(new PrimitiveType(DOMType.Int), "param", new Position(1, 9), false)]), funDef.Params);
        Assert.Equivalent(new List<IStatement>([new ReturnStatement(new IdentifierExpression("param", new Position(2, 7)), new Position(2, 1))]),
            funDef.Statements);
    }

    [Fact]
    public void TestFunDefNoOutputType()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Def, pos = new Position(1, 1) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 5), value = "fnc" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 8) },
            new Token { type = TokenType.IntType, pos = new Position(1, 9) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "param" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 20) },
            new Token { type = TokenType.Return, pos = new Position(2, 1) },
            new Token { type = TokenType.Identifier, pos = new Position(2, 7), value = "param" },
            new Token { type = TokenType.Semicolon, pos = new Position(2, 8) },
            new Token { type = TokenType.CurlyClose, pos = new Position(3, 1) },
            new Token { type = TokenType.Semicolon, pos = new Position(3, 2) },
            new Token { type = TokenType.EOF, pos = new Position(4, 1) }]);
        var parser = new Parser(new TestLexer(tokens));

        FunctionDefinition? funDef = parser.ParseFunctionDefinition();

        Assert.NotNull(funDef);

        Assert.Equal("fnc", funDef.Identifier);
        Assert.Equivalent(new  PrimitiveType(DOMType.Void), funDef.ReturnType);
        Assert.Equivalent(new List<Param>([new Param(new PrimitiveType(DOMType.Int), "param", new Position(1, 9), false)]), funDef.Params);
        Assert.Equivalent(new List<IStatement>([new ReturnStatement(new IdentifierExpression("param", new Position(2, 7)), new Position(2, 1))]),
            funDef.Statements);
    }

    [Fact]
    public void TestDeclaration()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Mutable, pos = new Position(1, 1) },
            new Token { type = TokenType.StringType, pos = new Position(1, 5)},
            new Token { type = TokenType.Identifier, pos = new Position(1, 12), value = "name" },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 18) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        Declaration? declaration = parser.ParseDeclaration();

        Assert.NotNull(declaration);
        Assert.True(declaration.IsMutable);
        Assert.Equal("name", declaration.Identifier);
        Assert.Equivalent(new PrimitiveType(DOMType.String) ,declaration.Type);
        Assert.Null(declaration.AssignedTo);
    }

    [Fact]
    public void TestDeclarationWithAssign()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Mutable, pos = new Position(1, 1) },
            new Token { type = TokenType.IntType, pos = new Position(1, 5) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 12), value = "value" },
            new Token { type = TokenType.Assign, pos = new Position(1, 18) },
            new Token { type = TokenType.Int, pos = new Position(1, 20), value = 21 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 22) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        Declaration? declaration = parser.ParseDeclaration();

        Assert.NotNull(declaration);
        Assert.True(declaration.IsMutable);
        Assert.Equal("value", declaration.Identifier);
        Assert.Equivalent(new PrimitiveType(DOMType.Int), declaration.Type);
        Assert.Equivalent(new IntegerExpression(21, new Position(1, 20)), declaration.AssignedTo);
    }

    [Fact]
    public void TestFunctionCall()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 5) },
            new Token { type = TokenType.Int, pos = new Position(1, 6), value = 2 },
            new Token { type = TokenType.Comma, pos = new Position(1, 7), },
            new Token { type = TokenType.Int, pos = new Position(1, 8), value = 2 },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 9)},
            new Token { type = TokenType.Semicolon, pos = new Position(1, 10)},
            new Token { type = TokenType.EOF, pos = new Position(1, 11) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        FunctionCall? call = (FunctionCall)parser.ParseAssignmentOrCall()!;

        Assert.NotNull(call);
        Assert.Equal("func", call.Identifier);
        Assert.Equivalent(new List<IExpression>([
            new IntegerExpression(2, new Position(1, 6)),
            new IntegerExpression(2, new Position(1, 8)),
        ]), call.Arguments);
    }

    [Fact]
    public void TestFunctionCallEmpty()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 5) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 6) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 7) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        FunctionCall? call = (FunctionCall)parser.ParseAssignmentOrCall()!;

        Assert.NotNull(call);
        Assert.Equal("func", call.Identifier);
        Assert.Equivalent(new List<IExpression>(), call.Arguments);
    }

    [Fact]
    public void TestAssign()
    {
        var tokens = new List<Token>([new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "one" },
            new Token { type = TokenType.Assign, pos = new Position(1, 2) },
            new Token { type = TokenType.Int, pos = new Position(1, 3), value = 1 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 4) },
            new Token { type = TokenType.EOF, pos = new Position(1, 5) }]);
        var parser = new Parser(new TestLexer(tokens));
        Assert.Equivalent(
            new Assignment(new Assignable("one", null, new Position(1, 1)), new IntegerExpression(1, new Position(1, 3)), new Position(1, 1)),
            parser.ParseAssignmentOrCall());
    }

    [Fact]
    public void TestAssignIndex()
    {
        var tokens = new List<Token>([new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "one" },
            new Token { type = TokenType.SquareOpen, pos = new Position(1, 4)},
            new Token { type = TokenType.Int, pos = new Position(1, 5), value = 1 },
            new Token { type = TokenType.SquareClose, pos = new Position(1, 6) },
            new Token { type = TokenType.Assign, pos = new Position(1, 7) },
            new Token { type = TokenType.Int, pos = new Position(1, 8), value = 1 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 9) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));
        Assert.Equivalent(
            new Assignment(new Assignable("one", new ListIndex(new IntegerExpression(1, new Position(1, 5)), null), new Position(1, 1)), new IntegerExpression(1, new Position(1, 8)), new Position(1, 1)),
            parser.ParseAssignmentOrCall());
    }
    [Fact]
    public void TestAssignMultipleIndex()
    {
        var tokens = new List<Token>([new Token { type = TokenType.Identifier, pos = new Position(1, 1), value = "one" },
            new Token { type = TokenType.SquareOpen, pos = new Position(1, 4) },
            new Token { type = TokenType.Int, pos = new Position(1, 5), value = 1 },
            new Token { type = TokenType.SquareClose, pos = new Position(1, 6) },
            new Token { type = TokenType.SquareOpen, pos = new Position(1, 7) },
            new Token { type = TokenType.Int, pos = new Position(1, 8), value = 2 },
            new Token { type = TokenType.SquareClose, pos = new Position(1, 9) },
            new Token { type = TokenType.Assign, pos = new Position(1, 10) },
            new Token { type = TokenType.Int, pos = new Position(1, 11), value = 1 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 12) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) }]);
        var parser = new Parser(new TestLexer(tokens));
        Assert.Equivalent(
            new Assignment(new Assignable("one", new ListIndex(new IntegerExpression(1, new Position(1,5)), new ListIndex(new IntegerExpression(2, new Position(1,8)),null)), new Position(1, 1)), new IntegerExpression(1, new Position(1, 11)), new Position(1, 1)),
            parser.ParseAssignmentOrCall());
    }

    [Fact]
    public void TestReturn()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.Return, pos = new Position(1, 1) },
            new Token { type = TokenType.Int, pos = new Position(1, 7), value=19 },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 9) },
            new Token { type = TokenType.EOF, pos = new Position(2, 1) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        ReturnStatement? statement = parser.ParseReturnStatement();

        Assert.NotNull(statement);
        Assert.Equivalent(new IntegerExpression(19, new Position(1, 7)), statement.ReturnedExpression);
    }

    [Fact]
    public void TestIfConditional()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.If, pos = new Position(1, 1) },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value = "cond" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 11) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 12) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 17) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 1) },
            new Token { type = TokenType.EOF, pos = new Position(2, 3) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        ConditionalStatement? statement = parser.ParseConditionalStatement();

        Assert.NotNull(statement);
        Assert.Equivalent(new IdentifierExpression("cond", new Position(1, 7)), statement.Condition);
        Assert.Equivalent(new List<IStatement>([
                new FunctionCall("func", new List<IExpression>(), new Position(1, 13))
            ]), statement.TrueBlock);
        Assert.Equivalent(null, statement.ElseBlock);
    }

    [Fact]
    public void TestConditional()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.If, pos = new Position(1, 1) },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value = "cond" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 11) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 12) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 17) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 1) },
            new Token { type = TokenType.Else, pos = new Position(2, 2) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(2, 4) },
            new Token { type = TokenType.Identifier, pos = new Position(2, 14), value = "func2" },
            new Token { type = TokenType.RoundOpen, pos = new Position(2, 18) },
            new Token { type = TokenType.RoundClose, pos = new Position(2, 19) },
            new Token { type = TokenType.Semicolon, pos = new Position(2, 20) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 21)},
            new Token { type = TokenType.EOF, pos = new Position(2, 3) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        ConditionalStatement? statement = parser.ParseConditionalStatement();

        Assert.NotNull(statement);
        Assert.Equivalent(new IdentifierExpression("cond", new Position(1, 7)), statement.Condition);
        Assert.Equivalent(new List<IStatement>([
                new FunctionCall("func", new List<IExpression>(), new Position(1, 13))
            ]), statement.TrueBlock);
        Assert.Equivalent(new List<IStatement>([
                new FunctionCall("func2", new List<IExpression>(), new Position(2, 14))
            ]), statement.ElseBlock);
    }


    [Fact]
    public void TestElsIfConditional()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.If, pos = new Position(1, 1) },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value = "cond" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 11) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 12) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 17) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 1) },
            new Token { type = TokenType.Else, pos = new Position(2, 2) },
            new Token { type = TokenType.If, pos = new Position(2, 8) },
            new Token { type = TokenType.RoundOpen, pos = new Position(2, 11) },
            new Token { type = TokenType.Identifier, pos = new Position(2, 12), value = "cond2" },
            new Token { type = TokenType.RoundClose, pos = new Position(2, 18) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(2, 19) },
            new Token { type = TokenType.Identifier, pos = new Position(3, 1), value = "func2" },
            new Token { type = TokenType.RoundOpen, pos = new Position(3, 7) },
            new Token { type = TokenType.RoundClose, pos = new Position(3, 8) },
            new Token { type = TokenType.Semicolon, pos = new Position(3, 9) },
            new Token { type = TokenType.CurlyClose, pos = new Position(4, 1) },
            new Token { type = TokenType.EOF, pos = new Position(5, 1) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        ConditionalStatement? statement = parser.ParseConditionalStatement();

        Assert.NotNull(statement);
        Assert.Equivalent(new IdentifierExpression("cond", new Position(1, 7)), statement.Condition);
        Assert.Equivalent(new List<IStatement>([
                new FunctionCall("func", new List<IExpression>(), new Position(1, 13))
            ]), statement.TrueBlock);
        Assert.Null(statement.ElseBlock);
        Assert.NotNull(statement.SubStatement);
        Assert.Equivalent(new IdentifierExpression("cond2", new Position(2, 12)), statement.SubStatement!.Condition);
        Assert.Equivalent(new List<IStatement>([new FunctionCall("func2", new List<IExpression>(), new Position(3, 1))]), statement.SubStatement!.TrueBlock);
    }

    [Fact]
    public void TestElsIfElseConditional()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.If, pos = new Position(1, 1) },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value = "cond" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 11) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 12) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 17) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 1) },
            new Token { type = TokenType.Else, pos = new Position(2, 2) },
            new Token { type = TokenType.If, pos = new Position(2, 8) },
            new Token { type = TokenType.RoundOpen, pos = new Position(2, 11) },
            new Token { type = TokenType.Identifier, pos = new Position(2, 12), value = "cond2" },
            new Token { type = TokenType.RoundClose, pos = new Position(2, 18) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(2, 19) },
            new Token { type = TokenType.Identifier, pos = new Position(3, 1), value = "func2" },
            new Token { type = TokenType.RoundOpen, pos = new Position(3, 7) },
            new Token { type = TokenType.RoundClose, pos = new Position(3, 8) },
            new Token { type = TokenType.Semicolon, pos = new Position(3, 9) },
            new Token { type = TokenType.CurlyClose, pos = new Position(4, 1) },
            new Token { type = TokenType.Else, pos = new Position(5, 1) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(5, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(6, 1), value = "func2" },
            new Token { type = TokenType.RoundOpen, pos = new Position(6, 7) },
            new Token { type = TokenType.RoundClose, pos = new Position(6, 8) },
            new Token { type = TokenType.Semicolon, pos = new Position(6, 9) },
            new Token { type = TokenType.CurlyClose, pos = new Position(7, 1) },
            new Token { type = TokenType.EOF, pos = new Position(8, 1) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        ConditionalStatement? statement = parser.ParseConditionalStatement();

        Assert.NotNull(statement);
        Assert.Equivalent(new IdentifierExpression("cond", new Position(1, 7)), statement.Condition);
        Assert.Equivalent(new List<IStatement>([
                new FunctionCall("func", new List<IExpression>(), new Position(1, 13))
            ]), statement.TrueBlock);
        Assert.NotNull(statement.SubStatement);
        Assert.Equivalent(new IdentifierExpression("cond2", new Position(2, 12)), statement.SubStatement!.Condition);
        Assert.Equivalent(new List<IStatement>([new FunctionCall("func2", new List<IExpression>(), new Position(3, 1))]), statement.SubStatement!.TrueBlock);
        Assert.Equivalent(new List<IStatement>([new FunctionCall("func2", new List<IExpression>(), new Position(6, 1))]), statement.SubStatement!.ElseBlock);
    }

    [Fact]
    public void TestWhile()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.While, pos = new Position(1, 1) },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value="cond" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 11) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 12) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value="func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 17) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 1) },
            new Token { type = TokenType.Semicolon, pos = new Position(2, 2) },
            new Token { type = TokenType.EOF, pos = new Position(2, 3) },
        ]);
        var parser = new Parser(new TestLexer(tokens));

        LoopStatement? statement = parser.ParseLoopStatement();

        Assert.NotNull(statement);
        Assert.Equivalent(
            new LoopStatement(new IdentifierExpression("cond", new Position(1, 7)), new List<IStatement>([new FunctionCall("func", new List<IExpression>(), new Position(1, 13))])
            , new Position(1, 1)),
            statement
            );
    }

    [Fact]
    public void TestParseProgram()
    {
        var tokens = new List<Token>([
            new Token { type = TokenType.While, pos = new Position(1, 1) },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 6) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 7), value = "cond" },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 11) },
            new Token { type = TokenType.CurlyOpen, pos = new Position(1, 12) },
            new Token { type = TokenType.Identifier, pos = new Position(1, 13), value = "func" },
            new Token { type = TokenType.RoundOpen, pos = new Position(1, 17) },
            new Token { type = TokenType.RoundClose, pos = new Position(1, 18) },
            new Token { type = TokenType.Semicolon, pos = new Position(1, 19) },
            new Token { type = TokenType.CurlyClose, pos = new Position(2, 1) },
            new Token { type = TokenType.Semicolon, pos = new Position(2, 2) },
            new Token { type = TokenType.Identifier, pos = new Position(3, 1), value = "one" },
            new Token { type = TokenType.Assign, pos = new Position(3, 5) },
            new Token { type = TokenType.Int, pos = new Position(3, 6), value = 1 },
            new Token { type = TokenType.Semicolon, pos = new Position(3, 7) },
            new Token { type = TokenType.EOF, pos = new Position(4, 1) }
        ]);
        var parser = new Parser(new TestLexer(tokens));

        List<IStatement>? statements = parser.ParseProgram().Statements;

        Assert.NotNull(statements[0]);
        Assert.Equivalent(
            new LoopStatement(new IdentifierExpression("cond", new Position(1, 7)), new List<IStatement>([new FunctionCall(
                "func", new List<IExpression>(), new Position(1, 13))])
            , new Position(1, 1)),
            statements[0]
            );
        Assert.Equivalent(
            new Assignment(new Assignable("one", null, new Position(3, 1)), new IntegerExpression(1, new Position(3, 6)), new Position(3, 1))
            ,
            statements[1]
            );
    }

    public class TestLexer : ILexer
    {
        private Queue<Token> _tokens;

        public TestLexer(List<Token> tokens)
        {
            _tokens = new Queue<Token>(tokens);
        }

        public Token NextToken()
        {
            _tokens.Dequeue();
            return _tokens.First();
        }
        public Token GetCurrentToken()
        {
            return _tokens.First();
        }
    }
}