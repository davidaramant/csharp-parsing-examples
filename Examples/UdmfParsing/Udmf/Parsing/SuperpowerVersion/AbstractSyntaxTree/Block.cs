// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.SuperpowerVersion.AbstractSyntaxTree
{
    public sealed class Block : IGlobalExpression
    {
        public readonly Identifier Name;
        public readonly ImmutableArray<Assignment> Fields;

        public Block(Identifier name, IEnumerable<Assignment> fields)
        {
            Name = name;
            Fields = fields.ToImmutableArray();
        }
    }
}