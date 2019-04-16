// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using UdmfParsing.Udmf.Parsing.PigletVersion;
using Is = NUnit.DeepObjectCompare.Is;

namespace UdmfParsingTests.PigletVersion
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [Test]
        public void ShouldRoundTripDemoMap()
        {
            var map = DemoMap.Create();

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var sa = new UdmfSyntaxAnalyzer();
                    var roundTripped = UdmfParser.Parse(sa.Analyze(new UdmfLexer(textReader)));

                    Assert.That(roundTripped, Is.DeepEqualTo(map));                   
                }
            }
        }

        [Test]
        public void ShouldParseOldDemoMap()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "PigletVersion", "TEXTMAP.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new UdmfSyntaxAnalyzer();
                var map = UdmfParser.Parse(sa.Analyze(new UdmfLexer(textReader)));
            }
        }
    }
}