﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin.AbstractSyntaxTree;

namespace UdmfParsing.Udmf.Parsing.CustomLexerWithPidgin
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

        static int ReadIntValue(Assignment assignment, string context)
        {
            if (assignment.Value is IntegerToken i)
            {
                return i.Value;
            }

            throw new ParsingException($"Expected {typeof(int)} for {context} but got {assignment.Value.GetType()}.");
        }
        static double ReadDoubleValue(Assignment assignment, string context)
        {
            if (assignment.Value is FloatToken f)
            {
                return f.Value;
            }

            throw new ParsingException($"Expected {typeof(double)} for {context} but got {assignment.Value.GetType()}.");
        }
        static bool ReadBoolValue(Assignment assignment, string context)
        {
            if (assignment.Value is BooleanToken b)
            {
                return b.Value;
            }

            throw new ParsingException($"Expected {typeof(bool)} for {context} but got {assignment.Value.GetType()}.");
        }
        static string ReadStringValue(Assignment assignment, string context)
        {
            if (assignment.Value is StringToken s)
            {
                return s.Value;
            }

            throw new ParsingException($"Expected {typeof(string)} for {context} but got {assignment.Value.GetType()}.");
        }

    }
}