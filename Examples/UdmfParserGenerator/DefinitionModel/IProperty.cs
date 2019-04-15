// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace UdmfParserGenerator.DefinitionModel
{
    public interface IProperty
    {
        bool IsRequired { get; }
        string DefaultValue { get; }

        string ConstructorArgumentName { get; }
        string ConstructorArgumentType { get; }

        string PropertyName { get; }
        string PropertyType { get; }
    }
}