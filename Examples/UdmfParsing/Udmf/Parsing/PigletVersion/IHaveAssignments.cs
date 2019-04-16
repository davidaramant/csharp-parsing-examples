// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Functional.Maybe;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.PigletVersion
{
    public interface IHaveAssignments
    {
        bool HasAssignments { get; }
        Maybe<Token> GetValueFor(string name);
        Maybe<Token> GetValueFor(Identifier name);
    }
}