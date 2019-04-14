// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Superpower.Model;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.SuperpowerVersion.AbstractSyntaxTree
{
    public sealed class Value
    {
        public readonly TextSpan Data;
        public readonly UdmfToken Type;

        public Value(TextSpan data, UdmfToken type)
        {
            Data = data;
            Type = type;
        }
    }
}