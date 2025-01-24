using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes
{
    public class Assignable : Visitable
    {
        public string Identifier;
        public ListIndex? Index;
        public Position Position;
        public Assignable(string identifier, ListIndex? index, Position position)
        {
            Identifier = identifier;
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
