// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion;
using Superpower;

namespace UdmfParsingTests.SuperpowerVersion
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [Test]
        public void ShouldParseAssignment()
        {
            var input = "a = 1;";

            var result = UdmfParser.Assignment.Parse(UdmfTokenizerRules.Tokenizer.Tokenize(input));

            Assert.That(result.Name.ToString(), Is.EqualTo("a"));
            Assert.That(result.Value.Data.ToStringValue(), Is.EqualTo("1"));
            Assert.That(result.Value.Type, Is.EqualTo(UdmfToken.Number));
        }

        [Test]
        public void ShouldParseEmptyBlock()
        {
            var input = "a { }";

            var result = UdmfParser.Block.Parse(UdmfTokenizerRules.Tokenizer.Tokenize(input));

            Assert.That(result.Name.ToString(), Is.EqualTo("a"));
            Assert.That(result.Fields, Is.Empty);
        }

        [Test]
        public void ShouldParseBlockWithAssignment()
        {
            var input = "a { b = 1.2; }";

            var result = UdmfParser.Block.Parse(UdmfTokenizerRules.Tokenizer.Tokenize(input));

            Assert.That(result.Name.ToString(), Is.EqualTo("a"));
            Assert.That(result.Fields.Length, Is.EqualTo(1));
            Assert.That(result.Fields[0].Name.ToString(), Is.EqualTo("b"));
        }

        [Test]
        public void ShouldParseGlobalScope()
        {
            var input = "a { b = 1.2; }" +
                        "c = true;";

            var result = UdmfParser.GlobalExpressions.Parse(UdmfTokenizerRules.Tokenizer.Tokenize(input));

            Assert.That(result.Length, Is.EqualTo(2));
        }

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
                    var text = textReader.ReadToEnd();

                    var result = UdmfParser.GlobalExpressions.Parse(UdmfTokenizerRules.Tokenizer.Tokenize(text));
                }
            }
        }
    }
}