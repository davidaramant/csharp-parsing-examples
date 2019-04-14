// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.Wad
{
    public sealed class UdmfLump : ILump
    {
        private readonly MapData _mapData;
        public LumpName Name { get; }

        public UdmfLump(LumpName name, MapData mapData)
        {
            Name = name;
            _mapData = mapData;
        }

        public void WriteTo(Stream stream) => _mapData.WriteTo(stream);
    }
}
