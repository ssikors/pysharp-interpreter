using PySharpCompiler.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Components
{
    public enum DOMType {
        Int,
        Float,
        Bool,
        String,
        Function,
        List,
        Void
    }
    public class TokenTypeToDOMMapper
    {
        public TokenTypeToDOMMapper() { }

        public Dictionary<TokenType, DOMType> mapping = new Dictionary<TokenType, DOMType>()
        {
            { TokenType.IntType, DOMType.Int },
            { TokenType.FloatType, DOMType.Float },
            { TokenType.BoolType, DOMType.Bool },
            { TokenType.StringType, DOMType.String },
            { TokenType.FunctionType, DOMType.Function },
            { TokenType.ListType, DOMType.List },
            { TokenType.Void, DOMType.Void },
        };

        public DOMType Get(TokenType type)
        {
            return mapping[type];
        }
    }

    public interface IDOMType
    {
        public dynamic GetType();
    }

    public class PrimitiveType : IDOMType
    {
        public DOMType Type;
        public PrimitiveType(DOMType type)
        {
            Type = type;
        }
        public dynamic GetType()
        {
            return Type;
        }
    }

    public class ListType : IDOMType 
    {
        public dynamic GetType()
        {
            return DataType;
        }
        public IDOMType DataType;
        public ListType(IDOMType dataType) { DataType = dataType; }
    }

    public class FunType : IDOMType
    {
        public List<IDOMType> InputType;
        public IDOMType OutputType;

        public dynamic GetType()
        {
            return ( InputType, OutputType );
        }
        public FunType(List<IDOMType> inputType, IDOMType outputType )
        {
            InputType = inputType;
            OutputType = outputType;
        }
    }
}
