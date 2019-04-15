// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UdmfParserGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionBasePath = Path.Combine(Enumerable.Repeat("..", 4).ToArray());
            var corePath = Path.Combine(solutionBasePath, "UdmfParsing");
            var udmfPath = Path.Combine(corePath, "FormatModels", "Udmf");
            var udmfParsingPath = Path.Combine(udmfPath, "Parsing");

            // Create data model
            UdmfModelGenerator.WriteToPath(udmfPath);

            using (var analyzerStream = File.CreateText(Path.Combine(udmfParsingPath, "PidginVersion", "UdmfSemanticAnalyzer.Generated.cs")))
            {
                UdmfPidginSemanticAnalyzerGenerator.WriteTo(analyzerStream);
            }

            using (var analyzerStream = File.CreateText(Path.Combine(udmfParsingPath, "SuperpowerVersion", "UdmfSemanticAnalyzer.Generated.cs")))
            {
                UdmfSuperpowerSemanticAnalyzerGenerator.WriteTo(analyzerStream);
            }

            // Generate HIME lexer / parser
            using (var himeProcess = Process.Start(
                Path.Combine(solutionBasePath, "..", "hime", "himecc.bat"),
                "Udmf.gram"))
            {
                var himeOutputPath = Path.Combine(udmfParsingPath, "HimeVersion");

                // Generate mapping of HIME output to data model
                using (var analyzerStream = File.CreateText(Path.Combine(himeOutputPath, "UdmfSemanticAnalyzer.Generated.cs")))
                {
                    UdmfHimeSemanticAnalyzerGenerator.WriteTo(analyzerStream);
                }


                himeProcess.WaitForExit();
                if (himeProcess.ExitCode != 0)
                {
                    Console.ReadLine();
                }
                else
                {
                    void CopyFileToParsingPath(string fileName)
                    {
                        File.Copy(fileName, Path.Combine(himeOutputPath, fileName), overwrite: true);
                    }

                    CopyFileToParsingPath("UdmfLexer.cs");
                    CopyFileToParsingPath("UdmfParser.cs");
                    CopyFileToParsingPath("UdmfLexer.bin");
                    CopyFileToParsingPath("UdmfParser.bin");
                }
            }
        }
    }
}
