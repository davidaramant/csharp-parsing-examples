// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pidgin;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing.PidginVersion;
using SectorDirector.Core.FormatModels.Udmf.Parsing.PidginVersion.AbstractSyntaxTree;

namespace UdmfParsingTests.PidginVersion
{
    [TestFixture]
    public sealed class UdmfParserTests
    {
        [TestCase("boring")]
        [TestCase("num123")]
        [TestCase("user_defined")]
        public void ShouldParseIdentifier(string idString)
        {
            Assert.That(UdmfParser.Identifier.ParseOrThrow(idString), Is.EqualTo(idString));
        }

        [Test]
        public void ShouldParseBoolean([Values(true, false)] bool value)
        {
            Assert.That(UdmfParser.Boolean.ParseOrThrow(value.ToString().ToLowerInvariant()), Is.EqualTo(value));
        }

        [Test]
        public void ShouldParseHexInteger()
        {
            Assert.That(UdmfParser.HexInteger.ParseOrThrow("0x1234"), Is.EqualTo(0x1234));
        }

        [TestCase("1", 1)]
        [TestCase("0001", 1)]
        [TestCase("123456789", 123456789)]
        [TestCase("+5", 5)]
        [TestCase("-6", -6)]
        [TestCase("0x1234", 0x1234)]
        public void ShouldParseInteger(string input, int expectedValue)
        {
            Assert.That(UdmfParser.Integer.ParseOrThrow(input), Is.EqualTo(expectedValue));
        }

        [TestCase("1.", 1d)]
        [TestCase("1.0", 1d)]
        [TestCase("+1.0", 1d)]
        [TestCase("-1.0", -1d)]
        [TestCase("1.234", 1.234d)]
        public void ShouldParseFloat(string input, double expectedValue)
        {
            Assert.That(UdmfParser.Float.ParseOrThrow(input), Is.EqualTo(expectedValue));
        }

        [TestCase("")]
        [TestCase("abc")]
        [TestCase("a b c")]
        [TestCase("AZaz09:;_.")]
        public void ShouldParseQuotedString(string input)
        {
            Assert.That(UdmfParser.QuotedString.ParseOrThrow($"\"{input}\""), Is.EqualTo(input));
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("123", 123)]
        [TestCase("0x123", 0x123)]
        [TestCase("1.23", 1.23d)]
        [TestCase("\"string\"", "string")]
        public void ShouldParseValue(string input, object expected)
        {
            Assert.That(UdmfParser.Value.ParseOrThrow(input), Is.EqualTo(expected));
        }

        [TestCase("a=1;")]
        [TestCase("a=1; ")]
        [TestCase("a=1;\n//YO")]
        [TestCase("a\n=\n1\n;")]
        [TestCase("a = 1 ;")]
        [TestCase("a  =  1  ;")]
        [TestCase("a\t=\t1\t;")]
        [TestCase("a/*this is really obnoxious*/=1;")]
        [TestCase("a//lots of\n=//comments\n1//everywhere\n;")]
        public void ShouldParseAssignmentWithVariousSeparators(string input)
        {
            var assignment = UdmfParser.Assignment.ParseOrThrow(input);

            Assert.That(assignment.Name.ToString(), Is.EqualTo("a"));
            Assert.That(assignment.Value, Is.EqualTo(1));
        }

        [TestCase("a=1;", 1)]
        [TestCase("a=1.2;", 1.2)]
        [TestCase("a=true;", true)]
        [TestCase("a=\"1\";", "1")]
        public void ShouldParseAssignmentWithVariousValues(string input, object expected)
        {
            var assignment = UdmfParser.Assignment.ParseOrThrow(input);

            Assert.That(assignment.Name.ToString(), Is.EqualTo("a"));
            Assert.That(assignment.Value, Is.EqualTo(expected));
        }

        [TestCase("a{}")]
        [TestCase("a { }")]
        [TestCase("a\t{\t}")]
        [TestCase("a\n{\n}")]
        public void ShouldParseEmptyBlockWithVariousSeparators(string input)
        {
            var block = UdmfParser.Block.ParseOrThrow(input);

            Assert.That(block.Name.ToString(), Is.EqualTo("a"));
            Assert.That(block.Fields, Is.Empty);
        }

        [TestCase("a{b=1;c=\"2\";}")]
        [TestCase(@"a
{
    // A comment for some reason
    b=1;
    c=""2"";
}")]
        public void ShouldParseBlockWithAssignmentsWithVariousSeparators(string input)
        {
            var block = UdmfParser.Block.ParseOrThrow(input);

            Assert.That(block.Name.ToString(), Is.EqualTo("a"));
            Assert.That(block.Fields, Has.Length.EqualTo(2));
            Assert.That(block.Fields[0], Is.EqualTo(new Assignment(new Identifier("b"), 1)));
            Assert.That(block.Fields[1], Is.EqualTo(new Assignment(new Identifier("c"), "2")));
        }

        [TestCase("a=1;", 1)]
        [TestCase("   a=1;   ", 1)]
        [TestCase("a=1;b{}", 2)]
        [TestCase("a=1;b{}c=2;d{e=true;}", 4)]
        public void ShouldParseRealSnippets(string input, int expectedCount)
        {
            var result = UdmfParser.TranslationUnit.ParseOrThrow(input);

            Assert.That(result.ToArray(), Has.Length.EqualTo(expectedCount));
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
                    var result = UdmfParser.TranslationUnit.ParseOrThrow(textReader);
                }
            }
        }
    }
}