using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.Expressions.Operators;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class ExpressionOperationHandler
    {
        public ExpressionOperationHandler() { }

        // Alternative
        public BoolValue? Alternative(dynamic left, dynamic right)
        {
            return new BoolValue((ConvertToBool(left).Value || ConvertToBool(right).Value), left.Position);
        }

        // Conjunction
        public BoolValue? Conjunction(dynamic left, dynamic right)
        {
            return new BoolValue(( ConvertToBool(left).Value && ConvertToBool(right).Value ), left.Position);
        }

        // Relation

        public BoolValue? RelationExpression(OperatorType opr, dynamic left, dynamic right)
        {
            try
            {
                switch (opr)
                {
                    case OperatorType.Equal:
                        return IsEqual(left, right);
                    case OperatorType.NotEqual:
                        return IsNotEqual(left, right);
                    case OperatorType.MoreThan:
                        return MoreThan(left, right);
                    case OperatorType.MoreOrEqual:
                        return MoreOrEqual(left, right);
                    case OperatorType.LessOrEqual:
                        return LessOrEqual(left, right);
                    case OperatorType.Less:
                        return LessThan(left, right);
                }
            }
            catch (RuntimeBinderException ex)
            {
                ReportError($"Cannot use {opr} with {left} and {right}", left.Position);
            } 
            return null;
        }
        
        // Inequalities
        public BoolValue MoreThan(IntValue left, dynamic right)
        {
            try
            {
                return new BoolValue((left.Value > ConvertToInt(right).Value), left.Position);
            } catch
            {
                return new BoolValue((left.Value > ConvertToFloat(right).Value), left.Position);
            }
        }

        public BoolValue MoreThan(FloatValue left, dynamic right)
        {
            try
            {
                return new BoolValue((left.Value > ConvertToInt(right).Value), left.Position);
            }
            catch
            {
                return new BoolValue((left.Value > ConvertToFloat(right).Value), left.Position);
            }
        }

        public BoolValue MoreThan(StringValue left, dynamic right)
        {
            return new BoolValue((left.Value.Length > ConvertToString(right).Value.Length), left.Position);
        }

        public BoolValue LessThan(IntValue left, dynamic right)
        {
            try
            {
                return new BoolValue((left.Value < ConvertToInt(right).Value), left.Position);
            }
            catch
            {
                return new BoolValue((left.Value < ConvertToFloat(right).Value), left.Position);
            }
        }

        public BoolValue LessThan(FloatValue left, dynamic right)
        {
            try
            {
                return new BoolValue((left.Value < ConvertToInt(right).Value), left.Position);
            }
            catch
            {
                return new BoolValue((left.Value < ConvertToFloat(right).Value), left.Position);
            }
        }

        public BoolValue LessThan(StringValue left, dynamic right)
        {
            return new BoolValue((left.Value < ConvertToString(right).Value.Length), left.Position);
        }

        public BoolValue MoreOrEqual(dynamic left, dynamic right)
        {
            return new BoolValue(!LessThan(left, right).Value, left.Position);
        }

        public BoolValue LessOrEqual(dynamic left, dynamic right)
        {
            return new BoolValue(!MoreThan(left, right).Value, left.Position);
        }

        // Equal / Not equal
        public BoolValue? IsEqual(IntValue left, dynamic right)
        {
            return new BoolValue((left.Value == ConvertToInt(right).Value), left.Position);
        }

        public BoolValue? IsEqual(FloatValue left, dynamic right)
        {
            return new BoolValue((left.Value == ConvertToFloat(right).Value), left.Position);
        }

        public BoolValue? IsEqual(StringValue left, dynamic right)
        {
            return new BoolValue((left.Value == ConvertToString(right).Value), left.Position);
        }

        public BoolValue? IsEqual(BoolValue left, dynamic right)
        {
            return new BoolValue((left.Value == ConvertToBool(right).Value), left.Position);
        }

        // TODO functions, lists

        public BoolValue? IsNotEqual(dynamic left, dynamic right)
        {
            return new BoolValue(!IsEqual(left, right).Value, left.Position);
        }

        // Additive

        public dynamic? AdditiveOperation(OperatorType opr, dynamic left, dynamic right)
        {   try
            {
                if (opr == OperatorType.Plus)
                {
                    return Add(left, right);
                }
                else if (opr == OperatorType.Minus)
                {
                    return Substract(left, right);
                }
            } catch (RuntimeBinderException ex)
            {
                ReportError($"Cannot use {opr} with {left} and {right}", left.Position);
            }
            return null;
        }

        // Add
        public FloatValue Add(FloatValue left, FloatValue right)
        {
            return new FloatValue(left.Value + right.Value, left.Position);
        }

        public FloatValue Add(FloatValue left, IntValue right)
        {
            return new FloatValue(left.Value + right.Value, left.Position);
        }

        public FloatValue Add(FloatValue left, StringValue right)
        {
            try
            {
                var rint = ConvertToInt(right);
                return Add(left, rint!);
            }
            catch
            {
                var rfloat = ConvertToFloat(right);
                return Add(left, rfloat!);
            }
        }

        public FloatValue Add(FloatValue left, BoolValue right)
        {
            var rint = ConvertToFloat(right);
            return Add(left, rint!);
        }

        public IntValue Add(IntValue left, IntValue right)
        {
            return new IntValue(left.Value + right.Value, left.Position);
        }

        public IntValue Add(IntValue left, FloatValue right)
        {
            return new IntValue((int)Math.Floor((left.Value + right.Value)), left.Position);
        }

        public IntValue Add(IntValue left, StringValue right)
        {
            try
            {
                var rint = ConvertToInt(right);
                return Add(left, rint!);
            } catch
            {
                var rfloat = ConvertToFloat(right);
                return Add(left, rfloat!);
            } 
        }

        public IntValue Add(IntValue left, BoolValue right)
        {
            var rint = ConvertToInt(right);
            return Add(left, rint!);
        }

        public StringValue Add(StringValue left, StringValue right)
        {
            return new StringValue(left.Value + right.Value, left.Position);
        }

        public StringValue Add(StringValue left, IntValue right)
        {
            var rstring = ConvertToString(right);
            return new StringValue(left.Value + rstring!.Value, left.Position);
        }

        public StringValue Add(StringValue left, FloatValue right)
        {
            var rstring = ConvertToString(right);
            return new StringValue(left.Value + rstring!.Value, left.Position);
        }

        public StringValue Add(StringValue left, BoolValue right)
        {
            var rstring = ConvertToString(right);
            return new StringValue(left.Value + rstring!.Value, left.Position);
        }

        // Substract
        public FloatValue Substract(FloatValue left, FloatValue right)
        {
            return new FloatValue(left.Value - right.Value, left.Position);
        }

        public FloatValue Substract(FloatValue left, IntValue right)
        {
            return new FloatValue(left.Value - right.Value, left.Position);
        }

        public IntValue Substract(IntValue left, IntValue right)
        {
            return new IntValue(left.Value - right.Value, left.Position);
        }

        public IntValue Substract(IntValue left, FloatValue right)
        {
            return new IntValue((int)Math.Floor((left.Value - right.Value)), left.Position);
        }


        // Multiplicative

        public dynamic? MultiplicativeOperation(OperatorType opr, dynamic left, dynamic right)
        {
            try
            {
                if (opr == OperatorType.Multiply)
                {
                    return Multiply(left, right);
                }
                else if (opr == OperatorType.Divide)
                {
                    return Divide(left, right);
                }
            } catch (Exception ex)
            {
                if (ex is DivideByZeroException)
                {
                    throw new Exception(ex.Message);
                }
                ReportError($"Cannot use {opr} with {left} and {right}", left.Position);
            }
            return null;
        }

        public IntValue? Multiply(IntValue left, IntValue right)
        {
            return new IntValue(
                (left.Value * right.Value),
                left.Position
            );
        }

        public IntValue? Multiply(IntValue left, FloatValue right)
        {
            return new IntValue(
                (int)(left.Value * right.Value),
                left.Position
            );
        }

        public IntValue? Multiply(IntValue left, StringValue right)
        {
            try
            {
                return Multiply(left, ConvertToInt(right));
            }
            catch
            {
                return Multiply(left, ConvertToFloat(right));
            }
        }

        public IntValue? Multiply(IntValue left, BoolValue right)
        {
            if (right.Value == false)
            {
                left.Value = 0;

            }
            return left;
        }

        public FloatValue? Multiply(FloatValue left, FloatValue right)
        {
            return new FloatValue(left.Value * right.Value, left.Position);
        }

        public FloatValue? Multiply(FloatValue left, IntValue right)
        {
            return new FloatValue(left.Value * right.Value, left.Position);
        }

        public StringValue? Multiply(StringValue left, IntValue right)
        {
            return new StringValue(
                new StringBuilder().Insert(0, left.Value, right.Value).ToString(),
                left.Position
            );
        }

        public FloatValue? Multiply(FloatValue left, StringValue right)
        {
            try
            {
                return Multiply(left, ConvertToInt(right));
            }
            catch
            {
                return Multiply(left, ConvertToFloat(right));
            }
        }

        public FloatValue? Multiply(FloatValue left, BoolValue right)
        {
            if (right.Value == false)
            {
                left.Value = 0;
                
            } 
            return left;
        }


        // Divide
        public IntValue? Divide(IntValue left, IntValue right)
        {
            if (right.Value == 0)
            {
                throw new DivideByZeroException($"ExpressionHandler error at {right.Position}: Attempted divide by zero");
            }
            return new IntValue(
                (left.Value / right.Value),
                left.Position
            );
        }

        public IntValue? Divide(IntValue left, FloatValue right)
        {
            if (right.Value == 0)
            {
                throw new DivideByZeroException($"ExpressionHandler error at {right.Position}: Attempted divide by zero");
            }
            return new IntValue(
                (int)(left.Value / right.Value),
                left.Position
            );
        }

        public IntValue? Divide(IntValue left, StringValue right)
        {
            try
            {
                return Divide(left, ConvertToInt(right));
            } catch
            {
                return Divide(left, ConvertToFloat(right));
            }
        }

        public FloatValue? Divide(FloatValue left, FloatValue right)
        {
            return new FloatValue(left.Value / right.Value, left.Position);
        }

        public FloatValue? Divide(FloatValue left, IntValue right)
        {
            return new FloatValue(left.Value / right.Value, left.Position);
        }

        public FloatValue? Divide(FloatValue left, StringValue right)
        {
            try
            {
                return Divide(left, ConvertToInt(right));
            }
            catch
            {
                return Divide(left, ConvertToFloat(right));
            }
        }

        // Unary

        public dynamic? UnaryOperation(OperatorType opr, dynamic factor)
        {
            if (opr == OperatorType.Minus)
            {
                try
                {
                    return UnaryMinus(factor);
                } catch
                {
                    ReportError($"Cannot use unary minus with {factor}", factor.Position);
                    return null;
                }
                
            }
            else if (opr == OperatorType.Negate)
            {
                try
                {
                    return UnaryNegate(factor);
                }
                catch
                {
                    ReportError($"Cannot use unary minus with {factor}", factor.Position);
                    return null;
                }
            }

            return 0;
        }

        public FloatValue UnaryMinus(FloatValue factor)
        {
            factor.Value = -factor.Value;
            return factor;
        }

        public IntValue UnaryMinus(IntValue factor)
        {
            factor.Value = -factor.Value;
            return factor;
        }

        public dynamic? UnaryMinus(StringValue factor)
        {
            try
            {
                var converted = ConvertToInt(factor);
                converted!.Value = -converted.Value;
                return converted;
            } catch
            {
                var converted = ConvertToFloat(factor);
                converted!.Value = -converted.Value;
                return converted;
            }
        }

        public dynamic? UnaryMinus(BoolValue factor)
        {
            if (factor.Value == true)
            {
                return new IntValue(-1, factor.Position);
            } else
            {
                return new IntValue(0, factor.Position);
            }
        }



        public BoolValue? UnaryNegate(BoolValue factor)
        {
            factor.Value = !factor.Value;
            return factor;
        }

        public dynamic? UnaryNegate(dynamic factor)
        {
            factor = ConvertToBool(factor);

            factor.Value = !factor.Value;
            return factor;
        }


        // Conversions

        public dynamic? TypeConversion(IDOMType desired, dynamic value)
        {
            if (value is DeclaredFunction || value is PipeFunction || value is BuiltInFunction)
            {
                return value;
            }

            try
            {
                var desiredType = desired.GetType();
                if (desired is ListType)
                {
                    return ConvertToList(value, desiredType);
                }
                switch (desiredType)
                {
                    case DOMType.String:
                        return ConvertToString(value);
                    case DOMType.Bool:
                        return ConvertToBool(value);
                    case DOMType.Int:
                        return ConvertToInt(value);
                    case DOMType.Float:
                        return ConvertToFloat(value);
                    case DOMType.Function:
                        return ConvertToFunction(value);         
                }
            } catch
            {
                ReportError($"Failed conversion from {value} to {desired}", value.Position);
            }
            
            return null;
        }

        // To list
        public ListValue ConvertToList(ListValue list, dynamic type)
        {
            List<DOMObject> convertedExpressions = new List<DOMObject>([]);
            foreach (var expression in list.Expressions)
            {
                convertedExpressions.Add(TypeConversion(type, expression));
            }
            return new ListValue(convertedExpressions, list.Position, type);
        }


        // To function
        public DeclaredFunction? ConvertToFunction(DeclaredFunction function)
        {
            return function;
        }

        // To bool
        public BoolValue? ConvertToBool(BoolValue factor)
        {
            return factor;
        }

        public BoolValue? ConvertToBool(StringValue factor)
        {
            if (factor.Value == "")
            {
                return new BoolValue(false, factor.Position);
            }
            else
            {
                return new BoolValue(true, factor.Position);
            }
        }

        public BoolValue? ConvertToBool(FloatValue factor)
        {
            if (factor.Value == 0)
            {
                return new BoolValue(false, factor.Position);
            }
            else
            {
                return new BoolValue(true, factor.Position);
            }
        }

        public BoolValue? ConvertToBool(IntValue factor)
        {
            if (factor.Value == 0)
            {
                return new BoolValue(false, factor.Position);
            }
            else
            {
                return new BoolValue(true, factor.Position);
            }
        }

        // To int
        public IntValue? ConvertToInt(IntValue factor)
        {
            return factor;
        }

        public IntValue? ConvertToInt(StringValue factor)
        {
            string number = factor.Value;
            bool converted = int.TryParse(number, out int value);
            if (!converted)
            {
                ReportError($"Cannot convert string {factor.Value} to int", factor.Position);
                return null;
            }

            return new IntValue(value, factor.Position);
        }

        public IntValue? ConvertToInt(BoolValue factor)
        {
            if (factor.Value == true)
            {
                return new IntValue(1, factor.Position);
            }
            else
            {
                return new IntValue(0, factor.Position);
            }
        }

        public IntValue? ConvertToInt(FloatValue factor)
        {
            return new IntValue((int)Math.Floor(factor.Value), factor.Position);
        }


        // To float
        public FloatValue? ConvertToFloat(FloatValue factor)
        {
            return factor;
        }

        public FloatValue? ConvertToFloat(StringValue factor)
        {
            string number = factor.Value;
            bool converted = double.TryParse(number, CultureInfo.InvariantCulture, out double value);
            if (!converted)
            {
                ReportError($"Cannot convert string {factor.Value} to float", factor.Position);
                return null;
            }

            return new FloatValue(value, factor.Position);
        }

        public FloatValue? ConvertToFloat(BoolValue factor)
        {
            if (factor.Value == true)
            {
                return new FloatValue(1, factor.Position);
            }
            else
            {
                return new FloatValue(0, factor.Position);
            }
        }

        public FloatValue? ConvertToFloat(IntValue factor)
        {
            return new FloatValue(factor.Value, factor.Position);
        }

        // To string
        public StringValue? ConvertToString(IntValue factor)
        {
            return new StringValue(factor.Value.ToString(), factor.Position);
        }

        public StringValue? ConvertToString(StringValue factor)
        {
            return factor;
        }

        public StringValue? ConvertToString(BoolValue factor)
        {
            if (factor.Value == true)
            {
                return new StringValue("true", factor.Position);
            }
            else
            {
                return new StringValue("false", factor.Position);
            }
        }

        public StringValue? ConvertToString(FloatValue factor)
        {
            return new StringValue(factor.Value.ToString(CultureInfo.InvariantCulture), factor.Position);
        }


        // Utilities

        public void ReportError(string message, Position? position)
        {
            throw new Exception($"ExpressionHandler error at {position}: {message}");
        }

        public void ReportWarning(string message, Position? position)
        {
            Console.WriteLine($"ExpressionHandler warning at {position}: {message}");
        }
    }
}
