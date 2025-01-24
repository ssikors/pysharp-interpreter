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
    public class MultiplicativeExpression : IExpression
    {
        public IExpression Left;
        public OperatorType OperatorType;
        public IExpression Right;
        public Position Position;
        public MultiplicativeExpression(IExpression left, OperatorType operatorType, IExpression right, Position position)
        {
            Left = left;
            OperatorType = operatorType;
            Right = right;
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
