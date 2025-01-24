using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.Expressions
{
    public class IndexedExpression : IExpression
    {
        public IExpression Expression;
        public Position Position;
        public ListIndex Index;

        public IndexedExpression(IExpression expression, ListIndex index, Position position)
        {
            Expression = expression;
            Index = index;
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
