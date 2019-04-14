// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion.AbstractSyntaxTree;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion
{
    public static partial class UdmfSemanticAnalyzer
    {
        public static MapData Process(IGlobalExpression[] result)
        {
            var map = new MapData();

            foreach (var globalExpression in result)
            {
                switch (globalExpression)
                {
                    case Assignment a:
                        ProcessGlobalAssignment(map,a);
                        break;

                    case Block b:
                        ProcessBlock(map,b);
                        break;
                }
            }

            return map;
        }

        static partial void ProcessGlobalAssignment(MapData map, Assignment assignment);
        static partial void ProcessBlock(MapData map, Block block);

        static UnknownBlock ProcessUnknownBlock(Block block)
        {
            var unknownBlock = new UnknownBlock(block.Name);

            unknownBlock.Properties.AddRange(block.Fields.Select(a => new UnknownProperty(a.Name, a.Value.Data.ToStringValue())));

            return unknownBlock;
        }

        static int ReadIntValue(Assignment assignment, string context)
        {
            if (assignment.Value.Type == UdmfToken.Number)
            {
                return int.Parse(assignment.Value.Data.ToStringValue());
            }
            throw new ParsingException($"Expected {UdmfToken.Number} for {context} but got {assignment.Value.Type}.");
        }

        static bool ReadBoolValue(Assignment assignment, string context)
        {
            if (assignment.Value.Type == UdmfToken.True)
            {
                return true;
            }
            if (assignment.Value.Type == UdmfToken.False)
            {
                return false;
            }
            throw new ParsingException($"Expected boolean for {context} but got {assignment.Value.Type}.");
        }

        static string ReadStringValue(Assignment assignment, string context)
        {
            if (assignment.Value.Type == UdmfToken.QuotedString)
            {
                var quotedString = assignment.Value.Data.ToStringValue();
                return quotedString.Substring(1, quotedString.Length - 2);
            }
            throw new ParsingException($"Expected {UdmfToken.QuotedString} for {context} but got {assignment.Value.Type}.");
        }

        static double ReadDoubleValue(Assignment assignment, string context)
        {
            if (assignment.Value.Type == UdmfToken.Number)
            {
                return double.Parse(assignment.Value.Data.ToStringValue());
            }
            throw new ParsingException($"Expected {UdmfToken.Number} for {context} but got {assignment.Value.Type}.");
        }
    }
}