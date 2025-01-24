using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Statements;
using PySharpCompiler.Classes;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PySharpCompiler.Tests.ParserTests.ParserTests;
using PySharpCompiler.Classes.InterpreterClasses;

namespace PySharpCompiler.Tests.IntegrationTests
{
    public class IntegrationTests
    {
        [Fact]
        public void TestParseProgram()
        {
            var codeReader = GetCodeReader("def func(int param)\n" +
                                           "{ int result = param;\n" +
                                           " return result;\n" +
                                           "};");
            var parser = new Parser(new Lexer(codeReader));

            List<IStatement>? statements = parser.ParseProgram().Statements;

            Assert.NotNull(statements[0]);
            Assert.Equivalent(
                new FunctionDefinition(
                        "func",
                        new List<Param>([new Param(new PrimitiveType(DOMType.Int), "param", new Position(1,10), false)]),
                        new List<IStatement>([
                            new Declaration(false, new PrimitiveType(DOMType.Int), "result", new IdentifierExpression("param", new Position(2, 16)), new Position(2, 3)),
                            new ReturnStatement(new IdentifierExpression("result", new Position(3, 9)), new Position(3, 2))
                        ]),
                        new PrimitiveType(DOMType.Void),
                        new Position(1,1)
                    )
                , statements[0]);
        }

        [Fact]
        public void TestParseAndExecuteProgram()
        {
            var codeReader = GetCodeReader("def func(int param)\n" +
                                           "{ int result = param;\n" +
                                           " return result;\n" +
                                           "};");
            var parser = new Parser(new Lexer(codeReader));

            var program = parser.ParseProgram();
            List<IStatement>? statements = program.Statements;

            Assert.NotNull(statements[0]);
            Assert.Equivalent(
                new FunctionDefinition(
                        "func",
                        new List<Param>([new Param(new PrimitiveType(DOMType.Int), "param", new Position(1, 10), false)]),
                        new List<IStatement>([
                            new Declaration(false, new PrimitiveType(DOMType.Int), "result", new IdentifierExpression("param", new Position(2, 16)), new Position(2, 3)),
                            new ReturnStatement(new IdentifierExpression("result", new Position(3, 9)), new Position(3, 2))
                        ]),
                        new PrimitiveType(DOMType.Void),
                        new Position(1, 1)
                    )
                , statements[0]);

            var interpreter = new Interpreter();

            interpreter.Visit(program);
            Assert.Equivalent(new DeclaredFunction(
                     new List<IStatement>([
                            new Declaration(false, new PrimitiveType(DOMType.Int), "result", new IdentifierExpression("param", new Position(2, 16)), new Position(2, 3)),
                            new ReturnStatement(new IdentifierExpression("result", new Position(3, 9)), new Position(3, 2))
                     ]),
                      new List<ParamValue>([new ParamValue(new PrimitiveType(DOMType.Int), "param", new Position(1, 10), false)]),
                      new Position(1, 1),
                      new PrimitiveType(DOMType.Void)
                ), interpreter.GetSymbolValue(new Identifier("func", new Position(1,1))));
        }

        [Fact]
        public void TestBasicFunctionExample()
        {
            var code = "def addition(int a, int b) -> int {\r\n   int result = a + b;\r\n   return result;\r\n};\r\n\r\nint three = addition(1, 2);\r\n\r\nreturn three;";
            var codeReader = GetCodeReader(code);
            
            var parser = new Parser(new Lexer(codeReader));
            var program = parser.ParseProgram();
            var interpreter = new Interpreter();


            Assert.Equivalent(
                new IntValue(3, new Position(6,22)),
                interpreter.Visit(program)
            );
        }

        [Fact]
        public void TestFunctionExample()
        {
            var code = "def CreateIntFromString(string myString) -> int {\r\n  int myInt = myString;\r\n  return myInt;\r\n};\r\n\r\nprint(CreateIntFromString('123') - 1);\r\n\r\ndef CountFromNumber(mut int number) -> void {\r\n  if (number > 0) {\r\n    print(number); \r\n    number = number - 1;\r\n    CountFromNumber(number);\r\n  } else {\r\n    print('Done!');\r\n    return;\r\n  } \r\n};\r\n\r\nCountFromNumber(5);\r\n\r\nreturn CreateIntFromString('123') - 1;";
            var codeReader = GetCodeReader(code);

            var parser = new Parser(new Lexer(codeReader));
            var program = parser.ParseProgram();
            var interpreter = new Interpreter();


            Assert.Equivalent(
                new IntValue(122, new Position(21, 28)),
                interpreter.Visit(program)
            );
        }

        [Fact]
        public void TestRecursionFunction()
        {
            var code = "def recursion(mut int a) -> int {\r\nif (a < 10)\r\n{ a = a + 1; a = recursion(a); }\r\nreturn a;\r\n};\r\nrecursion(1);";
            var codeReader = GetCodeReader(code);

            var parser = new Parser(new Lexer(codeReader));
            var program = parser.ParseProgram();
            var interpreter = new Interpreter();


            Assert.Equivalent(
                new IntValue(10, new Position(6, 11)),
                interpreter.Visit(program)
            );
        }

        [Fact]
        public void TestNestedExpression()
        {
            var code = "return (2 + 2) * 3;";
            var codeReader = GetCodeReader(code);

            var parser = new Parser(new Lexer(codeReader));
            var program = parser.ParseProgram();
            var interpreter = new Interpreter();


            Assert.Equivalent(
                new IntValue(12, new Position(1, 9)),
                interpreter.Visit(program)
            );
        }

        public CodeReader GetCodeReader(string testContent)
        {
            byte[] mockFile = Encoding.UTF8.GetBytes(testContent);
            var memorySteam = new MemoryStream(mockFile);
            var streamReader = new StreamReader(memorySteam);
            CodeReader codeReader = new CodeReader(streamReader);
            return codeReader;
        }
    }
}
