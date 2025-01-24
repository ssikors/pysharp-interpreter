using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes
{
    public abstract class Visitable
    {
        public abstract void Visit(VisualizeVisitor type);
        public abstract dynamic? Visit(Interpreter type);
    }
}
