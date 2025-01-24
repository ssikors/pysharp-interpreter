using PySharpCompiler.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Components
{
    public interface ILexer
    {
        public Token GetCurrentToken();
        public Token NextToken();
    }
}
