// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using UdmfParsing.Udmf;
using UdmfParsing.Wad;

namespace UdmfParsingBenchmarks
{
    [SimpleJob(RunStrategy.Monitoring)]
    public class MapLoadingBenchmarks
    {
        [Benchmark]
        public MapData Piglet()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingPiglet(wad.GetMapStream("MAP28"));
            }
        }

        [Benchmark]
        public MapData CustomLexerWithPidginParser()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingCustomWithPidgin(wad.GetMapStream("MAP28"));
            }
        }

        [Benchmark(Baseline = true)]
        public MapData TotallyCustom()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingTotallyCustom(wad.GetMapStream("MAP28"));
            }
        }

        [Benchmark]
        public MapData Pidgin()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingPidgin(wad.GetMapStream("MAP28"));
            }
        }

        [Benchmark]
        public MapData Superpower()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingSuperpower(wad.GetMapStream("MAP28"));
            }
        }

        [Benchmark]
        public MapData Hime()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingHime(wad.GetMapStream("MAP28"));
            }
        }
    }
}
