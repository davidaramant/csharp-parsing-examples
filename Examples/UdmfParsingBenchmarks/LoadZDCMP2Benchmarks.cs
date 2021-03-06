﻿using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using UdmfParsing.Udmf;
using UdmfParsing.Wad;

namespace UdmfParsingBenchmarks
{
//    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 0, targetCount: 1, invocationCount: 1)]
    [SimpleJob(RunStrategy.Monitoring)]
    public class LoadZDCMP2Benchmarks
    {
        [Benchmark]
        public MapData Piglet()
        {
            using (var reader = WadReader.Read("zdcmp2.wad"))
            {
                var stream = reader.GetLumpStream(reader.Directory.First(l => l.Name == "TEXTMAP"));
                return MapData.LoadFromUsingPiglet(stream);
            }
        }

        [Benchmark]
        public MapData Pidgin()
        {
            using (var reader = WadReader.Read("zdcmp2.wad"))
            {
                var stream = reader.GetLumpStream(reader.Directory.First(l => l.Name == "TEXTMAP"));
                return MapData.LoadFromUsingPidgin(stream);
            }
        }

        [Benchmark]
        public MapData CustomLexerWithPidginParser()
        {
            using (var reader = WadReader.Read("zdcmp2.wad"))
            {
                var stream = reader.GetLumpStream(reader.Directory.First(l => l.Name == "TEXTMAP"));
                return MapData.LoadFromUsingCustomWithPidgin(stream);
            }
        }

        [Benchmark(Baseline = true)]
        public MapData TotallyCustom()
        {
            using (var reader = WadReader.Read("zdcmp2.wad"))
            {
                var stream = reader.GetLumpStream(reader.Directory.First(l => l.Name == "TEXTMAP"));
                return MapData.LoadFromUsingTotallyCustom(stream);
            }
        }

        [Benchmark]
        public MapData Superpower()
        {
            using (var reader = WadReader.Read("zdcmp2.wad"))
            {
                var stream = reader.GetLumpStream(reader.Directory.First(l => l.Name == "TEXTMAP"));
                return MapData.LoadFromUsingSuperpower(stream);
            }
        }
    }
}
