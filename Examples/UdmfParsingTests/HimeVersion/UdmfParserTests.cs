// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using Hime.Redist;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf.Parsing.HimeVersion;

namespace UdmfParsingTests.HimeVersion
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [Test]
        public void ShouldHandleParsingDemoMap()
        {
            var map = DemoMap.Create();

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var lexer = new UdmfLexer(textReader);
                    var parser = new UdmfParser(lexer);

                    ParseResult result = parser.Parse();
                    Assert.That(result.IsSuccess, Is.True);
                }
            }
        }

        [Test]
        public void ShouldHandleParsingMapWithSingleLineComment()
        {
            var map = @" // Here is a comment
namespace = ""Doom"";";
            var lexer = new UdmfLexer(map);
            var parser = new UdmfParser(lexer);

            ParseResult result = parser.Parse();
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void ShouldHandleParsingMapWithMultiLineComment()
        {
            var map = @" /* Here is a comment
that is multiline */
namespace = ""Doom"";";
            var lexer = new UdmfLexer(map);
            var parser = new UdmfParser(lexer);

            ParseResult result = parser.Parse();
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}