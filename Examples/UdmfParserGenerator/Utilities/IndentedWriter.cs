// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;

namespace SectorDirector.DataModelGenerator.Utilities
{
    public sealed class IndentedWriter : IDisposable
    {
        private readonly StreamWriter _writer;
        public IndentedWriter(StreamWriter writer) => _writer = writer;

        public int IndentionLevel { get; private set; }
        public string CurrentIndent => new string(' ', IndentionLevel*4);

        public IndentedWriter IncreaseIndent()
        {
            IndentionLevel++;
            return this;
        }

        public IndentedWriter DecreaseIndent()
        {
            if (IndentionLevel == 0)
                throw new InvalidOperationException();
            IndentionLevel--;
            return this;
        }

        public IndentedWriter OpenParen() => Line("{").IncreaseIndent();
        public IndentedWriter CloseParen()=> DecreaseIndent().Line("}");

        public IndentedWriter Line(string line)
        {
            _writer.WriteLine(CurrentIndent + line);
            return this;
        }

        public IndentedWriter Line()
        {
            _writer.WriteLine();
            return this;
        }

        public void Dispose()
        {
            if (IndentionLevel != 0)
            {
                throw new InvalidOperationException("Indention level is screwed up.");
            }
        }
    }
}