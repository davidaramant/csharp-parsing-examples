// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.PigletVersion
{
    [DebuggerDisplay("{Name} = {Value}")]
    public sealed class Assignment : IEquatable<Assignment>
    {
        public Identifier Name { get; }
        public Token Value { get; }

        public Assignment(string name, Token value) : 
            this(new Identifier(name), value)
        {
        }

        public Assignment(Identifier name, Token value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"{Name} = {Value}";

        #region Equality members

        public bool Equals(Assignment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.Equals(other.Name) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Assignment && Equals((Assignment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        #endregion
    }
}