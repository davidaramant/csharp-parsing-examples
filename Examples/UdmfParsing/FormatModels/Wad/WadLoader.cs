﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Core.FormatModels.Wad
{
    public static class WadLoader
    {
        public static List<MapData> Load(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                maps.AddRange(
                    wad.GetMapNames().Select(name => MapData.LoadFrom(wad.GetMapStream(name))));
            }

            return maps;
        }
    }
}