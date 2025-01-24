using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Expressions.Operators;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Classes.Statements;

namespace PySharpCompiler.Components
{
    public class Interpreter
    {
        private ExpressionOperationHandler _operationHandler = new ExpressionOperationHandler();
        private Stack<Scope> _scopes = new Stack<Scope>([new Scope("Global")]);

        public Interpreter()
        {
            DeclareBuiltinFunction(new List<IDOMType>([new PrimitiveType(DOMType.String)]), new PrimitiveType(DOMType.Void),
                (expressions) =>
                {
                    var text = expressions.First();
                    Console.WriteLine(text.GetValue());
                    return null;
                },
                "print"
            );

            DeclareBuiltinFunction(new List<IDOMType>([new PrimitiveType(DOMType.String)]), new PrimitiveType(DOMType.String),
                (expressions) =>
                {
                    Console.WriteLine(expressions[0].GetValue());
                    var input = Console.ReadLine();
                    var value = new StringValue(input!.ToString(), new Position(0,0));
                    return value;
                },
                "input"
            );
        }

        public void DeclareBuiltinFunction(List<IDOMType> inputTypes, PrimitiveType outputType, Func<List<DOMObject>, DOMObject?> function, string name)
        {
            BuiltInFunction builtIn = new BuiltInFunction(function, inputTypes);

            DeclareSymbol(new Identifier(name, new Position(0, 0)), builtIn, false, new FunType(inputTypes, outputType));
        }

        public bool DeclareSymbol(Identifier identifier, DOMObject? expr, bool isMutable, IDOMType type)
        {
            foreach (var scope in _scopes)
            {
                if (scope.GetSymbol(identifier.Name) != null)
                {
                    ReportError($"Attempted to redeclare '{identifier}'", identifier.Position);
                }

                if (scope.IsFunCall)
                {
                    // Identical variable names allowed between function call and higher scopes
                    break;
                }
            }

            return _scopes.First().AddSymbol(identifier.Name, expr, isMutable, identifier.Position, type);
        }

        public bool AssignValue(Identifier identifier, DOMObject expr)
        {
            foreach (var scope in _scopes)
            {
                if (scope.IsDeclared(identifier.Name) != false)
                {
                    scope.AssignValue(identifier.Name, expr);
                    return true;
                }
            }

            ReportError($"Cannot find variable {identifier}", identifier.Position);
            return false;
        }

