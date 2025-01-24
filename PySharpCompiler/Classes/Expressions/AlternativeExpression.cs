using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.Expressions
{
    public class AlternativeExpression : IExpression
    {
        public IExpression Left;
        public IExpression Right;
        public Position Position;
        public AlternativeExpression(IExpression left, IExpression right, Position position)
        {
            Left = left;
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
