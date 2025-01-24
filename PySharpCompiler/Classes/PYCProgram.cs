using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Classes.Statements;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes
{
    public class PYCProgram : Visitable
    {
        public List<IStatement> Statements;
        public PYCProgram(List<IStatement> statements)
        {
            Statements = statements;
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
