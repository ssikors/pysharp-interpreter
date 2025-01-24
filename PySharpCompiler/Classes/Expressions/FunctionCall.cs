using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes.Statements
{
    public class FunctionCallExpression : IExpression
    {
        public string Identifier;
        public List<IExpression> Arguments;
        public Position Position;
        public FunctionCallExpression(string identifier, List<IExpression> arguments, Position position)
        {
            Identifier = identifier;
            Arguments = arguments;
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