        public DOMObject? GetSymbolValue(Identifier identifier)
        {
            foreach (var scope in _scopes)
            {
                var value = scope.GetSymbol(identifier.Name);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public void AddScope(string name)
        {
            _scopes.Push(new Scope(name));
        }

        public void AddScope(string name, bool isFunc)
        {
            _scopes.Push(new Scope(name, isFunc));
        }

        public void PopScope()
        {
            _scopes.Pop();
        }

        public Stack<Scope> GetScopes()
        {
            return _scopes;
        }

        public DOMObject? Visit(PYCProgram program)
        {
            foreach (var statement in program.Statements)
            {
                var result = statement.Visit(this);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        // Statements
        public DOMObject? Visit(Assignment statement)
        {
            Identifier identifier = (Identifier)statement.Left.Visit(this)!;
            var value = statement.Right.Visit(this);
            AssignValue(identifier, value);
            return null;
        }

        public DOMObject? Visit(ConditionalStatement statement)
        {
            BoolValue condition = _operationHandler.ConvertToBool(statement.Condition.Visit(this));

            AddScope("If statement");
            if (condition.Value)
            {
                foreach (var blockStatement in statement.TrueBlock)
                {
                    if (blockStatement is ReturnStatement)
                    {
                        var result = blockStatement.Visit(this);
                        PopScope();
                        return result;
                    }

                    blockStatement.Visit(this);
                }
            } else
            {
                if (statement.SubStatement != null)
                {
                    var result = statement.SubStatement.Visit(this);
                    if (result != null)
                    {
                        PopScope();
                        return result;
                    }
                } else if (statement.ElseBlock != null)
                {
                    foreach (var blockStatement in statement.ElseBlock)
                    {
                        if (blockStatement is ReturnStatement)
                        {
                            var result = blockStatement.Visit(this);
                            PopScope();
                            return result;
                        }

                        blockStatement.Visit(this);
                    }
                }
            }
            PopScope();
            return null;
        }

        public DOMObject? Visit(Declaration statement)
        {
            DOMObject? expr = null;

            if (statement.AssignedTo != null)
            {
                expr = statement.AssignedTo.Visit(this);
            }

            DeclareSymbol(
                new Identifier(statement.Identifier, statement.Position),
                expr,
                statement.IsMutable,
                statement.Type
            );

            return null;
        }

        public DOMObject? Visit(FunctionCall statement)
        {
            dynamic function = GetSymbolValue(new Identifier(statement.Identifier, statement.Position))!;

            if (function == null)
            {
                ReportError($"Attempted call of undefined function {statement.Identifier}", statement.Position);
                return null;
            }

            List<IExpression> arguments = statement.Arguments;

            return CallFunction(function, arguments, statement.Identifier, statement.Position);
        }

        public DOMObject? Visit(FunctionCallExpression expression)
        {
            dynamic function = GetSymbolValue(new Identifier(expression.Identifier, expression.Position))!;

            if (function == null)
            {
                ReportError($"Attempted call of undefined function {expression.Identifier}", expression.Position);
                return null;
            }

            List<IExpression> arguments = expression.Arguments;

            return CallFunction(function, arguments, expression.Identifier, expression.Position);
        }

        public DOMObject? CallFunction(DeclaredFunction function, List<IExpression> arguments, string identifier, Position pos)
        {
            if (arguments.Count + function.BoundArguments.Count != function.GetParams().Count)
            {
                ReportError($"Arguments do not match the parameters of function {identifier}:\nExpected: {function.GetParams()}\nGot: {arguments}"
                    , pos);
            }
            AddScope($"{identifier}", true);
            var convertedArguments = new List<DOMObject>();
            for (int i = 0; i < function.GetParams().Count; i++)
            {
                var parameter = function.GetParams()[i];

                if (i < function.BoundArguments.Count)
                {
                    DeclareSymbol(new Identifier(parameter.Identifier, pos), function.BoundArguments[i], parameter.IsMutable, parameter.Type);
                } else
                {
                    var argument = arguments[i].Visit(this);
                    var convertedArgument = _operationHandler.TypeConversion(parameter.Type, argument);
                    convertedArguments.Add(convertedArgument);
                    DeclareSymbol(new Identifier(parameter.Identifier, argument!.Position), convertedArgument, parameter.IsMutable, parameter.Type);
                }
            }

            foreach (var functionStatement in function.Statements)
            {
                if (functionStatement is ReturnStatement)
                {
                    var res = _operationHandler.TypeConversion(function.GetReturnType(), functionStatement.Visit(this));
                    PopScope();
                    return res;
                }
                var res2 = functionStatement.Visit(this);
                if (res2 != null)
                {
                    PopScope();
                    return res2;
                }
            }
            PopScope();
            return null;
        }

        public DOMObject? CallFunction(BuiltInFunction function, List<IExpression> arguments, string identifier, Position pos)
        {
            List<DOMObject> convertedArguments = new List<DOMObject>();
            for (int i = 0; i < arguments.Count ; i++)
            {
                if (i > function.ParamTypes.Count)
                {
                    ReportError($"Arguments do not match the types of parameters: {function.ParamTypes}", pos);
                    return null;
                }
                convertedArguments.Add(_operationHandler.TypeConversion(function.ParamTypes[i], arguments[i].Visit(this)));
            }

            return function.RunFunction(convertedArguments);
        }

        public DOMObject? CallFunction(PipeFunction function, List<IExpression> arguments, string identifier, Position pos)
        {
            if (arguments.Count != 1)
            {
                ReportError($"A pipe function can only take in one argument"
                    , pos);
                return null;
            }
            var firstArg = arguments.FirstOrDefault();
            dynamic IDomArg = firstArg!.Visit(this)!;
            var convertedArgument = _operationHandler.TypeConversion(function.Parameter.Type, IDomArg);

            return CallPipeFunction(function, convertedArgument, identifier, pos);
        }

        public DOMObject? CallPipeFunction(PipeFunction function, DOMObject argument, string identifier, Position pos)
        {
            AddScope(identifier);
            DeclareSymbol(new Identifier(function.Parameter.Identifier, pos), argument, function.Parameter.IsMutable, function.Parameter.Type);

            DOMObject? result = null;

            foreach (var functionStatement in function.Statements)
            {
                if (functionStatement is ReturnStatement)
                {
                    result = _operationHandler.TypeConversion(function.GetReturnType(), functionStatement.Visit(this));
                    PopScope();
                    break;
                }
                result = functionStatement.Visit(this);
                if (result != null)
                {
                    PopScope();
                    break;
                }
            }

            if (result == null)
            {
                ReportError("Pipe function element did not return a value", pos);
            }
            else
            {
                if (function.NextFunction != null)
                {
                    return CallPipeFunction(function.NextFunction, result, identifier, pos);
                }
                else
                {
                    return result;
                }
            }

            return null;
        }

        public DOMObject? Visit(FunctionDefinition statement)
        {
            var paramsList = new List<ParamValue>([]);

            var inputTypes = new List<IDOMType>();

            if (statement.Params != null)
            {
                foreach (var expr in statement.Params)
                {
                    ParamValue? param = (ParamValue)expr.Visit(this);
                    paramsList.Add(param);
                    inputTypes.Add(param.Type);
                }
            }

            DeclaredFunction func = new DeclaredFunction((statement.Statements != null ? statement.Statements : new List<IStatement>([])),
                paramsList, statement.Position, (statement.ReturnType != null ? statement.ReturnType : new PrimitiveType(DOMType.Void)));

            DeclareSymbol(
                new Identifier(statement.Identifier, statement.Position),
                func,
                false,
                new FunType(inputTypes, (statement.ReturnType != null ? statement.ReturnType : new PrimitiveType(DOMType.Void)))
            );

            return null;
        }

        public DOMObject? Visit(LoopStatement statement)
        {
            var condition = _operationHandler.ConvertToBool(statement.Condition.Visit(this));
            while (condition!.Value)
            {
                AddScope("While scope");
                foreach (var blockStatement in statement.Block)
                {
                    var result = blockStatement.Visit(this);
                    if (result != null)
                    {
                        PopScope();

                        return result;
                    }
                    
                }

                PopScope();
                condition = _operationHandler.ConvertToBool(statement.Condition.Visit(this));
            }
            
            return null;
        }

        public DOMObject? Visit(ReturnStatement statement)
        {
            if (statement.ReturnedExpression != null)
            {
                return statement.ReturnedExpression.Visit(this);
            }
            return null;
        }


        // Expressions etc

        public DOMObject? Visit(AlternativeExpression expression)
        {
            var left = expression.Left.Visit(this);
            var right = expression.Right.Visit(this);

            var result = _operationHandler.Alternative(left, right);
            return result;
        }

        public DOMObject? Visit(ConjunctionExpression expression)
        {
            var left = expression.Left.Visit(this);
            var right = expression.Right.Visit(this);

            var result = _operationHandler.Conjunction(left, right);
            return result;
        }

        public DOMObject? Visit(RelationExpression expression)
        {
            var left = expression.Left.Visit(this);
            var right = expression.Right.Visit(this);
            var opr = expression.OperatorType;

            var result = _operationHandler.RelationExpression(opr, left, right);
            return result;
        }

        public DOMObject? Visit(AdditiveExpression expression)
        {
            var left = expression.Left.Visit(this);
            var right = expression.Right.Visit(this);
            var opr = expression.OperatorType;

            var result = _operationHandler.AdditiveOperation(opr, left, right);
            return result;
        }

        public dynamic Visit(MultiplicativeExpression expression)
        {
            var left =  expression.Left.Visit(this);
            var right = expression.Right.Visit(this);
            var opr = expression.OperatorType;

            if (opr == OperatorType.Pipe)
            {
                return PipeOperation(left, right);
            }

            if (opr == OperatorType.BindFront)
            {
                return BindFront(left, right);
            }

            var result = _operationHandler.MultiplicativeOperation(opr, left, right);
            return result;
        }

        public DeclaredFunction BindFront(DeclaredFunction func, dynamic right)
        {
            DeclaredFunction boundFunc = new DeclaredFunction(
                func.Statements,
                func.Parameters,
                right.Position,
                func.ReturnType
            );

            boundFunc.BoundArguments = func.BoundArguments;
            boundFunc.BoundArguments.Add(right);

            return boundFunc;
        }

        public PipeFunction? PipeOperation(DeclaredFunction left, DeclaredFunction right)
        {
            if (right.Parameters.Count != 1 || left.Parameters.Count != 1)
            {
                ReportError("A function must have exactly 1 parameter to be part of a pipe", right.Position);
                return null;
            }

            PipeFunction rightPipe = new PipeFunction(
                right.Statements,
                right.Parameters.FirstOrDefault()!,
                right.Position,
                right.ReturnType,
                null
            );

            PipeFunction leftPipe = new PipeFunction(
                left.Statements,
                left.Parameters.FirstOrDefault()!,
                left.Position,
                left.ReturnType,
                rightPipe

            );
            

            return leftPipe;
        }

        public PipeFunction? PipeOperation(DeclaredFunction left, PipeFunction right)
        {
            if (left.Parameters.Count != 1)
            {
                ReportError("A function must have exactly 1 parameter to be part of a pipe", right.Position);
                return null;
            }

            PipeFunction leftPipe = new PipeFunction(
                left.Statements,
                left.Parameters.FirstOrDefault()!,
                left.Position,
                left.ReturnType,
                right
            );

            return leftPipe;
        }

        public PipeFunction? PipeOperation(PipeFunction left, PipeFunction right)
        {
            PipeFunction leftPipe = new PipeFunction(
                left.Statements,
                left.Parameter,
                left.Position,
                left.ReturnType,
                right
            );

            return leftPipe;
        }


        public DOMObject? Visit(MyUnaryExpression expression)
        {
            DOMObject? factor = expression.Operand.Visit(this);
            OperatorType opr = expression.Operator;

            var result = _operationHandler.UnaryOperation(opr, factor!);
            return result;
        }

        public DOMObject? Visit(IndexedExpression expression)
        {
            dynamic? listOrValue = expression.Expression.Visit(this);
            if (listOrValue is not ListValue)
            {
                ReportError($"This >{listOrValue}< expression is not a list", expression.Position);
                return null;
            }
        
            dynamic? index = expression.Index.Visit(this);
            while (index != null)
            {
                var indexValue = index.Value.Visit(this);
                listOrValue = listOrValue!.Expressions[_operationHandler.ConvertToInt(indexValue).Value];
                index = index.SubIndex;
            }

            return listOrValue;
        }


        // FACTORS
        public BoolValue Visit(BooleanExpression expression)
        {
            return new BoolValue(expression.Value, expression.Position);
        }

        public StringValue Visit(StringExpression expression)
        {
            return new StringValue(expression.Value, expression.Position);
        }

        public FloatValue Visit(FloatExpression expression)
        {
            return new FloatValue(expression.Value, expression.Position);
        }

        public DOMObject? Visit(IdentifierExpression expression)
        {
            var identifier =  new Identifier(expression.Name, expression.Position);
            return GetSymbolValue(identifier);
        }

        public IntValue Visit(IntegerExpression expression)
        {
            return new IntValue(expression.Value, expression.Position);
        }

        public DOMObject? Visit(ListExpression expression)
        {
            List<DOMObject> expressions = new List<DOMObject>();
            foreach (var expr in expression.Elements)
            {
                expressions.Add(expr.Visit(this));
            }
            return new ListValue(expressions, expression.Position);
        }

        public DOMObject? Visit(ListLengthExpression expression)
        {
            var listValue = GetSymbolValue(new Identifier(expression.ListIdentifier, expression.Position));

            if (listValue is ListValue)
            {
                ListValue list = (ListValue) listValue;
                return new IntValue(list.Expressions.Count, expression.Position);
            }
            else
            {
                ReportError($"Cannot find list named '{expression.ListIdentifier}'", expression.Position);
                return null;
            }
        }

        public DOMObject Visit(Assignable expression)
        {
            if (expression.Index != null)
            {
                try
                {
                    dynamic? listOrValue = (ListValue)GetSymbolValue(new Identifier(expression.Identifier, expression.Position));
                    dynamic? index = expression.Index.Visit(this);
                    while (index != null)
                    {
                        Console.WriteLine(index.Value.Visit(this));
                        listOrValue = listOrValue!.Expressions[_operationHandler.ConvertToInt(index.Value.Visit(this)).Value];
                        index = index.SubIndex;
                    }

                    return listOrValue;
                } catch
                {
                    ReportError($"No list such as '{expression.Identifier}'", expression.Position);
                }
            }
            return new Identifier(expression.Identifier, expression.Position);
        }

        public dynamic? Visit(ListIndex expression)
        {
            return new ListIndexValue(expression.Value, expression.SubIndex);
        }

        public DOMObject? Visit(Param expression)
        {
            return new ParamValue(expression.Type, expression.Identifier, expression.Position, expression.IsMutable);
        }

        public void ReportError(string message, Position position)
        {
            throw new Exception($"Interpreter error at {position}: {message}");
        }
    }
}
