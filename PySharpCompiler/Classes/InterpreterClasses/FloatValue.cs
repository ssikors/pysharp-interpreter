using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class FloatValue : DOMObject
    {
        public double Value;
        public Position Position;

        public FloatValue(double value, Position position)
        {
            CheckValue(value);
            Value = value;
            Position = position;
        }

        public void SetValue(double value)
        {
            CheckValue(value);
            Value = value;
        }

        public dynamic GetValue()
        {
            return Value;
        }

        public void CheckValue(double value)
        {
            if (value > ValueLimits.maxFloatValue)
            {
                throw new Exception($"Interpreter error at {Position}: Float too big");
            }
            if (value < ValueLimits.minFloatValue)
            {
                throw new Exception($"Interpreter error at {Position}: Float too small");
            }
        }
    }
}
