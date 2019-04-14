// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MapLoadingBenchmarks>();

            GC.KeepAlive(MapLoadingBenchmarks.LoadAllFreedoomMaps());
            GC.KeepAlive(MapLoadingBenchmarks.LoadZDCMP2());
        }
    }
}
