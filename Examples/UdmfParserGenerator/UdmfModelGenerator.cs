// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using UdmfParserGenerator.DefinitionModel;
using UdmfParserGenerator.Utilities;

namespace UdmfParserGenerator
{
    public static class UdmfModelGenerator
    {

        public static void WriteToPath(string basePath)
        {
            foreach (var block in UdmfDefinitions.Blocks)
            {
                using (var blockStream = File.CreateText(Path.Combine(basePath, block.CodeName.ToPascalCase() + ".Generated.cs")))
                using (var output = new IndentedWriter(blockStream))
                {
                    output.Line(
                        $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf.WritingExtensions;

namespace SectorDirector.Core.FormatModels.Udmf");

                    output.OpenParen();
                    output.Line(
                        $"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]");
                    output.Line(
                        $"public sealed partial class {block.CodeName}");
                    output.OpenParen();

                    WriteProperties(block, output);

                    output.Line();

                    WriteConstructors(output, block);

                    output.Line();

                    WriteWriteToMethod(block, output);
                    
                    output.Line();

                    WriteSemanticValidityMethods(output, block);
                    WriteCloneMethod(output, block);

                    output.CloseParen();
                    output.Line();
                    output.CloseParen(); // End namespace
                }
            }
        }

        private static void WriteCloneMethod(IndentedWriter output, Block block)
        {
            output.
                Line().
                Line($"public {block.CodeName} Clone()").
                OpenParen().
                Line($"return new {block.CodeName}(").IncreaseIndent();

            foreach (var indexed in block.OrderedProperties().Select((param, index) => new { param, index }))
            {
                var postfix = indexed.index == block.Properties.Count() - 1 ? ");" : ",";

                if (indexed.param is BlockList)
                {
                    postfix = ".Select(item => item.Clone())" + postfix;

                }
                output.Line(indexed.param.ConstructorArgumentName + ": " + indexed.param.PropertyName + postfix);
            }

            output.
                DecreaseIndent().
                CloseParen();
        }

        private static void WriteProperties(Block block, IndentedWriter sb)
        {
            foreach (var property in block.Fields.Where(_ => _.IsRequired))
            {
                sb.Line($"private bool {property.FieldName}HasBeenSet = false;").
                    Line($"private {property.PropertyType} {property.FieldName};").
                    Line($"public {property.PropertyType} {property.PropertyName}").
                    OpenParen().
                    Line($"get {{ return {property.FieldName}; }}").
                    Line($"set").
                    OpenParen().
                    Line($"{property.FieldName}HasBeenSet = true;").
                    Line($"{property.FieldName} = value;").
                    CloseParen().
                    CloseParen();
            }

            foreach (var field in block.Fields.Where(_ => !_.IsRequired))
            {
                sb.Line($"public {field.PropertyType} {field.PropertyName} {{ get; set; }} = {field.DefaultValue};");
            }

            foreach (var subBlock in block.SubBlocks)
            {
                sb.Line(
                    $"public {subBlock.PropertyType} {subBlock.PropertyName} {{ get; }} = new {subBlock.PropertyType}();");
            }
        }

        private static void WriteConstructors(IndentedWriter sb, Block block)
        {
            sb.Line($"public {block.CodeName}() {{ }}");
            sb.Line($"public {block.CodeName}(");
            sb.IncreaseIndent();

            foreach (var indexed in block.OrderedProperties().Select((param, index) => new { param, index }))
            {
                var property = indexed.param;
                var argument = $"{property.ConstructorArgumentType} {property.ConstructorArgumentName}" +
                               (property.IsRequired ? string.Empty : $" = {property.DefaultValue}");
                sb.Line(argument + (indexed.index == block.Properties.Count() - 1 ? ")" : ","));
            }

            sb.DecreaseIndent();
            sb.OpenParen();

            foreach (var property in block.OrderedProperties())
            {
                switch (property)
                {
                    case Field field:
                        sb.Line($"{field.PropertyName} = {field.ConstructorArgumentName};");
                        break;
                    case BlockList requiredBlockList when requiredBlockList.IsRequired:
                        sb.Line($"{requiredBlockList.PropertyName}.AddRange({requiredBlockList.ConstructorArgumentName});");
                        break;
                    case BlockList blockList:
                        sb.Line($"{blockList.PropertyName}.AddRange({blockList.ConstructorArgumentName} ?? Enumerable.Empty<{blockList.SingularName}>());");
                        break;
                }
            }

            sb.Line(@"AdditionalSemanticChecks();");
            sb.CloseParen();
        }

        private static void WriteWriteToMethod(Block block, IndentedWriter sb)
        {
            sb.Line(@"public Stream WriteTo(Stream stream)").
                OpenParen().
                Line("CheckSemanticValidity();");

            var indent = block.IsSubBlock ? "true" : "false";

            if (block.IsSubBlock)
            {
                sb.Line($"stream.WriteLine(\"{block.FormatName}\");");
                sb.Line("stream.WriteLine(\"{\");");
            }

            // WRITE ALL REQUIRED PROPERTIES
            foreach (var field in block.Fields.Where(_ => _.IsRequired))
            {
                sb.Line(
                    $"stream.WriteProperty(\"{field.FormatName}\", {field.FieldName}, indent: {indent});");
            }
            // WRITE OPTIONAL PROPERTIES
            foreach (var field in block.Fields.Where(_ => !_.IsRequired))
            {
                sb.Line(
                    $"if ({field.PropertyName} != {field.DefaultValue}) stream.WriteProperty(\"{field.FormatName}\", {field.PropertyName}, indent: {indent});");
            }

            // WRITE UNKNOWN PROPERTIES
            sb.Line($"foreach (var property in UnknownProperties)").
                OpenParen().
                Line($"stream.WritePropertyVerbatim((string)property.Name, property.Value, indent: {indent});").
                CloseParen();

            // WRITE SUB-BLOCKS
            foreach (var subBlock in block.SubBlocks.Where(b => !(b is UnknownPropertiesList)))
            {
                var variable = subBlock.FormatName;
                sb.Line($"foreach(var {variable} in {subBlock.PropertyName})").
                    OpenParen().
                    Line($"{variable}.WriteTo(stream);").
                    CloseParen();                
            }

            if (block.IsSubBlock)
            {
                sb.Line("stream.WriteLine(\"}\");");
            }
            sb.Line("return stream;").
                CloseParen();
        }

        private static void WriteSemanticValidityMethods(IndentedWriter output, Block block)
        {
            output.Line(@"public void CheckSemanticValidity()").
                OpenParen();

            // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
            foreach (var field in block.Fields.Where(_ => _.IsRequired))
            {
                output.Line(
                    $"if (!{field.FieldName}HasBeenSet) throw new InvalidUdmfException(\"Did not set {field.PropertyName} on {block.CodeName.ToPascalCase()}\");");
            }

            output.Line(@"AdditionalSemanticChecks();").
                CloseParen().
                Line().
                Line("partial void AdditionalSemanticChecks();");
        }
    }
}