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
            (Parser.Char(' ').IgnoreResult().
            Or(Parser.Char('\t').IgnoreResult()).
            Or(Parser.EndOfLine.IgnoreResult()).
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

        public static readonly Parser<char, double> Float =
            Parser.Map((sign, integerPart, decimalSeparator, fractionalPart) => double.Parse(
                    sign.GetValueOrDefault(' ') +
                    integerPart +
                    decimalSeparator +
                    fractionalPart.GetValueOrDefault(string.Empty)),
                Parser.Char('-').Or(Parser.Char('+')).Optional(),
                Parser.Digit.ManyString(),
                Parser.Char('.'),
                Parser.Digit.ManyString().Optional());

        public static readonly Parser<char, Identifier> Identifier = Parser.Map(
                    (first, rest) => new Identifier(first + rest),
                    Parser<char>.Token(c => char.IsLetter(c) || c == '_'),
                    Parser<char>.Token(c => char.IsLetterOrDigit(c) || c == '_').ManyString());

        public static readonly Parser<char, object> Value =
            Parser.Try(Float).Cast<object>()
            .Or(Integer.Cast<object>())
            .Or(Boolean.Cast<object>())
            .Or(QuotedString.Cast<object>());

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