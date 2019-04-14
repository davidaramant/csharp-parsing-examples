// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion
{
    public static class UdmfTokenizerRules
    {
        public static readonly Tokenizer<UdmfToken> Tokenizer = new TokenizerBuilder<UdmfToken>()
            .Ignore(Comment.CPlusPlusStyle)
            .Ignore(Comment.CStyle)
            .Ignore(Span.WhiteSpace)
            .Match(Character.EqualTo('='), UdmfToken.Equals)
            .Match(Character.EqualTo(';'), UdmfToken.Semicolon)
            .Match(Character.EqualTo('{'), UdmfToken.OpenBracket)
            .Match(Character.EqualTo('}'), UdmfToken.CloseBracket)
            .Match(Span.EqualTo("0x").Then(_ => Numerics.HexDigits), UdmfToken.Number)
            .Match(Numerics.Decimal, UdmfToken.Number)
            .Match(QuotedString.CStyle, UdmfToken.QuotedString)
            .Match(Span.EqualToIgnoreCase("false"), UdmfToken.False)
            .Match(Span.EqualToIgnoreCase("true"), UdmfToken.True)
            .Match(Span.Regex("[A-Za-z_][A-Za-z_0-9]*"), UdmfToken.Identifier)
            .Build();
    }
}