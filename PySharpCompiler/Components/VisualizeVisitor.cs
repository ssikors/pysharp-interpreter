using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Statements;

namespace PySharpCompiler.Components
{
    public class VisualizeVisitor
    {
        public int _depth = 0;
        public VisualizeVisitor() { }
        public void Visit(PYCProgram program)
        {
            Console.WriteLine("Program");
            _depth++;
            foreach (var statement in program.Statements)
            {
                statement.Visit(this);
            }
            _depth--;
        }

        // Statements
        public void Visit(Assignment statement)
        {
            PrintObject(statement.Position, "Assignment");
            _depth++;
            statement.Left.Visit(this);
            statement.Right.Visit(this);
            _depth--;
        }

        public void Visit(ConditionalStatement statement)
        {
            PrintObject(statement.Position, "Conditional statement");
            _depth++;
            PrintMessage("Condition:");
            _depth++;
            statement.Condition.Visit(this);
            _depth--;
            PrintMessage("If block:");
            _depth++;
            foreach (var blockStatement in statement.TrueBlock)
            {
                blockStatement.Visit(this);
            }
            _depth--;

            if (statement.SubStatement != null){
                PrintMessage("Else If block:");
                _depth++;
                statement.SubStatement.Visit(this);
                _depth--;
            }

            if (statement.ElseBlock != null)
            {
                PrintMessage("Else block:");
                _depth++;
                foreach (var blockStatement in statement.ElseBlock)
                {
                    blockStatement.Visit(this);
                }
                _depth--;
            }
            _depth--;
        }

        public void Visit(Declaration statement)
        {
            PrintObject(statement.Position, $"Declaration of {(statement.IsMutable ? "Mutable" : "Immutable")} '{statement.Identifier}' type {statement.Type}");
            _depth++;
            if (statement.AssignedTo != null)
            {
                PrintMessage("Assigned to:");
                _depth++;
                statement.AssignedTo.Visit(this);
                _depth--;
            }
            _depth--;
        }

        public void Visit(FunctionCall statement)
        {
            PrintObject(statement.Position, $"FunctionCall '{statement.Identifier}'");
            _depth++;
            PrintMessage("Arguments:");
            _depth++;
            foreach (var expr in statement.Arguments)
            {
                expr.Visit(this);
            }
            _depth -= 2;
        }

        public void Visit(FunctionDefinition statement)
        {
            PrintObject(statement.Position, $"FunctionDefinition '{statement.Identifier}'");
            _depth++;
            if (statement.Params != null)
            {
                PrintMessage("Params:");
                _depth++;
                foreach (var expr in statement.Params)
                {
                    expr.Visit(this);
                }
                _depth--;
            } else
            {
                PrintMessage("(No Params)");
            }

            if (statement.Statements != null)
            {
                PrintMessage("Block statements:");
                _depth++;
                foreach (var expr in statement.Statements)
                {
                    expr.Visit(this);
                }
                _depth--;
            } else
            {
                PrintMessage("(No Statements)");
            }

            if (statement.ReturnType != null) { PrintMessage($"Returns type {statement.ReturnType}"); }
            _depth--;
        }

        public void Visit(LoopStatement statement)
        {
            PrintObject(statement.Position, $"LoopStatement:");
            _depth++;
            PrintMessage("Condition:");
            _depth++;
            statement.Condition.Visit( this );
            _depth--;
            PrintMessage("Block:");
            _depth++;
            foreach (var blockStatement in statement.Block)
            {
                blockStatement.Visit(this);
            }
            _depth -= 2;
        }

        public void Visit(ReturnStatement statement)
        {
            PrintObject(statement.Position, $"ReturnStatement:");
            _depth++;
            if (statement.ReturnedExpression != null)
            {
                PrintMessage("Returns expression:");
                _depth++;
                statement.ReturnedExpression.Visit( this );
                _depth--;
            }
            _depth--;
        }


        // Expressions etc

