using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes.Statements
{
    public class FunctionDefinition : IStatement
    {
        public string Identifier;
        public List<Param>? Params; 
        public List<IStatement>? Statements;
        public IDOMType? ReturnType;
        public Position Position;
        public FunctionDefinition(string identifier, List<Param>? parameters, List<IStatement>? statements, IDOMType? returnType, Position position)
        {
            Identifier = identifier;
            Params = parameters;
            Statements = statements;
            ReturnType = returnType;
            Position = position;
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
