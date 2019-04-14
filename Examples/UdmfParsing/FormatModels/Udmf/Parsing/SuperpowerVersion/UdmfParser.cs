// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion.AbstractSyntaxTree;
using Superpower;
using Superpower.Parsers;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion
{
    public sealed class UdmfParser
    {
        private static readonly TokenListParser<UdmfToken, Value> Value = 
            Token.EqualTo(UdmfToken.Number)
                .Or(Token.EqualTo(UdmfToken.QuotedString))
                .Or(Token.EqualTo(UdmfToken.True))
                .Or(Token.EqualTo(UdmfToken.False))
            .Select(value => new Value(value.Span,value.Kind));

        public static readonly TokenListParser<UdmfToken, Assignment> Assignment =
            Token.EqualTo(UdmfToken.Identifier)
                .Then(name => Token.EqualTo(UdmfToken.Equals)
                    .IgnoreThen(Value.Then(value =>
                        Token.EqualTo(UdmfToken.Semicolon).
                            Select(_ => new Assignment(new Common.Identifier(name.ToStringValue()), value)))));

        public static readonly TokenListParser<UdmfToken, Block> Block =
            Token.EqualTo(UdmfToken.Identifier)
                .Then(name => Token.EqualTo(UdmfToken.OpenBracket)
                    .IgnoreThen(Assignment.Many()
                        .Then(assignments => Token.EqualTo(UdmfToken.CloseBracket)
                            .Select(_ => new Block(new Common.Identifier(name.ToStringValue()), assignments)))));

        private static readonly TokenListParser<UdmfToken, IGlobalExpression> AssignmentGlobalExpression =
            from assignment in Assignment
            select (IGlobalExpression) assignment;

        private static readonly TokenListParser<UdmfToken, IGlobalExpression> BlockGlobalExpression =
            from block in Block
            select (IGlobalExpression) block;
            
        public static readonly TokenListParser<UdmfToken, IGlobalExpression[]> GlobalExpressions =
            BlockGlobalExpression.Try().Or(AssignmentGlobalExpression).Many();
    }
}