// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin;
using NUnit.Framework;
using System.Linq;
using System.IO;

namespace UdmfParsingTests.CustomLexerWithPidgin
{
    [TestFixture]
    public sealed class LexerTests
    {
        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("0000045", 45)]
        [TestCase("123", 123)]
        [TestCase("-123", -123)]
        [TestCase("+123", 123)]
        [TestCase("0x1234", 0x1234)]
        public void ShouldParseInteger(string input, int expected)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<IntegerToken>());
            Assert.That(((IntegerToken)tokens[0]).Value, Is.EqualTo(expected));
        }

        [TestCase("0.", 0d)]
        [TestCase("1.", 1d)]
        [TestCase("1.23", 1.23)]
        [TestCase("-1.23", -1.23)]
        [TestCase("+1.23", 1.23)]
        public void ShouldParseFloat(string input, double expected)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<FloatToken>());
            Assert.That(((FloatToken)tokens[0]).Value, Is.EqualTo(expected));
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        public void ShouldParseBoolean(string input, bool expected)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<BooleanToken>());
            Assert.That(((BooleanToken)tokens[0]).Value, Is.EqualTo(expected));
        }

        private static Token[] Scan(string input)
        {
            using (var stringReader = new StringReader(input))
            {
                var lexer = new Lexer(stringReader);
                return lexer.Scan().ToArray();
            }
        }
    }
}