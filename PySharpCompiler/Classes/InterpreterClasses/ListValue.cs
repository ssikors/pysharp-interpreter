using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class ListValue : DOMObject
    {
        public List<DOMObject> Expressions;
        public IDOMType? DataType;
        public Position Position;

        public ListValue(List<DOMObject> expressions, Position pos)
        {
            Expressions = expressions;
            Position = pos;
        }

        public ListValue(List<DOMObject> expressions, Position pos, IDOMType type)
        {
            Expressions = expressions;
            DataType = type;
            Position = pos;
        }

        public dynamic GetValue()
        {
            return Expressions;
        }
    }
}
