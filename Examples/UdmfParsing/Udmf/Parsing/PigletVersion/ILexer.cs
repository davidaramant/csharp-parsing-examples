// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace UdmfParsing.Udmf.Parsing.PigletVersion
{
    public interface ILexer
    {
        Token MustReadTokenOfTypes(params TokenType[] types);
    }
}