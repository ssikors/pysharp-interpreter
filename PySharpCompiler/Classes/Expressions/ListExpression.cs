using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.Expressions
{
    public class ListExpression : IExpression
    {
        public List<IExpression> Elements;
        public Position Position;
        public ListExpression(List<IExpression> elements, Position position)
        {
            Elements = elements;
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
