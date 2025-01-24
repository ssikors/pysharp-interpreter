using PySharpCompiler.Classes;
using PySharpCompiler.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Components
{
    static class ValueLimits
    {
        public const int maxCommentLength = 256;
        public const int maxIdentifierLength = 64;
        public const int maxStringLength = 512000;

        public const int maxIntLength = 10;
        public const int maxIntValue = 1000000000;
        public const int minIntValue = -1000000000;

        public const int maxFloatValue = 1000000000;
        public const int minFloatValue = -1000000000;

        public const int maxFractionLength = 32;
    }

    public class Lexer : ILexer
    {
        private Position _position = null;
        private TokenType _tokenType;
        private object? _tokenValue;

        private CodeReader _codeReader;

        public Dictionary<string, int> keywords = new Dictionary<string, int> {

            { "bool", (int)TokenType.BoolType },
            { "int", (int)TokenType.IntType },
            { "float", (int)TokenType.FloatType },
            { "string", (int)TokenType.StringType },
            { "function", (int)TokenType.FunctionType },
            { "list", (int)TokenType.ListType },
            { "mut", (int)TokenType.Mutable },
            { "true", (int)TokenType.True },
            { "false", (int)TokenType.False },
            { "return", (int)TokenType.Return },
            { "while", (int)TokenType.While },
            { "or", (int)TokenType.Or },
            { "and", (int)TokenType.And },
            { "if", (int)TokenType.If },
            { "else", (int)TokenType.Else },
            { "void", (int)TokenType.Void },
            { "def", (int)TokenType.Def }
        };

        public Lexer(CodeReader codeReader)
        {
            _codeReader = codeReader;
            _codeReader.NextChar();
        }


        public Token NextToken()
        {
            
            MoveToNextToken();

            _position = new Position (_codeReader.GetPosition().Line, _codeReader.GetPosition().Column);

            var currentChar = _codeReader.CurrentChar();
            Token token = ParseToken(currentChar);
            return token;
        }

        public Token ParseToken(char firstChar)
        {

            if (TryParseNumber(firstChar) != null ||
                TryParseIdentifier(firstChar) != null ||
                TryParseString(firstChar) != null ||
                TryParseComment(firstChar) != null ||
                TryParseOperator(firstChar) != null 
                )
            {
                return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
            }

            _tokenType = TokenType.BadToken;
            ReportWarning(_position, "Bad token");

            return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
        }

        private Token? TryParseString(char firstChar)
        {
            if (firstChar != '\'')
            {
                return null;
            }
            
            StringBuilder stringBuilder = new StringBuilder();

            var currentChar = _codeReader.NextChar();
            int length = 0;
            while (length <= ValueLimits.maxStringLength)
            {
                if (currentChar == '\'')
                {
                    _codeReader.NextChar();
                    break;
                }

                if (currentChar == '\\')
                {
                    if (_codeReader.PeekChar() == 'n')
                    {
                        stringBuilder.Append('\n');
                        _codeReader.NextChar();
                    } else if (_codeReader.PeekChar() == '\\')
                    {
                        stringBuilder.Append('\\');
                        _codeReader.NextChar();
                    } else if (_codeReader.PeekChar() == 't')
                    {
                        stringBuilder.Append('\t');
                        _codeReader.NextChar();
                    } else if (_codeReader.PeekChar() == '\'')
                    {
                        stringBuilder.Append('\'');
                        _codeReader.NextChar();
                    } else
                    {
                        stringBuilder.Append(currentChar);
                    }
                    currentChar = _codeReader.NextChar();
                    length++;
                    continue;
                }

                stringBuilder.Append(currentChar);
                length++;
                currentChar = _codeReader.NextChar();
            }

            if (length > ValueLimits.maxStringLength)
            {
                ReportError(_position, "String length exceeded");
                _tokenType = TokenType.BadToken;
                return null;
            }

            var stringContent = stringBuilder.ToString();
            _tokenValue = stringContent;
            _tokenType = TokenType.String;

            return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
        }

        private Token? TryParseComment(char firstChar)
        {
            if (firstChar != '#')
            {
                return null;
            }

            StringBuilder stringBuilder = new StringBuilder();

            char currentChar = _codeReader.NextChar();
            int length = 0;
            while (currentChar != '\n' & currentChar != '\0' & length <= ValueLimits.maxCommentLength)
            {
                stringBuilder.Append(currentChar);
                length++;
                currentChar = _codeReader.NextChar();
            }

            if (length > ValueLimits.maxCommentLength)
            {
                ReportError(_position, "Comment length exceeded");
                _tokenType = TokenType.BadToken;
                return null;
            }

            var comment = stringBuilder.ToString();
            _tokenValue = comment;
            _tokenType = TokenType.Comment;

            return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
        }

        private Token? TryParseIdentifier(char firstChar)
        {
            if (!char.IsLetterOrDigit(firstChar))
            {
                return null;
            }

            StringBuilder stringBuilder = new StringBuilder();

            var currentChar = firstChar;
            int length = 0;
            while (char.IsLetterOrDigit(currentChar) & length <= ValueLimits.maxIdentifierLength)
            {
                stringBuilder.Append(currentChar);
                length++;
                currentChar = _codeReader.NextChar();
            }

            if (length > ValueLimits.maxIdentifierLength)
            {
                ReportError(_position, "The identifier is too long");
                _tokenType = TokenType.BadToken;
                return null;
            }

            var identifier = stringBuilder.ToString();

            if (keywords.ContainsKey(identifier))
            {
                var keyword = keywords[identifier];
                _tokenType = (TokenType)keyword;
            }
            else
            {
                _tokenValue = identifier;
                _tokenType = TokenType.Identifier;
            }

            return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
        }

        private Token? TryParseNumber(char firstChar)
        {
            if (!(char.IsDigit(firstChar) || firstChar == '.'))
            {
                return null;
            }

            int length = 0;
            int value = 0;
            var currentChar = firstChar;
            while ((char.IsDigit(currentChar) || currentChar == '.' ) & length <= ValueLimits.maxIntLength)
            {
                if (currentChar == '.')
                {
                    _codeReader.NextChar();
                    var fraction = GetFraction();
                    if (fraction != null)
                    {
                        double floatValue = value + (double)fraction;
                        _tokenType = TokenType.Float;
                        _tokenValue = floatValue;
                        return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
                    } else
                    {
                        return null;
                    };
                }
                value = value * 10 + currentChar - '0';
                currentChar = _codeReader.NextChar();
                length++;
            }
            
            if (length > ValueLimits.maxIntLength)
            {
                ReportError(_position, "The number is too big to parse");
                _tokenType = TokenType.BadToken;
                return null;
            }

            _tokenType = TokenType.Int;
            _tokenValue = value;

            return new Token { pos= _position!, type=_tokenType, value=_tokenValue};
        }

        public double? GetFraction()
        {
            var currentChar = _codeReader.CurrentChar();
            var length = 0;
            int fractionPart = 0;
            int exponent = 0;
            while ((char.IsDigit(currentChar)) & length <= ValueLimits.maxFractionLength) {
                fractionPart = fractionPart * 10 + currentChar - '0';
                currentChar = _codeReader.NextChar();
                length++;
                exponent++;
            }

            if (length > ValueLimits.maxFractionLength)
            {
                ReportError(_position, "The fraction is too big to parse");
                _tokenType = TokenType.BadToken;
                return null;
            }

            double fraction = (fractionPart / Math.Pow(10, exponent));

            return fraction;
        }

        private Token? TryParseOperator(char firstChar)
        {
            switch( firstChar )
            { 
                case '\0':
                    _tokenType = TokenType.EOF;
                    break;
                case '\n':
                    _tokenType = TokenType.Newline;
                    break;
                /* Operators */
                case '+':
                    _tokenType = TokenType.Plus;
                    break;
                case '-': // - lub ->
                    if (_codeReader.PeekChar() == '>')
                    {
                        _tokenType = TokenType.TypeArrow;
                        _codeReader.NextChar();
                        break;
                    }
                    _tokenType = TokenType.Minus;
                    break;
                case '*':
                    _tokenType = TokenType.Multiply;
                    break;
                case '/':
                    _tokenType = TokenType.Divide;
                    break;
                case '=': // = lub ==
                    if (_codeReader.PeekChar() == '=')
                    {
                        _tokenType = TokenType.IsEqual;
                        _codeReader.NextChar();
                        break;
                    }
                    _tokenType = TokenType.Assign;
                    break;
                case '>': // > lub >= 
                    if (_codeReader.PeekChar() == '=')
                    {
                        _tokenType = TokenType.MoreOrEqual;
                        _codeReader.NextChar();
                        break;
                    }
                    _tokenType = TokenType.MoreThan;
                    break;
                case '<': // < lub <= lub list<>
                    if (_codeReader.PeekChar() == '=')
                    {
                        _tokenType = TokenType.LessOrEqual;
                        _codeReader.NextChar();
                        break;
                    }
                    _tokenType = TokenType.LessThan;
                    break;
                case '!': //  ! lub !=
                    if (_codeReader.PeekChar() == '=')
                    {
                        _tokenType = TokenType.IsNotEqual;
                        _codeReader.NextChar();
                        break;
                    }
                    _tokenType = TokenType.Negate;
                    break;
                case '%': // bind front
                    _tokenType = TokenType.BindFront;
                    break;
                case '|': // potok  lub dlugosc listy
                    if (_codeReader.PeekChar() == '>')
                    {
                        _tokenType = TokenType.Pipe;
                        _codeReader.NextChar();
                        break;
                    }
                    _tokenType = TokenType.ListLength;
                    break;
                /* utilities */
                case ';':
                    _tokenType = TokenType.Semicolon;
                    break;
                case '{':
                    _tokenType = TokenType.CurlyOpen;
                    break;
                case '}':
                    _tokenType = TokenType.CurlyClose;
                    break;
                case '[':
                    _tokenType = TokenType.SquareOpen;
                    break;
                case ']':
                    _tokenType = TokenType.SquareClose;
                    break;
                case ',':
                    _tokenType = TokenType.Comma;
                    break;
                case '(':
                    _tokenType = TokenType.RoundOpen;
                    break;
                case ')':
                    _tokenType = TokenType.RoundClose;
                    break;
                case '.':
                    _tokenType = TokenType.Dot;
                    break;
                default:
                    _tokenType = TokenType.BadToken;
                    break;
            }
            _tokenValue = null;
            _codeReader.NextChar();
            return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
        }

        public Token GetCurrentToken()
        {
            if (_position == null)
            {
                NextToken();
            }
            return new Token { pos = _position!, type = _tokenType, value = _tokenValue };
        }


        public void MoveToNextToken()
        {
            SkipWhitespaces();
            _tokenValue = null;
        }


        public void SkipWhitespaces()
        {
            char currentChar = _codeReader.CurrentChar();
            
            while (currentChar == ' ' || currentChar == '\t' || currentChar == '\n')
            {
                _codeReader.NextChar();
                currentChar = _codeReader.CurrentChar();
            }
        }

        public void ReportError(Position position, string message)
        {
            throw new Exception($"Encountered a critical error at {position}: {message}");
        }

        public void ReportWarning(Position position, string message)
        {
            Console.WriteLine($"Warning at {position}: {message}");
        }
    }
}

