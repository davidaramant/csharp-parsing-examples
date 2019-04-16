// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.PigletVersion
{
    public sealed class ArrayBlock : IEnumerable<IReadOnlyList<int>>
    {
        public Identifier Name { get; }
        private readonly List<IReadOnlyList<int>> _elements = new List<IReadOnlyList<int>>();
        public int Count => _elements.Count;
        public IReadOnlyList<int> this[int index] => _elements[index];

        public ArrayBlock(Identifier name, IEnumerable<IReadOnlyList<int>> elements )
        {
            Name = name;
            _elements.AddRange(elements);
        }

        public IEnumerator<IReadOnlyList<int>> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}