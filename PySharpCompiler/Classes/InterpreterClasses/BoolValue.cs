using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class BoolValue : DOMObject
    {
        public bool Value;
        public Position Position;
        public BoolValue(bool value, Position position)
        {
            Value = value;
            Position = position;
        }

        public dynamic GetValue()
        {
            return Value;
        }
    }
}
