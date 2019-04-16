// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using Piglet.Lexer;
using UdmfParsing.Udmf.Parsing.PigletVersion;

namespace UdmfParsingTests.PigletVersion
{
    public abstract class BaseLexerTests
    {
        [TestCase("someProperty = 10;")]
        [TestCase("      someProperty=10;")]
        [TestCase("      someProperty  = 10 ;")]
        public void ShouldIgnoreWhitespace(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(10),
                Token.Semicolon);
        }

        [TestCase("someProperty = 0;", 0)]
        [TestCase("someProperty = 10;", 10)]
        [TestCase("someProperty = 0010;", 10)]
        [TestCase("someProperty = 0xa;", 10)]
        [TestCase("someProperty = +10;", 10)]
        [TestCase("someProperty = -10;", -10)]
        public void ShouldLexIntegers(string input, int value)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(value),
                Token.Semicolon);
        }

        [TestCase("someProperty = 10.5;", 10.5)]
        [TestCase("someProperty = +10.5;", 10.5)]
        [TestCase("someProperty = 1.05e1;", 10.5)]
        [TestCase("someProperty = 1e10;", 1e10)]
        public void ShouldLexDoubles(string input, double value)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Double(value),
                Token.Semicolon);
        }

        [TestCase("someProperty = true;", true)]
        [TestCase("someProperty = false;", false)]
        public void ShouldLexBooleans(string input, bool boolValue)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                boolValue ? Token.BooleanTrue : Token.BooleanFalse,
                Token.Semicolon);
        }

        [TestCase("someProperty = \"true\";", "true")]
        [TestCase("someProperty = \"0xFB010304\";", "0xFB010304")]
        [TestCase("someProperty = \"\";", "")]
        public void ShouldLexStrings(string input, string stringValue)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.String(stringValue),
                Token.Semicolon);
        }

        [Test]
        public void ShouldLexEmptyBlock()
        {
            VerifyLexing("block { }",
                Token.Identifier("block"),
                Token.OpenParen,
                Token.CloseParen);
        }

        [Test]
        public void ShouldLexBlockWithAssignments()
        {
            VerifyLexing("block { id1 = 1; id2 = false; }",
                Token.Identifier("block"),
                Token.OpenParen,
                Token.Identifier("id1"),
                Token.Equal,
                Token.Integer(1),
                Token.Semicolon,
                Token.Identifier("id2"),
                Token.Equal,
                Token.BooleanFalse,
                Token.Semicolon,
                Token.CloseParen);
        }

        [Test]
        public void ShouldLexBlockWithArrays()
        {
            VerifyLexing("block { {1,2},{3,4} }",
                Token.Identifier("block"),
                Token.OpenParen,

                Token.OpenParen,
                Token.Integer(1),
                Token.Comma,
                Token.Integer(2),
                Token.CloseParen,

                Token.Comma,

                Token.OpenParen,
                Token.Integer(3),
                Token.Comma,
                Token.Integer(4),
                Token.CloseParen,

                Token.CloseParen);
        }

        [TestCase("// Comment\r\nsomeProperty = 10;")]
        [TestCase("// Comment\nsomeProperty = 10;")]
        [TestCase("someProperty = 10; // Comment")]
        public void ShouldIgnoreComments(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(10),
                Token.Semicolon);
        }

        protected void VerifyLexing(string input, params Token[] expectedTokens)
        {
            var actualTokens = GetDefinition().Tokenize(input).Select(t => t.Item2).ToArray();

            Assert.That(actualTokens, Is.EqualTo(expectedTokens), $"Did not correct tokenize {input}");
        }

        protected abstract ILexer<Token> GetDefinition();
    }
}