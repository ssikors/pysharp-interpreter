using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Classes.Expressions;
using PySharpCompiler.Classes.InterpreterClasses;
using PySharpCompiler.Classes;
using PySharpCompiler.Components;

namespace PySharpCompiler.Tests.InterpreterExpressionOperationsTests
{
    public class InterpreterExpressionOperationsTests
    {

        // CONVERSIONS

        // To int
        [Fact]
        public void TestBoolToIntTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(true, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToInt(DOMTree);

            Assert.Equivalent(new IntValue(1, pos), result);
        }

        [Fact]
        public void TestBoolToIntFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(false, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToInt(DOMTree);

            Assert.Equivalent(new IntValue(0, pos), result);
        }

        [Fact]
        public void TestFloatToInt()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(43.6, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToInt(DOMTree);

            Assert.Equivalent(new IntValue(43, pos), result);
        }

        [Fact]
        public void TestStringToInt()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("134", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToInt(DOMTree);

            Assert.Equivalent(new IntValue(134, pos), result);
        }

        [Fact]
        public void TestStringToIntOverflow()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("133253278526925378952378952379852738752890705823902573958323254", pos);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(() => { operationHandler.ConvertToInt(DOMTree); });
        }

        // To float
        [Fact]
        public void TestBoolToFloatTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(true, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToFloat(DOMTree);

            Assert.Equivalent(new FloatValue(1.0, pos), result);
        }

        [Fact]
        public void TestBoolToFloatFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(false, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToFloat(DOMTree);

            Assert.Equivalent(new FloatValue(0.0, pos), result);
        }

        [Fact]
        public void TestIntToFloat()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(21, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToFloat(DOMTree);

            Assert.Equivalent(new FloatValue(21.0, pos), result);
        }

        [Fact]
        public void TestStringToFloat()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("21.34", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToFloat(DOMTree);

            Assert.Equivalent(new FloatValue(21.34, pos), result);
        }

