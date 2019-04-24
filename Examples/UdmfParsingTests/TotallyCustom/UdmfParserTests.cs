// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.TotallyCustom;
using System.IO;
using System.Text;
using NUnit.Framework;
using System.Linq;
using UdmfParsing.Udmf.Parsing.TotallyCustom.AbstractSyntaxTree;

namespace UdmfParsingTests.TotallyCustom
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

            var results = UdmfParser.Parse(tokenStream).ToArray();
            Assert.That(results, Has.Length.EqualTo(1));
            Assert.That(results[0], Is.TypeOf<Assignment>());
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

            var results = UdmfParser.Parse(tokenStream).ToArray();
            Assert.That(results, Has.Length.EqualTo(1));
            Assert.That(results[0], Is.TypeOf<Block>());
            Assert.That(((Block)results[0]).Fields, Has.Length.EqualTo(1));
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
                    var result = UdmfParser.Parse(lexer.Scan()).ToArray();
                }
            }
        }
    }
}