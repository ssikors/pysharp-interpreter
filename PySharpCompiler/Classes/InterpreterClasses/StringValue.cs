using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class StringValue : DOMObject
    {
        public string Value;
        public Position Position;

        public StringValue(string value, Position position)
        {
            CheckValue(value);
            Value = value;
            Position = position;
        }

        public void SetValue(string value)
        {
            CheckValue(value);
            Value = value;
        }

        public dynamic GetValue()
        {
            return Value;
        }

        public void CheckValue(string value)
        {
            if (value.Length > ValueLimits.maxStringLength)
            {
                throw new Exception($"Interpreter error at {Position}: String longer than {ValueLimits.maxStringLength} characters");
            }
        }
    }
}
