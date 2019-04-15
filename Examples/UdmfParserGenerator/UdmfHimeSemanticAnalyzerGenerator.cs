// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using UdmfParserGenerator.DefinitionModel;
using UdmfParserGenerator.Utilities;

namespace UdmfParserGenerator
{ 
    public static class UdmfHimeSemanticAnalyzerGenerator
    {
        public static void WriteTo(StreamWriter stream)
        {
            using (var output = new IndentedWriter(stream))
            {
                output.Line(
                        $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Linq;
using Hime.Redist;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.HimeVersion").OpenParen()
                    .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
                    .Line("public static partial class UdmfSemanticAnalyzer").OpenParen();

                WriteGlobalFieldParsing(output);

                output.Line();

                WriteBlockParsing(output);

                output.Line();

                foreach (var block in UdmfDefinitions.Blocks.Where(b => b.IsSubBlock))
                {
                    WriteBlockParser(block, output);
                }

                output.CloseParen();
                output.CloseParen();
            }
        }

        private static void WriteGlobalFieldParsing(IndentedWriter output)
        {
            output.
                Line("static partial void ProcessGlobalExpression(MapData map, ASTNode assignment)").
                OpenParen();

            var block = UdmfDefinitions.Blocks.Single(b => b.CodeName.ToPascalCase() == "MapData");

            WriteFieldSwitch(output, block, "map");

            output.
                CloseParen();
        }

        private static void WriteBlockParsing(IndentedWriter output)
        {
            output.
                Line("static partial void ProcessBlock(MapData map, ASTNode block)").
                OpenParen().
                Line("var blockName = new Identifier(block.Children[0].Value);").
                Line("switch (blockName.ToLower())").
                OpenParen();

            foreach (var block in UdmfDefinitions.Blocks.Single(b => b.CodeName.ToPascalCase() == "MapData").SubBlocks
                .Where(b => b.IsRequired))
            {
                output.
                    Line($"case \"{block.FormatName.ToLowerInvariant()}\":").
                    IncreaseIndent().
                    Line($"map.{block.PropertyName}.Add(Process{block.FormatName.ToPascalCase()}(block));").
                    Line("break;").
                    DecreaseIndent();
            }

            output.
                Line("default:").
                IncreaseIndent().
                Line($"map.UnknownBlocks.Add(ProcessUnknownBlock(blockName, block));").
                Line("break;").
                DecreaseIndent().
                CloseParen().
                CloseParen();
        }

        private static void WriteBlockParser(Block block, IndentedWriter output)
        {
            var variable = block.CodeName.ToCamelCase();

            output.
                Line($"static {block.CodeName} Process{block.CodeName}(ASTNode block)").
                OpenParen().
                Line($"var {variable} = new {block.CodeName}();").
                Line("foreach (var assignment in block.Children.Skip(1))").
                OpenParen();

            WriteFieldSwitch(output, block, variable);
            
            output.
                CloseParen().
                Line($"return {variable};").
                CloseParen().
                Line();
        }

        private static void WriteFieldSwitch(IndentedWriter output, Block block, string variable)
        {
            output.
                Line("var id = GetAssignmentIdentifier(assignment);").
                Line("switch (id.ToLower())").
                OpenParen();


            foreach (var field in block.Fields)
            {
                output.
                    Line($"case \"{field.FormatName.ToLowerInvariant()}\":").
                    IncreaseIndent().
                    Line($"{variable}.{field.PropertyName} = Read{field.PropertyType.ToPascalCase()}(assignment, \"{block.CodeName}.{field.PropertyName}\");").
                    Line("break;").
                    DecreaseIndent();
            }

            output.
                Line("default:").
                IncreaseIndent().
                Line($"{variable}.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));").
                Line("break;").
                DecreaseIndent().
                CloseParen();
        }
    }
}