// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using UdmfParserGenerator.Utilities;

namespace UdmfParserGenerator.DefinitionModel
{
    public enum FieldType
    {
        Boolean,
        Integer,
        Float,
        String,
    }

    public sealed class Field : IProperty
    {
        public string FormatName { get; }
        public string PropertyName { get; }
        public string FieldName => PropertyName.ToFieldName();
        public string ConstructorArgumentName => PropertyName.ToCamelCase();
        public string ConstructorArgumentType => PropertyType;
        public bool IsRequired => DefaultValue == null;
        public string DefaultValue { get; }

        public string PropertyType
        {
            get
            {
                switch (Type)
                {
                    case FieldType.Boolean: return "bool";
                    case FieldType.Integer: return "int";
                    case FieldType.Float: return "double";
                    case FieldType.String: return "string";
                    default: throw new Exception("New field???");
                }
            }
        }

        public FieldType Type { get; }
        
        public Field(string name, FieldType type, string formatName = null, object defaultValue = null)
        {
            FormatName = formatName ?? name;
            PropertyName = name.ToPascalCase();
            Type = type;

            if (defaultValue == null)
            {
                DefaultValue = null;
            }
            else
            {
                switch (Type)
                {
                    case FieldType.Boolean:
                        DefaultValue = defaultValue.ToString().ToLowerInvariant();
                        break;
                    case FieldType.Integer:
                    case FieldType.Float:
                        DefaultValue = defaultValue.ToString();
                        break;
                    case FieldType.String:
                        DefaultValue = $"\"{defaultValue}\"";
                        break;
                    default:
                        throw new Exception("New field type?!");
                }
            }
        }
    }
}