        [Fact]
        public void TestStringToFloatOverflow()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("2411235265434574237542365454324135223552352424214214121241.34143624152324", pos);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(() => { operationHandler.ConvertToFloat(DOMTree); });
        }

        // To string
        [Fact]
        public void TestBoolToStringTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(true, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToString(DOMTree);

            Assert.Equivalent(new StringValue("true", pos), result);
        }

        [Fact]
        public void TestBoolToStringFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(false, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToString(DOMTree);

            Assert.Equivalent(new StringValue("false", pos), result);
        }

        [Fact]
        public void TestIntToString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(231, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToString(DOMTree);

            Assert.Equivalent(new StringValue("231", pos), result);
        }

        [Fact]
        public void TestFloatToString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(231.432, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToString(DOMTree);

            Assert.Equivalent(new StringValue("231.432", pos), result);
        }

        [Fact]
        public void TestStringToString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("efsweffwe", pos);
            var operationHandler = new ExpressionOperationHandler();
        }

        // To bool
        [Fact]
        public void TestIntToBoolTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(1, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToBool(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestIntToBoolFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(0, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToBool(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestFloatToBoolTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(24121.1, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToBool(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestFloatToBoolFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(0, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToBool(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestStringToBoolTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("aaaa", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToBool(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestStringToBoolFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.ConvertToBool(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestBoolToBoolTrue()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(true, pos);
            var operationHandler = new ExpressionOperationHandler();
        }

        [Fact]
        public void TestBoolToBoolFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(false, pos);
            var operationHandler = new ExpressionOperationHandler();
        }

        // To function?
        // To List?



        // UNARY OPERATIONS


        // Negate

        [Fact]
        public void TestNegateBool()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(true, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestNegateBoolFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(false, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestNegateInt()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(1, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestNegateIntFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(0, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestNegateFloatFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(0.0, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestNegateFloat()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(0.214, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestNegateString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("saddsa", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestNegateStringFalse()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryNegate(DOMTree);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        // Unary minus

        [Fact]
        public void TestUMinusInt()
        {
            var pos = new Position(1, 2);
            var DOMTree = new IntValue(123, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryMinus(DOMTree);

            Assert.Equivalent(new IntValue(-123, pos), result);
        }

        [Fact]
        public void TestUMinusFloat()
        {
            var pos = new Position(1, 2);
            var DOMTree = new FloatValue(123.33, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryMinus(DOMTree);

            Assert.Equivalent(new FloatValue(-123.33, pos), result);
        }

        [Fact]
        public void TestUMinusIntString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("123", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryMinus(DOMTree);

            Assert.Equivalent(new IntValue(-123, pos), result);
        }

        [Fact]
        public void TestUMinusFloatString()
        {
            var pos = new Position(1, 2);
            var DOMTree = new StringValue("333.33", pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryMinus(DOMTree);

            Assert.Equivalent(new FloatValue(-333.33, pos), result);
        }

        [Fact]
        public void TestUMinusBool()
        {
            var pos = new Position(1, 2);
            var DOMTree = new BoolValue(true, pos);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.UnaryMinus(DOMTree);

            Assert.Equivalent(new IntValue(-1, pos), result);

            DOMTree = new BoolValue(false, pos);

            result = operationHandler.UnaryMinus(DOMTree);

            Assert.Equivalent(new IntValue(0, pos), result);
        }



        // MULTIPLICATIVE EXPRESSIONS


        // Multiply
        [Fact]
        public void TestMultiplyInts()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(2, pos);
            var right = new IntValue(9, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new IntValue(18, pos), result);
        }

        [Fact]
        public void TestMultiplyIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(2, pos);
            var right = new FloatValue(2.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new IntValue(5, pos), result);
        }

        [Fact]
        public void TestMultiplyFloats()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(2.5, pos);
            var right = new FloatValue(9.4, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new FloatValue(2.5 * 9.4, pos), result);
        }

        [Fact]
        public void TestMultiplyFloatsOverflow()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(24124124.5, pos);
            var right = new FloatValue(9124144.4, pos2);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(() => { operationHandler.Multiply(left, right); });
        }

        [Fact]
        public void TestMultiplyFloatInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.353, pos);
            var right = new IntValue(2, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new FloatValue(3.353 * 2, pos), result);
        }

        [Fact]
        public void TestMultiplyFloatStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.353, pos);
            var right = new StringValue("2", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new FloatValue(3.353 * 2, pos), result);
        }

        [Fact]
        public void TestMultiplyFloatStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.353, pos);
            var right = new StringValue("2.5", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new FloatValue(3.353 * 2.5, pos), result);
        }

        [Fact]
        public void TestMultiplyStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new StringValue("abc", pos);
            var right = new IntValue(2, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new StringValue("abcabc", pos), result);
        }

        [Fact]
        public void TestMultiplyIntStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(2, pos);
            var right = new StringValue("2", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new IntValue(4, pos), result);
        }

        [Fact]
        public void TestMultiplyIntStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(2, pos);
            var right = new StringValue("2.5", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new IntValue(5, pos), result);
        }

        [Fact]
        public void TestMultiplyIntBool()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(2, pos);
            var right = new BoolValue(true, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new IntValue(2, pos), result);
        }

        [Fact]
        public void TestMultiplyIntsOverflow()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(100000001, pos);
            var right = new IntValue(10, pos2);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(() => { operationHandler.Multiply(left, right); });
        }

        [Fact]
        public void TestMultiplyIntBoolFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(2, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new IntValue(0, pos), result);
        }

        [Fact]
        public void TestMultiplyFloatBool()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(2.5, pos);
            var right = new BoolValue(true, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Multiply(left, right);

            Assert.Equivalent(new FloatValue(2.5, pos), result);
        }

        


        // Divide
        [Fact]
        public void TestDivideInts()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(6, pos);
            var right = new IntValue(3, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new IntValue(2, pos), result);
        }

        [Fact]
        public void TestDivideIntFLoatOverflow()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(1000000000, pos);
            var right = new FloatValue(0.01, pos2);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(()=> { operationHandler.Divide(left, right); });
        }

        [Fact]
        public void TestDivideIntsUneven()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(3, pos);
            var right = new IntValue(2, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new IntValue(1, pos), result);
        }

        [Fact]
        public void TestDivideIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(5, pos);
            var right = new FloatValue(2.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new IntValue(2, pos), result);
        }

        [Fact]
        public void TestDivideIntStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(5, pos);
            var right = new StringValue("5", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new IntValue(1, pos), result);
        }

        [Fact]
        public void TestDivideIntStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(5, pos);
            var right = new StringValue("2.5", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new IntValue(2, pos), result);
        }

        [Fact]
        public void TestDivideFloats()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(2.5, pos);
            var right = new FloatValue(9.4, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new FloatValue(2.5 / 9.4, pos), result);
        }

        [Fact]
        public void TestDivideFloatsOverflow()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(1000000000, pos);
            var right = new FloatValue(0.01, pos2);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(() => { operationHandler.Divide(left, right); });
        }

        [Fact]
        public void TestDivideFloatInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.353, pos);
            var right = new IntValue(2, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new FloatValue(3.353 / 2, pos), result);
        }

        [Fact]
        public void TestDivideFloatStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.353, pos);
            var right = new StringValue("2", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new FloatValue(3.353 / 2, pos), result);
        }

        [Fact]
        public void TestDivideFloatStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.353, pos);
            var right = new StringValue("412.412", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Divide(left, right);

            Assert.Equivalent(new FloatValue(3.353 / 412.412, pos), result);
        }

        [Fact]
        public void TestDividePreventZeroDivision()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(6, pos);
            var right = new IntValue(0, pos2);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<DivideByZeroException>(() => { operationHandler.Divide(left, right); });
        }


        // Pipe


        // Bind front


        // ADDITIVE EXPRESSIONS


        // Add
        [Fact]
        public void TestAddInts()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(24, pos);
            var right = new IntValue(6, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new IntValue(30, pos), result);
        }

        [Fact]
        public void TestAddIntsOverflow()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(1000000000, pos);
            var right = new IntValue(1000000000, pos2);
            var operationHandler = new ExpressionOperationHandler();

            Assert.Throws<Exception>(() => { operationHandler.Add(left, right); });
        }

        [Fact]
        public void TestAddIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(24, pos);
            var right = new FloatValue(6.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new IntValue(30, pos), result);
        }

        [Fact]
        public void TestAddFloats()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.33, pos);
            var right = new FloatValue(6.53, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new FloatValue(3.33 + 6.53, pos), result);
        }

        [Fact]
        public void TestAddFloatInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.33, pos);
            var right = new IntValue(6, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new FloatValue(3.33 + 6, pos), result);
        }

        [Fact]
        public void TestAddStrings()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new StringValue("abc", pos);
            var right = new StringValue("def", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new StringValue("abcdef", pos), result);
        }

        [Fact]
        public void TestAddIntStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(24, pos);
            var right = new StringValue("6", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new IntValue(30, pos), result);
        }

        [Fact]
        public void TestAddIntStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(24, pos);
            var right = new StringValue("7.2", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new IntValue(31, pos), result);
        }

        [Fact]
        public void TestAddFloatStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.33, pos);
            var right = new StringValue("6.53", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new FloatValue(3.33 + 6.53, pos), result);
        }

        [Fact]
        public void TestAddFloatStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.33, pos);
            var right = new StringValue("6", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Add(left, right);

            Assert.Equivalent(new FloatValue(3.33 + 6, pos), result);
        }

        // Substract

        [Fact]
        public void TestSubstractInts()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(5, pos);
            var right = new IntValue(3, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Substract(left, right);

            Assert.Equivalent(new IntValue(2, pos), result);
        }

        [Fact]
        public void TestSubstractIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(24, pos);
            var right = new FloatValue(6.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Substract(left, right);

            Assert.Equivalent(new IntValue(17, pos), result);
        }

        [Fact]
        public void TestSubstractFloats()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.33, pos);
            var right = new FloatValue(6.53, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Substract(left, right);

            Assert.Equivalent(new FloatValue(3.33 - 6.53, pos), result);
        }

        [Fact]
        public void TestSubstractFloatInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(3.33, pos);
            var right = new IntValue(6, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Substract(left, right);

            Assert.Equivalent(new FloatValue(3.33 - 6, pos), result);
        }


        // Relation
        [Fact]
        public void TestMoreThanInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(65, pos);
            var right = new IntValue(23, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreThan(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestMoreThanIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(42, pos);
            var right = new FloatValue(41.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreThan(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestMoreThanIntString()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(42, pos);
            var right = new StringValue("21", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreThan(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestMoreThanIntStringFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(42, pos);
            var right = new StringValue("212.42", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreThan(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestMoreOrEqualInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(23, pos);
            var right = new IntValue(23, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreOrEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestMoreOrEqualIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(1, pos);
            var right = new FloatValue(41.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreOrEqual(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestMoreThanStrings()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new StringValue("short", pos);
            var right = new StringValue("long string", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreThan(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestMoreThanStringInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new StringValue("short", pos);
            var right = new IntValue(123, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.MoreThan(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestLessThanInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(23, pos);
            var right = new IntValue(25, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.LessThan(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestLessThanIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(1, pos);
            var right = new FloatValue(41.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.LessThan(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestLessOrEqualInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(23, pos);
            var right = new IntValue(25, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.LessOrEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestLessOrEqualIntFloat()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(142, pos);
            var right = new FloatValue(41.5, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.LessOrEqual(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestIsEqualInts()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(41, pos);
            var right = new IntValue(41, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestIsEqualIntsFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(41, pos);
            var right = new IntValue(42, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestIsEqualStrings()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new StringValue("abc", pos);
            var right = new StringValue("abc", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestIsEqualStringsFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new StringValue("41", pos);
            var right = new StringValue("aa", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestIsEqualFloats()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(421.42, pos);
            var right = new FloatValue(421.4, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestIsEqualBools()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new BoolValue(false, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestIsEqualIntString()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(11, pos);
            var right = new StringValue("11", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestIsEqualIntStringFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(11, pos);
            var right = new StringValue("12", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestIsEqualFloatString()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new FloatValue(1.421, pos);
            var right = new StringValue("1.421", pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestIsEqualBoolInt()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new BoolValue(false, pos);
            var right = new IntValue(0, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.IsEqual(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        // Conjunction
        [Fact]
        public void TestConjunctionBool()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new BoolValue(true, pos);
            var right = new BoolValue(true, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Conjunction(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestConjunctionBoolFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new BoolValue(false, pos);
            var right = new BoolValue(true, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Conjunction(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestConjunctionIntConversion()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(312, pos);
            var right = new BoolValue(true, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Conjunction(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestConjunctionIntConversionFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(0, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Conjunction(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        // Alternative
        [Fact]
        public void TestAlternativeBool()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new BoolValue(true, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Alternative(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestAlternativeBoolFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new BoolValue(false, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Alternative(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }

        [Fact]
        public void TestAlternativeIntConversion()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(312, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Alternative(left, right);

            Assert.Equivalent(new BoolValue(true, pos), result);
        }

        [Fact]
        public void TestAlternativeIntConversionFalse()
        {
            var pos = new Position(1, 2);
            var pos2 = new Position(1, 4);
            var left = new IntValue(0, pos);
            var right = new BoolValue(false, pos2);
            var operationHandler = new ExpressionOperationHandler();

            var result = operationHandler.Alternative(left, right);

            Assert.Equivalent(new BoolValue(false, pos), result);
        }
    }
}
