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
    public class Declaration : IStatement
    {
        public bool IsMutable;
        public IDOMType Type;
        public string Identifier;
        public IExpression? AssignedTo;
        public Position Position;

        public Declaration( bool isMutable, IDOMType type, string identifier, IExpression? assignedTo, Position position)
        {
            IsMutable = isMutable;
            Type = type;
            Identifier = identifier;
            AssignedTo = assignedTo;
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
