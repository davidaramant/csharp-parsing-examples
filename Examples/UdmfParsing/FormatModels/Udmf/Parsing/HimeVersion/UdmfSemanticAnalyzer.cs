// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Hime.Redist;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.HimeVersion
{ 
    public static partial class UdmfSemanticAnalyzer
    {
        public static MapData Process(ParseResult result)
        {
            if (!result.IsSuccess)
            {
                throw new ParsingException($"{result.Errors.Count} errors found trying to parse UDMF");
            }

            var map = new MapData();

            foreach (var globalExpression in result.Root.Children)
            {
                var child = globalExpression.Children[0];
                if (child.Symbol.Name == "assignment_expr")
                {
                    ProcessGlobalExpression(map, child);
                }
                else
                {
                    ProcessBlock(map, child);
                }
            }

            return map;
        }

        static partial void ProcessGlobalExpression(MapData map, ASTNode assignment);
        static partial void ProcessBlock(MapData map, ASTNode block);

        static UnknownBlock ProcessUnknownBlock(Identifier blockName, ASTNode block)
        {
            var unknownBlock = new UnknownBlock(blockName);

            var allAssignments = block.Children.Skip(1);
            unknownBlock.Properties.AddRange(
                from assignment in allAssignments
                let id = GetAssignmentIdentifier(assignment)
                let value = ReadRawValue(assignment)
                select new UnknownProperty(id, value));

            return unknownBlock;
        }

        static Identifier GetAssignmentIdentifier(ASTNode assignment) => new Identifier(assignment.Children[0].Value);

        static string ReadRawValue(ASTNode assignment) => assignment.Children[1].Value;

        static int ReadInt(ASTNode assignment, string context)
        {
            var valueChild = assignment.Children[1];
            var type = valueChild.Symbol.Name;

            if (type != "INTEGER")
            {
                throw new ParsingException($"Expected INTEGER for {context} but got {type}.");
            }

            return int.Parse(valueChild.Value);
        }

        static double ReadDouble(ASTNode assignment, string context)
        {
            var valueChild = assignment.Children[1];
            var type = valueChild.Symbol.Name;

            if (type != "FLOAT" && type != "INTEGER")
            {
                throw new ParsingException($"Expected FLOAT for {context} but got {type}.");
            }

            return double.Parse(valueChild.Value);
        }

        static bool ReadBool(ASTNode assignment, string context)
        {
            var valueChild = assignment.Children[1];
            var type = valueChild.Symbol.Name;

            if (type != "BOOLEAN")
            {
                throw new ParsingException($"Expected BOOLEAN for {context} but got {type}.");
            }

            return bool.Parse(valueChild.Value);
        }

        static string ReadString(ASTNode assignment, string context)
        {
            var valueChild = assignment.Children[1];
            var type = valueChild.Symbol.Name;

            if (type != "QUOTED_STRING")
            {
                throw new ParsingException($"Expected QUOTED_STRING for {context} but got {type}.");
            }

            var quotedString = valueChild.Value;
            return quotedString.Substring(1, quotedString.Length - 2);
        }
    }
}