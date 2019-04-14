// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing.PidginVersion.AbstractSyntaxTree;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.PidginVersion
{
    public static partial class UdmfSemanticAnalyzer
    {
        public static MapData Process(IEnumerable<IGlobalExpression> result)
        {
            var map = new MapData();

            foreach (var globalExpression in result)
            {
                switch (globalExpression)
                {
                    case Assignment assignment:
                        ProcessGlobalAssignment(map, assignment);
                        break;

                    case Block block:
                        ProcessBlock(map, block);
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

            unknownBlock.Properties.AddRange(block.Fields.Select(a => new UnknownProperty(a.Name, a.ValueAsString())));

            return unknownBlock;
        }

        static int ReadIntValue(Assignment assignment, string context) => ReadValue<int>(assignment, context);
        static bool ReadBoolValue(Assignment assignment, string context) => ReadValue<bool>(assignment, context);
        static string ReadStringValue(Assignment assignment, string context) => ReadValue<string>(assignment, context);

        static T ReadValue<T>(Assignment assignment, string context)
        {
            if (assignment.Value is T t)
            {
                return t;
            }
            throw new ParsingException($"Expected {typeof(T)} for {context} but got {assignment.Value.GetType()}.");
        }

        static double ReadDoubleValue(Assignment assignment, string context)
        {
            if (assignment.Value is int i)
            {
                return i;
            }
            if (assignment.Value is double d)
            {
                return d;
            }

            throw new ParsingException($"Expected {typeof(double)} for {context} but got {assignment.Value.GetType()}.");
        }
    }
}