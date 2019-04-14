// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion;

namespace UdmfParsingTests.SuperpowerVersion
{
    [TestFixture]
    public sealed class UdmfTokenizerRulesTests
    {
        [TestCase("=", UdmfToken.Equals)]
        [TestCase(";", UdmfToken.Semicolon)]
        [TestCase("{", UdmfToken.OpenBracket)]
        [TestCase("}", UdmfToken.CloseBracket)]
        [TestCase("true", UdmfToken.True)]
        [TestCase("false", UdmfToken.False)]
        [TestCase("name", UdmfToken.Identifier)]
        [TestCase("name123", UdmfToken.Identifier)]
        [TestCase("\"string\"", UdmfToken.QuotedString)]
        [TestCase("123", UdmfToken.Number)]
        [TestCase("0x12af", UdmfToken.Number)]
        [TestCase("123.4", UdmfToken.Number)]
        [TestCase("-123.4", UdmfToken.Number)]
        public void ShouldParseTokenType(string input, UdmfToken expectedToken)
        {
            var result = UdmfTokenizerRules.Tokenizer.Tokenize(input);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Kind, Is.EqualTo(expectedToken));
            Assert.That(result.First().Span.ToStringValue(), Is.EqualTo(input));
        }

        [Test]
        public void ShouldIgnoreComments()
        {
            var udmf = @"
/* Start off with a
  comment block */
field = 4;
blockName // Some comment 
{
}";
            var result = UdmfTokenizerRules.Tokenizer.Tokenize(udmf);

            Assert.That(result.Select(r=>r.Kind).ToArray(), Is.EqualTo(new[]
            {
                UdmfToken.Identifier,
                UdmfToken.Equals,
                UdmfToken.Number,
                UdmfToken.Semicolon,
                UdmfToken.Identifier,
                UdmfToken.OpenBracket,
                UdmfToken.CloseBracket,
            }));
        }
    }
}