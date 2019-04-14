// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using Pidgin;

namespace SectorDirector.Core.FormatModels.Udmf
{
    public sealed partial class MapData
    {
        public static MapData LoadFromUsingPidgin(TextReader reader)
        {
            var result = Parsing.PidginVersion.UdmfParser.TranslationUnit.ParseOrThrow(reader);
            return Parsing.PidginVersion.UdmfSemanticAnalyzer.Process(result);
        }

        public static MapData LoadFromUsingPidgin(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                return LoadFromUsingPidgin(textReader);
            }
        }
    }
}