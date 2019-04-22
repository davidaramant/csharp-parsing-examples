// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin;
using System.IO;
using System.Text;
using Pidgin;
using NUnit.Framework;

namespace UdmfParsingTests.CustomLexerWithPidgin
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [Test]
        public void ShouldParseAssignment()
        {
            var tokenStream = new Token[]
            {
                new IdentifierToken(FilePosition.StartOfFile(), new Identifier("id")),
                new EqualsToken(FilePosition.StartOfFile()),
                new IntegerToken(FilePosition.StartOfFile(), 5),
                new SemicolonToken(FilePosition.StartOfFile()),
            };

            var assignment = UdmfParser.Assignment.ParseOrThrow(tokenStream);
        }

        [Test]
        public void ShouldParseBlock()
        {
            var tokenStream = new Token[]
            {
                new IdentifierToken(FilePosition.StartOfFile(), new Identifier("blockName")),
                new OpenBraceToken(FilePosition.StartOfFile()),
                new IdentifierToken(FilePosition.StartOfFile(), new Identifier("id")),
                new EqualsToken(FilePosition.StartOfFile()),
                new IntegerToken(FilePosition.StartOfFile(), 5),
                new SemicolonToken(FilePosition.StartOfFile()),
                new CloseBraceToken(FilePosition.StartOfFile()),
            };

            var block = UdmfParser.Block.ParseOrThrow(tokenStream);
            Assert.That(block.Fields, Has.Length.EqualTo(1));
        }

        [Test]
        public void ShouldHandleParsingDemoMap()
        {
            var map = DemoMap.Create();

            using (var fs = File.OpenWrite("text.udmf"))
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
                    var result = UdmfParser.TranslationUnit.ParseOrThrow(lexer.Scan());
                }
            }
        }
    }
}