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
    public class ReturnStatement : IStatement
    {
        public IExpression? ReturnedExpression;
        public Position Position;
        public ReturnStatement(IExpression? expression, Position position)
        {
            ReturnedExpression = expression;
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
