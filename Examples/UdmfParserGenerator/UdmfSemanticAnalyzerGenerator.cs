// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using SectorDirector.DataModelGenerator.DefinitionModel;
using SectorDirector.DataModelGenerator.Utilities;

namespace SectorDirector.DataModelGenerator
{
    public static class UdmfSemanticAnalyzerGenerator
    {
        public static void WriteTo(StreamWriter stream)
        {
            using (var output = new IndentedWriter(stream))
            {
                output.Line(
                        $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using SectorDirector.Core.FormatModels.Udmf.Parsing.AbstractSyntaxTree;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing").OpenParen()
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
                Line("static partial void ProcessGlobalAssignment(MapData map, Assignment assignment)").
                OpenParen();

            var block = UdmfDefinitions.Blocks.Single(b => b.CodeName.ToPascalCase() == "MapData");

            WriteFieldSwitch(output, block, "map");

            output.
                CloseParen();
        }

        private static void WriteBlockParsing(IndentedWriter output)
        {
            output.
                Line("static partial void ProcessBlock(MapData map, Block block)").
                OpenParen().
                Line("switch (block.Name.ToLower())").
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
                Line($"map.UnknownBlocks.Add(ProcessUnknownBlock(block));").
                Line("break;").
                DecreaseIndent().
                CloseParen().
                CloseParen();
        }

        private static void WriteBlockParser(Block block, IndentedWriter output)
        {
            var variable = block.CodeName.ToCamelCase();

            output.
                Line($"static {block.CodeName} Process{block.CodeName}(Block block)").
                OpenParen().
                Line($"var {variable} = new {block.CodeName}();").
                Line("foreach (var assignment in block.Fields)").
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
                Line("switch (assignment.Name.ToLower())").
                OpenParen();


            foreach (var field in block.Fields)
            {
                output.
                    Line($"case \"{field.FormatName.ToLowerInvariant()}\":").
                    IncreaseIndent().
                    Line($"{variable}.{field.PropertyName} = Read{field.PropertyType.ToPascalCase()}Value(assignment, \"{block.CodeName}.{field.PropertyName}\");").
                    Line("break;").
                    DecreaseIndent();
            }

            output.
                Line("default:").
                IncreaseIndent().
                Line($"{variable}.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));").
                Line("break;").
                DecreaseIndent().
                CloseParen();
        }
    }
}