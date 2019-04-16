// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Superpower.Display;

namespace UdmfParsing.Udmf.Parsing.SuperpowerVersion
{
    public enum UdmfToken
    {
        None,
        Number,
        True,
        False,
        QuotedString,
        Identifier,
        [Token(Example = "=")]
        Equals,
        [Token(Example = ";")]
        Semicolon,
        [Token(Example = "{")]
        OpenBracket,
        [Token(Example = "}")]
        CloseBracket,
    }
}