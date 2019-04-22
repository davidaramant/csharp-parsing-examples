// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Pidgin;
using UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin.AbstractSyntaxTree;

namespace UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin
{
    public static class UdmfParser
    {
        public static readonly Parser<Token, Token> Integer = Parser<Token>.Token(t => t is IntegerToken);
        public static readonly Parser<Token, Token> Float = Parser<Token>.Token(t => t is FloatToken);
        public static readonly Parser<Token, Token> Boolean = Parser<Token>.Token(t => t is BooleanToken);
        public static readonly Parser<Token, Token> String = Parser<Token>.Token(t => t is StringToken);

        public static readonly Parser<Token, Token> Value = Integer.Or(Float).Or(Boolean).Or(String);

        public static readonly Parser<Token, IdentifierToken> Identifier = Parser<Token>.Token(t => t is IdentifierToken).Cast<IdentifierToken>();

        public static readonly Parser<Token, Unit> LBrace = Parser<Token>.Token(t => t is OpenBraceToken).IgnoreResult();
        public static readonly Parser<Token, Unit> RBrace = Parser<Token>.Token(t => t is CloseBraceToken).IgnoreResult();
        public static readonly Parser<Token, Unit> EqualSign = Parser<Token>.Token(t => t is EqualsToken).IgnoreResult();
        public static readonly Parser<Token, Unit> Semicolon = Parser<Token>.Token(t => t is SemicolonToken).IgnoreResult();

        public static readonly Parser<Token, Assignment> Assignment =
            Parser.Map((id, equalsSign, value, semicolon) => new Assignment(id.Id, value),
                Identifier,
                EqualSign,
                Value,
                Semicolon);

        public static readonly Parser<Token, Block> Block =
            Parser.Map((id, open, assignments, close) => new Block(id.Id, assignments.ToImmutableArray()),
                Identifier,
                LBrace,
                Assignment.Many(),
                RBrace);

        public static readonly Parser<Token, IEnumerable<IGlobalExpression>> TranslationUnit =
            Parser.Try(Block.Cast<IGlobalExpression>()).Or(Assignment.Cast<IGlobalExpression>()).Many();
    }
}