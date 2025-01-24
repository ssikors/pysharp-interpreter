using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.Statements
{
    public class Assignment : IStatement
    {
        public Assignable Left;
        public IExpression Right;
        public Position Position;
        public Assignment(Assignable left, IExpression right, Position position)
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
