// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.SuperpowerVersion.AbstractSyntaxTree
{
    public sealed class Assignment : IGlobalExpression
    {
        public readonly Identifier Name;
        public readonly Value Value;

        public Assignment(Identifier name, Value value)
        {
            Name = name;
            Value = value;
        }

        public string ValueAsString() => Value.Data.ToStringValue();
    }
}