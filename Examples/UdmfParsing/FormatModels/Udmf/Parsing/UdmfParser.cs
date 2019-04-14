// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Pidgin;
using Pidgin.Comment;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing.AbstractSyntaxTree;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing
{
    public static class UdmfParser
    {
        public static readonly Parser<char, Unit> Separator =
            ((Char(' ').IgnoreResult()).
            Or(Char('\t').IgnoreResult()).
            Or(EndOfLine.IgnoreResult()).
            Or(Try(CommentParser.SkipBlockComment(String("/*"), String("*/")))).
            Or(CommentParser.SkipLineComment(String("//")))).SkipMany();

        public static readonly Parser<char, char> LBrace = Char('{');
        public static readonly Parser<char, char> RBrace = Char('}');
        public static readonly Parser<char, char> EqualSign = Char('=');
        public static readonly Parser<char, char> Quote = Char('"');
        public static readonly Parser<char, char> Semicolon = Char(';');

        public static readonly Parser<char, string> QuotedString =
            Token(c => c != '"')
                .ManyString()
                .Between(Quote);

        public static readonly Parser<char, bool> Boolean =
            String("true").Or(String("false")).Select(s => s == "true");

        public static readonly Parser<char, int> HexInteger =
            String("0x").IgnoreResult().Then(HexNum);

        public static readonly Parser<char, int> Integer = Try(HexInteger).Or(DecimalNum);

        public static readonly Parser<char, double> Float =
            Map((sign, integerPart, decimalSeparator, fractionalPart) => double.Parse(
                    sign.GetValueOrDefault(' ') +
                    integerPart +
                    decimalSeparator +
                    fractionalPart.GetValueOrDefault(string.Empty)),
                Char('-').Or(Char('+')).Optional(),
                Digit.ManyString(),
                Char('.'),
                Digit.ManyString().Optional());

        public static readonly Parser<char, Identifier> Identifier = Map(
                    (first, rest) => new Identifier(first + rest),
                    Token(c => char.IsLetter(c) || c == '_'),
                    Token(c => char.IsLetterOrDigit(c) || c == '_').ManyString());

        public static readonly Parser<char, object> Value =
            Boolean.Cast<object>()
            .Or(Try(Float).Cast<object>())
            .Or(Integer.Cast<object>())
            .Or(QuotedString.Cast<object>());

        public static readonly Parser<char, Assignment> Assignment =
            Map((id, equalsSign, value, semicolon) => new Assignment(id, value),
                Identifier.Before(Separator),
                EqualSign.Before(Separator),
                Value.Before(Separator),
                Semicolon.Before(Separator));

        public static readonly Parser<char, Block> Block =
            Map((id, open, assignments, close) => new Block(id, assignments.ToImmutableArray()),
                Identifier.Before(Separator),
                LBrace.Before(Separator),
                Assignment.Many(),
                RBrace.Before(Separator));

        public static readonly Parser<char, IEnumerable<IGlobalExpression>> TranslationUnit =
            Separator.Then(Try(Block.Cast<IGlobalExpression>()).Or(Assignment.Cast<IGlobalExpression>()).Many());
    }
}