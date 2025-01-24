using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class ListIndexValue : DOMObject
    {
        public ListIndex? SubIndex;
        public IExpression Value;
        public ListIndexValue(IExpression value, ListIndex? subIndex)
        {
            Value = value;
            SubIndex = subIndex;
        }

        public dynamic GetValue()
        {
            return Value;
        }
    }
}
