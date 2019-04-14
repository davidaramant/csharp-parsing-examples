// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace SectorDirector.DataModelGenerator.DefinitionModel
{
    public sealed class UnknownPropertiesList : BlockList
    {
        public UnknownPropertiesList() : base("unknownProperties", singularName: "UnknownProperty")
        {
            IsRequired = false;
        }
    }
}