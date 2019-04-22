// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin;
using NUnit.Framework;
using System.Linq;
using System.IO;
using System.Text;

namespace UdmfParsingTests.CustomLexerWithPidgin
{
    [TestFixture]
    public sealed class UdmfLexerTests
    {
        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("0000045", 45)]
        [TestCase("123", 123)]
        [TestCase("-123", -123)]
        [TestCase("+123", 123)]
        [TestCase("0x1234", 0x1234)]
        public void ShouldLexInteger(string input, int expected)
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
        public void ShouldLexFloat(string input, double expected)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<FloatToken>());
            Assert.That(((FloatToken)tokens[0]).Value, Is.EqualTo(expected));
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        public void ShouldLexBoolean(string input, bool expected)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<BooleanToken>());
            Assert.That(((BooleanToken)tokens[0]).Value, Is.EqualTo(expected));
        }

        [TestCase("\"\"", "")]
        [TestCase("\"Some value 123 _\"", "Some value 123 _")]
        public void ShouldLexString(string input, string expected)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<StringToken>());
            Assert.That(((StringToken)tokens[0]).Value, Is.EqualTo(expected));
        }

        [TestCase("a")]
        [TestCase("A")]
        [TestCase("_")]
        [TestCase("someName_123")]
        [TestCase("t")]
        [TestCase("true_1")]
        public void ShouldLexIdentifier(string id)
        {
            var tokens = Scan(id);
            Assert.That(tokens, Has.Length.EqualTo(1));
            Assert.That(tokens[0], Is.TypeOf<IdentifierToken>());
            Assert.That(((IdentifierToken)tokens[0]).Id.ToString(), Is.EqualTo(id));
        }

        [TestCase("id=1;")]
        [TestCase(" id = 1 ; ")]
        [TestCase("// Some comment\n id = 1 ; ")]
        [TestCase("/*\nSome comment\n*/id = 1 ; ")]
        [TestCase("id=1;//ending comment")]
        [TestCase("id=1;/*ending block comment*/")]
        public void ShouldLexAssignment(string input)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(4));
            Assert.That(tokens[0], Is.TypeOf<IdentifierToken>());
            Assert.That(tokens[1], Is.TypeOf<EqualsToken>());
            Assert.That(tokens[2], Is.TypeOf<IntegerToken>());
            Assert.That(tokens[3], Is.TypeOf<SemicolonToken>());
        }

        [TestCase("blockName{}")]
        [TestCase("blockName\n{\n}\n")]
        public void ShouldLexBlock(string input)
        {
            var tokens = Scan(input);
            Assert.That(tokens, Has.Length.EqualTo(3));
            Assert.That(tokens[0], Is.TypeOf<IdentifierToken>());
            Assert.That(tokens[1], Is.TypeOf<OpenBraceToken>());
            Assert.That(tokens[2], Is.TypeOf<CloseBraceToken>());
        }

        private static Token[] Scan(string input)
        {
            using (var stringReader = new StringReader(input))
            {
                var lexer = new UdmfLexer(stringReader);
                return lexer.Scan().ToArray();
            }
        }

        [Test]
        public void ShouldHandleLexingDemoMap()
        {
            var map = DemoMap.Create();

            using(var fs = File.OpenWrite("text.udmf"))
            {
                map.WriteTo(fs);
            }

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var lexer = new UdmfLexer(textReader);
                    var result = lexer.Scan().ToArray();
                }
            }
        }
    }
}