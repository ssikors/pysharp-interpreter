using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Classes;
using PySharpCompiler.Components;

namespace PySharpCompiler.Tests.ScopeTests
{
    public class ScopeTests
    {
        // Scope

        [Fact]
        public void TestAddSymbolScope()
        {
            var pos = new Position(1, 1);
            var expression = new IntValue(11, pos);
            var type = new PrimitiveType(DOMType.Int);

            var scope = new Scope("Global");

            scope.AddSymbol("thing", expression, false, pos, type);

            Assert.Equal(expression, scope.GetSymbol("thing"));
        }

        [Fact]
        public void ReAssignSymbol()
        {
            var pos = new Position(1, 1);
            var newPos = new Position(3, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new IntValue(11, pos);
            var newExpression = new IntValue(8, newPos);
            var type = new PrimitiveType(DOMType.Int);

            var scope = new Scope("Global");

            scope.AddSymbol("thing", expression, true, pos, type);
            Assert.Equal(expression, scope.GetSymbol("thing"));

            scope.AssignValue("thing", newExpression);
            Assert.Equal(newExpression, scope.GetSymbol("thing"));
        }

        [Fact]
        public void AssignSymbolFailImmutable()
        {
            var pos = new Position(1, 1);
            var newPos = new Position(3, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new IntValue(11, pos);
            var newExpression = new IntValue(8, newPos);
            var type = new PrimitiveType(DOMType.Int);

            var scope = new Scope("Global");

            scope.AddSymbol("thing", expression, false, pos, type);
            Assert.Equal(expression, scope.GetSymbol("thing"));


            Assert.Throws<Exception>(() => { scope.AssignValue("thing", newExpression); });
        }

        // Interpreter

        [Fact]
        public void TestAddSymbolInterpreter()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new IntValue(11, pos);
            var type = new PrimitiveType(DOMType.Int);
            var interpreter = new Interpreter();


            interpreter.DeclareSymbol(identifier, expression, false, type);

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));
        }

        [Fact]
        public void TestAddSymbolInterpreterLastScope()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new IntValue(11, pos);
            var type = new PrimitiveType(DOMType.Int);
            var interpreter = new Interpreter();

            interpreter.AddScope("Nested");
            interpreter.DeclareSymbol(identifier, expression, false, type);

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));
        }

        [Fact]
        public void TestInterpreterPopScope()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new StringValue("abc", pos);

            var interpreter = new Interpreter();

            interpreter.AddScope("Nested");

            Assert.Equivalent(new Stack<Scope>([new Scope("Global"), new Scope("Nested")]), interpreter.GetScopes());

            interpreter.PopScope();

            Assert.Equivalent(new Stack<Scope>([new Scope("Global")]), interpreter.GetScopes());

            interpreter.PopScope();

            Assert.Empty(interpreter.GetScopes());
        }

        [Fact]
        public void TestAddSymbolInterpreterPopScope()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new StringValue("abc", pos);
            var type = new PrimitiveType(DOMType.String);
            var interpreter = new Interpreter();

            interpreter.AddScope("Nested");

            Assert.Equivalent(new Stack<Scope>([new Scope("Global"), new Scope("Nested")]), interpreter.GetScopes());

            interpreter.DeclareSymbol(identifier, expression, false, type);

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));

            interpreter.PopScope();

            Assert.Equivalent(new Stack<Scope>([new Scope("Global")]), interpreter.GetScopes());

            Assert.Equivalent(null, interpreter.GetSymbolValue(identifier));

            Assert.True(interpreter.DeclareSymbol(identifier, expression, false, type));
        }

        [Fact]
        public void TestInterpreterGetFromHigherUp()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new FloatValue(421.42, pos);
            var type = new PrimitiveType(DOMType.Float);

            var interpreter = new Interpreter();

            interpreter.DeclareSymbol(identifier, expression, false, type);

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));

            interpreter.AddScope("Nested");
            interpreter.AddScope("SecondNested");

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));
        }

        [Fact]
        public void TestInterpreterPreventRedeclare()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new FloatValue(421.42, pos);
            var type = new PrimitiveType(DOMType.Float);

            var interpreter = new Interpreter();

            interpreter.DeclareSymbol(identifier, expression, false, type);

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));

            interpreter.AddScope("Nested");
            interpreter.AddScope("SecondNested");

            Assert.Throws<Exception>(() => { interpreter.DeclareSymbol(identifier, expression, false, type); });
        }

        [Fact]
        public void TestInterpreterPreventDeclareWrongType()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new StringValue("afvsa", pos);
            var type = new PrimitiveType(DOMType.Int);

            var interpreter = new Interpreter();

            

            Assert.Throws<Exception>(() => { interpreter.DeclareSymbol(identifier, expression, false, type); });
        }

        [Fact]
        public void TestInterpreterPreventReassignImmutable()
        {
            var pos = new Position(1, 1);
            var identifier = new Identifier("thing", pos);
            var expression = new FloatValue(421.42, pos);
            var type = new PrimitiveType(DOMType.Float);

            var interpreter = new Interpreter();

            interpreter.DeclareSymbol(identifier, expression, false, type);

            Assert.Equal(expression, interpreter.GetSymbolValue(identifier));

            Assert.Throws<Exception>(() => { interpreter.AssignValue(identifier, expression); });
        }

        [Fact]
        public void TestInterpreterNonexistentAssignError()
        {
            var pos = new Position(1, 1);
            var expression = new FloatValue(421.42, pos);
            var identifier = new Identifier("nothing", pos);

            var interpreter = new Interpreter();

            Assert.Throws<Exception>(() => { interpreter.AssignValue(identifier, expression); });
        }
    }
}
