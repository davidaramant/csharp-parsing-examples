﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.PidginVersion.AbstractSyntaxTree
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class Assignment : IGlobalExpression, IEquatable<Assignment>
    {
        public readonly Identifier Name;
        public readonly object Value;

        public Assignment(Identifier name, object value)
        {
            Name = name;
            Value = value;
        }

        public string ValueAsString()
        {
            switch (Value)
            {
                case bool b:
                    return b.ToString().ToLowerInvariant();

                case string s:
                    return '"' + s + '"';

                default:
                    return Value.ToString();
            }
        }

        public override string ToString() => $"{Name}: {ValueAsString()} ({Value.GetType()})";

        #region Equality
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
            return obj is Assignment other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(Assignment left, Assignment right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Assignment left, Assignment right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}