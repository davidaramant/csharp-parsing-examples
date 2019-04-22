// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Text;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin
{
    public sealed class Lexer
    {
        private readonly TextReader _reader;
        private FilePosition _currentPosition = FilePosition.StartOfFile();
        private const char Null = '\0';
        private StringBuilder _tokenBuffer = new StringBuilder();

        public Lexer(TextReader reader) => _reader = reader;

        public IEnumerable<Token> Scan()
        {
            char next = PeekChar();
            while (next != Null)
            {
                switch (next)
                {
                    case '=':
                        yield return new EqualsToken(_currentPosition);
                        SkipChar();
                        break;
                    case ';':
                        yield return new SemicolonToken(_currentPosition);
                        SkipChar();
                        break;
                    case '{':
                        yield return new OpenBraceToken(_currentPosition);
                        SkipChar();
                        break;
                    case '}':
                        yield return new CloseBraceToken(_currentPosition);
                        SkipChar();
                        break;
                    case char digit when char.IsDigit(next):
                        yield return ParseNumber(digit);
                        break;
                    case '-':
                    case '+':
                        yield return ParseNumber(next);
                        break;
                    default:
                        SkipChar();
                        break;
                }

                next = PeekChar();
            }
            yield break;
        }

        private Token ParseNumber(char first)
        {
            var start = _currentPosition;
            ConsumeChar();

            if (first == '0' && PeekChar() == 'x')
            {
                _tokenBuffer.Clear();
                SkipChar();

                bool IsHexChar(char c) =>
                    char.IsDigit(c) ||
                    (c >= 'a' && c <= 'f') ||
                    (c >= 'A' && c <= 'F');

                if (!IsHexChar(PeekChar()))
                {
                    throw new ParsingException("Malformed hex number: " + _currentPosition);
                }

                while (IsHexChar(PeekChar()))
                {
                    ConsumeChar();
                }
                return new IntegerToken(start, BufferAsHexInteger());
            }

            while (char.IsDigit(PeekChar()))
            {
                ConsumeChar();
            }
            if (PeekChar() != '.')
            {
                return new IntegerToken(start, BufferAsInteger());
            }

            ConsumeChar();

            while (char.IsDigit(PeekChar()))
            {
                ConsumeChar();
            }

            return new FloatToken(start, BufferAsFloat());
        }

        private char PeekChar()
        {
            var next = _reader.Peek();
            return next > -1 ? (char)next : Null;
        }

        private void SkipChar()
        {
            _currentPosition = _currentPosition.NextChar();
            _reader.Read();
        }

        private void ConsumeChar()
        {
            _currentPosition = _currentPosition.NextChar();
            _tokenBuffer.Append((char)_reader.Read());
        }

        private int BufferAsHexInteger()
        {
            var value = int.Parse(_tokenBuffer.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
            _tokenBuffer.Clear();
            return value;
        }

        private int BufferAsInteger()
        {
            var value = int.Parse(_tokenBuffer.ToString());
            _tokenBuffer.Clear();
            return value;
        }

        private double BufferAsFloat()
        {
            var value = double.Parse(_tokenBuffer.ToString());
            _tokenBuffer.Clear();
            return value;
        }
    }
}