        public void Visit(IndexedExpression expression)
        {
            PrintObject(expression.Position, $"IndexedExpression:");
            _depth++;
            expression.Expression.Visit( this );
            PrintMessage("Index:");
            _depth++;
            expression.Index.Visit( this );
            _depth -= 2;
        }
        public void Visit(FunctionCallExpression expression)
        {
            PrintObject(expression.Position, $"FunCallExpr '{expression.Identifier}'");
            _depth++;
            PrintMessage("Arguments:");
            _depth++;
            foreach (var expr in expression.Arguments)
            {
                expr.Visit(this);
            }
            _depth -= 2;
        }
        public void Visit(Assignable expression)
        {
            PrintObject(expression.Position, $"Assignable '{expression.Identifier}' ");
            _depth++;
            if (expression.Index != null)
            {
                expression.Index.Visit(this);
            }
            _depth--;
        }

        public void Visit(ListIndex expression)
        {
            expression.Value.Visit(this);
            if (expression.SubIndex != null)
            {
                expression.SubIndex.Visit(this);
            }
        }

        public void Visit(Param expression)
        {
            PrintObject(expression.Position, $"{(expression.IsMutable ? "Mutable" : "Immutable")} Param '{expression.Identifier}' of type {expression.Type}");
        }

        public void Visit(AdditiveExpression expression)
        {
            PrintObject(expression.Position, $"AdditiveExpression");
            _depth++;
            expression.Left.Visit(this);
            PrintObject(expression.Position, $"{expression.OperatorType}");
            expression.Right.Visit(this);
            _depth--;
        }

        public void Visit(AlternativeExpression expression)
        {
            PrintObject(expression.Position, $"AlternativeExpression");
            _depth++;
            expression.Left.Visit(this);
            PrintObject(expression.Position, $"or");
            expression.Right.Visit(this);
            _depth--;
        }

        public void Visit(BooleanExpression expression)
        {
            PrintObject(expression.Position, $"BooleanExpression: {(expression.Value ? "true" : "false")}");
        }

        public void Visit(ConjunctionExpression expression)
        {
            PrintObject(expression.Position, $"ConjunctionExpression");
            _depth++;
            expression.Left.Visit(this);
            PrintObject(expression.Position, $"and");
            expression.Right.Visit(this);
            _depth--;
        }

        public void Visit(FloatExpression expression)
        {
            PrintObject(expression.Position, $"FloatExpression: {expression.Value}");
        }

        public void Visit(IdentifierExpression expression)
        {
            PrintObject(expression.Position, $"IdentifierExpression: {expression.Name}");
        }

        public void Visit(IntegerExpression expression)
        {
            PrintObject(expression.Position, $"IntegerExpression: {expression.Value}");
        }

        public void Visit(ListExpression expression)
        {
            PrintObject(expression.Position, $"ListExpression:");
            _depth++;
            foreach (var expr in expression.Elements)
            {
                expr.Visit(this);
            }
            _depth--;
        }

        public void Visit(ListLengthExpression expression)
        {
            PrintObject(expression.Position, $"ListLengthExpression: length of '{expression.ListIdentifier}'");
        }

        public void Visit(MultiplicativeExpression expression)
        {
            PrintObject(expression.Position, $"MultiplicativeExpression");
            _depth++;
            expression.Left.Visit(this);
            PrintObject(expression.Position, $"{expression.OperatorType}");
            expression.Right.Visit(this);
            _depth--;
        }

        public void Visit(MyUnaryExpression expression)
        {
            PrintObject(expression.Position, $"UnaryExpression");
            _depth++;
            if (expression.Operator != null)
            {
                PrintObject(expression.Position, $"{expression.Operator}");
            }
            expression.Operand.Visit(this);
            _depth--;
        }

        public void Visit(RelationExpression expression)
        {
            PrintObject(expression.Position, $"RelationExpression");
            _depth++;
            expression.Left.Visit(this);
            PrintObject(expression.Position, $"{expression.OperatorType}");
            expression.Right.Visit(this);
            _depth--;
        }

        public void Visit(StringExpression expression)
        {
            PrintObject(expression.Position, $"StringExpression: '{expression.Value}'");
        }

        public void PrintObject(Position position, string message)
        {
            string indent = new String(' ', _depth*2);
            Console.WriteLine($"{indent}{position}: {message}");
        }

        public void PrintMessage(string message)
        {
            string indent = new String(' ', _depth*2);
            Console.WriteLine($"{indent}{message}");
        }
    }
}
