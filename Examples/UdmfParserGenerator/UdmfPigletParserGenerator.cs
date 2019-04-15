
// This uses the pre-refactor metadata model so it won't compile

//// Copyright (c) 2016, David Aramant
//// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

//using System;
//using System.IO;
//using System.Linq;
//using UdmfParserGenerator.DefinitionModel;
//using UdmfParserGenerator.Utilities;

//namespace UdmfParserGenerator
//{
//    public static class UdmfPigletParserGenerator
//    {
//        public static void WriteTo(StreamWriter stream)
//        {
//            using (var output = new IndentedWriter(stream))
//            {
//                output.Line(
//                        $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
//// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

//using System.CodeDom.Compiler;
//using SectorDirector.Core.FormatModels.Common;

//namespace SectorDirector.Core.FormatModels.Udmf.Parsing").OpenParen()
//                    .Line($"[GeneratedCodeAttribute(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
//                    .Line("public static partial class UdmfParser").OpenParen();

//                WriteGlobalAssignmentParsing(output);
//                WriteBlockParsing(output);

//                foreach (var block in UdmfDefinitions.Blocks.Where(_ => _.NormalParsing))
//                {
//                    output.Line(
//                            $"public static {block.ClassName.ToPascalCase()} Parse{block.ClassName.ToPascalCase()}(IHaveAssignments block)")
//                        .OpenParen().Line($"var parsedBlock = new {block.ClassName.ToPascalCase()}();");

//                    WritePropertyAssignments(block, output, assignmentHolder: "block", owner: "parsedBlock");

//                    output.Line("return parsedBlock;").CloseParen();
//                }

//                output.CloseParen();
//                output.CloseParen();
//            }
//        }

//        private static void WritePropertyAssignments(Block block, IndentedWriter output, string assignmentHolder, string owner)
//        {
//            foreach (var property in block.Properties.Where(p => p.IsScalarField))
//            {
//                var level = property.IsRequired ? "Required" : "Optional";

//                output.Line(
//                    $"{assignmentHolder}.GetValueFor(\"{property.ClassName.ToPascalCase()}\")" +
//                    $".Set{level}{property.Type}(" +
//                    $"value => {owner}.{property.ClassName.ToPascalCase()} = value, " +
//                    $"\"{block.ClassName.ToPascalCase()}\", " +
//                    $"\"{property.ClassName.ToPascalCase()}\");");
//            }
//        }

//        private static void WriteGlobalAssignmentParsing(IndentedWriter output)
//        {
//            output.
//                Line("static partial void SetGlobalAssignments(MapData map, UdmfSyntaxTree tree)").
//                OpenParen();

//            WritePropertyAssignments(
//                UdmfDefinitions.Blocks.Single(b => b.ClassName.ToPascalCase() == "MapData"),
//                output, assignmentHolder: "tree", owner: "map");

//            output.CloseParen();
//        }

//        private static void WriteBlockParsing(IndentedWriter output)
//        {
//            output.
//                Line("static partial void SetBlocks(MapData map, UdmfSyntaxTree tree)").
//                OpenParen();

//            output.
//                Line("foreach (var block in tree.Blocks)").
//                OpenParen();

//            output.Line("switch(block.Name.ToLower())");
//            output.OpenParen();

//            // HACK: Get around the vertex/vertices problem
//            foreach (var block in UdmfDefinitions.Blocks.Single(_ => !_.IsSubBlock).Properties.Where(p => p.IsUdmfSubBlockList && p.Type != PropertyType.UnknownBlocks))
//            {
//                output.
//                    Line($"case \"{block.SingularName.ToLower()}\":").
//                    IncreaseIndent().
//                    Line($"map.{block.PropertyName}.Add(Parse{block.CollectionType}(block));").
//                    Line("break;").
//                    DecreaseIndent();
//            }

//            output.CloseParen();

//            output.CloseParen().CloseParen();
//        }
//    }
//}