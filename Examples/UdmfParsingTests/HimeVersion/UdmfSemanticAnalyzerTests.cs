﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using UdmfParsing.Common;
using UdmfParsing.Udmf;
using UdmfParsing.Wad;
using Is = NUnit.DeepObjectCompare.Is;

namespace UdmfParsingTests.HimeVersion
{
    [TestFixture]
    public sealed class UdmfSemanticAnalyzerTests
    {
        [TestCase("boring")]
        [TestCase("With Spaces")]
        [TestCase("numbers1")]
        [TestCase("weird chars \\")]
        public void ShouldHandleQuotedStrings(string text)
        {
            var udmf = $"namespace = \"{text}\";";
            var mapData = Parse(udmf);
            Assert.That(mapData.NameSpace, Is.EqualTo(text));
        }

        [Test]
        public void ShouldParseGlobalFields()
        {
            var map = new MapData()
            {
                NameSpace = "NameSpace",
                Comment = "Comment",
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseUnknownGlobalFields()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                UnknownProperties = { new UnknownProperty(new Identifier("user_global"), "\"someValue\"") }
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseBlocks()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                Vertices = { new Vertex(1, 2) }
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseUnknownBlocks()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                UnknownBlocks =
                {
                    new UnknownBlock(new Identifier("SomeWeirdBlock"))
                    {
                        Properties =
                        {
                            new UnknownProperty(new Identifier("id1"), "1" ),
                            new UnknownProperty(new Identifier("id2"), "true" ),
                        }
                    }
                }
            };

            AssertRoundTrip(map);
        }

        [Test]
        public void ShouldParseUnknownFieldsInBlock()
        {
            var map = new MapData()
            {
                NameSpace = "ThisIsRequired",
                Vertices = { new Vertex(1, 2)
                {
                    UnknownProperties =
                    {
                        new UnknownProperty(new Identifier("id1"), "1" ),
                        new UnknownProperty(new Identifier("id2"), "true" ),
                    }
                } },
            };

            AssertRoundTrip(map);
        }


        [Test]
        public void ShouldRoundTripDemoMap()
        {
            AssertRoundTrip(DemoMap.Create());
        }

        [Test, Explicit]
        public void ShouldLoadAllTestMaps()
        {
            foreach (var wadPath in Directory.GetFiles(".", searchPattern: "*.wad"))
            {
                TestContext.WriteLine(Path.GetFileName(wadPath));
                using (var wadReader = WadReader.Read(wadPath))
                {
                    foreach (var mapName in wadReader.GetMapNames())
                    {
                        TestContext.WriteLine(" * " + mapName);
                        MapData.LoadFromUsingHime(wadReader.GetMapStream(mapName));
                    }
                }
            }
        }

        static void AssertRoundTrip(MapData map)
        {
            var roundTripped = RoundTrip(map);

            Assert.That(roundTripped, Is.DeepEqualTo(map));
        }

        static MapData RoundTrip(MapData map)
        {
            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                return MapData.LoadFromUsingHime(stream);
            }
        }

        static MapData Parse(string rawUdmf)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(rawUdmf)))
            {
                return MapData.LoadFromUsingHime(stream);
            }
        }

    }
}