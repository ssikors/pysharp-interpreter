using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Statements;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class DeclaredFunction : DOMObject
    {
        public List<IStatement> Statements;
        public List<ParamValue> Parameters;
        public List<DOMObject> BoundArguments = new List<DOMObject>([]);
        public IDOMType ReturnType;
        public Position Position;

        public DeclaredFunction(List<IStatement> statements, List<ParamValue> paramsList, Position position, IDOMType returnType)
        {
            Statements = statements;
            Position = position;
            Parameters = paramsList;
            ReturnType = returnType;
        }

        public List<ParamValue> GetParams()
        {
            return Parameters;
        }

        public IDOMType GetReturnType()
        {
            return ReturnType;
        }

        public dynamic GetValue()
        {
            return this;
        }
    }

    public class PipeFunction : DOMObject
    {
        public List<IStatement> Statements;
        public ParamValue Parameter;
        public IDOMType ReturnType;
        public Position Position;
        public PipeFunction? NextFunction;

        public PipeFunction(List<IStatement> statements, ParamValue param, Position position, IDOMType returnType, PipeFunction? nextFunction)
        {
            Statements = statements;
            Position = position;
            Parameter = param;
            ReturnType = returnType;
            NextFunction = nextFunction;
        }

        public ParamValue GetParams()
        {
            return Parameter;
        }

        public IDOMType GetReturnType()
        {
            return ReturnType;
        }

        public dynamic GetValue()
        {
            return this;
        }
    }

    public class BuiltInFunction : DOMObject
    {

        public Func<List<DOMObject>, DOMObject?>  Function;
        public List<IDOMType> ParamTypes;

        public BuiltInFunction(Func<List<DOMObject>, DOMObject?> function, List<IDOMType> paramTypes)
        {
            Function = function;
            ParamTypes = paramTypes;
        }

        public DOMObject? RunFunction(List<DOMObject> arguments)
        {
            return Function.Invoke(arguments);
        }


        public dynamic GetValue()
        {
            return this;
        }
    }
}
