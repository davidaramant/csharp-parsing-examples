// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using NUnit.Framework;
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.PigletVersion;

namespace UdmfParsingTests.PigletVersion
{ 
    [TestFixture]
    public sealed class UdmfSyntaxAnalyzerTests
    {
        [Test]
        public void ShouldParseGlobalAssignment()
        {
            var input = @"prop = 1;";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(
                result.GetGlobalAssignments().ToArray(),
                Is.EqualTo(new[] { new Assignment(new Identifier("prop"), Token.Integer(1)) }),
                "Did not parse the assignment.");
        }

        [Test]
        public void ShouldParseMultipleGlobalAssignments()
        {
            var input = @"prop = 1;prop2 = ""string"";";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(
                result.GetGlobalAssignments().ToArray(),
                Is.EqualTo(new[]
                {
                    new Assignment(new Identifier("prop"), Token.Integer(1)),
                    new Assignment(new Identifier("prop2"), Token.String("string"))
                }),
                "Did not parse the assignments.");
        }

        [Test]
        public void ShouldParseEmptyBlock()
        {
            var input = @"block {}";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(result.Blocks, Has.Count.EqualTo(1), "Did not parse out a block");

            var block = result.Blocks.First();

            Assert.That(block.Name, Is.EqualTo(new Identifier("block")), "Did not get correct name.");
        }

        [Test]
        public void ShouldParseFullBlock()
        {
            var input = @"block {prop = 1;prop2 = ""string"";}";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(result.Blocks, Has.Count.EqualTo(1), "Did not parse out a block");

            var block = result.Blocks.First();

            Assert.That(block.Name, Is.EqualTo(new Identifier("block")), "Did not get correct name.");
            Assert.That(block.ToArray(), Is.EqualTo(new[]
            {
                new Assignment(new Identifier("prop"), Token.Integer(1)),
                new Assignment(new Identifier("prop2"), Token.String("string"))
            }), "Did not parse assignments.");
        }

        [Test]
        public void ShouldParseFullBlockMixedWithGlobalAssignments()
        {
            var input = @"gProp =1;block {prop = 1;prop2 = ""string"";}gProp2 = 5;";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(result.Blocks, Has.Count.EqualTo(1), "Did not parse out a block");

            var block = result.Blocks.First();

            Assert.That(block.Name, Is.EqualTo(new Identifier("block")), "Did not get correct name.");
            Assert.That(block.ToArray(), Is.EqualTo(new[]
                {
                    new Assignment(new Identifier("prop"), Token.Integer(1)),
                    new Assignment(new Identifier("prop2"), Token.String("string"))
                }), "Did not parse assignments.");

            Assert.That(
                result.GetGlobalAssignments().ToArray(),
                Is.EqualTo(new[]
                {
                    new Assignment(new Identifier("gProp"), Token.Integer(1)),
                    new Assignment(new Identifier("gProp2"), Token.Integer(5))
                }),
                "Did not parse the assignments.");
        }

        [Test]
        public void ShouldParseArrayBlockWithOneTuple()
        {
            var input = @"block {{1,2,3}}";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(result.ArrayBlocks, Has.Count.EqualTo(1), "Did not parse out a block");

            var block = result.ArrayBlocks.First();

            Assert.That(block.Name, Is.EqualTo(new Identifier("block")), "Did not get correct name.");
            Assert.That(block.First(), Is.EqualTo(new[] { 1, 2, 3 }),
                "Did not parse block.");
        }

        [Test]
        public void ShouldParseArrayBlockWitMultipleTuples()
        {
            var input = @"block {{1,2,3},{4,5,6},{7,8,9,10}}";

            var syntaxAnalzer = new UdmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UdmfLexer(new StringReader(input)));

            Assert.That(result.ArrayBlocks, Has.Count.EqualTo(1), "Did not parse out a block");

            var block = result.ArrayBlocks.First();

            Assert.That(block.Name, Is.EqualTo(new Identifier("block")), "Did not get correct name.");
            Assert.That(block[0], Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(block[1], Is.EqualTo(new[] { 4, 5, 6 }));
            Assert.That(block[2], Is.EqualTo(new[] { 7, 8, 9, 10 }));
        }
    }
}