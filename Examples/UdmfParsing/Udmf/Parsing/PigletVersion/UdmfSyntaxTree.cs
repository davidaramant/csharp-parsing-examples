// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.PigletVersion
{
    public sealed class UdmfSyntaxTree : IHaveAssignments
    {
        private readonly Dictionary<Identifier, Token> _globalAssignments;

        public bool HasAssignments => _globalAssignments.Any();

        public Maybe<Token> GetValueFor(string name)
        {
            return GetValueFor(new Identifier(name));
        }

        public Maybe<Token> GetValueFor(Identifier name)
        {
            return _globalAssignments.TryGetValue(name, out var value) ? value.ToMaybe() : Maybe<Token>.Nothing;
        }

        public IEnumerable<Block> Blocks { get; }
        public IEnumerable<ArrayBlock> ArrayBlocks { get; }

        public UdmfSyntaxTree(
            IEnumerable<Assignment> globalAssignments,
            IEnumerable<Block> blocks,
            IEnumerable<ArrayBlock> arrayBlocks)
        {
            Blocks = blocks;
            ArrayBlocks = arrayBlocks;
            _globalAssignments = globalAssignments.ToDictionary(a => a.Name, a => a.Value);
        }

        public IEnumerable<Assignment> GetGlobalAssignments()
        {
            return _globalAssignments.Select(pair => new Assignment(pair.Key, pair.Value));
        }
    }
}