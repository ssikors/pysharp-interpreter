using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class SymbolContainer
    {
        public DOMObject? Value;
        public IDOMType Type;
        public Position PositionDeclared;
        public bool IsMutable;

        public SymbolContainer(DOMObject? value, Position position, bool isMutable, IDOMType type)
        {
            Value = value;
            PositionDeclared = position;
            IsMutable = isMutable;
            Type = type;
        }
    }
}
