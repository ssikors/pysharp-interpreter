using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class Identifier : DOMObject
    {
        public string Name;
        public Position Position;
        public Identifier(string name, Position pos)
        {
            Name = name;
            Position = pos;
        }

        public dynamic GetValue()
        {
            return Name;
        }

        public override string ToString() { return Name; }
    }
}
