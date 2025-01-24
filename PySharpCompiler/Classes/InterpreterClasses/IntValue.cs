using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class IntValue : DOMObject
    {
        public int Value;
        public Position Position;

        public IntValue(int value, Position position)
        {
            CheckValue(value);
            Value = value;
            Position = position;
        }

        public void SetValue(int value)
        {
            CheckValue(value);
            Value = value;
        }

        public dynamic GetValue()
        {
            return Value;
        }

        public void CheckValue(int value)
        {
            if (value > ValueLimits.maxIntValue)
            {
                throw new Exception($"Interpreter error at {Position}: Int too big");
            }
            if (value < ValueLimits.minIntValue)
            {
                throw new Exception($"Interpreter error at {Position}: Int too small");
            }
        }
    }
}
