// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Pidgin;
using Pidgin.Comment;
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.PidginVersion.AbstractSyntaxTree;

namespace UdmfParsing.Udmf.Parsing.PidginVersion
{
    public static class UdmfParser
    {
        public static readonly Parser<char, Unit> Separator =
            (Parser.Whitespace.SkipAtLeastOnce().
            Or(Parser.Try(CommentParser.SkipBlockComment(Parser.String("/*"), Parser.String("*/")))).
            Or(CommentParser.SkipLineComment(Parser.String("//")))).SkipMany();

        public static readonly Parser<char, char> LBrace = Parser.Char('{');
        public static readonly Parser<char, char> RBrace = Parser.Char('}');
        public static readonly Parser<char, char> EqualSign = Parser.Char('=');
        public static readonly Parser<char, char> Quote = Parser.Char('"');
        public static readonly Parser<char, char> Semicolon = Parser.Char(';');

        public static readonly Parser<char, string> QuotedString =
            Parser<char>.Token(c => c != '"')
                .ManyString()
                .Between(Quote);

        public static readonly Parser<char, bool> Boolean =
            Parser.String("true").Or(Parser.String("false")).Select(s => s == "true");

        public static readonly Parser<char, int> HexInteger =
            Parser.String("0x").IgnoreResult().Then(Parser.HexNum);

        public static readonly Parser<char, int> Integer = Parser.Try(HexInteger).Or(Parser.DecimalNum);

        private static readonly Parser<char,string> FractionalComponent = 
            Parser.Char('.').Then(Parser<char>.Token(char.IsDigit).ManyString());

        public static readonly Parser<char, object> Number =
            Parser.Map((integerPart, fractionalPart) =>
                fractionalPart.HasValue ? (object)(integerPart + System.Math.Sign(integerPart) * double.Parse("0." + fractionalPart.Value)) : (object)integerPart,
                Integer,
                FractionalComponent.Optional());

        public static readonly Parser<char, object> Value =
            Number
            .Or(Boolean.Cast<object>())
            .Or(QuotedString.Cast<object>());

        public static readonly Parser<char, Identifier> Identifier = Parser.Map(
            (first, rest) => new Identifier(first + rest),
            Parser<char>.Token(c => char.IsLetter(c) || c == '_'),
            Parser<char>.Token(c => char.IsLetterOrDigit(c) || c == '_').ManyString());

        public static readonly Parser<char, Assignment> Assignment =
            Parser.Map((id, equalsSign, value, semicolon) => new Assignment(id, value),
                Identifier.Before(Separator),
                EqualSign.Before(Separator),
                Value.Before(Separator),
                Semicolon.Before(Separator));

        public static readonly Parser<char, Block> Block =
            Parser.Map((id, open, assignments, close) =>
                    new Block(id, assignments.ToImmutableArray()),
                Identifier.Before(Separator),
                LBrace.Before(Separator),
                Assignment.Many(),
                RBrace.Before(Separator));

        public static readonly Parser<char, IEnumerable<IGlobalExpression>> TranslationUnit =
            Separator.Then(Parser.Try(Block.Cast<IGlobalExpression>()).Or(Assignment.Cast<IGlobalExpression>()).Many());
    }
}