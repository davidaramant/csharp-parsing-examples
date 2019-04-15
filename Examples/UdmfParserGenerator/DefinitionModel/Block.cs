// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UdmfParserGenerator.Utilities;

namespace UdmfParserGenerator.DefinitionModel
{
    [DebuggerDisplay("{" + nameof(CodeName) + "}")]
    public sealed class Block
    {
        private readonly List<IProperty> _properties = new List<IProperty>();
        
        public string FormatName { get; }
        public string CodeName { get; }
        public IEnumerable<IProperty> Properties => _properties;
        public IEnumerable<Field> Fields => _properties.OfType<Field>();
        public IEnumerable<BlockList> SubBlocks => _properties.OfType<BlockList>();
        public bool IsSubBlock { get; }

        public IEnumerable<IProperty> OrderedProperties() => 
            Properties.Where(p => p.IsRequired).
            Concat(Properties.Where(p => !p.IsRequired));

        public Block(
            string formatName,
            IEnumerable<IProperty> properties,
            string codeName = null,
            bool isSubBlock = true) 
        {
            FormatName = formatName;
            CodeName = (codeName ?? formatName).ToPascalCase();
            IsSubBlock = isSubBlock;
            _properties.AddRange(properties);
        }
    }
}