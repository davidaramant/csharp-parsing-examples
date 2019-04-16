// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace UdmfParsing.Udmf.Parsing.PigletVersion
{
    public static partial class UdmfParser
    {
        public static MapData Parse(UdmfSyntaxTree syntaxTree)
        {
            var map = new MapData();

            SetGlobalAssignments(map, syntaxTree);
            SetBlocks(map, syntaxTree);

            map.CheckSemanticValidity();

            return map;
        }

        static partial void SetGlobalAssignments(MapData mapData, UdmfSyntaxTree tree);
        static partial void SetBlocks(MapData mapData, UdmfSyntaxTree tree);
    }
}