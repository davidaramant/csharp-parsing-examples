// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;

namespace SectorDirector.DataModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionBasePath = Path.Combine(Enumerable.Repeat("..", 5).ToArray());
            var corePath = Path.Combine(solutionBasePath, "UdmfParsing");
            var udmfPath = Path.Combine(corePath, "FormatModels", "Udmf");
            var udmfParsingPath = Path.Combine(udmfPath, "Parsing");

            // Create data model
            UdmfModelGenerator.WriteToPath(udmfPath);

            using (var analyzerStream = File.CreateText(Path.Combine(udmfParsingPath, "UdmfSemanticAnalyzer.Generated.cs")))
            {
                UdmfSemanticAnalyzerGenerator.WriteTo(analyzerStream);
            }
        }
    }
}
