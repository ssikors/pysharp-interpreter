using PySharpCompiler.Classes;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Expressions.Operators;
using PySharpCompiler.Classes.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PySharpCompiler.Components
{
    public class Parser
    {
        ILexer _lexer;

        TokenTypeToDOMMapper _typeMapper = new TokenTypeToDOMMapper();
        OperatorMapper _operatorMapper = new OperatorMapper();

        public Parser(ILexer lexer) {
            _lexer = lexer;
        }

        public PYCProgram ParseProgram()
        {
            List<IStatement> statements = new List<IStatement>();

            // program = { statement }, end_of_file;
            IStatement? statement = ParseStatement();
            while (statement != null)
            {
                statements.Add(statement);
                statement = ParseStatement();
            }
            MustBe(CurrentToken(), TokenType.EOF, "Expected EOF after the last statement");

            return new PYCProgram(statements);
        }

        public IStatement? ParseStatement()
        { 
            IStatement? statement = ParseFunctionDefinition();
            if ( statement != null){ return statement; };
            statement = ParseDeclaration();
            if (statement != null) { return statement; };
            statement = ParseAssignmentOrCall();
            if (statement != null) { return statement; };
            statement = ParseReturnStatement();
            if (statement != null) { return statement; };
            statement = ParseConditionalStatement();
            if (statement != null) { return statement; };
            statement = ParseLoopStatement();
            if (statement != null) { return statement; };
            return null;
        }

        // "def", identifier, "(", params, ")", [ "->", type ], block, ";"
        public FunctionDefinition? ParseFunctionDefinition()
        {
            if (CurrentToken().type != TokenType.Def)
            {
                return null;
            }

            var pos = GetPosition();

            MustBe(NextToken(), TokenType.Identifier, "Syntax error: expected identifier");

            string identifier = CurrentToken().value!.ToString()!;

            NextToken();
            var parameters = ParseParams();

            IDOMType? outputType = new PrimitiveType(DOMType.Void);
            if (CurrentToken().type == TokenType.TypeArrow) {
                NextToken();
                outputType = ParseType();

                if (outputType == null) { ReportError(CurrentToken().pos, "Syntax error: no type after type arrow"); }
            }

            var blockStatements = ParseBlock();

            MustBe(CurrentToken(), TokenType.Semicolon, "Syntax error: expected semicolon");
            NextToken();
            var fundef = new FunctionDefinition(identifier, parameters, blockStatements, outputType, pos);
            return fundef;
        }

        // params = [ [ "mut" ], type, identifier, { ",", [ "mut" ], type, identifier } ];
        public List<Param>? ParseParams()
        {
            if (CurrentToken().type != TokenType.RoundOpen)
            {
                return null;
            }
            var parameters = new List<Param>();
            
            NextToken();
            var parameter = ParseParam();
            while (parameter != null) {
                parameters.Add(parameter);
                if (CurrentToken().type == TokenType.Comma)
                {
                    NextToken();
                }
                parameter = ParseParam();
            }
            MustBe(CurrentToken(), TokenType.RoundClose, "Syntax error: expected closing round bracket");

            NextToken();
            return parameters;
        }

        public Param? ParseParam()
        {
            var pos = GetPosition();
            bool isMutable = ParseMutable();

            var type = ParseType();
            if (type == null)
            {
                return null;
            }

            var token = CurrentToken();
            MustBe(token, TokenType.Identifier, "Syntax error: expected identifier");
            string identifier = token.value!.ToString()!;

            Param parameter = new Param(type, identifier, pos, isMutable);
            NextToken();
            return parameter;
        }

        // block = "{", { statement }, "}";
        public List<IStatement> ParseBlock()
        {
            var statements = new List<IStatement>();

            MustBe(CurrentToken(), TokenType.CurlyOpen, "expected a '{' sign at the start of a block statement");
            NextToken();
            var statement = ParseStatement();
            while (statement != null)
            {
                statements.Add(statement!);
                statement = ParseStatement();
            }
            MustBe(CurrentToken(), TokenType.CurlyClose, "expected a '}' sign at the end of a block statement");

            NextToken();
            return statements;
        }

        public bool ParseMutable()
        {
            if (CurrentToken().type == TokenType.Mutable) { 
                NextToken();
                return true;
            } else { return false; }
        }

        // type = primitive_type | list_type | function_type;
        public IDOMType? ParseType()
        {
            //primitive_type = "int"| "float"| "bool"| "string| "void";
            TokenType[] types = [TokenType.IntType, TokenType.FloatType, TokenType.BoolType, TokenType.FunctionType, TokenType.StringType, TokenType.ListType, TokenType.Void];
            // MustBeIn(CurrentToken(), types, "Syntax error: expected a type");
            if (Array.IndexOf(types, CurrentToken().type) == -1)
            {
                return null;
            }

            var funType = ParseFunctionType();
            if (funType != null)
            {
                return funType;
            }

            var listType = ParseListType();
            if (listType != null)
            {
                return listType;
            }

            PrimitiveType type = new PrimitiveType(_typeMapper.Get(CurrentToken().type));
            NextToken();
            return type;
        }

        // function_type = "function","<", type, { ",", type } ">", "->", type;
        public FunType? ParseFunctionType()
        {
            if (CurrentToken().type != TokenType.FunctionType)
            {
                return null;
            }

            MustBe(NextToken(), TokenType.LessThan, "Syntax error: expected a '<' sign");

            var inputTypes = new List<IDOMType>();

            NextToken();
            var inputType = ParseType();
            while (inputType != null)
            {
                
                inputTypes.Add(inputType!);
                if (CurrentToken().type == TokenType.Comma)
                {
                    NextToken();
                }
                inputType = ParseType();
            }
            MustBe(CurrentToken(), TokenType.MoreThan, "Syntax error: expected a '>' sign");
            MustBe(NextToken(), TokenType.TypeArrow, "Syntax error: expected a '->' sign");
            NextToken();
            var outputType = ParseType();

            if (outputType == null)
            {
                ReportError(GetPosition(), "Syntax error: expected output type");
            }

            var funType = new FunType(inputTypes, outputType!);

            return funType;
        }

        // list_type = "list", "<", type, ">";
        public ListType? ParseListType()
        {
            if (CurrentToken().type != TokenType.ListType)
            {
                return null;
            }

            MustBe(NextToken(), TokenType.LessThan, "Syntax error: expected a '<' sign");
            NextToken();
            var type = ParseType();
            if (type == null)
            {
                ReportError(GetPosition(), "Syntax error: no list element type");
                return null;
            }
            var listType = new ListType(type);
            MustBe(CurrentToken(), TokenType.MoreThan, "Syntax error: expected a '>' sign");
            NextToken();
            return listType;
        }

        //declaration = [ "mut" ], type, identifier, [ "=", expression ], ";";
        public Declaration? ParseDeclaration()
        {
            
            var pos = GetPosition();
            var isMutable = ParseMutable();

            var type = ParseType();

            if (type == null)
            {
                return null;
            }

            string identifier = CurrentToken().value!.ToString()!;

            NextToken();

            IExpression? expression = null;
            if (CurrentToken().type == TokenType.Assign) {
                NextToken();
                expression = ParseExpression();

                if (expression == null) {
                    ReportError(CurrentToken().pos, "Syntax error: expected assignment after '=");
                }
            } 
            MustBe(CurrentToken(), TokenType.Semicolon, "Syntax error: a declaration must end with a semicolon"); 
            NextToken();
            return new Declaration(isMutable, type, identifier, expression, pos);
        }

        //assignment  = assignable, "=", expression, ";";
        public IStatement? ParseAssignmentOrCall()
        {
            if (CurrentToken().type != TokenType.Identifier)
            {
                return null;
            }
            var pos = GetPosition();

            // assignable = identifier | list_element;
            string identifier = CurrentToken().value!.ToString()!;

            NextToken();

            var arguments = ParseArguments();
            if (arguments != null)
            {
                MustBe(CurrentToken(), TokenType.Semicolon, "Syntax error: expected a semicolon");
                NextToken();
                return new FunctionCall(identifier, arguments, pos);
            }

            // list_element = identifier, { "[", integer, "]"};

            ListIndex? index = ParseIndex();

            MustBe(CurrentToken(), TokenType.Assign, "Syntax error: expected an '=' sign");
            
            NextToken();
            var expression = ParseExpression();

            if (expression == null) {
                ReportError(CurrentToken().pos, "Syntax error: no assigned expression in assignment");
                return null;
            }

            MustBe(CurrentToken(), TokenType.Semicolon, "Syntax error: expected a semicolon");

            var assignment = new Assignment(new Assignable(identifier, index, pos), expression, pos);
            NextToken();
            return assignment;
        }

        public ListIndex? ParseIndex()
        {
            if (CurrentToken().type != TokenType.SquareOpen)
            {
                return null;
            }

            NextToken();
            var index = ParseExpression();
            if (index == null)
            {
                ReportError(GetPosition(), "Syntax error: missing index");
                return null;
            }
            MustBe(CurrentToken(), TokenType.SquareClose, "Syntax error: expected a ']' after index");
            
            NextToken();
            ListIndex? subIndex = ParseIndex();

            ListIndex listIndex = new ListIndex(index, subIndex);

            return listIndex;
        }

        public ReturnStatement? ParseReturnStatement()
        {
            if (CurrentToken().type != TokenType.Return)
            {
                return null;
            }
            var pos = GetPosition();

            NextToken();
            var expression = ParseExpression();
            MustBe(CurrentToken(), TokenType.Semicolon, "Syntax error: expected a semicolon");
            NextToken();
            return new ReturnStatement(expression, pos);
        }

        public List<IExpression>? ParseArguments()
        {
            if (CurrentToken().type != TokenType.RoundOpen)
            {
                return null;
            }

            var expressions = new List<IExpression>();

            NextToken();
            var expression = ParseExpression();
            while (expression != null)
            {
                expressions.Add(expression!);
                if (CurrentToken().type == TokenType.Comma)
                {
                    NextToken();
                }
                expression = ParseExpression();
            }
            MustBe(CurrentToken(), TokenType.RoundClose, "Syntax error: expected a ')' after arguments");
            NextToken();
            return expressions;
        }

        // conditional_statement = "if", "(", expression, ")", block, { "else", conditional_statement}, [ "else", block];
        public ConditionalStatement? ParseConditionalStatement()
        {
            if (CurrentToken().type != TokenType.If)
            {
                return null;
            }
            var pos = GetPosition();

            MustBe(NextToken(), TokenType.RoundOpen, "Syntax error: expected an open round bracket");

            NextToken();
            var condition = ParseExpression();

            if (condition == null) {
                ReportError(CurrentToken().pos, "Syntax error: Missing expression in if statement");
                return null;
            }

            MustBe(CurrentToken(), TokenType.RoundClose, "Syntax error: expected a closed round bracket");

            NextToken();
            var trueBlock = ParseBlock();

            var token = CurrentToken();

            ConditionalStatement conditional = new ConditionalStatement(condition, trueBlock, null, null, pos);

            if (token.type == TokenType.Else)
            {
                token = NextToken();
                if (token.type == TokenType.If)
                {
                    var subConditional = ParseConditionalStatement();

                    conditional.SubStatement = subConditional;
                } else
                {
                    var elseBlock = ParseBlock();
                    conditional.ElseBlock = elseBlock;
                }
            }
            return conditional;
        }

        // loop_statement = "while", "(", expression, ")", block, ";";
        public LoopStatement? ParseLoopStatement()
        {
            if (CurrentToken().type != TokenType.While) {
                return null;
            }
            var pos = GetPosition();

            MustBe(NextToken(), TokenType.RoundOpen, "Syntax error: expected open round bracket");

            NextToken();
            var condition = ParseExpression();

            if (condition == null) {
                ReportError(CurrentToken().pos, "Syntax error: A while statement must have a condition");
                return null;
            }

            MustBe(CurrentToken(), TokenType.RoundClose, "Syntax error: expected closed round bracket");

            NextToken();
            var block = ParseBlock();

            MustBe(CurrentToken(), TokenType.Semicolon, "Syntax error: expected a semicolon");
        
            var loopStatement = new LoopStatement(condition, block, pos);
            NextToken();
            return loopStatement;
        }

        // expression = conjunction, { "or", conjunction };
        public IExpression? ParseExpression()
        {
            var pos = GetPosition();
            IExpression? left = ParseConjunctionTerm();

            if (left == null) { return null; }

            while (CurrentToken().type == TokenType.Or)
            {
                NextToken();
                IExpression? right = ParseConjunctionTerm();
                if (right == null)
                {
                    ReportError(GetPosition(), "Syntax error: expected expression after relation operator");
                    return null;
                }

                left = new AlternativeExpression(left, right, pos);
            }

            return left;
        }

        //conjunction = relation_term, { "and", relation_term };
        public IExpression? ParseConjunctionTerm()
        {
            var pos = GetPosition();
            IExpression? left = ParseRelationTerm();

            if (left == null) { return null; }

            while (CurrentToken().type == TokenType.And)
            {
                NextToken();
                IExpression? right = ParseRelationTerm();
                if (right == null)
                {
                    ReportError(GetPosition(), "Syntax error: expected expression after relation operator");
                    return null;
                }

                left = new ConjunctionExpression(left, right, pos);
            }

            return left;
        }

        // relation_term = additive_term, [ (relation_operator ), additive_term ];
        public IExpression? ParseRelationTerm()
        {
            var pos = GetPosition();
            IExpression? left = ParseAdditiveTerm();

            if (left == null) { return null; }

            if (_operatorMapper.GetRelation(CurrentToken().type) != null)
            {
                var operatorType = _operatorMapper.GetRelation(CurrentToken().type);
                NextToken();
                IExpression? right = ParseAdditiveTerm();
                if (right == null)
                {
                    ReportError(GetPosition(), "Syntax error: expected expression after relation operator");
                    return null;
                }

                left = new RelationExpression(left, (OperatorType)operatorType!, right, pos);
            }

            return left;
        }

        // additive_term = multiplicative_term, { ("+" | "-"), multiplicative_term };
        public IExpression? ParseAdditiveTerm()
        {
            var pos = GetPosition();
            IExpression? left = ParseMultiplicativeTerm();

            if (left == null) { return null; }

            while (_operatorMapper.GetAdditive(CurrentToken().type) != null)
            {
                var operatorType = _operatorMapper.GetAdditive(CurrentToken().type);
                NextToken();
                IExpression? right = ParseMultiplicativeTerm();
                if (right == null)
                {
                    ReportError(GetPosition(), "Syntax error: expected expression after additive operator");
                    return null;
                }

                left = new AdditiveExpression(left, (OperatorType)operatorType!, right, pos);
            }

            return left;
        }

        // multiplicative_term = unary, { ( "*" | "/" | "%" | "|>" ), unary };
        public IExpression? ParseMultiplicativeTerm()
        {
            var pos = GetPosition();
            IExpression? left = ParseUnaryTerm();

            if (left == null) { return null; }

            while (_operatorMapper.GetMultiplicative(CurrentToken().type) != null)
            {
                var operatorType = _operatorMapper.GetMultiplicative(CurrentToken().type);
                NextToken();
                IExpression? right = ParseUnaryTerm();
                if (right == null) {
                    ReportError(GetPosition(), "Syntax error: expected expression after multiplicative operator");
                    return null;
                }

                left = new MultiplicativeExpression(left, (OperatorType)operatorType!, right, pos);
            }

            return left;
        }

        // unary = [ ("-" | "!" | "+"  ) ], factor;
        public IExpression? ParseUnaryTerm()
        {
            var pos = GetPosition();
            IExpression? factor;
            OperatorType? unaryOperator = null;
            if (CurrentToken().type == TokenType.Minus || CurrentToken().type == TokenType.Negate || CurrentToken().type == TokenType.Plus)
            {
                unaryOperator = _operatorMapper.Get(CurrentToken().type);
                 
                NextToken();
                factor = ParseIndexedTerm();
                if (factor == null)
                {
                    ReportError(CurrentToken().pos, "Syntax error: no factor after operator");
                    return null;
                }

                return new MyUnaryExpression((OperatorType)unaryOperator, factor, pos);
            }

            factor = ParseIndexedTerm();
            return factor;
        }


        public IExpression? ParseIndexedTerm()
        {
            var pos = GetPosition();
            var factor = ParseFactor();
            if (factor == null)
            {
                return null;
            }

            var index = ParseIndex();

            if (index == null)
            {
                return factor;
            } else
            {
                return new IndexedExpression(factor, index, pos);
            }
            
        }

        // factor = identifier | string | integer | float | bool | function_call | list | list_length | "(", expression ,")";
        public IExpression? ParseFactor()
        {
            IExpression? factor = ParseIdentifier();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseFloat();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseString();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseBool();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseListExpression();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseListLength();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseInteger();
            if (factor != null)
            {
                return factor;
            }
            factor = ParseNestedExpression();
            return factor;
        }

        public IExpression? ParseNestedExpression()
        {
            if (CurrentToken().type != TokenType.RoundOpen)
            {
                return null;
            }

            NextToken();
            var expression = ParseExpression();
            if (expression == null)
            {
                ReportError(CurrentToken().pos, "Syntax error: expected a nested expression");
            }
            MustBe(CurrentToken(), TokenType.RoundClose, "Syntax error: expected ')' at the end of nested expression");

            NextToken();

            return expression;
        }

        public IExpression? ParseIdentifier()
        {
            if (CurrentToken().type != TokenType.Identifier)
            {
                return null;
            }
            var pos = GetPosition();

            var identifier = CurrentToken().value!.ToString();
            
            NextToken();

            var funCall = ParseFunCallExpression(identifier!);
            if (funCall != null)
            {
                return funCall;
            }
            
            return new IdentifierExpression(identifier!, pos);
        }
        public StringExpression? ParseString()
        {
            if (CurrentToken().type != TokenType.String)
            {
                return null;
            }
            var pos = GetPosition();
            var value = CurrentToken().value!.ToString();

            NextToken();
            return new StringExpression(value!, pos);
        }
        public IntegerExpression? ParseInteger()
        {
            if (CurrentToken().type != TokenType.Int) { 
                return null;
            }
            var pos = GetPosition();
            var value = (int)CurrentToken().value!;

            NextToken();
            return new IntegerExpression(value, pos);
        }
        public FloatExpression? ParseFloat()
        {
            if (CurrentToken().type != TokenType.Float)
            {
                return null;
            }
            var pos = GetPosition();
            var value = (double)CurrentToken().value!;

            NextToken();
            return new FloatExpression(value, pos);
        }
        public BooleanExpression? ParseBool()
        {
            var pos = GetPosition();
            if (CurrentToken().type == TokenType.True)
            {
                NextToken();
                return new BooleanExpression(true, pos);
            } else if (CurrentToken().type == TokenType.False)
            {
                NextToken();
                return new BooleanExpression(false, pos);
            }

            return null;
        }
        public FunctionCallExpression? ParseFunCallExpression(string identifier)
        {
            var pos = GetPosition();
            var arguments = ParseArguments();
            if (arguments != null)
            { return new FunctionCallExpression(identifier, arguments, pos); }
            return null;
        }
        public ListExpression? ParseListExpression()
        {
            if (CurrentToken().type != TokenType.SquareOpen)
            {
                return null;
            }
            var pos = GetPosition();

            List<IExpression> elements = new List<IExpression>();

            var token = NextToken();
            while (token.type != TokenType.SquareClose)
            {
                var expression = ParseExpression();
                if (expression != null)
                {
                    elements.Add(expression);
                }
                token = CurrentToken();
                if (token.type != TokenType.SquareClose)
                {
                    MustBe(token, TokenType.Comma, "Syntax error: expected comma in list");
                } else
                {
                    break;
                }
                token = NextToken();
            }

            NextToken();
            return new ListExpression(elements, pos);
        }
        public ListLengthExpression? ParseListLength()
        {
            if (CurrentToken().type != TokenType.ListLength)
            {
                return null;
            }
            var pos = GetPosition();

            MustBe(NextToken(), TokenType.Identifier, "Syntax error: expected an identifier of a list");

            var identifier = CurrentToken().value!.ToString();

            MustBe(NextToken(), TokenType.ListLength, "Syntax error: expected a '|' closing the list length");

            NextToken();
            return new ListLengthExpression(identifier!, pos);
        }

        private void MustBe(Token token, TokenType type, string message)
        {
            if (token.type != type) {
                ReportError(token.pos, message);
            }
        }

        private void MustBeIn(Token token, Array types, string message)
        {
            if (Array.IndexOf(types, token.type) == -1)
            {
                ReportError(token.pos, message);
            }
        }

        private Token CurrentToken()
        {
            var nextToken = _lexer.GetCurrentToken();
            if (nextToken != null && nextToken.type == TokenType.Comment)
            {
                CurrentToken();
            }
            return nextToken!;
        }
        private Token NextToken()
        {
            var nextToken = _lexer.NextToken();
            if (nextToken != null && nextToken.type == TokenType.Comment)
            {
                NextToken();
            }
            return nextToken!;
        }

        private Position GetPosition()
        {
            return _lexer.GetCurrentToken().pos;
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
