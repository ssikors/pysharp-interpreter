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
    public class ConditionalStatement : IStatement
    {
        public IExpression Condition;
        public List<IStatement> TrueBlock;
        public List<IStatement>? ElseBlock;
        public ConditionalStatement? SubStatement;
        public Position Position;

        public ConditionalStatement(IExpression condition, List<IStatement> trueBlock, List<IStatement>? elseBlock, ConditionalStatement? subStatement, Position position)
        {
            Condition = condition;
            TrueBlock = trueBlock;
            ElseBlock = elseBlock;
            SubStatement = subStatement;
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
