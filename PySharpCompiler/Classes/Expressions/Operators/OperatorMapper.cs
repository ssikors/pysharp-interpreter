using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes.Expressions.Operators
{
    public class OperatorMapper
    {
        public OperatorMapper() { }

        public Dictionary<TokenType, OperatorType> mapping = new Dictionary<TokenType, OperatorType>()
        {
            { TokenType.Minus, OperatorType.Minus},
            { TokenType.Negate, OperatorType.Negate },
            { TokenType.Multiply, OperatorType.Multiply },
            { TokenType.Divide, OperatorType.Divide },
            { TokenType.Plus, OperatorType.Plus },
            { TokenType.BindFront, OperatorType.BindFront },
            { TokenType.And, OperatorType.And },
            { TokenType.Or, OperatorType.Or }
        };

        public Dictionary<TokenType, OperatorType> unaryMapping = new Dictionary<TokenType, OperatorType>()
        {
            { TokenType.Minus, OperatorType.Minus},
            { TokenType.Negate, OperatorType.Negate }
        };

        public Dictionary<TokenType, OperatorType> multiplicativeMapping = new Dictionary<TokenType, OperatorType>()
        {
            { TokenType.Multiply, OperatorType.Multiply },
            { TokenType.Divide, OperatorType.Divide },
            { TokenType.BindFront, OperatorType.BindFront },
            { TokenType.Pipe, OperatorType.Pipe },
        };

        public Dictionary<TokenType, OperatorType> additiveMapping = new Dictionary<TokenType, OperatorType>()
        {
            { TokenType.Plus, OperatorType.Plus },
            { TokenType.Minus, OperatorType.Minus }
        };

        public Dictionary<TokenType, OperatorType> relationMapping = new Dictionary<TokenType, OperatorType>()
        {
            { TokenType.MoreThan, OperatorType.MoreThan },
            { TokenType.MoreOrEqual, OperatorType.MoreOrEqual },
            { TokenType.IsEqual, OperatorType.Equal },
            { TokenType.IsNotEqual, OperatorType.NotEqual },
            { TokenType.LessThan, OperatorType.Less },
            { TokenType.LessOrEqual, OperatorType.LessOrEqual }
        };

        public OperatorType Get(TokenType type)
        {
            return mapping[type];
        }

        public OperatorType? GetUnary(TokenType type)
        {
            if (unaryMapping.ContainsKey(type))
            {
                return unaryMapping[type];
            } else
            {
                return null;
            }
        }

        public OperatorType? GetMultiplicative(TokenType type)
        {
            if (multiplicativeMapping.ContainsKey(type))
            {
                return multiplicativeMapping[type];
            }
            else
            {
                return null;
            }
        }

        public OperatorType? GetAdditive(TokenType type)
        {
            if (additiveMapping.ContainsKey(type))
            {
                return additiveMapping[type];
            }
            else
            {
                return null;
            }
        }

        public OperatorType? GetRelation(TokenType type)
        {
            if (relationMapping.ContainsKey(type))
            {
                return relationMapping[type];
            }
            else
            {
                return null;
            }
        }
    }

    public enum OperatorType
    {
        Minus,
        Plus,
        Negate,
        Multiply,
        Divide,
        BindFront,
        And,
        Or,
        Pipe,
        MoreThan,
        MoreOrEqual,
        Equal,
        NotEqual,
        LessOrEqual,
        Less
    }
}
