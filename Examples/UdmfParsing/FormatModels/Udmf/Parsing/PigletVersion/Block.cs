// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Functional.Maybe;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.PigletVersion
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class Block : IEnumerable<Assignment>, IHaveAssignments
    {
        private readonly Dictionary<Identifier, Token> _properties;
        public Identifier Name { get; }

        public bool HasAssignments => _properties.Any();

        public Maybe<Token> GetValueFor(string name)
        {
            return GetValueFor(new Identifier(name));
        }

        public Maybe<Token> GetValueFor(Identifier name)
        {
            return _properties.TryGetValue(name, out var value) ? value.ToMaybe() : Maybe<Token>.Nothing;
        }

        public Block(Identifier name, IEnumerable<Assignment> propertyAssignments)
        {
            Name = name;
            _properties = propertyAssignments.ToDictionary(a => a.Name, a => a.Value);
        }

        public IEnumerator<Assignment> GetEnumerator()
        {
            return _properties.Select(pair => new Assignment(pair.Key, pair.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString() => Name + "\n" + string.Join("\n", _properties.Select(u => $"* {u.Key} = {u.Value}"));
    }
}