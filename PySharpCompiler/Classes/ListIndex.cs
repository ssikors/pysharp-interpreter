using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes
{
    public class ListIndex : Visitable
    {
        public ListIndex? SubIndex;
        public IExpression Value;
        public ListIndex(IExpression value, ListIndex? subIndex)
        {
            Value = value;
            SubIndex = subIndex;
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
