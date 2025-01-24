using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.Statements
{
    public class LoopStatement : IStatement
    {
        public IExpression Condition;
        public List<IStatement> Block;
        public Position Position;
        public LoopStatement(IExpression condition, List<IStatement> block, Position position)
        {
            Condition = condition;
            Block = block;
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
