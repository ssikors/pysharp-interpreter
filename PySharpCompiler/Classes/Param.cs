using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes
{
    public class Param : Visitable
    {
        public IDOMType Type;
        public string Identifier;
        public Position Position;
        public bool IsMutable;

        public Param(IDOMType type, string identifier, Position position, bool isMutable)
        {
            Type = type;
            Identifier = identifier;
            Position = position;
            IsMutable = isMutable;
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
