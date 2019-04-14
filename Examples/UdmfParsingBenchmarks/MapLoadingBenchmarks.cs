// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;

namespace UdmfParsingBenchmarks
{
    [SimpleJob(RunStrategy.Monitoring)]
    [CsvExporter]
    public class MapLoadingBenchmarks
    {
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
    }
}
