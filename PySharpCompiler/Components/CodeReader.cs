using PySharpCompiler.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Components
{
    public class CodeReader
    {
        private char _currentChar;
        private StreamReader _reader;
        private Position _position = new Position( 1, 0 );

        public CodeReader(StreamReader streamReader) {
            _reader = streamReader;
        }

        public char NextChar() {
            var nextByte = _reader.Read();

            if (nextByte == -1) {
                _currentChar = '\0';
                _position.Column += 1;
                return _currentChar;
            }

            char nextChar = (char)nextByte;
            
            if (nextChar == '\r' || nextChar == '\n') {
                HandleNewline(nextChar);
                return _currentChar;
            }

            // Moving a column
            _position.Column += 1;

            _currentChar = nextChar;
            return _currentChar;
        }
        
        public void HandleNewline(char nextChar)
        {
            var peekByte = _reader.Peek();
            if (peekByte == -1)
            {
                _currentChar = '\n';
                return;
            }

            var peekChar = (char)peekByte;

            if (nextChar == '\n')
            {
                // '\n\r'
                if (peekChar == '\r') {
                    // Setting current char as unified '\n'
                    _currentChar = '\n';

                    // Skipping the '\r'
                    _reader.Read();
                } else
                {
                    _currentChar = nextChar;
                }
            } else
            {
                if (peekChar == '\n')
                {
                    // '\r\n'
                    // Setting current char as unified '\n'
                    _currentChar = '\n';
                    
                    // Skipping the '\r'
                    _reader.Read(); 
                } else
                {
                    // If it's a lone '\r' we change it to '\n'
                    _currentChar = '\n';
                }
            }

            // Moving position a line
            _position.Line += 1;
            _position.Column = 0;
        }

        public char PeekChar()
        {
            var peekByte = _reader.Peek();
            var peekChar = (char)peekByte;

            if (peekChar == '\r') {
                return '\n';
            }
            return peekChar;
        }

        public char CurrentChar()
        {
            return _currentChar;
        }

        public Position GetPosition()
        {
            return _position;
        }
    }
}
