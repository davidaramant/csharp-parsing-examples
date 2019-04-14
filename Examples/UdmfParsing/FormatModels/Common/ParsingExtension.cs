// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace SectorDirector.Core.FormatModels.Common
{
    public sealed class ParsingException : Exception
    {
        public ParsingException(string message) : base(message)
        {            
        }
    }
}