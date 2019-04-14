// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Running;

namespace UdmfParsingBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MapLoadingBenchmarks>();
            BenchmarkRunner.Run<LoadAllFreedoomMapsBenchmarks>();
            BenchmarkRunner.Run<LoadZDCMP2Benchmarks>();
        }
    }
}
