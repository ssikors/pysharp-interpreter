using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes;
using PySharpCompiler.Components;
using static PySharpCompiler.Tests.ParserTests.ParserTests;
using PySharpCompiler.Classes.InterpreterClasses;
using System.Linq.Expressions;
using PySharpCompiler.Classes.Expressions.Operators;
using PySharpCompiler.Classes.Statements;
using Newtonsoft.Json.Linq;

namespace PySharpCompiler.Tests.InterpreterTests
{
    public class InterpreterTests
    {
        // PRIMITIVE FACTORS

        [Fact]
        public void TestVisitIntegerExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntegerExpression(21, pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(21, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitStringExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringExpression("text", pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new StringValue("text", pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitIdentifierExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IdentifierExpression("variable", pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), "variable", new IntegerExpression(1, pos), pos);
            
            var interpreter = new Interpreter();
            interpreter.Visit(declaration);
            Assert.Equivalent(new IntValue(1, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitFloatExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatExpression(1.5, pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new FloatValue(1.5, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitBoolExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BooleanExpression(false, pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(false, pos), interpreter.Visit(DOMTree));
        }

        // COMPLEX FACTORS

        [Fact]
        public void TestVisitListExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new Declaration(true,
                new ListType(new PrimitiveType(DOMType.Int)),
                "lista",
                new ListExpression(
                    new List<IExpression>([new IntegerExpression(1,pos), new IntegerExpression(2, pos)]),
                    pos
                ),
                pos                    
            );

            var interpreter = new Interpreter();
            interpreter.Visit(DOMTree);

            Assert.Equivalent(new ListValue(new List<DOMObject>([new IntValue(1,pos), new IntValue(2, pos)]), pos, new PrimitiveType(DOMType.Int)),
            interpreter.GetSymbolValue(new Identifier("lista", pos)));
        }

        [Fact]
        public void TestVisitListLength()
        {
            var pos = new Position(1, 2);
            var DOMTree = new Declaration(true,
                new ListType(new PrimitiveType(DOMType.Int)),
                "lista",
                new ListExpression(
                    new List<IExpression>([new IntegerExpression(1, pos), new IntegerExpression(2, pos)]),
                    pos
                ),
                pos
            );

            var interpreter = new Interpreter();
            interpreter.Visit(DOMTree);

            var listLenght = new ListLengthExpression("lista", pos);

            Assert.Equivalent(new IntValue(2, pos), interpreter.Visit(listLenght));
        }

        [Fact]
        public void TestVisitListIndex()
        {
            var pos = new Position(1, 2);
            var DOMTree = new Declaration(true,
                new ListType(new PrimitiveType(DOMType.Int)),
                "lista",
                new ListExpression(
                    new List<IExpression>([new IntegerExpression(1, pos), new IntegerExpression(2, pos)]),
                    pos
                ),
                pos
            );

            var interpreter = new Interpreter();
            interpreter.Visit(DOMTree);

            var Indexed = new IndexedExpression(new IdentifierExpression("lista", pos), new ListIndex(new IntegerExpression(0, pos), null) , pos);

            Assert.Equivalent(new IntValue(1, pos), interpreter.Visit(Indexed));
        }


        // COMPLEX EXPRESSIONS



        // Unary expression

        [Fact]
        public void TestVisitUnaryExpressionNegate()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MyUnaryExpression(OperatorType.Negate, new BooleanExpression(true, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(false, pos), interpreter.Visit(DOMTree));
        }


        [Fact]
        public void TestVisitUnaryExpressionNegateStringConversion()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MyUnaryExpression(OperatorType.Negate, new StringExpression("", pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));

            DOMTree = new MyUnaryExpression(OperatorType.Negate, new StringExpression("abcd", pos), pos);
            Assert.Equivalent(new BoolValue(false, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitUnaryExpressionMinus()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MyUnaryExpression(OperatorType.Minus, new IntegerExpression(12, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(-12, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitUnaryExpressionMinusString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MyUnaryExpression(OperatorType.Minus, new StringExpression("12", pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(-12, pos), interpreter.Visit(DOMTree));
        }

        // Multiplicative expressions

        [Fact]
        public void TestVisitMultiplicativeMultiply()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MultiplicativeExpression(new IntegerExpression(3, pos), OperatorType.Multiply, new IntegerExpression(12, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(36, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitMultiplicativeMultiplyFloatInt()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MultiplicativeExpression(new FloatExpression(3.3, pos), OperatorType.Multiply, new IntegerExpression(12, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new FloatValue(3.3*12, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitMultiplicativeMultiplyComplex()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MultiplicativeExpression(
                new MyUnaryExpression(OperatorType.Minus, new IntegerExpression(21, pos), pos), OperatorType.Multiply,
                new IntegerExpression(10, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(-210, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitMultiplicativeDivide()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MultiplicativeExpression(new IntegerExpression(12, pos), OperatorType.Divide, new IntegerExpression(3, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(4, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitMultiplicativeDivideComplex()
        {
            var pos = new Position(1, 2);
            var DOMTree = new MultiplicativeExpression(
                new MyUnaryExpression(OperatorType.Minus, new IntegerExpression(10, pos), pos),
                OperatorType.Divide,
                new IntegerExpression(-5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(2, pos), interpreter.Visit(DOMTree));
        }

        // Pipe
        [Fact]
        public void TestVisitPipe()
        {
            var pos = new Position(1, 2);
            var funId1 = "fun1";

            var statements1 = new List<IStatement>([
                new ReturnStatement(
                    new IdentifierExpression("abc", pos),
                    pos
                )
            ]);
            var parameters = new List<Param>([ new Param( new PrimitiveType(DOMType.Int), "abc", pos, true) ]);
            var definition1 = new FunctionDefinition(
                funId1,
                parameters,
                statements1,
                new PrimitiveType(DOMType.Int),
                pos
            );


            var funId2 = "fun2";

            var statements2 = new List<IStatement>([
                new ReturnStatement(
                    new AdditiveExpression(new IdentifierExpression("processedInput", pos), OperatorType.Plus, new IntegerExpression(1412, pos), pos),
                    pos
                )
            ]);
            var parameters2 = new List<Param>([new Param(new PrimitiveType(DOMType.Int), "processedInput", pos, true)]);
            var definition2 = new FunctionDefinition(
                funId2,
                parameters2,
                statements2,
                new PrimitiveType(DOMType.Int),
                pos
            );

            var pipeDeclare = new Declaration(
                true,
                new FunType(
                    new List<IDOMType>([new PrimitiveType(DOMType.Int)]),
                    new PrimitiveType(DOMType.Int)
                ),
                "pipeFunction",
                new MultiplicativeExpression(
                new IdentifierExpression(funId1, pos),
                OperatorType.Pipe,
                new IdentifierExpression(funId2, pos),
                pos
                 ),
                pos
            );

            var interpreter = new Interpreter();

            interpreter.Visit(definition1);
            interpreter.Visit(definition2);

            interpreter.Visit(pipeDeclare);

            var execute = new ReturnStatement(
                new FunctionCallExpression(
                    "pipeFunction",
                    new List<IExpression>([new IntegerExpression(1,pos)]),
                    pos
                ),
                pos
            );

            Assert.Equivalent(new IntegerExpression(1413,pos), interpreter.Visit(execute));
        }

        [Fact]
        public void TestVisitPipeComplex()
        {
            var pos = new Position(1, 2);
            var funId1 = "fun1";

            var statements1 = new List<IStatement>([
                new ReturnStatement(
                    new IdentifierExpression("abc", pos),
                    pos
                )
            ]);
            var parameters = new List<Param>([new Param(new PrimitiveType(DOMType.Int), "abc", pos, true)]);
            var definition1 = new FunctionDefinition(
                funId1,
                parameters,
                statements1,
                new PrimitiveType(DOMType.Int),
                pos
            );


            var funId2 = "fun2";

            var statements2 = new List<IStatement>([
                new ReturnStatement(
                    new AdditiveExpression(new IdentifierExpression("processedInput", pos), OperatorType.Plus, new IntegerExpression(1408, pos), pos),
                    pos
                )
            ]);
            var parameters2 = new List<Param>([new Param(new PrimitiveType(DOMType.Int), "processedInput", pos, true)]);
            var definition2 = new FunctionDefinition(
                funId2,
                parameters2,
                statements2,
                new PrimitiveType(DOMType.Int),
                pos
            );

            var funId3 = "fun3";

            var statements3 = new List<IStatement>([
                new ReturnStatement(
                    new AdditiveExpression(new IdentifierExpression("processedInput2", pos), OperatorType.Plus, new IntegerExpression(1, pos), pos),
                    pos
                )
            ]);
            var parameters3 = new List<Param>([new Param(new PrimitiveType(DOMType.Int), "processedInput2", pos, true)]);
            var definition3 = new FunctionDefinition(
                funId3,
                parameters3,
                statements3,
                new PrimitiveType(DOMType.Int),
                pos
            );

            var pipeDeclare = new Declaration(
                true,
                new FunType(
                    new List<IDOMType>([new PrimitiveType(DOMType.Int)]),
                    new PrimitiveType(DOMType.Int)
                ),
                "pipeFunction",
                new MultiplicativeExpression(
                new IdentifierExpression(funId1, pos),
                OperatorType.Pipe,
                new MultiplicativeExpression(
                    new IdentifierExpression(funId2, pos),
                    OperatorType.Pipe,
                    new IdentifierExpression(funId3, pos),
                    pos
                 ),
                pos
                 ),
                pos
            );

            var interpreter = new Interpreter();

            interpreter.Visit(definition1);
            interpreter.Visit(definition2);
            interpreter.Visit(definition3);

            interpreter.Visit(pipeDeclare);

            var execute = new ReturnStatement(
                new FunctionCallExpression(
                    "pipeFunction",
                    new List<IExpression>([new IntegerExpression(1, pos)]),
                    pos
                ),
                pos
            );

            Assert.Equivalent(new IntegerExpression(1410, pos), interpreter.Visit(execute));
        }

        // Bind front
        [Fact]
        public void TestVisitBind()
        {
            var pos = new Position(1, 2);
            var funId = "fun";

            var statements = new List<IStatement>([
                new ReturnStatement(
                    new IdentifierExpression("input", pos),
                    pos
                )
            ]);
            var parameters = new List<Param>([new Param(new PrimitiveType(DOMType.Int), "input", pos, true)]);
            var definition = new FunctionDefinition(
                funId,
                parameters,
                statements,
                new PrimitiveType(DOMType.Int),
                pos
            );

            var defineBound = new Declaration(
                true,
                new FunType(new List<IDOMType>([]), new PrimitiveType(DOMType.Int)),
                "boundFunction",
                new MultiplicativeExpression(
                    new IdentifierExpression(funId, pos),
                    OperatorType.BindFront,
                    new IntegerExpression(10, pos),
                    pos
                ),
                pos
            );

            var interpreter = new Interpreter();

            interpreter.Visit(definition);
            interpreter.Visit(defineBound);

            var callBound = new FunctionCall(
                "boundFunction",
                new List<IExpression>([]),
                pos
            );

            Assert.Equivalent(new IntValue(10, pos), interpreter.Visit(callBound));
        }


        // Additive expressions

        [Fact]
        public void TestVisitAdditiveAdd()
        {
            var pos = new Position(1, 2);
            var DOMTree = new AdditiveExpression(new IntegerExpression(3, pos), OperatorType.Plus, new IntegerExpression(12, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(15, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitAdditiveAddStrings()
        {
            var pos = new Position(1, 2);
            var DOMTree = new AdditiveExpression(new StringExpression("pi is 3", pos), OperatorType.Plus, new StringExpression(".14", pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new StringExpression("pi is 3.14", pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitAdditiveAddComplex()
        {
            var pos = new Position(1, 2);
            var DOMTree = new AdditiveExpression(
                new MultiplicativeExpression(
                    new IntegerExpression(3, pos),
                    OperatorType.Multiply,
                    new IntegerExpression(5, pos),
                    pos
                ),
                OperatorType.Plus,
                new MultiplicativeExpression(
                    new IntegerExpression(12, pos),
                    OperatorType.Divide,
                    new IntegerExpression(3, pos),
                    pos
                ),
                pos
             );

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(19, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitAdditiveSubstract()
        {
            var pos = new Position(1, 2);
            var DOMTree = new AdditiveExpression(new IntegerExpression(3, pos), OperatorType.Minus, new IntegerExpression(12, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntValue(-9, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitAdditiveSubstractComplexFloatInt()
        {
            var pos = new Position(1, 2);
            var DOMTree = new AdditiveExpression(
                new MultiplicativeExpression(
                    new FloatExpression(2.5, pos),
                    OperatorType.Multiply,
                    new IntegerExpression(4, pos),
                    pos
                ),
                OperatorType.Minus,
                new MultiplicativeExpression(
                    new FloatExpression(12, pos),
                    OperatorType.Divide,
                    new IntegerExpression(3, pos),
                    pos
                ),
                pos
             );

            var interpreter = new Interpreter();
            Assert.Equivalent(new FloatValue(6, pos), interpreter.Visit(DOMTree));
        }

        // Relations expressions
        [Fact]
        public void TestVisitRelationMoreThan()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(new IntegerExpression(3, pos), OperatorType.MoreThan, new IntegerExpression(5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(false, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitRelationMoreOrEqual()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(new FloatExpression(5.6, pos), OperatorType.MoreOrEqual, new IntegerExpression(5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitRelationLessOrEqual()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(new FloatExpression(5, pos), OperatorType.LessOrEqual, new IntegerExpression(5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestVisitRelationLess()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(new FloatExpression(5.5, pos), OperatorType.LessOrEqual, new IntegerExpression(5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(false, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestIsEqual()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(new IntegerExpression(5, pos), OperatorType.Equal, new IntegerExpression(5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestIsNotEqual()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(new IntegerExpression(6, pos), OperatorType.NotEqual, new IntegerExpression(5, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestComplexRelation()
        {
            var pos = new Position(1, 2);
            var DOMTree = new RelationExpression(
                new AdditiveExpression(
                    new IntegerExpression(3, pos),
                    OperatorType.Plus,
                    new MultiplicativeExpression(
                         new IntegerExpression(4, pos),
                         OperatorType.Multiply,
                         new FloatExpression(2.5, pos),
                         pos
                    ),
                    pos
                ),
                OperatorType.MoreThan,
                new MyUnaryExpression(OperatorType.Minus, new IntegerExpression(50, pos), pos),
                pos
             );

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));
        }

        // Conjunction expressions
        [Fact]
        public void TestConjunctionExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new ConjunctionExpression(new BooleanExpression(true, pos), new BooleanExpression(false, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(false, pos), interpreter.Visit(DOMTree));
        }

        // TODO more cases!!

        // Alternative expressions
        [Fact]
        public void TestAlternativeExpression()
        {
            var pos = new Position(1, 2);
            var DOMTree = new AlternativeExpression(new BooleanExpression(true, pos), new BooleanExpression(false, pos), pos);

            var interpreter = new Interpreter();
            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(DOMTree));
        }



        // STATEMENTS

        // Return statement

        [Fact]
        public void TestReturnStatement()
        {
            var pos = new Position(1, 2);
            var DOMTree = new ReturnStatement(
                    new IntegerExpression(12, pos),
                    pos
                );

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntegerExpression(12, pos), interpreter.Visit(DOMTree));
        }

        [Fact]
        public void TestReturnProgram()
        {
            var pos = new Position(1, 2);
            var DOMTree = new PYCProgram(new List<IStatement>([new ReturnStatement(new IntegerExpression(12, pos),pos)]));

            var interpreter = new Interpreter();
            Assert.Equivalent(new IntegerExpression(12, pos), interpreter.Visit(DOMTree));
        }


        // Declaration

        [Fact]
        public void PreventDeclareWrongType()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, new StringExpression("ascasc", pos), pos);

            var interpreter = new Interpreter();

            Assert.Throws<Exception>(() => { interpreter.Visit(declaration); });
        }

        [Fact]
        public void TestDeclarationNoAssignment()
        {
            var pos = new Position(1, 2);
            var DOMTree = new Declaration(false, new PrimitiveType(DOMType.Int), "newVariable", null, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(DOMTree);

            Assert.Null(interpreter.GetSymbolValue(new Identifier("newVariable", pos)));
        }

        [Fact]
        public void TestDeclarationAssignment()
        {
            var pos = new Position(1, 2);

            var value = new IntegerExpression(12, pos);
            var DOMTree = new Declaration(false, new PrimitiveType(DOMType.Int), "newVariable", value, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(DOMTree);

            Assert.IsType<IntValue>(interpreter.GetSymbolValue(new Identifier("newVariable", pos)));
            Assert.Equivalent(new IntValue(12, pos), interpreter.GetSymbolValue(new Identifier("newVariable", pos)));
        }

        [Fact]
        public void TestDeclarationAssignmentComplex()
        {
            var pos = new Position(1, 2);
            var value = new MultiplicativeExpression(
                    new IntegerExpression(12, pos),
                    OperatorType.Multiply,
                    new FloatExpression(1.5, pos),
                    pos
                );
            
            var DOMTree = new Declaration(false, new PrimitiveType(DOMType.Int), "newVariable", value, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(DOMTree);

            Assert.Equivalent(new IntValue(18, pos), interpreter.GetSymbolValue(new Identifier("newVariable", pos)));
        }

        // Assignment 
        [Fact]
        public void TestAssignToUninitialized()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, null, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            Assert.Null(interpreter.GetSymbolValue(new Identifier(id, pos)));

            var value = new IntegerExpression(12, pos);
            var assignment = new Assignment(new Assignable(id, null, pos),value, pos);

            interpreter.Visit(assignment);

            Assert.Equivalent(new IntValue(12, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void TestAssignToUninitializedComplex()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, null, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            Assert.Null(interpreter.GetSymbolValue(new Identifier(id, pos)));

            var value = new AdditiveExpression(
                    new IntegerExpression(12, pos),
                    OperatorType.Plus,
                    new MyUnaryExpression(OperatorType.Minus,
                    new IntegerExpression(2, pos), pos),
                    pos
                );
            var assignment = new Assignment(new Assignable(id, null, pos), value, pos);

            interpreter.Visit(assignment);

            Assert.Equivalent(new IntValue(10, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void TestReAssign()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var initialValue = new IntegerExpression(51, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, initialValue, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            Assert.Equivalent(initialValue, interpreter.GetSymbolValue(new Identifier(id, pos)));

            var value = new IntegerExpression(12, pos);
            var assignment = new Assignment(new Assignable(id, null, pos), value, pos);

            interpreter.Visit(assignment);

            Assert.IsType<IntValue>(interpreter.GetSymbolValue(new Identifier(id, pos)));
            Assert.Equivalent(new IntValue(12, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void TestAssignWithOtherIdentifier()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var id2 = "second";
            var initialValue = new IntegerExpression(51, pos);
            var newValue = new IntegerExpression(11, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, initialValue, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            Assert.Equivalent(initialValue, interpreter.GetSymbolValue(new Identifier(id, pos)));

            var declaration2 = new Declaration(true, new PrimitiveType(DOMType.Int), id2, newValue, pos);

            interpreter.Visit(declaration2);

            var assignment = new Assignment(new Assignable(id, null, pos), new IdentifierExpression(id2, pos), pos);

            interpreter.Visit(assignment);

            Assert.IsType<IntValue>(interpreter.GetSymbolValue(new Identifier(id, pos)));
            Assert.Equivalent(new IntValue(11, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void PreventAssignToImmutable()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var declaration = new Declaration(false, new PrimitiveType(DOMType.Int), id, null, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            Assert.Null(interpreter.GetSymbolValue(new Identifier(id, pos)));

            var value = new IntegerExpression(12, pos);
            var assignment = new Assignment(new Assignable(id, null, pos), value, pos);

            Assert.Throws<Exception>(() => { interpreter.Visit(assignment); });
        }

        [Fact]
        public void PreventAssignWrongType()
        {
            var pos = new Position(1, 2);
            var id = "newVariable";
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, null, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            Assert.Null(interpreter.GetSymbolValue(new Identifier(id, pos)));

            var value = new StringExpression("abcd", pos);
            var assignment = new Assignment(new Assignable(id, null, pos), value, pos);

            Assert.Throws<Exception>(() => { interpreter.Visit(assignment); });
        }


        // While statement

        [Fact]
        public void WhileLoopTest()
        {
            var pos = new Position(1, 2);
            var id = "counter";
            var value = new IntegerExpression(1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            var loopStatement = new LoopStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.Less,
                    new IntegerExpression(5, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new AdditiveExpression(
                            new IdentifierExpression(id, pos),
                            OperatorType.Plus,
                            new IntegerExpression(1, pos),
                            pos
                        ),
                        pos)
                    ]),
                pos
            );

            interpreter.Visit(loopStatement);

            Assert.Equivalent(new IntValue(5, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }


        [Fact]
        public void WhileLoopTestScope()
        {
            var pos = new Position(1, 2);
            var id = "counter";
            var value = new IntegerExpression(1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();

            interpreter.Visit(declaration);

            var loopStatement = new LoopStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.Less,
                    new IntegerExpression(5, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new AdditiveExpression(
                            new IdentifierExpression(id, pos),
                            OperatorType.Plus,
                            new IntegerExpression(1, pos),
                            pos
                        ),
                        pos),
                        new Declaration(true, new PrimitiveType(DOMType.Int), "abc", value, pos)
                    ]),
                pos
            );

            interpreter.Visit(loopStatement);

            Assert.Equivalent(null, interpreter.GetSymbolValue(new Identifier("abc", pos)));
        }


        // Conditional statement

        [Fact]
        public void ConditionalTestIf()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(5, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();
            interpreter.Visit(declaration);

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(0, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                null,
                null,
                pos

            );

            interpreter.Visit(ifStatement);

            Assert.Equivalent(new IntValue(10, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void ConditionalTestIfScopeTest()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(5, pos);

            var interpreter = new Interpreter();

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IntegerExpression(1, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(0, pos),
                    pos
                ),
                new List<IStatement>([
                    new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos),
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                null,
                null,
                pos

            );

            interpreter.Visit(ifStatement);
            Assert.Equivalent(null, interpreter.GetSymbolValue(new Identifier(id, pos)));
        }


        [Fact]
        public void ConditionalTestIfFalse()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(-1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();
            interpreter.Visit(declaration);

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(0, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                null,
                null,
                pos

            );

            interpreter.Visit(ifStatement);

            Assert.Equivalent(new IntValue(-1, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void ConditionalTestIfElse()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(-1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();
            interpreter.Visit(declaration);

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(0, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(1, pos),
                        pos
                    )
                ]),
                null,
                pos

            );

            interpreter.Visit(ifStatement);

            Assert.Equivalent(new IntValue(1, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void ConditionalTestIfElseIf()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(-1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();
            interpreter.Visit(declaration);

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(0, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                null,
                new ConditionalStatement(
                    new RelationExpression(
                        new IdentifierExpression(id, pos),
                        OperatorType.Less,
                        new IntegerExpression(0, pos),
                        pos
                    ),
                    new List<IStatement>([
                        new Assignment(
                            new Assignable(id, null, pos),
                            new IntegerExpression(10, pos),
                            pos
                        )
                    ]),
                    null,
                    null,
                    pos
                ),
                pos

            );

            interpreter.Visit(ifStatement);

            Assert.Equivalent(new IntValue(10, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void ConditionalTestIfElseIfFalse()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(-1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();
            interpreter.Visit(declaration);

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(0, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                null,
                new ConditionalStatement(
                    new RelationExpression(
                        new IdentifierExpression(id, pos),
                        OperatorType.MoreThan,
                        new IntegerExpression(0, pos),
                        pos
                    ),
                    new List<IStatement>([
                        new Assignment(
                            new Assignable(id, null, pos),
                            new IntegerExpression(10, pos),
                            pos
                        )
                    ]),
                    null,
                    null,
                    pos

                ),
                pos

            );

            interpreter.Visit(ifStatement);

            Assert.Equivalent(new IntValue(-1, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        [Fact]
        public void ConditionalTestIfElseIfElse()
        {
            var pos = new Position(1, 2);
            var id = "condition";
            var value = new IntegerExpression(-1, pos);
            var declaration = new Declaration(true, new PrimitiveType(DOMType.Int), id, value, pos);

            var interpreter = new Interpreter();
            interpreter.Visit(declaration);

            var ifStatement = new ConditionalStatement(
                new RelationExpression(
                    new IdentifierExpression(id, pos),
                    OperatorType.MoreThan,
                    new IntegerExpression(10, pos),
                    pos
                ),
                new List<IStatement>([
                    new Assignment(
                        new Assignable(id, null, pos),
                        new IntegerExpression(10, pos),
                        pos
                    )
                ]),
                null,
                new ConditionalStatement(
                    new RelationExpression(
                        new IdentifierExpression(id, pos),
                        OperatorType.MoreThan,
                        new IntegerExpression(0, pos),
                        pos
                    ),
                    new List<IStatement>([
                        new Assignment(
                            new Assignable(id, null, pos),
                            new IntegerExpression(10, pos),
                            pos
                        )
                    ]),
                    new List<IStatement>([
                        new Assignment(
                            new Assignable(id, null, pos),
                            new IntegerExpression(78345, pos),
                            pos
                        )
                    ]),
                    null,
                    pos
                ),
                pos

            );

            interpreter.Visit(ifStatement);

            Assert.Equivalent(new IntValue(78345, pos), interpreter.GetSymbolValue(new Identifier(id, pos)));
        }


        // Function definition test

        [Fact]
        public void DeclareFunctionEmpty()
        {
            var pos = new Position(1, 2);
            var id = "NewFunction";
            var statements = new List<IStatement>([]);
            var parameters = new List<ParamValue>([]);
            var definition = new FunctionDefinition(
                id,
                null,
                statements,
                new PrimitiveType(DOMType.Void),
                pos
            );
            var expected = new DeclaredFunction(statements, parameters, pos, new PrimitiveType(DOMType.Void));

            var interpreter = new Interpreter();
            interpreter.Visit(definition);

            Assert.Equivalent(expected, interpreter.GetSymbolValue(new Identifier(id, pos)));
        }

        // TODO more tests


        // Function calls

        [Fact]
        public void CallFunction()
        {
            var pos = new Position(1, 2);
            var id = "ReturnTwelve";
            var statements = new List<IStatement>([
                new ReturnStatement(
                    new IntegerExpression(12, pos),
                    pos
                )
            ]);
            var parameters = new List<ParamValue>([]);
            var definition = new FunctionDefinition(
                id,
                null,
                statements,
                new PrimitiveType(DOMType.Int),
                pos
            );
            var expected = new DeclaredFunction(statements, parameters, pos, new PrimitiveType(DOMType.Int));

            var interpreter = new Interpreter();
            interpreter.Visit(definition);

            Assert.Equivalent(expected, interpreter.GetSymbolValue(new Identifier(id, pos)));

            var call = new FunctionCall(id, new List<IExpression>([]), pos);

            Assert.Equivalent(new IntValue(12, pos), interpreter.Visit(call));
        }

        [Fact]
        public void CallFunctionWithDeclaration()
        {
            var pos = new Position(1, 2);
            var id = "ReturnTwelve";
            var statements = new List<IStatement>([
                new Declaration(false, new PrimitiveType(DOMType.Int), "newVariable", new BooleanExpression(true, pos), pos),
                new ReturnStatement(
                    new IdentifierExpression("newVariable", pos),
                    pos
                )
            ]);
            var parameters = new List<ParamValue>([]);
            var definition = new FunctionDefinition(
                id,
                null,
                statements,
                new PrimitiveType(DOMType.Bool),
                pos
            );
            var expected = new DeclaredFunction(statements, parameters, pos, new PrimitiveType(DOMType.Bool));

            var interpreter = new Interpreter();
            interpreter.Visit(definition);

            Assert.Equivalent(expected, interpreter.GetSymbolValue(new Identifier(id, pos)));

            var call = new FunctionCall(id, new List<IExpression>([]), pos);

            Assert.Equivalent(new BoolValue(true, pos), interpreter.Visit(call));
        }

        // TODO check redeclaration


        [Fact]
        public void CallFunctionRecursive()
        {
            var pos = new Position(1, 2);
            var id = "ReturnTen";
            var statements = new List<IStatement>([
                new ConditionalStatement(
                      new RelationExpression(
                        new IdentifierExpression("number",pos),
                        OperatorType.Less,
                        new IntegerExpression(10, pos),
                        pos
                      ),
                      new List<IStatement>([new Assignment(
                        new Assignable(
                            "number",
                            null,
                            pos
                        ),
                        new AdditiveExpression(
                            new IdentifierExpression("number",pos),
                            OperatorType.Plus,
                            new IntegerExpression(1, pos),
                            pos
                        ),
                        pos
                        ),
                        new Assignment(
                        new Assignable(
                            "number",
                            null,
                            pos
                        ),
                        new FunctionCallExpression(id, new List<IExpression>([new IdentifierExpression("number",pos)]), pos),
                        
                        pos
                        )]),
                        null,
                        null,
                        pos
                ),
                new ReturnStatement(new IdentifierExpression("number",pos), pos)
            ]);
            var parameters = new List<ParamValue>([
                    new ParamValue(new PrimitiveType(DOMType.Int), "number", pos, true),
                ]);
            var definition = new FunctionDefinition(
                id,
                new List<Param>([new Param(new PrimitiveType(DOMType.Int), "number", pos, true)]),
                statements,
                new PrimitiveType(DOMType.Int),
                pos
            );
            var expected = new DeclaredFunction(statements, parameters, pos, new PrimitiveType(DOMType.Int));

            var interpreter = new Interpreter();
            interpreter.Visit(definition);

            var call = new FunctionCall(id, new List<IExpression>([new IntegerExpression(1, pos)]), pos);

            Assert.Equivalent(new IntValue(10, pos), interpreter.Visit(call));
        }
    }
}
