
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes
{
    public enum TokenType
    {
        Newline,
        Comment,
        EOF,
        BadToken,
        Identifier,
        String,
        Plus,
        Minus,
        Multiply,
        Divide,
        LessThan,
        LessOrEqual,
        MoreThan,
        MoreOrEqual,
        IsEqual,
        IsNotEqual,
        Assign,
        Negate,
        BindFront,
        Pipe,
        ListLength,
        TypeArrow,
        Semicolon,
        CurlyOpen,
        CurlyClose,
        SquareOpen,
        SquareClose,
        RoundOpen,
        RoundClose,
        Comma,
        Dot,
        BoolType, // Keywords
        IntType,
        FloatType,
        StringType,
        FunctionType,
        ListType,
        Mutable,
        True,
        False,
        Return,
        While,
        Or,
        And,
        If,
        Else,
        Int,
        Float,
        Void,
        Def
    }

    public class Token
    {
        public required TokenType type;
        public object? value;
        public required Position pos;

        public override string ToString()
        {
            if (value != null)
            {
                return $"{type} at {pos}, value: {value}";
            }
            return $"{type} at {pos}";
        }
    }
}
