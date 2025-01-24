using PySharpCompiler.Classes.Expressions.Operators;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes.Expressions
{
    public class MyUnaryExpression : IExpression
    {
        public OperatorType Operator;
        public IExpression Operand;
        public Position Position;
        public MyUnaryExpression(OperatorType oprator, IExpression operand, Position position)
        {
            Operator = oprator;
            Operand = operand;
            Position = position;
        }

        public override void Visit(VisualizeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override DOMObject? Visit(Interpreter visitor)
        {
            return visitor.Visit(this);
        }
    }
}
