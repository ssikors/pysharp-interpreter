using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class ParamValue : DOMObject
    {
        public IDOMType Type;
        public string Identifier;
        public Position Position;
        public bool IsMutable;

        public ParamValue(IDOMType type, string identifier, Position position, bool isMutable)
        {
            Type = type;
            Identifier = identifier;
            Position = position;
            IsMutable = isMutable;
        }

        public dynamic GetValue()
        {
            return Identifier;
        }
    }
}
