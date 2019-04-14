// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using SectorDirector.Core.FormatModels.Udmf;
using SectorDirector.Core.FormatModels.Wad;

namespace Benchmarks
{
    [SimpleJob(RunStrategy.Monitoring)]
    public class MapLoadingBenchmarks
    {
        [Benchmark]
        public MapData LoadLargeMap()
        {
            using (var wad = WadReader.Read("freedoom2-udmf.wad"))
            {
                return MapData.LoadFromUsingPidgin(wad.GetMapStream("MAP28"));
            }
        }

        public static IEnumerable<MapData> LoadAllFreedoomMaps()
        {
            Console.WriteLine("Loading all Freedoom maps...");
            using (new Timed())
            {
                return WadLoader.LoadUsingPidgin("freedoom2-udmf.wad");
            }
        }

        public static MapData LoadZDCMP2()
        {
            Console.WriteLine("Loading ZDCMP2...");
            using (var reader = WadReader.Read("zdcmp2.wad"))
            {
                var stream = reader.GetLumpStream(reader.Directory.First(l => l.Name == "TEXTMAP"));
                using (new Timed())
                {
                    return MapData.LoadFromUsingPidgin(stream);
                }
            }
        }

        sealed class Timed : IDisposable
        {
            private readonly Stopwatch _timer = Stopwatch.StartNew();

            public void Dispose()
            {
                _timer.Stop();
                Console.WriteLine(_timer.Elapsed);
            }
        }
    }
}
