// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using UdmfParserGenerator.Utilities;

namespace UdmfParserGenerator.DefinitionModel
{
    public class BlockList : IProperty
    {
        public string FormatName { get; }
        public string PropertyName { get; }
        public string SingularName { get; }
        public string FieldName => PropertyName.ToFieldName();
        public string ConstructorArgumentName => PropertyName.ToCamelCase();
        public string ConstructorArgumentType => $"IEnumerable<{SingularName}>";
        public string PropertyType => $"List<{SingularName}>";
        public string DefaultValue => "null";
        public bool IsRequired { get; protected set; } = true;

        public BlockList(string name, string singularName = null)
        {
            FormatName = singularName ?? name;
            PropertyName = (singularName == null ) ? name.ToPluralPascalCase() : name.ToPascalCase();
            SingularName = singularName?.ToPascalCase() ?? name.ToPascalCase();
        }
    }
}