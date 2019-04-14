using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;

namespace UdmfParsingBenchmarks
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 0, targetCount: 1, invocationCount: 1)]
    [CsvExporter]
    public class LoadAllFreedoomMapsBenchmarks
    {
        [Benchmark]
        public List<MapData> Pidgin()
        {
            return WadLoader.LoadUsingPidgin("freedoom2-udmf.wad");
        }

        [Benchmark]
        public List<MapData> Superpower()
        {
            return WadLoader.LoadUsingSuperpower("freedoom2-udmf.wad");
        }
    }
}
