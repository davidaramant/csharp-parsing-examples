using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using UdmfParsing.Udmf;
using UdmfParsing.Wad;

namespace UdmfParsingBenchmarks
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 0, targetCount: 1, invocationCount: 1)]
    public class LoadAllFreedoomMapsBenchmarks
    {
        [Benchmark(Baseline = true)]
        public List<MapData> Piglet()
        {
            return WadLoader.LoadUsingPiglet("freedoom2-udmf.wad");
        }

        [Benchmark]
        public List<MapData> Pidgin()
        {
            return WadLoader.LoadUsingPidgin("freedoom2-udmf.wad");
        }

        [Benchmark]
        public List<MapData> CustomLexerWithPidginParser()
        {
            return WadLoader.LoadUsingCustomWithPidgin("freedoom2-udmf.wad");
        }

        [Benchmark]
        public List<MapData> TotallyCustom()
        {
            return WadLoader.LoadUsingTotallyCustom("freedoom2-udmf.wad");
        }

        [Benchmark]
        public List<MapData> Superpower()
        {
            return WadLoader.LoadUsingSuperpower("freedoom2-udmf.wad");
        }
    }
}
