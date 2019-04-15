// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Piglet.Lexer;
using System.Linq;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.PigletVersion
{
    public abstract class BaseLexer : ILexer
    {
        public const int EndOfInputTokenNumber = -1;

        private readonly ILexerInstance<Token> _lexer;

        protected BaseLexer(ILexerInstance<Token> lexer)
        {
            _lexer = lexer;
        }

        public Token ReadToken()
        {
            var pair = _lexer.Next();
            if (pair.Item1 == EndOfInputTokenNumber)
            {
                return Token.EndOfFile;
            }
            return pair.Item2;
        }

        public Token MustReadTokenOfTypes(params TokenType[] expectedTypes)
        {
            var token = ReadToken();
            if (!expectedTypes.Contains(token.Type))
            {
                throw new ParsingException($"Line {_lexer.CurrentLineNumber}: Expected {string.Join(", ", expectedTypes)} but got {token.Type}");
            }
            return token;
        }
    }
}