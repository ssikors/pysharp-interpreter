using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PySharpCompiler.Components;

namespace PySharpCompiler.Classes.InterpreterClasses
{
    public class Scope
    {
        public string Name;
        public bool IsFunCall;
        private Dictionary<string, SymbolContainer> _table = [];
        private ExpressionOperationHandler _operationHandler = new ExpressionOperationHandler();

        public Scope(string name)
        {
            Name = name;
        }

        public Scope(string name, bool isFunCall)
        {
            Name = name;
            IsFunCall = isFunCall;
        }

        public bool AddSymbol(string identifier, DOMObject? expr, bool isMutable, Position pos, IDOMType type)
        {
            if (_table.ContainsKey(identifier))
            {
                ReportError($"Cannot redeclare '{identifier}' declared at {_table[identifier].PositionDeclared}");
                return false;
            }
            else
            {
                if (expr != null)
                {
                    expr = _operationHandler.TypeConversion(type, expr);
                }
                
                _table.Add(identifier, new SymbolContainer(expr, pos, isMutable, type));
                return true;
            }
        }

        public bool IsDeclared(string identifier)
        {
            if (_table.ContainsKey(identifier))
            {
                return true;
            }
            return false;
        }

        public bool AssignValue(string identifier, DOMObject expr)
        {
            if (_table.ContainsKey(identifier))
            {
                if (_table[identifier].IsMutable)
                {
                    _table[identifier].Value = _operationHandler.TypeConversion(_table[identifier].Type, expr);
                    return true;
                }
                else
                {
                    ReportError($"Cannot assign to immutable '{identifier}' declared at {_table[identifier].PositionDeclared}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public DOMObject? GetSymbol(string identifier)
        {
            if (_table.ContainsKey(identifier))
            {
                return _table[identifier].Value;
            }
            else
            {
                return null;
            }
        }

        public void ReportError(string message)
        {
            throw new Exception($"Scope error: {message}");
        }
    }
}
